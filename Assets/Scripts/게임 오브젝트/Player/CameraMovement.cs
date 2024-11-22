using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public bool isWorking = true;
    
    [SerializeField] private float currentSensitivity = 25f; // 초기 카메라 감도

    private Vector2 mouse = Vector2.zero;

    private void Update()
    {
        if (isWorking)
        {
            // 현재 감도 값 적용 (줌 상태에 따라 감도 변화 가능)
            //float sensitivity = zoomAim.IsZoomed ? currentSensitivity / 2 : currentSensitivity;

            mouse += Mouse.current.delta.ReadValue() * currentSensitivity * Time.deltaTime;
            mouse.y = Mathf.Clamp(mouse.y, -90f, 90f);

            transform.rotation = Quaternion.Euler(-mouse.y, mouse.x, 0);
        }
        else
        {
            Quaternion look = Quaternion.LookRotation(transform.forward, Vector3.up);

            mouse = new Vector2(look.eulerAngles.y, -look.eulerAngles.x);
        }
    }
    
    public void UpdateSensitivity(float newSensitivity)
    {
        currentSensitivity = newSensitivity;
    }

    public void SetWorking(bool value)
    {
        isWorking = value;
    }
}
