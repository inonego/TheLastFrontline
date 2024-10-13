using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundBullet : MonoBehaviour
{
    public bool isStart = false;
    public GameObject bullet;
    
    public Vector3 direction;
    public float angle;
    public float length;

    public int fireCount;
    public float fireDamp;
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
        while (true)
        {
          
            
            for(int i = 0; i < fireCount; i++)
            {
                currentdir = CreateVector();
                Fire(currentdir);
                yield return new WaitForSeconds(fireDamp);
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

    
    
    private void OnDrawGizmos()
    {
        DrawGizmosCone(transform.position,angle,length,(transform.rotation * direction).normalized,Color.red,true);
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
