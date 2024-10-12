using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public GameObject gunObject;
    private Gun gun;
    public InputActionReference fireAction; // 총알 발사
    public InputActionReference pauseAction; // esc 누름

    private void Awake()
    {
        gun = gunObject.GetComponent<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isPaused && fireAction.action.IsPressed())
        {
            Debug.Log("Shoot is pressed.");
            gun.Fire();
        }
        
        if (pauseAction.action.IsPressed())
        {
            Debug.Log("ESC is pressed.");
            GameManager.instance.PauseGame();
        }
        
    }
}
