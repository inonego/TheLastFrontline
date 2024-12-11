using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Radio : MonoBehaviour
{
    public Bombard bombard;

    private Vector3 InitialPosition;
    
    public bool canUsed = true;
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
            bombard.Execute();
            
            canUsed = false;
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
