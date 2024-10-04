using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool IsGameStarted { get; set;}= false; //플레이가 시작되었나
    public bool IsGamePaused { get; set; }= false;//멈췄나
    public bool IsGameOver { get; set;} = false;//끝났나
    
    
    public float gameTime = 0f;
    [SerializeField]
    private float remainTime;

    
    public float restartTime = 0f;
    private TimeCounter playTimeCounter;
    private TimeCounter restartTimeCounter;
    
    // Start is called before the first frame update
    void Start()
    {
        IsGameStarted = true;
        playTimeCounter = new TimeCounter();
        restartTimeCounter = new TimeCounter();
    }

    // Update is called once per frame
    void Update()
    {
        playTimeCounter.Update();
        restartTimeCounter.Update();
        remainTime = playTimeCounter.GetTimeLeft();
        
        if (IsGameStarted||restartTimeCounter.WasEndedThisFrame()) //게임 플레이 시작 (재시작)
        {
            //대충 장면 전환
            
            playTimeCounter.Start(gameTime);
            IsGameStarted = false;
        }

        if (IsGameOver) //실패 
        {
            //대충 실패 효과
            
            
            restartTimeCounter.Start(restartTime);
            IsGameOver = false;
        }

        if (playTimeCounter.WasEndedThisFrame()) //승리
        {
            //대충 성공 효과
            
            
        }
    }
}
