using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int index; // 몬스터 종류 구분 인덱스
    // 임시로 해파리는 1로 하겠음
    public float speed;
    public int hp = 4;
    
    public Randomizer speedRandomizer;
    public Randomizer hpRandomizer;

    public int damage = 10;

    private Transform target; // 적이 달려오는 타겟
    private Rigidbody rigid;

    public GameObject effectOnDead;
    
    // dissolve관련 변수
    public Material dissolveMaterial;
    private MeshRenderer mesh;
    public float dissolveSpeed = 1.5f; // Dissolve 속도
    private float dissolveAmount = 0f; // 현재 Dissolve 상태
    private bool isDissolving = false; // Dissolve 시작 여부
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
        EnemyManager.Instance.enemies.Add(this);

        speed = speed * speedRandomizer.value;
        hp = (int)(hp * hpRandomizer.value);
    }

    private void Start()
    {
        target = GameObject.Find("Target Point").transform;
    }

    private void Update()
    {
        if (isDissolving)
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount + Time.deltaTime * dissolveSpeed);
            
            mesh.material.SetFloat("_DissolveAmount", dissolveAmount);  // 셰이더 값 업데이트

            // Dissolve가 완료되면 오브젝트 제거
            if (dissolveAmount >= 1f)
            {
                Destroy(transform.root.gameObject);  // 몬스터 제거 (부모까지 다,,)
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (target != null)
        {
            Vector3 delta = target.position - transform.position;

            Vector3 direction = delta.normalized;

            rigid.velocity = direction * speed;
            rigid.rotation = Quaternion.LookRotation(direction, Vector3.up);
            
            if (index == 1)
            {
                rigid.rotation *= Quaternion.Euler(60, -20, -60);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp -= 1;

            if (hp <= 0)
            {
                Destroy();
            }
        }
    }

    public void Destroy()
    {
        EnemyManager.Instance.enemies.Remove(this);

        if (index == 1)
        {
            dissolve();
        }
        else
        {
            Instantiate(effectOnDead, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void dissolve()
    {
        mesh.material = dissolveMaterial;
        isDissolving = true;
    }
}
