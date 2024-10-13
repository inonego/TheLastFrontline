using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundExplosion : MonoBehaviour
{   
    public bool isStart = false;
    
    public float radius;

    public float minTimeBetweenExplosion;
    public float maxTimeBetweenExplosion;
    
    public GameObject explosionPosition;
    public GameObject noneExplosionPosition;
    public float noneRadius;
 
    public Mesh gizmoMesh;
    public Coroutine WorkingCoroutine;
    private ParticleSystem explosionParticle;
    void Start()
    {
        isStart = true;
        explosionParticle = explosionPosition.GetComponent<ParticleSystem>();
    }


    void Update()
    {
        if (isStart)
        {
            WorkingCoroutine = StartCoroutine(ExplodeCoroutine());
            isStart = false;
        }
    }

    IEnumerator ExplodeCoroutine()
    {
        while (true)
        {
            //실행
            RandomPosition();
            explosionParticle.Play();
            
            yield return new WaitForSeconds(Random.Range(minTimeBetweenExplosion, maxTimeBetweenExplosion));
        }
    }
    
    void RandomPosition()
    {
        Vector3 randPos;
        while (true)
        {
            randPos = Random.insideUnitCircle * radius;
            randPos.z = randPos.y;
            randPos+=transform.position;
            randPos.y = noneExplosionPosition.transform.position.y;
            if(Vector3.Distance(noneExplosionPosition.transform.position, randPos)>noneRadius)
                break;
        }
        RaycastHit hit;
        Physics.Raycast(randPos,Vector3.down,out hit,100f,LayerMask.GetMask("Ground"));
        randPos.y = hit.point.y;
        explosionPosition.transform.position = randPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.1f);
        Gizmos.DrawWireMesh(gizmoMesh,transform.position,transform.rotation,transform.lossyScale*radius*2);
        Gizmos.color = new Color(0f, 0f, 1f, 0.1f);
        Gizmos.DrawWireMesh(gizmoMesh,noneExplosionPosition.transform.position,noneExplosionPosition.transform.rotation,transform.lossyScale*noneRadius*2);
    }
}
