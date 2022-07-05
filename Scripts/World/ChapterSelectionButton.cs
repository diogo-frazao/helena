using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterSelectionButton : MonoBehaviour
{
    private void Start()
    {
        Cursor.SetCursor(CustomMouseManager.Instance.GetNormalCursor(),
            Vector2.zero, CursorMode.ForceSoftware);
    }
    private void OnMouseDown()
    {
        GameManager.Instance.IsFirstTimeOnGame = false;
        Cursor.SetCursor(CustomMouseManager.Instance.GetNormalCursor(),
            Vector2.zero, CursorMode.ForceSoftware);
        LoadSceneManager.Instance.FadeOutGoToNextScene();
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(CustomMouseManager.Instance.GetHoverCursor(),
            Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(CustomMouseManager.Instance.GetNormalCursor(),
            Vector2.zero, CursorMode.ForceSoftware);
    }
}
