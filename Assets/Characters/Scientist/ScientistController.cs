using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScientistController : MonoBehaviour
{
    [SerializeField]
    private Sprite characterHead;

    private bool hasSpokeAlready = false;

    // Use Awake to enforce singleton pattern
    private void Awake()
    {
        int scientistCount = FindObjectsOfType<ScientistController>().Length;
        if (scientistCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the dialog only triggers the first time the scientist is created.
        if (!hasSpokeAlready)
        {
            hasSpokeAlready = true;
            var gameSession = FindObjectOfType<GameSession>();
            gameSession.StartConversation(
                characterHead,
                new string[] {
                    "Hello, I'm a scientist. I'm here to help you find your missing memories.",
                    "I moved your brain into this body to help solve why you died, I got your body and nothing seemed wrong.",
                    "But your memories did not transfer correctly and now you don't know who you are.",
                    "By interacting with different people in town you should be able to rebuild your memories and reconstruct your past, maybe even solve what happened to you and find closer and peace.",
                    "Oh, one more thing, you are a robot now so you need power. When your power gets low come back and recharge at the yellow crystal."
                },
                5
            );
        }
    }
}
