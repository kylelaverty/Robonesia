using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenAdventurerController : MonoBehaviour
{
    [SerializeField]
    private Sprite characterHead;

    private bool hasSpokeAlready = false;

    // Use Awake to enforce singleton pattern
    private void Awake()
    {
        int citizenAdventurerCount = FindObjectsOfType<CitizenAdventurerController>().Length;
        if (citizenAdventurerCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !hasSpokeAlready)
        {
            hasSpokeAlready = true;
            var gameSession = FindObjectOfType<GameSession>();
            gameSession.StartConversation(
                characterHead,
                new string[] {
                    "Hello, I'm an Adventurer.",
                    "I have heard from the scientist that you are looking for your lost past.",
                    "I met you in your old life, your name was Thomas Cooper.",
                    "The last time I saw you was a few months ago and you were talking about taking a camping trip.",
                    "You should go to the smith's house, you often talked about him and how you two would hang out often. He may know if you went on that trip.",
                    "Hope that helps you in your journey of discovery."
                },
                100
            );
        }
    }
}
