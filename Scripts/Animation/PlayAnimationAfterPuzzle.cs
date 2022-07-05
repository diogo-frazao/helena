using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationAfterPuzzle : MonoBehaviour
{
    [SerializeField]
    private float timeToPlayAnimation = 3f;

    [SerializeField]
    private string animationString;

    public bool CanPlayAnimation { get; set; } = false;

    private void Update()
    {
        if (CanPlayAnimation)
        {
            Invoke(nameof(StartAnimation), timeToPlayAnimation);
            CanPlayAnimation = false;
        }
    }

    private void StartAnimation()
    {
        ArmandoAnimationManager.Instance.PlayAnimationTrigger(animationString);
    }
}
