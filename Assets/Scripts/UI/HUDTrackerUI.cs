using System.Collections;
using System.Collections.Generic;
using inonego;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class HUDTrackerUI : MonoBehaviour
{
    public AudioClip GetDamagedSound;

    public TextMeshProUGUI DistanceUI;
    public TextMeshProUGUI HealthUI;

    public UILineRenderer LineRenderer;

    public RectTransform UIParent;
    private Animator animator;
    private AudioSource audioSource;

    public float LineLengthStartOffset;
    public float LineLengthEndOffset;

    public Vector2 EndPoint { get; private set; }

    public float LerpSpeed;

    public HUDTracker.TrackInfo? TrackInfo { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        if (TrackInfo == null) return;

        transform.localPosition = TrackInfo.Value.ScreenPosition;

        DistanceUI.text = $"{Mathf.Round(TrackInfo.Value.WorldSpaceDistance)}M";
        HealthUI.text = $"{TrackInfo.Value.Enemy.health.HP}";
    }

    private void OnEnable()
    {
        animator.Rebind();
        animator.Update(0f);
    }

    private void OnDisable()
    {
        SetTrackInfo(null);
    }

    public void SetTrackInfo(HUDTracker.TrackInfo? trackInfo)
    {
        if (TrackInfo != null)
        {
            TrackInfo.Value.Enemy.health.OnHealDamageApplied -= OnEnemyDamaged;
        }

        TrackInfo = trackInfo;

        if (TrackInfo != null)
        {
            TrackInfo.Value.Enemy.health.OnHealDamageApplied += OnEnemyDamaged;
        }
    }

    private void OnEnemyDamaged(Health sender, Health.AppliedEventArgs e)
    {
        animator.SetTrigger("GetDamaged");
        audioSource.PlayOneShot(GetDamagedSound);
    }

    public void UpdateLine(Vector2 endPoint)
    {
        endPoint -= TrackInfo.Value.ScreenPosition;

        EndPoint = Vector3.Slerp(EndPoint, endPoint, LerpSpeed * Time.unscaledDeltaTime);

        UIParent.localPosition = EndPoint;

        float lineStartLength = Mathf.Max(LineLengthStartOffset, 0f);
        float lineEndLength = Mathf.Max(EndPoint.magnitude + LineLengthEndOffset, 0f);

        LineRenderer.Points[0] = new Vector2(0.5f, 0f) + EndPoint.normalized * lineStartLength;
        LineRenderer.Points[1] = new Vector2(0.5f, 0f) + EndPoint.normalized * lineEndLength;
    }
}
