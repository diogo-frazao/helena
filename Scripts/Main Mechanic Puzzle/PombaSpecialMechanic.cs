using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PombaSpecialMechanic : MonoBehaviour
{
    [SerializeField]
    private Transform pieceToFollowTransform;

    private float scaleSpeed = 500f;
    private Vector3 scaleDuringAnimation = new Vector3(85.74617f, 80.26741f, 0f);
    private Vector3 desiredScale = new Vector3(270f, 252.7483f, 0f);

    private PuzzlePiece myPuzzlePiece;

    private void Awake()
    {
        myPuzzlePiece = GetComponent<PuzzlePiece>();
    }

    private void Start()
    {
        pieceToFollowTransform.GetComponentInParent<Animator>().SetTrigger("fly");
    }

    private void Update()
    {
        if (myPuzzlePiece.pieceState == PuzzlePiece.State.Idle)
        {
            // Follow Tuna Note.
            transform.position = pieceToFollowTransform.position;
            transform.localScale = scaleDuringAnimation;
        }
        else if (myPuzzlePiece.pieceState == PuzzlePiece.State.Dragging)
        {
            // Adjust scale to match the desired.
            if (Vector3.Distance(transform.localScale, desiredScale) > 0.05f)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale,
                    desiredScale, scaleSpeed * Time.deltaTime);
            }
        }
        else if (myPuzzlePiece.pieceState == PuzzlePiece.State.Stop)
        {
            pieceToFollowTransform.GetComponentInParent<Animator>().SetTrigger("stop");
        }
    }
}
