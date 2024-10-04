using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public float barrierHpMax = 100f;
    [SerializeField]
    private float barrierHp;
    public Material barrierMat; 
    
    public float damage; //몬스터 한마리당 소모되는 HP량
    void Start()
    {
        barrierHp = barrierHpMax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            barrierHp-=damage; 
            
            //대충 베리어 이펙트
            
            
            //대충 타 죽는 몬스터
            Destroy(collision.gameObject);

            if (barrierHp <= 0)
            {
                GameManager.instance.IsGameOver = true;
            }
        }
        
        
    }
}
