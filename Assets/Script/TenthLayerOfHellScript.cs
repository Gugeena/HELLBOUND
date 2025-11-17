using System.Collections;
using UnityEngine;

public class TenthLayerOfHellScript : MonoBehaviour
{
    public GameObject fireball;
    Animator TLOHanim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(EventManager());
        TLOHanim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator EventManager()
    {
        int random = UnityEngine.Random.Range(0, 2);
        switch(random)
        {
            case 0: StartCoroutine(Fireballrain()); break;
            case 1: StartCoroutine(snakeattack()); break;
        }
        yield break;
    }

    public IEnumerator Fireballrain()
    {
        for (int i = 0; i < 40; i++)
        {
            yield return new WaitForSeconds(0.25f);
            float x1 = UnityEngine.Random.Range(14.07f, 36.05f);
            Instantiate(fireball, new Vector2(x1, 6.86f), Quaternion.identity);
        }
        StartCoroutine(EventManager());
    }

    public IEnumerator snakeattack()
    {
        string animation;
        int random = UnityEngine.Random.Range(0, 3);
        if (random == 0) animation = "SnakeHorizontal";
        else if (random == 1) animation = "SnakeVertical";
        else animation = "SnakeBoth";
        TLOHanim.Play(animation);
        yield return new WaitForSeconds(4f);
        StartCoroutine(EventManager());
    }
}
