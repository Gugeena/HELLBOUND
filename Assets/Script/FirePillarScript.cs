using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePillarScript : MonoBehaviour
{
    public BoxCollider2D boxcollider;
    public AudioSource firepillarSource;
    public AudioClip firepillarClip;

    // Start is called before the first frame update
    void Start()
    {
        firepillarSource = audioManager.instance.playAudio(firepillarClip, 1, 1, this.transform, audioManager.instance.sfx);
        camShakerScript cum = GetComponent<camShakerScript>();
        StartCoroutine(cum.shake());
        StartCoroutine(firepillarmeresmalllifetimeohtobeanenemybeloved());
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerMovement.shouldMakeSound && firepillarSource != null) firepillarSource.Stop();
    }

    public IEnumerator firepillarmeresmalllifetimeohtobeanenemybeloved()
    {
        yield return new WaitForSeconds(0.2f);
        boxcollider.enabled = true;
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
