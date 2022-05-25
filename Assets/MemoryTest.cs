using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryTest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        FindObjectOfType<GameSession>().IncreaseMemory(50);
    }
}
