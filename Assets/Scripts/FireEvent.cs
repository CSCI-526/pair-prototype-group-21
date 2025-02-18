using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEvent : MonoBehaviour
{
    public GameObject bulletPrefabA;
    public GameObject bulletPrefabB;

    public Transform firePoint;

    public float fireRate = 0.2f;
    private float nextFireTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
            if (Input.GetKey(KeyCode.Z) && Time.time >= nextFireTime)
            {
                ShootBullet(bulletPrefabA);
                nextFireTime = Time.time + fireRate;
            }   

            if (Input.GetKey(KeyCode.X) && Time.time >= nextFireTime)
            {
                ShootBullet(bulletPrefabB);
                nextFireTime = Time.time + fireRate;
            }

 
    }

    void ShootBullet(GameObject bulletPrefab)
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
