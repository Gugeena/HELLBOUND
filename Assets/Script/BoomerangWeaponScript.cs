using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoomerangWeaponScript : MonoBehaviour
{
    bool shouldReturn = false;
    public GameObject player;
    Vector2 targetPoint;
    public GameObject blowUpParticles;
    public GameObject pickup;
    public Rigidbody2D rb;

    public Transform RLocation;
    public Transform LLocation;

    private int teleportCount = 0;

    public AudioSource boomerangSound;

    private void Start()
    {
        player = GameObject.Find("Player(Clone)");
        rb = GetComponent<Rigidbody2D>();
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "LLocation")
        {
            RLocation = GameObject.Find("RLOCATIONLOCATION").transform;
            this.transform.position = new Vector3(RLocation.position.x, this.transform.position.y, 0);
            teleportCount++;
            shouldReturn = true;
            return;
        }
        else if (collision.gameObject.name == "RLocation")
        {
            LLocation = GameObject.Find("LLOCATIONLOCATION").transform;
            this.transform.position = new Vector3(LLocation.position.x, this.transform.position.y, 0);
            teleportCount++;
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

        print("collided with: " + collision.gameObject.name + ", with tag: " + collision.gameObject.tag);
    }

    public int getteleportCount()
    {
        return teleportCount;
    }

    private void OnDestroy()
    {
        boomerangSound.Stop();
    }
}

