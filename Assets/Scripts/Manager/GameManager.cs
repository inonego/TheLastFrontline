using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public enum GameState
{
    Idle, Running, Paused, Finished
}

public class GameManager : PersistentMonoSingleton<GameManager>
{
    public GameState state = GameState.Idle;
    
    public float gameTime = 300f;
    public float RemainTime => playTimeCounter.GetTimeLeft();
    public float ElapsedTime => playTimeCounter.GetElapsedTime();

    private readonly TimeCounter playTimeCounter = new TimeCounter(); //플레이 타이머

    [SerializeField] private Barrier barrier;
    
    [SerializeField] private TimelineAsset gameClearTimelineAsset;
    [SerializeField] private TimelineAsset gameFailTimelineAsset;

    private PlayableDirector playableDirector;

    protected override void Awake()
    {
        base.Awake();
        
        playableDirector = GetComponent<PlayableDirector>();
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
        }
    }
    
    // 씬 로드 시 초기화
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬이 새로 로드될 때 초기화 수행
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetGame();
    }

    public void GoToTitleMenu()
    {
        SceneManager.LoadScene("TitleScene");
    }
    
    // 게임 초기화 메서드
    private void ResetGame()
    {
        FinishGame();
        
        state = GameState.Idle;
        
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        state = GameState.Running;
        
        SpawnerManager.Instance.SpawnerStart(); //스포너 활성화
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
    
    private void FinishGame()
    {
        state = GameState.Finished;
        
        playTimeCounter.Stop(); //플레이 타이머 종료
        EnemyManager.Instance.DeleteAllEnemies(); //적 모두 삭제
        SpawnerManager.Instance.SpawnerStop(); //스포너 
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
}
