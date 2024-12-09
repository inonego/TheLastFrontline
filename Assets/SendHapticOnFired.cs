using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SendHapticOnFired : MonoBehaviour
{
    private XRController xrController;

    private Gun gun;

    public float amplitude = 0.5f;
    public float duration = 0.1f;

    private void Awake()
    {
        xrController = GetComponentInParent<XRController>();

        gun = GetComponent<Gun>();

        gun.OnFired += OnGunFired;
    }

    private void OnGunFired()
    {   
        xrController.inputDevice.SendHapticImpulse(0, amplitude, duration);
    }
}
