using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using inonego;

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

    private void OnEnable()
    {
        rigidbody.velocity = transform.forward * bulletSpeed;

        lifeTimeCounter.Start(bulletLifeTime);
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector3.zero;

        lifeTimeCounter.Stop();
    }


    private void Update()
    {
        lifeTimeCounter.Update();

        if (lifeTimeCounter.WasEndedThisFrame())
        {
            Destroy();
        }
    }

    private void FixedUpdate()
    {
        rigidbody.rotation = rigidbody.rotation * Quaternion.LookRotation(Vector3.up);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        gameObject.Despawn();
    }
}
