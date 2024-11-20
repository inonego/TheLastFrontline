using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] private float hitAlertMinPitch = 0.8f;
    [SerializeField] private float hitAlertMaxPitch = 1.2f;

    private new Animation animation;
    private AudioSource audioSource;
    public Health health {get; private set; }

    private void Awake()
    {
        animation = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
        health = GetComponent<Health>();

        health.OnStateChanged += OnHealthStateChanged;
        health.OnDamaged += OnDamaged;
    }

    private void OnHealthStateChanged(Health.StateChangedEventArgs e)
    {
        if (e.Current == Health.State.Dead)
        {
            GameManager.Instance.SetGameOver();
        }
    }

    public void OnDamaged(Health.DamagedEventArgs e)
    {   
        PlayHit();
    }
    
    private void PlayHit()
    {
        audioSource.pitch = Mathf.Lerp(hitAlertMinPitch, hitAlertMaxPitch, (float)health.HP / health.MaxHP);

        animation.Play();
        audioSource.Play();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (health.IsDead) return;
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();
            
            health.TakeDamage(enemy.damage);
            
            enemy.health.SetDead();
        }
    }
}
