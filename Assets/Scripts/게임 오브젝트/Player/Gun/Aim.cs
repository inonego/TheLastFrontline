using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Aim : MonoBehaviour
{
    [Serializable]
    public class View
    {
        public Transform Point;
        public float FocalLength;
    }

    public float SwapSpeed = 10f;

    [SerializeField]
    private List<View> views = new List<View>();
    public IReadOnlyList<View> Views => views;

    public View CurrentView { get; private set; }

    public Vector3 Position { get; private set; } = Vector3.zero;
    public Quaternion Rotation { get; private set; } = Quaternion.identity;

    private float FocalLength;

    private void Awake()
    {
        if (Views[0] != null)
        {
            CurrentView = Views[0];
            FocalLength = Views[0].FocalLength;
        }
    }

    private void Update()
    {
        if (CurrentView == null) return;

        Vector3 position    = CurrentView.Point.localPosition;
        Quaternion rotation = CurrentView.Point.localRotation;

        Position = Vector3.Lerp(Position, position, SwapSpeed * Time.deltaTime);
        Rotation = Quaternion.Slerp(Rotation, rotation, SwapSpeed * Time.deltaTime);

        FocalLength = Mathf.Lerp(FocalLength, CurrentView.FocalLength, SwapSpeed * Time.deltaTime);
    }

    public void SetView(View view)
    {
        this.CurrentView = view;
    }

    public float GetFieldOfView(CinemachineVirtualCamera camera)
    {   
        if (FocalLength < 0.001f) return 180f;

        return Mathf.Rad2Deg * 2.0f * Mathf.Atan(camera.m_Lens.SensorSize.y * 0.5f / FocalLength);
    }
}
