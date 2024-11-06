using System.Collections;
using System.Collections.Generic;
using UnityCommunity.UnitySingleton;
using UnityEngine;

public class EnemyManager : PersistentMonoSingleton<EnemyManager>
{
    public List<Enemy> enemies;
    

    public void DeleteAllEnemies(float deleteTime = 0f)
    {
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject,deleteTime);
        }
    }

    public void ResetList()
    {
        enemies.Clear();
    }
}
