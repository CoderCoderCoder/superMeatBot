using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditableBlock : MonoBehaviour {

    private SpriteRenderer childSpriteRenderer;
    private EditorGrid parentGrid;
    private BlockType blockType;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        var paint = parentGrid.EditorPalette.CurrentPaint;

        parentGrid.PrePaint(paint.blockType);
        ApplyTilePaint(paint);
    }

    private void ApplyTilePaint(EditorPalette.TilePaint paint)
    {

        childSpriteRenderer.sprite = paint.sprite;
        blockType = paint.blockType;
    }

    internal void Initialize(EditorGrid editorGrid, Sprite initialSprite)
    {
        parentGrid = editorGrid;
        childSpriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        childSpriteRenderer.sprite = initialSprite;
    }

    internal void ConvertToAirIfCurrentlyPlayerStart()
    {
        if (blockType == BlockType.PlayerStart)
        {
            ApplyTilePaint(parentGrid.EditorPalette.CreateAirTilePaint());
        }
    }
}
