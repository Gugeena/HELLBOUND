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
    public GameObject spawningparticles, stonedspawningparticles;

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
        shouldSpawn = true;
        shouldChangeBack = false;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        cooldownTimer = 1f;
        spawningcount = 1;
        PauseScript.kill = 0;
        lastProccesedamountofkills = -1;
        StartCoroutine(theCoolerDanielSpawner());
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

        if(PauseScript.kill >= 30 && spawningcount != 2)
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
    }

    public IEnumerator theCoolerDanielSpawner()
    {
        while (true)
        {
            if (shouldSpawn)
            {
                canspawn = false;
                yield return new WaitForSeconds(cooldownTimer);
                for (int i = 0; i < spawningcount; i++)
                {
                    bool isStoned = TenthLayerOfHellScript.stoned;
                    GameObject[] POOL = isStoned ? stonedbanished : banished;
                    GameObject spawnParticles = isStoned ? stonedspawningparticles : spawningparticles;

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

                    if (!Equals(enemy, eyeBall) && !Equals(enemy, stonedeyeBall)) Instantiate(spawnParticles, new Vector2(pos.x, pos.y - 0.75f), Quaternion.identity);
                    else Instantiate(spawnParticles, new Vector2(pos.x, pos.y), Quaternion.identity);

                    Instantiate(POOL[enemyToSpawn], pos, Quaternion.identity);
                }
                canspawn = true;
            }
            else yield return null;
        }
    }
}
