
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class input_player_movement : MonoBehaviour
{

    player_animator_controller animation_controller;
    
    bool is_attack;
    bool next_attack;
    private bool isOnCooldown = false;
    bool isGrounded;
    public bool isAttack1 = true;

    public float checkRadius = 0.1f;

    Rigidbody2D fizyka;
    Vector2 dane_wejscowe;
    PlayerInput player;
    Animator animacja;
    

    [SerializeField] float przyspiesznie = 6f;
    [SerializeField] float moc_skoku = 5f;
    [SerializeField] float attackRange = 0.5f;

    public int attakDamage = 40;

    Transform transformacja;
    public Transform attack_colider;
    public Transform groundCheckPoint;

    public LayerMask enemyLayers;
    public LayerMask groundLayer;

    public int maxHealth = 100;
    public int currentHealth;

    public Health_bar healthBar;

    string behaviour;

    void Start()
    {
        animation_controller = GetComponent<player_animator_controller>();
        animation_controller.test();
        fizyka = GetComponent<Rigidbody2D>();
        transformacja = GetComponent<Transform>();
        animacja = GetComponent<Animator>();

        InvokeRepeating("test1",0.0f,0.001f);

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
    void test1()
    {
        animation_controller.controller(dane_wejscowe,isGrounded,is_attack);
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, checkRadius, groundLayer);
        healthBar.SetHeath(currentHealth);
        movement();
        flip();
    }

    // movement -----------------------------

    void movement()
    {
        fizyka.velocity = new Vector2(dane_wejscowe.x * przyspiesznie, fizyka.velocity.y);
       
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

    // input movement -----------------------------

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
    //attack logic -------------------------------
    public void attack_checker()
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

    // others ---------------------------------

    public void test()
    {
        //Debug.Log("ja tez cie widze");
    }
}




