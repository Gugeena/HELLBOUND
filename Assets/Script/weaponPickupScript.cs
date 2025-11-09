using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class weaponPickupScript : MonoBehaviour
{
    [HideInInspector]
    public int weaponID;

    public bool disappear = true;

    enum weapon { random = 0, bow = 1, boomerang = 2, mf = 3, spear = 4, mjolnir = 5 };
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private BoxCollider2D bc;
    private Rigidbody2D rb;

    [SerializeField] private weapon preSelectedWeapon;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (preSelectedWeapon == 0)
        {
            setWeapon(Random.Range(1, 5));
            throwUp();
        }
        else
        {
            setWeapon((int)preSelectedWeapon);
            //bc.enabled = true;
            //StartCoroutine(delay());
        }

        StartCoroutine(deleter());
    }

    private void throwUp()
    {

        rb.AddForce(transform.up * 200);
        int[] t = { -1, 1 };
        rb.AddTorque(Random.Range(170f, 210f) * t[Random.Range(0, 2)]);

        if (disappear) StartCoroutine(delay());
        else bc.enabled = true;
    }

    private void setWeapon(int w)
    {
        weaponID = w;
        sr.sprite = sprites[weaponID - 1];
    }

    private IEnumerator delay()
    {
       // if (this.gameObject.name == "BoomerangWhichYouJustShot") yield break;
        bc.enabled = false;
        yield return new WaitForSeconds(0.6f);
        bc.enabled = true;
    }

    private IEnumerator deleter()
    {
        yield return new WaitForSeconds(6f);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, 2.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }
}