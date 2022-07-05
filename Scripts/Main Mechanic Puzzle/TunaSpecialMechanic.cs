using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunaSpecialMechanic : MonoBehaviour
{
    [SerializeField]
    private Transform pieceToFollowTransform;

    [SerializeField]
    private Transform tunaTransform;

    private const float ColliderRadius = 0.3f;
    private const float ScaleDifference = 50.4f;
    [SerializeField]
    private float scaleSpeed = 20f;

    private Vector3 desiredScale = new Vector3(252.7484f, 252.7484f, 0f);

    private PuzzlePiece myPuzzlePiece;
    private CircleCollider2D myCircleCollider;

    private void Awake()
    {
        myPuzzlePiece = GetComponent<PuzzlePiece>();
        myCircleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        pieceToFollowTransform.GetComponentInParent<Animator>().SetTrigger("sing");
        tunaTransform.GetComponent<Animator>().SetTrigger("sing");
    }

    private void Update()
    {
        if (myPuzzlePiece.pieceState == PuzzlePiece.State.Idle)
        {
            // Avoids the collider getting smaller as it follows the piece.
            myCircleCollider.radius = ColliderRadius;

            // Follow Tuna Note.
            transform.position = pieceToFollowTransform.position;
            transform.rotation = pieceToFollowTransform.rotation;
            transform.localScale = pieceToFollowTransform.localScale * ScaleDifference;
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
    }
}
