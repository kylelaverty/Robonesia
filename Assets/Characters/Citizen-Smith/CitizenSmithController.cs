using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenSmithController : MonoBehaviour
{
    [SerializeField]
    private Sprite characterHead;
    [SerializeField]
    private ConversationHistory playerConversationHistory;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !playerConversationHistory.metSmith)
        {
            playerConversationHistory.metSmith = true;
            var conversationManager = FindObjectOfType<ConversationManager>();
            conversationManager.StartConversation(
                characterHead,
                new string[] {
                    "Hello, I'm a Smith.",
                    "I have heard from the scientist that you are looking for your lost past.",
                    "We used to hang out a lot by the lake and talk about life.",
                    "You used to work a 9 to 5 in the big city as a paper pushing office drone. You came here to find a simpler life.",
                    "I think you found it, doing odd jobs around town, helping people out.",
                    "You were planning a camping trip a few weeks back, the gas station attendant said he saw you heading out to the camp grounds."
                },
                90
            );
        }
    }
}
