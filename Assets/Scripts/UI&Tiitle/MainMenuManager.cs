using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject titlePanel;
    public GameObject settingsPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGameButton()
    {
        SceneManager.LoadScene("Scenes/MainScene", LoadSceneMode.Single);
        //GameManager.instance.ResetGame();  // 씬 전환 후 게임 초기화
    }

    public void ShowSettings()
    {
        titlePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToTitle()
    {
        settingsPanel.SetActive(false);
        titlePanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }


}
