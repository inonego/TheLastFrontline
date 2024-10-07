using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Material enemyMat;
    private Transform target; // 적이 달려오는 타겟
    
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Barrier").transform;
        EnemyManager.instance.enemies.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // 맞은거 확인용 (임시)
            Debug.Log("Shoot Success");
            enemyMat.color = Color.yellow;
        }
    }

    private void OnDestroy()
    {
        EnemyManager.instance.enemies.Remove(this);
    }
}
