using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class flashScript : MonoBehaviour
{
    private Coroutine _coroutine;

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void CallFlash()
    {
        _anim.Play("whiteFlash");
    }
}


