using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeMusicManager : MonoBehaviour
{
    public static OfficeMusicManager Instance { get; private set; }

    [Space]
    [Header("First Music")]

    [SerializeField]
    private AudioSource firstMusic;

    [SerializeField]
    private Transform startFirstMusicTransform;

    [SerializeField]
    private Transform stopFirstMusicTransform;

    [Space]
    [Header("Second Music")]

    [SerializeField]
    private AudioSource secondMusic;

    [SerializeField]
    private Transform startSecondMusicTransform;

    [SerializeField]
    private Transform stopSecondMusicTransform;

    [Space]
    [Header("Puzzle Music")]

    [SerializeField]
    private AudioSource puzzleFirstMusic;

    [SerializeField]
    private AudioSource puzzleSecondMusic;

    [SerializeField]
    private AudioSource puzzleThirdMusic;

    //TODO add fourth puzzle music

    //Cached References
    private MoveController player;

    //Instance Variables
    private bool canPlay = true;
    private bool canStop = true;
    
    //Music state
    public enum Music { First, SecondFadeIn, Second, PuzzleFirst, PuzzleSecond, PuzzleThird, PuzzleFourth}
    public Music music;

    private void Awake()
    {
        SingletonPattern();
    }

    private void SingletonPattern()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = FindObjectOfType<MoveController>();
        music = Music.First;
    }

    private void Update()
    {
        switch (music)
        {
            case Music.First:
                MusicBehaviour(firstMusic, startFirstMusicTransform, stopFirstMusicTransform, true);
                break;
            case Music.Second:
                MusicBehaviour(secondMusic, startSecondMusicTransform, stopSecondMusicTransform, false);
                break;
        }
    }

    private void MusicBehaviour(AudioSource music, Transform startTransform, Transform endTransform, bool hasDynamicVolume)
    {
        float distanceToStopPos = Vector2.Distance(player.transform.position, endTransform.position);

        bool isBetweenMinAndMax = player.transform.position.x > startTransform.position.x &&
                                  player.transform.position.x < endTransform.position.x;

        if (isBetweenMinAndMax)
        {
            PlayMusic(music);
            SetDynamicVolume(music, startTransform, endTransform, hasDynamicVolume, distanceToStopPos);
        }
        else
        {
            StopMusicAndGoToNext(music, hasDynamicVolume);
        }
    }
    private void PlayMusic(AudioSource music)
    {
        if (canPlay)
        {
            music.Play();
            canPlay = false;
            canStop = true;
        }
    }

    private void SetDynamicVolume(AudioSource music, Transform startTransform, Transform endTransform, bool hasDynamicVolume, float distanceToStopPos)
    {
        if (hasDynamicVolume)
        {
            music.volume = (distanceToStopPos / Vector2.Distance(startTransform.position, endTransform.position));
        }
        else
        {
            music.volume = 1;
        }
    }

    private void StopMusicAndGoToNext(AudioSource currentMusic, bool hasDynamicVolume)
    {
        if (canStop)
        {
            if (hasDynamicVolume)
            {
                StopOnce(currentMusic);
            }
            else                                  //Smooth vanish (to avoid being abrupt)
            {
                SmoothStop(currentMusic);
            }
        }
    }

    private void SmoothStop(AudioSource currentMusic)
    {
        currentMusic.volume -= 0.5f * Time.deltaTime;

        if (currentMusic.volume <= 0f)
        {
            StopOnce(currentMusic); 
        }
    }

    private void StopOnce(AudioSource currentMusic)
    {
        currentMusic.Stop();               //Stop normal (its already decrementing until 0
        music++;
        canStop = false;
        canPlay = true;
    }

    //Called by puzzle piece when it should change to another puzzle song
    public void GoToNextPuzzleSong()
    {
        music++;

        switch (music)
        {
            case Music.PuzzleSecond:

                puzzleFirstMusic.Stop();
                puzzleSecondMusic.Play();

                break;
            case Music.PuzzleThird:

                puzzleSecondMusic.Stop();
                puzzleThirdMusic.Play();

                break;
            case Music.PuzzleFourth:
                // TODO ainda não temos 4th music.
                break;
        }
    }
}
