using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditableBlock : MonoBehaviour {

    private SpriteRenderer childSpriteRenderer;
    private EditorGrid parentGrid;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        childSpriteRenderer.sprite = parentGrid.EditorPalette.CurrentTileToPaintWith;
    }

    internal void Initialize(EditorGrid editorGrid, Sprite initialSprite)
    {
        parentGrid = editorGrid;
        childSpriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        childSpriteRenderer.sprite = initialSprite;
    }
}
