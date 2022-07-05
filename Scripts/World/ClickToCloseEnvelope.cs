using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToCloseEnvelope : MonoBehaviour
{
    // Set to false while over button.
    private bool canCloseEnvelope = true;

    [SerializeField]
    private int numberOfClicks = 0;

    public void CanNotCloseEnvelope()
    {
        canCloseEnvelope = false;
    }

    public void CanCloseEnvelope()
    {
        canCloseEnvelope = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && 
            !EnvelopeManager.Instance.GetIsEnvelopeOpen() &&
            canCloseEnvelope)
        {
            if (numberOfClicks == 2)
            {
                EnvelopeManager.Instance.CloseEnvelope();
            }
            else
            {
                numberOfClicks++;
            }
        }
    }
}
