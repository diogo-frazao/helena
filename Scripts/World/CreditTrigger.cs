using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditTrigger : MonoBehaviour
{
    [SerializeField]
    private Text[] textsToShow;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var text in textsToShow)
                text.gameObject.SetActive(true);
        }
    }
}
