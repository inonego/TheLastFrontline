using System.Collections;
using System.Collections.Generic;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnerManager : MonoSingleton<SpawnerManager>
{
    [SerializeField]
    private int phaseNumber = 1; // phase 번호 (1~3?)
    public List<EnemySpawner> phase1Spawners;
    public List<EnemySpawner> phase2Spawners;
    public List<EnemySpawner> phase3Spawners;

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainScene")
        {
            return;
        }
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
        phase = phaseNumber;
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
        if (phaseNumber == 1 && GameManager.Instance.ElapsedTime >= 120f) // phase2 진입 (2분 경과)
        {
            SpawnerStop();
            phaseNumber++;
            SpawnerStart(2);
            
        } else if (phaseNumber == 2 && GameManager.Instance.ElapsedTime >= 240f) // phase3 진입 (4분 경과)
        {
            SpawnerStop();
            phaseNumber++;
            SpawnerStart(3);
        }
    }
    
    public void ResetList()
    {
        phase1Spawners.Clear();
        phase2Spawners.Clear();
        phase3Spawners.Clear();
        phaseNumber = 1;
    }
}
