using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Material enemyMat;
    
    private Transform target; // 적이 달려오는 타겟
    private Rigidbody rigid;
    private bool canMove = true;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        EnemyManager.instance.enemies.Add(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Barrier").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (canMove && target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Vector3 reactVec = transform.position - collision.transform.position;
            
            StartCoroutine(OnDamage(reactVec));
        }
    }

    private void OnDestroy()
    {
        
    }

    IEnumerator OnDamage(Vector3 reactDirection)
    {
        canMove = false;
        Debug.Log("Shoot Success");
        enemyMat.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);

        // 대충 적 죽는 애니메이션
        enemyMat.color = Color.red;
        reactDirection = reactDirection.normalized;
        reactDirection += Vector3.up;
        rigid.AddForce(reactDirection * 5, ForceMode.Impulse);
        EnemyManager.instance.enemies.Remove(this);
        
        Destroy(this, 3);
        
    }
}
