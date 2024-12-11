using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{    
    [Header("General")]
    public GameObject Bullet;                   // 총알 프리팹
    public Transform ShootPoint;                // 발사 지점
    public float BulletCount;                   // 현재 총알 개수
    public float MaxBulletCount;                // 최대 총알 개수

    [Header("Time")]
    public float DelayTime;                     // 연사 딜레이
    public float ReloadTime;                    // 재장전 시간
    
    [Header("Effect")]
    public AudioClip FireSound;
    public AudioClip ReloadSound;

    public List<Material> MuzzleFlashMaterialList;

    public bool IsTriggerPressed { get; private set; } = false;

    public bool isDelaying      { get; private set; } = false;       // 딜레이 적용 중
    public bool isReloading     { get; private set; } = false;      // 재장전 중

    public delegate void OnFireEvent();
    public delegate void OnReloadEvent();

    public event OnFireEvent OnFired;
    public event OnReloadEvent OnReloaded;

    private Pool pool;
    private ParticleSystem muzzleFlashParticle;
    private ParticleSystemRenderer muzzleFlashRenderer;
    private AudioSource audioSource;
    private Animator animator;
    private void Awake()
    {
        pool = GetComponent<Pool>();
        muzzleFlashParticle = ShootPoint.GetComponent<ParticleSystem>();
        muzzleFlashRenderer = ShootPoint.GetComponent<ParticleSystemRenderer>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        BulletCount = MaxBulletCount;
    }

    private void Update()
    {
        if (IsTriggerPressed)
        {
            // 현재 연사 딜레이 적용 중이거나 재장전 중이 아니라면 
            if (!isDelaying && !isReloading)
            {
                Fire();
            }
        }
    }

    public void SetTriggerPressed(bool value)
    {
        IsTriggerPressed = value;
    }

    public void Reload()
    {
        if(!isReloading)
        {
            StartCoroutine(DoReload());
        }
    }

    private IEnumerator DoReload()
    {
        isReloading = true; // 재장전 시작
        
        animator.SetTrigger("Reloading");
            
        float playSoundTime = Mathf.Max(ReloadTime - ReloadSound.length, 0f); // 소리 재생 시간 계산
        
        yield return new WaitForSeconds(playSoundTime); // 소리 재생 대기
        
        audioSource.PlayOneShot(ReloadSound); // 재장전 소리 재생
        
        yield return new WaitForSeconds(ReloadTime - playSoundTime); // 재장전 시간 대기

        BulletCount = MaxBulletCount; // 총알 수 최대치로 재설정

        OnReloaded?.Invoke(); // 재장전 이벤트 호출

        isReloading = false; // 재장전 완료
    }

    private void Fire()
    { 
        if (BulletCount <= 0) return;

        BulletCount--;

        GameObject GO = pool.packList[0].Spawn(ShootPoint.position, ShootPoint.rotation);

        muzzleFlashRenderer.material = MuzzleFlashMaterialList[UnityEngine.Random.Range(0, MuzzleFlashMaterialList.Count)];

        muzzleFlashParticle.Emit(1);
        audioSource.PlayOneShot(FireSound);

        OnFired?.Invoke();

        StartCoroutine(DoDelay());
    }

    private IEnumerator DoDelay()
    {
        isDelaying = true;

        yield return new WaitForSeconds(DelayTime);

        isDelaying = false;
    }
}
