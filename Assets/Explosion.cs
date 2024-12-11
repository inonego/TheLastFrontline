using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int minDamage;
    public int maxDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();

            enemy.health.ApplyDamage(Random.Range(minDamage, maxDamage));
        }
    }
}
