using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSignal : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void FinishGame()
    {
        GameManager.Instance.FinishGame();
    }

    public void GoToScene(string sceneName)
    {
        GameManager.Instance.GoToScene(sceneName);
    }

    public void MuteAudioMixerGroup(bool value)
    {
        AudioManager.Instance.MuteAudioGroup(value);
    }
}
