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
<<<<<<< Updated upstream
        
=======

>>>>>>> Stashed changes
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            barrierHp-=damage; 
            
            
            //대충 베리어 이펙트
            
            
            //대충 타 죽는 몬스터
            Destroy(other.gameObject, 0.5f); //죽는 모션 이후 삭제
            
            
            // 배리어 HP 상태에 따라 배리어 색상 다르게
            // HP에 따라 배리어 색상 변화
            // 임의로 색상 정함...
            if (barrierHp / barrierHpMax < 0.5f)
            {
                barrierMat.color = Color.red;
            }
            else if (barrierHp / barrierHpMax < 0.75f)
            {
                barrierMat.color = Color.yellow;
            }
            // 대충 배리어 파괴 직전 이펙트나 파티클 효과같은거 넣기
            if (barrierHp <= 0)
            {
                GameManager.instance.IsGameOver = true;
            }
        }
        
        
    }
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
}
