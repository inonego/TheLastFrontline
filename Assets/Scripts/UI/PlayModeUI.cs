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
    
    [Header("Sensitivity Settings")]
    public Slider SensitivitySlider;
    public CameraMovement CameraMovement;
    
    [Header("Pause & Settings")]
    public GameObject GamePanel;
    public GameObject PausePanel;
    public GameObject SettingsPanel;

    private InputAction pauseAction => InputManager_InGame.Instance.inputActions[InputManager_InGame.InputType.Pause].action;

    public bool IsPaused => GameManager.Instance.State == GameState.Paused;

    void Start()
    {
        if (SensitivitySlider != null)
        {
            SensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        }

        ScriptPlayer.OnScriptTextChanged += OnScriptTextChanged;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateUIText();
        UpdateTimeWheel();
        
        if (pauseAction.IsPressed() && !IsPaused)
        {
            ShowPauseMenu();
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

    public void ShowPauseMenu()
    {
        GameManager.Instance.PauseGame();
        
        PausePanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HidePauseMenu()
    {
        GameManager.Instance.ResumeGame();

        PausePanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowSettings()
    {
        PausePanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void BackToPausePanel()
    {
        PausePanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }

    public void MainMenu()
    {
        PausePanel.SetActive(false);

        GameManager.Instance.GoToScene("Scenes/TitleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void OnSensitivityChanged(float sensitivity)
    {
        if (CameraMovement != null)
        {
            CameraMovement.UpdateSensitivity(sensitivity);
        }
    }
    
#endregion

}
