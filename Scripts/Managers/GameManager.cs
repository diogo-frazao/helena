using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const string NormalWordTag = "Normal Word";

    private Camera mainCamera;
    private SmoothFollow cameraSmoothFollow;

    private GameObject bounceBoundariesParent;

    private static bool canPlayFinalSong = false;

    public bool IsFirstTimeOnGame { get; set; } = true;

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
        cameraSmoothFollow = FindObjectOfType<Camera>().GetComponent<SmoothFollow>();

        bounceBoundariesParent = GameObject.FindGameObjectWithTag("Bounce Boundaries Parent");
        if (bounceBoundariesParent != null)
            bounceBoundariesParent.SetActive(false);

        if (LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.FirstAct)
        {
            Cursor.SetCursor(CustomMouseManager.Instance.GetNormalCursor(),
                Vector2.zero, CursorMode.ForceSoftware);
            Cursor.visible = true;
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        cameraSmoothFollow = mainCamera.GetComponent<SmoothFollow>();
    }

    public string GetNormalWordTag()
    {
        return NormalWordTag;
    }

    public bool IsPuzzlePiece(Transform letter)
    {
        if (letter.gameObject.CompareTag(GetNormalWordTag()))
        {
            return false;
        }
        return true;
    }

    public void CameraFollowBehaviour()
    {
        if (cameraSmoothFollow.enabled == true)
        {
            cameraSmoothFollow.enabled = false;
        }
        else
        {
            cameraSmoothFollow.enabled = true;
        }
    }

    public void CanPlayFinalSong(bool value)
    {
        canPlayFinalSong = value;
    }

    public bool GetCanPlayFinalSong()
    {
        return canPlayFinalSong;
    }

    public void ActivatePuzzleBoundaries(bool value)
    {
        bounceBoundariesParent.SetActive(value);
    }
}
