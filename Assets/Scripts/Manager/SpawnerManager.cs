using System;
using System.Collections.Generic;
using UnityCommunity.UnitySingleton;

[Serializable]
public class SpawnerPack
{
    public List<EnemySpawner> spawners;

    public void Start()
    {
        foreach (var spawner in spawners)
        {
            spawner.Activate();
        }
    }

    public void Stop()
    {
        foreach (var spawner in spawners)
        {
            spawner.Deactivate();
        }
    }
}

public class SpawnerManager : MonoSingleton<SpawnerManager>
{
    public List<SpawnerPack> SpawnerPackList;
}
