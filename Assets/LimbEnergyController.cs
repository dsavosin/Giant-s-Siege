using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbEnergyController : MonoBehaviour
{
    [SerializeField]
    SteamVR_Behaviour_Pose controller;
    enum Limb {LeftHand,RightHand,LeftLeg,RightLeg}
    [SerializeField]
    Limb limb;

    [SerializeField]
    float energy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
