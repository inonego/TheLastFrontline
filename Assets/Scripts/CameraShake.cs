using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    public float lerpAmount;
    public float minShakeDuration;
    public float maxShakeDuration;
    public float minShakeMagnitude;
    public float maxShakeMagnitude;

    private Coroutine shakeCoroutine;

    private Vector3 origin;

    private bool isShaking = false;

    protected override void Awake()
    {
        origin = transform.position;
    }

    private void Update()
    {
        if (!isShaking)
        {
            transform.position = Vector3.Lerp(transform.position, origin, lerpAmount);
        }
    }

    private IEnumerator DoGiveShake(float magnitude, float duration)
    {
        isShaking = true;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 delta = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * magnitude * Time.deltaTime;

            transform.position += delta;

            elapsed += Time.deltaTime;

            yield return null;
        }

        isShaking = false;
    }

    public void GiveShake(float magnitude, float duration)
    {
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(DoGiveShake(magnitude, duration));
    }

    public void GiveShake(GameObject origin)
    {
        float magnitude = 1.0f / Mathf.Max(1.0f, Vector3.Distance(transform.position, origin.transform.position));

        GiveShake(Random.Range(minShakeMagnitude, maxShakeMagnitude) * magnitude, Random.Range(minShakeDuration, maxShakeDuration));
    }
    public void GiveShake(float magnitude)
    {
        GiveShake(maxShakeMagnitude * magnitude, maxShakeDuration);
    }
}
