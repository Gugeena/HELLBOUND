using System.Collections;
using UnityEngine;

public class DummySpawner : MonoBehaviour
{
    [SerializeField] private GameObject dummy, spawnParticles;

    private bool carryOn = true;
    public void Spawn(float time)
    {
        StartCoroutine(spawnCRT(time));
    }

    private IEnumerator spawnCRT(float time)
    {
        yield return new WaitForSeconds(time);
        Instantiate(spawnParticles, transform.position, Quaternion.identity);
        GameObject d = Instantiate(dummy, transform.position, Quaternion.identity);
        d.GetComponent<tutorialEnemy>().spawner = this;
        yield break;
    }
}
