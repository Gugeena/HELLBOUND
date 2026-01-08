using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrystalScript : MonoBehaviour
{
    public GameObject[] particles;
    public GameObject Hitbox;
    public bool shouldBreakToPlatform;
    bool hasExploded = false;
    public GameObject deathParticles;
    public Transform RLocation;
    public Transform LLocation;
    public GameObject poisonBefore;
    public AudioClip crystalBreak;
    public AudioSource crystalBreakSource;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(failsafe());
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerMovement.shouldMakeSound && crystalBreakSource != null && crystalBreakSource.isPlaying) crystalBreakSource.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.gameObject.tag == "mfHitbox")
        {
            Instantiate(deathParticles, new Vector2(this.transform.position.x, this.transform.position.y - 0.25f), Quaternion.identity);
            Destroy(gameObject);
        }
        */

        if (collision.gameObject.name == "LLocation")
        {
            RLocation = GameObject.Find("RLOCATIONLOCATION").transform;
            this.transform.position = new Vector3(RLocation.position.x, this.transform.position.y, this.transform.position.z);
            return;
        }
        else if (collision.gameObject.name == "RLocation")
        {
            LLocation = GameObject.Find("LLOCATIONLOCATION").transform;
            this.transform.position = new Vector3(LLocation.position.x, this.transform.position.y, this.transform.position.z);
            return;
        }

        if (collision.gameObject.tag == "enemyorb" ||
          collision.gameObject.name == "Hand_L" ||
          collision.gameObject.name == "Hand_R" ||
          collision.gameObject.name == "Leg_L" ||
          collision.gameObject.name == "Leg_R" ||
          collision.gameObject.name == "headPivot" ||
          collision.gameObject.tag == "weaponPickup" ||
          collision.gameObject.name == "square" ||
          collision.gameObject.name == "Torso" ||
          collision.gameObject.tag == "poison" || 
          collision.gameObject.tag == "Fireball" ||
          collision.gameObject.tag == "FireballP" ||
          collision.gameObject.tag == "mfHitbox")
        {

            return;
        }

        if (collision.gameObject.layer == 8 && !shouldBreakToPlatform)
        {
            return;
        }

        if (!hasExploded)
        {
            hasExploded = true;
            float adder = 0f;
            if (collision.gameObject.layer == 3 && collision.gameObject.name != "Movable")
            {
                adder = 1f;
            }

            if(PlayerMovement.shouldMakeSound) crystalBreakSource = audioManager.instance.playAudio(crystalBreak, 1, 1, this.transform, audioManager.instance.sfx);
            Instantiate(poisonBefore, new Vector2(this.transform.position.x, this.transform.position.y + adder), Quaternion.identity);
            for (int i = 0; i < particles.Length; i++)
            {
                Instantiate(particles[i], new Vector2(this.transform.position.x, this.transform.position.y + adder), Quaternion.identity);
            }
            float elapsed = 0f;
            float duration = 0.1f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
            }
            GameObject poisonhitbox = Instantiate(Hitbox, new Vector2(this.transform.position.x, this.transform.position.y + adder), Quaternion.identity);
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
            BoxCollider2D boxCollider = gameObject.GetComponent<BoxCollider2D>();
            boxCollider.enabled = false;
            StartCoroutine(deleter(poisonhitbox));
        }
    }

    public IEnumerator platformbreakfailsafe()
    {
        shouldBreakToPlatform = false;
        yield return new WaitForSeconds(0.1f);
        shouldBreakToPlatform = true;
    }

    public IEnumerator failsafe()
    {
        yield return new WaitForSeconds(5f);
        Instantiate(deathParticles, new Vector2(this.transform.position.x, this.transform.position.y - 0.25f), Quaternion.identity);
        Destroy(gameObject);
    }

    public IEnumerator deleter(GameObject hitbox)
    {
        yield return new WaitForSeconds(3.8f);
        Destroy(hitbox);
        Destroy(gameObject);
    }
}
