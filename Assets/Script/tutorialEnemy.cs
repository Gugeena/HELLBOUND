using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class tutorialEnemy : MonoBehaviour
{
    Rigidbody2D rb;
    public Animator animator;
    float hp = 2f;

    public GameObject particles;

    private bool isGrounded, isFalling;

    public GameObject bow;

    public AudioClip[] idleSounds, damageSounds;
    public AudioClip deathSound;
    public GameObject player;
    public float healamount = 5f;
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isGrounded = true;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        StartCoroutine(sounds());
    }
    void Update()
    {
        if (hp <= 0)
        {
            StartCoroutine(death());
        }
        animator.SetBool("isGrounded", isGrounded);
    }

    private IEnumerator sounds()
    {
        yield return new WaitForSeconds(Random.Range(4f, 7f));
        audioManager.instance.playRandomAudio(idleSounds, 0.45f, 1 ,transform, audioManager.instance.sfx);
        StartCoroutine(sounds());
    }

    public IEnumerator death()
    {
        audioManager.instance.playAudio(deathSound, 0.65f, 1 ,transform, audioManager.instance.sfx);
        Rigidbody2D[] rbs = GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D rbb in rbs)
        {
            rbb.bodyType = RigidbodyType2D.Dynamic;
            rbb.AddForce(transform.up * Random.Range(2f, 3f), ForceMode2D.Impulse);
            rbb.AddTorque(Random.Range(6f, 7f));
            rbb.gameObject.transform.parent = null;
            rbb.excludeLayers = rbb.excludeLayers ^ (1 << LayerMask.NameToLayer("Player"));
            BoxCollider2D bc = rbb.gameObject.GetComponent<BoxCollider2D>();
            if (bc != null) bc.isTrigger = false;
        }
        Vector2 pos = this.transform.position;
        pos.y -= 0.3f;
        Instantiate(bow, pos, Quaternion.identity);
        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
        playerScript.hp += healamount;
        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "meleehitbox")
        {
            hp--;
            float direction = Mathf.Sign(transform.localScale.x);
            audioManager.instance.playRandomAudio(damageSounds, 0.65f, 1, transform, audioManager.instance.sfx);
            float knockback = 2f;
            Vector2 force = new Vector2(-direction, 0);
            rb.AddForce(force * (knockback * 1000f), ForceMode2D.Impulse);
        }

        if (collision.gameObject.tag == "mfHitbox")
        {
            StartCoroutine(death());
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            isGrounded = true;
        }

        if (collision.gameObject.tag == "mfHitbox")
        {
            StartCoroutine(death());
        }

    }
}
