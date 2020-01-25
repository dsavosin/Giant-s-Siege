using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthController : MonoBehaviour
{
    [SerializeField]
    float energyPerSoldier = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Food")
        {
            EnergyController.instance.AddSetEnergy(energyPerSoldier);
            Destroy(other.transform.gameObject);

        }
    }
}
