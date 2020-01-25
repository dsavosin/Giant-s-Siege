using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    public float energy=100;

    public static EnergyController instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubtractEnergy(float velo)
    {
        energy -= velo;
    }
}
