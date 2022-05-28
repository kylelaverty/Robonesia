using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoadByName;
    [SerializeField]
    private Vector2 playerPosition;
    [SerializeField]
    private VectorValue playerPositionStorage;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerPositionStorage.initialValue = playerPosition;
            SceneManager.LoadScene(sceneToLoadByName);
        }
    }
}
