using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FastPlayTest : MonoBehaviour
{
    private PlayableDirector playableDirector;

    private void Awake()
    {
        playableDirector = GetComponentInChildren<PlayableDirector>();
    }
    
    public void Start()
    {
        playableDirector.Stop();

        GameManager.Instance.StartGame();

        AudioManager.Instance.MuteAudioGroup(false);
    }
    
}
