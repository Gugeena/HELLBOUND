using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;
using static UnityEditor.PlayerSettings;

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
    public Animator heartAnimator;
    public GameObject fireBall;
    public GameObject fireBallSpawnParticles;
    public bool isGrounded = true;
    public bool hasTeleported = false;
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

    public AudioClip lilithdeath;

    void Start()
    {
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
        shouldFlip = false;
        shouldPURPLE = true;
        spriteFlash = GetComponent<spriteFlashScript>();
        hpslider = GameObject.Find("LilithHP").GetComponent<UnityEngine.UI.Slider>();
        heartAnimator.Rebind();
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
        handleHP();
        if (!isDead)
        {
            if (hasBatted && canTeleport && !hasTeleported)
            {
                StartCoroutine(HandleTeleport());
            }
            if (isDoneWithBats && !canTeleport && canAttack)
            {
                HandleCombat();
            }

            if (shouldFlip) handleflip();
        }

        // if(!canTeleport && !isDoneWithBats && hasBatted) StartCoroutine(firstTeleportCooldown());
    }

    IEnumerator death()
    {
        cum.Stop();
        cum.amplitude = 0.13f;
        cum.frequency = 0.2f;
        cum.time = 17f;
        StartCoroutine(cum.shake());
        isDead = true;
        animator.Play("LilithDeathAnimation");
        ShakeSelfScript staffshake = handrb.gameObject.GetComponent<ShakeSelfScript>();
        ShakeSelfScript shakeself = gameObject.GetComponent<ShakeSelfScript>();
        staffshake.Begin();
        shakeself.Begin();
        Animator vinnigreteAnimator = GameObject.Find("Vinigreti(Lilith)").GetComponent<Animator>();
        vinnigreteAnimator.Play("VinnigreteDissapearence");
        Animator lilithhpanimator = GameObject.Find("HPFADER").GetComponent<Animator>();
        lilithhpanimator.Play("LilithHPFadeOut");
        audioSource.Stop();
        for (int i = 0; i < destroyParticleLocations.Length; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Instantiate(destroyParticles[j], destroyParticleLocations[i].position, Quaternion.identity);
            }
        }
        Destroy(handrb.gameObject);
        audioManager.instance.playAudio(lilithdeath, 1f, 1, transform, audioManager.instance.sfx);
        yield return new WaitForSeconds(1f);
        audioManager.instance.stopLillith();
        yield return new WaitForSeconds(2.4f);
        for(int i = 0; i < deathParticles.Length; i++) Instantiate(deathParticles[i], this.transform.position, Quaternion.identity);
        //PlayerMovement.Beyonder = true;
        PlayerMovement playermovement = player.GetComponent<PlayerMovement>();
        playermovement.goultraandbeyond();
        cum.Stop();
        Destroy(gameObject);
    }

    void handleflip()
    {
        Vector3 scale = transform.localScale;
        Vector3 controllerScale = staffController.transform.localScale;
        if (player.transform.position.x > this.transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        scale.y = Mathf.Abs(scale.y);
        transform.localScale = scale;
        //staffScale();
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

        if(hp <= 0 && !isDead)
        {
            StartCoroutine(death());
        }
    }

    void staffScale()
    {
        Vector2 dir = new Vector2(0, 0);
        Vector3 scale = staffController.transform.localScale;
        //staffController.transform.rotation = Quaternion.identity;
        //Quaternion originalrotation = staffController.transform.rotation;
        dir = (new Vector3(player.position.x, player.position.y - 1.6f) - staffController.transform.position).normalized;

        float angle = MathF.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (player.position.x > staffController.transform.position.x)
        {
            scale.y = Mathf.Abs(scale.y);
            scale.x = Mathf.Abs(scale.x);
            angle = 0;
            //angle -= 5f;
            //angle -= 60f;
        }
        else if (player.position.x < staffController.transform.position.x)
        {
            scale.y = -Mathf.Abs(scale.y);
            scale.x = -Mathf.Abs(scale.x);
            angle = -180;
            //angle += 5f;
            //angle += 60f;
        }
        else
        {
            print("TRIIIIPLE GGG");
        }

        staffController.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        staffController.transform.localScale = scale;
        //staffController.transform.localScale = scale;
    }

    IEnumerator HandleTeleport()
    {
        if (isDead) yield break;
        //staffScale();
        StopCoroutine(teleportCooldownBats());
        if (!canTeleport) yield break;
        canTeleport = false;
        shouldFlip = false;
        if (shouldPURPLE) animator.SetBool("shouldPURPLE", true);
        else animator.SetBool("shouldPURPLE", false);
        animator.SetBool("shouldPORT", true);
        yield return new WaitForSeconds(0.25f);
        //canAttack = false;
        Instantiate(teleportParticles, this.transform.position, Quaternion.identity);
        Vector2 pos = new Vector2(0, 0);
        Vector2 scale = staffController.transform.localScale;
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
        while (Vector2.Distance(pos, this.transform.position) < 0.1f);
        animator.SetBool("shouldPORT", false);
        shouldFlip = true;
        Instantiate(teleportParticles1, this.transform.position, Quaternion.identity);
        audioManager.instance.playRandomAudio(lillithLaughs, 0.7f, 1, transform, audioManager.instance.sfx);
        this.transform.position = pos;
        staffScale();
        if (isDead) yield break;
        yield return new WaitForSeconds(0.5F);
        staff.SetActive(true);
        handR.SetActive(false);
        hasAlreadyTeleportedonce = true;
        hasPredeterminedPlace = false;
        if (isDead) yield break;
        //if(isDoneWithBats)StartCoroutine(attackCooldown(0.3f));
    }

    void HandleCombat()
    {
        if (canAttack && attackCount % 15 != 0 && attackCount != 0)
        {
            while (true)
            {
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
                            int dir;
                            if (isInFarRightPosition) dir = 1;
                            else dir = -1;
                            StartCoroutine(flameAttack(dir));
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
                        if (!isInFarRightPosition && !isInFarLeftPosition) continue;
                        if (!isInFarRightPosition && !isInFarLeftPosition) continue;
                        else
                        {
                            int dir;
                            if (isInFarRightPosition) dir = 1;
                            else dir = -1;
                            StartCoroutine(flameAttack(dir));
                        }
                    }
                    break;
                }
            }
        }
        else if (attackCount % 15 == 0 && attackCount != 0) StartCoroutine(BatAttack());
    }

    public IEnumerator flameAttack(int direction)
    {
        bool isInFarLeftPosition = Vector2.Distance(new Vector2(16.27f, -2.539f), this.transform.position) < 0.1f;
        bool isInFarRightPosition = Vector2.Distance(new Vector2(33.98f, -2.539f), this.transform.position) < 0.1f;
        if (!isInFarLeftPosition && !isInFarRightPosition)
        {
            shouldFlip = true;
            staffScale();
            StartCoroutine(attackCooldown(0.4f, 0));
            yield break;
        }
        Vector3 scale = transform.localScale;
        shouldFlip = false;
        animator.SetBool("shouldFLAME", true);
        Debug.Log("Playing animation: LilithStaffHeart(Red-Orange)");
        heartAnimator.Play("LilithStaffHeart(Red-Orange)");
        yield return new WaitForSeconds(1.4f);
        animator.SetBool("shouldFLAME", false);
        cum.amplitude = 0.3f;
        cum.frequency = 0.4f;
        cum.time = 3f;
        StartCoroutine(cum.shake());
        staffScale();
        if (direction == 1)
        {
            scale.y = -Mathf.Abs(scale.y);
            scale.x = -Mathf.Abs(scale.x);
        }
        else
        {
            scale.y = Mathf.Abs(scale.y);
            scale.x = Mathf.Abs(scale.x);
        }
        if (direction == -1) flameanimator.Play("FlamesController-LeftFlame");
        else flameanimator.Play("FlamesController-RightFlame");
        heartAnimator.Play("LilithStaffHeart(Orange-Red)");
        yield return new WaitForSeconds(4f);
        shouldFlip = true;
        StartCoroutine(attackCooldown(0.4f, 0));
    }

    IEnumerator summonCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canSummon = true;
        yield break;
    }

    public IEnumerator shardAttack()
    {
        Debug.Log("Playing animation: LilithStaffHeart(Red-Green)");
        heartAnimator.Play("LilithStaffHeart(Red-Green)");
        shouldFlip = true;
        float duration = 1f;
        float elapsed = 0f;
        float angle = 0f;
        Vector2 dir = new Vector2(0, 0);
        Vector3 scale = staffController.transform.localScale;
        //staffController.transform.rotation = Quaternion.identity;
        //Quaternion originalrotation = staffController.transform.rotation;
        while (elapsed < duration)
        {
            dir = (new Vector3(player.position.x, player.position.y - 1.6f) - staffController.transform.position).normalized;

            angle = MathF.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            if (player.position.x > staffController.transform.position.x)
            {
                scale.y = Mathf.Abs(scale.y);
                scale.x = Mathf.Abs(scale.x);
                angle = Mathf.Clamp(angle, -60, 60);
                //angle -= 5f;
                //angle -= 60f;
            }
            else if (player.position.x < staffController.transform.position.x)
            {
                scale.y = -Mathf.Abs(scale.y);
                scale.x = -Mathf.Abs(scale.x);
                if (angle > 0) angle -= 360;
                angle = Mathf.Clamp(angle, -240f, -120f);
                //angle += 5f;
                //angle += 60f;
            }
            else
            {
                print("TRIIIIPLE GGG");
            }

            staffController.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            staffController.transform.localScale = scale;
            elapsed += Time.deltaTime;
            if (isDead) yield break;
            yield return null;
        }
        shouldFlip = false;
        heartAnimator.Play("LilithStaffHeart(Green-Red)");
        yield return new WaitForSeconds(0.05f);
        GameObject particles = Instantiate(shardSpawnParticles, shardSpawner.transform.position, Quaternion.identity);
        particles.transform.rotation = Quaternion.Euler(0, 0, angle);
        Vector2 direction = Vector2.zero;
        audioSource.clip = cast;
        audioSource.Play();
        GameObject shard = Instantiate(crystal, shardSpawner.transform.position, Quaternion.identity);
        //if (isGrounded) direction = (new Vector3(player.transform.position.x, player.transform.position.y -0.4f) - shard.transform.position).normalized;
        //else direction = (new Vector3(player.transform.position.x, player.transform.position.y - 0.2f) - shard.transform.position).normalized;
        direction = (new Vector3(player.transform.position.x, player.transform.position.y - 0.4f) - shard.transform.position).normalized;
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
        StartCoroutine(rotateToOriginal(staffController.transform.rotation, 0.3f, staffController.transform));
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(attackCooldown(0.4f, 0));
    }

    public IEnumerator fireBallAttack()
    {
        heartAnimator.Play("LilithStaffHeart(Red-Purple)");
        shouldFlip = true;
        float duration = 1f;
        float elapsed = 0f;
        float angle = 0f;
        Vector2 dir = new Vector2(0, 0);
        Vector3 scale = staffController.transform.localScale;
        //staffController.transform.rotation = Quaternion.identity;
        //Quaternion originalrotation = staffController.transform.rotation;

        while (elapsed < duration)
        {
            dir = (new Vector3(player.position.x, player.position.y - 1.6f) - staffController.transform.position).normalized;

            angle = MathF.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            if (player.position.x > staffController.transform.position.x)
            {
                scale.y = Mathf.Abs(scale.y);
                scale.x = Mathf.Abs(scale.x);
                angle = Mathf.Clamp(angle, -60, 60);
                //angle -= 5f;
                //angle -= 60f;
            }
            else if (player.position.x < staffController.transform.position.x)
            {
                scale.y = -Mathf.Abs(scale.y);
                scale.x = -Mathf.Abs(scale.x);
                if (angle > 0) angle -= 360;
                angle = Mathf.Clamp(angle, -240f, -120f);
                //angle += 5f;
                //angle += 60f;
            }
            else
            {
                print("TRIIIIPLE GGG");
            }
            if (isDead) yield break;
            staffController.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            staffController.transform.localScale = scale;
            elapsed += Time.deltaTime;
            yield return null;
        }
        shouldFlip = false;
        //if (isToTheRight) staffController.transform.localScale = new Vector3(-3.835358f, -3.835358f, 0);
        heartAnimator.Play("LilithStaffHeart(Purple-Red)");
        //yield return new WaitForSeconds(0.02f);
        audioSource.clip = cast;
        audioSource.Play();
        GameObject particles = Instantiate(fireBallSpawnParticles, shardSpawner.transform.position, Quaternion.identity);
        particles.transform.rotation = Quaternion.Euler(0, 0, angle);
        Vector2 direction = Vector2.zero;
        GameObject shard = Instantiate(fireBall, shardSpawner.transform.position, Quaternion.identity);
        //if (isGrounded) direction = (new Vector3(player.transform.position.x, player.transform.position.y -0.4f) - shard.transform.position).normalized;
        //else direction = (new Vector3(player.transform.position.x, player.transform.position.y - 0.2f) - shard.transform.position).normalized;
        direction = (new Vector3(player.transform.position.x, player.transform.position.y - 0.4f) - shard.transform.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90;
        shard.transform.rotation = Quaternion.Euler(0, 0, angle);
        Rigidbody2D shardrb = shard.GetComponent<Rigidbody2D>();
        //shardrb.velocity = shard.transform.up * 10f;
        shardrb.linearVelocity = direction * 20f;
        //bool isToTheRight = this.transform.position.x > center.x && this.transform.position.x > player.transform.position.x;
        yield return new WaitForSeconds(0.2f);
        if (isDead) yield break;
        //if(this.transform.position.x < player.transform.position.x) staffController.transform.localScale = scale;
        //else staffController.transform.localScale = -scale;
        //StartCoroutine(rotateToOriginal(staffController.transform.rotation, originalrotation, 0.1f, staffController.transform));
        //yield return new WaitForSeconds(0.1f);
        //bool isToTheRight = this.transform.position.x > center.x && this.transform.position.x > player.transform.position.x;
        StartCoroutine(rotateToOriginal(staffController.transform.rotation, 0.3f, staffController.transform));
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(attackCooldown(0.4f, 0));
    }

    public IEnumerator rotateToOriginal(Quaternion current, float duration, Transform target)
    {
        //Quaternion goal = Quaternion.identity;
        //if (isToTheRight)
        //{
        //goal = Quaternion.Euler(0, 0, 180);
        //}
        /*
        staffScale();
        Quaternion goal = isToTheRight ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
        float elapsed = 0f;
        if(isToTheRight)
        {
            goal = Quaternion.Euler(0, 0, 180);
        }
        //target.localScale = new Vector3(3.835358f, 3.835358f, 0);
        while (elapsed < duration)
        {
            //bool still = this.transform.position.x > center.x && this.transform.position.x > player.transform.position.x;
            //if (still) goal = goal;
            //else goal = Quaternion.identity;
            float t = elapsed / duration;
            target.rotation = Quaternion.Slerp(current, goal, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.rotation = goal;
        staffScale();
        */
        shouldFlip = false;
        float elapsed = 0f;
        //target.localScale = new Vector3(3.835358f, 3.835358f, 0);
        Quaternion goal = transform.localScale.x < 0 ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
        //Quaternion goal = (angle >= -60 && angle <= 60) ? Quaternion.identity : Quaternion.Euler(0, 0, 180);
        //Quaternion goal = Quaternion.identity;
        //Quaternion goal = center.x > this.transform.position.x ? Quaternion.identity : Quaternion.Euler(0, 0, 180);
        //Quaternion goal = player.transform.position.x > transform.position.x ? Quaternion.identity : Quaternion.Euler(0, 0, 180);
        while (elapsed < duration)
        {
            //bool still = this.transform.position.x > center.x && this.transform.position.x > player.transform.position.x;
            //if (still) goal = goal;
            //else goal = Quaternion.identity;
            float t = elapsed / duration;
            target.rotation = Quaternion.Slerp(current, goal, t);
            elapsed += Time.deltaTime;
            if (isDead) yield break;
            yield return null;
        }
        target.rotation = goal;
        shouldFlip = true;
        staffScale();
        //target.localScale = new Vector3(3.835358f, 3.835358f, 0);
    }

    public IEnumerator summonAttack()
    {
        // staffController.transform.rotation = Quaternion.identity;
        // if (player.transform.position.x < transform.position.x) staffController.transform.localScale = new Vector3(-3.835358f, -3.835358f, staffController.transform.localScale.z);
        staffScale();
        animator.SetBool("shouldSUMMON", true);
        heartAnimator.Play("LilithStaffHeart(Red-Yellow)");
        yield return new WaitForSeconds(1f);
        animator.SetBool("shouldSUMMON", false);
        float x1 = UnityEngine.Random.Range(0, 1.6f);
        cum.time = 0.5f;
        StartCoroutine(cum.shake());
        audioSource.clip = cast;
        audioSource.Play();
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
        staffScale();
        canSummon = false;
        heartAnimator.Play("LilithStaffHeart(Yellow-Red)");
        StartCoroutine(summonCooldown(8f));
        print("bouta start attack cooldown");
        StartCoroutine(attackCooldown(3f, 3));
        yield break;
    }

    IEnumerator attackCooldown(float cooldown, int attack) // 0 - Bats || 1 - Posion || 2 - Fireball || 3 - Summon || 4 - FirePillars
    {
        if (!isDoneWithBats) yield break;
        print("bouta wait for the cooldown");
        //if (attack == 0 || attack == 2) heartAnimator.Play("LilithStaffHeart(Purple-Red)");
        // else if (attack == 3) heartAnimator.Play("LilithStaffHeart(Yellow-Red)");
        //else if (attack == 1) heartAnimator.Play("LilithStaffHeart(Green-Red)");
        canTeleport = true;
        yield return new WaitForSeconds(cooldown);
        StartCoroutine(HandleTeleport());
        yield return new WaitForSeconds(0.2f);
        canAttack = true;
        attackCount++;
        yield break;
    }

    IEnumerator teleportCooldownBats()
    {
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
        isDoneWithBats = false;
        hasBatted = false;
        yield return null;
        heartAnimator.Play("LilithStaffHeart(Red-Purple)");
        //yield return new WaitForSeconds(1f);
        //animator.SetBool("shouldBATS", true);
        //hasBatted = true;
        canTeleport = true;
        animator.Play("LillithBats");
        //audioManager.instance.playAudio(hailMary, 1, 1, transform, audioManager.instance.sfx);
        audioSource.clip = cast;
        audioSource.Play();
        animator.SetBool("shouldBATS", true);
        yield return new WaitForSeconds(0.15f);
        audioSource.Stop();
        audioSource.clip = hailMary;
        audioSource.Play();
        StartCoroutine(batHailMary());
        cum.amplitude = 0.13f;
        cum.frequency = 0.2f;
        cum.time = 17f;
        StartCoroutine(cum.shake());
        yield return new WaitForSeconds(0.8f);
        canTeleport = true;
        hasBatted = true;
        canTeleport = true;
        shouldFlip = true;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(teleportCooldownBats());
        yield return new WaitForSeconds(0.6f);
        //animator.SetBool("shouldBATS", false);
        yield break;
    }

    IEnumerator batHailMary()
    {
        animator.SetBool("shouldBATS", false);
        Vector3 scale = staffController.transform.localScale;
        scale = new Vector3(3.835358f, 3.835358f, 0);
        for (int i = 0; i < 200; i++)
        {
            if (isDead) yield break;
            yield return new WaitForSeconds(0.05f);
            float x1 = UnityEngine.Random.Range(14.07f, 36.05f);
            //float x2 = UnityEngine.Random.Range(15.28f, 35.68f);
            Instantiate(bat, new Vector2(x1, 6.86f), Quaternion.identity);
            //Instantiate(bat, new Vector2(x2, 6.86f), Quaternion.identity);
        }
        StopCoroutine(teleportCooldownBats());
        staffScale();
        canAttack = true;
        isDoneWithBats = true;
        shouldPURPLE = false;
        //StartCoroutine(attackCooldown(0.2f, 0));
        attackCount++;
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

    public void damage(int teleportCount, float damageCount)
    {
        //animator.Play("LilithHurt");

        spriteFlash.callFlash();
        hp -= damageCount + teleportCount;
    }

    public int RetrieveTeleportCount(Collider2D collision)
    {
        int teleportCount = 0;
        String weapon = collision.gameObject.name;
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

        }
        return teleportCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "mfHitbox")
        {
            if (collision.gameObject.name == "meleehitbox") damage(RetrieveTeleportCount(collision), 0.2f);
            else damage(RetrieveTeleportCount(collision), 1f);
        }
    }
}
