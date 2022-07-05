using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMouseOnHover : MonoBehaviour
{
    [Tooltip("If set to true, after on mouse click the mouse will not change anymore above this GO")]
    [SerializeField]
    private bool canChangeOnlyOnce = false;

    private bool cannotChange = false;

    private void OnMouseDown()
    {
        if (canChangeOnlyOnce)
            cannotChange = true;

        Cursor.SetCursor(CustomMouseManager.Instance.GetNormalCursor(),
            Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseEnter()
    {
        if (cannotChange) { return; }

        Cursor.SetCursor(CustomMouseManager.Instance.GetHoverCursor(), 
            Vector2.zero, CursorMode.ForceSoftware);
    }
    private void OnMouseExit()
    {
        if (cannotChange) { return; }

        Cursor.SetCursor(CustomMouseManager.Instance.GetNormalCursor(),
            Vector2.zero, CursorMode.ForceSoftware);
    }

    public void SetCanChangeOnlyOnce(bool value)
    {
        canChangeOnlyOnce = value;
    }
}
