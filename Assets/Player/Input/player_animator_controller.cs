using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class player_animator_controller : MonoBehaviour
{
    input_player_movement movement_player;
    Animator anim;
    Rigidbody2D rb;
    Vector2 dane_wejscowe;
    AnimatorClipInfo[] animatorinfo;

    string current_animation;
    [SerializeField] private bool isAttack1 = false;
    public bool freeze_attack = false;
    public bool freeze_IDLE = false;
    public float sec = 2.0f;

    void Start()
    {
        movement_player = GetComponent<input_player_movement>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("test3",0.0f,0.001f);
    }

    void test3()
    {
        animatorinfo = this.anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && current_animation == "attack_1")
        {
            isAttack1 = !isAttack1;
            anim.SetTrigger("IDLE");
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && current_animation == "attack_2")
        {
            isAttack1 = !isAttack1;
            anim.SetTrigger("IDLE");
        }
    }
    void OnAttack(InputValue value)
    {
        freeze_IDLE = true;
        if(!freeze_attack && !movement_player.death)
        {
            movement_player.attack_checker();
            if (isAttack1)
            {
                anim.ResetTrigger("IDLE");
                anim.SetTrigger("Attack1Trigger");
                StartCoroutine(AttackCooldown());
            }
            else
            {
                anim.ResetTrigger("IDLE");
                anim.SetTrigger("Attack2Trigger");
                StartCoroutine(AttackCooldown());
            }
        }

    }
    public void controller(Vector2 dane_wejscowe, bool isGrounded,bool is_attack)
    {
        
        if (Mathf.Abs(dane_wejscowe.x ) > 0 && isGrounded && !movement_player.death )
        {
            if(freeze_attack)
            {
                anim.ResetTrigger("Running");
            }
            else
            {
                anim.SetTrigger("Running");
            }
        }
        if(rb.velocity.y > 1 && !movement_player.death )
        {
            anim.SetTrigger("Jump");
            anim.ResetTrigger("Running");
        }
        if(rb.velocity.y < -1 && !isGrounded && !movement_player.death )
        {
            anim.SetTrigger("Fall");
            anim.ResetTrigger("Running");

        }
        if((rb.velocity.y == 0 && rb.velocity.x == 0) && isGrounded && !freeze_IDLE && !movement_player.death)
        {
            anim.ResetTrigger("Fall");
            anim.SetTrigger("IDLE");
        }
        if(movement_player.death)
        {
            anim.ResetTrigger("IDLE");
            anim.SetTrigger("death");
        }
    }

    private IEnumerator AttackCooldown()
    {
       freeze_attack = true;
        {
            yield return new WaitForSeconds(0.44f);
        }
        
       freeze_attack = false;
       freeze_IDLE = false;
    }
}
