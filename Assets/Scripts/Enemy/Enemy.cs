using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;

    public int hp = 4;

    public float damage = 10f;

    private Transform target; // 적이 달려오는 타겟
    private Rigidbody rigid;

    public GameObject effectOnDead;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        EnemyManager.instance.enemies.Add(this);
    }

    private void Start()
    {
        target = GameObject.Find("Target Point").transform;
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
        Instantiate(effectOnDead, transform.position, Quaternion.identity);

        EnemyManager.instance.enemies.Remove(this);

        Destroy(gameObject);
    }
}
