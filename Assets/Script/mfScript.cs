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

    private bool move, goBack;
    [SerializeField]
    private float speed, returnSpeed;

    private Vector2 startPos;

    private Transform playerTransform; 

    public int direction;
    private int teleportCount = 0;

    public GameObject blowUpParticles;
    public GameObject pickup;

    [SerializeField]
    private ParticleSystem particles;
    public Coroutine runningShake;

    public AudioSource mfSound;

    private void Start()
    {
        playerTransform = GameObject.Find("Player(Clone)").transform;
    }

    void Awake()
    {
        
        move = false;
        playerTransform = GameObject.Find("Player(Clone)").transform;
        rb = GetComponent<Rigidbody2D>();
        camShakerScript = GetComponent<camShakerScript>();
        StartCoroutine(life());
        shakeSelfScript.Begin();
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
        if(teleportCount >= 4 && (int) transform.position.x == (int) startPos.x)
        {
           StartCoroutine(back());
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
        move = true;
        runningShake = StartCoroutine(camShakerScript.shake());
        startPos = transform.position;
    }

    private IEnumerator back()
    {
        rb.linearVelocity = Vector2.zero;
        move = false;
        camShakerScript.Stop();
        particles.Stop();
        yield return new WaitForSeconds(0.6f);
        rb.gravityScale = 1;
        goBack = true;
    }

    private IEnumerator teleport()
    {
        yield return new WaitForSeconds(0.2f);
        teleportCount++;
    }

    public void OnDestroy()
    {
        camShakerScript.Stop();
        mfSound.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "LLocation")
        {
            rb.MovePosition(new Vector2(SidePortalScript.RLocation.position.x - 0.25f, transform.position.y));
            teleportCount++;
            StartCoroutine(teleport());
        }
        else if (collision.gameObject.name == "RLocation")
        {
            rb.MovePosition(new Vector2(SidePortalScript.LLocation.position.x + 0.25f, transform.position.y));
            teleportCount++;
            StartCoroutine(teleport());
        }
    }

    public int getteleportCount()
    {
        return teleportCount;
    }
}
