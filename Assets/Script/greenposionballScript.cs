using Unity.VisualScripting;
using UnityEngine;

public class greenposionballScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Sprite[] sprites;
    int random;
    public GameObject[] poisons;
    public Animator animator;
    public GameObject deathParticles;
    void Start()
    {
        random = UnityEngine.Random.Range(0, 2);
        Sprite currentsprite = GetComponent<Sprite>();
        currentsprite = sprites[random];
        animator = GetComponent<Animator>();
        if (random == 0) animator.Play("WithRockAnimationOfGreenPosionRock");
        else animator.Play("WithoutRockAnimationOfGreenPoisonRock");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if(obj.layer == 8 && random == 0)
        {
            GameObject particle = Instantiate(deathParticles, new Vector2(this.transform.position.x, this.transform.position.y - 0.70f), deathParticles.transform.rotation);
            Renderer renderer = particle.GetComponent<Renderer>();
            renderer.sortingOrder = -601;
            Destroy(gameObject);
        }
        else if(obj.layer == 3 && obj.name != "Movable")
        {
            Instantiate(deathParticles, new Vector2(this.transform.position.x, this.transform.position.y - 0.5f), deathParticles.transform.rotation);
            Destroy(gameObject);
        }
    }
}
