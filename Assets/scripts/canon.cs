using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float duration = 0.5f; 

    // Update is called once per frame
    void Start()
    {
        InvokeRepeating("Shoot",1.0f,duration);
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position,firePoint.rotation);
    }
}
