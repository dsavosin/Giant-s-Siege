using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    public float energy=100;

    public static EnergyController instance;

    public static SocketManager wss;
    
    public string wssUri = "wss://ldss.xyz";

    public float leftHandVelo, rightHandVelo;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        wss = SocketManager.getInstance(wssUri);
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSetEnergy(float val)
    {
        energy += val;
        if (energy > 100)
            energy = 100;
    }

    public void SetVelocity(Limb lim,float velo)
    {
        if (lim.ToString() == "LeftHand")
        {
            leftHandVelo = velo;
        }
        if (lim.ToString() == "RightHand")
        {
            rightHandVelo = velo;
        }
    }

    public void AddEnergy(float velo)
    {
        if (energy < 100)
        {
            energy += velo * Time.deltaTime;
        }
    }
    
    public void SubtractEnergy(float velo)
    {
        if (energy >= 0 && energy < 1000)
        {
            energy -= velo;
        }
    }
}
