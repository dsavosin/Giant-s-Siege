using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    public float energy=100;

    public static EnergyController instance;

    public float leftHandVelo, rightHandVelo;

    public GameObject groundUnitPrefab;

    public Transform spawnPoint1, spawnPoint2;

    bool flipSwitch;

    public bool hitCastleFirstTime;


    public bool canSpawn;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        flipSwitch = false;
        hitCastleFirstTime = false;
        canSpawn = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            SpawnUnit();
        }

        if(energy > 50 && canSpawn)
        {
            canSpawn = false;
            SpawnUnit();
        }
    }

    public void SpawnUnit()
    {
        if (flipSwitch)
        {
            Instantiate(groundUnitPrefab, spawnPoint1.position, spawnPoint1.rotation);
            flipSwitch = false;
        }
        else
        {
            Instantiate(groundUnitPrefab, spawnPoint2.position, spawnPoint2.rotation);
            flipSwitch = true;
        }
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

        if(energy <= 50 && canSpawn == false)
        {
            canSpawn = true;
        }
    }
}
