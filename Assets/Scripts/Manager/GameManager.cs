using System;
using System.Collections.Generic;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public enum GameState
{
    Idle, Running, Paused, Finished
}

[Serializable]
public class Phase
{
    public float Time;
}

public class GameManager : MonoSingleton<GameManager>
{
    public GameState State = GameState.Idle;
    
    public float GameTime = 180f;

    public List<Phase> PhaseList = new List<Phase>();

    public int CurrentPhase { get; private set; } = 0;

    [Header("Cut Scenes")]
    [SerializeField] private TimelineAsset gameClearTimelineAsset;
    [SerializeField] private TimelineAsset gameFailTimelineAsset;

    private readonly TimeCounter playTimeCounter = new TimeCounter(); //플레이 타이머

    public float RemainTime => playTimeCounter.GetTimeLeft();
    public float ElapsedTime => playTimeCounter.GetElapsedTime();
    
    private PlayableDirector playableDirector;

    protected override void Awake()
    {
        base.Awake();
        
        playableDirector = GetComponentInChildren<PlayableDirector>();
    }
    
    private void OnDestroy()
    {
        AudioManager.Instance.MuteAudioGroup(false);
    }

    private void Update()
    {
        if (State == GameState.Running)
        {
            playTimeCounter.Update();
            
            if (playTimeCounter.WasEndedThisFrame())
            {
                SetGameClear();
            }
            
            ProcessPhase();

            Time.timeScale = 1f;
        }
        else if (State == GameState.Paused)
        {    
            Time.timeScale = 0f;
        }
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void StartGame()
    {
        State = GameState.Running;

        // 플레이 관련 초기화
        SetPhase(0);

        playTimeCounter.Start(GameTime); //플레이 타이머 시작
    }

    public void PauseGame()
    {
        State = GameState.Paused;

        playableDirector.Pause();
    }

    public void ResumeGame()
    {
        State = GameState.Running;

        playableDirector.Resume();
    }
    
    public void FinishGame()
    {
        State = GameState.Finished;
        
        playTimeCounter.Stop(); //플레이 타이머 종료
        EnemyManager.Instance.DeleteAllEnemies(1.5f); //적 모두 삭제
    }
    
    public void SetGameClear()
    {
        FinishGame();
        
        playableDirector.Play(gameClearTimelineAsset);
    }

    public void SetGameOver()
    {
        FinishGame();

        playableDirector.Play(gameFailTimelineAsset);
    }

    private void SetPhase(int next)
    {
        SpawnerManager.Instance.SpawnerPackList[CurrentPhase].Stop();

        CurrentPhase = next;

        SpawnerManager.Instance.SpawnerPackList[CurrentPhase].Start();
    }

    private void ProcessPhase()
    {
        float time = playTimeCounter.GetElapsedTime();

        int nextPhase = CurrentPhase + 1;

        if (nextPhase < PhaseList.Count && time >= PhaseList[nextPhase].Time)
        {
            SetPhase(nextPhase);
        }
    }
}
