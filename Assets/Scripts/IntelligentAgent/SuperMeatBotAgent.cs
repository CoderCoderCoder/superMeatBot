using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMeatBotAgent : Agent
{
    public override void InitializeAgent()
    {

    }

    public override void CollectObservations()
    {
		AddVectorObs(gameObject.transform.localPosition.x);
        
        SetTextObs("Testing " + gameObject.GetInstanceID());
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
            float action_z = 2f * Mathf.Clamp(vectorAction[0], -1f, 1f);
            if ((gameObject.transform.rotation.z < 0.25f && action_z > 0f) ||
                (gameObject.transform.rotation.z > -0.25f && action_z < 0f))
            {
                gameObject.transform.Rotate(new Vector3(0, 0, 1), action_z);
            }
            float action_x = 2f * Mathf.Clamp(vectorAction[1], -1f, 1f);
            if ((gameObject.transform.rotation.x < 0.25f && action_x > 0f) ||
                (gameObject.transform.rotation.x > -0.25f && action_x < 0f))
            {
                gameObject.transform.Rotate(new Vector3(1, 0, 0), action_x);
            }

            SetReward(0.1f);

        }
//        if ((ball.transform.position.y - gameObject.transform.position.y) < -2f ||
//            Mathf.Abs(ball.transform.position.x - gameObject.transform.position.x) > 3f ||
//            Mathf.Abs(ball.transform.position.z - gameObject.transform.position.z) > 3f)
//        {
//            Done();
//            SetReward(-1f);
//        }

    }

    public override void AgentReset()
    {
		gameObject.GetComponent<PlayerController>().KillPlayer(true);
    }

}
