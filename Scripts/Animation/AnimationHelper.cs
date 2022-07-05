using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    // Called by animation event.
    public void DeactivateObject()
    {
        gameObject.SetActive(false);
    }

    // Called by animation event.
    public void ActivatePausePanel()
    {
        PauseManager.Instance.ActivatePausePanel();
    }
}
