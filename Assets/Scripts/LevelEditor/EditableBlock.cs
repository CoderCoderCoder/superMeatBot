using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditableBlock : MonoBehaviour {

    [SerializeField]
    private Sprite EmptySpace;
    [SerializeField]
    private Sprite Wall;
    [SerializeField]
    private Sprite PlayerStart;
    [SerializeField]
    private Sprite Coin;

    private SpriteRenderer childSpriteRenderer;

	// Use this for initialization
	void Start () {
        childSpriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        childSpriteRenderer.sprite = EmptySpace;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
