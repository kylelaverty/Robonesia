using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField]
    private int playerEnergy = 100;

    [SerializeField]
    private int playerMemories = 0;

    [SerializeField]
    private Slider playerEnergySlider;

    [SerializeField]
    private Slider playerMemoriesSlider;

    [SerializeField]
    private GameObject menuPanel;

    [SerializeField]
    private ConversationHistory playerConversationHistory;

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
        playerEnergySlider.value = playerEnergy;
        playerMemoriesSlider.value = playerMemories;
    }

    public void IncreaseEnergy(int value)
    {
        // We only want to increase the energy if the player is not already dead.
        // This prevents the player from touching the charge pod just as they die.
        if (playerEnergy > 0)
        {
            playerEnergy += value;
            playerEnergySlider.value = playerEnergy;
        }
    }

    public void DecreaseEnergy(int value)
    {
        // Only decrease energy if player is not actively recharging.
        if (!isRecharging)
        {
            // Decrease the energy.
            playerEnergy -= value;

            // If they are now dead, then we need to load the death scene.
            if (playerEnergy <= 0)
            {
                EnergyZero();
            }
            else
            {
                playerEnergySlider.value = playerEnergy;

                if (playerEnergy <= 0)
                {
                    EnergyZero();
                }
            }
        }
    }

    private void EnergyZero()
    {
        // Set the energy to 0 just incase and update the UI.
        playerEnergy = 0;
        playerEnergySlider.value = playerEnergy;

        // Load the death scene.
        PlayerDeadEndGame();
    }

    public void IncreaseMemory(int value)
    {
        playerMemories += value;
        if (playerMemories >= 100)
        {
            playerMemories = 100;
            playerMemoriesSlider.value = playerMemories;
            PlayerWinGame();
        }
        else
        {
            playerMemoriesSlider.value = playerMemories;
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
        ResetGameSession();
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
