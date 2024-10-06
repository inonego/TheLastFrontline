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
        UpdateBarrierVisual();
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

    // 배리어 HP 상태에 따라 배리어 색상 다르게
    void UpdateBarrierVisual()
    {
        float hpRatio = barrierHp / barrierHpMax;
        
        // HP에 따라 배리어 색상 변화
        // 임의로 색상 정함...
        if (hpRatio < 0.5f)
        {
            barrierMat.color = Color.red;
        }
        else if (hpRatio < 0.75f)
        {
            barrierMat.color = Color.yellow;
        }
        
        
        // 대충 배리어 파괴 직전 이펙트나 파티클 효과같은거 넣기
    }
    
    
}
