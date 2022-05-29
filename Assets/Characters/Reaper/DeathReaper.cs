using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathReaper : MonoBehaviour
{
    [SerializeField]
    private Sprite characterHead;
    [SerializeField]
    private ConversationHistory playerConversationHistory;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !playerConversationHistory.metReaper)
        {
            Debug.Log("DeathReaper: Player entered");
            playerConversationHistory.metReaper = true;
            var conversationManager = FindObjectOfType<ConversationManager>();
            conversationManager.StartConversation(
                characterHead,
                new string[] {
                    "Hello Thomas, I'm the Reaper.",
                    "I assume you know why you are here.",
                    "Your new body ran out of power and you have passed on for the final time.",
                    "There is no going back now, at least not as you once were.",
                    "The mirror to the right will give you another shot at this journey but with a catch.",
                    "You will forget everything you have learned about yourself.",
                    "I'm sorry, but that is the best I can offer you.",
                    "You are free to stay by the fire for as long as you would like."
                },
                0
            );
        }
    }
}
