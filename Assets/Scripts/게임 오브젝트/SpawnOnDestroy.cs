using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
    public int count;
    public GameObject prefab;

    private void OnDestroy()
    { 
        if (gameObject.scene.isLoaded) 
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject GO = Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}

