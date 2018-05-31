using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorGrid : MonoBehaviour {
    
    [SerializeField]
    private Vector2Int tileMapSize = new Vector2Int(16, 16);

    [SerializeField]
    private GameObject editableTilePrefab;

    [SerializeField]
    private EditorPalette editorPalette;
    public EditorPalette EditorPalette
    { 
        get { return editorPalette; } 
    }

	// Use this for initialization
	void Start () {
        ClearChildren();
        CreateChildren();
	}

    private void CreateChildren()
    {
        for (var x = 0; x < tileMapSize.x; ++x )
        {
            for (var y = 0; y < tileMapSize.y; ++y )
            {
                var newCell = Instantiate(editableTilePrefab, new Vector3(x, y, 0.0f), Quaternion.identity, transform);
                var block = newCell.GetComponent<EditableBlock>();
                Debug.Assert(block != null);
                block.Initialize(this, EditorPalette.EmptySpace);
            }
        }
    }

    private void ClearChildren()
    {
        while (transform.childCount != 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
