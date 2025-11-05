using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shockwaveScript : MonoBehaviour
{
    [SerializeField] private float _shockWaveTime = 1.25f;

    private Coroutine _coroutine;

    private Material _material;

    private static int _waveDist = Shader.PropertyToID("waveDist");

    private void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }

    public void CallShockWave()
    {
        _coroutine = StartCoroutine(ShockWaveAction(-0.1f, 1f));
    }

    private IEnumerator ShockWaveAction(float startPos, float endPos)
    {
        _material.SetFloat(_waveDist, startPos);

        float lerpedAmount = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < _shockWaveTime)
        {
            elapsedTime += Time.deltaTime;

            lerpedAmount = Mathf.Lerp(startPos, endPos, (elapsedTime / _shockWaveTime));
            _material.SetFloat(_waveDist, lerpedAmount);
        }

        yield return null;
    }
}
