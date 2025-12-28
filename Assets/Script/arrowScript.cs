using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowScript : MonoBehaviour
{
    private Rigidbody2D rb;

    private bool landed = false;
    private bool back = false;


    [SerializeField]
    private float returnSpeed = 20;
    [SerializeField]
    private GameObject returnHB;
    [SerializeField]
    private AudioClip land, returnSound;

    private int teleportCount = 0, killcount;
    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (player == null)
        {
            Destroy(gameObject);
        }

        if (rb.linearVelocity != Vector2.zero && !landed)
        {
            Vector2 direction = rb.linearVelocity;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        else if (back)
        {
            backing();
        }
    }

    private void backing()
    {
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) player = GameObject.Find("Player_Tutorial");
        Transform playerTransform = player.transform;
         

        Vector2 direction = playerTransform.position - this.transform.position;

        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, returnSpeed * Time.deltaTime);
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private IEnumerator goBack()
    {
        yield return new WaitForSeconds(0.5f);
        returnHB.SetActive(true);
        back = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            landed = true;
            rb.simulated = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            if (PlayerMovement.shouldMakeSound) audioManager.instance.playAudio(land, 1f, 1, transform, audioManager.instance.sfx);
            StartCoroutine(goBack());
        }

        if (collision.gameObject.name == "LLocation")
        {
            if (teleportCount >= 1) killcount = 0;
            Vector2 vel = rb.linearVelocity;
            rb.MovePosition(new Vector2(SidePortalScript.RLocation.position.x, transform.position.y));
            teleportCount++;
            rb.linearVelocity = vel;
        }
        else if (collision.gameObject.name == "RLocation")
        {
            if (teleportCount >= 1) killcount = 0;
            Vector2 vel = rb.linearVelocity;
            rb.MovePosition(new Vector2(SidePortalScript.LLocation.position.x, transform.position.y));
            teleportCount++;
            rb.linearVelocity = vel;
        }
    }

    public int getteleportCount()
    {
        return teleportCount;
    }

    public void increaseKillCount()
    {
        if (teleportCount == 0) return;
        killcount++;
    }

    public int getKillCount()
    {
        return killcount;
    }
}
