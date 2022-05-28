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

    [Header("Conversation")]
    [SerializeField]
    private GameObject conversationPanel;
    [SerializeField]
    private Image conversationSpeakerImage;
    [SerializeField]
    private TextMeshProUGUI conversationText;
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private float typingSpeed = 0.02f;

    private string[] conversationSentences;
    private int conversationSentenceIndex;
    private int memoryToAwardAfterConversation;
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
        Debug.Log("GameSession Start");
        Debug.Log("Player Energy: " + playerEnergy);
        Debug.Log("Player Memories: " + playerMemories);
        playerEnergySlider.value = playerEnergy;
        playerMemoriesSlider.value = playerMemories;
    }

    private void Update()
    {
        if (conversationSentences != null && conversationText.text == conversationSentences[conversationSentenceIndex])
        {
            continueButton.gameObject.SetActive(true);
        }
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

    public void OnClickQuick()
    {
        Destroy(FindObjectOfType<PlayerController>());
        ResetGameSession();
    }

    // Accept in an image referencing the speaker and an array of the sentences to be spoken.
    public void StartConversation(Sprite entityHeadReference, string[] newConversationSentences, int memoryAwarded)
    {
        conversationSentences = newConversationSentences;
        memoryToAwardAfterConversation = memoryAwarded;
        FindObjectOfType<PlayerController>().canMove = false;
        conversationPanel.SetActive(true);
        conversationSpeakerImage.sprite = entityHeadReference;
        continueButton.gameObject.SetActive(false);
        StartCoroutine(TypeConversation());
    }

    private IEnumerator TypeConversation()
    {
        foreach (char letter in conversationSentences[conversationSentenceIndex].ToCharArray())
        {
            conversationText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        continueButton.gameObject.SetActive(false);

        if (conversationSentenceIndex < conversationSentences.Length - 1)
        {
            conversationSentenceIndex++;
            conversationText.text = string.Empty;
            StartCoroutine(TypeConversation());
        }
        else
        {
            Debug.Log("Finished Conversation");
            conversationPanel.SetActive(false);
            FindObjectOfType<PlayerController>().canMove = true;
            conversationText.text = string.Empty;
            conversationSpeakerImage.sprite = null;
            Debug.Log("Memory Awarded: " + memoryToAwardAfterConversation);
            IncreaseMemory(memoryToAwardAfterConversation);
            memoryToAwardAfterConversation = 0;
        }
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
