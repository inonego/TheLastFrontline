using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSquid : MonoBehaviour
{
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeAni()
    {
        animator.SetTrigger("Walk");
    }
}
