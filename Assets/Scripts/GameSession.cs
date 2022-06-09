using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField]
    private Slider playerEnergySlider;

    [SerializeField]
    private Slider playerMemoriesSlider;

    [SerializeField]
    private GameObject menuPanel;

    [SerializeField]
    private ConversationHistory playerConversationHistory;

    [SerializeField]
    private PlayerState playerState;

    private int playerEnergyMax = 100;
    private int playerEnergyMin = 0;
    private int playerMemoriesMax = 100;
    private int playerMemoriesMin = 0;

    private Coroutine recharger;
    private bool isRecharging;

    // Use Awake to enforce singleton pattern
    private void Awake()
    {
        int gameSessionCount = FindObjectsOfType<GameSession>().Length;
        if (gameSessionCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // User Start to set the intial values of the sliders
    private void Start()
    {
        playerEnergySlider.value = playerState.playerEnergy;
        playerMemoriesSlider.value = playerState.playerMemories;
    }

    public void SetEnergy(int energy)
    {
        playerState.playerEnergy = Mathf.Clamp(energy, playerEnergyMin, playerEnergyMax);
        playerEnergySlider.value = playerState.playerEnergy;
    }

    public void IncreaseEnergy(int value)
    {
        // We only want to increase the energy if the player is not already dead.
        // This prevents the player from touching the charge pod just as they die.
        if (playerState.playerEnergy > 0)
        {
            playerState.playerEnergy = Mathf.Clamp(playerState.playerEnergy + value, playerEnergyMin, playerEnergyMax);
            playerEnergySlider.value = playerState.playerEnergy;
        }
    }

    public void DecreaseEnergy(int value)
    {
        // Only decrease energy if player is not actively recharging.
        if (!isRecharging)
        {
            // We only want to decrease the energy if the player is not already dead.
            if (playerState.playerEnergy > 0)
            {
                playerState.playerEnergy = Mathf.Clamp(playerState.playerEnergy - value, playerEnergyMin, playerEnergyMax);
            }

            // If they are now dead, then we need to load the death scene.
            if (playerState.playerEnergy <= 0)
            {
                EnergyZero();
            }
            else
            {
                playerEnergySlider.value = playerState.playerEnergy;

                if (playerState.playerEnergy <= 0)
                {
                    EnergyZero();
                }
            }
        }
    }

    private void EnergyZero()
    {
        // Set the energy to 0 just incase and update the UI.
        playerState.playerEnergy = 0;
        playerEnergySlider.value = playerState.playerEnergy;

        // Load the death scene.
        PlayerDeadEndGame();
    }

    public void SetMemories(int memories)
    {
        playerState.playerMemories = Mathf.Clamp(memories, playerMemoriesMin, playerMemoriesMax);
        playerMemoriesSlider.value = playerState.playerMemories;
    }

    public void IncreaseMemory(int value)
    {
        playerState.playerMemories = Mathf.Clamp(playerState.playerMemories + value, playerMemoriesMin, playerMemoriesMax);
        playerMemoriesSlider.value = playerState.playerMemories;
        if (playerState.playerMemories >= 100)
        {
            PlayerWinGame();
        }
    }

    // Reset the game session and send back to the main menu
    public void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Dispose();
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }

    private void PlayerDeadEndGame()
    {
        LoadMap("99 - Death");
    }

    private void PlayerWinGame()
    {
        LoadMap("98 - Win");
    }

    private void LoadMap(string sceneName)
    {
        // Cleanup the player controller to make sure the DecreaseEnergy process is stopped.
        Destroy(FindObjectOfType<PlayerController>());
        SceneManager.LoadScene(sceneName);
    }

    public void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    public void OnClickQuit()
    {
        Destroy(FindObjectOfType<PlayerController>());
        playerConversationHistory.Reset();
        playerState.Reset();
        ResetGameSession();
    }

    public void OnClickSave()
    {
        var saveManager = FindObjectOfType<SaveManager>();
        saveManager.SetConversationState(playerConversationHistory);
        saveManager.SetPlayerStats(playerState.playerEnergy, playerState.playerMemories);
        saveManager.SetPlayerScene();
        saveManager.SetPlayerPosition(FindObjectOfType<PlayerController>().transform.position);
        saveManager.SaveGameState();
        ToggleMenu();
    }

    public void StartRecharge(int chargeAmount, float chargeSpeedSeconds)
    {
        isRecharging = true;
        recharger = StartCoroutine(Recharge(chargeAmount, chargeSpeedSeconds));
    }

    private IEnumerator Recharge(int chargeAmount, float chargeSpeedSeconds)
    {
        while (true)
        {
            IncreaseEnergy(chargeAmount);
            yield return new WaitForSeconds(chargeSpeedSeconds);
        }
    }

    public void StopRecharge()
    {
        isRecharging = false;
        StopCoroutine(recharger);
    }
}
