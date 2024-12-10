using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Radio : MonoBehaviour
{
    public int damage;
    private Vector3 InitialPosition;
    
    private bool canUsed=false;
    private XRGrabInteractable grabble;
    void Awake()
    {
        grabble = GetComponent<XRGrabInteractable>();
        grabble.activated.AddListener(UseRadio);
        InitialPosition = transform.position;
    }
    
    public void UseRadio(ActivateEventArgs arg)
    {
        if (canUsed)
        {
            //폭격
            foreach (var enemy in EnemyManager.Instance.Enemies)
            {
                enemy.health.ApplyDamage(damage);
            }
            
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(BackToPosition());
        }
    }
    
    IEnumerator BackToPosition()
    {
        yield return new WaitForSeconds(2f);
        transform.position = InitialPosition;
        
    }
}
