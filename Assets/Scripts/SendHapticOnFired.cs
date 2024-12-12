using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SendHapticOnFired : MonoBehaviour
{
    private XRController xrController;

    private Gun gun;

    public float amplitudeFired = 0.5f;
    public float durationFired = 0.1f;
    public float amplitudeReloaded = 0.5f;
    public float durationReloaded = 0.1f;

    private void Awake()
    {
        xrController = GetComponentInParent<XRController>();

        gun = GetComponent<Gun>();

        gun.OnFired += OnGunFired;
        gun.OnReloaded += OnGunReloaded;
    }

    private void OnGunFired()
    {   
        xrController.inputDevice.SendHapticImpulse(0, amplitudeFired, durationFired);
    }

    private void OnGunReloaded()
    {   
        xrController.inputDevice.SendHapticImpulse(0, amplitudeReloaded, durationReloaded);
    }
}
