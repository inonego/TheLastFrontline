using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class GameManager : Singleton<GameManager>
{
    public bool IsGameStarted { get; set; } = false; //플레이가 시작되었나
    public bool IsGamePaused { get; set; } = false;//멈췄나
    public bool IsGameOver { get; set; } = false;//끝났나


    public float gameTime = 0f;
    public float RemainTime { get; private set; }
    public float ElapsedTime { get; private set; }
    [SerializeField]
    private float remainTime;

    public float restartTime = 0f;
    private TimeCounter playTimeCounter; //플레이 타이머
    private TimeCounter restartTimeCounter; //재시작 타이머

    // UI용 변수들
    [Header("Game Play UI")]
    public GameObject gamePanel;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI phaseText;
    public GameObject ScriptPanel;
    public TextMeshProUGUI ScriptText;
    public GameObject BarrierUI;
    public GameObject BulletUI;
    public Image BarrierHP;
    public Image BulletCount;

    // 스크립트 대사용 변수
    private float scriptTime = 0f; // 각 스크립트 보여주는 시간
    private bool isFadingOut = false; // Panel 페이드 아웃 중인지 확인
    private float fadeDuration = 2f; // 페이드 인/아웃 시간
    private int currentPhase = 0; // 현재 phase

    // PausePanel용 변수
    public InputActionReference pauseAction;
    public GameObject pausePanel;
    public bool isPaused { get; private set; }

    //Settings용 변수
    public GameObject settingsPanel;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        IsGameStarted = true; //임의로 시작 
        playTimeCounter = new TimeCounter();
        restartTimeCounter = new TimeCounter();

        BarrierHP = BarrierUI.GetComponent<Image>();
        BulletCount = BulletUI.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
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

        ShowScriptText();
    }

    void LateUpdate()
    {
        UpdateUIText();
    }

    // EnemySpawner에서 남은 시간 확인용
    public float GetElapsedTime()
    {
        return playTimeCounter.GetElapsedTime();
    }

    public void DecreaseBarrierHP()
    {
        BarrierHP.fillAmount -= 0.05f;
    }

    public void ShowBulletCount(float ratio)
    {
        BulletCount.fillAmount = ratio;
    }

    void UpdateUIText()
    {

        if (playTimeCounter.GetElapsedTime() < 120f)
        {
            phaseText.text = "Phase 1";
        }
        else if (playTimeCounter.GetElapsedTime() < 240f)
        {
            phaseText.text = "Phase 2";
        }
        else
        {
            phaseText.text = "Phase 3";
        }

        int min = (int)(RemainTime / 60f);
        int sec = (int)(RemainTime % 60);

        timeText.text = string.Format("{0:00}:{1:00}", min, sec);
    }

    void ShowScriptText()
    {
        float nowTime = playTimeCounter.GetElapsedTime();

        if (scriptTime < fadeDuration && !isFadingOut) // 페이드 인 효과
        {
            ScriptPanel.GetComponent<Image>().color = new Color(1, 1, 1, scriptTime / fadeDuration);
            ScriptText.text = "";
        }
        else if (nowTime < 120f) // phase 1
        {
            currentPhase = 1;

            if (nowTime < 5f)
            {
                ScriptText.text = "The detonation device has been activated, but it's set to trigger in 5 minutes for safety.";
            }
            else if (nowTime < 8f)
            {
                ScriptText.text = "I have to hold off these monsters for the next 5 minutes.";
            }
            else if (nowTime < 11f)
            {
                ScriptText.text = "… My legs are shattered. I can barely move.";
            }
            else if (nowTime < 14f)
            {
                ScriptText.text = "Damn it…";
            }
            else if (!isFadingOut) // Panel 페이드 아웃
            {
                StartFadeOut();
            }
        }
        else if (nowTime < 240f) // phase 2
        {
            if (currentPhase != 2)
            {
                StartNewPhase(2);
            }
            if (nowTime < 125f)
            {
                ScriptText.text = "More enemies are closing in.";
            }
            else if (nowTime < 128f)
            {
                ScriptText.text = "What are these things? What are they really?";
            }
            else if (nowTime < 131f)
            {
                ScriptText.text = "What is their purpose for invading Earth over and over again?";
            }
            else if (!isFadingOut)
            {
                StartFadeOut();
            }
        }
        else // phase 3
        {
            if (currentPhase != 3)
            {
                StartNewPhase(3);
            }
            if (nowTime < 245f)
            {
                ScriptText.text = "This is an onslaught on a completely different scale.";
            }
            else if (nowTime < 248f)
            {
                ScriptText.text = "This must be their final assault. If I can hold them off this time, victory will be ours.";
            }
            else if (!isFadingOut)
            {
                StartFadeOut();
            }
        }

        if (isFadingOut)
        {
            float fadeOutTime = scriptTime - fadeDuration;
            ScriptPanel.GetComponent<Image>().color = new Color(1, 1, 1, 1 - (fadeOutTime / fadeDuration));
            if (fadeOutTime >= fadeDuration)
            {
                isFadingOut = false; // 페이드 아웃 완료
                ScriptPanel.SetActive(false); // 대화창 비활성화
            }
        }

        scriptTime += Time.deltaTime;
    }

    void StartFadeOut()
    {
        isFadingOut = true;
        scriptTime = fadeDuration; // 페이드 아웃 시작 시간 설정
    }

    void StartNewPhase(int phase)
    {
        currentPhase = phase;
        scriptTime = 0f; // 새로운 phase -> 시간 초기화
        isFadingOut = false;
        ScriptPanel.SetActive(true); // 대화창 활성화
        ScriptPanel.GetComponent<Image>().color = new Color(1, 1, 1, 0); // 투명하게 시작
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f; // 게임 중단 (시간 정지)
            isPaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f; // 게임 재개 (시간 정상)
        isPaused = false;
    }

    public void ShowSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToGame()
    {
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }
}
