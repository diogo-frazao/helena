using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBars : MonoBehaviour
{

    private Animator myAnimator;
    private bool canFadeIn = true;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void BlackBarsBechaviour()
    {
        if (canFadeIn)
        {
            myAnimator.SetTrigger("fadeIn");
            canFadeIn = false;
        }
        else //can fade out
        {
            myAnimator.SetTrigger("fadeOut");
            canFadeIn = true;
        }
    }
}
