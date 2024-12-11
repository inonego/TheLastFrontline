using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandAnimator : MonoBehaviour
{
    private XRDirectInteractor _interactor;
    public Animator animator;
    
    private bool justGrabbed = false;
    private bool justReleased = false;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _interactor = GetComponent<XRDirectInteractor>();
    }
    
    void Start()
    {
        _interactor.selectEntered.AddListener(_ => justGrabbed = true);
        _interactor.selectExited.AddListener(_ => justReleased = true);
    }

    // Update is called once per frame
    void Update()
    {
        if (justGrabbed)
        {
            animator.SetTrigger("Grab");
            justGrabbed = false;
        }

        if (justReleased)
        {
            animator.SetTrigger("Throw");
            justReleased = false;
            
        }
    }
}
