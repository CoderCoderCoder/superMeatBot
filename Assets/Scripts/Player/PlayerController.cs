using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class PlayerController : MonoBehaviour {

	private Rigidbody2D playerPhysicsBody = null;
	private bool hasJumped = false;		// Has the player finished a jump and can he jump again
	private float jumpTime = 0.05f;
	private const float jumpSpeed = 5f;
	private const float jumpForce = 100f;
	private const float axisForce = 50f;
	private const float axisSpeed = 5f;

	// Use this for initialization
	void Start () {
		playerPhysicsBody = this.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Jump
		if (!hasJumped && Input.GetKey(KeyCode.UpArrow))  
		{
			TryPerformJump();
		}

		if((jumpTime > 0f) && Input.GetKeyUp(KeyCode.UpArrow))
		{
			// User has let go of the UP button, stop jump attempt
			Debug.Log("Jump Attempt Finished");
			hasJumped = true;
		}

		if(Input.GetKey(KeyCode.LeftArrow))
		{
			TryMoveLeft();
		}
		else if(Input.GetKey(KeyCode.RightArrow))
		{
			TryMoveRight();
		}
		else 
		{
			
		}
	}

	void TryPerformJump()
	{
		// Timer for how long we can jump
		if(jumpTime > 0f)
		{
			// Add force, clamp max velocity, reduce time allowed for jump
			playerPhysicsBody.AddForce(new Vector2 (0f, jumpForce));
			if(playerPhysicsBody.velocity.y > jumpSpeed)
			{
				playerPhysicsBody.velocity = new Vector2(playerPhysicsBody.velocity.x, jumpSpeed);
			}
			jumpTime -= Time.deltaTime;
		}
		else
		{
			// Times up, stop allowing jump
			Debug.Log("Jump Attempt Finished");
			hasJumped = true;
		}
	}

	void TryMoveLeft()
	{
		playerPhysicsBody.AddForce(new Vector2(-axisForce, 0f));
		if(playerPhysicsBody.velocity.x < -axisSpeed)
		{
			playerPhysicsBody.velocity = new Vector2(-axisSpeed, playerPhysicsBody.velocity.y);
		}
	}

	void TryMoveRight()
	{
		playerPhysicsBody.AddForce(new Vector2(axisForce, 0f));
		if(playerPhysicsBody.velocity.x > axisSpeed)
		{
			playerPhysicsBody.velocity = new Vector2(axisSpeed, playerPhysicsBody.velocity.y);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (hasJumped && collision.gameObject.tag == "FloorTile") 
		{
			hasJumped = false;
			jumpTime = 0.05f;
			Debug.Log ("Jump Reset. Can jump again.");
		}
	}
}
