using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePillarScript : MonoBehaviour
{
    public BoxCollider2D boxcollider;
    // Start is called before the first frame update
    void Start()
    {
        camShakerScript cum = GetComponent<camShakerScript>();
        StartCoroutine(cum.shake());
        StartCoroutine(firepillarmeresmalllifetimeohtobeanenemybeloved());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator firepillarmeresmalllifetimeohtobeanenemybeloved()
    {
        yield return new WaitForSeconds(0.2f);
        boxcollider.enabled = true;
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
