using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusicPlayer : MonoBehaviour
{
    private AudioSource menuMusic;
    private AudioSource finalMenuMusic;

    private void OnEnable()
    {
        menuMusic = GetComponent<AudioSource>();
        finalMenuMusic = transform.Find("Final Menu Music").GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.FirstAct ||
            LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.SecondAct)
        {
            menuMusic.Stop();
        }
        // If is a menu scene.
        else
        {
            if (GameManager.Instance.GetCanPlayFinalSong())
            {
                finalMenuMusic.Play();

                // Time that the final menu music lasts.
                Invoke(nameof(PlayMenuMusic), 12f);

                GameManager.Instance.CanPlayFinalSong(false);
            }
            // Can play menu song.
            else
            {
                if (finalMenuMusic.isPlaying) { menuMusic.Stop(); }
                if (!menuMusic.isPlaying) { menuMusic.Play(); }
            }
        }
    }

    private void PlayMenuMusic()
    {
        menuMusic.Play();
    }

    private void Awake()
    {
        int numberOfMe = FindObjectsOfType<MenuMusicPlayer>().Length;

        if (numberOfMe > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
