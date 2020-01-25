using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierUnit : MonoBehaviour
{
    [SerializeField]
    float soldierSpeed;

    public Vector3 localInitPosition;
    public Quaternion localInitRotation;
    public bool returnToPosition;
    
    void Start()
    {
        returnToPosition = false;
        localInitPosition = transform.localPosition;
        localInitRotation = transform.rotation;
    }

    void Update()
    {
        if(returnToPosition)
        {
            returnToPosition = false;
            transform.localPosition = localInitPosition;
            transform.rotation = localInitRotation;
        }
    }
}
