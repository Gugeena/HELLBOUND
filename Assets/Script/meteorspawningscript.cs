using System.Collections;
using UnityEngine;

public class meteorspawningscript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject meteor;
    public GameObject[] meteors;
    void Start()
    {
        StartCoroutine(spawnmeteors());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator spawnmeteors()
    {
        yield return new WaitUntil(() => arrowScript.hasShot);
        yield return new WaitForSeconds(0.1f);
        for(int i = 0; i < meteors.Length; i++)
        {
            meteors[i].SetActive(true);
        }
        float x = UnityEngine.Random.RandomRange(39.04f, 49.34f);
        float y = UnityEngine.Random.RandomRange(-2.45f, 6.31f);
        Instantiate(meteor, new Vector2(x,y), meteor.transform.rotation);
        Debug.LogError("you're a dumbass");
        StartCoroutine(spawnmeteors());
    }
}
