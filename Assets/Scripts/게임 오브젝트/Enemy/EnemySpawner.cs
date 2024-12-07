using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{   
    [Serializable]
    public class SpawnInfo
    {
        public GameObject prefab;
        public float chance;
    }

    public List<SpawnInfo> spawnInfoList = new List<SpawnInfo>(); 

    public float minTimeBetweenSpawns;
    public float maxTimeBetweenSpawns;
    private float spawnRate = 1.5f;
    private TimeCounter respawnCounter = new TimeCounter();

    // 스폰 공간 범위 설정
    public GameObject rangeObject;
    private BoxCollider rangeCollider;
    
    private void Awake()
    {
        rangeCollider = rangeObject.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        respawnCounter.Update();

        if(respawnCounter.WasEndedThisFrame())
        {
            Spawn();
        }
    }

    public void Activate()//스포너 활성화
    {
        Spawn();
    }

    public void Deactivate()//스포너 비활성화
    {
        respawnCounter.Stop();
    }
    
    void Spawn()
    {
        // GetRandomEnemyPrefab => 여기서 프리팹 확률에 따라서 선택
        GameObject enemyPrefab = GetRandomEnemyPrefab();

        if (gameObject.scene.isLoaded) 
        {
            Instantiate(enemyPrefab, ReturnRandomPosition(), transform.rotation);
        }
        
        // 스폰 후에 다시 respawnCounter 시작
        spawnRate = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);

        respawnCounter.Start(spawnRate); // 스폰 후 다음 스폰 카운트다운 시작
    }
    
    // 프리팹 리스트에서 확률로 랜덤 고르기
    GameObject GetRandomEnemyPrefab()
    {
        float randomValue = Random.value; // 0 ~ 1 사이 확률 float 값
        float cumulativeProbability = 0f;

        for (int i = 0; i < spawnInfoList.Count; i++)
        {
            cumulativeProbability += spawnInfoList[i].chance;

            if (randomValue <= cumulativeProbability)
            {
                return spawnInfoList[i].prefab;
            }
        }

        return null;
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
