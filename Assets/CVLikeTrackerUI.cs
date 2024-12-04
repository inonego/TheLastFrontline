using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class CVLikeTrackerUI : MonoBehaviour
{
    public TextMeshProUGUI DistanceUI;
    public TextMeshProUGUI HealthUI;

    public UILineRenderer LineRenderer;

    public RectTransform UIParent;

    public Vector2 RandomRange;
    public float LineLengthOffset;
    private float LineLength;
    private Vector2 LineDirection;

    public float SlerpSpeed;

    public CVLikeTracker.TrackInfo? trackInfo { get; private set; }

    private void Update()
    {
        if (trackInfo == null) return;

        transform.position = trackInfo.Value.ScreenPosition;

        DistanceUI.text = $"{Mathf.Round(trackInfo.Value.WorldSpaceDistance)}M";
        HealthUI.text = $"{trackInfo.Value.Enemy.health.HP}";
    }
    
    public void Reset()
    {
        LineLength = Random.Range(RandomRange.x, RandomRange.y);
    }

    public void SetTrackInfo(CVLikeTracker.TrackInfo trackInfo)
    {
        this.trackInfo = trackInfo;
    }

    public void UpdateLine(Vector2 direction)
    {
        LineDirection = Vector3.Slerp(LineDirection, direction, SlerpSpeed * Time.deltaTime);

        UIParent.localPosition = LineDirection * LineLength;

        float lineLength = Mathf.Max(LineLength + LineLengthOffset, 0f);

        LineRenderer.Points[0] = new Vector2(0.5f, 0f);
        LineRenderer.Points[1] = new Vector2(0.5f, 0f) + LineDirection * lineLength;
    }
}
