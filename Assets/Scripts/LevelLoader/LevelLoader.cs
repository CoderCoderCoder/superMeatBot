using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    [SerializeField]
    private GameObject WallPrefab;

	[SerializeField]
	private GameObject TrapPrefab;

    [SerializeField]
    private GameObject PlayerPrefab;

    [SerializeField]
    private GameObject CoinPrefab;

	[SerializeField]
	private GameObject OutOfBoundsPrefab;

    private LevelDefinition levelDefinition;

    [SerializeField]
    private int levelNr;

	// Use this for initialization
	void Start () {
        LoadLevel();
	}

    private void LoadLevel()
    {
        var levelFilepathToLoad = GenerateLevelFilepathToLoad(levelNr);
        LoadLevelFromFile(levelFilepathToLoad);
    }

    private void LoadLevelFromFile(string levelFilepathToLoad)
    {
        levelDefinition = DeserializeFromLevelFile(levelFilepathToLoad);
        if (levelDefinition == null)
            return;
        var levelData = levelDefinition;

        for (var x = 0; x < levelData.dimensions.x; ++x)
        {
            for (var y = 0; y < levelData.dimensions.y; ++y)
            {
                var index = levelData.CalculateLinearizedCoordinates(x, y);
                var blockType = levelData.blockTypeSerializedGrid[index];
                switch(blockType)
                {
                    case BlockType.Wall:
                        CreateWall(x, y);
                        break;
					case BlockType.Trap:
						CreateTrap(x, y);
						break;
                    case BlockType.PlayerStart:
                        CreatePlayer(x, y);
                        break;
                    case BlockType.Coin:
                        CreateCoin(x, y);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void CreateCoin(int x, int y)
    {
        var newCoin = Instantiate(CoinPrefab, transform);
        newCoin.transform.localPosition = new Vector3(x, y, 0f);
    }

    private void CreatePlayer(int x, int y)
    {
        var newPlayer = Instantiate(PlayerPrefab, transform);
        newPlayer.transform.localPosition = new Vector3(x, y, 0f);
        PlayerController playerController = newPlayer.GetComponent<PlayerController>();

        playerController.startPosition = newPlayer.transform.position;
        playerController.SetLevel(levelDefinition, transform.position);

        Debug.Log("Creating player at " + x + ", " + y);
    }

    private void CreateWall(int x, int y)
    {
        var newWall = Instantiate(WallPrefab, transform);
        newWall.transform.localPosition = new Vector3(x, y, 0f);
    }

	private void CreateTrap(int x, int y)
	{
		var newTrap = Instantiate(TrapPrefab, transform);
		newTrap.transform.localPosition = new Vector3(x, y, 0f);
	}

    private LevelDefinition DeserializeFromLevelFile(string levelFilepathToLoad)
    {
        var formatter = new BinaryFormatter();
        if(!File.Exists(levelFilepathToLoad))
        {
            return null;
        }
        var file = File.Open(levelFilepathToLoad, FileMode.Open, FileAccess.Read);
        var jsonRepresentation = (string)formatter.Deserialize(file);
        file.Close();
        return JsonUtility.FromJson<LevelDefinition>(jsonRepresentation);
    }

    private string GenerateLevelFilepathToLoad(int levelNr)
    {
        return Application.persistentDataPath + "/level"+ levelNr+".level";
    }
	
}
