using System;
using System.Collections;
using System.Collections.Generic;
using inonego;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Grenade : MonoBehaviour
{
    public float BoomRadius = 3f;
    public int damage = 1;
    
    private bool isTriggered = false;
    
    private XRGrabInteractable grabble;
    private Vector3 InitialPosition;
    void Awake()
    {
        grabble = GetComponent<XRGrabInteractable>();
        grabble.activated.AddListener(TriggerBoom);
        InitialPosition = transform.position;
    }


    private void OnCollisionEnter(Collision other)//위치 조정
    {
        if (other.gameObject.CompareTag("Ground")&&isTriggered==false)
        {
            StartCoroutine(BackToPosition());
        }
    }

    private void TriggerBoom(ActivateEventArgs arg)//핀뽑기
    {
        StartCoroutine(Boom());
        isTriggered=true;
        //대충 핀뽑는 소리
        
    }
    
    IEnumerator Boom()
    {
        yield return new WaitForSeconds(3f);
        
        //폭발 이펙트,사운드 등등
        Collider[] enemies = Physics.OverlapSphere(transform.position, BoomRadius, LayerMask.GetMask("Enemy"));
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Health>().ApplyDamage(damage);
        }
        
    }
    
    IEnumerator BackToPosition()
    {
        yield return new WaitForSeconds(2f);
        transform.position = InitialPosition;
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, BoomRadius);
    }
}
