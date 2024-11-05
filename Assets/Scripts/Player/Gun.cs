using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ProceduralRecoil))]
public class Gun : MonoBehaviour
{
    [Header("General")]
    public float maxBullet;                     // 최대 총알 개수
    public float delayTime;                     // 연사 딜레이
    public float reloadTime;                    // 재장전 시간
    public GameObject bullet;                   // 총알 프리팹
    public Transform shootPoint;                // 발사 지점

    public AudioClip fireSound;
    public AudioClip reloadSound;

    public List<Material> muzzleFlashMaterialList;

    [Header("Current State")]
    [SerializeField] private float currentBullet;   // 현재 총알 개수
    [SerializeField] bool isDelaying = false;       // 딜레이 적용 중
    [SerializeField] bool isReloading = false;      // 재장전 중

    public Action OnFire = null;
    public Action OnReload = null;
    private PlayModeUI gunUI;

    private Pool pool;
    private ParticleSystem muzzleFlashParticle;         // 발사 파티클
    private ParticleSystemRenderer muzzleFlashRenderer; //
    private AudioSource audioSource;

    private InputAction fireInputAction => InputManager.Instance.inputActions[InputType.Fire].action;
    private InputAction reloadInputAction => InputManager.Instance.inputActions[InputType.Reload].action;

    private void Awake()
    {
        gunUI = FindAnyObjectByType<PlayModeUI>();
        pool = GetComponent<Pool>();
        muzzleFlashParticle = shootPoint.GetComponent<ParticleSystem>();
        muzzleFlashRenderer = shootPoint.GetComponent<ParticleSystemRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        currentBullet = maxBullet;
    }

    private void Update()
    {
        if (gunUI.isPaused) return;
        gunUI.ShowBulletCount(currentBullet, maxBullet);

        if (fireInputAction.IsPressed())
        {
            // 현재 연사 딜레이 적용 중이거나 재장전 중이 아니라면 
            if (!(isDelaying || isReloading))
            {
                if (currentBullet > 0)
                {
                    Fire();
                }
                else
                {
                    // 총알이 부족한 경우 재장전
                    Reload();
                }
            }
        }

        if (!isReloading && reloadInputAction.WasPressedThisFrame())
        {
            // 총알이 부족한 경우 재장전
            Reload();
        }
    }

    private IEnumerator DoDelay()
    {
        isDelaying = true;

        yield return new WaitForSeconds(delayTime);

        isDelaying = false;
    }

    private IEnumerator DoReload()
    {
        isReloading = true;

        float playSoundTime = Mathf.Max(reloadTime - reloadSound.length, 0f);

        yield return new WaitForSeconds(playSoundTime);

        audioSource.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(reloadTime - playSoundTime);

        currentBullet = maxBullet;

        if (OnReload != null) OnReload.Invoke();

        isReloading = false;
    }

    public void Fire()
    {
        currentBullet--;

        GameObject GO = pool.packList[0].Spawn(shootPoint.position, shootPoint.rotation);

        muzzleFlashRenderer.material = muzzleFlashMaterialList[UnityEngine.Random.Range(0, muzzleFlashMaterialList.Count)];

        muzzleFlashParticle.Emit(1);
        audioSource.PlayOneShot(fireSound);

        if (OnFire != null) OnFire.Invoke();

        StartCoroutine(DoDelay());
    }

    public void Reload()
    {
        StartCoroutine(DoReload());
    }
}
