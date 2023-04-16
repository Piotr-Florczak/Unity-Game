using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class dummy : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;
    public GameObject  animacja_napisu;
    private Vector2 position;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void takeDamage(int damage)
    {
        position = new Vector2(transform.position.x - 0.7f, transform.position.y+1.2f);
        currentHealth -= damage;
        
        GameObject points = Instantiate(animacja_napisu, position, Quaternion.identity) as GameObject;
        points.transform.GetChild(0).GetComponent<TextMeshPro>().text=Random.Range(5,15).ToString();
        StartCoroutine(RemoveAfterAnimation(points));

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("Enemy died!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator RemoveAfterAnimation(GameObject child)  
    {
    // Pobierz Animator komponent
    Animator animator = child.GetComponent<Animator>();

    // Czekaj na zakończenie animacji
    yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

    // Usuń obiekt z hierarchii sceny
    Destroy(child);
    }


}
