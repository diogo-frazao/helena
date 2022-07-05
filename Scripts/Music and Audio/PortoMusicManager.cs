using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortoMusicManager : MonoBehaviour
{
    public static PortoMusicManager Instance { get; private set; }

    [SerializeField]
    private AudioSource[] backgroundMusics;

    private AudioSource distractionsMusic;

    [SerializeField]
    private AudioSource firstTunaMusic;

    [SerializeField]
    public AudioSource secondTunaMusic;

    [SerializeField]
    public AudioSource thirdTunaMusic;

    [SerializeField]
    private AudioSource finalFirstMusic;

    [SerializeField]
    private AudioSource finalSecondMusic;

    [SerializeField]
    private AudioSource finalThirdMusic;

    private bool isInDistraction = false;

    public enum DistractionMusic { Chair = 1, Tuna = 2, FinalFirst = 4, FinalSecond = 5,
    FinalThird = 6}
    public DistractionMusic distraction;

    private bool canPlay = true; // Avoids playing 60x per second.

    public bool CanDecreaseSecondTunaMusic { get; set; } = false;

    private void Awake()
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
        distraction = 0;

        foreach (var backgroundMusic in backgroundMusics)
            backgroundMusic.Play();
    }

    private void Update()
    {
        if (CanDecreaseSecondTunaMusic)
        {
            SmoothDecrease(secondTunaMusic, false, 0f);
        }

        if (isInDistraction)
        {
            switch(distraction)
            {
                case DistractionMusic.Tuna:
                    SmoothDecrease(backgroundMusics[0], false, 0f);
                    SmoothDecrease(backgroundMusics[1], false, 0f);
                    // If already passed once.
                    if (CanDecreaseSecondTunaMusic)
                    {
                        thirdTunaMusic.volume = 1f;
                        PlayOnce(thirdTunaMusic);
                    }
                    else
                    {
                        PlayOnce(firstTunaMusic);
                    }
                    SmoothPlay(firstTunaMusic, 1f);
                    break;

                case DistractionMusic.FinalFirst:
                    backgroundMusics[0].Stop();
                    backgroundMusics[1].Stop();
                    PlayOnce(finalFirstMusic);
                    break;

                default:
                    SmoothDecrease(backgroundMusics[0], false, 0f);
                    break;
            }
        }
        // Is outside of a distraction.
        else
        {
            SmoothDecrease(thirdTunaMusic, false, 0f);
            SoundEffectsManager.Instance.CanIncreasePortoBackgroundVolume = true;

            SmoothPlay(backgroundMusics[0], 1f);
        }
    }

    private void SmoothDecrease(AudioSource music, bool canStop, float minValue)
    {
        music.volume -= 0.5f * Time.deltaTime;

        if (music.volume <= minValue)
        {
            if (canStop)
            {
                music.Stop();
            }
            else
            {
                music.volume = minValue;
            }
        }
    }

    private void SmoothPlay(AudioSource music, float maxVolume)
    {
        music.volume += 0.5f * Time.deltaTime;

        if (music.volume >= maxVolume)
        {
            music.volume = maxVolume;
        }
    }

    private void PlayOnce(AudioSource music)
    {
        if (canPlay)
        {
            music.Play();
            canPlay = false;
        }
    }

    public void SetIsInDistraction(bool value)
    {
        isInDistraction = value;
    }

    public void SetCanPlay(bool value)
    {
        canPlay = value;
    }

    public void GoToNextPuzzleSong()
    {
        distraction++;

        switch (distraction)
        {
            case DistractionMusic.FinalFirst:
                finalFirstMusic.Play();

                break;
            case DistractionMusic.FinalSecond:
                finalFirstMusic.Stop();
                finalSecondMusic.Play();

                break;
            case DistractionMusic.FinalThird:
                finalSecondMusic.Stop();
                finalThirdMusic.Play();

                break;
        }
    }
}
