using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;
    Rigidbody2D rb;
    Animator anim;
    AnimatorClipInfo[] animatorinfo;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.velocity = new Vector2 (-1f,0) * speed;
    }

    void OnTriggerEnter2D (Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        if (hitInfo.name == "PLAYER")
        {
            anim.SetTrigger("destory");
        }
        
    }
}
