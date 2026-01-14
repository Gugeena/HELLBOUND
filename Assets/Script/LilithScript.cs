using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LilithScript : MonoBehaviour
{
    public GameObject staff;
    public Transform player;
    public GameObject crystal;
    public GameObject bat;
    public bool hasBatted;
    public bool isDoneWithBats;
    public Animator animator;
    public bool hasAlreadyTeleportedonce = false;
    public GameObject teleportParticles;
    public GameObject teleportParticles1;
    public bool canTeleport = true;
    public bool canAttack;
    public Transform[] spawners;
    public GameObject skyCandle;
    public GameObject eyeBall;
    public GameObject charger;
    public GameObject handR;
    public GameObject staffController;
    public bool canSummon;
    public GameObject shardSpawner;
    public GameObject shardSpawnParticles;
    public bool shouldFlip;
    public GameObject fireBall;
    public GameObject fireBallSpawnParticles;
    public bool isGrounded = true;
    public bool shouldPURPLE = true;
    public GameObject spawnParticles;
    public Vector3 center = new Vector3(24.61f, -1.1f, 0);
    public Animator flameanimator;
    public bool hasPredeterminedPlace = false;
    public int attackCount;
    public float hp = 40;
    public UnityEngine.UI.Slider hpslider;
    public float hpCurVel = 0;
    camShakerScript cum;
    public GameObject[] candles;
    public int[] candlevalues;
    public GameObject HPFADER;
    public static bool shouldturnoff = true;
    public bool hasAlreadystarted = false;
    public CapsuleCollider2D capsuleCollider2;
    public SpriteRenderer[] sprites;
    public bool cantlosehp = true;
    public bool isDead = false;
    public GameObject[] deathParticles;
    public Rigidbody2D handrb;
    public BoxCollider2D handcollider;
    public GameObject fadeOut;

    [Header("Sounds")]
    [SerializeField] private AudioClip[] lillithLaughs;
    [SerializeField] private AudioClip hailMary, cast;
    [SerializeField] private AudioSource audioSource;

    public spriteFlashScript spriteFlash;

    public GameObject[] destroyParticles;
    public Transform[] destroyParticleLocations;


    public AudioClip lilithDeathSound;

    public static event Action lilithDeathEvent;
    public static bool bossfightstarted = false;

    private bool invincible = false;

    AudioSource hailmarysound;

    public SpriteRenderer staffSpriteRenderer;

    [SerializeField]
    private LillithStaffScript staffScript;

    Vector3 scale = new Vector3 (1, 1, 1);

    public bool shoulddieyet = true;
    public bool washitbyspear = false, wasStunned = false;

    public bool attackInProgress = false;
    public bool teleportInProgress = false, canBeStunned = true;
    public static bool stunned = false;

    Coroutine batCoroutine;

    public static bool isRight;

    public bool alreadychoppin;

    public AudioClip firePillarCast;

    public AudioSource firepillarcastSource;

    void Start()
    {
        shouldFlip = true;
        scale = transform.localScale;
        bossfightstarted = true;
        stunned = false;
        alreadychoppin = false;
        StartCoroutine(waiter());
        /*
        canTeleport = true;
        if (!hasBatted) StartCoroutine(BatAttack());
        player = GameObject.Find("Player(Clone)").transform;
        canSummon = true;
        hasAlreadyTeleportedonce = false;
        shouldFlip = false;
        shouldPURPLE = true;
        cum = GetComponent<camShakerScript>();
        HPFADER.SetActive(true);
        */
    }

    IEnumerator waiter()
    {
        hasAlreadystarted = true;
        player = GameObject.Find("Player(Clone)").transform;
        cum = GetComponent<camShakerScript>();
        cum.Stop();
        spawners[0] = GameObject.Find("SCS").transform;
        spawners[1] = GameObject.Find("SCS1").transform;
        spawners[4] = GameObject.Find("C").transform;
        spawners[5] = GameObject.Find("C1").transform;
        spawners[6] = GameObject.Find("C2").transform;
        spawners[7] = GameObject.Find("C3").transform;

        candles[0] = GameObject.Find("Candle");
        candles[1] = GameObject.Find("Candle1");
        candles[2] = GameObject.Find("Candle2");
        candles[3] = GameObject.Find("Candle3");

        flameanimator = GameObject.Find("FlameController").GetComponent<Animator>();
        yield return new WaitForSeconds(1f);
        yield return null;
        canTeleport = true;
        if (!hasBatted) StartCoroutine(BatAttack());
        canSummon = true;
        hasAlreadyTeleportedonce = false;
        shouldPURPLE = true;
        spriteFlash = GetComponent<spriteFlashScript>();
        hpslider = GameObject.Find("LilithHP").GetComponent<UnityEngine.UI.Slider>();
        //staffSpriteRenderer.material = new Material(staffSpriteRenderer.material);
        cantlosehp = false;
        yield return new WaitForSeconds(1f);
        candles[0] = GameObject.Find("Candle");
        candles[1] = GameObject.Find("Candle1");
        candles[2] = GameObject.Find("Candle2");
        candles[3] = GameObject.Find("Candle3");
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stunned && wasStunned)
        {
            stunned = false;
            animator.speed = 1;
            wasStunned = false;
            canBeStunned = false;
            StartCoroutine(StunCooldown());
            //canTeleport = false;
            // StartCoroutine(damage(RetrieveTeleportCount(collision), 0.25f, 0.25f));
        }

        handleHP();

        if (stunned) return;

        if (!isDead)
        {
            if (hasBatted && canTeleport && !teleportInProgress)
            {
                StartCoroutine(HandleTeleport());
            }
            if (isDoneWithBats && !canTeleport && canAttack)
            {
                HandleCombat();
            }

            if (shouldFlip) handleflip();
        }

        if (!PlayerMovement.shouldMakeSound)
        {
            if (hailmarysound != null && hailmarysound.isPlaying) hailmarysound.Stop();
            if (firepillarcastSource != null && firepillarcastSource.isPlaying) firepillarcastSource.Stop();
        }

        // if(!canTeleport && !isDoneWithBats && hasBatted) StartCoroutine(firstTeleportCooldown());
    }

    IEnumerator StunCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        canBeStunned = true;
    }

    IEnumerator death()
    {
        yield return new WaitUntil(() => !stunned);
        print("stunned: " + stunned);
        print("totxmeti");
        if(hailmarysound != null) hailmarysound.Stop();
        PlayerMovement.TLOHLFADEOUTANDINNER(1);
        TenthLayerOfHellScript.shouldturnoffforawhile = true;
        invincible = true;
        cum.Stop();
        cum.amplitude = 0.13f;
        cum.frequency = 0.2f;
        cum.time = 17f;
        StartCoroutine(cum.shake());
        isDead = true;
        animator.SetBool("shouldBATS", false);
        animator.Play("LilithDeathAnimation");
        ShakeSelfScript staffshake = handrb.gameObject.GetComponent<ShakeSelfScript>();
        ShakeSelfScript shakeself = gameObject.GetComponent<ShakeSelfScript>();
        staffshake.Begin();
        shakeself.Begin();
        if (!PlayerMovement.isintenthlayer)
        {
            Animator vinnigreteAnimator = GameObject.Find("Vinigreti(Lilith)").GetComponent<Animator>();
            vinnigreteAnimator.Play("VinnigreteDissapearence");
        }
        Animator lilithhpanimator = GameObject.Find("HPFADER").GetComponent<Animator>();
        lilithhpanimator.Play("LilithHPFadeOut");
        if(audioSource != null) audioSource.Stop();
        if (hailmarysound != null) hailmarysound.Stop();
        for (int i = 0; i < destroyParticleLocations.Length; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Instantiate(destroyParticles[j], destroyParticleLocations[i].position, Quaternion.identity);
            }
        }
        Destroy(handrb.gameObject);
        if(PlayerMovement.shouldMakeSound) audioManager.instance.playAudio(lilithDeathSound, 1f, 1, transform, audioManager.instance.sfx);
        yield return new WaitForSeconds(1f);
        audioManager.instance.stopLillith();
        yield return new WaitForSeconds(2.4f);
        for(int i = 0; i < deathParticles.Length; i++) Instantiate(deathParticles[i], this.transform.position, Quaternion.identity);
        cum.Stop();
        lilithDeathEvent.Invoke();
        yield return null;
        AchivementScript.instance.UnlockAchivement("DEFEAT_LILITH");
        if(washitbyspear) AchivementScript.instance.UnlockAchivement("FORGIVE_ME");
        if(washitbyspear) AchivementScript.instance.UnlockAchivement("FORGIVE_ME");
        Destroy(gameObject);
    }

    void handleflip()
    {
        if (!shouldFlip) return;
        scale = transform.localScale;

        if (player.transform.position.x > this.transform.position.x) scale.x = Mathf.Abs(scale.x);
        else scale.x = -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    public void handleHP()
    {
        if (cantlosehp) return;
        hp = Mathf.Clamp(hp, 0, 40);
        float currHP = Mathf.SmoothDamp(hpslider.value, hp, ref hpCurVel, 0.2f);
        hpslider.value = currHP;

        for (int i = 0; i < candlevalues.Length; i++)
        {
            if (hp <= candlevalues[i])
            {
                candles[i]?.SetActive(false);
            }
        }

        if(hp <= 0 && !isDead && shoulddieyet && !stunned)
        {
            StartCoroutine(death());
        }
    }


    IEnumerator HandleTeleport()
    {
        if (isDead || !canTeleport) yield break;
        while (stunned) yield return null;
        canBeStunned = false;
        teleportInProgress = true;
        StopCoroutine(teleportCooldownBats());
        canTeleport = false;
        shouldFlip = false;
        if (shouldPURPLE) animator.SetBool("shouldPURPLE", true);
        else animator.SetBool("shouldPURPLE", false);
        animator.SetBool("shouldPORT", true);

        //resetStaff();

        yield return new WaitForSeconds(0.25f);
        //canAttack = false;
        Instantiate(teleportParticles, this.transform.position, Quaternion.identity);
        Vector2 pos = new Vector2(0, 0);
        do
        {
            int x = hasAlreadyTeleportedonce ? UnityEngine.Random.Range(1, 7) : UnityEngine.Random.Range(2, 7);
            if (isDead) yield break;
            switch (x)
            {
                case 1: pos = new Vector2(25.533f, -2.539f); break;
                case 2: pos = new Vector2(16.27f, -2.539f); break;
                case 3: pos = new Vector2(33.98f, -2.539f); break;
                case 4: pos = new Vector2(31.187f, 2.221f); break;
                case 5: pos = new Vector2(25.167f, 2.94f); break;
                case 6: pos = new Vector2(19.057f, 2.221f); break;
            }
        }
        while (Vector2.Distance(pos, this.transform.position) < 0.1f || Vector2.Distance(pos, PauseScript.lastPosition) < 0.1f);
        animator.SetBool("shouldPORT", false);
        shouldFlip = true;
        Instantiate(teleportParticles1, this.transform.position, Quaternion.identity);
        if(PlayerMovement.shouldMakeSound) audioManager.instance.playRandomAudio(lillithLaughs, 0.7f, 1, transform, audioManager.instance.sfx);
        this.transform.position = pos;
        staffScale();
        if (isDead) yield break;
        yield return new WaitForSeconds(0.5f);
        staff.SetActive(true);
        handR.SetActive(false);
        hasAlreadyTeleportedonce = true;
        hasPredeterminedPlace = false;
        teleportInProgress = false;
        if (isDead) yield break;
        canBeStunned = true;
        //if(isDoneWithBats)StartCoroutine(attackCooldown(0.3f));
    }

    void HandleCombat()
    {
        if (canAttack && attackCount % 15 != 0 && attackCount != 0 && !attackInProgress)
        {
            while (true)
            {
                attackInProgress = true;
                bool isInFarLeftPosition = Vector2.Distance(new Vector2(16.27f, -2.539f), this.transform.position) < 0.1f;
                bool isInFarRightPosition = Vector2.Distance(new Vector2(33.98f, -2.539f), this.transform.position) < 0.1f;

                canAttack = false;
                float x = UnityEngine.Random.Range(0f, 1.1f);
                if (canSummon)
                {
                    if (x < 0.3) StartCoroutine(fireBallAttack());
                    else if (x < 0.6) StartCoroutine(shardAttack());
                    else if (x < 0.9)
                    {
                        if (!isInFarRightPosition && !isInFarLeftPosition) continue;
                        else
                        {
                            StartCoroutine(flameAttack(isInFarLeftPosition, isInFarRightPosition));
                        }
                    }
                    else StartCoroutine(summonAttack());
                    break;
                }
                else
                {
                    if (x < 0.33f) StartCoroutine(fireBallAttack());
                    else if (x < 0.66f) StartCoroutine(shardAttack());
                    else
                    {
                        if (!isInFarRightPosition && !isInFarLeftPosition || isInFarLeftPosition && player.transform.position.x < this.transform.position.x || isInFarRightPosition && player.transform.position.x > this.transform.position.x) continue;
                        else
                        {
                            StartCoroutine(flameAttack(isInFarLeftPosition, isInFarRightPosition));
                        }
                    }
                    break;
                }
            }
        }
        else if (attackCount % 15 == 0 && attackCount != 0) StartCoroutine(BatAttack());
    }

    public IEnumerator flameAttack(bool isfarleft, bool isfarright)
    {
        while (stunned) yield return null;
        shoulddieyet = false;
        bool isInFarLeftPosition = isfarleft;
        bool isInFarRightPosition = isfarright;
        if (!isInFarLeftPosition && !isInFarRightPosition || isInFarLeftPosition && player.transform.position.x < this.transform.position.x || isInFarRightPosition && player.transform.position.x > this.transform.position.x)
        {
            shouldFlip = true;
            staffScale();
            StartCoroutine(attackCooldown(0.4f, 0));
            yield break;
        }
        Vector3 scale = transform.localScale;
        shouldFlip = false;
        if (isInFarRightPosition)
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        else if (isInFarLeftPosition)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
        if (stunCheck()) yield break;
        animator.SetBool("shouldFLAME", true);
        firepillarcastSource = audioManager.instance.playAudio(firePillarCast, 1, 1, this.transform, audioManager.instance.sfx);
        if (PlayerMovement.shouldMakeSound) firepillarcastSource = audioManager.instance.playAudio(firePillarCast, 1, 1, this.transform, audioManager.instance.sfx);
        //int stateHash = Animator.StringToHash("LilithStaffHeartRedToOrange");
        //heartAnimator.Play(stateHash, 0);
        staffScript.changeColor(LillithStaffScript.Colors.orange);

        yield return new WaitForSeconds(1.4f);
        if (PlayerMovement.shouldMakeSound) audioSource.Play();
        animator.SetBool("shouldFLAME", false);
        cum.amplitude = 0.3f;
        cum.frequency = 0.4f;
        cum.time = 3f;
        StartCoroutine(cum.shake());
        staffScale();
        shouldFlip = false;
        if (isInFarLeftPosition == true) flameanimator.Play("FlamesController-LeftFlame");
        else if (isInFarRightPosition == true) flameanimator.Play("FlamesController-RightFlame");
        //stateHash = Animator.StringToHash("LilithStaffHeartOrangeToRed");
        //heartAnimator.Play(stateHash, 0);
        staffScript.changeColor(LillithStaffScript.Colors.red);
        shoulddieyet = true;
        yield return new WaitForSeconds(4f);
        while (stunned) yield return null;
        shouldFlip = true;
        StartCoroutine(attackCooldown(0.4f, 0));
    }

    IEnumerator summonCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canSummon = true;
        yield break;
    }

    void staffScale()
    {
        int dir = getDirection();

        Vector3 direction = (new Vector3(player.position.x, player.position.y - 1.6f) - staffController.transform.position).normalized;
        float angle = MathF.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Vector3 scale = staffController.transform.localScale;

        if (dir > 0)
        {
            scale.y = Mathf.Abs(scale.y);
            scale.x = Mathf.Abs(scale.x);
        }
        else
        {
            scale.y = Mathf.Abs(scale.y);
            scale.x = Mathf.Abs(scale.x);
        }
        //staffController.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        staffController.transform.rotation = Quaternion.Euler(0, 0, 0);
        staffController.transform.localScale = scale;
    }

    IEnumerator rotatetooriginal(Transform target)
    {
        shouldFlip = false;
        float elapsed = 0f;
        float duration = 0.3f;
        //target.localScale = new Vector3(3.835358f, 3.835358f, 0);
        bool wasFlippedleft = transform.localScale.x < 0f;
        //Quaternion goal = (angle >= -60 && angle <= 60) ? Quaternion.identity : Quaternion.Euler(0, 0, 180);
        //Quaternion goal = Quaternion.identity;
        //Quaternion goal = center.x > this.transform.position.x ? Quaternion.identity : Quaternion.Euler(0, 0, 180);
        //Quaternion goal = player.transform.position.x > transform.position.x ? Quaternion.identity : Quaternion.Euler(0, 0, 180);
        Quaternion current = staffController.transform.rotation;
        while (elapsed < duration)
        {
            //bool still = this.transform.position.x > center.x && this.transform.position.x > player.transform.position.x;
            //if (still) goal = goal;
            //else goal = Quaternion.identity;
            shouldFlip = false;
            Quaternion goal = transform.localScale.x < 0 ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
            float t = elapsed / duration;
            target.rotation = Quaternion.Slerp(current, goal, t);
            elapsed += Time.deltaTime;
            if (isDead) yield break;
            yield return null;
        }
        Quaternion finalgoal = transform.localScale.x < 0 ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
        target.rotation = finalgoal;
    }

    public IEnumerator shardAttack()
    {
        //int stateHash = Animator.StringToHash("LilithStaffHeartRedToGreen");
        //heartAnimator.Play(stateHash, 0);
        staffScript.changeColor(LillithStaffScript.Colors.green);
        shouldFlip = true;
        float duration = 1f;
        float elapsed = 0f;
        float angle = 0f;
        Vector2 dir = new Vector2(0, 0);
        //staffController.transform.rotation = Quaternion.identity;
        //Quaternion originalrotation = staffController.transform.rotation;
        bool hasplayed = false;
        Vector3 scale = staffController.transform.localScale;
        while (elapsed < duration)
        {
            dir = (new Vector3(player.position.x, player.position.y - 1.6f) - staffController.transform.position).normalized;

            angle = MathF.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            if (player.position.x > transform.position.x)
            {
                scale.y = Mathf.Abs(scale.y);
                scale.x = Mathf.Abs(scale.x);
                angle = Mathf.Clamp(angle, -60, 60);
                //angle -= 5f;
                //angle -= 60f;
            }
            else if (player.position.x < transform.position.x)
            {
                scale.y = -Mathf.Abs(scale.y);
                scale.x = -Mathf.Abs(scale.x);
                if (angle > 0) angle -= 360;
                angle = Mathf.Clamp(angle, -240f, -120f);
                //angle += 5f;
                print("elseshi shevedi");
                //angle += 60f;
            }

            staffController.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            staffController.transform.localScale = scale;
            elapsed += Time.deltaTime;
            if (elapsed >= 0.3f && !hasplayed)
            {
                yield return new WaitUntil(() => !stunned);
                if (PlayerMovement.shouldMakeSound) audioManager.instance.playAudio(cast, 1, 1, transform, audioManager.instance.sfx); //audioSource.Play(); audioManager.instance.playAudio(cast, 1, 1, transform, audioManager.instance.sfx);
                hasplayed = true;
            }
            if (isDead) yield break;
            yield return null;
        }
        //stateHash = Animator.StringToHash("LilithStaffHeartGreenToRed");
        //heartAnimator.Play(stateHash, 0);
        staffScript.changeColor(LillithStaffScript.Colors.red);
        //yield return new WaitForSeconds(0.05f);
        Vector2 direction = Vector2.zero;
        direction = (new Vector3(player.transform.position.x, player.transform.position.y - 0.4f) - shardSpawner.transform.position).normalized;
        shouldFlip = false;
        GameObject particles = Instantiate(shardSpawnParticles, shardSpawner.transform.position, Quaternion.identity);
        GameObject shard = Instantiate(crystal, shardSpawner.transform.position, Quaternion.identity);
        particles.transform.rotation = Quaternion.Euler(0, 0, angle);
        //if (isGrounded) direction = (new Vector3(player.transform.position.x, player.transform.position.y -0.4f) - shard.transform.position).normalized;
        //else direction = (new Vector3(player.transform.position.x, player.transform.position.y - 0.2f) - shard.transform.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90;
        shard.transform.rotation = Quaternion.Euler(0, 0, angle);
        Rigidbody2D shardrb = shard.GetComponent<Rigidbody2D>();
        //shardrb.velocity = shard.transform.up * 10f;
        shardrb.linearVelocity = direction * 30f;
        //bool isToTheRight = this.transform.position.x > center.x && this.transform.position.x > player.transform.position.x;
        yield return new WaitForSeconds(0.2f);
        //if(this.transform.position.x < player.transform.position.x) staffController.transform.localScale = scale;
        //else staffController.transform.localScale = -scale;
        //staffController.transform.rotation = Quaternion.identity;
        //bool isToTheRight = this.transform.position.x > center.x && this.transform.position.x > player.transform.position.x;
        if (isDead) yield break;
        StartCoroutine(rotatetooriginal(staffController.transform));
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(attackCooldown(0.4f, 0));
    }

    public bool stunCheck()
    {
        if (stunned)
        {
            StartCoroutine(reset());
        }
        return stunned;
    }

    public IEnumerator reset()
    {
        if (isDead) yield break;
        while (stunned) yield return null;
        StartCoroutine(rotatetooriginal(staffController.transform));
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(attackCooldown(0.4f, 0));
    }

    private int getDirection()
    {
        if (player.transform.position.x > this.transform.position.x) return 1;
        else return -1;
    }

    public IEnumerator fireBallAttack()
    {
        staffScript.changeColor(LillithStaffScript.Colors.purple);

        shouldFlip = true;
        float duration = 1f;
        float elapsed = 0f;
        float angle = 0f;
        Vector2 dir = new Vector2(0, 0);
        Vector3 scale = staffController.transform.localScale;
        bool hasplayed = false;
        while (elapsed < duration)
        {
            dir = (new Vector3(player.position.x, player.position.y - 1.6f) - staffController.transform.position).normalized;

            angle = MathF.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            if (player.position.x > transform.position.x)
            {
                scale.y = Mathf.Abs(scale.y);
                scale.x = Mathf.Abs(scale.x);
                angle = Mathf.Clamp(angle, -60, 60);
                //angle -= 5f;
                //angle -= 60f;
            }
            else if (player.position.x < transform.position.x)
            {
                scale.y = -Mathf.Abs(scale.y);
                scale.x = -Mathf.Abs(scale.x);
                if (angle > 0) angle -= 360;
                angle = Mathf.Clamp(angle, -240f, -120f);
                //angle += 5f;
                //angle += 60f;
            }

            staffController.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            staffController.transform.localScale = scale;
            elapsed += Time.deltaTime;
            if (elapsed >= 0.3f && !hasplayed)
            {
                yield return new WaitUntil(() => !stunned);
                if (PlayerMovement.shouldMakeSound) audioManager.instance.playAudio(cast, 1, 1, transform, audioManager.instance.sfx);  //audioSource.Play(); audioManager.instance.playAudio(cast, 1, 1, transform, audioManager.instance.sfx);
                hasplayed = true;
            }
            if (isDead) yield break;
            yield return null;
        }
        Vector2 direction = Vector2.zero;
        direction = (new Vector3(player.transform.position.x, player.transform.position.y - 0.4f) - shardSpawner.transform.position).normalized;
        shouldFlip = false;
        staffScript.changeColor(LillithStaffScript.Colors.red);
        GameObject particles = Instantiate(fireBallSpawnParticles, shardSpawner.transform.position, Quaternion.identity);
        GameObject shard = Instantiate(fireBall, shardSpawner.transform.position, Quaternion.identity);
        particles.transform.rotation = Quaternion.Euler(0, 0, angle);
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90;
        shard.transform.rotation = Quaternion.Euler(0, 0, angle);
        Rigidbody2D shardrb = shard.GetComponent<Rigidbody2D>();

        shardrb.linearVelocity = direction * 20f;
        yield return new WaitForSeconds(0.2f);
        if (isDead) yield break;
        StartCoroutine(rotatetooriginal(staffController.transform));
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(attackCooldown(0.4f, 0));
    }

    private void resetStaff()
    {
        staffController.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public IEnumerator summonAttack()
    {
        staffScale();
        animator.SetBool("shouldSUMMON", true);
        if (isDead) yield break;
        //int stateHash = Animator.StringToHash("LilithStaffHeartRedToYellow");
        //heartAnimator.Play(stateHash, 0);
        staffScript.changeColor(LillithStaffScript.Colors.yellow);

        yield return new WaitForSeconds(1f);
        if (isDead) yield break;
        animator.SetBool("shouldSUMMON", false);
        float x1 = UnityEngine.Random.Range(0, 1.6f);
        cum.time = 0.5f;
        StartCoroutine(cum.shake());
        if (x1 < 0.5f)
        {
            for (int i = 7; i >= 4; i--)
            {
                Instantiate(spawnParticles, spawners[i].transform.position, Quaternion.identity);
                Instantiate(charger, spawners[i].transform.position, Quaternion.identity);
            }

            for (int i = 3; i >= 2; i--)
            {
                Instantiate(spawnParticles, spawners[i].transform.position, Quaternion.identity);
                Instantiate(eyeBall, spawners[i].transform.position, Quaternion.identity);
            }
        }
        else if (x1 < 1f)
        {
            for (int i = 7; i >= 4; i--)
            {
                Instantiate(spawnParticles, spawners[i].transform.position, Quaternion.identity);
                Instantiate(charger, spawners[i].transform.position, Quaternion.identity);
            }

            for (int i = 1; i >= 0; i--)
            {
                Instantiate(spawnParticles, spawners[i].transform.position, Quaternion.identity);
                Instantiate(skyCandle, spawners[i].transform.position, Quaternion.identity);
            }
        }
        else
        {
            for (int i = 7; i >= 4; i--)
            {
                Instantiate(spawnParticles, spawners[i].transform.position, Quaternion.identity);
                Instantiate(charger, spawners[i].transform.position, Quaternion.identity);
            }

            for (int i = 1; i >= 0; i--)
            {
                Instantiate(spawnParticles, spawners[i].transform.position, Quaternion.identity);
                Instantiate(skyCandle, spawners[i].transform.position, Quaternion.identity);
            }

            for (int i = 3; i >= 2; i--)
            {
                Instantiate(spawnParticles, spawners[i].transform.position, Quaternion.identity);
                Instantiate(eyeBall, spawners[i].transform.position, Quaternion.identity);
            }
        }
        canSummon = false;
        //stateHash = Animator.StringToHash("LilithStaffHeartYellowToRed");
        //heartAnimator.Play(stateHash, 0);
        staffScript.changeColor(LillithStaffScript.Colors.red);

        StartCoroutine(summonCooldown(8f));
        StartCoroutine(attackCooldown(3f, 3));
        yield break;
    }

    IEnumerator attackCooldown(float cooldown, int attack) // 0 - Bats || 1 - Posion || 2 - Fireball || 3 - Summon || 4 - FirePillars
    {
        if (!isDoneWithBats) yield break;
        while (stunned) yield return null;
        //if (attack == 0 || attack == 2) heartAnimator.Play("LilithStaffHeart(Purple-Red)");
        // else if (attack == 3) heartAnimator.Play("LilithStaffHeart(Yellow-Red)");
        //else if (attack == 1) heartAnimator.Play("LilithStaffHeart(Green-Red)");
        canTeleport = true;
        yield return new WaitForSeconds(cooldown);
        StartCoroutine(HandleTeleport());
        yield return new WaitForSeconds(0.2f);
        canAttack = true;
        shouldFlip = true;
        attackCount++;
        attackInProgress = false;
        yield break;
    }

    IEnumerator teleportCooldownBats()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 4; i++)
        {
            canTeleport = false;
            yield return new WaitForSeconds(4.4f);
            if (isDoneWithBats) break;
            canTeleport = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator BatAttack()
    {
        if (isDead) yield break;
        while (stunned) yield return null;
        PlayerMovement.TLOHLFADEOUTANDINNER(1);
        TenthLayerOfHellScript.shouldturnoffforawhile = true;
        isDoneWithBats = false;
        hasBatted = false;
        canTeleport = false;
        yield return null;

        //int stateHash = Animator.StringToHash("LilithStaffHeartRedToPurple");
        //heartAnimator.Play(stateHash, 0);
        staffScript.changeColor(LillithStaffScript.Colors.purple);

        //yield return new WaitForSeconds(1f);
        //animator.SetBool("shouldBATS", true);
        //hasBatted = true;
        batCoroutine = StartCoroutine(teleportCooldownBats());
        animator.Play("LilithBats");
        shouldFlip = false;
        //audioManager.instance.playAudio(hailMary, 1, 1, transform, audioManager.instance.sfx);
        yield return new WaitUntil(() => !stunned);
        if (!isDead) animator.SetBool("shouldBATS", true);
        while (stunned) yield return null;
        yield return new WaitForSeconds(0.8f);
        if (isDead) yield break;
        if (PlayerMovement.shouldMakeSound) hailmarysound = audioManager.instance.playAudio(hailMary, 1, 1, transform, audioManager.instance.sfx);

        StartCoroutine(batHailMary());
        cum.amplitude = 1.3f;
        cum.frequency = 0.2f;
        cum.time = 13f;
        StartCoroutine(cum.shake());
        yield return new WaitForSeconds(0.8f);
        while (stunned) yield return null;
        hasBatted = true;
        yield return new WaitForSeconds(1f);
        while (stunned) yield return null;
        animator.SetBool("shouldBATS", false);
        shouldFlip = true;
        //canTeleport = true;
        //animator.SetBool("shouldBATS", false);
        yield break;
    }

    IEnumerator batHailMary()
    {
        yield return new WaitUntil(() => !stunned);
        for (int i = 0; i < 200; i++)
        {
            if (isDead) yield break;
            yield return new WaitForSeconds(0.05f);
            float x1 = UnityEngine.Random.Range(14.07f, 36.05f);
            //float x2 = UnityEngine.Random.Range(15.28f, 35.68f);
            Instantiate(bat, new Vector2(x1, 6.86f), Quaternion.identity);
            //Instantiate(bat, new Vector2(x2, 6.86f), Quaternion.identity);
        }
        while (stunned) yield return null;
        yield return new WaitForSeconds(0.5f);
        StopCoroutine(batCoroutine);
        canAttack = true;
        isDoneWithBats = true;
        shouldPURPLE = false;
        //StartCoroutine(attackCooldown(0.2f, 0));
        attackCount++;
        PlayerMovement.TLOHLFADEOUTANDINNER(2);
        TenthLayerOfHellScript.shouldturnoffforawhile = false;
        yield break;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            isGrounded = false;
        }
    }

    public IEnumerator damage(int teleportCount, float damageCount, float timer)
    {
        //animator.Play("LilithHurt");
        if (invincible) yield break;
        invincible = true;
        spriteFlash.callFlash();
        staffScript.callFlash();
        hp -= damageCount + teleportCount;
        yield return new WaitForSeconds(timer);
        invincible = false;
    }
        

    public int RetrieveTeleportCount(Collider2D collision)
    {
        teleportCountScript tpcs = collision.gameObject.GetComponent<teleportCountScript>();
        if (tpcs != null) return tpcs.teleportCount;
        return 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleHit(collision);
    }

    public void HandleHit(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("mfHitbox") && !isDead && !obj.name.StartsWith("motherfuckr"))
        {
            washitbyspear = false;
            float timer = 0.1f;
            string name = collision.gameObject.name.ToLower();
            if (obj.name == "meleehitbox") StartCoroutine(damage(RetrieveTeleportCount(collision), 0.2f, timer));
            else
            {
                float damagexz = 0;
                if (name.StartsWith("boomerangprefab"))
                {
                    if (Vector2.Distance(this.transform.position, player.transform.position) > 0.5f)
                    {
                        timer = 0.5f;
                    }
                    damagexz = 0.55f;
                }
                else damagexz = 1f;

                if (name.StartsWith("spear")) washitbyspear = true;

                StartCoroutine(damage(RetrieveTeleportCount(collision), damagexz, timer));
            }
        }

        if (obj.name.StartsWith("motherfuckr") && canBeStunned)
        {
            if (collision.gameObject.transform.position.x > this.transform.position.x) isRight = true;
            else isRight = false;
            stunned = true;
            animator.speed = 0;
            wasStunned = true;
            StartCoroutine(choppitychop());
            //canTeleport = false;
            // StartCoroutine(damage(RetrieveTeleportCount(collision), 0.25f, 0.25f));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
    
    }

    public IEnumerator choppitychop()
    {
        if (alreadychoppin) yield break;
        alreadychoppin = true;
        for (int i = 0; i < 4; i++)
        {
            hp -= 0.25f;
            yield return new WaitForSeconds(0.25f);
        }
        alreadychoppin = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject.name.StartsWith("motherfuckr"))
        //{
            //canTeleport = true;
            //stunned = false;
        //}
    }
}
