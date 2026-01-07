using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LilithDummyScript : MonoBehaviour
{
    public GameObject ChopParticles, ChopUpParticlesLocation;
    public spriteFlashScript spriteFlashScript;
    public LilithScript LilithScript;
    public bool hit;

    void Start()
    {
        GameObject particles = Instantiate(ChopParticles, ChopUpParticlesLocation.transform.position, ChopParticles.transform.rotation);
        if(!LilithScript.isRight) particles.transform.rotation = Quaternion.Euler(0,0, -85.672f);
        spriteFlashScript.callFlash();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "mfHitbox" ||  collision.gameObject.name == "meleehitbox")
        {
           if (hit) return;
           if(LilithScript == null) LilithScript = GameObject.Find("Lilith(Clone)").GetComponent<LilithScript>();
           LilithScript.HandleHit(collision);
           print("gavartyi");
           hit = true;
           StartCoroutine(Coooldown());
        }
    }

    public IEnumerator Coooldown()
    {
        yield return new WaitForSeconds(0.2f);
    }
}
