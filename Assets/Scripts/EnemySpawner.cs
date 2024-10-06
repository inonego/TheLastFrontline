using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemy;
    public float minTimeBetweenSpawns;
    public float maxTimeBetweenSpawns;
    private float spawnRate = 1.5f;

    [SerializeField]
    private int phaseNumber = 1; // phase 번호 (1~3?)
    
    private TimeCounter respawnCounter;

    // 스폰 공간 범위 설정
    public GameObject rangeObject;
    private BoxCollider rangeCollider;

    void Start()
    {
        rangeCollider = rangeObject.GetComponent<BoxCollider>();
        
        respawnCounter = new TimeCounter();
        // 여기서 TimeCounter 시작해줘야 스폰되던데..
        // 아닐 수도,,, 일단 고침
        spawnRate = Random.Range(minTimeBetweenSpawns,maxTimeBetweenSpawns);
        respawnCounter.Start(spawnRate);
    }
    void Update()
    {
        respawnCounter.Update();
        
        if(respawnCounter.WasEndedThisFrame())
            Spawn();
        
        // 남은 시간에 따라 Phase 변경
        CheckPhase();
    }

    void Spawn()
    {
        Instantiate(enemy, ReturnRandomPosition(),transform.rotation);
        
        // 스폰 후에 다시 respawnCounter 시작
        spawnRate = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
        respawnCounter.Start(spawnRate); // 스폰 후 다음 스폰 카운트다운 시작
    }

    void CheckPhase()
    {
        if (phaseNumber == 1 && GameManager.instance.GetElapsedTime() >= 120f) // phase2 진입 (2분 경과)
        {
            IncreaseDifficulty();
        } else if (phaseNumber == 2 && GameManager.instance.GetElapsedTime() >= 240f) // phase3 진입 (4분 경과)
        {
            IncreaseDifficulty();
        }
    }
    
    void IncreaseDifficulty()
    {
        Debug.Log(phaseNumber + " phase Started");
        phaseNumber++; // 다음 페이즈로 이동됨
        
        // 임의로 난이도 조정 ;; ㅎ
        minTimeBetweenSpawns -= 0.1f; // 스폰 최소 시간 감소
        maxTimeBetweenSpawns -= 0.1f; // 스폰 최대 시간 감소

        // 스폰 간격 -> 너무 짧아지지 않도록 일단 제한
        minTimeBetweenSpawns = Mathf.Clamp(minTimeBetweenSpawns, 0.5f, 10f);
        maxTimeBetweenSpawns = Mathf.Clamp(maxTimeBetweenSpawns, 1f, 15f);
    }
    
    // 스폰 범위에서 랜덤한 위치 반환
    Vector3 ReturnRandomPosition()
    {
        Vector3 originPosition = rangeObject.transform.position;
        // 콜라이더 사이즈를 가져오기
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;
        
        range_X = Random.Range( (range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range( (range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, 0f, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
}
