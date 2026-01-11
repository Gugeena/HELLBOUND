using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using static UnityEngine.ParticleSystem;

public class EyeballMovementScript : MonoBehaviour
{
    Rigidbody2D rb;

    Animator anim;

    SpriteRenderer sr;

    public Transform player;

    public float movespeed;

    float stopDistance = 2.5f;

    public GameObject Hitbox;

    public GameObject BlowUpParticles;

    public GameObject Glow;

    bool Dying = false;

    bool canMove;

    public GameObject DeathParticles;

    public AudioClip deathSound, explodeSound, stonedExplodeSound;
    public AudioClip[] sisini;

    public Transform LPortal;
    public Transform RPortal;

    public float lastproccessedkill = -1f;

    public float healamount = 5f;

    float distance;

    float closestPortal;

    float closestPortalPlayer;

    float distanceToPlayer;

    float distanceL;
    float distanceR;

    float distancePlayerL;
    float distancePlayerR;

    public Transform LLOCATION;
    public Transform RLLOCATION;

    float leftPortalpath;
    float righttPortalpath;
    float Directpath;

    bool charge;

    Quaternion savedRotation;
    bool lockedRotation = false;

    int teleportCount = 0;

    public GameObject weaponPickup;

    public bool stoned;

    public GameObject[] deathparticles;

    private spriteFlashScript colorFlash;

    private Coroutine destroyCoroutine;

    private bool shouldbookit = false;
    bool mowed = false, hammed = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        colorFlash = GetComponent<spriteFlashScript>();

        if(stoned)
        {
            anim.SetBool("stoned", true);
        }

