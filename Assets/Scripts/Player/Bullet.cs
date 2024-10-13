using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 100f;
    public float bulletLifeTime = 3f;

    private new Rigidbody rigidbody;

    private TimeCounter lifeTimeCounter = new TimeCounter();

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigidbody.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);

        lifeTimeCounter.Start(bulletLifeTime);
    }

    private void Update()
    {
        lifeTimeCounter.Update();

        if (lifeTimeCounter.WasEndedThisFrame())
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
