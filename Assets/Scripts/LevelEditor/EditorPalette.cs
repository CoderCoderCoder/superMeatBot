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
    [SerializeField]
    private Sprite playerStart;
    [SerializeField]
    private Sprite coin;

    public class TilePaint
    {
        public BlockType blockType;
        public Sprite sprite;
    }
    public TilePaint CurrentPaint
    {
        get;
        private set;
    }
     
	// Use this for initialization
	void Start () {
        SetCurrentBrushToAir();
	}
	
    public void SetCurrentBrushToAir()
    {
        CurrentPaint = CreateAirTilePaint();
    }

    internal TilePaint CreateAirTilePaint()
    {
        return new TilePaint
        {
            blockType = BlockType.Air,
            sprite = emptySpace
        };
    }

    public void SetCurrentBrushToWall()
    {
        CurrentPaint = new TilePaint
        {
            blockType = BlockType.Wall,
            sprite = wall
        };
    }
    public void SetCurrentBrushToPlayerStart()
    {
        CurrentPaint = new TilePaint
        {
            blockType = BlockType.PlayerStart,
            sprite = playerStart
        };
    }
    public void SetCurrentBrushToCoin()
    {
        CurrentPaint = new TilePaint
        {
            blockType = BlockType.Coin,
            sprite = coin
        };
    }
}
