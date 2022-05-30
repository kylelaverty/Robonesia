using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinReaper : MonoBehaviour
{
    [SerializeField]
    private Sprite characterHead;
    [SerializeField]
    private ConversationHistory playerConversationHistory;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !playerConversationHistory.metReaper)
        {
            playerConversationHistory.metReaper = true;
            var conversationManager = FindObjectOfType<ConversationManager>();
            conversationManager.StartConversation(
                characterHead,
                new string[] {
                    "Hello Thomas, I'm the Reaper.",
                    "So, you know the truth now.",
                    "Did it bring you closure?",
                    "People wonder what the afterlife is like, what it will be for them.",
                    "You get the unique chance to experience it twice.",
                    "The mirror to the right will end your journey and let your sprit finally rest. You are free to linger here as long as you wish."
                },
                0
            );
        }
    }
}
