using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilithMeteorScript : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject target;
    Vector2 dir;
    float speed = 0.9f;
    bool canmore = true;
    GameObject spawnbacklocation;
    public GameObject fadeIn;
    public GameObject[] lilithSpawnStuff;
    public camShakerScript cum;
    public bool shouldstart = false;
    public GameObject lilith;
    public Transform lilithspawninglocation;
    public GameObject hpfader;
    public GameObject[] lilithcandles;

    [SerializeField]
    private ParticleSystem particles;

    [SerializeField]
    private Animator impactAnim;

    [SerializeField]
    private ShakeSelfScript sSelf;
    [SerializeField]
    private GameObject groundFire, crack;
    [SerializeField]
    private Sprite flamelessComet;

    [SerializeField]
    private AudioClip sequenceAudio;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        audioManager.instance.playAudio(sequenceAudio, 1, 1, transform, audioManager.instance.sfx);
        yield return new WaitForSeconds(1f);
        cum = GetComponent<camShakerScript>();
        sSelf = GetComponent<ShakeSelfScript>();
        StartCoroutine(cum.shake());
        yield return new WaitForSeconds(0.5f);
        target = GameObject.Find("Tracker");
        rb = GetComponent<Rigidbody2D>();
        dir = target.transform.position - this.transform.position;
        spawnbacklocation = GameObject.Find("LLOCATIONLOCATION (2)");
        shouldstart = true;
        PlayerMovement.canPause = false;
    }

    private void OnDestroy()
    {
        PlayerMovement.canPause = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldstart)
        {
            if(rb != null && dir != null && speed != null) rb.linearVelocity = dir * speed;
            StartCoroutine(speedBuildUp());
        }
    }

    public IEnumerator speedBuildUp()
    {
        if (!canmore) yield break;
        canmore = false;
        yield return new WaitForSeconds(0.4f);
        speed += 0.2f;
        canmore = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "LLocation (2)")
        {
            this.transform.position = new Vector2(spawnbacklocation.transform.position.x, this.transform.position.y - 0.8f);
        }
        if(collision.gameObject.name == "Ground")
        {
            StartCoroutine(lilithSpawning());
        }
    }

    public IEnumerator lilithSpawning()
    {
        rb.simulated = false;
        sSelf.Begin();
        groundFire.transform.parent = null;
        groundFire.SetActive(true);
        GetComponent<Animator>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = flamelessComet;
        impactAnim.Play("impact");
        cum.Stop();
        cum.amplitude = 3.5f;
        cum.time = 0.5f;
        StartCoroutine(cum.shake());
        yield return new WaitForSeconds(1f);
        cum.Stop();
        cum.amplitude = 5f;
        cum.time = 4f;
        StartCoroutine(cum.shake());
        particles.Play();
        yield return new WaitForSeconds(1f);
        crack.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        audioManager.instance.startLillith();
        fadeIn.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        crack.SetActive(false);
        Destroy(particles.gameObject);
        Destroy(groundFire);
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        circle.enabled = false;
        yield return new WaitForSeconds(1.5f);
        for(int i = 0; i < lilithSpawnStuff.Length; i++)
        {
            if(lilithSpawnStuff[i] != null) lilithSpawnStuff[i].SetActive(true);
        }
        Instantiate(lilith, lilithspawninglocation.position, Quaternion.identity);
        yield return new WaitForSeconds(0.65f);
        hpfader.SetActive(true);
        for (int i = 0; i < lilithcandles.Length; i++)
        {
            if (lilithcandles[i] != null) lilithcandles[i].SetActive(true);
        }
        Destroy(lilithspawninglocation.gameObject);
        Destroy(target);
        cum.Stop();
        Destroy(gameObject);
    }
}
