using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperMeatBotAgent : Agent
{
	PlayerController player;
	CoinCollector collector;
	int action = 0;

    public override void InitializeAgent()
    {
		player = gameObject.GetComponent<PlayerController>();
		collector = gameObject.GetComponent<CoinCollector>();

		var obj = GameObject.Find("ActionText");
		obj.GetComponent<Text>().text = "Action: " + action.ToString();
		Debug.Log("Agent Created");

		this.GiveBrain(GameObject.Find("Ball3DBrain").GetComponent<Brain>());
    }

    public override void CollectObservations()
    {
		AddVectorObs(gameObject.transform.localPosition.x);
        
        SetTextObs("Testing " + gameObject.GetInstanceID());
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
		action = Mathf.FloorToInt(vectorAction[0]);

		var obj = GameObject.Find("ActionText");
		obj.GetComponent<GUIText>().text = "Action: " + action.ToString();

		switch(action)
		{
			case 0:
				player.TryPerformJump();
				break;
			case 1:
				player.TryPerformJump();
				player.TryMoveLeft();
				break;
			case 2:
				player.TryPerformJump();
				player.TryMoveRight();
				break;
			case 3:
				player.TryMoveLeft();
				break;
			case 4:
				player.TryMoveRight();
				break;
			case 5:
			default:
				// Do nothing
				break;
		}
			
		SetReward(collector.ConsumeCollectedCoins());

		if(player.playerDead)
		{
			Done();
			SetReward(-1f);
		}
    }

    public override void AgentReset()
    {
		player.KillPlayer(true);
    }
}
