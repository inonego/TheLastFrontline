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
    public GameObject UIDevice;
    public GameObject UIDevicePanel;
    public GameObject UIPausePanel;
    
    [Header("Input")]
    public InputActionReference PauseAction;
    public InputActionReference SelectAction;

    [Header("Speed")]
    public float MoveLerpSpeed;
    public float LookLerpSpeed;
    public float PauseMenuMoveLerpSpeed;
    public float PauseMenuLookLerpSpeed;

    [Header("Offset")]
    public float UIVisibleOffset = 0.2f;
    public float UIPanelHeightOffset = 0.8f;
    public float UIPanelViewOffset = 0.8f;
    public float UIPausePanelOffset = 0.8f;
    public float UIPausePanelMoveThreshold = 0.1f;
    

    [Header("Sound")]
    public AudioClip ShowSound;
    public AudioClip HideSound;

    private Camera mainCamera;

    private bool isVisible = false;

    private AudioSource audioSource;

    private Animator UIPanelAnimator;
    private Animator UIPausePanelAnimator;

    public bool IsPaused => GameManager.Instance.State == GameState.Paused;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        UIPanelAnimator = UIDevicePanel.GetComponent<Animator>();
        UIPausePanelAnimator = UIPausePanel.GetComponent<Animator>();
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
                HidePauseMenu();
            }
        }

        Vector3 UIPoint = (UIDevicePanel.transform.position - mainCamera.transform.position).normalized * UIPanelViewOffset;

        // UIDevicePanel에 대해 이동 및 회전
        Vector3 UIDevicePanelP = Vector3.Lerp(UIDevicePanel.transform.position, UIDevice.transform.position + Vector3.up * UIPanelHeightOffset + UIPoint, MoveLerpSpeed * Time.unscaledDeltaTime);
        Quaternion UIDevicePanelR = Quaternion.Slerp(UIDevicePanel.transform.rotation, Quaternion.LookRotation(UIDevicePanel.transform.position - mainCamera.transform.position, Vector3.up), LookLerpSpeed * Time.unscaledDeltaTime);

        UIDevicePanel.transform.position = UIDevicePanelP;
        UIDevicePanel.transform.rotation = UIDevicePanelR;

        // UIPausePanel에 대해 이동 및 회전
        Vector3 UIPausePanelTargetP = mainCamera.transform.position + mainCamera.transform.forward * UIPausePanelOffset;
       
        Quaternion UIPausePanelR = Quaternion.Slerp(UIPausePanel.transform.rotation, Quaternion.LookRotation(UIPausePanel.transform.position - mainCamera.transform.position, Vector3.up), PauseMenuLookLerpSpeed * Time.unscaledDeltaTime);

        // UIPausePanel의 위치가 카메라와의 거리가 일정 이상 멀어지면 이동
        if (Vector3.Distance(UIPausePanel.transform.position, UIPausePanelTargetP) > UIPausePanelMoveThreshold)
        { 
            Vector3 UIPausePanelP = Vector3.Lerp(UIPausePanel.transform.position, UIPausePanelTargetP, PauseMenuMoveLerpSpeed * Time.unscaledDeltaTime);

            UIPausePanel.transform.position = UIPausePanelP;
        }

        UIPausePanel.transform.rotation = UIPausePanelR;

        if (IsPaused)
        {
            if (!isVisible) return;

            HideUIDevicePanel();
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
        
        UIPausePanelAnimator.SetBool("IsVisible", true);
        audioSource.PlayOneShot(ShowSound);
    }

    // 게임을 재시작하고 일시정지 메뉴를 숨깁니다.
    public void HidePauseMenu()
    {
        GameManager.Instance.ResumeGame();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        UIPausePanelAnimator.SetBool("IsVisible", false);
        audioSource.PlayOneShot(HideSound);
    }

    // UIDevicePanel을 표시합니다.
    public void ShowUIDevicePanel()
    {
        UIPanelAnimator.SetBool("IsVisible", true);
        audioSource.PlayOneShot(ShowSound);

        isVisible = true;
    }

    // UIDevicePanel을 숨깁니다.
    public void HideUIDevicePanel()
    {
        UIPanelAnimator.SetBool("IsVisible", false);
        audioSource.PlayOneShot(HideSound);

        isVisible = false;
    }
    
    private void CheckVisible()
    {
        Vector3 viewPoint = mainCamera.WorldToViewportPoint(UIDevice.transform.position);  
        
        if (viewPoint.x >= 0 - UIVisibleOffset && viewPoint.y >= 0 - UIVisibleOffset && viewPoint.x <= 1 + UIVisibleOffset && viewPoint.y <= 1 + UIVisibleOffset && viewPoint.z >= 0) 
        {
            if (isVisible) return;

            ShowUIDevicePanel();
        }
        else
        {
            if (!isVisible) return;

            HideUIDevicePanel();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
#endregion

}
