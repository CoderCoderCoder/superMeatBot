using System.Collections.Generic;
using UnityEngine;

public class SuperMeatBotDecision : MonoBehaviour, Decision
{
    public float[] Decide(
        List<float> vectorObs,
        List<Texture2D> visualObs,
        float reward,
        bool done,
        List<float> memory)
    {
		List<float> act = new List<float>();

		act.Add(0);
		 
		return act.ToArray();
    }

    public List<float> MakeMemory(
        List<float> vectorObs,
        List<Texture2D> visualObs,
        float reward,
        bool done,
        List<float> memory)
    {
        return new List<float>();
    }
}
