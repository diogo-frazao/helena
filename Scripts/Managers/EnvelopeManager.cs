using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvelopeManager : MonoBehaviour
{
    public static EnvelopeManager Instance { get; private set; }

    [SerializeField]
    private Mask[] envelopes = new Mask[4];

    [SerializeField]
    private Button envelopeButton;

    private int currentEnvelope = 0;

    private Animator envelopePlayerGuideAnimator;

    public bool IsEnvelopeOpen { get; private set; }

    private void Awake()
    {
        SingletonPattern();

        if (envelopeButton == null) { return; }
        envelopePlayerGuideAnimator = envelopeButton.GetComponent<Animator>();
    }

    private void SingletonPattern()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Called on click event on the envelope button.
    public void EnvelopeIconBehaviour()
    {
        IsEnvelopeOpen = !envelopes[currentEnvelope].gameObject.activeInHierarchy;

        if (IsEnvelopeOpen)
        {
            SoundEffectsManager.Instance.letterOpenAudio.Play();
            envelopePlayerGuideAnimator.SetTrigger("open");
            envelopes[currentEnvelope].gameObject.SetActive(true);
        }
        else // Is active.
        {
            CloseEnvelope();
        }
    }

    public bool GetIsEnvelopeOpen()
    {
        return !envelopes[currentEnvelope].gameObject.activeInHierarchy;
    }

    public void CloseEnvelope()
    {
        SoundEffectsManager.Instance.letterCloseAudio.Play();
        envelopePlayerGuideAnimator.SetTrigger("close");
        envelopes[currentEnvelope].gameObject.SetActive(false);
    }

    // Called when the player enters a distraction.
    public void ShowEnvelopeIcon(bool value)
    {
        // Avoids crashing on office.
        if (envelopeButton == null) { return; }

        envelopeButton.gameObject.SetActive(value);
    }

    // Called when the player finishes a distraction.
    public void GoToNextEnvelope()
    {
        currentEnvelope++;
        // TODO adicionar um brilho ou algo desse género.
    }

    public void HideEnvelopes()
    {
        // There are no envelopes to hide in the office (first act).
        if (LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.FirstAct) { return; }

        foreach (var envelope in envelopes)
        {
            if (envelope.gameObject.activeInHierarchy)
            {
                envelope.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateEnvelopeButton(bool value)
    {
        envelopeButton.gameObject.SetActive(value);
    }
}
