using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FastPlayTest : MonoBehaviour
{
    private PlayableDirector playableDirector;

    public bool CutSceneOnStart = true;
    public GameState StartState = GameState.Idle;
    public bool SetGameClear = true;
    public bool SetGameFail = true;


    private void Awake()
    {
        playableDirector = GetComponentInChildren<PlayableDirector>();
    }
    
    public void Start()
    {
        if (!CutSceneOnStart)
        {
            playableDirector.Stop();

            AudioManager.Instance.MuteAudioGroup(false);
        }

        if (StartState == GameState.Idle)
        {
            GameManager.Instance.StartGame();
        }
        else
        //
        if (StartState == GameState.Finished)
        {
            if (SetGameClear)
            {
                GameManager.Instance.SetGameClear();
            }

            if (SetGameFail)
            {
                GameManager.Instance.SetGameOver();
            }
        }
    }
    
}
