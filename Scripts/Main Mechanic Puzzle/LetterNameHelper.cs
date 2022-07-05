using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterNameHelper : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();

        if (transform.childCount == 0)
        {
            gameObject.tag = GameManager.Instance.GetNormalWordTag();
        }

        gameObject.name = text.text;
    }

    //Called by animation when changing to another sentence (after fading out)
    public void DisableSentence()
    {
        gameObject.SetActive(false);
    }
}
