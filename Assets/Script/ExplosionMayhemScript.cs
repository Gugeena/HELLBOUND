using JetBrains.Annotations;
using System;
using UnityEngine;

public class ExplosionMayhemScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int mayhemcounter = 0;
    public bool hammed = false;
    public ContactFilter2D contactFilter;
    
    void Start()
    {
        Collider2D[] colliders = new Collider2D[3];
        Physics2D.OverlapCircle(this.transform.position, 3.34568f, contactFilter, colliders);
        if (hammed == false && colliders[2] != null)
        {
            hammed = true;
            StyleManager.instance.growStyle(1);
            StyleManager.instance.undisputed(3);
        }
    }
}
