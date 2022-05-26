using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScene : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        FindObjectOfType<AudioManager>().PlayDoorOpen();
        FindObjectOfType<GameSession>().GoOutside();
    }
}
