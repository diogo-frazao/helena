using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortoMusicColliders : MonoBehaviour
{
    [Tooltip("0 Means chair, 1 Tuna and 2 Malibox")]
    [SerializeField]
    private PortoMusicManager.DistractionMusic currentDistraction;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PortoMusicManager.Instance.distraction = currentDistraction;

            PortoMusicManager.Instance.SetCanPlay(true);
            PortoMusicManager.Instance.SetIsInDistraction(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PortoMusicManager.Instance.SetIsInDistraction(false);
        }
    }
}
