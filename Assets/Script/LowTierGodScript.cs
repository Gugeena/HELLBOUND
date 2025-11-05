using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowTierGodScript : MonoBehaviour
{
    [SerializeField] private float time;
    private void Awake()
    {
        Destroy(gameObject, time);
    }
}
