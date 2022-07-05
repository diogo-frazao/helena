using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CurrentScene
{
    MainMenu,
    Storyboard,
    ChapterSelection,
    PombaCutscene,
    FirstAct,
    SecondAct
}

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager Instance { get; private set; }

    private CurrentScene currentScene;
    private Animator myAnimator;
    private Transform fadeImage;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = (CurrentScene)SceneManager.GetActiveScene().buildIndex;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        fadeImage = GetComponentInChildren<Transform>();
    }

    private void Update()
    {
        if (Input.anyKeyDown && currentScene == CurrentScene.MainMenu)
        {
            if (Input.GetButtonDown("Exit"))
            {
                Application.Quit();
            }
            else
            {
                FadeOutGoToNextScene();
            }
        }
    }

    public void FadeOutGoToNextScene()
    {
        FadeOut();
        Invoke(nameof(FadeIn), 1f);
    }


    private void FadeOut()
    {
        fadeImage.position = new Vector3(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y,
            fadeImage.position.z);
        myAnimator.SetTrigger("fadeOut");
    }

    private void FadeIn()
    {
        fadeImage.position = new Vector3(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y,
            fadeImage.position.z);
        myAnimator.SetTrigger("fadeIn");
    }

    // Called by animation event.
    private void ResetTriggers()
    {
        myAnimator.ResetTrigger("fadeOut");
        myAnimator.ResetTrigger("fadeIn");
    }

    // Called by animation event and by other classes.
    public void LoadNextScene()
    {
        var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadSceneAsync(nextSceneIndex);
        }
        // Return to main menu.
        else
        {
            GameManager.Instance.CanPlayFinalSong(true);
            SceneManager.LoadSceneAsync(0);
        }
    }

    // Method Overloading.
    public void LoadNextScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public CurrentScene GetCurrentScene()
    {
        return (CurrentScene) SceneManager.GetActiveScene().buildIndex;
    }
}
