using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private PlayerData playerData;
    private string saveFullPath;
    private const string saveFileName = "gamesave.json";

    public bool MetScientist { get { return playerData.metScientist; } }
    public bool MetAdventurer { get { return playerData.metAdventurer; } }
    public bool MetSmith { get { return playerData.metSmith; } }
    public bool MetReaper { get { return playerData.metReaper; } }

    public float PlayerPositionX { get { return playerData.playerPositionX; } }
    public float PlayerPositionY { get { return playerData.playerPositionY; } }

    public string PlayerScene { get { return playerData.playerScene; } }

    public int PlayerEnergy { get { return playerData.playerEnergy; } }

    public int PlayerMemories { get { return playerData.playerMemories; } }

    public void SetPlayerStats(int energy, int memories)
    {
        playerData.playerEnergy = energy;
        playerData.playerMemories = memories;
    }

    public void SetPlayerScene()
    {
        playerData.playerScene = SceneManager.GetActiveScene().name;
    }

    public void SetPlayerPosition(Vector3 playerPosition)
    {
        playerData.playerPositionX = playerPosition.x;
        playerData.playerPositionY = playerPosition.y;
    }

    public void SetConversationState(ConversationHistory conversationHistory)
    {
        playerData.metScientist = conversationHistory.metScientist;
        playerData.metAdventurer = conversationHistory.metAdventurer;
        playerData.metReaper = conversationHistory.metReaper;
        playerData.metSmith = conversationHistory.metSmith;
    }

    // Use Awake to enforce singleton pattern.
    private void Awake()
    {
        int saveManagerCount = FindObjectsOfType<SaveManager>().Length;
        if (saveManagerCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Use Start to set the intial values of the player data.
    private void Start()
    {
        // Setup dummy player data.
        playerData = new PlayerData();

        saveFullPath = Path.Combine(Application.persistentDataPath, saveFileName);
    }

    public void SaveGameState()
    {
        var jsonData = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveFullPath, jsonData, System.Text.Encoding.UTF8);
    }

    public bool LoadGameState()
    {
        if (File.Exists(saveFullPath))
        {
            var jsonData = File.ReadAllText(saveFullPath, System.Text.Encoding.UTF8);
            JsonUtility.FromJsonOverwrite(jsonData, playerData);
            return true;
        }

        return false;
    }
}