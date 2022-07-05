using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [SerializeField]
    private Image darkenBackgroundImage;

    [SerializeField]
    private Image envelopOpenningImage;

    [Header("Pause Panel")]

    [SerializeField]
    private Image pausePanel;

    [SerializeField]
    private Button returnButton;

    [SerializeField]
    private Button leaveButton;

    [Space]
    [Header("Are you sure Panel")]

    [SerializeField]
    private Image areYouSurePanel;

    [SerializeField]
    private Button yesButton;

    [SerializeField]
    private Button noButton;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.FirstAct ||
            LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.SecondAct)
        {
            gameObject.SetActive(true);
        }
        // If is a menu scene.
        else
        {
            gameObject.SetActive(false);
        }
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

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            PauseBehavior();
        }
    }

    public void PauseBehavior()
    {
        if (areYouSurePanel.gameObject.activeInHierarchy)
        {
            ReturnToPreviousScreen();
        }
        else if (pausePanel.gameObject.activeInHierarchy)
        {
            ResumeButton();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        darkenBackgroundImage.gameObject.SetActive(true);
        envelopOpenningImage.gameObject.SetActive(true);
    }

    private void ReturnToPreviousScreen()
    {
        Time.timeScale = 0f;
        areYouSurePanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(true);
    }

    // Called by animation event.
    public void ActivatePausePanel()
    {
        pausePanel.gameObject.SetActive(true);
        areYouSurePanel.gameObject.SetActive(false);
    }

    #region Called By Pause Buttons

    public void ResumeButton()
    {
        Time.timeScale = 1f;
        darkenBackgroundImage.gameObject.SetActive(false);
        envelopOpenningImage.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        areYouSurePanel.gameObject.SetActive(false);
    }

    public void LeaveButton()
    {
        pausePanel.gameObject.SetActive(false);
        areYouSurePanel.gameObject.SetActive(true);
    }

    public void YesButton()
    {
        // Hides Pause Panel.
        ResumeButton();
        LoadSceneManager.Instance.LoadNextScene(0);
    }

    public void NoButton()
    {
        areYouSurePanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(true);
    }

    #endregion
}
