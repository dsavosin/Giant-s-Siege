using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbEnergyController : MonoBehaviour
{
    [SerializeField]
    Valve.VR.InteractionSystem.Hand controller;
    enum Limb {LeftHand,RightHand,LeftLeg,RightLeg,Head}
    [SerializeField]
    Limb limb;
    [SerializeField]
    float speed;

    [SerializeField]
    float minEnergyTreshold;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speed = controller.GetTrackedObjectVelocity().magnitude;
        if (speed > minEnergyTreshold)
        {
            EnergyController.instance.SubtractEnergy(speed - minEnergyTreshold);
            
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if (speed > minEnergyTreshold)
        {
            Destroy(other.gameObject);
            StartCoroutine(DisableCollider());
        }

    }
    IEnumerator DisableCollider()
    {
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        yield return new WaitForSeconds(2);
        col.enabled = true;
    }
}
