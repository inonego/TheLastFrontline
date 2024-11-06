using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class DivideOnDeath : MonoBehaviour
{
    public int count;
    public GameObject divideTo;
    
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();

        enemy.onDeath.AddListener(Divide);
    }

    public void Divide()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject GO = Instantiate(divideTo, transform.position, Quaternion.identity);

            Enemy other = GO.GetComponent<Enemy>();

            if (other != null)
            {
                EnemyManager.Instance.enemies.Add(GO);
            }
        }
    }
}

