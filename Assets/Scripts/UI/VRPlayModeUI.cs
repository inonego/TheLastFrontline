using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class VRPlayModeUI : MonoBehaviour
{
    // UI용 변수들
    [Header("Script UI")]
    public ScriptPlayer ScriptPlayer;
    public TextMeshProUGUI ScriptText;
    
    [Header("Timer & Phase UI")]
    public TextMeshProUGUI TimeUI;
    public TextMeshProUGUI PhaseUI;
    public Image TimeWheelUI;
    
    [Header("Barrier UI")]
    public Barrier Barrier;
    public Image BarrierUI;
    
    [Header("Bullet UI")]
    public Gun Gun;
    public TextMeshProUGUI BulletCountUI;
    
    [Header("Panel")]
    public VRModeUI DeviceUI;
    public VRModeUI PauseUI;
    
    [Header("Device UI")]
    public float DeviceUIVisibleOffset = 0.1f;

    [Header("Pause UI")]
    public List<Button> PauseMenuButtons;
    public int PauseMenuIndex = 0;

    [Header("Input")]
    public InputActionReference PauseAction;
    public InputActionReference SelectAction;

    
    [Header("Sound")]
    public AudioClip SelectSound;

    private Camera mainCamera;

    private AudioSource audioSource;

    public bool IsPaused => GameManager.Instance.State == GameState.Paused;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        
        ScriptPlayer.OnScriptTextChanged += OnScriptTextChanged;
    }

    private void Update()
    {
        UpdateUIText();
        UpdateTimeWheel();
        
        if (PauseAction.action.WasPressedThisFrame())
        {
            if (!IsPaused)
            {
                ShowPauseMenu();
            }
            else
            {
                PauseMenuButtons[PauseMenuIndex].onClick.Invoke();
            }
        }

        if (IsPaused)
        {            
            if (SelectAction.action.WasPressedThisFrame())
            {
                float selectValue = SelectAction.action.ReadValue<float>();

                PauseMenuIndex = (PauseMenuIndex + (selectValue > 0f ? 1 : -1) + PauseMenuButtons.Count) % PauseMenuButtons.Count;

                audioSource.PlayOneShot(SelectSound);
            }

            PauseMenuButtons[PauseMenuIndex].Select();

            if (!DeviceUI.IsVisible) return;

            DeviceUI.Hide();
        }
        else
        {
            CheckVisible();
        }
    }
    
    public void OnScriptTextChanged(string text)
    {
        ScriptText.text = text;
    }

    public void UpdateTimeWheel()
    {
        TimeWheelUI.fillAmount = GameManager.Instance.ElapsedTime/ GameManager.Instance.GameTime;
    }

    void UpdateUIText()
    {
        BarrierUI.fillAmount = (float)Barrier.health.HP / Barrier.health.MaxHP;
        
        BulletCountUI.text = $"{Gun.BulletCount}/{Gun.MaxBulletCount}";

        PhaseUI.text = $"Phase { GameManager.Instance.CurrentPhase + 1 }";

        int min = (int)(GameManager.Instance.RemainTime / 60f);
        int sec = (int)(GameManager.Instance.RemainTime % 60);

        TimeUI.text = string.Format("{0:00}:{1:00}", min, sec);
    }

#region Pause Menu

    // 게임을 일시정지하고 일시정지 메뉴를 표시합니다.
    public void ShowPauseMenu()
    {
        GameManager.Instance.PauseGame();
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
            
        PauseUI.Show();

        PauseMenuIndex = 0;
    }

    // 게임을 재시작하고 일시정지 메뉴를 숨깁니다.
    public void HidePauseMenu()
    {
        GameManager.Instance.ResumeGame();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        PauseUI.Hide();

        PauseMenuIndex = 0;
    }

    private void CheckVisible()
    {
        Vector3 viewPoint = mainCamera.WorldToViewportPoint(DeviceUI.transform.position);  
        
        if (viewPoint.x >= 0 - DeviceUIVisibleOffset && viewPoint.y >= 0 - DeviceUIVisibleOffset && viewPoint.x <= 1 + DeviceUIVisibleOffset && viewPoint.y <= 1 + DeviceUIVisibleOffset && viewPoint.z >= 0) 
        {
            if (DeviceUI.IsVisible) return;

            DeviceUI.Show();
        }
        else
        {
            if (!DeviceUI.IsVisible) return;

            DeviceUI.Hide();
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
#endregion

}
