using UnityEngine;

public class LogScript : MonoBehaviour
{
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    // Update is called once per frame
    void Update()
    {
        rb.linearVelocityX = 5;
    }
}
