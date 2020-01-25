using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{

    public float DestructionThreshold = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > DestructionThreshold)
            {
                Destroy(gameObject);
            }
        }

        
    }
}
