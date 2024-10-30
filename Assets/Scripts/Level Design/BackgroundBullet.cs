using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundBullet : MonoBehaviour
{
    public bool isStart = false;
    
    
    public GameObject bullet;
    [Header("체크 안하면 다시 시작하기 전까지 방향 안바뀜")]
    public bool isAllRandomDirection = false;
    [Header("범위 설정 ")]
    public Vector3 direction;
    public float angle;
    
    [Header("총알 개수 및 연사 속도")]
    public int fireCount;
    public float fireDampMin, fireDampMax;
    
    [Header("초기 시작 시간")]
    public float startTime;
    
    [Header("다시 시작 시간")]
    public float restartDelay;
    
    private Coroutine workingCoroutine;
    private Vector3 currentdir;
    // Start is called before the first frame update
    void Start()
    {
        isStart = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            workingCoroutine = StartCoroutine(BulletFire());
            isStart = false;
        }
    }

    IEnumerator BulletFire()
    {
        yield return new WaitForSeconds(startTime);
        
        while (true)
        {
            if(!isAllRandomDirection)
                currentdir = CreateVector();
            
            for(int i = 0; i < fireCount; i++)
            {
                if(isAllRandomDirection)
                    currentdir = CreateVector();
                
                Fire(currentdir);
                yield return new WaitForSeconds(Random.Range(fireDampMin, fireDampMax));
            }
            yield return new WaitForSeconds(restartDelay);
        }
    }

    private void Fire(Vector3 dir)
    {
        Instantiate(bullet,transform.position,Quaternion.LookRotation(dir,Vector3.up));
    }

    private Vector3 CreateVector()
    {
        float alpha = Random.Range(0f, angle*0.5f);
        float theta = Random.Range(0f, 360f);
        Vector3 dir = transform.rotation * direction;
        return Quaternion.AngleAxis(theta, dir) * Quaternion.AngleAxis(alpha,Vector3.up+dir) * dir;
    }

    private void OnDrawGizmosSelected()
    {
        DrawGizmosCone(transform.position,angle,10f,(transform.rotation * direction).normalized,Color.red,true);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position,transform.position + currentdir);
    }

    public static void DrawGizmosCircle(Vector3 pos,float radius,Vector3 up,Color color,int step=10,Action<Vector3>action=null)
    {
        float theta=360f/(float)step;
        Vector3 cross=Vector3.Cross(up,Vector3.up);
        if(cross.magnitude==0f)
        {
            cross=Vector3.forward;
        }

        Vector3 prev=pos+Quaternion.AngleAxis(0f,up)*cross*radius;
        Vector3 next=prev;
        Gizmos.color=color;

        for(int i=1;i<=step;++i)
        {
            next=pos+Quaternion.AngleAxis(theta*(float)i,up)*cross*radius;

            Gizmos.DrawLine(prev,next);

            if(null!=action)
            {
                action(prev);
            }

            prev=next;
        }
    }

    static Vector3 top=Vector3.zero;
    public static void DrawGizmosCone(Vector3 pos,float angle,float height,  Vector3 up,Color color,bool inverse=false,int step=10)
    {
        float radius=height*Mathf.Tan(angle*Mathf.Deg2Rad*0.5f);

        if(inverse)
        {
            top=pos;
            DrawGizmosCircle(pos+up*height,radius,up,color,step,(v)=>{Gizmos.DrawLine(top,v);});
        }
        else
        {
            top=pos+up*height;
            DrawGizmosCircle(pos,radius,up,color,step,(v)=>{Gizmos.DrawLine(top,v);});
        }
    }
}
