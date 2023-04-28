using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_animator_controller : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    Vector2 dane_wejscowe;
    AnimatorClipInfo[] animatorinfo;

    string current_animation;
    [SerializeField] private bool isAttack1 = false;
    private bool isOnCooldown = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
    }

    void FixedUpdate()
    {

        animatorinfo = this.anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && current_animation == "attack_1")
        {
            anim.SetTrigger("IDLE");
            isAttack1 = !isAttack1;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && current_animation == "attack_2")
        {
            anim.SetTrigger("IDLE");
            isAttack1 = !isAttack1;

        }
    }
    public void controller(Vector2 dane_wejscowe, bool isGrounded,bool is_attack)
    {
        Debug.Log(isOnCooldown);
        

        if(is_attack && isOnCooldown)
        {
            if (isAttack1)
            {
                anim.SetTrigger("Attack1Trigger");
                anim.ResetTrigger("IDLE");
                StartCoroutine(AttackCooldown());
            }
            else
            {   
                anim.SetTrigger("Attack2Trigger");
                StartCoroutine(AttackCooldown());
                anim.ResetTrigger("IDLE");
            }
        }

        if (Mathf.Abs(dane_wejscowe.x ) > 0 && isGrounded && isOnCooldown)
        {
            anim.SetTrigger("Running");
        }
        if(rb.velocity.y > 1 )
        {
            anim.SetTrigger("Jump");
            anim.ResetTrigger("Running");
        }
        if(rb.velocity.y < -1 && !isGrounded )
        {
            anim.SetTrigger("Fall");
            anim.ResetTrigger("Running");

        }
        if((rb.velocity.y == 0 && rb.velocity.x == 0) && isGrounded && isOnCooldown)
        {
            anim.ResetTrigger("Fall");
            anim.SetTrigger("IDLE");
        }
    }

    private IEnumerator AttackCooldown()
    {
        isOnCooldown = false;
        if (current_animation == "attack_1" || current_animation == "attack_2")
        {
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }
        isOnCooldown = true;
    }

    public void test()
    {
        Debug.Log("test");
    }
}
