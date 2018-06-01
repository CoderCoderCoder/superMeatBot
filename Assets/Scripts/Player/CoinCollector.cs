using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour {

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
        
        Destroy(other.gameObject);
    }

    private int coins = 0;
}
