using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int phase=1;
    public List<GameObject> enemyPrefabs;
    public List<float> spawnChances; //각 프리팹 스폰 확률?
                                     //(0~1 사이 값으로 합이 무조건 1이 돼야함)
    public GameObject enemy;
    public float minTimeBetweenSpawns;
    public float maxTimeBetweenSpawns;
    private float spawnRate = 1.5f;
    private TimeCounter respawnCounter;

    // 스폰 공간 범위 설정
    public GameObject rangeObject;
    private BoxCollider rangeCollider;
    
    void Start()
    {
        respawnCounter = new TimeCounter();

        rangeCollider = rangeObject.GetComponent<BoxCollider>();
        respawnCounter = new TimeCounter();
        SpawnerManager.Instance.SpawnerAdd(phase, this);

    }
    void Update()
    {
        respawnCounter.Update();
        if(respawnCounter.WasEndedThisFrame())
            Spawn();
    }

    public void ActivationSpawner()//스포너 활성화
    {
        spawnRate = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
        respawnCounter.Start(spawnRate);
    }

    public void InactivationSpawner()//스포너 비활성화
    {
        respawnCounter.Stop();
    }
    
    void Spawn()
    {

        // GetRandomEnemyPrefab => 여기서 프리팹 확률에 따라서 선택
        GameObject enemyPrefab = GetRandomEnemyPrefab();
        Instantiate(enemyPrefab, ReturnRandomPosition(), transform.rotation);
        
        // 스폰 후에 다시 respawnCounter 시작
        spawnRate = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
        respawnCounter.Start(spawnRate); // 스폰 후 다음 스폰 카운트다운 시작
    }
    
    // 프리팹 리스트에서 확률로 랜덤 고르기
    GameObject GetRandomEnemyPrefab()
    {
        float randomValue = Random.value; // 0 ~ 1 사이 확률 float 값
        float cumulativeProbability = 0f;

        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            cumulativeProbability += spawnChances[i];
            if (randomValue <= cumulativeProbability)
            {
                return enemyPrefabs[i];
            }
        }

        // 확률 계산 잘못됨 => 기본값으로 첫번째 프리팹 반환
        return enemyPrefabs[0];
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
