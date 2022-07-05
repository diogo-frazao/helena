using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }

    private const int MaxCollectibles = 12;
    private int numberOfCollectibles = 0;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset collectibles when returning to office.
        if (LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.FirstAct)
        {
            numberOfCollectibles = 0;
        }
        else if (LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.SecondAct)
        {
            Collectible.SetCanPickCollectible(true);
        }
    }

    // Called by the collectible in order to update the UI
    public void AddNewCollectible()
    {
        numberOfCollectibles++;
    }

    public int GetNumberOfCollectibles()
    {
        return numberOfCollectibles;
    }

    public int GetMaxNumberOfCollectibles()
    {
        return MaxCollectibles;
    }
}
