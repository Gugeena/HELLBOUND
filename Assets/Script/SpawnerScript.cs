using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject[] banished, stonedbanished;

    [SerializeField]
    private GameObject skyCandle, eyeBall, stonedskyCandle, stonedeyeBall;

    bool canspawn = true;
    public GameObject spawningparticles;

    public Transform Player;

    public float cooldownTimer;

    int seconds;

    int lastProccesedamountofkills = -1;

    int spawningcount = 1;
    public static bool shouldChangeBack = false;
    enum Enemies { charger = 0 , skyCandle = 1, eyeball = 2 }
    public static bool shouldSpawn = true;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        cooldownTimer = 1f;
        spawningcount = 1;
    }

    void Update()
    {
        if ((int)PauseScript.kill % 10 == 0 && PauseScript.kill != 0 && (int)PauseScript.kill != lastProccesedamountofkills)
        {
            if (cooldownTimer > 0.8f)
            {
                cooldownTimer -= 0.1f;
                if (cooldownTimer < 0.8f) cooldownTimer = 0.8f;
            }
            else if (cooldownTimer <= 0.8f && cooldownTimer > 0.6f)
            {
                cooldownTimer -= 0.2f;
                if (cooldownTimer < 0.6f) cooldownTimer = 0.6f;
            }
            lastProccesedamountofkills = (int)PauseScript.kill;
        }

        if(PauseScript.kill >= 30)
        {
            spawningcount = 2;
        }

        if(shouldChangeBack)
        {
            cooldownTimer = 1f;
            spawningcount = 1;
            //shouldSpawn = true;
            shouldChangeBack = false;
        }
        /*
        if (PauseScript.kill >= 60 && PauseScript.kill > 20)
            spawningcount = 3;
        else if (PauseScript.kill >= 20 && PauseScript.kill < 60)
            spawningcount = 2;
        else
            spawningcount = 1;
        */

        //print(PauseScript.kill + "----kills");
        if (canspawn)
        {
            StartCoroutine(theCoolerDanielSpawner());
        }
    }

    public IEnumerator spawner()
    {
        canspawn = false;
        yield return new WaitForSeconds(cooldownTimer);
        for (int i = 0; i < spawningcount; i++)
        {
            float randomvalue = UnityEngine.Random.Range(0, 1f);
            float randomx = UnityEngine.Random.Range(15.379f, 35.6f);

            float randomOffsetX = UnityEngine.Random.Range(-2f, 2f);
            float randomYEyeball = UnityEngine.Random.Range(1f, 4f);
            if (randomvalue < 0.8f && randomvalue > 0.5f)
            {
                float randomy = UnityEngine.Random.Range(-2.24f, 3.4f);
                Instantiate(spawningparticles, new Vector2(randomx, randomy - 0.75f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Instantiate(banished[1], new Vector2(randomx, randomy), Quaternion.identity);

            }
            else if (randomvalue < 0.5f || randomvalue < 0.8f)
            {
                Instantiate(spawningparticles, new Vector2(randomx, -3.189f - 0.75f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Instantiate(banished[0], new Vector2(randomx, -3.189f), Quaternion.identity);
            }
            else
            {
                Instantiate(spawningparticles, new Vector2(Player.transform.position.x + randomOffsetX + 0.5f, randomYEyeball + 0.5f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
                Instantiate(banished[2], new Vector2(Player.transform.position.x + randomOffsetX, randomYEyeball), Quaternion.identity);
            }
            //yield return new WaitForSeconds(0.5f);
        }
        canspawn = true;
    }

    public IEnumerator theCoolerDanielSpawner()
    {
        if (shouldSpawn)
        {
            canspawn = false;
            yield return new WaitForSeconds(cooldownTimer);
            for (int i = 0; i < spawningcount; i++)
            {
                GameObject[] POOL = TenthLayerOfHellScript.stoned ? stonedbanished : banished;
                int enemyToSpawn = Random.Range(0, POOL.Length);

                float randomx = Random.Range(15.379f, 35.6f);

                Vector2 pos = new Vector2(randomx, 0);

                GameObject enemy = POOL[enemyToSpawn];

                if (Equals(enemy, skyCandle) || Equals(enemy, stonedskyCandle))
                {
                    pos.y = Random.Range(-2.24f, 3.4f);
                }
                else if (Equals(enemy, eyeBall) || Equals(enemy, stonedeyeBall))
                {
                    pos.y = Random.Range(1f, 4f);
                }
                else pos.y = -3.189f;

                if(!Equals(enemy, eyeBall)) Instantiate(spawningparticles, new Vector2(pos.x, pos.y - 0.75f), Quaternion.identity);
                else Instantiate(spawningparticles, new Vector2(pos.x, pos.y), Quaternion.identity);
                Instantiate(POOL[enemyToSpawn], pos, Quaternion.identity);

            }
            canspawn = true;
        }
    }
}
