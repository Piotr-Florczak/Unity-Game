using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class robaczek : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.0f; // Szybkość ruchu przeciwnika

    [SerializeField]
    private float jumpForce = 5.0f; // Siła skoku przeciwnika

    [SerializeField]
    private float groundCheckDistance = 0.5f; 

    [SerializeField]
    private float edgeCheckDistance = 1.0f; 

    private Vector2 originalVelocity;
    private Rigidbody2D rb;
    private float direction = 1.0f; 
    private float timeUntilNextJump;
    public bool isFreezeMovementRoutineActive = false;

    public GameObject  animacja_napisu;
    private Vector2 position;
    Transform transformacja;

    public float freezeDuration = 2.0f;
    public float freezeInterval = 5.0f;

    public int maxHealth = 100;
    public int currentHealth;

    public Health_bar healthBar;


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
        if(!isFreezeMovementRoutineActive)
        {
            rb.velocity = new Vector2(speed * direction, rb.velocity.y);

        }
        if (rb.velocity == new Vector2(0,0) && !isFreezeMovementRoutineActive)
        {
            Debug.Log("warunek");
            rb.AddForce(transform.up * speed);

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
    }
    public void takeDamage(int damage)
    {
        position = new Vector2(transform.position.x - 0.7f, transform.position.y+1.2f);
        currentHealth -= damage;
        Debug.Log("?");
        healthBar.SetHeath(currentHealth);
        
        GameObject points = Instantiate(animacja_napisu, position, Quaternion.identity) as GameObject;
        points.transform.GetChild(0).GetComponent<TextMeshPro>().text=Random.Range(5,15).ToString();
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
        }   
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        //ty to zrub





    }

}

