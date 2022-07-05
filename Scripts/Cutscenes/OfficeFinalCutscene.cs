using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeFinalCutscene : MonoBehaviour
{
    [SerializeField]
    private float desiredXPosition = 301.42f;

    [SerializeField]
    private float cameraSpeed = 4f;

    [SerializeField]
    private Canvas notebookCanvas;

    [SerializeField]
    private Canvas officePuzzleLetterCanvas;

    private Vector3 desiredPosition;
    private bool canChangeScene = true;

    private void OnEnable()
    {
        notebookCanvas.renderMode = RenderMode.WorldSpace;
        officePuzzleLetterCanvas.renderMode = RenderMode.WorldSpace;
    }

    private void Start()
    {
        desiredPosition = new Vector3(desiredXPosition, 
            transform.position.y, transform.position.z);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position,
            desiredPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, desiredPosition, 
                cameraSpeed * Time.deltaTime);
        }
        else
        {
            if (canChangeScene)
            {
                LoadSceneManager.Instance.LoadNextScene();
                canChangeScene = false;
            }
        }
    }
}
