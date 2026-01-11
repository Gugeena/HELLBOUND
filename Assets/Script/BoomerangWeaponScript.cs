using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoomerangWeaponScript : MonoBehaviour
{
    public bool shouldReturn = false;
    public GameObject player;
    Vector2 targetPoint;
    public GameObject blowUpParticles;
    public GameObject pickup;
    public Rigidbody2D rb;

    public Transform RLocation;
    public Transform LLocation;

    private teleportCountScript tpsc;

    public AudioSource boomerangSound;

    public int killed, lastamountkilled;
    public bool richoed = false, inreturning = false;

    private void Start()
    {
        player = GameObject.Find("Player(Clone)");
        rb = GetComponent<Rigidbody2D>();
        killed = 0;
        lastamountkilled = 0;
        tpsc = GetComponent<teleportCountScript>();
    }

    private void Update()
    {
        if(player == null)
        {
            Destroy(gameObject);
        }

        if (!PlayerMovement.shouldMakeSound) boomerangSound.Stop();

        if (PauseScript.Paused) return;

        transform.Rotate(0, 0, 1440 * Time.deltaTime);

        if (shouldReturn)
        {
            targetPoint = player.transform.position;
            if (Vector2.Distance(targetPoint, this.transform.position) < 0.5f)
            {
                if (player.GetComponent<PlayerMovement>().currentWeapon == 0)
                {
                    GameObject drop = Instantiate(pickup, player.transform.position, Quaternion.identity);
                    drop.name = "BoomerangWhichYouJustShot";
                    weaponPickupScript weaponpickup = drop.GetComponent<weaponPickupScript>();
                    weaponpickup.disappear = true;
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

        if (PlayerMovement.hasdiedforeverybody) Destroy(this.gameObject);

        if (!richoed)
        {
            if (shouldReturn && killed > 0 && lastamountkilled == 1)
            {
                richoed = true;
                StyleManager.instance.undisputed(4);
                StyleManager.instance.growStyle(1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (collision.gameObject.CompareTag("llocation"))
        {
            RLocation = GameObject.Find("RLOCATIONLOCATION").transform;
            this.transform.position = new Vector3(RLocation.position.x, this.transform.position.y, 0);
            tpsc.teleportCount++;
            shouldReturn = true;
            return;
        }
        else if (collision.gameObject.CompareTag("rlocation"))
        {
            LLocation = GameObject.Find("LLOCATIONLOCATION").transform;
            this.transform.position = new Vector3(LLocation.position.x, this.transform.position.y, 0);
            tpsc.teleportCount++;
            shouldReturn = true;
            return;
        }

        if (collision.gameObject.tag == "enemyorb" ||
            collision.gameObject.layer == 8 ||
            collision.gameObject.name == "Hand_L" ||
            collision.gameObject.name == "Hand_R" ||
            collision.gameObject.name == "Leg_L" ||
            collision.gameObject.name == "Leg_R" ||
            collision.gameObject.name == "headPivot" ||
            collision.gameObject.tag == "weaponPickup" ||
            collision.gameObject.name == "square" ||
            collision.gameObject.name == "Square" ||
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

        rb.linearVelocity = Vector2.zero;
        shouldReturn = true;
    }

    private void OnDestroy()
    {
        boomerangSound.Stop();
    }
}

