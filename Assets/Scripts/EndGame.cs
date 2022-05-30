using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    private ConversationHistory playerConversationHistory;

    public void OnTriggerEnter2D(Collider2D other)
    {
        // If the player triggers a new life, we want to reset everything.
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            var gameSession = FindObjectOfType<GameSession>();
            var audioManager = FindObjectOfType<AudioManager>();
            Destroy(FindObjectOfType<PlayerController>());
            if (audioManager)
            {
                audioManager.Dispose();
            }
            gameSession.Dispose();
            playerConversationHistory.Reset();
            SceneManager.LoadScene("00 - Menu");
        }
    }
}
