using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class robaczek : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f; // Szybkość ruchu przeciwnika

    [SerializeField]
    private float jumpForce = 5.0f; // Siła skoku przeciwnika

    [SerializeField]
    private float groundCheckDistance = 0.5f; 

    [SerializeField]
    private float edgeCheckDistance = 1.0f;

    [SerializeField]
    private int view_distance = 8; 

    private Vector2 originalVelocity;
    private Rigidbody2D rb;
    private float direction = 1.0f; 
    public bool isFreezeMovementRoutineActive = false;
    public bool Is_Rage_mode;

    public GameObject  animacja_napisu;
    Vector2 position;
    Transform transformacja;

    public float freezeDuration = 2.0f;
    public float freezeInterval = 5.0f;

    public int maxHealth = 100;
    public int currentHealth;
    float horizontalDifference;

    public Health_bar healthBar;
    public LayerMask Layer;

    public List<Collider2D> players = new List<Collider2D>();


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transformacja = GetComponent<Transform>();
        currentHealth = maxHealth;

        StartCoroutine(FreezeMovementRoutine());

        healthBar.SetMaxHealth(maxHealth);
    }

    private void FixedUpdate()
    {
        flip();
        if (!Is_Rage_mode)
        {
            if(!isFreezeMovementRoutineActive)
                {
                    rb.velocity = new Vector2(speed * direction, rb.velocity.y);
                }
            if (rb.velocity.x == 0 && !isFreezeMovementRoutineActive)
                {
                    Debug.Log("awaryjne pchnięcie");
                    rb.AddForce(transform.up * speed);
                }
        }
        

        RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance);

        Vector2 edgeCheckDirection = new Vector2(direction, 0);
        RaycastHit2D hitEdge1 = Physics2D.Raycast(transform.position + new Vector3(0, -0.5f, 0), edgeCheckDirection, edgeCheckDistance);
        RaycastHit2D hitEdge2 = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), edgeCheckDirection, edgeCheckDistance);

        // Jeśli przeciwnik utraci kontakt z podłożem lub zbliży się do krawędzi, zmień kierunek
        if (hitGround.collider == null || (hitEdge1.collider == null && hitEdge2.collider == null))
        {
            direction *= -1;
        }

        Collider2D[] collidersInCircle = Physics2D.OverlapCircleAll(transform.position, view_distance, Layer);

        players.Clear(); 

        players.AddRange(collidersInCircle);

        if (players.Count >0) //
        {
            isFreezeMovementRoutineActive = false;
            Is_Rage_mode = true;
            players[0].gameObject.GetComponent<input_player_movement>().test();
            
            horizontalDifference = gameObject.transform.position.x - transform.position.x;

            if( players[0].gameObject.transform.position.x > transform.position.x)
            {
                direction = 1;
                

            }
            else 
            {
                direction = -1;

            }

            rb.velocity = new Vector2((speed*1.2f) * direction, rb.velocity.y);

             if (rb.velocity == new Vector2(0,0) && !isFreezeMovementRoutineActive)
                {
                    rb.AddForce(transform.up * speed);
                }
        }
        else 
        {
            Is_Rage_mode = false;
        }


    }
    void flip()
    {
        if (direction < 0)
        {
            transformacja.localScale = new Vector2(4.5f, transformacja.localScale.y);
        }
        if (direction> 0)
        {
            transformacja.localScale = new Vector2(-4.5f, transformacja.localScale.y);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(transform.position + new Vector3(0, -0.5f, 0), transform.position + new Vector3(0, -0.5f, 0) + new Vector3(direction, 0, 0) * edgeCheckDistance);
        Gizmos.DrawLine(transform.position + new Vector3(0, 0.5f, 0), transform.position + new Vector3(0, 0.5f, 0) + new Vector3(direction, 0, 0) * edgeCheckDistance);
        Gizmos.DrawWireSphere(transform.position, view_distance);

    }

    public void takeDamage(int damage)
    {
        position = new Vector2(transform.position.x - 0.7f, transform.position.y+1.2f);
        currentHealth -= damage;
        
        healthBar.SetHeath(currentHealth);
        position = new Vector2 (transformacja.position.x,transformacja.position.y);
        GameObject points = Instantiate(animacja_napisu, position, Quaternion.identity) as GameObject;
        points.transform.GetChild(0).GetComponent<TextMeshPro>().text=damage.ToString();
        StartCoroutine(RemoveAfterAnimation(points));
        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private IEnumerator RemoveAfterAnimation(GameObject child)  
    {
        Animator animator = child.GetComponent<Animator>();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(child);
    }

    private IEnumerator FreezeMovementRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(freezeInterval);

            isFreezeMovementRoutineActive = true;
            
            originalVelocity = rb.velocity;

            rb.velocity = Vector2.zero;

            yield return new WaitForSeconds(freezeDuration);

            rb.velocity = originalVelocity;

            isFreezeMovementRoutineActive = false;

            rb.AddForce(transform.up * speed);
        }   
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        Destroy(gameObject);
    }

}