        player = GameObject.FindWithTag("Player").transform;
        LPortal = GameObject.Find("LLocation").transform;
        RPortal = GameObject.Find("RLocation").transform;
        LLOCATION = GameObject.Find("LLOCATIONLOCATION").transform;
        RLLOCATION = GameObject.Find("RLOCATIONLOCATION").transform;
        //handleDistance();
        StartRotation();

        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        StartCoroutine(waiter());
        //handleDistance();
    }

    public IEnumerator waiter()
    {
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        audioManager.instance.playRandomAudio(sisini, 0.2f, 1, transform, audioManager.instance.sfx);
        StartCoroutine(distanceDetection());
    }

    void Update()
    {
        if (canMove)
        {
            if (stopDistance <= distance && !charge && !shouldbookit)
            {
                Vector2 targetPosition;
                Vector2 moveDir;

                if (Directpath <= leftPortalpath && Directpath <= righttPortalpath)
                {
                    targetPosition = new Vector2(player.position.x, player.transform.position.y);
                    moveDir = (targetPosition - (Vector2)transform.position).normalized;

                    float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    rb.linearVelocity = moveDir * movespeed;
                }
                else if (leftPortalpath <= righttPortalpath)
                {
                    targetPosition = new Vector2(LPortal.position.x, transform.position.y);
                    moveDir = (targetPosition - (Vector2)transform.position).normalized;

                    float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    rb.linearVelocity = new Vector2(moveDir.x * movespeed, rb.linearVelocity.y);
                }
                else
                {
                    targetPosition = new Vector2(RPortal.position.x, transform.position.y);
                    moveDir = (targetPosition - (Vector2)transform.position).normalized;

                    float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    rb.linearVelocity = new Vector2(moveDir.x * movespeed, rb.linearVelocity.y);
                }
            }
            else
            {
                if (!charge) chargeUp();
                rb.linearVelocity = transform.right * movespeed;
            }

            if(PlayerMovement.hasdiedforeverybody && canMove)
            {
                if (shouldbookit) return;
                shouldbookit = true;
                if (!(distance < stopDistance)) chargeUp();
                rb.linearVelocity = (player.position - transform.position) * movespeed;
            }
        }
    }

    void chargeUp()
    {
        charge = true;
        colorFlash.targetValue = 0.4f;
        colorFlash._duration = 0.4f;
        colorFlash.callFlash();
        colorFlash.targetValue = 1;
        colorFlash._duration = 0.15f;

        movespeed = 11;
        destroyCoroutine = StartCoroutine(ExplodeAfterTime());
    }

    private void FixedUpdate()
    {
    }

    private IEnumerator distanceDetection()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            handleDistance();
        }
    }

    public void StartRotation()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void handleDistance()
    {
        distance = Mathf.Abs(player.position.x - transform.position.x); // THIS > Player

        distanceL = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(LPortal.transform.position.x + 0.25f, transform.position.y)); // THIS > LPortal
        distanceR = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(RPortal.transform.position.x - 0.25f, transform.position.y)); // THIS > RPortal

        distancePlayerL = Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), new Vector2(LPortal.transform.position.x, transform.position.y)); // Player > LPortal
        distancePlayerR = Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), new Vector2(RPortal.transform.position.x, transform.position.y)); // Player > RPortal

        leftPortalpath = distanceL + Mathf.Abs(player.position.x - RLLOCATION.position.x); // This > LPortal >  Player > RPortal
        righttPortalpath = distanceR + Mathf.Abs(player.position.x - LLOCATION.position.x); // This > RPortal >  Player > LPortal
        Directpath = distance;
    }

    IEnumerator FailSafe()
    {
        yield return new WaitForSeconds(3f);
        BlowUpParticles.SetActive(true);
        Destroy(gameObject);
    }

    private IEnumerator ExplosionHitbox()
    {
        Hitbox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Hitbox.SetActive(false);
    }

    private IEnumerator DestroyTimer()
    {
        Instantiate(BlowUpParticles, transform.position, Quaternion.identity);
        Instantiate(Hitbox, transform.position, Quaternion.identity);
        AudioClip explodeAudio = stoned ? stonedExplodeSound : explodeSound;
        if(PlayerMovement.shouldMakeSound) audioManager.instance.playAudio(explodeAudio, 1f, 1, transform, audioManager.instance.sfx);
        StartCoroutine(ExplosionHitbox());
        yield return null;
        Destroy(gameObject);
    }

    private IEnumerator ExplodeAfterTime()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(DestroyTimer());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 6)
        {
            StopCoroutine(destroyCoroutine);
            StartCoroutine(DestroyTimer());
        }

        if (collision.gameObject.CompareTag("llocation"))
        {
            Vector2 vel = rb.linearVelocity;
            rb.MovePosition(new Vector2(SidePortalScript.RLocation.position.x, transform.position.y));
            rb.linearVelocity = vel;
        }
        else if (collision.gameObject.CompareTag("rlocation"))
        {
            Vector2 vel = rb.linearVelocity;
            rb.MovePosition(new Vector2(SidePortalScript.LLocation.position.x, transform.position.y));
            rb.linearVelocity = vel;
        }

        if (collision.gameObject.CompareTag("mfHitbox") || collision.gameObject.CompareTag("meleehitbox"))
        {
            death();
            RetrieveTeleportCount(collision);
            arrowScript arrowscript = collision.gameObject.GetComponent<arrowScript>();
            if (arrowscript != null)
            {
                arrowscript.increaseKillCount();
                if (arrowscript.getKillCount() > 0 && teleportCount > 0) AchivementScript.instance.UnlockAchivement("FIVE_ONE_KILLS");
            }
            else if (collision.gameObject.name.ToLower().StartsWith("explosion")) hammed = true;
        }
    }

    public void RetrieveTeleportCount(Collider2D collision)
    {
        teleportCountScript tpcs = collision.gameObject.GetComponent<teleportCountScript>();
        if (tpcs != null)
        {
            teleportCount = tpcs.teleportCount;
            if (tpcs.weapon == 1)
            {
                BoomerangWeaponScript boom = collision.gameObject.GetComponent<BoomerangWeaponScript>();
                if (!boom.inreturning)
                {
                    boom.inreturning = true;
                    boom.lastamountkilled++;
                }
                else boom.killed++;
            }
            else if (tpcs.weapon == 2)
            {
                mfScript mfscript = collision.gameObject.GetComponent<mfScript>();
                mfscript.killed++;
            }
        }
        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        string killedby = pm.getWeapon(collision.gameObject);
        string killed = stoned ? "seye" : "eye";
        if (!PlayerMovement.lastTwokilled.Contains(killed) && !PlayerMovement.lastkilledby.Contains(killedby))
        {
            PlayerMovement.lastkilledstreak++;
            pm.streaklosingstart();
            if (PlayerMovement.lastkilledstreak == 3)
            {
                AchivementScript.instance.UnlockAchivement("THREE_FOR_THREE");
            }
        }
        else
        {
            pm.StopCoroutine(pm.streaklosingtimer);
            PlayerMovement.lastkilledstreak = 0;
            PlayerMovement.lastkilledby.Clear();
            PlayerMovement.lastTwokilled.Clear();
        }
        PlayerMovement.lastTwokilled.Add(killed);
        PlayerMovement.lastkilledby.Add(killedby);
        pm.killwitheveryweapon(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "autodamaylevebeli")
        {
            death();
        }
    }

    void death()
    {
        canMove = false;
        Dying = true;
        StopAllCoroutines();
        Destroy(Glow);
        PauseScript.kill++;
        /*
        if ((int)PauseScript.kill % 15 == 0 && PauseScript.kill != 0 && (int)PauseScript.kill != lastproccessedkill)
        {
            if (healamount < 12 && healamount > 9)
            {
                healamount++;
            }
            lastproccessedkill = (int)PauseScript.kill;
        }
        */
        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
        if (PlayerMovement.shouldMakeSound) audioManager.instance.playAudio(deathSound, 0.65f, 1, transform, audioManager.instance.sfx);
        if (!stoned || stoned == null)
        {
            playerScript.hp += healamount;
            if (teleportCount <= 0 && !PlayerMovement.hasdiedforeverybody)
            {
                //printv(dist);
                if (Directpath < 7f)
                {
                    if (playerScript.hp > 0)
                    {
                        if (!playerScript.isAngelicGetter()) playerScript.hp += healamount;
                        else playerScript.hp += healamount + 2f;
                    }
                }
            }
            else if (teleportCount > 0 && !PlayerMovement.hasdiedforeverybody)
            {
                if (playerScript.hp > 0) playerScript.hp += healamount * 0.7f;
            }
            Instantiate(DeathParticles, transform.position, Quaternion.identity);
        }
        else if (stoned) Instantiate(deathparticles[Random.Range(0, deathparticles.Length)], transform.position, Quaternion.identity);
        if (playerScript.shouldGainStyle && !PlayerMovement.hasdiedforeverybody)
        {
            int growth = 1;

            if (teleportCount > 0)
            {
                growth += teleportCount;
                StyleManager.instance.undisputed(0);
            }
            
            if(playerScript.hp <= 30)
            {
                StyleManager.instance.undisputed(1);
                growth += 1;
            }

            StyleManager.instance.growStyle(growth);
        }
        if (playerScript.isAngelic) Instantiate(weaponPickup, this.gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
