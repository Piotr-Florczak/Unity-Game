using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class input_player_movement : MonoBehaviour
{
    Vector2 dane_wejscowe;
    bool is_attack;

    Rigidbody2D fizyka;
    Transform transformacja;
    Animator animacja;
    PlayerInput player;


    [SerializeField] float przyspiesznie = 6f;
    [SerializeField] float moc_skoku = 5f;
    [SerializeField] float attackRange = 0.5f;


    public Transform attack_colider;
    public LayerMask enemyLayers;


    void Start()
    {
        //pobiernie wszystkich funkcji z komonentów w grze do zmiennych 
        fizyka = GetComponent<Rigidbody2D>();      // pobiera wszystkie funkcje komponentu "Rigidbody2d" do zmiennej "fizyka itd."
        transformacja = GetComponent<Transform>();
        animacja = GetComponent<Animator>();

    }

    void Update()
    {
        movement();
        flip();
        if (animacja.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            animacja.SetBool("attack(1)", false);
        }

    }

    void OnMove(InputValue value)             // fukncja wywo³uje siê automatycznie kiedy zostaje wcisniêty klwiesz akcji typu W,A,S,D. Oraz przyjmuje (ze œrodowiska unity) arugumenty typu vector. np kiedy wcisniemy 'D' zmienna typu inputvalue bêdzie siê równaæ (1,0).
    {
        dane_wejscowe = value.Get<Vector2>(); // zmienna inputvalue dzia³a tylko w ciele funkcji onMove dlatego stworzy³em zmienn¹ globaln¹ typu Vecotr2 do której przypisuje te wartoœci ponadto InputValue jest obiektem który posiada wile zmiennych, dlatego getem pobieramy zmienn¹ typu vacotr
    }
    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            fizyka.velocity += new Vector2(fizyka.velocity.x, moc_skoku);
        }
    }
    void OnAttack(InputValue value)
    {
        is_attack = true;
    }
    void OnAttack_relase(InputValue value)
    {
        is_attack = false;
    }
    void attack()
    {
        animatinon_controler("attack");
    }
    void movement()
    {
        fizyka.velocity = new Vector2(dane_wejscowe.x * przyspiesznie, fizyka.velocity.y);
        if(Mathf.Abs(dane_wejscowe.x) > 0)
        {
            animatinon_controler("biega");
        }
        if(fizyka.velocity.y > 1)
        {
            animatinon_controler("skacze");
        }
        if(fizyka.velocity.y < -1)
        {
            animatinon_controler("spada");
        }
        if(fizyka.velocity.y == 0 && fizyka.velocity.x == 0)
        {
            animatinon_controler("nic");
        }
        if(is_attack)
        {
            animatinon_controler("attack");
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

        }
        if (behaviour == "biega") //biega
        {
            animacja.SetBool("Running", true);

            animacja.SetBool("Fallen", false);
            animacja.SetBool("attack(1)", false);
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
        Collider2D[] hitEnemis = Physics2D.OverlapCircleAll(attack_colider.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemis)
        {
            Debug.Log("We hit" + enemy.name);
        }
    }
    void OnDrawGizmosSelected()
    {
        if(attack_colider == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attack_colider.position, attackRange);
    }

}



