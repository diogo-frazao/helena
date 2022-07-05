using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PuzzlePiece : MonoBehaviour
{
    [Space]

    [SerializeField]
    private ParticleSystem feedbackParticle;

    [Space]

    [SerializeField]
    private bool isWrongPiece = false;

    [SerializeField]
    private bool isSpecialPuzzlePiece = true;

    [SerializeField]
    private Transform desiredPosition;

    [Range(0, 1)]
    [SerializeField]
    private float accuracy = 0.4f;

    [SerializeField]
    private PlayAnimationAfterPuzzle myAnimationToPlayAfterPuzzle;

    [Space]
    [Header("Bounce Related")]

    [SerializeField]
    private bool canBounce = true;

    [SerializeField]
    private float pieceBounceSpeed = 0.5f;

    [Space]
    [Header("Moving Related")]

    [SerializeField]
    private float mouseSpeed = 4f;

    [SerializeField]
    private float pieceGoingMoveSpeed = 6f;

    [SerializeField]
    private float pieceReturningMoveSpeed = 7f;

    [SerializeField]
    private float pieceReturningAccuracy = 0.5f;

    // Instance variables.
    private bool canDrag = true;
    private float pieceMoveAccuracy = 0.05f;
    public bool WasDragged { get; private set; } = false;


    private PuzzlePiece[] piecesForThisWord;

    // Bounce Related.
    private bool canSelectDirection = true;
    private Vector3 direction;
    private Vector2[] directions = new Vector2[4]
    {
        new Vector2(-1, 1),
        new Vector2(1, 1),
        new Vector2(-0.5f, 0.5f),
        new Vector2(0.5f, 0.5f)
    };

    // Piece state Related.
    public enum State { Idle, Dragging, Returning, Going, Stop}
    public State pieceState;

    // Cached References.
    private Camera mainCamera;
    private Vector3 startingPosition;
    private DistractionPuzzle myDistractionPuzzle;
    private Animator myAnimator;
    private Rigidbody2D myRigidbody;


    public void CheckIsSpecialPiece()
    {
        if (isSpecialPuzzlePiece)
        {
            GetComponent<Animator>().SetBool("isSpecialPiece", true);
        }
    }

    private void Awake()
    {
        desiredPosition.SetParent(transform.parent);
        pieceState = State.Idle;
        myDistractionPuzzle = GetComponentInParent<TextRevealer>().GetDistraction();
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (SoundEffectsManager.Instance.penWritingAudio.isPlaying)
            SoundEffectsManager.Instance.penWritingAudio.Stop();

        mainCamera = Camera.main;
        startingPosition = transform.position;
        accuracy = Vector2.Distance(transform.position, desiredPosition.position) * accuracy;
        piecesForThisWord = GetComponentInParent<TextRevealer>().GetPiecesInChildren();

        if (!canBounce) { canSelectDirection = false; }

        StartCoroutine(FixColliderBug());
    }

    private IEnumerator FixColliderBug()
    {
        Vector2 colliderOffset = new Vector2(0.5f, 0.5f);
        GetComponent<CapsuleCollider2D>().size -= colliderOffset;
        yield return new WaitForSeconds(0.1f);
        GetComponent<CapsuleCollider2D>().size += colliderOffset;
    }

    private void Update()
    {
        switch (pieceState)
        {
            case State.Returning:

                MoveTowardsStartingPosition();
                break;
            case State.Going:

                MoveTowardsDesiredPosition();
                break;

            case State.Idle:

                if (canSelectDirection)
                {
                    MoveRandomDirection();
                }
                break;
        }

        // TODO can be improved but since the piece only lives for a short time.
        // it doesn't impact performance that much.
        if (pieceState != State.Idle)
        {
            // Avoids moving in a direction on other states.
            myRigidbody.velocity = Vector2.zero;
        }
    }

    private void MoveRandomDirection()
    {
        direction = SelectRandomDirection();
        myRigidbody.velocity = direction * pieceBounceSpeed;
    }

    private Vector3 SelectRandomDirection()
    {
        int randomInt = Random.Range(0, directions.Length);
        canSelectDirection = false;

        return directions[randomInt];
    }

    private void MoveTowardsStartingPosition()
    {
        if (Vector2.Distance(transform.position, startingPosition)
            >= pieceReturningAccuracy)
        {
            Vector3 direction = startingPosition - transform.position;
            transform.position += direction * pieceReturningMoveSpeed * 0.4f * Time.deltaTime;
        }
        else
        {
            if (canBounce) { canSelectDirection = true; }
            pieceState = State.Idle;
            WasDragged = true;
        }
    }

    private void MoveTowardsDesiredPosition()
    {
        if (Vector2.Distance(transform.position, desiredPosition.position)
            >= pieceMoveAccuracy)
        {
            Vector3 direction = desiredPosition.position - transform.position;
            transform.position += direction * pieceGoingMoveSpeed * 0.4f * Time.deltaTime;
        }
        // If I've reached the desired position.
        else
        {
            if (isWrongPiece)
            {
                Invoke(nameof(ChangeToReturning), 0.5f);
                SoundEffectsManager.Instance.puzzleIncorrectAudio.Play();
            }
            else
            {
                // Disable all the pieces for this word.
                foreach (var piece in piecesForThisWord)
                {
                    if (piece.isWrongPiece)
                    {
                        piece.myAnimator.SetBool("canBlink", false);
                        piece.myAnimator.SetBool("fadeOut", true);
                    }
                    // It's the correct piece.
                    else
                    {
                        Cursor.SetCursor(CustomMouseManager.Instance.GetNormalCursor(),
                           Vector2.zero, CursorMode.ForceSoftware);

                        myAnimator.SetBool("canBlink", false);
                        pieceState = State.Stop;
                        canDrag = false;

                        SoundEffectsManager.Instance.puzzleCorrectAudio.Play();

                        if (myAnimationToPlayAfterPuzzle != null) 
                        { myAnimationToPlayAfterPuzzle.CanPlayAnimation = true; }
                    }

                    if (!SoundEffectsManager.Instance.penWritingAudio.isPlaying)
                        SoundEffectsManager.Instance.penWritingAudio.Play();
                }

                Instantiate(feedbackParticle, new Vector3(
                    transform.position.x, transform.position.y, 
                    feedbackParticle.transform.position.z), Quaternion.identity);

                myDistractionPuzzle.GoToNext();
            }
        }
    }

    private void OnMouseDrag()
    {
        if (!canDrag) { return; }

        // Change mouse cursor.
        Cursor.SetCursor(CustomMouseManager.Instance.GetPieceCursor(),
            Vector2.zero, CursorMode.ForceSoftware);

        pieceState = State.Dragging;

        // Smooth follow mouse position.
        Vector2 desiredPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Vector2.Lerp(transform.position, desiredPosition, 
            mouseSpeed * Time.deltaTime);
    }

    private void OnMouseUp()
    {
        float distanceToTarget = Vector2.Distance(transform.position, desiredPosition.position);

        if (distanceToTarget <= accuracy)
        {
            // Go Back to inicial position.
            pieceState = State.Going;
        }
        else
        {
            // Stay where you are but keep moving randomly.
            canSelectDirection = true;
            pieceState = State.Idle;
        }
    }

    private void OnMouseDown()
    {
        SoundEffectsManager.Instance.pieceGrabSound.Play();
    }

    private void OnMouseEnter()
    {
        if (!canDrag) { return; }

        Cursor.SetCursor(CustomMouseManager.Instance.GetPieceCursor(),
            Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(CustomMouseManager.Instance.GetNormalCursor(),
            Vector2.zero, CursorMode.ForceSoftware);
    }

    private void ChangeToReturning()
    {
        pieceState = State.Returning;
    }

    // Called by animation when changing to another sentence (after fading out).
    public void DisablePuzzlePiece()
    {
        Destroy(gameObject);
    }

    // Called by the last frame of the fade in animation.
    public void CanBlink()
    {
        myAnimator.SetBool("canBlink", true);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        direction = Vector3.Reflect(myRigidbody.velocity.normalized, other.contacts[0].normal);
    }
}
