using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishermanSpecialMechanic : MonoBehaviour
{
    private Transform pieceTransform;
    private PuzzlePiece myPuzzlePiece;
    private Rigidbody2D myRigidbody;

    private void Awake()
    {
        pieceTransform = GameObject.FindGameObjectWithTag("Fisherman Piece").transform;
        myPuzzlePiece = GetComponent<PuzzlePiece>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        pieceTransform.GetComponentInParent<Animator>().SetTrigger("fish");
    }

    private void Update()
    {
        if (myPuzzlePiece.pieceState == PuzzlePiece.State.Idle)
        {
            transform.position = pieceTransform.position;
        }
        else if (myPuzzlePiece.pieceState == PuzzlePiece.State.Stop)
        {
            pieceTransform.GetComponentInParent<Animator>().ResetTrigger("fish");
            pieceTransform.GetComponentInParent<Animator>().SetTrigger("stop");
        }
    }
}
