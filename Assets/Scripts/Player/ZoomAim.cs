using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZoomAim : MonoBehaviour
{
    [Header("General")]
    public float zoomSpeed = 10f;

    public Transform zoomedPoint, unzoomedPoint;

    [Header("Camera")]
    public new CinemachineVirtualCamera camera;

    public float zoomedFocalLength;
    public float unzoomedFocalLength;

    private float currentFocalLength;

    private InputAction zoomAction => InputManager.Instance.inputActions[InputType.Zoom].action;

    public bool isZoomed => zoomAction.IsPressed();

    private void Awake()
    {
        currentFocalLength = unzoomedFocalLength;
    }

    private float FocalLengthToFOV(float focalLength)
    {
        if (focalLength < 0.001f) return 180f;

        return Mathf.Rad2Deg * 2.0f * Mathf.Atan(camera.m_Lens.SensorSize.y * 0.5f / focalLength);
    }

    private void Update()
    {
        Vector3 position = isZoomed ? zoomedPoint.localPosition : unzoomedPoint.localPosition;

        // ���� ��ġ�� �ε巴�� �����մϴ�.
        transform.localPosition = Vector3.Lerp(transform.localPosition, position, zoomSpeed * Time.deltaTime);

        // ���� Focal Length�� �ε巴�� �����մϴ�.
        currentFocalLength = Mathf.Lerp(currentFocalLength, isZoomed ? zoomedFocalLength : unzoomedFocalLength, zoomSpeed * Time.deltaTime);

        camera.m_Lens.FieldOfView = FocalLengthToFOV(currentFocalLength);
    }
}
