using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class mfScript : MonoBehaviour
{
    [SerializeField]
    private ShakeSelfScript shakeSelfScript;
    private camShakerScript camShakerScript;
    private Rigidbody2D rb;

    public bool move, goBack;
    [SerializeField]
    private float speed, returnSpeed;

    private Vector2 startPos;

    private Transform playerTransform; 

    public int direction;

    public GameObject blowUpParticles;
    public GameObject pickup;

    [SerializeField]
    private ParticleSystem particles;
    public Coroutine runningShake;

    public AudioSource mfSound;

    public int killed = 0;
    bool mowed = false;
    public teleportCountScript tpcs;
    public bool canStun;

    private void Start()
    {
        playerTransform = GameObject.Find("Player(Clone)").transform;
        killed = 0;
        tpcs = GetComponent<teleportCountScript>();
        canStun = true;
    }

    void Awake()
    {
        move = false;
        playerTransform = GameObject.Find("Player(Clone)").transform;
        rb = GetComponent<Rigidbody2D>();
        if (!PlayerMovement.instance.inTutorial) camShakerScript = GetComponent<camShakerScript>();
        else camShakerScript = null;
        StartCoroutine(life());
        if (camShakerScript != null) shakeSelfScript.Begin();
        runningShake = null;
    }

    private void Update()
    {
        if (playerTransform == null)
        {
            Destroy(gameObject);
        }

        if (move)
        {
            rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);

        }
        else if (goBack) {
            backing();
        }
        if(tpcs.teleportCount >= 2 && (int) transform.position.x == (int) startPos.x)
        {
           StartCoroutine(back());
        }

        if(killed == 3 && !mowed)
        {
            mowed = true;
            StyleManager.instance.growStyle(1);
            StyleManager.instance.undisputed(2);
        }
        if (PlayerMovement.hasdiedforeverybody) Destroy(this.gameObject);
    }

    private void backing()
    {
        Vector2 direction = playerTransform.position - transform.position;
        if (Vector2.Distance(playerTransform.position, this.transform.position) < 0.5f && playerTransform.gameObject.GetComponent<PlayerMovement>().currentWeapon == 0)
        {
            Instantiate(pickup, playerTransform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (Vector2.Distance(playerTransform.position, this.transform.position) < 0.5f && playerTransform.gameObject.GetComponent<PlayerMovement>().currentWeapon != 0)
        {
            Instantiate(blowUpParticles, this.gameObject.transform.position, Quaternion.identity);   
            Destroy(gameObject);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, returnSpeed * Time.deltaTime);
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private IEnumerator life()
    {
        yield return new WaitForSeconds(0.8f);
        if (camShakerScript != null) runningShake = StartCoroutine(camShakerScript.shake());
        startPos = transform.position;
        if (!canStun) yield break;
        move = true;
    }

    private IEnumerator back()
    {
        rb.linearVelocity = Vector2.zero;
        move = false;
        if(camShakerScript != null)camShakerScript.Stop();
        particles.Stop();
        yield return new WaitForSeconds(0.6f);
        rb.gravityScale = 1;
        goBack = true;
    }

    private IEnumerator teleport()
    {
        yield return new WaitForSeconds(0.2f);
        tpcs.teleportCount++;
    }

    public void OnDestroy()
    {
        if(camShakerScript != null)camShakerScript.Stop();
        mfSound.Stop();
        mfSound.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("llocation1"))
        {
            rb.MovePosition(new Vector2(SidePortalScript.RLocation.position.x - 0.25f, transform.position.y));
            StartCoroutine(teleport());
        }
        else if (obj.CompareTag("rlocation1"))
        {
            rb.MovePosition(new Vector2(SidePortalScript.LLocation.position.x + 0.25f, transform.position.y));
            StartCoroutine(teleport());
        }

        if (obj.CompareTag("LilithGravityBox") && canStun && !goBack)
        {
            canStun = false;
            bool canBeStunned = GameObject.Find("Lilith(Clone)").GetComponent<LilithScript>().canBeStunned;
            if (!canBeStunned) return;
            StartCoroutine(lilithChopUp());
        }
    }

    IEnumerator lilithChopUp()
    {
        rb.linearVelocity = Vector2.zero;
        move = false;
        yield return new WaitForSeconds(2f);
        goBack = true;
    }
}
