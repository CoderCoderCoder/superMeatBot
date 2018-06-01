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
		obj.GetComponent<Text>().text = "Action: Not set yet";
		Debug.Log("Agent Created");

		this.GiveBrain(GameObject.Find("Ball3DBrain").GetComponent<Brain>());
    }

    public override void CollectObservations()
    {

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0)
                    AddVectorObs((int)player.GetBlockType(x, y));
            }
        }
        //SetTextObs("Testing " + gameObject.GetInstanceID());
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
		action = Mathf.FloorToInt(vectorAction[0]);

		if(action < 0)
		{
			action = -action;
		}

		var obj = GameObject.Find("ActionText");
		obj.GetComponent<Text>().text = "Action: " + action.ToString() + "    Y: " + gameObject.transform.localPosition.y;

		switch(action)
		{
			case 0:
				player.TryPerformJump();
				break;
			case 1:
				player.TryPerformJump();
				player.TryMoveRight();
				break;
			case 2:
				player.TryMoveRight();
				break;
			case 3:
				player.TryMoveLeft();
				break;
			case 4:
				player.TryPerformJump();
				player.TryMoveLeft();
				break;
			case 5:
			default:
				// Do nothing
				break;
		}
			
		SetReward(collector.ConsumeCollectedCoins());

		if(player.playerDied)
		{
			Done();
			SetReward(-1f);
		}
    }

    public override void AgentReset()
    {
		player.playerDied = false;
    }
}
