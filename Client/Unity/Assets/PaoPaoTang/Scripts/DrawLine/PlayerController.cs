using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 1f;

    public void Update()
    {
        if(Input.GetKey(KeyCode.D))
            rb.velocity = new Vector2(speed, rb.velocity.y);
        else if(Input.GetKey(KeyCode.A))
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            rb.velocity = new Vector2(0, rb.velocity.y);
    }
}
