using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeMusicCollider : MonoBehaviour
{
    [SerializeField]
    private AudioSource myMusic;

    [SerializeField]
    private bool isSecondMusicFade = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            myMusic.Play();

            if (isSecondMusicFade)
            {
                //the time it takes to reach the pillar
                Invoke(nameof(GoToNextSong), 14f);
            }
        }
    }

    private void GoToNextSong()
    {
        OfficeMusicManager.Instance.music++;
    }
}
