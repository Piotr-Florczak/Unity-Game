using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float minDuration = 0.2f; 
    public float maxDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootLoop());
        Debug.Log("c");

    }

    IEnumerator ShootLoop()
    {
        while (true)
        {
            float delay = Random.Range(minDuration, maxDuration);
            yield return new WaitForSeconds(delay);
            Shoot();
        }
    }

    void Shoot()
    {
        Debug.Log("a");
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
