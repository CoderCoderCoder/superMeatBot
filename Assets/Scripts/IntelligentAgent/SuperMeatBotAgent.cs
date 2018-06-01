using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMeatBotAgent : Agent
{
	PlayerController player;
	CoinCollector collector;

    public override void InitializeAgent()
    {
		player = gameObject.GetComponent<PlayerController>();
		collector = gameObject.GetComponent<CoinCollector>();
    }

    public override void CollectObservations()
    {
		AddVectorObs(gameObject.transform.localPosition.x);
        
        SetTextObs("Testing " + gameObject.GetInstanceID());
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
		int action = Mathf.FloorToInt(vectorAction[0]);

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
