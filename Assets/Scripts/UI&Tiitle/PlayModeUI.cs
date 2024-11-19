using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayModeUI : MonoBehaviour
{
    // UI용 변수들
    [Header("Script UI")]
    public GameObject ScriptPanel;
    public TextMeshProUGUI ScriptText;
    private float scriptTime = 0f; // 각 스크립트 보여주는 시간
    private bool isFadingOut = false; // Panel 페이드 아웃 중인지 확인
    private float fadeDuration = 2f; // 페이드 인/아웃 시간
    private int currentPhase = 0; // 현재 phase
    
    [Header("Timer & Phase UI")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI phaseText;
    public GameObject TimeWheelGameObject;
    private Image timeWheel;
    
    [Header("Barrier UI")]
    public GameObject BarrierHpGameObject;
    public Barrier barrier;
    private Image BarrierHP;
    
    [Header("Bullet UI")]
    public GameObject BulletCountGameObject;
    public Gun gun;
    private TextMeshProUGUI BulletCount;
    
    [Header("Sensitivity Settings")]
    public Slider sensitivitySlider; // 감도 슬라이더
    public CameraMovement cameraMovement;
    
    [Header("Pause & Settings")]
    public GameObject gamePanel;
    public GameObject pausePanel;
    public GameObject settingsPanel;
    private InputAction pauseAction => InputManager.Instance.inputActions[InputType.Pause].action;
    public bool isPaused { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        BarrierHP = BarrierHpGameObject.GetComponent<Image>();
        BulletCount = BulletCountGameObject.GetComponent<TextMeshProUGUI>();
        timeWheel = TimeWheelGameObject.GetComponent<Image>();
        
        // Slider 초기화
        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = 1f;
            sensitivitySlider.maxValue = 25f;
            sensitivitySlider.value = 25f; // 이게 Default 값일듯?
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseAction.IsPressed() && !isPaused)
        {
            PauseGame();
        }

        ShowScriptText();
    }
    
    void LateUpdate()
    {
        UpdateUIText();
        UpdateTimeWheel();
    }

    public void UpdateTimeWheel()
    {
        float ratio = GameManager.Instance.ElapsedTime/ GameManager.Instance.gameTime;
        timeWheel.fillAmount = ratio;
    }

    void UpdateUIText()
    {
        BarrierHP.fillAmount = (float)barrier.health.HP / barrier.health.MaxHP;
        
        BulletCount.text = $"{gun.BulletCount}/{gun.MaxBulletCount}";

        if (GameManager.Instance.ElapsedTime < 60f)
        {
            phaseText.text = "Phase 1";
        }
        else if (GameManager.Instance.ElapsedTime < 120f)
        {
            phaseText.text = "Phase 2";
        }
        else
        {
            phaseText.text = "Phase 3";
        }

        int min = (int)(GameManager.Instance.RemainTime/ 60f);
        int sec = (int)(GameManager.Instance.RemainTime % 60);

        timeText.text = string.Format("{0:00}:{1:00}", min, sec);
    }

    void ShowScriptText()
    {
        float nowTime = GameManager.Instance.ElapsedTime;

        if (scriptTime < fadeDuration && !isFadingOut) // 페이드 인 효과
        {
            ScriptText.text = "";
        }
        else if (nowTime < 60f) // phase 1
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
        else if (nowTime < 120f) // phase 2
        {
            if (currentPhase != 2)
            {
                StartNewPhase(2);
            }
            if (nowTime < 65f)
            {
                ScriptText.text = "More enemies are closing in.";
            }
            else if (nowTime < 68f)
            {
                ScriptText.text = "What are these things? What are they really?";
            }
            else if (nowTime < 71f)
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
            if (nowTime < 125f)
            {
                ScriptText.text = "This is an onslaught on a completely different scale.";
            }
            else if (nowTime < 128f)
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

    public void MainMenu()
    {
        EnemyManager.Instance.DeleteAllEnemies();
        SpawnerManager.Instance.ResetList();
        pausePanel.SetActive(false);
        SceneManager.LoadScene("Scenes/TitleScene", LoadSceneMode.Single);
    }
    
    // Slider 값 조절 -> 호출됨
    private void OnSensitivityChanged(float newSensitivity)
    {
        if (cameraMovement != null)
        {
            cameraMovement.UpdateSensitivity(newSensitivity);
        }
    }
}
