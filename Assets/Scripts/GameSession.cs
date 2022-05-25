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
        Debug.Log("GameSession Start");
        Debug.Log("Player Energy: " + playerEnergy);
        Debug.Log("Player Memories: " + playerMemories);
        playerEnergySlider.value = playerEnergy;
        playerMemoriesSlider.value = playerMemories;
    }

    public void IncreaseEnergy(int value)
    {
        // We only want to increase the energy if the player is not already dead.
        // This prevents the player from touching the charge pod just as they die.
        if (playerEnergy < 0)
        {
            playerEnergy += value;
            playerEnergySlider.value = playerEnergy;
        }
    }

    public void DecreaseEnergy(int value)
    {
        // Decrease the energy.
        playerEnergy -= value;

        // If they are now dead, then we need to load the death scene.
        if (playerEnergy <= 0)
        {
            // Set the energy to 0 just incase and update the UI.
            playerEnergy = 0;
            playerEnergySlider.value = playerEnergy;

            // Load the death scene.
            PlayerDeadEndGame();
        }
        else
        {
            playerEnergySlider.value = playerEnergy;
        }
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
        Destroy(gameObject);
    }

    private void PlayerDeadEndGame()
    {
        LoadFinalMap("99 - Death");
    }

    private void PlayerWinGame()
    {
        LoadFinalMap("98 - Win");
    }

    private void LoadFinalMap(string sceneName)
    {
        // Cleanup the player controller to make sure the DecreaseEnergy process is stopped.
        Destroy(FindObjectOfType<PlayerController>());
        SceneManager.LoadScene(sceneName);
    }
}
