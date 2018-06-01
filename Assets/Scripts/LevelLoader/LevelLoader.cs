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

	// Use this for initialization
	void Start () {
        LoadLevel();
	}

    private void LoadLevel()
    {
        var levelFilepathToLoad = GenerateLevelFilepathToLoad();
        LoadLevelFromFile(levelFilepathToLoad);
    }

    private void LoadLevelFromFile(string levelFilepathToLoad)
    {
        var levelData = DeserializeFromLevelFile(levelFilepathToLoad);
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
		CreateOutOfBounds();
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

	private void CreateOutOfBounds()
	{
		Instantiate(OutOfBoundsPrefab, GameObject.FindGameObjectWithTag("MainCamera").transform);
	}

    private LevelDefinition DeserializeFromLevelFile(string levelFilepathToLoad)
    {
        var formatter = new BinaryFormatter();
        var file = File.Open(Application.persistentDataPath + "/level.level", FileMode.Open, FileAccess.Read);
        var jsonRepresentation = (string)formatter.Deserialize(file);
        return JsonUtility.FromJson<LevelDefinition>(jsonRepresentation);
    }

    private string GenerateLevelFilepathToLoad()
    {
        return Application.persistentDataPath + "/level.level";
    }
	
}
