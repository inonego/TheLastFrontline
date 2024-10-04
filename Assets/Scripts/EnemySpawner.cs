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

    
    private TimeCounter respawnCounter;

    void Start()
    {
        respawnCounter = new TimeCounter();
    }
    void Update()
    {
        respawnCounter.Update();
        
        if(respawnCounter.WasEndedThisFrame())
            Spawn();
    }

    void Spawn()
    {
        Instantiate(enemy,transform.position,transform.rotation);
        spawnRate = Random.Range(minTimeBetweenSpawns,maxTimeBetweenSpawns);
        respawnCounter.Start(spawnRate);
    }
}
