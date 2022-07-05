using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePuzzleMusic : MonoBehaviour
{
    // Called by the first word of the sentence that changes the  music
    private void Start()
    {
        if (LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.FirstAct)
        {
            OfficeMusicManager.Instance.GoToNextPuzzleSong();
        }
        else if (LoadSceneManager.Instance.GetCurrentScene() == CurrentScene.SecondAct)
        {
            PortoMusicManager.Instance.GoToNextPuzzleSong();
        }
    }
}
