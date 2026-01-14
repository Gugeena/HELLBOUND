
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EnemySkyCandleScript : MonoBehaviour
{
    public float movespeed = 3f;
    public Transform player;
    public float stopDistance = 3f;
    public GameObject attackHitbox;
    public bool canAttack = true;
    public float detectiondistance = 8f;
    public Animator animator;
    public bool canMove = true;
    Rigidbody2D rb;
    float hp = 2f;
    public GameObject particles;
    public GameObject headPivot;

    public Transform location;
    public GameObject orb;

    public Animator animatorofparent;

    public GameObject glow;

    private float originalYPosition;
    public float returnToYSpeed = 2f;
    public float verticalSlowdownFactor = 0.25f;
    public float randomizedx;
    public float randomizedy;
    bool shouldrandomize = false;
    bool hasRandomized = true;

    [SerializeField]
    private AudioClip idle;

    public AudioClip deathSound;

    public Transform LPortal;
    public Transform RPortal;

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

    float attackcooldowntimer = 1f;

    public float lastproccessedkill = -1f;

    public float healamount = 5f;
    int teleportCount = 0;

    public int loopCount;
    PlayerMovement playerScript;

    public GameObject weaponPickup;

    public bool stoned;

    public GameObject[] deathparticles;

    public GameObject hitbox;

    public bool hammed = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<PlayerMovement>();
        LPortal = GameObject.Find("LLocation (1)").transform;
        RPortal = GameObject.Find("RLocation (1)").transform;
        LLOCATION = GameObject.Find("LLOCATIONLOCATION").transform;
        RLLOCATION = GameObject.Find("RLOCATIONLOCATION").transform;
        handleDistance();
        handleFlip();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb.bodyType = RigidbodyType2D.Kinematic;
        originalYPosition = transform.position.y;

        int randomizedxx = UnityEngine.Random.Range(0, 2);
        randomizedx = randomizedxx == 0 ? UnityEngine.Random.Range(1f, 5f) : UnityEngine.Random.Range(-1f, -5f);
        randomizedy = UnityEngine.Random.Range(3f, 5f);

        canMove = false;
        canAttack = false;
        StartCoroutine(waiter());
    }

    public IEnumerator waiter()
    {
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        canAttack = true;
        StartCoroutine(distanceDetection());
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0 || player == null)
        {
            StartCoroutine(death());
        }

        if (distance > stopDistance)
        {
            if (canMove)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0);
                animator.SetBool("shouldAttack", false);

                if (Directpath <= leftPortalpath && Directpath <= righttPortalpath)
                {
                    Vector2 targetposition = new Vector2(player.position.x - randomizedx, originalYPosition - randomizedy);
                    float currentY = transform.position.y;
                    float TargetY = Mathf.MoveTowards(currentY, originalYPosition, returnToYSpeed * Time.deltaTime);
                    Vector2 newposition = Vector2.MoveTowards(new Vector2(transform.position.x, currentY), new Vector2(targetposition.x, TargetY), movespeed * Time.deltaTime);

                    transform.position = newposition;

                    if (player.transform.position.x > this.transform.position.x)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                }
                else if (leftPortalpath <= righttPortalpath)
                {
                    Vector2 targetposition = new Vector2(LPortal.position.x, originalYPosition);
                    float currentY = transform.position.y;
                    float TargetY = Mathf.MoveTowards(currentY, originalYPosition, returnToYSpeed * Time.deltaTime);
                    Vector2 newposition = Vector2.MoveTowards(new Vector2(transform.position.x, currentY), new Vector2(targetposition.x, TargetY), movespeed * Time.deltaTime);

                    transform.position = newposition;

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
                    Vector2 targetposition = new Vector2(RPortal.position.x, originalYPosition);
                    float currentY = transform.position.y;
                    float TargetY = Mathf.MoveTowards(currentY, originalYPosition, returnToYSpeed * Time.deltaTime);
                    Vector2 newposition = Vector2.MoveTowards(new Vector2(transform.position.x, currentY), new Vector2(targetposition.x, TargetY), movespeed * Time.deltaTime);

                    transform.position = newposition;

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
            int minusrandomizedx = UnityEngine.Random.RandomRange(0, 2);
            Vector2 targetposition;
            if (minusrandomizedx < 0)
            {
                targetposition = new Vector2(player.position.x + randomizedx, player.position.y + randomizedy);
            }
            else
            {
                targetposition = new Vector2(player.position.x - randomizedx, player.position.y + randomizedy);
            }
            transform.position = Vector2.MoveTowards(transform.position, targetposition, (movespeed * verticalSlowdownFactor) * Time.deltaTime);

            if (canAttack)
            {
                StartCoroutine(attack());
                canAttack = false;
            }
        }

        //if (canAttack) handleFlip();
        handleFlip();
    }

    private IEnumerator distanceDetection()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            handleDistance();
        }
    }

    public void handleDistance()
    {
        distance = Mathf.Abs(player.position.x - randomizedx - transform.position.x); // THIS > Player

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
            if (player.transform.position.x > this.transform.position.x)
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

    public IEnumerator randomizertimer()
    {
        yield return new WaitForSeconds(10f);
        hasRandomized = false;
    }

    public IEnumerator attack()
    {
        if (PlayerMovement.stopAttacking) yield break;
        if ((int)PauseScript.kill % 15 == 0 && PauseScript.kill != 0 && (int)PauseScript.kill != lastproccessedkill)
        {
            if (attackcooldowntimer > 0.8f)
            {
                attackcooldowntimer -= 0.1f;
            }
            else if (attackcooldowntimer == 0.8f && attackcooldowntimer > 0.7f)
            {
                attackcooldowntimer -= 0.05f;
            }
            lastproccessedkill = (int)PauseScript.kill;
        }

        Instantiate(orb, location.position, Quaternion.identity);
        yield return new WaitForSeconds(attackcooldowntimer);
        canAttack = true;//
        canMove = true;
    }

    public void damage(int damage)
    {
        hp -= damage;
        float direction = Mathf.Sign(transform.localScale.x);
        float knockback = 4f;
        Vector2 force = new Vector2(-direction, 0);
        rb.AddForce(force * (knockback * 1000f), ForceMode2D.Impulse);
    }

    public int RetrieveTeleportCount(Collider2D collision)
    {
        int teleportCount = 0;
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
        string killed = stoned ? "scandle" : "candle";
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
        return teleportCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("meleehitbox"))
        {
            damage(1);
        }

        if (obj.CompareTag("mfHitbox"))
        {
            teleportCount = RetrieveTeleportCount(collision);
            damage(2);
            arrowScript arrowscript = obj.GetComponent<arrowScript>();
            if (arrowscript != null)
            {
                arrowscript.increaseKillCount();
                if (arrowscript.getKillCount() > 0 && teleportCount > 0) AchivementScript.instance.UnlockAchivement("FIVE_ONE_KILLS");
            }
            //else if (collision.gameObject.name.StartsWith("Explosion")) hammed = true;
        }

        if (obj.CompareTag("Crystal") || obj.CompareTag("poison") || obj.CompareTag("Fireball") || obj.CompareTag("FireballP")) StartCoroutine(death());

        //if (collision.gameObject.name.StartsWith("FirePillar")) StartCoroutine(death());

        if (obj.CompareTag("llocation1"))
        {
            this.transform.position = new Vector3(RLLOCATION.position.x + 0.4f, this.transform.position.y, 0);
            canMove = true;
        }
        else if (obj.CompareTag("rlocation1"))
        {
            this.transform.position = new Vector3(LLOCATION.position.x - 0.4f, this.transform.position.y, 0);
            canMove = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("autodamaylevebeli"))
        {
            StartCoroutine(death());
        }
    }

    public IEnumerator death()
    {
        Destroy(hitbox);
        animatorofparent.Play("NULLSTATE");
        PauseScript.kill++;
        float pitch = UnityEngine.Random.RandomRange(0.8f, 1.01f);
        if (PlayerMovement.shouldMakeSound) audioManager.instance.playAudio(deathSound, 0.8f, pitch, transform, audioManager.instance.sfx);
        Rigidbody2D[] rbs = GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D rbb in rbs)
        {
            rbb.bodyType = RigidbodyType2D.Dynamic;
            rbb.AddForce(transform.up * Random.Range(2f, 3f), ForceMode2D.Impulse);
            rbb.AddTorque(Random.Range(-1f, 1f));
            //rbb.gameObject.transform.SetParent(null, false);
            animatorofparent.enabled = false;
            rbb.gameObject.transform.parent = null;
            rbb.excludeLayers = rbb.excludeLayers ^ (1 << LayerMask.NameToLayer("Player"));
            rbb.excludeLayers = rbb.excludeLayers ^ (1 << LayerMask.NameToLayer("Default"));
            rbb.gameObject.GetComponent<bodyPartScript>().disappear();
            bodyPartScript bpscript = rbb.gameObject.GetComponent<bodyPartScript>();
            if (bpscript != null) bpscript.disappear();
            BoxCollider2D bc = rbb.gameObject.GetComponent<BoxCollider2D>();
            if (bc != null) bc.isTrigger = false;
            BoxCollider2D boxcollider = rbb.gameObject.GetComponent<BoxCollider2D>();
            boxcollider.enabled = true;
            Destroy(attackHitbox);
        }
        canMove = false;
        canAttack = false;

        if ((int)PauseScript.kill % 15 == 0 && PauseScript.kill != 0 && (int)PauseScript.kill != lastproccessedkill)
        {
            if (healamount < 12 && healamount > 9)
            {
                healamount++;
            }
            lastproccessedkill = (int)PauseScript.kill;
        }

        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();

        if (!stoned)
        {
            if (teleportCount <= 0 && !PlayerMovement.hasdiedforeverybody)
            {
                //print(dist);
                if (Directpath < 8f)
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
        if (playerScript.shouldGainStyle && !PlayerMovement.hasdiedforeverybody)
        {
            int growth = 1;
            if (teleportCount > 0)
            {
                growth += teleportCount;
                StyleManager.instance.undisputed(0);
            }

            if (playerScript.hp <= 30)
            {
                StyleManager.instance.undisputed(1);
                growth += 1;
            }

            StyleManager.instance.growStyle(growth);
        }
        if (playerScript.isAngelic) Instantiate(weaponPickup, this.gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
        yield break;
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySkyCandleScript : MonoBehaviour
{
    public float movespeed = 3f;
    public Transform player;
    public float stopDistance = 3f;
    public GameObject attackHitbox;
    public bool canAttack = true;
    public float detectiondistance = 8f;
    public Animator animator;
    public bool canMove = true;
    Rigidbody2D rb;
    float hp = 2f;
    public GameObject particles;
    public GameObject headPivot;

    public Transform location;
    public GameObject orb;

    public Animator animatorofparent;

    public GameObject glow;

    private float originalYPosition;
    public float returnToYSpeed = 2f;
    public float verticalSlowdownFactor = 0.25f;
    public float randomizedx;
    public float randomizedy;
    bool shouldrandomize = false;
    bool hasRandomized = true;

    [SerializeField]
    private AudioClip idle;

    public AudioClip deathSound;

    public Transform LPortal;
    public Transform RPortal;

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

    float attackcooldowntimer = 1f;

    public float lastproccessedkill = -1f;

    public float healamount = 5f;
    int teleportCount = 0;

    public int loopCount;
    PlayerMovement playerScript;

    public GameObject weaponPickup;

    public bool stoned;

    public GameObject[] deathparticles;

    public GameObject hitbox;
    bool mowed = false, hammed = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.GetComponent<PlayerMovement>();
        LPortal = GameObject.Find("LLocation (1)").transform;
        RPortal = GameObject.Find("RLocation (1)").transform;
        LLOCATION = GameObject.Find("LLOCATIONLOCATION").transform;
        RLLOCATION = GameObject.Find("RLOCATIONLOCATION").transform;
        //handleDistance();
        //handleFlip();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb.bodyType = RigidbodyType2D.Kinematic;
        originalYPosition = transform.position.y;

        int randomizedxx = UnityEngine.Random.Range(0, 2);
        randomizedx = randomizedxx == 0 ? UnityEngine.Random.Range(1f, 5f) : UnityEngine.Random.Range(-1f, -5f);
        randomizedy = UnityEngine.Random.Range(3f, 5f);

        canMove = false;
        canAttack = false;
        StartCoroutine(waiter());
    }

    public IEnumerator waiter()
    {
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0 || player == null)
        {
            StartCoroutine(death());
        }

        if (distance > stopDistance)
        {
            if (canMove)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0);
                //animator.SetBool("shouldAttack", false);

                if (Directpath <= leftPortalpath && Directpath <= righttPortalpath)
                {
                    Vector2 targetposition = new Vector2(player.position.x - randomizedx, originalYPosition - randomizedy);
                    float currentY = transform.position.y;
                    float TargetY = Mathf.MoveTowards(currentY, originalYPosition, returnToYSpeed * Time.deltaTime);
                    Vector2 newposition = Vector2.MoveTowards(new Vector2(transform.position.x, currentY), new Vector2(targetposition.x, TargetY), movespeed * Time.deltaTime);

                    transform.position = newposition;

                    if (player.transform.position.x > this.transform.position.x)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                }
                else if (leftPortalpath <= righttPortalpath)
                {
                    Vector2 targetposition = new Vector2(LPortal.position.x, originalYPosition);
                    float currentY = transform.position.y;
                    float TargetY = Mathf.MoveTowards(currentY, originalYPosition, returnToYSpeed * Time.deltaTime);
                    Vector2 newposition = Vector2.MoveTowards(new Vector2(transform.position.x, currentY), new Vector2(targetposition.x, TargetY), movespeed * Time.deltaTime);

                    transform.position = newposition;

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
                    Vector2 targetposition = new Vector2(RPortal.position.x, originalYPosition);
                    float currentY = transform.position.y;
                    float TargetY = Mathf.MoveTowards(currentY, originalYPosition, returnToYSpeed * Time.deltaTime);
                    Vector2 newposition = Vector2.MoveTowards(new Vector2(transform.position.x, currentY), new Vector2(targetposition.x, TargetY), movespeed * Time.deltaTime);

                    transform.position = newposition;

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
            int minusrandomizedx = UnityEngine.Random.RandomRange(0, 2);
            Vector2 targetposition;
            if (minusrandomizedx < 0)
            {
                targetposition = new Vector2(player.position.x + randomizedx, player.position.y + randomizedy);
            }
            else
            {
                targetposition = new Vector2(player.position.x - randomizedx, player.position.y + randomizedy);
            }
            transform.position = Vector2.MoveTowards(transform.position, targetposition, (movespeed * verticalSlowdownFactor) * Time.deltaTime);

            if (canAttack)
            {
                StartCoroutine(attack());
                canAttack = false;
            }
        }

        //if (canAttack) handleFlip();
        handleFlip();

        if (canMove) handleDistance();
    }


    private void FixedUpdate()
    {
    }

    public void handleDistance()
    {
        distance = Mathf.Abs(player.position.x - randomizedx - transform.position.x); // THIS > Player

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
            if (player.transform.position.x > this.transform.position.x)
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

    public IEnumerator randomizertimer()
    {
        yield return new WaitForSeconds(10f);
        hasRandomized = false;
    }

    public IEnumerator attack()
    {
        if (PlayerMovement.stopAttacking) yield break;
        if ((int)PauseScript.kill % 15 == 0 && PauseScript.kill != 0 && (int)PauseScript.kill != lastproccessedkill)
        {
            if (attackcooldowntimer > 0.8f)
            {
                attackcooldowntimer -= 0.1f;
            }
            else if (attackcooldowntimer == 0.8f && attackcooldowntimer > 0.7f)
            {
                attackcooldowntimer -= 0.05f;
            }
            lastproccessedkill = (int)PauseScript.kill;
        }

        Instantiate(orb, location.position, Quaternion.identity);
        yield return new WaitForSeconds(attackcooldowntimer);
        canAttack = true;//
        canMove = true;
    }

    public void damage(int damage)
    {
        hp -= damage;
        float direction = Mathf.Sign(transform.localScale.x);
        float knockback = 4f;
        Vector2 force = new Vector2(-direction, 0);
        rb.AddForce(force * (knockback * 1000f), ForceMode2D.Impulse);
    }

    public int RetrieveTeleportCount(Collider2D collision)
    {
        int teleportCount = 0;
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
        string killed = stoned ? "scandle" : "candle";
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
        return teleportCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("meleehitbox"))
        {
            damage(1);
        }

        if (collision.gameObject.CompareTag("mfHitbox"))
        {
            teleportCount = RetrieveTeleportCount(collision);
            damage(2);
            arrowScript arrowscript = collision.gameObject.GetComponent<arrowScript>();
            if (arrowscript != null)
            {
                arrowscript.increaseKillCount();
                if (arrowscript.getKillCount() > 0 && teleportCount > 0) AchivementScript.instance.UnlockAchivement("FIVE_ONE_KILLS");
            }
            else if (collision.gameObject.name.StartsWith("Explosion")) hammed = true;
        }

        if (collision.gameObject.CompareTag("Crystal") || collision.gameObject.CompareTag("poison") || collision.gameObject.CompareTag("Fireball") || collision.gameObject.CompareTag("FireballP")) StartCoroutine(death());

        //if (collision.gameObject.name.StartsWith("FirePillar")) StartCoroutine(death());

        if (collision.gameObject.CompareTag("llocation1"))
        {
            this.transform.position = new Vector3(RLLOCATION.position.x + 0.4f, this.transform.position.y, 0);
            canMove = true;
        }
        else if (collision.gameObject.CompareTag("rlocation1"))
        {
            this.transform.position = new Vector3(LLOCATION.position.x - 0.4f, this.transform.position.y, 0);
            canMove = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("autodamaylevebeli"))
        {
            StartCoroutine(death());
        }
    }

    public IEnumerator death()
    {
        Destroy(hitbox);
        animatorofparent.Play("NULLSTATE");
        PauseScript.kill++;
        float pitch = UnityEngine.Random.RandomRange(0.8f, 1.01f);
        if(PlayerMovement.shouldMakeSound) audioManager.instance.playAudio(deathSound, 0.8f, pitch, transform, audioManager.instance.sfx);
        Rigidbody2D[] rbs = GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D rbb in rbs)
        {
            rbb.bodyType = RigidbodyType2D.Dynamic;
            rbb.AddForce(transform.up * Random.Range(2f, 3f), ForceMode2D.Impulse);
            rbb.AddTorque(Random.Range(-1f, 1f));
            //rbb.gameObject.transform.SetParent(null, false);
            animatorofparent.enabled = false;
            rbb.gameObject.transform.parent = null;
            rbb.excludeLayers = rbb.excludeLayers ^ (1 << LayerMask.NameToLayer("Player"));
            rbb.excludeLayers = rbb.excludeLayers ^ (1 << LayerMask.NameToLayer("Default"));
            rbb.gameObject.GetComponent<bodyPartScript>().disappear();
            bodyPartScript bpscript = rbb.gameObject.GetComponent<bodyPartScript>();
            if (bpscript != null) bpscript.disappear();
            BoxCollider2D bc = rbb.gameObject.GetComponent<BoxCollider2D>();
            if (bc != null) bc.isTrigger = false;
            BoxCollider2D boxcollider = rbb.gameObject.GetComponent<BoxCollider2D>();
            boxcollider.enabled = true;
            Destroy(attackHitbox);
        }
        canMove = false;
        canAttack = false;
        /*
        if ((int)PauseScript.kill % 15 == 0 && PauseScript.kill != 0 && (int)PauseScript.kill != lastproccessedkill)
        {
            if (healamount < 12 && healamount > 9)
            {
                healamount++;
            }
            lastproccessedkill = (int)PauseScript.kill;
        }

        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();

        if (!stoned)
        {
            if (teleportCount <= 0 && !PlayerMovement.hasdiedforeverybody)
            {
                //print(dist);
                if (Directpath < 8f)
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
        if (playerScript.shouldGainStyle && !PlayerMovement.hasdiedforeverybody)
        {
            int growth = 1;
            if (teleportCount > 0)
            {
                growth += teleportCount;
                StyleManager.instance.undisputed(0);
            }

            if (playerScript.hp <= 30)
            {
                StyleManager.instance.undisputed(1);
                growth += 1;
            }

            StyleManager.instance.growStyle(growth);
        }
        if (playerScript.isAngelic) Instantiate(weaponPickup, this.gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
        yield break;
    }
}
*/