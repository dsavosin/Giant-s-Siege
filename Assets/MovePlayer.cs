using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField]
    Transform startLocation;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(startLocation.position.x, transform.position.y, startLocation.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
