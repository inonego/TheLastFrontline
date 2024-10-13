using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public List<Enemy> enemies;
    

    public void DeleteAllEnemies(float deleteTime = 0f)
    {
        foreach (var enemy in enemies)
        {
            Destroy(enemy,deleteTime);
        }
    }
}
