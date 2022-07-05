using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Distraction
{
    OfficeChair, 
    OportoChair,
    Tuna,
    MailBox
}

public class DistractionCutscene : MonoBehaviour
{
    [SerializeField]
    private Distraction distraction;

    [SerializeField]
    private Transform desiredTransform;

    [Tooltip("The string that triggers the desired animation")]
    [SerializeField]
    private string animationString;

    [SerializeField]
    private float cutscenePlayerSpeedFactor = 1.1f;

    private bool canMoveToDesiredPosition = false;
    private float stoppingDistance = 0.2f;

    private bool hasAlreadyEnteredCutscene = false;

    private Vector2 desiredPosition;

    // Cached References.
    private MoveController player;
    private BlackBars blackBars;
    private Animator playerAnimator;
    private CameraZoom cameraZoom;

    private void Start()
    {
        player = FindObjectOfType<MoveController>();
        playerAnimator = player.GetComponent<Animator>();
        blackBars = FindObjectOfType<BlackBars>();
        cameraZoom = FindObjectOfType<CameraZoom>();

        // Makes sure that the player doesn't go down and keeps the same y during cutscenes.
        desiredPosition = new Vector2(desiredTransform.position.x, 
                                      player.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasAlreadyEnteredCutscene)
        {
            EnvelopeManager.Instance.HideEnvelopes();
            EnvelopeManager.Instance.ShowEnvelopeIcon(false);

            GameManager.Instance.CameraFollowBehaviour();
            blackBars.BlackBarsBechaviour();

            // Player Related.
            player.CanMove(false);
            canMoveToDesiredPosition = true;

            // Used for avoiding repeating the cutscene.
            hasAlreadyEnteredCutscene = true;

            Collectible.SetCanPickCollectible(false);
        }
    }

    private void Update()
    {
        if (canMoveToDesiredPosition)
        {
            SmoothMoveTo(player, desiredPosition);
        }
    }

    private void SmoothMoveTo(MoveController player, Vector3 desiredPosition)
    {
        if (Vector2.Distance(player.transform.position, desiredPosition) > stoppingDistance)
        {
            // Moves a bit slower to the desired position to show its an automate movement.
            player.SetXInput(cutscenePlayerSpeedFactor);
        }
        else
        {
            CutsceneBehavior(distraction);

            canMoveToDesiredPosition = false;
            blackBars.BlackBarsBechaviour();

            // Stops moving since the player is on the puzzle distraction.
            player.SetXInput(0f);

            if (LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.FirstAct)
            {
                SoundEffectsManager.Instance.officeChairDragAudio.Play(44100);
            }

            if (distraction == Distraction.MailBox)
            {
                SoundEffectsManager.Instance.doveIdleSound.Play();
            }

            cameraZoom.ZoomIn(cameraZoom.MaxZoom);

            playerAnimator.SetTrigger(animationString);
        }
    }

    private void CutsceneBehavior(Distraction distraction)
    {
        switch (distraction)
        {
            case Distraction.OfficeChair:
                ArmandoAnimationManager.Instance.EnableArmandoChair();
                //ArmandoAnimationManager.Instance.DisableOfficeChair();
                SoundEffectsManager.Instance.CanDecreaseOfficeBackgroundVolume = true;
                break;

            case Distraction.Tuna:
                SoundEffectsManager.Instance.CanDecreasePortoBackgroundVolume = true;
                break;
            case Distraction.MailBox:
                SoundEffectsManager.Instance.CanDecreasePortoBackgroundVolume = true;
                break;

        }
    }
}
