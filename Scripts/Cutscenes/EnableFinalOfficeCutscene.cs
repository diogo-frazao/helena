using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableFinalOfficeCutscene : MonoBehaviour
{
    [SerializeField]
    private PuzzlePiece[] officePuzzlePieces = new PuzzlePiece[3];

    private bool canChange = true;

    private void Update()
    {
        if (CanChangeScene() && canChange)
        {
            FadeOutPieces();
            ArmandoAnimationManager.Instance.PlayAnimationTrigger("throw paper");
            // Play the song delayed 1.5 seconds, 44100 = 1sec.
            SoundEffectsManager.Instance.paperScrunch.Play(44100 + 22050);
            // The activate office cutscene function is called by an animation event on the trigger.
            canChange = false;
        }
    }

    private bool CanChangeScene()
    {
        // If at least one of them was not dragged.
        foreach (var piece in officePuzzlePieces)
        {
            if (!piece.WasDragged)
            {
                return false;
            }
        }
        return true;
    }

    private void FadeOutPieces()
    {
        foreach (var piece in officePuzzlePieces)
        {
            piece.GetComponent<Animator>().SetBool("canBlink", false);
            piece.GetComponent<Animator>().SetBool("fadeOut", true);
        }
    }
}
