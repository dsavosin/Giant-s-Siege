using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultForce : MonoBehaviour
{
    [SerializeField]
    float thrust;
    [SerializeField]
    Vector3 launchAxis;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch(Vector3 axis)
    {
        rb.AddExplosionForce(thrust, new Vector3(0,0,0), 1, 10.0F);
    }
}
