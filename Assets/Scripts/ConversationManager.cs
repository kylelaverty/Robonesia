using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ConversationManager : MonoBehaviour
{
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

    private void Update()
    {
        if (conversationSentences != null && conversationText.text == conversationSentences[conversationSentenceIndex])
        {
            continueButton.gameObject.SetActive(true);
        }
    }

    // Accept in an image referencing the speaker, an array of the sentences to be spoken and the memory to award when it is done.
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

    // Handle typing the sentence one letter at a time.
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
            conversationPanel.SetActive(false);
            FindObjectOfType<PlayerController>().canMove = true;
            conversationText.text = string.Empty;
            conversationSpeakerImage.sprite = null;
            FindObjectOfType<GameSession>().IncreaseMemory(memoryToAwardAfterConversation);
            memoryToAwardAfterConversation = 0;
        }
    }
}
