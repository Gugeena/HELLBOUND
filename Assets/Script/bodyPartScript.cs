using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyPartScript : MonoBehaviour
{
    public Animator animator;

    public void disappear()
    {
        animator = GetComponentInChildren<Animator>();
        animator.enabled = true;
        Destroy(gameObject, 4.5f);
    }
}
