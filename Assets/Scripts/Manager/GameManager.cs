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
    public float time;
}

public class GameManager : MonoSingleton<GameManager>
{
    public GameState state = GameState.Idle;
    
    public List<Phase> phaseList = new List<Phase>();

    public int currentPhase { get; private set; }= 0;

    public float gameTime = 180f;

    private readonly TimeCounter playTimeCounter = new TimeCounter(); //플레이 타이머

    public float RemainTime => playTimeCounter.GetTimeLeft();
    public float ElapsedTime => playTimeCounter.GetElapsedTime();
    
    [SerializeField] private TimelineAsset gameClearTimelineAsset;
    [SerializeField] private TimelineAsset gameFailTimelineAsset;

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
        if (state == GameState.Running)
        {
            playTimeCounter.Update();
            
            if (playTimeCounter.WasEndedThisFrame())
            {
                SetGameClear();
            }
            
            ProcessPhase();
        }
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void StartGame()
    {
        state = GameState.Running;

        Time.timeScale = 1f;

        // 플레이 관련 초기화
        SetPhase(0);

        playTimeCounter.Start(gameTime); //플레이 타이머 시작
    }

    public void PauseGame()
    {
        state = GameState.Paused;
        
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        state = GameState.Running;

        Time.timeScale = 1f;
    }
    
    public void FinishGame()
    {
        state = GameState.Finished;
        
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
        SpawnerManager.Instance.SpawnerPackList[currentPhase].Stop();

        currentPhase = next;

        SpawnerManager.Instance.SpawnerPackList[currentPhase].Start();
    }

    private void ProcessPhase()
    {
        float time = playTimeCounter.GetElapsedTime();

        int nextPhase = currentPhase + 1;

        if (nextPhase < phaseList.Count && time >= phaseList[nextPhase].time)
        {
            SetPhase(nextPhase);
        }
    }
}
