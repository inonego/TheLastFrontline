using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DissolveEffect : MonoBehaviour
{   
    [Header("General")]
    public bool useOnSpawn;
    public bool useOnDead;
    public float dissolveSpeed = 1.5f;

    [Header("Material")]
    public bool replaceMaterialOnDissolve;
    public Material dissolveMaterial;

    private Health health;

    private MeshRenderer[] meshRenderers;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    private void Awake()
    {   
        health = GetComponent<Health>();

        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        health.OnStateChanged += OnHealthStateChanged;
    }

    private void OnEnable()
    {
        if (useOnSpawn)
        {
            Dissolve(inverse: true);
        }        
    }

    private void OnHealthStateChanged(Health.StateChangedEventArgs e)
    {
        if(e.Current == Health.State.Dead)
        { 
            if (useOnDead)
            {
                DissolveOnDead();
            }        
        }
    }

    private void DissolveOnDead()
    {
        StartCoroutine(DoDissolveOnDead());
    }


    private IEnumerator DoDissolveOnDead()
    {
        yield return StartCoroutine(DoDissolve(inverse: false));
        
        Destroy(transform.root.gameObject);
    }

    public void Dissolve(bool inverse = false)
    {
        StartCoroutine(DoDissolve(inverse));
    }

    private IEnumerator DoDissolve(bool inverse = false)
    {
        if (replaceMaterialOnDissolve)
        {
            SetDissolveMaterial();
        }

        float dissolveAmount = inverse ? 1f : 0f;

        while (inverse && dissolveAmount > 0f || !inverse && dissolveAmount < 1f)
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount + dissolveSpeed * Time.deltaTime * (inverse ? -1f : 1f));

            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material.SetFloat("_DissolveAmount", dissolveAmount);
            }

            foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
            {
                skinnedMeshRenderer.material.SetFloat("_DissolveAmount", dissolveAmount);
            }
            
            yield return null;
        }
    }

    private void SetDissolveMaterial()
    {
        if(dissolveMaterial == null) return;

        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material = dissolveMaterial;
        }

        foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
        {
            skinnedMeshRenderer.material = dissolveMaterial;
        }
    }
}
