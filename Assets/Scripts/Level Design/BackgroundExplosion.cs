using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using inonego;

using Random = UnityEngine.Random;

[RequireComponent(typeof(PoolPack))]
public class BackgroundExplosion : MonoBehaviour
{
    [Header("General")]
    public LayerMask groundLayer;

    [Header("Time")]
    public float minTimeBetweenExplosion;
    public float maxTimeBetweenExplosion;

    public float despawnTime;

    [Header("Range")]
    public float distance;
    public float radius;
    public float noneRadius;
    public Transform noneExplosionPoint;

    [Header("Gizmo")]
    public Mesh gizmoMesh;

    private Coroutine workingCoroutine;

    private PoolPack poolPack;

    private void Awake()
    {
        poolPack = GetComponent<PoolPack>();
    }

    private void Start()
    {
        BeginExplosion();
    }

    public void BeginExplosion()
    {
        StopExplosion();

        workingCoroutine = StartCoroutine(ExplodeCoroutine());
    }

    public void StopExplosion()
    {
        if (workingCoroutine != null) StopCoroutine(workingCoroutine);
    }

    public void MakeExplosion(Vector3 position)
    {
        int index = Random.Range(0, poolPack.PoolList.Count);

        GameObject GO = poolPack.PoolList[index].Spawn();

        GO.transform.position = position;

        CameraShake.Instance.GiveShake(GO);

        StartCoroutine(DespawnExplosion(GO));
    }

    private IEnumerator DespawnExplosion(GameObject GO)
    {
        yield return new WaitForSeconds(despawnTime);

        GO.Despawn();
    }

    private IEnumerator ExplodeCoroutine()
    {
        while (true)
        {
            Vector3 randPos = GetRandomPosition();

            MakeExplosion(randPos);
            
            yield return new WaitForSeconds(Random.Range(minTimeBetweenExplosion, maxTimeBetweenExplosion));
        }
    }
    
    private Vector3 GetRandomPosition()
    {
        Vector3 randPos;

        while (true)
        {
            Vector2 randCircle = Random.insideUnitCircle * radius;

            randPos = new Vector3(randCircle.x, 0f, randCircle.y) + transform.position;

            randPos.y = noneExplosionPoint.transform.position.y;

            if (Vector3.Distance(noneExplosionPoint.transform.position, randPos) > noneRadius) break;
        }

        Physics.Raycast(randPos,Vector3.down, out RaycastHit hit, distance, groundLayer);

        randPos.y = hit.point.y;

        return randPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.1f);
        Gizmos.DrawWireMesh(gizmoMesh,transform.position, transform.rotation, transform.lossyScale * radius * 2);
        Gizmos.color = new Color(0f, 0f, 1f, 0.1f);
        Gizmos.DrawWireMesh(gizmoMesh, noneExplosionPoint.transform.position, noneExplosionPoint.transform.rotation, transform.lossyScale * noneRadius * 2);
    }
}
