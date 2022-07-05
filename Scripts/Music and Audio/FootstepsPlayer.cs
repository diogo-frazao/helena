using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;

    //Cached References
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StepSound()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    // Used as a simulation for the paper hitting the floor.
    public void SecondStepSound()
    {
        audioSource.PlayOneShot(clips[1]);
    }

    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }
}
