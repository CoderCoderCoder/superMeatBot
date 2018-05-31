using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorPalette : MonoBehaviour {

    [SerializeField]
    private Sprite emptySpace;
    public Sprite EmptySpace
    {
        get { return emptySpace; }
    }
    [SerializeField]
    private Sprite wall;
    public Sprite Wall
    {
        get { return wall; }
    }
    [SerializeField]
    private Sprite playerStart;
    public Sprite PlayerStart
    {
        get { return playerStart; }
    }
    [SerializeField]
    private Sprite coin;
    public Sprite Coin
    {
        get { return coin; }
    }

    private Sprite currentTileToPaintWith;
    public Sprite CurrentTileToPaintWith
    {
        get { return currentTileToPaintWith; }
    }


	// Use this for initialization
	void Start () {
        currentTileToPaintWith = wall;
	}
	
    public void SetCurrentBrushToAir()
    {
        currentTileToPaintWith = emptySpace;
    }
    public void SetCurrentBrushToWall()
    {
        currentTileToPaintWith = wall;
    }
    public void SetCurrentBrushToPlayerStart()
    {
        currentTileToPaintWith = playerStart;
    }
    public void SetCurrentBrushToCoin()
    {
        currentTileToPaintWith = coin;
    }
}
