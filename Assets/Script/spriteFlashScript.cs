using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class spriteFlashScript : MonoBehaviour
{

    public float _duration;
    public float targetValue = 1f;

    private int _hitEffectAmount = Shader.PropertyToID("_hEffectAmount");
    private int _hcolor = Shader.PropertyToID("_hEffectColor");

    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;

    private float _lerpAmount;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        _materials = new Material[_spriteRenderers.Length];
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
        }
    }

    private void OnLerpUpdate()
    {
        for (int i = 0; i < _materials.Length; ++i)
        {
            _materials[i].SetFloat(_hitEffectAmount, GetLerpValue());
        }
    }

    private void OnLerpComplete()
    {
        DOTween.To(GetLerpValue, SetLerpValue, 0f, _duration).SetEase(Ease.OutExpo).OnUpdate(OnLerpUpdate);
    }

    private float GetLerpValue()
    {
        return _lerpAmount;
    }

    private void SetLerpValue(float f)
    {
        _lerpAmount = f;
    }

    public void callFlash()
    {
        _lerpAmount = 0;
        DOTween.To(GetLerpValue, SetLerpValue, targetValue, _duration).SetEase(Ease.OutExpo).OnUpdate(OnLerpUpdate).OnComplete(OnLerpComplete);
    }

    public void changeColor(bool isRed)
    {
        foreach(Material material in _materials)
        {
            Color color = isRed ? Color.red : Color.green;
            material.SetColor(_hcolor, color);
        }
    }

    public void forceDefault()
    {
        DOTween.Clear();
        for (int i = 0; i < _materials.Length; ++i)
        {
            _materials[i].SetFloat(_hitEffectAmount, 0);
        }
    }    
}
