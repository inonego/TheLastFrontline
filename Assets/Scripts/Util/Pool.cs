using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolPack
{
    public Transform spawnedToParent { get; private set; }

    public GameObject Prefab;

    public int InitalCount = 0;

    public int LeftCount => pool.Count;
    public int TotalCount => spawned.Count + pool.Count;

    private Queue<GameObject> pool = new Queue<GameObject>();
    private List<GameObject> spawned = new List<GameObject>();

    public IReadOnlyList<GameObject> Spawned => spawned;

    public void Init(Transform spawnedToParent)
    {
        this.spawnedToParent = spawnedToParent;

        // 초기 개수 만큼 게임 오브젝트를 추가합니다.
        for (int i = 0; i < InitalCount; i++)
        {
            pool.Enqueue(InstantiateGO());
        }
    }

    private GameObject InstantiateGO()
    {
        GameObject GO = MonoBehaviour.Instantiate(Prefab);

        // 게임 오브젝트 상태 설정
        GO.transform.SetParent(spawnedToParent);
        GO.SetActive(false);

        return GO;
    }

    public GameObject Spawn()
    {
        return Spawn(Vector3.zero, Quaternion.identity);
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        // 풀 목록에서 제거
        if (!pool.TryDequeue(out GameObject GO))
        {
            GO = InstantiateGO();
        }

        // 스폰 목록에 추가
        spawned.Add(GO);

        PoolUtil.Register(this, GO);

        GO.transform.position = position;
        GO.transform.rotation = rotation;

        // 게임 오브젝트 상태 설정
        GO.transform.SetParent(null);
        GO.SetActive(true);

        return GO;
    }

    internal void Despawn(GameObject GO)
    {
        // 스폰 목록에서 제거
        spawned.Remove(GO);

        // 풀 목록에 추가
        pool.Enqueue(GO);

        // 게임 오브젝트 상태 설정
        GO.transform.SetParent(spawnedToParent);
        GO.SetActive(false);
    }
}

public class Pool : MonoBehaviour
{
    [SerializeField]
    private List<PoolPack> _packList = new List<PoolPack>();

    public IReadOnlyList<PoolPack> packList => _packList;

    private void Awake()
    {
        foreach (var pack in packList)
        {
            pack.Init(transform);
        }
    }
}

public static class PoolUtil
{
    private static SerializedDictionary<GameObject, PoolPack> GOPoolPack = new SerializedDictionary<GameObject, PoolPack>();

    internal static void Register(PoolPack poolPack, GameObject GO)
    {
        if (GOPoolPack.ContainsKey(GO))
        {
            GOPoolPack[GO] = poolPack;
        }
        else
        {
            GOPoolPack.Add(GO, poolPack);
        }
    }

    public static void Despawn(this GameObject GO)
    {
        if (GOPoolPack.ContainsKey(GO))
        {
            GOPoolPack[GO].Despawn(GO);

            GOPoolPack.Remove(GO);
        }
        else
        {
            MonoBehaviour.Destroy(GO);
        }
    }
}