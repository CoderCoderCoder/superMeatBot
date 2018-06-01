using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour {


    private List<GameObject> coinsCollected = new List<GameObject>();
    internal int ConsumeCollectedCoins()
    {
        var coinsConsumed = coins;
        ClearConsumedCoins();
        return coinsConsumed;
    }

    internal void ClearConsumedCoins()
    {
        coins = 0;
    }

	// Use this for initialization
	void Start () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collected coin");
        coins++;

        other.gameObject.SetActive(false);
        coinsCollected.Add(other.gameObject);
    }
    

    private int coins = 0;

    internal void Reset()
    {
        for (int i = coinsCollected.Count - 1; i >= 0; i--)
        {
            coinsCollected[i].SetActive(true);
            coinsCollected.RemoveAt(i);
        }
    }
}
