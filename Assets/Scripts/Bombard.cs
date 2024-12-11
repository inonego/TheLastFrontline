using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombard : MonoBehaviour
{
    public float delayMinTime;
    public float delayMaxTime;
    public int explosionPerArea;

    public AudioClip requestAudioClip;
    public AudioClip airDropAudioClip;
    public float airDropAudioClipLength;

    public List<AudioClip> audioClips;

    public List<BackgroundExplosion> explosions;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator ExecuteCoroutine()
    {
        audioSource.clip = requestAudioClip;
        if (audioSource.clip != null) audioSource.Play();

        yield return new WaitForSeconds(requestAudioClip.length);

        audioSource.clip = airDropAudioClip;
        if (audioSource.clip != null) audioSource.Play();

        yield return new WaitForSeconds(airDropAudioClipLength);

        for (int i = 0; i < explosions.Count; i++)
        {
            for (int j = 0; j < explosionPerArea; j++)
            {
                if (explosions.Count > 0)
                {
                    // 폭발 효과 생성
                    explosions[i].MakeExplosionRandom();
                }

                if (audioClips.Count > 0)
                {
                    audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]);
                }

                yield return new WaitForSeconds(Random.Range(delayMinTime, delayMaxTime));
            }
        }
    }

    public void Execute()
    {
        StartCoroutine(ExecuteCoroutine());
    }
}
