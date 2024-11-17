using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class SpawnOnDead : MonoBehaviour
{
    public int count;
    public GameObject prefab;
    
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();

        health.OnStateChanged += OnHealthStateChanged;
    }

    private void OnHealthStateChanged(Health.State state)
    {
        if(state == Health.State.Dead)
        {
            Divide();
        }
    }

    public void Divide()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject GO = Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}

