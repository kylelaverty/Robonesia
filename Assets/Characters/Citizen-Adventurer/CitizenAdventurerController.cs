using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenAdventurerController : MonoBehaviour
{
    [SerializeField]
    private Sprite characterHead;
    [SerializeField]
    private ConversationHistory playerConversationHistory;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !playerConversationHistory.metAdventurer)
        {
            playerConversationHistory.metAdventurer = true;
            var conversationManager = FindObjectOfType<ConversationManager>();
            conversationManager.StartConversation(
                characterHead,
                new string[] {
                    "Hello, I'm an Adventurer.",
                    "I have heard from the scientist that you are looking for your lost past.",
                    "I met you in your old life, your name was Thomas Cooper.",
                    "The last time I saw you was a few months ago and you were talking about taking a camping trip.",
                    "You should go to the smith's house, you often talked about him and how you two would hang out often. He may know if you went on that trip.",
                    "Hope that helps you in your journey of discovery."
                },
                5
            );
        }
    }
}
