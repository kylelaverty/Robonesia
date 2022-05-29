using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip doorOpenSound;

    [SerializeField]
    private AudioClip doorCloseSound;

    [SerializeField]
    private AudioSource musicSource;

    // Use Awake to enforce singleton pattern.
    private void Awake()
    {
        int gameSessionCount = FindObjectsOfType<AudioManager>().Length;
        if (gameSessionCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        StartMusic();
    }

    // Play door open audio clip.
    public void PlayDoorOpen()
    {
        AudioSource.PlayClipAtPoint(doorOpenSound, Camera.main.transform.position);
    }

    // Play door close audio clip.
    public void PlayDoorClose()
    {
        AudioSource.PlayClipAtPoint(doorCloseSound, Camera.main.transform.position);
    }

    public void ChangeMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }

        GetComponent<AudioSource>().Play();

    }

    // If not already started, start the music source.
    private void StartMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }
}