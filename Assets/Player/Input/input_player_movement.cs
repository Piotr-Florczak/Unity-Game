
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class input_player_movement : MonoBehaviour
{
    
    bool is_attack;
    bool next_attack;
    private bool isOnCooldown = false;
    private bool isGrounded;

    public float checkRadius = 0.1f;

    Rigidbody2D fizyka;
    Vector2 dane_wejscowe;
    Animator animacja;
    PlayerInput player;

    [SerializeField] float przyspiesznie = 6f;
    [SerializeField] float moc_skoku = 5f;
    [SerializeField] float attackRange = 0.5f;

    public int attakDamage = 40;

    Transform transformacja;
    public Transform attack_colider;
    public Transform groundCheckPoint;

    public LayerMask enemyLayers;
    public LayerMask groundLayer;


    AnimatorClipInfo[] animatorinfo;
    string current_animation;

    public int maxHealth = 100;
    public int currentHealth;

    public Health_bar healthBar;


    void Start()
    {
        fizyka = GetComponent<Rigidbody2D>();
        transformacja = GetComponent<Transform>();
        animacja = GetComponent<Animator>();

        currentHealth = maxHealth;

        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, checkRadius, groundLayer);
        healthBar.SetHeath(currentHealth);
        movement();
        flip();
        animatorinfo = this.animacja.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;
        {
            //Debug.Log(current_animation); 
        }
        if (animacja.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && current_animation == "attack_2")
        {
            animacja.SetBool("attack(1)", false);
        }
        if (animacja.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && current_animation == "attack_2")
        {
            animacja.SetBool("attack(2)", false);
            next_attack = false;

        }
    }

    void OnMove(InputValue value)             
    {
        dane_wejscowe = value.Get<Vector2>(); 
    }
    void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            fizyka.velocity += new Vector2(fizyka.velocity.x, moc_skoku);
        }
    }
    void OnAttack(InputValue value)
    {
        PerformAttack();
        is_attack = true;

    }
    void OnAttack_relase(InputValue value)
    {
        is_attack = false;
    }

    public void PerformAttack()
    {
        if (!isOnCooldown)
        {  
            animatinon_controler("attack");
        
            animatinon_controler("attack_OnMove");

            
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        isOnCooldown = true;

        attack_checker();

        yield return new WaitForSeconds(animacja.GetCurrentAnimatorStateInfo(0).length * 0.4f);

        isOnCooldown = false;
    }

    void movement()
    {
        fizyka.velocity = new Vector2(dane_wejscowe.x * przyspiesznie, fizyka.velocity.y);
        if(Mathf.Abs(dane_wejscowe.x) > 0)
        {
            animatinon_controler("biega");
            if(is_attack)
            {
                animatinon_controler("attack");
            }
        }
        if(fizyka.velocity.y > 1)
        {
            animatinon_controler("skacze");
        }
        if(fizyka.velocity.y < -1)
        {
            animatinon_controler("spada");
        }
        if((fizyka.velocity.y == 0 && fizyka.velocity.x == 0))
        {
            animatinon_controler("nic");
        }
    }

    void animatinon_controler(string behaviour)
    {
        if(behaviour == "attack")
        {       
                animacja.SetBool("attack(1)", true);
                animacja.SetBool("Fallen", false);
                animacja.SetBool("Jump", false);
                animacja.SetBool("Running", false);

                if(next_attack)
                {
                    animacja.SetBool("attack(2)", true);
                    animacja.SetBool("Fallen", false);
                    animacja.SetBool("Jump", false);
                    animacja.SetBool("Running", false);

                }
                next_attack = true;

        }                
        if (behaviour == "biega") //biega
        {
            animacja.SetBool("Running", true);
            animacja.SetBool("Fallen", false);
            animacja.SetBool("attack(2)",false);
            animacja.SetBool("attack(1)",false);

        }
        if (behaviour=="skacze") // skacze
        {
            animacja.SetBool("Jump", true);

            animacja.SetBool("Running", false);
            animacja.SetBool("Fallen", false);
            animacja.SetBool("attack(1)", false);

        }
        if (behaviour=="spada") // spada
        {
            animacja.SetBool("Fallen", true);

            animacja.SetBool("Running", false);
            animacja.SetBool("Jump", false);
            animacja.SetBool("attack(1)", false);
        }

        if (behaviour == "nic") //nic nie robi
        {
            animacja.SetBool("Fallen", false);
            animacja.SetBool("Jump", false);
            animacja.SetBool("Running", false);
            animacja.SetBool("attack(2)", false);
        }
    }

    void flip()
    {
        if (dane_wejscowe.x < 0)
        {
            transformacja.localScale = new Vector2(-4.5f, transformacja.localScale.y);
        }
        if (dane_wejscowe.x > 0)
        {
            transformacja.localScale = new Vector2(4.5f, transformacja.localScale.y);
        }
    }

    void attack_checker()
    {
        Collider2D[] hitEnemis = Physics2D.OverlapCircleAll(attack_colider.position, attackRange, enemyLayers); //do tablicy przypisywane sa wszystkie obiekty ktore znajduja sie w zdefiniowanym opszarze

        foreach (Collider2D enemy in hitEnemis)
        {
            Debug.Log("We hit" + enemy.name);
            enemy.GetComponent<robaczek>().takeDamage(attakDamage);
        }
    }

    void OnDrawGizmosSelected() //rysuje obszar na ekranie  
    {
        if(attack_colider == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attack_colider.position, attackRange);
    }


    public void test()
    {
        //Debug.Log("ja tez cie widze");
    }

    

}




