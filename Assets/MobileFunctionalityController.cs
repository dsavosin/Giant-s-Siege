using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileFunctionalityController : MonoBehaviour
{
    [SerializeField]
    Valve.VR.SteamVR_Fade cameraFade;

    [SerializeField]
    bool test;

    [SerializeField]
    Color color;
    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip[] audios;


    public static MobileFunctionalityController instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            BlurVision();
            test = false;
        }
    }

    public void RecCommand(string command)
    {
        switch (command)
        {
            case "IncreaseDamage":
                MoreDamage();
                break;
            case "AddTrebuchet":

                break;
            case "SpawnReinforcements":
                SpawnReinforcements();
                break;

            case "BlurredVision":
                BlurVision();
                break;
        }
    }

    void SpawnReinforcements()
    {
        source.PlayOneShot(audios[2]);
        EnergyController.instance.SpawnUnit(true);
    }

    void BlurVision()
    {
        source.PlayOneShot(audios[3]);
        cameraFade.OnStartFade(color, 2, true);
        StartCoroutine(DelayBlur());
    }
    IEnumerator DelayBlur()
    {
        yield return new WaitForSeconds(5);
        cameraFade.OnStartFade(new Vector4(100, 100, 100, 0), 2, true);
    }
    void MoreDamage()
    {
        source.PlayOneShot(audios[0]);
        GroundUnit[] units = FindObjectsOfType<GroundUnit>();
        foreach (GroundUnit unit in units)
        {
            unit.unitDamage = 15;
        }
        
    }
    IEnumerator DisableDamage(GroundUnit[] units)
    {
        yield return new WaitForSeconds(5);
        foreach (GroundUnit unit in units)
        {
            unit.unitDamage = 10;
        }
    }
}
