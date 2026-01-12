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
                    weaponpickup.doDelay = true;
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
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("llocation"))
        {
            RLocation = GameObject.Find("RLOCATIONLOCATION").transform;
            this.transform.position = new Vector3(RLocation.position.x, this.transform.position.y, 0);
            tpsc.teleportCount++;
            shouldReturn = true;
            return;
        }
        else if (obj.CompareTag("rlocation"))
        {
            LLocation = GameObject.Find("LLOCATIONLOCATION").transform;
            this.transform.position = new Vector3(LLocation.position.x, this.transform.position.y, 0);
            tpsc.teleportCount++;
            shouldReturn = true;
            return;
        }

        if (obj.tag == "enemyorb" ||
            obj.layer == 8 ||
            obj.name == "Hand_L" ||
            obj.name == "Hand_R" ||
            obj.name == "Leg_L" ||
            obj.name == "Leg_R" ||
            obj.name == "headPivot" ||
            obj.tag == "weaponPickup" ||
            obj.name == "square" ||
            obj.name == "Square" ||
            obj.name == "Torso" ||
            obj.tag == "poison" ||
            obj.tag == "FireballP" ||
            obj.tag == "Fireball" ||
            obj.tag == "Explosion" ||
            obj.tag == "Log" ||
            obj.tag == "Crystal" ||
            obj.tag == "note")
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

