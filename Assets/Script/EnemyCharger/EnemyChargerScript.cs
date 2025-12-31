using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChargerScript : MonoBehaviour
{

    public float movespeed = 3f;
    public Transform player;
    public float stopDistance = 4f;
    public GameObject attackHitbox;
    public bool canAttack = true;
    public float detectiondistance = 8f;
    public Animator animator;
    public bool canMove = true;
    Rigidbody2D rbprint;
    float hp = 2f;
    public GameObject particles;

    [SerializeField]

    private bool isGrounded, isFalling;

    public GameObject[] weapons;
    public GameObject weaponPickup;

    public bool isdead = false;

    public AudioClip[] idleSounds, damageSounds;
    public AudioClip deathSound, attackSound;

    public Transform LPortal;
    public Transform RPortal;

    public Rigidbody2D rb;

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

    public float lastproccessedkill = -1f;

    public float healamount = 5f;
    int teleportCount = 0;
    PlayerMovement playerScript;
    bool alreadydetecting = false;

    public bool stoned;

    public GameObject[] deathparticles;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<PlayerMovement>();
        LPortal = GameObject.Find("LLocation").transform;
        RPortal = GameObject.Find("RLocation").transform;
        LLOCATION = GameObject.Find("LLOCATIONLOCATION").transform;
        RLLOCATION = GameObject.Find("RLOCATIONLOCATION").transform;
        handleDistance();
        handleFlip();
        isGrounded = true;
        animator.SetBool("isGrounded", true);
        rb = GetComponent<Rigidbody2D>();
        isGrounded = true;
        animator.SetBool("isGrounded", true);
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        playerScript = player.GetComponent<PlayerMovement>();
        canAttack = false;
        canMove = false;
        StartCoroutine(waiter());
        stopDistance = UnityEngine.Random.Range(1f, 1.41f);
    }

    public IEnumerator waiter()
    {
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        canAttack = true;
        StartCoroutine(distanceDetection());
    }

    private IEnumerator sounds()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 7f));
        float pitch = Random.Range(0.8f, 1.01f);
        audioManager.instance.playRandomAudio(idleSounds, 0.2f, pitch, transform, audioManager.instance.sfx);
        StartCoroutine(sounds());
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack) handleFlip();

        if (hp <= 0)
        {
            StartCoroutine(death());
        }

        if (distance > stopDistance)
        {
            if (canMove && !PlayerMovement.hasdiedforeverybody)
            {
                animator.SetBool("shouldRun", true);
                animator.SetBool("shouldFein", false);
                animator.SetBool("shouldAttack", false);

                Vector2 targetposition = new Vector2(player.position.x, transform.position.y);
                Vector2 moveDir = new Vector2(targetposition.x - transform.position.x, 0).normalized;

                rb.linearVelocity = new Vector2(moveDir.x * movespeed, rb.linearVelocity.y);
                if (Directpath <= leftPortalpath && Directpath <= righttPortalpath)
                {
                    targetposition = new Vector2(player.position.x, transform.position.y);
                    moveDir = new Vector2(targetposition.x - transform.position.x, 0).normalized;
                    rb.linearVelocity = new Vector2(moveDir.x * movespeed, rb.linearVelocity.y);

                    if (player.transform.position.x > this.transform.position.x)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }

                    rb.linearVelocity = new Vector2(moveDir.x * movespeed, rb.linearVelocity.y);
                }
                else if (leftPortalpath <= righttPortalpath)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    targetposition = new Vector2(LPortal.position.x, transform.position.y);
                    moveDir = new Vector2(targetposition.x - transform.position.x, 0).normalized;
                    rb.linearVelocity = new Vector2(moveDir.x * movespeed, rb.linearVelocity.y);

                    if (LPortal.position.x > this.transform.position.x)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }

                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);

                    targetposition = new Vector2(RPortal.position.x, transform.position.y);
                    moveDir = new Vector2(targetposition.x - transform.position.x, 0).normalized;
                    rb.linearVelocity = new Vector2(moveDir.x * movespeed, rb.linearVelocity.y);

                    if (RPortal.position.x > this.transform.position.x)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                }
            }
        }
        else
        {
            if (canAttack && (int)player.transform.position.y == (int)this.transform.position.y)
            {
                // && (int) player.position.y == (int) transform.position.y
                canMove = false;
                animator.SetBool("shouldRun", false);
                animator.SetBool("shouldAttack", true);
                animator.SetBool("shouldFein", false);
                StartCoroutine(attack());
            }
            else if ((int)player.transform.position.y != (int)this.transform.position.y)
            {
                animator.SetBool("shouldRun", false);
                animator.SetBool("shouldAttack", false);
                animator.SetBool("shouldFein", true);
            }
        }

        if (canAttack)
        {
            handleFlip();
        }

        if(PlayerMovement.hasdiedforeverybody && canMove)
        {
            canMove = false;
            animator.SetBool("shouldRun", false);
            animator.SetBool("shouldAttack", false);
            animator.SetBool("shouldFein", true);
        }

        animator.SetBool("isGrounded", isGrounded);
    }

    private IEnumerator distanceDetection()
    {
        yield return new WaitForFixedUpdate();
        handleDistance();
        StartCoroutine(distanceDetection());
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

    public void handleFlip()
    {
        if (Directpath < leftPortalpath && Directpath < righttPortalpath)
        {
            transform.localScale = new Vector3(1, 1, 1);
            if (player.transform.position.x > this.transform.position.x && player != null)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else if (leftPortalpath < righttPortalpath)
        {
            if (LPortal.position.x > this.transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            if (RPortal.position.x > this.transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    public IEnumerator death()
    {
        if (isdead) yield break;
        isdead = true;
        float pitch =  Random.Range(0.8f, 1.01f);
        if (PlayerMovement.shouldMakeSound) audioManager.instance.playAudio(deathSound, 0.65f, pitch, transform, audioManager.instance.sfx);
        PauseScript.kill++;
        Rigidbody2D[] rbs = GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D rbb in rbs)
        {
            rbb.bodyType = RigidbodyType2D.Dynamic;
            rbb.AddForce(transform.up * Random.Range(2f, 3f), ForceMode2D.Impulse);
            rbb.AddTorque(Random.Range(6f, 7f));
            rbb.gameObject.transform.parent = null;
            rbb.excludeLayers = rbb.excludeLayers ^ (1 << LayerMask.NameToLayer("Player"));
            rbb.excludeLayers = rbb.excludeLayers ^ (1 << LayerMask.NameToLayer("Default"));
            rbb.excludeLayers = rbb.excludeLayers ^ (1 << LayerMask.NameToLayer("Enemy"));
            Destroy(attackHitbox);
            bodyPartScript bpscript = rbb.gameObject.GetComponent<bodyPartScript>();
            if(bpscript != null)bpscript.disappear();
            BoxCollider2D bc = rbb.gameObject.GetComponent<BoxCollider2D>();
            if (bc != null) bc.isTrigger = false;
            GameObject bodypart = rbb.gameObject;
        }
        if (playerScript.shouldGainStyle && !PlayerMovement.hasdiedforeverybody)
        {
            if (teleportCount == 0) StyleManager.instance.growStyle(2);
            else if (teleportCount > 0) StyleManager.instance.growStyle(3);
        }
        canMove = false;
        canAttack = false;
        int chance = UnityEngine.Random.Range(0, 5);
        if(chance > 0) Instantiate(weaponPickup, this.gameObject.transform.position, Quaternion.identity);
        float dist = Directpath;
        if (!stoned || stoned == null)
        {
            if (teleportCount <= 0 && !PlayerMovement.hasdiedforeverybody)
            {
                PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
                //print(dist);
                if (dist < 4f)
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
            Instantiate(particles, transform.position, Quaternion.identity);
        }
        else if (stoned) Instantiate(deathparticles[Random.Range(0, deathparticles.Length)], transform.position, Quaternion.identity);
        Destroy(gameObject);
        yield break;
    }


    public void damage(int damage)
    {
        hp -= damage;
        float direction = Mathf.Sign(transform.localScale.x);
        float knockback = 4f;
        float pitch = UnityEngine.Random.Range(0.8f, 1.01f);
        audioManager.instance.playRandomAudio(damageSounds, 0.65f, pitch, transform, audioManager.instance.sfx);
        Vector2 force = new Vector2(-direction, 0);
        //StartCoroutine(hitScan());
        rb.AddForce(force * (knockback * 1000f), ForceMode2D.Impulse);
        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
    }

    public IEnumerator hitHalt()
    {
        canMove = false;
        yield return new WaitForSeconds(0.1f);
        canMove = true;
    }

    public IEnumerator attack()
    {
        //animplay
        canAttack = false;
        if (PlayerMovement.stopAttacking) yield break;
        float pitch = Random.Range(0.8f, 1.01f);
        audioManager.instance.playAudio(attackSound, 0.55f, pitch, transform, audioManager.instance.sfx);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.4f);
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        attackHitbox.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("shouldAttack", false);
        canAttack = true;
        canMove = true;
    }

    public int RetrieveTeleportCount(Collider2D collision)
    {
        int teleportCount = 0;
        string weapon = collision.gameObject.name;
        switch (weapon)
        {
            case "motherfuckr(Clone)":
                teleportCount = collision.gameObject.GetComponent<mfScript>().getteleportCount();
                break;
            case "BoomerangPrefab(Clone)":
                teleportCount = collision.gameObject.GetComponent<BoomerangWeaponScript>().getteleportCount();
                break;
            case "SpearPrefab(Clone)":
                teleportCount = collision.gameObject.GetComponent<spearScript>().getteleportCount();
                break;
            case "Arrow(Clone)":
                teleportCount = collision.gameObject.GetComponent<arrowScript>().getteleportCount();
                break;
            default: break;
        }
        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        if (collision.gameObject == null) print("yle xar");
        if (!PlayerMovement.lastkilled.Equals(this.gameObject) && !PlayerMovement.lastkilledby.Contains(collision.gameObject))
        {
            PlayerMovement.lastkilledstreak++;
            pm.streaklosingstart();
            if (PlayerMovement.lastkilledstreak == 3) AchivementScript.instance.UnlockAchivement("THREE_FOR_THREE");
        }
        else
        {
            pm.StopCoroutine(pm.streaklosingtimer);
            PlayerMovement.lastkilledstreak = 0;
            PlayerMovement.lastkilledby.Clear();
        }
        PlayerMovement.lastkilled = this.gameObject;
        PlayerMovement.lastkilledby.Add(collision.gameObject);
        pm.killwitheveryweapon(collision.gameObject);
        return teleportCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "meleehitbox")
        {
            damage(1);
        }

        if (collision.gameObject.tag == "mfHitbox")
        {
            teleportCount = RetrieveTeleportCount(collision);
            damage(2);
            if (collision.gameObject.name == "Arrow(Clone)")
            {
                arrowScript arrowscript = collision.gameObject.GetComponent<arrowScript>();
                arrowscript.increaseKillCount();
                if (arrowscript.getKillCount() >= 5 && teleportCount > 0) AchivementScript.instance.UnlockAchivement("FIVE_ONE_KILLS");
            }
        }
      
        if (collision.gameObject.CompareTag("Crystal")) StartCoroutine(death());

        if (collision.gameObject.CompareTag("poison")) StartCoroutine(death());

        if (collision.gameObject.CompareTag("Fireball")) StartCoroutine(death());

        if (collision.gameObject.CompareTag("FireballP")) StartCoroutine(death());

        if (collision.gameObject.name == "LLocation") this.transform.position = new Vector3(RLLOCATION.position.x, this.transform.position.y, 0);
        else if (collision.gameObject.name == "RLocation") this.transform.position = new Vector3(LLOCATION.position.x, this.transform.position.y, 0);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "autodamaylevebeli")
        {
            StartCoroutine(death());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3) isGrounded = true;

        if (collision.gameObject.tag == "meleehitbox") damage(1);

        if (collision.gameObject.tag == "mfHitbox") StartCoroutine(death());

        if (collision.gameObject.CompareTag("Crystal")) StartCoroutine(death());

        if (collision.gameObject.CompareTag("poison")) StartCoroutine(death());

        if (collision.gameObject.CompareTag("Fireball"))
        {
            StartCoroutine(death());
        }

        if (collision.gameObject.CompareTag("FireballP")) StartCoroutine(death());

        if (collision.gameObject.name == "LLocation") this.transform.position = new Vector3(RLLOCATION.position.x, this.transform.position.y, 0);
        else if (collision.gameObject.name == "RLocation") this.transform.position = new Vector3(LLOCATION.position.x, this.transform.position.y, 0);
    }
}
