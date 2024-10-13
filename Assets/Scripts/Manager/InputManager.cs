using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputType
{
    Fire, Pause
}

public class InputManager : Singleton<InputManager>
{
    public SerializedDictionary<InputType, InputActionReference> inputActions = new SerializedDictionary<InputType, InputActionReference>();

    // Update is called once per frame
    void Update()
    {
        //if (pauseAction.action.IsPressed())
        //{
        //    Debug.Log("ESC is pressed.");
        //    GameManager.instance.PauseGame();
        //}
        
    }
}
