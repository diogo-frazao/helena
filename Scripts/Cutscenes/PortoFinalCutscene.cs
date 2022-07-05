using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortoFinalCutscene : MonoBehaviour
{
    [SerializeField]
    private float desiredYPosition = 25f;

    [SerializeField]
    private float cameraSpeed = 3f;

    private Vector3 desiredPosition;

    private void Start()
    {
        // Stops following the player.
        GetComponent<SmoothFollow>().enabled = false;

        desiredPosition = new Vector3( transform.position.x, 
            desiredYPosition, transform.position.z);

        FindObjectOfType<ArtCollectiblesManager>().ShowCollectiblesArt();

        Invoke(nameof(EnableCollectiblesUI), 5f);
    }

    private void EnableCollectiblesUI()
    {
        UIManager.Instance.EnableAndStayEnabledCollectiblesUI();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, desiredPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, 
                desiredPosition, cameraSpeed * Time.deltaTime);
        }
        else
        {
            UIManager.Instance.DisableCollectiblesUI();
            LoadSceneManager.Instance.FadeOutGoToNextScene();
        }
    }
}
