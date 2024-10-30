using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    public bool IsGameStarted { get; set; } = false; //플레이가 시작되었나
    public bool IsGamePaused { get; set; } = false;//멈췄나
    public bool IsGameOver { get; set; } = false;//끝났나


    public float gameTime = 300f;
    public float RemainTime { get; private set; }
    public float ElapsedTime { get; private set; }
    [SerializeField]
    private float remainTime;

    public float restartTime = 0f;
    private TimeCounter playTimeCounter; //플레이 타이머
    private TimeCounter restartTimeCounter; //재시작 타이머
    
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
        Time.timeScale = 1f;
        ResetGame();
    }

    // 게임 초기화 메서드
    public void ResetGame()
    {
        if (SceneManager.GetActiveScene().name != "MainScene")
        {
            IsGameStarted = false;
        }
        else
        {
            IsGameStarted = true;
        }
        IsGamePaused = false;
        IsGameOver = false;

        playTimeCounter = new TimeCounter();
        restartTimeCounter = new TimeCounter();

        Debug.Log("GameManager: 게임 초기화 완료");
    }

    // Start is called before the first frame update
    void Start()
    {
        IsGameStarted = true; //임의로 시작 
        playTimeCounter = new TimeCounter();
        restartTimeCounter = new TimeCounter();
        Debug.Log("Game Start");
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainScene")
        {
            return;
        }
            
        
        playTimeCounter.Update();
        restartTimeCounter.Update();

        RemainTime = playTimeCounter.GetTimeLeft();
        ElapsedTime = playTimeCounter.GetElapsedTime();

        if (IsGameStarted || restartTimeCounter.WasEndedThisFrame()) //게임 플레이 시작 (재시작)
        {
            //대충 장면 전환
            SpawnerManager.instance.SpawnerStart(); //스포너 활성화
            playTimeCounter.Start(gameTime); //플레이 타이머 시작
            IsGameStarted = false;
        }

        if (IsGameOver) //실패 
        {
            //대충 실패 효과
            playTimeCounter.Stop(); //플레이 타이머 종료
            EnemyManager.instance.DeleteAllEnemies(); //적 모두 삭제
            SpawnerManager.instance.SpawnerStop(); //스포너 비활성화
            restartTimeCounter.Start(restartTime); //재시작 타이머 시작
            IsGameOver = false;
        }

        if (playTimeCounter.WasEndedThisFrame()) //승리
        {
            //대충 성공 효과

            EnemyManager.instance.DeleteAllEnemies(1f);//적 모두 삭제
            SpawnerManager.instance.SpawnerStop();//스포너 비활성화
        }

    }

    // EnemySpawner에서 남은 시간 확인용
    public float GetElapsedTime()
    {
        return playTimeCounter.GetElapsedTime();
    }

}
