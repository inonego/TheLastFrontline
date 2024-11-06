using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class Boss : MonoBehaviour
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
    
    private Collider[] colliders;
    
    // dissolve관련 변수
    private MeshRenderer[] mesh;
    private SkinnedMeshRenderer[] mesh2;
    public float dissolveSpeed = 1.5f; // Dissolve 속도
    private float dissolveAmount = 1f; // 현재 Dissolve 상태
    private bool isStartDis = true;
    private bool isDissolving = false; // Dissolve 시작 여부
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        mesh = GetComponentsInChildren<MeshRenderer>();
        mesh2 = GetComponentsInChildren<SkinnedMeshRenderer>();
        colliders = GetComponentsInChildren<Collider>();
        EnemyManager.Instance.enemies.Add(gameObject);

        speed = speed * speedRandomizer.value;
        hp = (int)(hp * hpRandomizer.value);
    }

    private void Start()
    {
        if (hp > 100)//boss
        {
            target = GameObject.Find("Target Point2").transform;
        }
        else
        {
            target = GameObject.Find("Target Point").transform;
        }
    }

    private void Update()
    {
        if (isStartDis)
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount - Time.deltaTime * dissolveSpeed);

            foreach (MeshRenderer mesh in mesh)
            {
                mesh.material.SetFloat("_DissolveAmount", dissolveAmount);  // 셰이더 값 업데이트
            }
            foreach (SkinnedMeshRenderer mesh in mesh2)
            {
                mesh.material.SetFloat("_DissolveAmount", dissolveAmount);  // 셰이더 값 업데이트
            }
            // Dissolve가 완료되면 오브젝트 제거
            if (dissolveAmount <= 0f)
            {
                isStartDis = false;
            }
        }
        
        if (isDissolving)
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount + Time.deltaTime * dissolveSpeed);

            foreach (MeshRenderer mesh in mesh)
            {
                mesh.material.SetFloat("_DissolveAmount", dissolveAmount);  // 셰이더 값 업데이트
            }
            foreach (SkinnedMeshRenderer mesh in mesh2)
            {
                mesh.material.SetFloat("_DissolveAmount", dissolveAmount);  // 셰이더 값 업데이트
            }
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
        EnemyManager.Instance.enemies.Remove(gameObject);
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
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
        isDissolving = true;
    }
}
