using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRModeUI : MonoBehaviour
{
    public Transform Anchor;
    public Transform Looker;

    [Header("General")]
    public bool ShowOnStart = false;

    public bool UseUnscaledTime = true;

    [Header("Move & Look")]
    public Vector3 LocalOffset;
    public Vector3 WorldOffset;
    public Vector3 LookerOffset;

    public float MoveThreshold = 0.1f;
    public float MoveLerpSpeed = 5f;
    public float LookLerpSpeed = 5f;
    
    [Header("Sound")]
    public AudioClip ShowSound;
    public AudioClip HideSound;

    public bool IsVisible { get; private set; } = false;

    private float deltaTime => UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

    private AudioSource audioSource;
    private Animator animator;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (ShowOnStart)
        {
            Show();
        }
    }

    private void Update()
    {
        Quaternion lookerRotation = Quaternion.LookRotation(Anchor.position - Looker.position, Vector3.up);

        // 앵커를 중심으로 오프셋을 조정해서 목표로 하는 위치 계산
        Vector3 targetP = Anchor.position + Anchor.rotation * LocalOffset + lookerRotation * LookerOffset + WorldOffset;

        Vector3 delta = targetP - transform.position;
        Vector3 nextP = transform.position + delta.normalized * Mathf.Max(delta.magnitude - MoveThreshold, 0f);

        Vector3 finalP = Vector3.Lerp(transform.position, nextP, MoveLerpSpeed * deltaTime);
        Quaternion finalR = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - Looker.transform.position, Vector3.up), LookLerpSpeed * deltaTime);

        transform.position = finalP;
        transform.rotation = finalR;
    }

    public void Show()
    {
        animator.SetBool("IsVisible", true);
        audioSource.PlayOneShot(ShowSound);

        IsVisible = true;
    }

    public void Hide()
    {
        animator.SetBool("IsVisible", false);
        audioSource.PlayOneShot(HideSound);

        IsVisible = false;
    }
}
