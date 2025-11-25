using DG.Tweening;
using UnityEngine;

public class LillithStaffScript : MonoBehaviour
{

    public float _duration, _hueDuration;

    private int _hitEffectAmount = Shader.PropertyToID("_hEffectAmount");
    private int _hueShiftAmount = Shader.PropertyToID("_hueShiftAmount");
    private int _hcolor = Shader.PropertyToID("_hEffectColor");

    private SpriteRenderer _spriteRenderer;
    private Material _material;

    private float _flashLerpAmount, _hueLerpAmount;

    public enum Colors {red, orange, green, purple, yellow }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
    }

    private void OnFlashUpdate()
    {
        _material.SetFloat(_hitEffectAmount, GetFlashValue());
    }

    private void OnHueUpdate()
    {
        _material.SetFloat(_hueShiftAmount, GetHueValue());
    }

    private void OnFlashComplete()
    {
        DOTween.To(GetFlashValue, SetFlashValue, 0f, _duration).SetEase(Ease.OutExpo).OnUpdate(OnFlashUpdate);
    }

    private float GetFlashValue()
    {
        return _flashLerpAmount;
    }

    private void SetFlashValue(float f)
    {
        _flashLerpAmount = f;
    }

    private float GetHueValue()
    {
        return _hueLerpAmount;
    }

    private void SetHueValue(float f)
    {
        _hueLerpAmount = f;
    }

    public void callFlash()
    {
        _flashLerpAmount = 0;
        DOTween.To(GetFlashValue, SetFlashValue, 1f, _duration).SetEase(Ease.OutExpo).OnUpdate(OnFlashUpdate).OnComplete(OnFlashComplete);
    }

    public void changeColor(Colors c)
    {
        float color = 0;
        if (c == Colors.red) color = 0;
        else if (c == Colors.green) color = 0.3f;
        else if (c == Colors.purple) color = 0.75f;
        else if (c == Colors.orange) color = 0.05f;
        else if (c == Colors.yellow) color = 0.15f;

        print(color + "-" + _hueLerpAmount + ">" + 1 + "-" + color);
        if (color - _hueLerpAmount >  1 - color) color = color-1;

        DOTween.To(GetHueValue, SetHueValue, color, _hueDuration).SetEase(Ease.InOutExpo).OnUpdate(OnHueUpdate);
    }
}
