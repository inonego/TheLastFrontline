using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private bool isActive;
    
    public Action onDamage;
    public Action onDeath;
    
    public int maxBarrierHp = 100;
    public int currentBarrierHp;

    [SerializeField] private float hitAlertMinPitch = 0.8f;
    [SerializeField] private float hitAlertMaxPitch = 1.2f;

    private new Animation animation;
    private AudioSource audioSource;

    private void Awake()
    {
        animation = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        isActive = true;
        
        currentBarrierHp = maxBarrierHp;

        onDeath += () => GameManager.Instance.SetGameOver();
    }

    public void TakeDamage(int damage)
    {   
        PlayHit();
        
        currentBarrierHp = Mathf.Max(currentBarrierHp - damage, 0);
        onDamage?.Invoke();
        
        if (currentBarrierHp == 0)
        {
            isActive = false;
            
            currentBarrierHp = -1;
            
            onDeath?.Invoke();
        }
    }
    
    private void PlayHit()
    {
        audioSource.pitch = Mathf.Lerp(hitAlertMinPitch, hitAlertMaxPitch, (float)currentBarrierHp / maxBarrierHp);

        animation.Play();
        audioSource.Play();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            
            TakeDamage(enemy.damage);
            
            enemy.Destroy();
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            Boss boss = other.gameObject.GetComponentInParent<Boss>();
            
            TakeDamage(boss.damage);
            
            boss.Destroy();
        }
    }
}
