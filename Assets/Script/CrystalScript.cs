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
        GameObject obj = collision.gameObject;

        if (obj.tag == "llocation")
        {
            if(RLocation == null) RLocation = GameObject.Find("RLOCATIONLOCATION").transform;
            this.transform.position = new Vector3(RLocation.position.x, this.transform.position.y, this.transform.position.z);
            return;
        }
        else if (obj.tag == "rlocation")
        {
            if (LLocation == null) LLocation = GameObject.Find("LLOCATIONLOCATION").transform;
            this.transform.position = new Vector3(LLocation.position.x, this.transform.position.y, this.transform.position.z);
            return;
        }

        if (obj.CompareTag("enemyorb") ||
          obj.name == "Hand_L" ||
          obj.name == "Hand_R" ||
          obj.name == "Leg_L" ||
          obj.name == "Leg_R" ||
          obj.name == "headPivot" ||
          obj.CompareTag("weaponPickup") ||
          obj.name == "square" ||
          obj.name == "Torso" ||
          obj.CompareTag("poison") ||
          obj.CompareTag("Fireball") ||
          obj.CompareTag("FireballP") ||
          obj.CompareTag("mfHitbox") ||
          obj.CompareTag("pushUp"))
        {
           return;
        }

        if (obj.layer == 8 && !shouldBreakToPlatform)
        {
            return;
        }

        if (!hasExploded)
        {
            hasExploded = true;
            float adder = 0f;
            if (obj.layer == 3 && obj.name != "Movable")
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
