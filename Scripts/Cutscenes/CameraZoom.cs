using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    private float cameraZoomSpeed = 3f;

    public enum CameraState { Idle, ZoomIn, ZoomOut, ZoomInMovement}
    public CameraState cameraState;

    private Camera myCamera;
    private MoveController player;

    public float DefaultZoom { get; private set; }
    public float MaxZoom { get; private set; }
    public float MinZoom { get; private set;}

    private float desiredZoom;

    private void Awake()
    {
        myCamera = GetComponent<Camera>();
        cameraState = CameraState.ZoomInMovement;
        player = FindObjectOfType<MoveController>();
    }

    private void Start()
    {
        DefaultZoom = myCamera.orthographicSize;
        MaxZoom = DefaultZoom - 0.72f;
        MinZoom = DefaultZoom + 0.48f;

        myCamera.orthographicSize = MinZoom;
    }

    private void Update()
    {
        switch (cameraState)
        {
            case CameraState.ZoomIn:
                if (myCamera.orthographicSize - desiredZoom > 0.001f)
                {
                    myCamera.orthographicSize -= cameraZoomSpeed * Time.deltaTime;
                }
                else
                {
                    cameraState = CameraState.Idle;
                }
                break;
            case CameraState.ZoomOut:
                if (DefaultZoom - myCamera.orthographicSize > 0.01f)
                {
                    myCamera.orthographicSize += cameraZoomSpeed * Time.deltaTime;
                }
                else
                {
                    cameraState = CameraState.Idle;
                }
                break;
            case CameraState.ZoomInMovement:
                if (myCamera.orthographicSize - DefaultZoom > 0.01f &&
                    player.GetIsMovingForward())
                {
                    myCamera.orthographicSize -= cameraZoomSpeed * 0.35f * Time.deltaTime;
                }
                else if (myCamera.orthographicSize - DefaultZoom <= 0.01f)
                {
                    cameraState = CameraState.Idle;
                }
                break;
        }
    }

    public void ZoomIn(float desiredZoom)
    {
        cameraState = CameraState.ZoomIn;
        this.desiredZoom = desiredZoom;
    }

    public void ZoomOut(float desiredZoom)
    {
        cameraState = CameraState.ZoomOut;
        this.desiredZoom = desiredZoom;
    }

    public void ZoomInWithMovement()
    {
        cameraState = CameraState.ZoomInMovement;
    }
}
