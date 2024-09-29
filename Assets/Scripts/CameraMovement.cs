using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 360f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private Vector2 mouse = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        mouse += Mouse.current.delta.ReadValue() * sensitivity * Time.smoothDeltaTime;

        mouse.y = Mathf.Clamp(mouse.y, -90f, 90f);

        transform.rotation = Quaternion.Euler(-mouse.y, mouse.x, 0);

    }
}
