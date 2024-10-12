using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : Singleton<SpawnerManager>
{
    [SerializeField]
    private int phaseNumber = 1; // phase 번호 (1~3?)
    public List<EnemySpawner> phase1Spawners;
    public List<EnemySpawner> phase2Spawners;
    public List<EnemySpawner> phase3Spawners;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 남은 시간에 따라 Phase 변경
        CheckPhase();
    }
    
    public void SpawnerAdd(int phase,EnemySpawner spawner) 
    {
        switch (phase)
        {
            case 1:
                phase1Spawners.Add(spawner);
                break;
            case 2:
                phase2Spawners.Add(spawner);
                break;
            case 3:
                phase3Spawners.Add(spawner);
                break;
        }
    }

    public void SpawnerStart(int phase=1) //스포너 활성화 
    {
        switch (phase)
        {
            case 1:
                foreach (var spawner in phase1Spawners)
                {
                    spawner.ActivationSpawner();
                }
                break;
            case 2:
                foreach (var spawner in phase2Spawners)
                {
                    spawner.ActivationSpawner();
                }
                break;
            case 3:
                foreach (var spawner in phase3Spawners)
                {
                    spawner.ActivationSpawner();
                }
                break;
        }
    }

    public void SpawnerStop()
    {
        switch (phaseNumber)
        {
            case 1:
                foreach (var spawner in phase1Spawners)
                {
                    spawner.InactivationSpawner();
                }
                break;
            case 2:
                foreach (var spawner in phase2Spawners)
                {
                    spawner.InactivationSpawner();
                }
                break;
            case 3:
                foreach (var spawner in phase3Spawners)
                {
                    spawner.InactivationSpawner();
                }
                break;
        }
    }
    
    private void CheckPhase()
    {
        if (GameManager.instance.ElapsedTime >= 120f) // phase2 진입 (2분 경과)
        {
            SpawnerStop();
            SpawnerStart(2);
            IncreaseDifficulty();
        } else if (GameManager.instance.ElapsedTime >= 240f) // phase3 진입 (4분 경과)
        {
            SpawnerStop();
            SpawnerStart(3);
            IncreaseDifficulty();
        }
    }
    void IncreaseDifficulty()//일단 보류
    {
        //Debug.Log(phaseNumber + " phase Started");
        phaseNumber++; // 다음 페이즈로 이동됨
        
        // 임의로 난이도 조정 ;; ㅎ
        //minTimeBetweenSpawns -= 0.1f; // 스폰 최소 시간 감소
        //maxTimeBetweenSpawns -= 0.1f; // 스폰 최대 시간 감소

        // 스폰 간격 -> 너무 짧아지지 않도록 일단 제한
        //minTimeBetweenSpawns = Mathf.Clamp(minTimeBetweenSpawns, 0.5f, 10f);
        //maxTimeBetweenSpawns = Mathf.Clamp(maxTimeBetweenSpawns, 1f, 15f);
    }
}
