using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCommunity.UnitySingleton;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class RestartManager : MonoSingleton<RestartManager>
{
    public InputActionReference RestartAction;
    public InputActionReference QuitAction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Update()
    {
        if (RestartAction.action.WasPressedThisFrame())
        {
            //게임 재시작
            SceneManager.LoadScene("MainScene");
        }
        
        if (QuitAction.action.WasPressedThisFrame())
        {
            Application.Quit();
        }
    }
}
