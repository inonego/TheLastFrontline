using System.Collections;
using System.Collections.Generic;
using UnityCommunity.UnitySingleton;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    public List<GameObject> Enemies = new List<GameObject>();
    
    public void DeleteAllEnemies(float deleteTime = 0f)
    {
        foreach (var enemy in Enemies)
        {
            Destroy(enemy, deleteTime);
        }
        
        Enemies.Clear();
    }
}
