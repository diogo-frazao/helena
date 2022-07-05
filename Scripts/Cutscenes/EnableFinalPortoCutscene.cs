using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableFinalPortoCutscene : MonoBehaviour
{
    [SerializeField]
    private string deliverLetterAnimationString;

    public bool wasEnvelopeOpenedLastTime = false;
    private void Update()
    {
        // If closed the letter, deliver it.
        if (SoundEffectsManager.Instance.letterCloseAudio.isPlaying &&
            wasEnvelopeOpenedLastTime)
        {
            ArmandoAnimationManager.Instance.PlayAnimationTrigger(deliverLetterAnimationString);
            PortoMusicManager.Instance.GoToNextPuzzleSong();
            wasEnvelopeOpenedLastTime = false;
        }
    }

    // Called by animation event only after delivering the letter.
    public void EnablePortoFinalCutscene()
    {
        FindObjectOfType<PortoFinalCutscene>().enabled = true;
    }

    public void SetWasEnvelopeOpenedLastTime(bool value)
    {
        wasEnvelopeOpenedLastTime = value;
    }
}
