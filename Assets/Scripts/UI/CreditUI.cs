using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CreditUI : MonoBehaviour
{
    public InputActionReference skipInputAction;
    public RectTransform rect;
    
    public GameObject thankUI;
    
    public float speed = 5f;
    public float skipAfterTime = 0f;
    public float thankTime = 0f;

    private void Update()
    {
        rect.position += Vector3.up * speed * Time.deltaTime;

        if (skipInputAction.action.WasPressedThisFrame())
        {
            Skip();
        }

        Invoke(nameof(Thank), skipAfterTime);
    }

    public void Thank()
    {
        thankUI.SetActive(true);
            
        Invoke(nameof(Skip), thankTime);
    }
    
    public void Skip()
    {
        SceneManager.LoadScene("Scenes/TitleScene");
    }
}
