using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spearScript : MonoBehaviour
{
    bool shouldReturn = false;
    public GameObject player;
    Vector2 targetPoint;
    public GameObject blowUpParticles;
    private camShakerScript camShakerScript;
    public GameObject pickup;
    public Rigidbody2D rb;

    public Transform RLocation;
    public Transform LLocation;

    public GameObject explosion;

    bool landed = false;

    public bool hasexploded = false;

    public GameObject particler;

    [SerializeField] private AudioClip explode;

    private int teleportCount = 0;
    
    private void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        camShakerScript = GetComponent<camShakerScript>();
        rb.linearVelocity = transform.up * 30;
    }

    private void Update()
    {
        if (rb.linearVelocity != Vector2.zero && !landed)
        {
            Vector2 direction = rb.linearVelocity;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        if (shouldReturn)
        {
            targetPoint = player.transform.position;
            if (Vector2.Distance(targetPoint, this.transform.position) < 0.5f)
            {
                if (player.GetComponent<PlayerMovement>().currentWeapon == 0)
                {
                    Instantiate(pickup, player.transform.position, Quaternion.identity);
                    Destroy(this.gameObject);
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPoint, 15f * Time.deltaTime);
                //float angle = Mathf.Atan2(targetPoint.x, targetPoint.y) * Mathf.Rad2Deg;
                //this.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "LLocation")
        {
            RLocation = GameObject.Find("RLOCATIONLOCATION").transform;
            this.transform.position = new Vector3(RLocation.position.x - 2f, this.transform.position.y, 0);
            //shouldReturn = true;
            teleportCount++;
            rb.gravityScale = 1f;
            BoxCollider2D boxCollider2D = new BoxCollider2D();
            boxCollider2D = GetComponent<BoxCollider2D>();
            boxCollider2D.isTrigger = false;
            return;
        }
        else if (collision.gameObject.name == "RLocation")
        {
            LLocation = GameObject.Find("LLOCATIONLOCATION").transform;
            this.transform.position = new Vector3(LLocation.position.x + 2f, this.transform.position.y, 0);
            rb.gravityScale = 1f;
            teleportCount++;
            BoxCollider2D boxCollider2D = new BoxCollider2D();
            boxCollider2D = GetComponent<BoxCollider2D>();
            boxCollider2D.isTrigger = false;
            //shouldReturn = true;
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
            collision.gameObject.tag == "FireballP" ||
            collision.gameObject.tag == "Fireball" || 
            collision.gameObject.tag == "Explosion" ||
            collision.gameObject.tag == "Log" ||
            collision.gameObject.tag == "Crystal")
        {
            return;
        }

        if (collision.gameObject.layer == 8 && !hasexploded || collision.gameObject.layer == 3 && !hasexploded)
        {
            Vector2 direction = collision.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); 

            StartCoroutine(Bouttaxplode());
            //rb.gravityScale = 1f;
            //BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
            //boxCollider2D.isTrigger = true;
            shouldReturn = false;
            //StartCoroutine(Bouttaxplode());
        }

        //shouldReturn = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
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
          collision.gameObject.tag == "FireballP" ||
          collision.gameObject.tag == "Fireball" ||
          collision.gameObject.tag == "Explosion" ||
          collision.gameObject.tag == "Log" ||
          collision.gameObject.tag == "Crystal")
        {
            return;
        }

        if (collision.gameObject.layer == 8 && !hasexploded || collision.gameObject.layer == 3 && !hasexploded)
        {
            Vector2 direction = collision.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            shouldReturn = false;
            StartCoroutine(Bouttaxplode());
        }
    }

    public IEnumerator Bouttaxplode()
    {
        hasexploded = true;
        Debug.Log("Explosion coroutine started");
        landed = true;
        particler.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        rb.linearVelocity = Vector2.zero;

        Debug.Log("Activating explosion GameObject");
        float pitch = UnityEngine.Random.Range(0.8f, 1.01f);
        audioManager.instance.playAudio(explode, 0.6f, pitch, transform, audioManager.instance.sfx);
        explosion.SetActive(true);

        Debug.Log("Instantiating blowUpParticles");
        StartCoroutine(camShakerScript.shake());
        Instantiate(blowUpParticles, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.1f);
        explosion.SetActive(false);
        Destroy(gameObject);
        camShakerScript.Stop();
    }

    public int getteleportCount()
    {
        return teleportCount;
    }
}
