using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed, dashForce, jumpForce;
    [SerializeField]
    private Transform camFollowTransform, headPivotTransform;

    [SerializeField]
    private AnimationClip[] punchCombos;
    [SerializeField]
    private GameObject leftHand, rightHand;

    private int comboIndex = 0;
    [SerializeField]
    private float comboTime;
    private float elapsed;

    [SerializeField]
    private ParticleSystem runParticles;

    private ParticleSystem.EmissionModule rPEmitter;

    private Rigidbody2D rb;
    private Animator anim;

    private bool isRunning, isDashing, isGrounded, isFalling;
    private bool canPunch;

    public bool godMode;

    private int direction;

    public Transform RLocation;
    public Transform LLocation;

    public bool canDash = true;

    public GameObject meleehitbox;
    public float hp = 0;
    public bool invincible = false;

    bool canLose = true;

    public UnityEngine.UI.Slider hpslider;
    float hpCurVel = 0f;

    [SerializeField]
    public int currentWeapon = 0; //0 - Fists // 1 - bow // 2 - Boomerang // 3 - motherfucker // 4 - spear // 5 - Mjolnir

    [Header("Weapons")]
    [SerializeField]
    private GameObject motherfucker;
    [SerializeField]
    private GameObject motherFuckerPrefab, arrow, bowHands;
    [SerializeField]
    private GameObject boomerangPrefab;

    public GameObject mfhitbox;

    private bool hasArrow;

    bool IsJumping = false;

    public GameObject spear;

    public GameObject boomerang;

    public GameObject mjolnirPrefab;

    public GameObject spearhitbox;

    public GameObject spearPrefab;

    [SerializeField]
    private Transform mjolnirAim, mjolnirAimPivot;
    [SerializeField]
    private GameObject mjolnir;
    private bool isMjolFlying = false;
    private float mjolTime = 1f;

    public bool ukvegadavaida = false;

    public GameObject fadeOut;
    public GameObject finalfadeOut;

    public GameObject hurtparticle;

    private bool canAngel, isAngeled;
    public bool angelTransistion;

    [SerializeField]
    private Animator angelOverlayAnim;
    public KeyCode Special, AttackButton, Jump, Dash, Drop;

    [Header("SFX")]
    [SerializeField] private AudioClip death, stylemastery, deathBG;
    [SerializeField] private AudioClip mfGarwoba, firstLandSound, ascension, hitStop, revival, finaldissapearence, Beyondascension, angelicDeath, levaniylea, chainBreak;
    [SerializeField] private AudioClip[] punches, gravelWalk, jumps, lands, bowShots, mfHits, spearHits, boomerangShots, hurts, chainSounds, deaths;
    private int walkedDistance = 1;

    private bool firstLand = false;
    private camShakerScript camShake;
    [SerializeField] private LayerMask startLM;
    [SerializeField] private LayerMask endLM;

    [SerializeField] private ShakeSelfScript[] bodyPartShakes;

    [SerializeField] private GameObject spawner;

    [SerializeField] private Transform angelicTransistionTrans;
    bool isPoisoned;
    bool canBePoisoned;

    public bool isAngelic;

    bool waiting = false;

    public flashScript flashScript;
    public GameObject angelic;

    public GameObject meteor;

    public bool shouldLoseStyle;
    public bool shouldGainStyle;
    private Coroutine styleRoutine;

    public Animator fadeoutAnimator;

    private Coroutine currentLoseStyleCoroutine;

    bool justShotBoomerang = false;

    public static bool shouldEnd = false;

    [SerializeField] private float hitFlashDuration = 0.2f;

    [SerializeField] private CapsuleCollider2D gravityBox;
    [SerializeField] private BoxCollider2D legBox;

    [SerializeField] private Animator camAnimator;

    private bool isDead;

    private spriteFlashScript damageFlashScript;
    private Canvas uiCanvas;
    public bool shouldmakeAudio = true;
    public BoxCollider2D autokillcollider;

    public bool canMove = true;
    public static bool Beyonder = false;
    public static bool hasAscendedonce = false;
    public GameObject truespirit;
    public bool hasbeyonded;

    public Animator canvasAnimator, styleAnimator;

    public static bool shouldMakeSound = true;

    public bool undachaijvayleoagarshemidzlia = false;

    public static bool stopAttacking = false;
    private bool hassubscribedtolilith = false;

    public static bool hasdiedforeverybody = false;

    public static bool canPause = true;

    public bool isinbubble = false;

    public Animator hpAnimator;

    public bool wasPoisoned;

    bool isPoisonRunning = false;

    public Queue<float> poisonQueue = new Queue<float>();

    public Coroutine PoisonQueCorountine, SecondQueue;

    public static Animator TLOHLANIMATOR;

    public static bool isintenthlayer;

    public AudioClip slash;

    public bool isFuckingPoisoned = false;

    public Animator[] animatorofhands;

    [SerializeField] ParticleSystem angelDeathParticles;

    public int deathcount = 0;

    public static GameObject lastkilled;
    public static List<String> lastkilledby = new List<String>();
    public static List<String> lastTwokilled = new List<String>();
    public static List<String> weaponsUsed = new List<String>();
    public int haskilledwithnewweapon = 0;
    public bool isinkwew = false;
    public static int lastkilledstreak;
    public Coroutine streaklosingtimer;

    public TenthLayerOfHellScript tenthscript;

    public Animation charchoba;

    public static bool isInAngelTransition;

    public Coroutine resetKW;

    public static bool shouldAim;
    public bool isMfSpecialing;

    public AudioClip arrowPickup;
    AudioSource arrowPickupSource;

    public bool inTutorial = false;
    private bool doBow;
    private bool trapped = false;
    private bool bowScene = false;
    private float moveCount = 0;
    [SerializeField] private Transform headTransform;
    [SerializeField] private Animator cineAnim;
    private ShakeSelfScript shakeSelfScript;
    [SerializeField]
    private ParticleSystem chainParticle;
    [SerializeField]
    private GameObject[] shackles;

    [SerializeField] private Animator scrollAnim;

    public static PlayerMovement instance;

    private bool canOpen=true;
    public bool isFirst, tutorialLock;
    bool hasheard = false;
    bool canEnd = true;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Functions.setUpView();
        findeverythingatspawn();
        variablesetting();

        if (!isAngelic) StartCoroutine(pickUpWeapon(0, "fists"));
        else StartCoroutine(beInvincbleforawhile());
        //else LilithScript.lilithDeathEvent += EnterFinalAscension;

        rPEmitter = runParticles.emission;

        rb.excludeLayers = startLM;

        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        if (inTutorial)
        {
            isGrounded = false;

            doBow = true;
            trapped = true;
            rb.simulated = false;
            anim.SetBool("trapped", true);
            anim.Play("player_chained");
        }
    }

    void variablesetting()
    {
        if (!isAngelic)
        {
            lastkilled = this.gameObject;
            direction = 1;
            canPause = true;
        }
        else StartCoroutine(canPauseEnabler());
        isInAngelTransition = false;
        LilithScript.bossfightstarted = false;
        Time.timeScale = 1;
        hp = 150f;
        hpslider.value = 150f;
        canBePoisoned = true;
        canPunch = true;
        canLose = true;
        lastkilledby = new List<String>();
        lastTwokilled = new List<String>();
        weaponsUsed = new List<String>();
        shouldEnd = false;
        isMfSpecialing = false;
        shouldMakeSound = true;
        hasheard = false;
        hasheard = false;
        shouldmakeAudio = true;
        stopAttacking = false;
        hasdiedforeverybody = false;
        shouldAim = true;
        String scenename = SceneManager.GetActiveScene().name;
        if (scenename == "TenthLayerOfHell")
        {
            AchivementScript.instance.UnlockAchivement("ENTER_TLOH");
            isintenthlayer = true;
            if(isAngelic) tenthscript = GameObject.Find("TLOH").GetComponent<TenthLayerOfHellScript>();
        }
        else isintenthlayer = false;
    }

    public IEnumerator canPauseEnabler()
    {
        yield return new WaitForSeconds(0.6f);
        canPause = true;
    }

    void findeverythingatspawn()
    {
        Animator[] animators = GameObject.FindObjectsOfType<Animator>(true);
        foreach (Animator animator in animators)
        {
            if (animator.gameObject.name == "FadeOut (2)") fadeOut = animator.gameObject;
            if (animator.gameObject.name == "FinalFadeOut") finalfadeOut = animator.gameObject;
            if (animator.gameObject.name == "Vinigreti") TLOHLANIMATOR = animator;
        }

        Special = KeyBindManagerScript.heavyKey;
        AttackButton = KeyBindManagerScript.attackKey;
        Jump = KeyBindManagerScript.jumpKey;
        Dash = KeyBindManagerScript.dashKey;
        Drop = KeyBindManagerScript.DropKey;

        rb = GetComponent<Rigidbody2D>();
        camShake = GetComponent<camShakerScript>();
        bodyPartShakes = GetComponentsInChildren<ShakeSelfScript>();
        damageFlashScript = GetComponent<spriteFlashScript>();

        RLocation = GameObject.Find("RLOCATIONLOCATION").transform;
        LLocation = GameObject.Find("LLOCATIONLOCATION").transform;
        hpslider = GameObject.Find("HP").GetComponent<Slider>();
        anim = GetComponent<Animator>();
        if (!inTutorial)
        {
            angelicTransistionTrans = GameObject.Find("angelicTransistionPos").transform;
            autokillcollider = GameObject.Find("InstaKillBox").GetComponent<BoxCollider2D>();
            autokillcollider.enabled = false;
            canvasAnimator = GameObject.Find("Canvas (1)").GetComponent<Animator>();

        }
        flashScript = GameObject.Find("Flash").GetComponent<flashScript>();
        angelOverlayAnim = GameObject.Find("angelStatus").GetComponent<Animator>();
        camAnimator = GameObject.Find("Main Camera").GetComponent<Animator>();
        uiCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (isAngelic) StartCoroutine(meteorSpawn());

        
        hpAnimator = GameObject.Find("HP").GetComponent<Animator>();
        styleAnimator = GameObject.Find("styleAnim").GetComponent<Animator>();

        shakeSelfScript = GetComponent<ShakeSelfScript>();
    }

    IEnumerator meteorSpawn()
    {
        yield return new WaitForSeconds(1.5f);
        camShake.time = -1f;
        StartCoroutine(camShake.shake());
        LilithMeteorScript meteorScript = GameObject.FindObjectOfType<LilithMeteorScript>(true);
        meteor = meteorScript.gameObject;
        meteor.SetActive(true);
        yield break;
    }

    public bool isAngelicGetter()
    {
        return isAngelic;
    }

    // Update is called once per frame
    void Update()
    {
        //if (PauseScript.Paused || isDead) return;
        /*
        if (LilithScript.bossfightstarted && !hassubscribedtolilith)
        {
            hassubscribedtolilith = true;
            LilithScript.lilithDeathEvent += EnterFinalAscension;
            print("entered");
        }
        */

        if (shouldEnd && !ukvegadavaida)
        {
            StartCoroutine(gadasvla(5));
        }

        if (angelTransistion)
        {
            transform.position = Vector2.MoveTowards(transform.position, angelicTransistionTrans.position, ((Time.unscaledDeltaTime * 10f) * Vector2.Distance(transform.position, angelicTransistionTrans.position)));
            return;
        }

        if (!hasbeyonded)
        {
            if (!godMode) handleHP();
            if (trapped == false && bowScene == false)
            {
                handleMovement();
                handleCombat();
            }
            else if (trapped == true) { handleChained(); }
            if (currentWeapon == 1 && !bowScene) bowAim();
            if (currentWeapon == 5) mjolAim();
            if (isMjolFlying)
            {
                mjolnirFly();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isRunning && !IsJumping) walkedDistance++;

        if (walkedDistance % 15 == 0 && isGrounded)
        {
            audioManager.instance.playRandomAudio(gravelWalk, 0.75f, 1, transform, audioManager.instance.sfx);
            walkedDistance++;
        }
    }

    public static void TLOHLFADEOUTANDINNER(int decision)
    {
        if (TLOHLANIMATOR == null) return;
        if (decision == 1) TLOHLANIMATOR.Play("TLOHLFADEOUT");
        else if (decision == 2) TLOHLANIMATOR.Play("TLOHFADEIN");
    }

    void handleMovement()
    {
        //if(ScrollScript.isIn)
        //{
           /// rb.angularVelocity = 0;
        ///    return;
        //}

        if (canMove && !PauseScript.Paused && !ScrollScript.isIn)
        {
            if (Input.GetKeyDown(Drop) && currentWeapon != 0)
            {
                StartCoroutine(pickUpWeapon(0, "fists"));
            }

            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            if (!isDashing) rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);

            isRunning = x != 0;

            anim.SetBool("isWalking", isRunning);
            if (isRunning)
            {
                if (currentWeapon == 0 || currentWeapon == 3)
                {
                    anim.SetLayerWeight(2, 0.5f);
                }

                if (!runParticles.isPlaying) rPEmitter.enabled = true;

                if (isGrounded)
                {
                    var emission = runParticles.emission;
                    emission.enabled = true;
                }
                else
                {
                    var emission = runParticles.emission;
                    emission.enabled = false;
                }
            }
            else
            {
                anim.SetLayerWeight(2, 1f);
                var emission = runParticles.emission;
                emission.enabled = false;
            }
            if (isFalling)
            {
                rb.gravityScale = 3f;
            }
            else
            {
                rb.gravityScale = 1f;
            }

            if (!isDashing && !PauseScript.Paused)
            {
                if (x < 0 && transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                    direction = -1;
                }
                else if (x > 0 && transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    direction = 1;
                }
            }

            if (Input.GetKeyDown(Dash) && !isDashing && canDash)
            {
                canDash = false;
                StartCoroutine(dash());
            }

            if (Input.GetKeyDown(Jump) && isGrounded && !IsJumping)
            {
                StartCoroutine(jump());
            }

            isFalling = rb.linearVelocity.y < -0.1f;
            anim.SetBool("isFalling", isFalling);
            anim.SetBool("isJumping", !isGrounded);
        }
    }

    void handleChained()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 pos = Vector2.Lerp(mousePos, headPivotTransform.position, 0.992f);
        pos.z = 0;
        headTransform.position = pos;

        Vector3 aimDirection = (mousePos - transform.position).normalized;

        float angle = (Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg);

        headTransform.eulerAngles = new Vector3(0, 0, angle);


        float x = mousePos.x - transform.position.x;
        if (x < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            direction = -1;
        }
        else if (x > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            direction = 1;
        }

        if (Input.anyKeyDown && moveCount < 5)
        {
            StartCoroutine(escape());
        }
        else if (Input.anyKeyDown && moveCount >= 5)
        {
            StartCoroutine(breakChains());
        }

    }
    private IEnumerator bowCutscene()
    {
        rb.linearVelocity = Vector2.zero;
        isRunning = false;
        isDashing = false;
        isFalling = false;
        anim.SetBool("isWalking", false);
        anim.SetBool("isFalling", false);
        anim.SetBool("isJumping", false);

        canPunch = false;
        anim.SetBool("isWalking", false);
        //anim.Play("player_idle", 0);

        anim.SetLayerWeight(2, 1);
        anim.Play("player_bowShoot");
        yield return new WaitForSeconds(0.4f);
        audioManager.instance.playRandomAudio(bowShots, 0.5f, 1, transform, audioManager.instance.sfx);
        Time.timeScale = 0.4f;
        var emission = runParticles.emission;
        emission.enabled = false;
        GameObject arr = Instantiate(arrow, bowHands.transform.position, bowHands.transform.rotation);
        arr.GetComponent<Rigidbody2D>().gravityScale = 0.2f;
        arr.GetComponent<Rigidbody2D>().AddForce(arr.transform.right * 400 * direction);
        canPunch = true;
        yield return new WaitForSeconds(1.3f);
        Time.timeScale = 1;
        bowScene = false;
    }


    private IEnumerator escape()
    {
        shakeSelfScript.Begin();
        audioManager.instance.playRandomAudio(chainSounds, 0.6f, 1, transform, audioManager.instance.sfx);
        yield return new WaitForSeconds(0.23f);
        shakeSelfScript.stopShake();
        moveCount++;
    }

    private IEnumerator breakChains()
    {
        canMove = false;
        canPunch = false;
        rb.simulated = true;
        trapped = false;
        anim.SetBool("trapped", false);
        chainParticle.gameObject.SetActive(true);
        chainParticle.Play();
        audioManager.instance.playAudio(chainBreak, 0.8f, 1, transform, audioManager.instance.sfx);
        foreach (GameObject g in shackles)
        {
            Rigidbody2D rb = g.GetComponent<Rigidbody2D>();
            rb.simulated = true;
            float randomX = UnityEngine.Random.Range(-5f, 5f);
            float randomY = UnityEngine.Random.Range(0f, 10f);
            Vector2 random = new Vector2(randomX, randomY);
            rb.AddForce(random * 10);
            rb.AddTorque(UnityEngine.Random.Range(-5f, 5f) * 15);
        }
        Time.timeScale = 0.2f;
        cineAnim.Play("cinecam_zoomin");
        yield return null;
    }


    public void streaklosingstart()
    {
        if (streaklosingtimer != null) StopCoroutine(streaklosingtimer);

        streaklosingtimer = StartCoroutine(streaklosing());    
    }

    IEnumerator streaklosing()
    {
        yield return new WaitForSeconds(3f);
        lastkilledstreak = 0;
        lastkilledby.Clear();
        lastTwokilled.Clear();
        streaklosingtimer = null;
        yield break;
    }

    void handleCombat()
    {
        if(isDead) return;

        if (Input.GetKeyDown(AttackButton) && canPunch)
        {
            if (currentWeapon == 0) StartCoroutine(punch());
            else if (currentWeapon == 3) StartCoroutine(mfAttack());
            else if (currentWeapon == 4) StartCoroutine(spearAttack());
            else if (currentWeapon == 2) StartCoroutine(boomerangAttack());
            else if (currentWeapon == 1 && hasArrow) StartCoroutine(bowShoot());
            else if (currentWeapon == 5) StartCoroutine(mjolAttack());
        }

        if (Input.GetKeyDown(Special) && canPunch)
        {
            if (currentWeapon == 3 && isGrounded) StartCoroutine(mfSpecial());
            if (currentWeapon == 4) StartCoroutine(spearspecialAttack());
        }

        /* 
         if (Input.GetKeyDown(KeyCode.G)) godMode = true;
         if (Input.GetKeyDown(KeyCode.H)) StartCoroutine(enterAngelic(false));
         if (Input.GetKeyDown(KeyCode.J)) StartCoroutine(pickUpWeapon(UnityEngine.Random.RandomRange(0, 5), "random"));
         */
        /*
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(deathCRT());
            //StartCoroutine(gadasvla(5));
            //StartCoroutine(pickUpWeapon(3, "fists"));
        }
        */
        /*
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(enterAngelic(false));
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(enterAngelic(true));
        }
        */

        if (StyleManager.canAscend && !isAngelic && !isDead)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(enterAngelic(false));
            }
        }

        if ((elapsed >= comboTime) || (comboIndex >= punchCombos.Length) || isRunning)
        {
            comboIndex = 0;
        }
        if (comboIndex > 0) elapsed += Time.deltaTime;
    }

    public void readyAngelic()
    {
        angelOverlayAnim.Play("angelReady");
        canAngel = true;
    }


    private void OnEnable()
    {
        LilithScript.lilithDeathEvent += EnterFinalAscension;
    }

    private void OnDisable()
    {
        LilithScript.lilithDeathEvent -= EnterFinalAscension;
    }

    public void EnterFinalAscension()
    {
        StartCoroutine(StartFinalAscension());
    }

    public IEnumerator StartFinalAscension()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(enterAngelic(true));
    }

    private IEnumerator enterAngelic(bool Beyonder)
    {
        if (isDead || PauseScript.Paused) yield break;
        canPause = false;
        runParticles.Stop();
        if (PoisonQueCorountine != null) StopCoroutine(PoisonQueCorountine);
        poisonQueue = new Queue<float>();
        isPoisoned = false;
        isPoisonRunning = false;
        TLOHLFADEOUTANDINNER(1);
        if (isintenthlayer) TenthLayerOfHellScript.shouldturnoffforawhile = true;
        isGrounded = false;
        hasAscendedonce = true;
        canPause = false;
        invincible = true;
        canLose = false;
        if (spawner != null) spawner.SetActive(false);
        if (!Beyonder)
        {
            if (isAngelic) yield break;
            //spawner.SetActive(false);
        }
        else
        {
            if (!isintenthlayer) shouldMakeSound = false;
            hasbeyonded = true;
        }
        StyleManager.shouldTurnOff = true;
        angelTransistion = true;
        unreadyAngelic();
        rb.linearVelocity = Vector3.zero;
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;

        Vector2 spawnLocation;
        string animtoplay;
        GameObject tospawn;
        if (Beyonder == true)
        {
            if (!isintenthlayer)
            {
                animtoplay = "player_finalangeltransition";
                hasAscendedonce = false;
                //StartCoroutine(UIfadeOut());
            }
            else animtoplay = "player_tenthlayerofhelltransition";
            tospawn = truespirit;
        }
        else
        {
            animtoplay = "player_angelicShaking";
            tospawn = angelic;
        }

        //transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        //direction = 1;

        damageFlashScript.forceDefault();
        isInAngelTransition = true;
        anim.Play(animtoplay);

        audioManager.instance.stopMusic();

        if (isintenthlayer && Beyonder)
        {
            audioManager.instance.playAudio(levaniylea, 0.65f, 1, transform, audioManager.instance.sfx);
            autokillcollider.enabled = true;
            stopAttacking = true;
            rb.gravityScale = 0f;
            yield return new WaitForSeconds(1.64f);
            anim.enabled = false;
            Rigidbody2D[] rbs = GetComponentsInChildren<Rigidbody2D>();
            flashScript.CallRedFlash();
            foreach (Rigidbody2D rbb in rbs)
            {
                rbb.bodyType = RigidbodyType2D.Dynamic;
                rbb.AddForce(transform.up * UnityEngine.Random.Range(2f, 3f), ForceMode2D.Impulse);
                rbb.AddTorque(UnityEngine.Random.Range(6f, 7f));
                rbb.gameObject.transform.parent = null;
                bodyPartScript bpscript = rbb.gameObject.GetComponent<bodyPartScript>();
                if (bpscript != null) bpscript.disappear();
                BoxCollider2D bc = rbb.gameObject.GetComponent<BoxCollider2D>();
                if (bc != null) bc.isTrigger = false;
                GameObject bodypart = rb.gameObject;
                Vector2 direction = UnityEngine.Random.Range(-1, 1) * rbb.transform.right;
                rbb.AddForce(200 * direction);
            }
            audioManager.instance.playAudio(slash, 1, 1, this.transform, audioManager.instance.sfx);
            Instantiate(hurtparticle, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(2f);
            fadeOut.SetActive(true);
            AchivementScript.instance.UnlockAchivement("BEAT_TLOH");
            if(deathcount == 0) AchivementScript.instance.UnlockAchivement("NO_DEATH_TLOH");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(1);
        }

        if (!Beyonder) audioManager.instance.playAudio(ascension, 0.65f, 1, transform, audioManager.instance.sfx);
        else audioManager.instance.playAudio(Beyondascension, 0.65f, 1, transform, audioManager.instance.sfx);
        //audioManager.instance.playAudio(ascension, 0.65f, 1, transform, audioManager.instance.sfx);
        foreach (ShakeSelfScript s in bodyPartShakes)
        {
            s.Begin();
        }

        Time.timeScale = 0.1f;
        rb.gravityScale = 0.0f;
        if (!Beyonder) yield return new WaitForSecondsRealtime(1.5f);
        if (Beyonder)
        {
            yield return new WaitForSeconds(0.5f);
            autokillcollider.enabled = true;
            //yield return new WaitForSecondsRealtime(1f);
        }
        foreach (ShakeSelfScript s in bodyPartShakes)
        {
            s.stopShake();
        }

        if (!Beyonder)
        {
            StartCoroutine(camShake.shake());
            yield return new WaitForSecondsRealtime(0.35f);
        }
        Time.timeScale = 1;
        if (!Beyonder)
        {
            flashScript.CallFlash();
            yield return new WaitForSecondsRealtime(0.05f);
        }

        anim.updateMode = AnimatorUpdateMode.Normal;
        angelTransistion = false;

        Vector2 center = new Vector2(25.43f, 0.7f);
        if (Beyonder) spawnLocation = center;
        else spawnLocation = this.transform.position;
        GameObject spawned = Instantiate(tospawn, spawnLocation, Quaternion.identity);
        GameObject TLOH = null;

        if (isintenthlayer)
        {
            TLOH = GameObject.Find("TLOH");
            if (TLOH != null) TLOH.SetActive(false);
            LogScript[] LogScripts = GameObject.FindObjectsOfType<LogScript>(true);
            foreach (LogScript script in LogScripts)
            {
                Destroy(script.gameObject);
            }
            greenposionballScript[] poisonscripts = GameObject.FindObjectsOfType<greenposionballScript>(true);
            foreach (greenposionballScript script in poisonscripts)
            {
                Destroy(script.gameObject);
            }
        }

        if (spawned.CompareTag("Player"))
        {
            PlayerMovement playerMovement = spawned.GetComponent<PlayerMovement>();
            StartCoroutine(playerMovement.pickUpWeapon(currentWeapon, "weapon"));
            spawned.transform.localScale = new Vector3(this.transform.localScale.x, 1, 1);
            playerMovement.direction = spawned.transform.localScale.x < 0 ? -1 : 1;
            playerMovement.deathcount = this.deathcount;
        }

        if (Beyonder)
        {
            yield return new WaitForSeconds(1.5f);
            audioManager.instance.startEnd();
            yield return new WaitForSeconds(6f);
            canvasAnimator.Play("credits");
            camShake.Stop();
            camAnimator.gameObject.transform.position = new Vector2(25.43f, 0.7f);
            yield return new WaitForSeconds(22f);
            //finalfadeOut.SetActive(true);
            audioManager.instance.playAudio(finaldissapearence, 1, 1, transform, audioManager.instance.sfx);
            yield return new WaitForSeconds(2f);
            audioManager.instance.stopEnd();
            yield return new WaitForSeconds(2f);
            GlobalSettings globalsettings = SaveSystem.Load();
            globalsettings.information.hasbeatthegame = 1;
            SaveSystem.Save(globalsettings);
            SceneManager.LoadScene(1);
            //audioManager.instance.playAudio(finaldissapearence, 1, 1, transform, audioManager.instance.sfx);
        }
        else invincible = false;

        if (isintenthlayer)
        {
            if (TLOH != null)
            {
                TLOH.SetActive(true);
                TenthLayerOfHellScript tlohscript = TLOH.GetComponent<TenthLayerOfHellScript>();
                tlohscript.alreadyin = false;
            }
        }

        if(Equals(this,spawned))
        {
            if(this.transform.localScale.x < 0)
            {
                float currentRotation = this.bowHands.transform.rotation.z;
                Transform tranBowHands = spawned.gameObject.transform.GetChild(2);
                GameObject bowHands = tranBowHands.gameObject;
                bowHands.transform.rotation = Quaternion.Euler(0,0,currentRotation);
                Time.timeScale = 0;
            }
        }

        isInAngelTransition = false;

        if (isintenthlayer) Destroy(gameObject);
        if (!isintenthlayer && !Beyonder) Destroy(gameObject);

        AchivementScript.instance.UnlockAchivement("MADE_IN_HEAVEN");
    }

    public void unreadyAngelic()
    {
        angelOverlayAnim.Play("angelUnready");
        canAngel = false;
    }

    /*
    public IEnumerator UIfadeOut()
    {
        yield return new WaitForSeconds(0.25f);
        styleAnimator.enabled = true;
        hpAnimator.enabled = true;
        hpAnimator.Play("HPFadeOut");
        styleAnimator.Play("FadeOutStyle");
    }
    */

    public IEnumerator spearAttack()
    {
        canPunch = false;
        anim.Play("player_spearattack");
        yield return new WaitForSeconds(0.3f);
        float pitch = UnityEngine.Random.Range(0.8f, 1.01f);
        audioManager.instance.playRandomAudio(spearHits, 1, pitch, transform, audioManager.instance.sfx);
        spearhitbox.SetActive(true);
        yield return new WaitForSeconds(0.12f);
        spearhitbox.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        canPunch = true;
    }

    public IEnumerator spearspecialAttack()
    {
        canPunch = false;
        //anim.Play("player_spearspecialattack");
        //yield return new WaitForSeconds(1.2f);
        Transform temp = transform.GetChild(3).GetChild(2);
        Vector2 mfPos = temp.position;
        Quaternion mfRot = temp.rotation;
        GameObject m = Instantiate(spearPrefab, mfPos, mfRot);
        spear.SetActive(false);
        currentWeapon = 0;
        canPunch = true;
        anim.SetBool("shouldChargeIn", false);
        yield return null;
    }

    public IEnumerator boomerangAttack()
    {
        //anim.Play("player_mf_special");
        //yield return new WaitForSeconds(1.2f);
        justShotBoomerang = true;
        Transform temp = transform.GetChild(3).GetChild(3);
        Vector2 mfPos = temp.position;
        Quaternion mfRot = temp.rotation;
        GameObject m = Instantiate(boomerangPrefab, mfPos, mfRot);
        Rigidbody2D mrb = m.GetComponent<Rigidbody2D>();


        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // gamoitvlis in world space sad aris mouse
        Vector2 dir = mousepos - mfPos; // gvadzlevs directions -1;0 for left da egeti shit boomerangidan mausamde
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // radianebidan gadaaq degreeshi
        float pitch = UnityEngine.Random.Range(0.8f, 1.01f);
        m.GetComponent<BoomerangWeaponScript>().boomerangSound = audioManager.instance.playRandomAudio(boomerangShots, 1, pitch, transform, audioManager.instance.sfx);
        mrb.linearVelocity = dir.normalized * 15f;

        boomerang.SetActive(false);
        currentWeapon = 0;
        yield break;
    }

    public IEnumerator mjolnirAttack()
    {
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousepos - (Vector2)transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        anim.Play("player_mjolnirattack");
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        rb.gravityScale = 0f;
        rb.linearVelocity = transform.up * 15f;
        Debug.Log("Direction: " + dir + ", Velocity: " + rb.linearVelocity);
        yield break;
    }

    public IEnumerator gadasvla(int scene)
    {
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(scene);
    }

    public void handleHP()
    {
        if (isPoisoned)
        {
            //if(!isPoisonRunning) hpAnimator.Play("PlayerPoisoning");
            if (canBePoisoned)
            {
                damageFlashScript.changeColor(false);
                damageFlashScript.callFlash(); 
                StartCoroutine(damage(5, 0, true));
                float seconds = isinbubble ? 0.3f : 1; StartCoroutine(posionCure(seconds));
            }
            else if (wasPoisoned) 
            {
                //if (!isPoisonRunning) hpAnimator.Play("PlayerDepoisoning");
            }
        }

        hp = Mathf.Clamp(hp, 0, 150);
        float currHP = Mathf.SmoothDamp(hpslider.value, hp, ref hpCurVel, 0.2f);
        hpslider.value = currHP;

        if (hp < 150 && canLose)
        {
            canLose = false;
        }
    }
    private IEnumerator punch()
    {
        canPunch = false;
        audioManager.instance.playRandomAudio(punches, 1, 1, transform, audioManager.instance.sfx);
        if (!isAngelic) anim.speed = 1f;
        else anim.speed = 2f;
        anim.Play(punchCombos[comboIndex].name);
        meleehitbox.SetActive(true);
        yield return new WaitForSeconds(0.12f);
        meleehitbox.SetActive(false);
        comboIndex++;
        elapsed = 0;
        if (!isAngelic) yield return new WaitForSeconds(0.4f);
        else yield return new WaitForSeconds(0.2f);
        anim.speed = 1f;
        canPunch = true;
    }

    private IEnumerator mjolAttack()
    {
        anim.Play("player_mjolnirAttack");

        yield return new WaitForSeconds(0.65f);
        mjolTime = 1;
        isMjolFlying = true;
        anim.SetBool("isMjolFlying", true);
    }

    private IEnumerator beInvincbleforawhile()
    {
        invincible = true;
        yield return new WaitForSeconds(1f);
        invincible = false;
    }

    private IEnumerator mfAttack()
    {
        canPunch = false;
        anim.Play("player_mf_attack");
        yield return new WaitForSeconds(0.4f);
        float pitch = UnityEngine.Random.Range(0.8f, 1.01f);
        audioManager.instance.playRandomAudio(mfHits, 0.75f, pitch, transform, audioManager.instance.sfx);
        mfhitbox.SetActive(true);
        yield return new WaitForSeconds(0.12f);
        mfhitbox.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        canPunch = true;
    }

    private IEnumerator mfSpecial()
    {
        canPunch = false;
        anim.Play("player_mf_special");
        isMfSpecialing = true;
        yield return new WaitForSeconds(1.2f);
        Transform temp = transform.GetChild(3).GetChild(1);
        Vector2 mfPos = temp.position;
        Quaternion mfRot = temp.rotation;
        if (isRunning)
        {
            if (this.transform.localScale.x < 0) mfRot = Quaternion.Euler(0f, 0f, 160f);
            else mfRot = Quaternion.Euler(0f, 0f, -155f);
        }
        GameObject m = Instantiate(motherFuckerPrefab, mfPos, mfRot);
        mfScript mfScript = m.GetComponent<mfScript>();
        mfScript.mfSound = audioManager.instance.playAudio(mfGarwoba, 1f, 1, transform, audioManager.instance.sfx);
        mfScript.direction = direction;
        motherfucker.SetActive(false);

        isMfSpecialing = false;
        currentWeapon = 0;
        canPunch = true;
    }

    private IEnumerator bowShoot()
    {
        canPunch = false;
        //anim.Play("player_bowShoot");
        anim.Play("player_bowShoot");
        yield return new WaitForSeconds(0.4f);
        hasArrow = false;
        GameObject arr = Instantiate(arrow, bowHands.transform.position, bowHands.transform.rotation);
        float pitch = UnityEngine.Random.Range(0.8f, 1.01f);
        audioManager.instance.playRandomAudio(bowShots, 0.5f, pitch, transform, audioManager.instance.sfx);
        arr.GetComponent<Rigidbody2D>().AddForce(arr.transform.right * 200 * direction);
        canPunch = true;
    }

    private void bowAim()
    {
        if (PauseScript.Paused || !shouldAim) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDirection = (mousePos - transform.position).normalized * direction;

        float angle = (Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg);

        bowHands.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void mjolAim()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(transform.position, mousePos) > 0.25f)
        {
            Vector3 aimDirection = (mousePos - transform.position).normalized * direction;

            float angle = (Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg);

            mjolnirAimPivot.eulerAngles = new Vector3(0, 0, angle);
        }

    }

    private void mjolnirFly()
    {
        rb.linearVelocity = mjolnirAimPivot.right * 17 * mjolTime * direction;

        //Vector2 dir = rb.velocity;

        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (mjolTime <= 0)
        {
            isMjolFlying = false;
            anim.SetBool("isMjolFlying", false);
            StopCoroutine(mjolnirDecay());
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (mjolTime == 1) StartCoroutine(mjolnirDecay());
    }

    private IEnumerator mjolnirDecay()
    {
        yield return new WaitForSeconds(0.5f);
        mjolTime -= 0.001f;
        StartCoroutine(mjolnirDecay());
    }

    private IEnumerator dash()
    {
        isDashing = true;
        rb.gravityScale = 0f;
        if (!isGrounded)
        {
            rb.AddForce(transform.right * direction * (dashForce - 200));
        }
        else
        {
            rb.AddForce(transform.right * direction * dashForce);
        }
        if (currentWeapon != 4) anim.Play("player_dash");
        else anim.Play("player_speardash");
        yield return new WaitForSeconds(0.3f);
        isDashing = false;
        rb.gravityScale = 1f;
        StartCoroutine(dashCooldown());
        yield break;
    }

    private IEnumerator jump()
    {
        IsJumping = true;
        rb.AddForce(transform.up * jumpForce);
        audioManager.instance.playRandomAudio(jumps, 0.7f, 1, transform, audioManager.instance.sfx);
        isGrounded = false;
        yield return null;
        IsJumping = false;
    }

    private IEnumerator pickUpWeapon(int id, String name)
    {
        if (isMfSpecialing) yield break;
        currentWeapon = id;
        if (anim == null) anim = GetComponent<Animator>();
        if (id == 0)
        {
            motherfucker.SetActive(false);
            spear.SetActive(false);
            boomerang.SetActive(false);
            mjolnir.SetActive(false);
            bowHands.SetActive(false);
            leftHand.SetActive(true);
            rightHand.SetActive(true);
            anim.SetBool("shouldChargeIn", false);
        }
        else if (id == 1)
        {
            hasArrow = true;
            bowHands.SetActive(true);
            leftHand.SetActive(false);
            rightHand.SetActive(false);
            mjolnir.SetActive(false);
            anim.SetBool("shouldChargeIn", false);

            if (doBow)
            {
                bowScene = true;
                doBow = false;
                StartCoroutine(bowCutscene());
            }
        }
        else if (id == 2)
        {
            boomerang.SetActive(true);

            motherfucker.SetActive(false);
            spear.SetActive(false);
            mjolnir.SetActive(false);

            bowHands.SetActive(false);
            leftHand.SetActive(true);
            rightHand.SetActive(true);
            anim.SetBool("shouldChargeIn", false);
        }
        else if (id == 3)
        {
            motherfucker.SetActive(true);

            spear.SetActive(false);
            boomerang.SetActive(false);
            mjolnir.SetActive(false);
            anim.SetBool("shouldChargeIn", false);
            bowHands.SetActive(false);
            leftHand.SetActive(true);
            rightHand.SetActive(true);
        }
        else if (id == 4)
        {
            spear.SetActive(true);
            motherfucker.SetActive(false);
            boomerang.SetActive(false);
            mjolnir.SetActive(false);
            anim = GetComponent<Animator>();
            anim.SetBool("shouldChargeIn", true);

            bowHands.SetActive(false);
            leftHand.SetActive(true);
            rightHand.SetActive(true);

        }
        else if (id == 5)
        {
            justShotBoomerang = false;
            spear.SetActive(false);
            motherfucker.SetActive(false);
            boomerang.SetActive(false);

            anim.SetBool("shouldChargeIn", false);

            bowHands.SetActive(false);
            leftHand.SetActive(true);
            rightHand.SetActive(true);

            mjolnir.SetActive(true);
        }
        if (name.Equals("BoomerangWhichYouJustShot"))
        {
            yield break;
        }
        if (currentLoseStyleCoroutine != null)
        {
            StopCoroutine(currentLoseStyleCoroutine);
        }
        currentLoseStyleCoroutine = StartCoroutine(loseStyle(id));
        yield return null;
    }

    IEnumerator loseStyle(int currentID)
    {
        shouldGainStyle = true;
        shouldLoseStyle = false;

        float timer = 0f;

        while (timer < 3f)
        {
            if (currentID != currentWeapon && currentID != 2 && currentWeapon != 0 && !justShotBoomerang)
            {
                shouldGainStyle = true;
                shouldLoseStyle = false;
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
        shouldGainStyle = false;
        timer = 0f;
        while (timer < 3f)
        {
            if (currentID != currentWeapon && currentID != 2 && currentWeapon != 0 && !justShotBoomerang)
            {
                shouldGainStyle = true;
                shouldLoseStyle = false;
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
        shouldLoseStyle = true;
        yield break;
    }

    public void killwitheveryweapon(GameObject weapon)
    {
        string weaponName = getWeapon(weapon);
        if (!weaponsUsed.Contains(weaponName) && weaponName != "0") weaponsUsed.Add(weaponName);
       
        if(weaponsUsed.Contains(weaponName) && resetKW != null)
        {
            StopCoroutine(resetKW);
            isinkwew = false;
        }    
        if(!isinkwew) resetKW = StartCoroutine(resetkwew());
        if (weaponsUsed.Count == 4) AchivementScript.instance.UnlockAchivement("MAL_ARSENAL");
    }

    public String getWeapon(GameObject weapon)
    {
        string name = weapon.name.ToLower();
        string weapon1;

        if (name.StartsWith("motherfuckr") || name.StartsWith("mf")) return weapon1 = "mf";
        else if (name.StartsWith("spear") || name.StartsWith("explosion")) return weapon1 = "spear";
        else if (name.StartsWith("boomerang")) return weapon1 = "boomerang";
        else if (name.StartsWith("arrow")) return weapon1 = "bow";
        else return weapon1 = "0";
    }

    public IEnumerator resetkwew()
    {
        isinkwew = true;
        yield return new WaitForSeconds(5f);
        weaponsUsed.Clear();
        isinkwew = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "LLocation")
        {
            this.transform.position = new Vector3(RLocation.position.x, this.transform.position.y, 0);
            if (isDashing)
            {
                //direction = -1;
                StartCoroutine(dash());
            }
        }
        else if (collision.gameObject.name == "RLocation")
        {
            this.transform.position = new Vector3(LLocation.position.x, this.transform.position.y, 0);
            if (isDashing)
            {
                //direction = -1;
                StartCoroutine(dash());
            }
        }

        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 8)
        {
            isGrounded = true;
            audioManager.instance.playRandomAudio(lands, 1f, 1, transform, audioManager.instance.sfx);
            if(isFirst)
            {
                isFirst = false;
                canPunch = true;
                canMove = true;
            }
        }

        if (collision.gameObject.layer == 3 && !firstLand && !inTutorial)
        {
            firstLand = true;
            rb.excludeLayers = endLM;
            StartCoroutine(camShake.shake());
            if (!isAngelic) audioManager.instance.playAudio(firstLandSound, 1, 1, transform, audioManager.instance.sfx);
        }

        if (collision.gameObject.layer == 3 && Time.timeScale < 1 && inTutorial)
        {
            Time.timeScale = 1;
            cineAnim.Play("cinecam_zoomout");
        }
    }

    public IEnumerator dashCooldown()
    {
        yield return new WaitForSeconds(0.8f);
        canDash = true;
    }

    public IEnumerator damage(int damage, float duration, bool poisoned)
    {
        invincible = true;
        
        //hasdiedforeverybody = true;
        //isPoisoned = false;
        PauseScript.dmg += damage;
        invincible = true;
        hp -= damage;
        
        if (hp <= 0 && !isDead)
        {
            StartCoroutine(deathCRT());
            yield break;
        }

        Instantiate(hurtparticle, new Vector2(transform.position.x, transform.position.y - 0.75f), Quaternion.identity);
        float pitch = UnityEngine.Random.Range(0.8f, 1.01f);
        audioManager.instance.playAudio(hitStop, 1f, pitch, transform, audioManager.instance.sfx);
        if (poisoned) damageFlashScript.changeColor(false);
        else damageFlashScript.changeColor(true);
        damageFlashScript.callFlash();
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(frameStop(duration));
        yield return new WaitForSeconds(0.2f);
        invincible = false;
    }

    private IEnumerator spriteFlash()
    {
        yield return null;
    }

    private IEnumerator endTutor()
    {
        GlobalSettings globalSettings = SaveSystem.Load();
        print("isFirst: " + globalSettings.information.isFirst);
        if (globalSettings.information.isFirst == 1)
        {
            globalSettings.information.doneTutorial = 1;
            globalSettings.information.turnedon = 0;
            globalSettings.information.isFirst = 0;
        }
        SaveSystem.Save(globalSettings);

        cineAnim.Play("cinecam_endd");
        canvasAnimator.Play("hpComeUp");
        yield return new WaitForSeconds(3f);
        loadScene.SceneToLoad = 4;
        SceneManager.LoadScene(2);
    }

    private IEnumerator deathCRT()
    {

        if(isintenthlayer) deathcount++;
        
        hasdiedforeverybody = true;
        if (PoisonQueCorountine != null) StopCoroutine(PoisonQueCorountine);
        if (currentWeapon == 1) StartCoroutine(pickUpWeapon(0, "fist"));
        
        poisonQueue = new Queue<float>();
        
        isPoisoned = false;
        isPoisonRunning = false;
        isFuckingPoisoned = false;
        shouldAim = false;
        isDead = true;
        canPause = false;
        invincible = true;
        //DisableAllActiveWeapons();
        stopAttacking = true;
        shouldMakeSound = false;

        
        rb.linearVelocity = Vector2.zero;
        
        canMove = false;
        
        SpawnerScript.shouldSpawn = false;

        
        TLOHLFADEOUTANDINNER(1);
        yield return StartCoroutine(frameStop(0.2f));

        string animclip = "player_death";
        AudioClip clip = death;

        if (isAngelic)
        {
            clip = angelicDeath;
            animclip = "player_angelic_death";

            foreach (ShakeSelfScript s in bodyPartShakes)
            {
                s.Begin();
            }
        }
        audioManager.instance.playAudio(clip, 1f, 1f, transform, audioManager.instance.sfx);

        gravityBox.enabled = false;
        legBox.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;

        //bool isInCharchoba = anim.GetCurrentAnimatorStateInfo(0).IsName("player_mf_special");
        if (arrowPickupSource != null && arrowPickupSource.isPlaying) arrowPickupSource.Stop();
        anim.CrossFade(animclip, 0);
        //anim.Play(animclip);

        //if (isintenthlayer) tenthscript.onPlayerDeath(isAngelic ? 1.5f : 2.5f);

        runParticles.Stop();
        
       
        if (isAngelic)
        {
            angelDeathParticles.Play();
            camShake.amplitude = 2;
            camShake.frequency = 1.2f;
            camShake.time = -1;
            StartCoroutine(camShake.shake());
            yield return new WaitForSeconds(1.5f);
            
            audioManager.instance.stopLillith();
            yield return new WaitForSeconds(2);
            AudioListener.pause = true;
            StartCoroutine(gadasvla(5));
            yield break;
        }
        

        yield return new WaitForSeconds(1f);
        rb.gravityScale = 8f;
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(1f);
        //DisableAllActiveWeapons();
        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        this.transform.position = new Vector3(this.transform.position.x, -4.4f, 0);
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        uiCanvas.renderMode = RenderMode.WorldSpace;
        //AudioClip revivalClip = revival;
        StartCoroutine(revivalPlayBack()); 
        camAnimator.Play("player_camera_fall");
        audioManager.instance.stopMusic();
        yield return new WaitForSeconds(2f);
        if (isintenthlayer) TenthLayerOfHellScript.shouldturnoffforawhile = true;
        //DisableAllActiveWeapons();
        autokillcollider.enabled = true;
        hp = 150f;
        if(isintenthlayer) hpAnimator.Play("PlayerDepoisoning");
        yield return new WaitForSeconds(8f);
        AchivementScript.instance.UnlockAchivement("MEET_RAPHAEL");
        camShake.StopAllCoroutines();
        StartCoroutine(revive());
        yield return null;

        
        yield break;
    }

    public IEnumerator revivalPlayBack()
    {
        float time = 3.0125f;
        AudioClip revivalClip;
        if (!hasheard)
        {
            hasheard = true;
            revivalClip = stylemastery;
        }
        else
        {
            revivalClip = deaths[UnityEngine.Random.RandomRange(0, deaths.Length)];
        }
        if (revivalClip == revival) time = 0;
        else audioManager.instance.playAudio(deathBG, 0.2f, 1, transform, audioManager.instance.music);
        yield return new WaitForSeconds(time);
        audioManager.instance.playAudio(revivalClip, 1, 1, transform, audioManager.instance.sfx);
        //Time.timeScale = 0;
    }

    public IEnumerator revive()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        audioManager.instance.startMusic();
        uiCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        anim.Play("player_revive");
        anim.SetBool("isWalking", false);
        anim.SetBool("isJumping", false);
        //yield return new WaitForSeconds(1f);
        rb.bodyType = RigidbodyType2D.Dynamic;
        gravityBox.enabled = true;
        legBox.enabled = true;
        rb.gravityScale = 1f;
        PauseScript.kill = 0f;
        PauseScript.dmg = 0;
        PauseScript.dro = 0f;
        StartCoroutine(pickUpWeapon(0, "null"));
        StyleManager.instance.reset();
        isDead = false;
        if (isintenthlayer) TenthLayerOfHellScript.shouldturnoffforawhile = false;
        shouldAim = true;
        yield return new WaitForSeconds(2f);
        //invincible = false;
        SpawnerScript.shouldSpawn = true;
        spawner.SetActive(true);
        SpawnerScript.shouldChangeBack = true;
        autokillcollider.enabled = false;
        canMove = true;
        shouldMakeSound = true;
        stopAttacking = false;
        hasdiedforeverybody = false;
        canPause = true;
        invincible = false;
        TLOHLFADEOUTANDINNER(2);
        yield return null;
    }

    IEnumerator posionCure(float seconds)
    {
        canBePoisoned = false;
        yield return new WaitForSeconds(seconds);
        canBePoisoned = true;
        wasPoisoned = false;
    }

    public IEnumerator frameStop(float duration)
    {
        if (waiting) yield break;
        waiting = true;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        if(!isInAngelTransition) Time.timeScale = 1f;
        waiting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.CompareTag("weaponPickup") && currentWeapon == 0)
        {
            justShotBoomerang = false;

            Transform parent1 = collision.transform.parent;
            if (parent1 != null)
            {
                weaponPickupScript wps = parent1.GetComponent<weaponPickupScript>();
                StartCoroutine(pickUpWeapon(wps.weaponID, collision.gameObject.name));
                if (!wps.infinite) Destroy(collision.transform.parent.gameObject);
            }
        }
        else if (obj.CompareTag("boomerangPickup") && currentWeapon == 0)
        {
            if (collision.gameObject.name == "BoomerangWhichYouJustShot") justShotBoomerang = true;
            StartCoroutine(pickUpWeapon(2, collision.gameObject.name));
            Destroy(collision.gameObject);
        }
        else if (obj.CompareTag("mfPickup") && currentWeapon == 0)
        {
            justShotBoomerang = false;
            StartCoroutine(pickUpWeapon(3, collision.gameObject.name));
            Destroy(collision.gameObject);
        }
        else if (obj.CompareTag("arrowPickup"))
        {
            justShotBoomerang = false;
            hasArrow = true;
            if (shouldMakeSound) arrowPickupSource = audioManager.instance.playAudio(arrowPickup, 1f, 1, this.transform, audioManager.instance.sfx);
            Destroy(collision.gameObject.transform.parent.gameObject);
        }

        if (!invincible && !godMode && !isInAngelTransition)
        {
            if (obj.CompareTag("enemyhitbox"))
            {
                /*
                float direction = Mathf.Sign(-transform.localScale.x);
                float knockback = 4f;
                Vector2 force = new Vector2(-direction, 0);
                rb.AddForce(force * (knockback * 100f), ForceMode2D.Impulse);
                */
                StartCoroutine(damage(30, 0.1f, false));
            }
            else if (obj.CompareTag("enemyorb"))
            {
                /*
               float direction = Mathf.Sign(-transform.localScale.x);
               float knockback = 4f;
               Vector2 force = new Vector2(-direction, 0);
               rb.AddForce(force * (knockback * 100f), ForceMode2D.Impulse);
               */
                StartCoroutine(damage(25, 0.1f, false));
            }
            else if (obj.CompareTag("Enemy"))
            {
                /*
                hp -= 5;
                float direction = Mathf.Sign(transform.localScale.x);
                float knockback = 4f;
                Vector2 force = new Vector2(-direction, 0);
                 */
                //rb.AddForce(force * (knockback * 10f), ForceMode2D.Impulse);
            }
            else if (obj.CompareTag("Explosion"))
            {
                StartCoroutine(damage(25, 0.2f, false));
            }
            else if (obj.name == "GreenFireBall(Clone)")
            {
                ApplyPoison(4f);
            }
            else if (obj.CompareTag("FireballP"))
            {
                StartCoroutine(damage(20, 0.2f, false));
            }
            else if (obj.CompareTag("Fireball"))
            {
                StartCoroutine(damage(8, 0.1f, false));
                //invincible = false;
            }
            else if(obj.CompareTag("Crystal"))
            {
                StartCoroutine(damage(15, 0.1f, false));
            }
            else if(obj.CompareTag("Log"))
            {
                StartCoroutine(damage(25, 0.1f, false));
            }
        }

        if (obj.layer == 3 && obj.name != "Movable")
        {
            GetComponent<CapsuleCollider2D>().isTrigger = false;
            isGrounded = true;
            isFalling = false;
        }
        else if (obj.CompareTag("camTriggerDown"))
        {
            cineAnim.Play("cinecam_fall");
            canvasAnimator.Play("hpComeDown");
            tutorialLock = false;
        }

        Transform parent = collision.gameObject.transform.parent;
        TutorialNoteScript note = null;
        if (parent != null) note = parent.GetComponent<TutorialNoteScript>();
        if (canOpen && note != null)
        {
            canOpen = false;
            ScrollScript.instance.rollInScroll(note.noteText, note.headerImage, note.fontSize);
        }
        else if (obj.CompareTag("endTutorial") && canEnd)
        {
            canEnd = false;
            StartCoroutine(endTutor());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 8)
        {
            isGrounded = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("poison"))
        {
            isPoisoned = false;
            isinbubble = false;
            wasPoisoned = false;
            hpAnimator.Play("PlayerDepoisoning");
        }

        if (!canOpen && obj.transform.parent.GetComponent<TutorialNoteScript>() != null)
        {
            canOpen = true;
        }
    }

    public IEnumerator poison(float seconds)
    {
        float timer = 0f;
        while (timer < seconds && !isDead)
        {
            yield return StartCoroutine(damage(20, 0, true));
            yield return new WaitForSeconds(0.7f);
            timer += 1f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("poison") && isPoisoned == false)
        {
            isPoisoned = true;
            isinbubble = true;
            wasPoisoned = true;
            if(isFuckingPoisoned) hpAnimator.Play("PlayerPoisoning");
            isFuckingPoisoned = true;
        }
    }

    public void ApplyPoison(float seconds)
    {
        poisonQueue.Enqueue(seconds);
        print("isPoisonRunning: " + isPoisonRunning);
        if (!isPoisonRunning)
            PoisonQueCorountine = StartCoroutine(ProcessPoisonQueue());
    }

    public IEnumerator ProcessPoisonQueue()
    {
        //if(!isFuckingPoisoned) hpAnimator.Play("PlayerPoisoning");
        //isPoisonRunning = true;
        if (!isFuckingPoisoned) hpAnimator.Play("PlayerPoisoning");
        print("shevedi shignit, isfuckingpoisoned: " + isFuckingPoisoned);
        isPoisonRunning = true;
        isFuckingPoisoned = true;

        while (poisonQueue.Count > 0 && !isDead)
        {
            float duration = poisonQueue.Dequeue();
            if (!isFuckingPoisoned) hpAnimator.Play("PlayerPoisoning");
            isFuckingPoisoned = true;
            yield return StartCoroutine(poison(duration));
        }

        hpAnimator.Play("PlayerDepoisoning");
        isPoisonRunning = false;
        isFuckingPoisoned = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            isGrounded = true;
        }
    }
}
