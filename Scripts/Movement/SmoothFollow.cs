using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [Tooltip("The higher the number the longer it takes to follow the target")]
    [SerializeField]
    private float smoothSpeed = 0.125f;

    [SerializeField]
    private float maxLimit = 38.3f;


    [SerializeField]
    private Vector3 offset = new Vector3(0f, 0f, -10f);

    //Cached References
    private Transform targetTransform = null;

    //Instance Variables
    private float minLimit = 0f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        targetTransform = FindObjectOfType<MoveController>().transform;

        if (LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.FirstAct)
        {
            minLimit = transform.position.x;
        }
        else if (LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.SecondAct)
        {
            minLimit = transform.position.x;
            Invoke(nameof(GetNewMinX), 1f);
        }
    }

    // Avoids the camera not following player on transition to seecond act
    private void GetNewMinX()
    {
        minLimit = 1f;
    }

    private void FixedUpdate()
    {
        SmoothFollowPlayer();
        ConstrainCameraPosition();
    }

    private void SmoothFollowPlayer()
    {
        Vector3 targetPosition = targetTransform.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, 
            targetPosition, ref velocity, smoothSpeed);
    }

    private void ConstrainCameraPosition()
    {
        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x, minLimit, maxLimit),
            transform.position.y,
            transform.position.z
        );
    }
}
