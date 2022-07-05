using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMouseManager : MonoBehaviour
{
    public static CustomMouseManager Instance { get; private set; }

    [SerializeField]
    private Texture2D normalCursor;

    [SerializeField]
    private Texture2D hoverCursor;

    [SerializeField]
    private Texture2D pieceCursor;

    [SerializeField]
    private Texture2D beforeGrabbingCursor;

    [SerializeField]
    private Texture2D grabbingCursor;

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
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    public Texture2D GetHoverCursor()
    {
        return hoverCursor;
    }

    public Texture2D GetNormalCursor()
    {
        return normalCursor;
    }

    public Texture2D GetPieceCursor()
    {
        return pieceCursor;
    }

    public Texture2D GetBeforeGrabbingCursor()
    {
        return beforeGrabbingCursor;
    }

    public Texture2D GetGrabbingCursor()
    {
        return grabbingCursor;
    }
}
