using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDevicePanel : MonoBehaviour
{
    public float UIVisibleOffset = 0.2f;
    public float MoveLerpSpeed;
    public float LookLerpSpeed;

    public float UIPanelHeightOffset = 0.8f;
    public float UIPanelViewOffset = 0.8f;
    public GameObject UIDevice;
    public GameObject UIPanel;
    public Animator UIPanelAnimator;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    
    private void Update()
    {
        Vector3 UIPoint = (UIPanel.transform.position - mainCamera.transform.position).normalized * UIPanelViewOffset;

        UIPanel.transform.position = Vector3.Lerp(UIPanel.transform.position, UIDevice.transform.position + Vector3.up * UIPanelHeightOffset + UIPoint, MoveLerpSpeed * Time.deltaTime);

        Vector3 viewPoint = mainCamera.WorldToViewportPoint(UIDevice.transform.position);  
        if (viewPoint.x >= 0 - UIVisibleOffset && viewPoint.y >= 0 - UIVisibleOffset && viewPoint.x <= 1 + UIVisibleOffset && viewPoint.y <= 1 + UIVisibleOffset && viewPoint.z >= 0) 
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void LateUpdate()
    {
        if (mainCamera == null) return;

        UIPanel.transform.rotation = Quaternion.Slerp(UIPanel.transform.rotation, Quaternion.LookRotation(UIPanel.transform.position - mainCamera.transform.position, Vector3.up), LookLerpSpeed * Time.deltaTime);
    }

    public void Show()
    {
        UIPanelAnimator.SetBool("IsVisible", true);
    }

    public void Hide()
    {
        UIPanelAnimator.SetBool("IsVisible", false);
    }
}
