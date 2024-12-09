using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using inonego;

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
        health.OnHealDamageApplied += OnHealDamageApplied;
    }

    private void OnHealthStateChanged(Health sender, Health.StateChangedEventArgs e)
    {
        if (e.Current == Health.State.Dead)
        {
            GameManager.Instance.SetGameOver();
        }
    }

    public void OnHealDamageApplied(Health sender, Health.AppliedEventArgs e)
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
            
            health.ApplyDamage(enemy.damage);
            
            enemy.health.SetDead();
        }
    }
}
