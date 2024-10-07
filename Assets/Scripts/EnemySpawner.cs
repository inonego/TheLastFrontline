using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int phase=1;
    public GameObject enemy;
    public float minTimeBetweenSpawns;
    public float maxTimeBetweenSpawns;
    private float spawnRate = 1.5f;
<<<<<<< Updated upstream

=======
    
>>>>>>> Stashed changes
    
    private TimeCounter respawnCounter;

    void Start()
    {
<<<<<<< Updated upstream
        respawnCounter = new TimeCounter();
=======
        rangeCollider = rangeObject.GetComponent<BoxCollider>();
        respawnCounter = new TimeCounter();
        SpawnerManager.instance.SpawnerAdd(phase, this);
>>>>>>> Stashed changes
    }
    void Update()
    {
        respawnCounter.Update();
        if(respawnCounter.WasEndedThisFrame())
            Spawn();
    }

    public void ActivationSpawner()//스포너 활성화
    {
        Spawn();
    }

    public void InactivationSpawner()//스포너 비활성화
    {
        respawnCounter.Stop();
    }
    
    void Spawn()
    {
<<<<<<< Updated upstream
        Instantiate(enemy,transform.position,transform.rotation);
        spawnRate = Random.Range(minTimeBetweenSpawns,maxTimeBetweenSpawns);
        respawnCounter.Start(spawnRate);
=======
        Instantiate(enemy, ReturnRandomPosition(),transform.rotation);
        
        // 스폰 후에 다시 respawnCounter 시작
        spawnRate = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
        respawnCounter.Start(spawnRate); // 스폰 후 다음 스폰 카운트다운 시작
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
>>>>>>> Stashed changes
    }
}
