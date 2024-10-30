using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Barrier : MonoBehaviour
{
    public float maxBarrierHp = 100f;
    [SerializeField] private float currentBarrierHp;

    public float damage; //몬스터 한마리당 소모되는 HP량

    public float hitAlertMinPitch = 0.8f;
    public float hitAlertMaxPitch = 1.2f;

    private new Animation animation;
    private AudioSource audioSource;

    private void Awake()
    {
        animation = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        currentBarrierHp = maxBarrierHp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            currentBarrierHp -= damage; 
            GameManager.instance.DecreaseBarrierHP();

            PlayHit();

            //대충 타 죽는 몬스터other.gameObject.GetComponent<Enemy>()
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            EnemyManager.instance.enemies.Remove(enemy);
            enemy.Destroy(); //죽는 모션 이후 삭제

            // 대충 배리어 파괴 직전 이펙트나 파티클 효과같은거 넣기
            if (currentBarrierHp <= 0)
            {
                GameManager.instance.IsGameOver = true;
            }
        }
        
    }

    public void PlayHit()
    {
        audioSource.pitch = Mathf.Lerp(hitAlertMinPitch, hitAlertMaxPitch, currentBarrierHp / maxBarrierHp);

        //대충 베리어 이펙트
        animation.Play();
        audioSource.Play();
    }
    
}
