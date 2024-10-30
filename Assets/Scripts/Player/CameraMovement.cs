using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public bool isWorking = true;

    public float sensitivity = 360f;
    public float zoomedSensitivity = 180f;

    private Vector2 mouse = Vector2.zero;

    private ZoomAim zoomAim;

    private void Awake()
    {
        zoomAim = GetComponentInChildren<ZoomAim>();
    }

    private void Update()
    {
        if (isWorking)
        {
            float currentSensitivity = zoomAim.isZoomed ? sensitivity : sensitivity;

            mouse += Mouse.current.delta.ReadValue() * currentSensitivity * Time.smoothDeltaTime;

            mouse.y = Mathf.Clamp(mouse.y, -90f, 90f);

            transform.rotation = Quaternion.Euler(-mouse.y, mouse.x, 0);
        }
        else
        {
            Quaternion look = Quaternion.LookRotation(transform.forward, Vector3.up);

            mouse = new Vector2(look.eulerAngles.y, -look.eulerAngles.x);
        }
    }

    public void SetWorking(bool value)
    {
        isWorking = value;
    }
}
