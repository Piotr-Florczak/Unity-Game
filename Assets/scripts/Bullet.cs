using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;
    Rigidbody2D rb;
    Animator anim;
    AnimatorClipInfo[] animatorinfo;
    string current_animation;
    public input_player_movement gracz;
    bool istouching = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.velocity = new Vector2 (-1f,0) * speed;
    }
    void FixedUpdate()
    {
        animatorinfo = this.anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;

         if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && current_animation == "destory")
        {
            Destroy(gameObject);

        }
    }

    void OnTriggerEnter2D (Collider2D hitInfo)
    {
        if (hitInfo.name == "PLAYER")
        {
            istouching = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            anim.SetTrigger("destory");
        }
        
    }
}
