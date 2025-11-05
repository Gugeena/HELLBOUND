using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class spriteFlashScript : MonoBehaviour
{

    public float _duration;

    private int _hitEffectAmount = Shader.PropertyToID("_hEffectAmount");

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
        DOTween.To(GetLerpValue, SetLerpValue, 1f, _duration).SetEase(Ease.OutExpo).OnUpdate(OnLerpUpdate).OnComplete(OnLerpComplete);
    }

}
