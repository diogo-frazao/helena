using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager Instance { get; private set; }

    [SerializeField]
    public AudioSource pikcupAudio, letterCloseAudio, letterOpenAudio,
        penWritingAudio, puzzleCorrectAudio, puzzleIncorrectAudio,
        officeChairDragAudio, paperScrunch, pieceGrabSound, fishermanRodBend, fishermanCaught,
        doveIdleSound, doveCantFlySound, doveFlySound;

    [SerializeField]
    private AudioSource[] officeBackgroundSounds;

    [SerializeField]
    private AudioSource[] portoBackgroundSounds;

    private const float MaxBackgroundSoundsVolume = 0.3f;

    public bool CanIncreaseOfficeBackgroundVolume { get; set; } = false;
    public bool CanDecreaseOfficeBackgroundVolume { get; set; } = false;

    public bool CanIncreasePortoBackgroundVolume { get; set; } = false;
    public bool CanDecreasePortoBackgroundVolume { get; set; } = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (CanIncreaseOfficeBackgroundVolume)
        {
            foreach (var officeSound in officeBackgroundSounds)
            {
                VolumeIncreaseOverTime(true, officeSound);
            }
        }

        if (CanDecreaseOfficeBackgroundVolume)
        {
            foreach (var officeSound in officeBackgroundSounds)
            {
                VolumeIncreaseOverTime(false, officeSound);
            }
        }

        if (CanIncreasePortoBackgroundVolume)
        {
            foreach (var portoSound in portoBackgroundSounds)
            {
                VolumeIncreaseOverTime(true, portoSound);
            }
        }

        if (CanDecreasePortoBackgroundVolume)
        {
            foreach (var portoSound in officeBackgroundSounds)
            {
                VolumeIncreaseOverTime(false, portoSound);
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.FirstAct)
        {
            foreach (var officeSound in officeBackgroundSounds)
            {
                officeSound.Play();
                officeSound.volume = 0;
            }
            CanIncreaseOfficeBackgroundVolume = true;
        }
        else if (LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.SecondAct)
        {
            foreach (var portoSound in portoBackgroundSounds)
            {
                portoSound.Play();
                portoSound.volume = 0;
            }
            CanIncreasePortoBackgroundVolume = true;
        }
        // Stop all sounds.
        else
        {
            foreach (var sound in officeBackgroundSounds)
                if (sound.isPlaying)
                    sound.Stop();

            foreach (var pSound in portoBackgroundSounds)
                if (pSound.isPlaying)
                    pSound.Stop();
        }
    }

    private void VolumeIncreaseOverTime(bool hasToIncrease, AudioSource audio)
    {
        if (hasToIncrease)
        {
            if (audio.volume < MaxBackgroundSoundsVolume)
            {
                audio.volume += 0.05f * Time.deltaTime;
            }
            else
            {
                audio.volume = MaxBackgroundSoundsVolume;

                CanIncreaseOfficeBackgroundVolume = false;
                CanIncreaseOfficeBackgroundVolume = false;
                CanDecreaseOfficeBackgroundVolume = false;
                CanDecreasePortoBackgroundVolume = false;
            }
        }
        else
        {
            if (audio.volume > 0f)
            {
                audio.volume -= 0.05f * Time.deltaTime;
            }
            else
            {
                audio.volume = 0f;

                CanIncreaseOfficeBackgroundVolume = false;
                CanIncreaseOfficeBackgroundVolume = false;
                CanDecreaseOfficeBackgroundVolume = false;
                CanDecreasePortoBackgroundVolume = false;
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
