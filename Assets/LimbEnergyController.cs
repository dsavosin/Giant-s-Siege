using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    float minEnergyThreshold;
    [SerializeField]
    float maxEnergyThreshold;
    [SerializeField]
    float BaseEnergyDeincrementalRatio;

    [SerializeField] private float energyIncrementPerTick = 0.01f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        calculateSubtractableEnergy();
        energyRegeneration();
    }

    private void energyRegeneration()
    {
        EnergyController.instance.AddEnergy(energyIncrementPerTick);
    }
    
    private void calculateSubtractableEnergy()
    {
        if (speed < minEnergyThreshold) return;
        speed = controller.GetTrackedObjectVelocity().magnitude;
        if(speed < maxEnergyThreshold)
        {
            speed = maxEnergyThreshold;
        }

        EnergyController.instance.SubtractEnergy(this.CalculateEnergySubtraction());
    }
    
    private float CalculateEnergySubtraction()
    {
      var energyPenaltyPercentage = EnergyController.instance.energy;
      return speed * BaseEnergyDeincrementalRatio * energyPenaltyPercentage * Time.deltaTime;
    }

    void OnTriggerEnter (Collider other)
    {
        if (speed > minEnergyThreshold)
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
