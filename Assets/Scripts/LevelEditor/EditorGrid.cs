using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEditor;

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

    private EditableBlock[] blocks;

    [SerializeField]
    private SceneAsset levelLoaderScene;

    [SerializeField]
    private WarningText levelWarning;

	// Use this for initialization
	void Start () {
        CentreSelfPosition();
        ClearChildren();
        CreateChildren();
	}

    private void CentreSelfPosition()
    {
        var floatSize = new Vector2(tileMapSize.x, tileMapSize.y);
        var localCentre = floatSize * transform.localScale / 2f;
        transform.localPosition = -localCentre;
    }

    private void CreateChildren()
    {
        blocks = new EditableBlock[tileMapSize.x * tileMapSize.y];

        for (var x = 0; x < tileMapSize.x; ++x )
        {
            for (var y = 0; y < tileMapSize.y; ++y )
            {
                var newCell = Instantiate(editableTilePrefab, Vector3.zero, Quaternion.identity, transform);
                newCell.transform.localPosition = new Vector3(x, y, 0f);
                var block = newCell.GetComponent<EditableBlock>();
                Debug.Assert(block != null);
                block.Initialize(this, EditorPalette.EmptySpace);

                var linearizedIndex = LevelDefinition.CalculateLinearizedCoordinates(x, y, tileMapSize.x);
                blocks[linearizedIndex] = block;
            }
        }


    }

    private void ClearChildren()
    {
        while (transform.childCount != 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        blocks = null;
    }

    internal void PrePaint(BlockType blockType)
    {
        if (blockType == BlockType.PlayerStart)
        {
            ConvertAllCurrentPlayerStartsToAir();
        }
    }

    private void ConvertAllCurrentPlayerStartsToAir()
    {
        for (var childIndex = 0; childIndex < transform.childCount; ++childIndex )
        {
            var child = transform.GetChild(childIndex);
            var editableBlock = child.GetComponent<EditableBlock>();
            Debug.Assert(editableBlock != null);
            editableBlock.ConvertToAirIfCurrentlyPlayerStart();
        }
    }

    public void SaveToFile()
    {
        if (!VerifyLevel())
        {
            return;
        }

        var levelDefinition = new LevelDefinition();
        levelDefinition.dimensions = tileMapSize;
        levelDefinition.blockTypeSerializedGrid = new BlockType[tileMapSize.x * tileMapSize.y];
        
        for (var x = 0; x < tileMapSize.x; ++x)
        {
            for (var y = 0; y < tileMapSize.y; ++y)
            {
                var linearMappedIndex = levelDefinition.CalculateLinearizedCoordinates(x, y);
                levelDefinition.blockTypeSerializedGrid[linearMappedIndex] = blocks[linearMappedIndex].blockType;
            }
        }

        var jsonRepresentation = JsonUtility.ToJson(levelDefinition);
        var formatter = new BinaryFormatter();
        var file = OpenMapFileForWriting();
        formatter.Serialize(file, jsonRepresentation);
        file.Close();
    }

    private bool VerifyLevel()
    {
        if (!blocks.Any(block => block.blockType == BlockType.PlayerStart))
        {
            levelWarning.EmitWarning("No player found in level! The agent can't play this level, so not exporting");
            return false;
        }

        if (!blocks.Any(block => block.blockType == BlockType.Coin))
        {
            levelWarning.EmitWarning("No coin found in level! The agent won't learn anything from this level, so not exporting");
            return false;
        }

        return true;
    }

    private FileStream OpenMapFileForWriting()
    {
        return File.Create(Application.persistentDataPath + "/level.level");
    }

    public void MoveToGameScene()
    {
        SceneManager.LoadScene(levelLoaderScene.name);
    }
}
