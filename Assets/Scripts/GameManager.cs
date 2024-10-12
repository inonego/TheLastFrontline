using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    public bool IsGameStarted { get; set;}= false; //플레이가 시작되었나
    public bool IsGamePaused { get; set; }= false;//멈췄나
    public bool IsGameOver { get; set;} = false;//끝났나
    
    
    public float gameTime = 0f; 
    public float RemainTime { get; private set; }
    public float ElapsedTime { get; private set; }

    
    public float restartTime = 0f;
    private TimeCounter playTimeCounter; //플레이 타이머
    private TimeCounter restartTimeCounter; //재시작 타이머
    
    // Start is called before the first frame update
    void Start()
    {
        IsGameStarted = true; //임의로 시작 
        playTimeCounter = new TimeCounter();
        restartTimeCounter = new TimeCounter();
    }

    // Update is called once per frame
    void Update()
    {
        playTimeCounter.Update();
        restartTimeCounter.Update();
        
        RemainTime = playTimeCounter.GetTimeLeft();
        ElapsedTime = playTimeCounter.GetElapsedTime();
        
        if (IsGameStarted||restartTimeCounter.WasEndedThisFrame()) //게임 플레이 시작 (재시작)
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
}
