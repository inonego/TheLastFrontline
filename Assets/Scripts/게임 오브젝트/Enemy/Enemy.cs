using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public float speed; // 적의 이동 속도

    public int damage = 10; // 적의 공격력
    
    private Transform target; // 적이 추적할 목표

    public Health health           { get; private set; }  // 적의 생명체 관리
    public new Rigidbody rigidbody { get; private set; }  // 물리적 특성을 위한 Rigidbody
    private Collider[] colliders;

    private void Awake()
    {
        health = GetComponent<Health>(); // Health 컴포넌트 가져오기
        rigidbody = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
        colliders = GetComponentsInChildren<Collider>();
        
        health.OnStateChanged += OnHealthStateChanged;
    }

    private void OnEnable()
    {
        if(EnemyManager.Instance != null)
        {
            EnemyManager.Instance.enemies.Add(gameObject); // 적을 적 관리자의 목록에 추가
        }
    }

    private void OnDisable()
    {
        if(EnemyManager.Instance != null)
        {
            EnemyManager.Instance.enemies.Remove(gameObject); // 적이 파괴될 때 목록에서 제거
        }
    }

    private void Start()
    {
        target = GameObject.Find("Target Point").transform;

        // 추후 수정 예정
    }

    private void FixedUpdate()
    {
        Move(); // 매 프레임마다 이동 메서드 호출
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (health.IsDead) return;

        if (collision.gameObject.CompareTag("Bullet")) // 충돌한 객체가 총알인지 확인
        {
            health.TakeDamage(1); // 피해를 입음
        }
    }

    private void OnHealthStateChanged(Health.State state)
    {
        if (state == Health.State.Dead)
        {
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }
        }
    }

    private void Move()
    {
        if (target != null) // 목표가 존재하는지 확인
        {   
            Vector3 delta = target.position - transform.position; // 목표와의 거리 계산

            Vector3 direction = delta.normalized; // 방향 벡터 정규화

            rigidbody.velocity = direction * speed; // Rigidbody의 속도 설정
            rigidbody.rotation = Quaternion.LookRotation(direction, Vector3.up); // 적의 회전 설정
        }
    }
}
