using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultController : MonoBehaviour
{
    public GameObject bullet;

    public Vector3 axis;

    public float fireRate;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Fire", 1, fireRate);
    }


    void Fire()
    {
        GameObject bul = Instantiate(bullet, transform.position, Quaternion.identity);
        bul.GetComponent<CatapultForce>().Launch(axis);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
