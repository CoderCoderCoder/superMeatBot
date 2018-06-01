using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class PlayerController : MonoBehaviour
{

	[SerializeField]
	private float playerFriction = 0.05f;
	[SerializeField]
	private float maxDownwardVelocity = 1.00f;
    [SerializeField]
    private GameObject deathParticles;

    private Rigidbody2D playerPhysicsBody = null;
    private Animator playerAnimator = null;
    public bool hasJumped = false;     // Has the player finished a jump and can he jump again
    public float jumpTime = 0.2f;
    private const float jumpSpeed = 7f;
    private const float jumpForce = 200f;
    private const float axisForce = 30f;
    private const float axisSpeed = 4f;
    public Vector3 startPosition;
    private float deathTimer = 1f;
    public bool playerDead = false;

    private CoinCollector coinCollector;

    //Level Details
    private Vector3 levelOffset;
    private LevelDefinition levelDefinition;

    internal void SetLevel(LevelDefinition levelDefinition, Vector3 position)
    {
        levelOffset = position;
        this.levelDefinition = levelDefinition;
    }

    public BlockType GetBlockType(int xOffset, int yOffset)
    {
        Vector2Int intPos = new Vector2Int((int)(transform.localPosition.x-0.5f) + xOffset, (int)(transform.localPosition.y - 0.5f) + yOffset);

        if (intPos.x < 0 || intPos.y < 0)
            return BlockType.Air;
        if (intPos.x >= levelDefinition.dimensions.x || intPos.y >= levelDefinition.dimensions.y)
            return BlockType.Air;

        BlockType type = levelDefinition.blockTypeSerializedGrid[levelDefinition.CalculateLinearizedCoordinates(intPos.x, intPos.y)];
        if (type == BlockType.PlayerStart)
            return BlockType.Air;
        else
            return type;
    }

    public Vector2 GetInTilePosition()
    {
        return new Vector2(transform.localPosition.x - (int)(transform.localPosition.x), transform.localPosition.y - (int)(transform.localPosition.y));
    }


    // Use this for initialization
    void Start()
    {
        playerPhysicsBody = this.GetComponent<Rigidbody2D>();
        playerAnimator = this.GetComponent<Animator>();
        coinCollector = GetComponent<CoinCollector>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GetBlockType(0, -1));

        // If they are jumping and we let go of the up key, stop the jump
        if ((jumpTime > 0f) && Input.GetKeyUp(KeyCode.UpArrow))
        {
            // User has let go of the UP button, stop jump attempt
            Debug.Log("Jump Attempt Finished");
            hasJumped = true;
            jumpTime = 0f;
        }

        if (playerDead || (transform.localPosition).y < 0)
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0f)
            {
                playerDead = false;
                this.GetComponent<SpriteRenderer>().enabled = true;
                ResetPlayer();
            }
        }
    }

    /*
	 * We use FixedUpdate for anything that requires use of the rigidbody.
	 * Gameplay mechanics are in Update
	 */
    void FixedUpdate()
    {
        // Jump
        if (!hasJumped && Input.GetKey(KeyCode.UpArrow))
        {
            TryPerformJump();
        }

        // Move the actor left and set the movement to use the correct animation
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            TryMoveLeft();
            playerAnimator.SetFloat("moveSpeed", 1f);
        }
        // Else, check the right key and move the character left (this prevents both actions occuring)
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            TryMoveRight();
            playerAnimator.SetFloat("moveSpeed", 1f);
        }
        // Else simulate friction (need a better method)
        else
        {
			playerPhysicsBody.velocity *= (1f - playerFriction);
            playerAnimator.SetFloat("moveSpeed", 0f);
        }

		ApplyDownwardVelocityConstraint();
    }

    private void ApplyDownwardVelocityConstraint()
    {
        var velocity = playerPhysicsBody.velocity;
        velocity.y = Mathf.Max(-maxDownwardVelocity, velocity.y);
        playerPhysicsBody.velocity = velocity;
    }

    public void TryPerformJump()
    {
        // Timer for how long we can jump
        if (jumpTime > 0f)
        {
            // Add force, clamp max velocity, reduce time allowed for jump
            playerPhysicsBody.velocity = new Vector2(playerPhysicsBody.velocity.x, jumpSpeed);
            jumpTime -= Time.deltaTime;
        }
        else
        {
            // Times up, stop allowing jump
            Debug.Log("Jump Attempt Finished");
            hasJumped = true;
        }
    }

    public void TryMoveLeft()
    {
        playerPhysicsBody.AddForce(new Vector2(-axisForce, 0f));
        if (playerPhysicsBody.velocity.x < -axisSpeed)
        {
            playerPhysicsBody.velocity = new Vector2(-axisSpeed, playerPhysicsBody.velocity.y);
        }
    }

    public void TryMoveRight()
    {
        playerPhysicsBody.AddForce(new Vector2(axisForce, 0f));
        if (playerPhysicsBody.velocity.x > axisSpeed)
        {
            playerPhysicsBody.velocity = new Vector2(axisSpeed, playerPhysicsBody.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasJumped && collision.gameObject.tag == "FloorTile")
        {
            hasJumped = false;
            jumpTime = 0.2f;
            Debug.Log("Jump Reset. Can jump again.");
        }
        else if (collision.gameObject.tag == "Danger")
        {
            KillPlayer(true);
        }
    }

    public void KillPlayer(bool reset)
    {
        playerDead = reset;
        deathTimer = 1f;
        if (deathParticles)
        {
            var deadEffect = Instantiate(deathParticles, transform.parent);
            deadEffect.transform.localPosition = this.transform.localPosition;
            Destroy(deadEffect, 5f);
        }
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void ResetPlayer()
    {
        this.transform.position = startPosition;
        this.playerPhysicsBody.velocity = Vector2.zero;
        coinCollector.Reset();
    }
}
