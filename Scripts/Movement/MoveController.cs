using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 4f;

    [SerializeField]
    private float minLimit;

    [SerializeField]
    private float maxLimit;

    //Cached References
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;

    //Instance variables
    private float xInput;
    private bool canMove = true;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Since Armando now has inspiration, we will wirte the first paragraph
        // of the letter.
        if (LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.SecondAct)
        {
            StartCoroutine(WriteBeforeWalking());
        }
    }

    private IEnumerator WriteBeforeWalking()
    {
        canMove = false;
        myAnimator.SetTrigger("write up");

        yield return new WaitForSeconds(5f);

        myAnimator.SetTrigger("stop writing");
        SoundEffectsManager.Instance.letterOpenAudio.Play();
        EnvelopeManager.Instance.ActivateEnvelopeButton(true);
        canMove = true;
    }

    private void Update()
    {
        if (canMove)
        {
            xInput = Input.GetAxis("Horizontal");
        }

        CheckFlip();
    }
    private void CheckFlip()
    {
        if (transform.right.x > 0 && xInput < 0 ||
            transform.right.x < 0 && xInput > 0)
        {
            Flip();
        }

        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x, minLimit, maxLimit),
            transform.position.y,
            transform.position.z
        );
    }

    private void FixedUpdate()
    {
        myRigidbody.velocity = new Vector2(xInput * moveSpeed, myRigidbody.velocity.y);

        myAnimator.SetFloat("horizontalSpeed", Mathf.Abs(myRigidbody.velocity.x));
    }

    private void Flip()
    {
        Vector3 myRotation = transform.localEulerAngles;
        myRotation.y += 180f;
        transform.localEulerAngles = myRotation;
    }

    // Called by the distraction cutscene since the player will move without input to the desired pos.
    public void CanMove(bool value)
    {
        canMove = value;
    }

    public void SetXInput(float value)
    {
        xInput = value;
    }

    public bool GetIsMovingForward()
    {
        if (xInput > 0)
        {
            return true;
        }
        return false;
    }
}
