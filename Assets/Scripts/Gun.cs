using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("General")]
    public float maxBullet; //최대 총알 개수
    public float fireDamp; //연사 딜레이
    public float reloadTime; //재장전 시간
    public GameObject bullet; //총알 프리팹
    public GameObject shotPoint; //발사 지점
    public InputActionReference inputAction; //입력

    [Header("Current State")]
    [SerializeField] private float currentBullet; //현재 총알 개수
    [SerializeField] private float currentDamp; //현재 딜레이
    [SerializeField] bool isReloading = false; //재장전 중

    private ParticleSystem fireParticle; //발사 파티클

    private TimeCounter reloadCounter;
    // Start is called before the first frame update
    void Start()
    {
        currentBullet = maxBullet;
        currentDamp = 0;
        reloadCounter = new TimeCounter();
        fireParticle = shotPoint.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDamp >= 0)
            currentDamp -= Time.deltaTime;
        reloadCounter.Update();

        if (reloadCounter.WasEndedThisFrame())
        {
            currentBullet = maxBullet;
            isReloading = false;
        }

        // 일단 InputManager에서 처리하게 만듦
        /*
        if (inputAction.action.IsPressed())
        {
            Debug.Log("Shoot is pressed.");
            Fire();
        }
        */

    }

    public void Fire()
    {
        if (currentDamp <= 0 && currentBullet > 0 && !isReloading) //총알 발사
        {
            currentDamp = fireDamp;
            currentBullet--;
            GameManager.instance.ShowBulletCount(currentBullet / maxBullet);

            Instantiate(bullet, shotPoint.transform.position, shotPoint.transform.rotation);

            fireParticle.Play();
        }
        else if (currentBullet <= 0 && !isReloading) //총알 없을 때 발사시 재장전
        {
            isReloading = true;
            reloadCounter.Start(reloadTime);
        }
    }

    public void OnReload()
    {
        isReloading = true;
        GameManager.instance.ShowBulletCount(currentBullet / maxBullet);
        reloadCounter.Start(reloadTime);
    }

}
