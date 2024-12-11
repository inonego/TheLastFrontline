using System;
using System.Collections;
using System.Collections.Generic;
using inonego;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Grenade : MonoBehaviour
{
    public float BoomRadius = 4f;
    public int damage = 3;
    public float boomForce = 500f;
    public GameObject explodePrefab;
    public AudioClip explodeSound;
    public AudioClip pinSound;
    
    private bool isTriggered = false;
    
    private XRGrabInteractable grabble;
    private AudioSource audioSource;
    private Vector3 InitialPosition;
    
    void Awake()
    {
        grabble = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
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
        audioSource.PlayOneShot(pinSound);
        //대충 핀뽑는 소리
    }
    
    IEnumerator Boom()
    {
        yield return new WaitForSeconds(4f);
        
        //폭발 이펙트,사운드 등등
        GameObject explodeObject = Instantiate(explodePrefab, transform.position, transform.rotation);
        audioSource.PlayOneShot(explodeSound);
        
        Collider[] enemies = Physics.OverlapSphere(transform.position, BoomRadius, LayerMask.GetMask("Enemy"));
        foreach (var enemy in enemies)
        {
            Rigidbody rbE = enemy.GetComponent<Rigidbody>();
            if (rbE != null)
            {
                rbE.AddExplosionForce(boomForce, transform.position, BoomRadius);
            }
            enemy.GetComponent<Health>().ApplyDamage(damage);
        }
        yield return new WaitForSeconds(1f);
        Destroy(explodeObject, 3f);
        Destroy(gameObject);
        
    }
    
    IEnumerator BackToPosition()
    {
        yield return new WaitForSeconds(2f);
        transform.position = InitialPosition;
        //rb.velocity = Vector3.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, BoomRadius);
    }
}
