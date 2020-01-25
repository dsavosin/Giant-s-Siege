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
    [HideInInspector]
    public bool eaten = false;
    //Store delta to parent and de-parent them

    void Start()
    {
        returnToPosition = false;
        localInitPosition = transform.localPosition;
        localInitRotation = transform.localRotation;
    }

    void Update()
    {
        if (returnToPosition&!eaten)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, localInitPosition, Time.deltaTime);
            transform.localRotation = localInitRotation;
        }
    }

    public void DisableSoldier()
    {
        transform.parent = null;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        tag = "Food";
        eaten = true;
    }
}
