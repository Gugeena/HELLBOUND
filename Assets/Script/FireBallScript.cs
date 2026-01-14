using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallScript : MonoBehaviour
{
    public GameObject deathParticles;
    public bool isProjectile;
    public GameObject firePillar;
    public Transform RLocation;
    public Transform LLocation;
    // Start is called before the first frame update

    void Start()
    {
        StartCoroutine(failsafe());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("llocation"))
        {
            if (RLocation == null) RLocation = GameObject.Find("RLOCATIONLOCATION").transform;
            this.transform.position = new Vector3(RLocation.position.x - 0.3f, this.transform.position.y, this.transform.position.z);
            return;
        }
        else if (obj.CompareTag("rlocation"))
        {
            if(LLocation == null) LLocation = GameObject.Find("LLOCATIONLOCATION").transform;
            this.transform.position = new Vector3(LLocation.position.x + 0.3f, this.transform.position.y, this.transform.position.z);
            return;
        }

        if (obj.layer == 3 && obj.name == "Ground" && isProjectile)
        {
            Instantiate(deathParticles, new Vector2(this.transform.position.x, this.transform.position.y - 0.5f), deathParticles.transform.rotation);
            //Instantiate(firePillar, new Vector2(this.transform.position.x, 1f), Quaternion.identity);
            Destroy(gameObject);
        }
        else if (obj.layer == 3 && !isProjectile)
        {
            Instantiate(deathParticles, new Vector2(this.transform.position.x, this.transform.position.y + 0.25f), Quaternion.identity);
            Quaternion rotation = obj.name != "Ground" ? Quaternion.Euler(0, 0, 180)  : Quaternion.identity;
            float y = obj.name != "Ground" ? 0 : 1;
            Instantiate(firePillar, new Vector2(this.transform.position.x, y), rotation);
            Destroy(gameObject);
        }
        else if (obj.tag == "Player")
        {
            Instantiate(deathParticles, new Vector2(this.transform.position.x, this.transform.position.y - 0.25f), Quaternion.identity);
            if(!isProjectile) Instantiate(firePillar, new Vector2(obj.transform.position.x, 1f), Quaternion.identity);
            Destroy(gameObject);
        }
        /*
        else if (collision.gameObject.tag == "mfHitbox")
        {
            Instantiate(deathParticles, new Vector2(this.transform.position.x, this.transform.position.y - 0.25f), Quaternion.identity);
            Destroy(gameObject);
        }
        */
    }

    public IEnumerator failsafe()
    {
        yield return new WaitForSeconds(5f);
        Instantiate(deathParticles, new Vector2(this.transform.position.x, this.transform.position.y - 0.25f), Quaternion.identity);
        Destroy(gameObject);
    }
}
