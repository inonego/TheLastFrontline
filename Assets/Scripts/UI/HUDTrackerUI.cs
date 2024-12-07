using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class HUDTrackerUI : MonoBehaviour
{
    public TextMeshProUGUI DistanceUI;
    public TextMeshProUGUI HealthUI;

    public UILineRenderer LineRenderer;

    public RectTransform UIParent;

    public float LineLengthStartOffset;
    public float LineLengthEndOffset;

    public Vector2 EndPoint { get; private set; }

    public float LerpSpeed;

    public HUDTracker.TrackInfo? TrackInfo { get; private set; }

    private void LateUpdate()
    {
        if (TrackInfo == null) return;

        transform.localPosition = TrackInfo.Value.ScreenPosition;

        DistanceUI.text = $"{Mathf.Round(TrackInfo.Value.WorldSpaceDistance)}M";
        HealthUI.text = $"{TrackInfo.Value.Enemy.health.HP}";
    }

    public void SetTrackInfo(HUDTracker.TrackInfo trackInfo)
    {
        TrackInfo = trackInfo;
    }

    public void UpdateLine(Vector2 endPoint)
    {
        endPoint -= TrackInfo.Value.ScreenPosition;

        EndPoint = Vector3.Slerp(EndPoint, endPoint, LerpSpeed * Time.deltaTime);

        UIParent.localPosition = EndPoint;

        float lineStartLength = Mathf.Max(LineLengthStartOffset, 0f);
        float lineEndLength = Mathf.Max(EndPoint.magnitude + LineLengthEndOffset, 0f);

        LineRenderer.Points[0] = new Vector2(0.5f, 0f) + EndPoint.normalized * lineStartLength;
        LineRenderer.Points[1] = new Vector2(0.5f, 0f) + EndPoint.normalized * lineEndLength;
    }
}
