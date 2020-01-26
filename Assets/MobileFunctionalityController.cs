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
    MouthController mouthCtrl;
    [SerializeField]
    GameObject poisonCloud;
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

    public void RecCommand(string e)
    {
        
        switch (e)
        {
            case "IncreaseDamage":
                MoreDamage();
                break;
            case "PoisonousKnights":
                PoisonKnights();
                break;
            case "IncreaseReinforcements":
                SpawnReinforcements();
                break;

            case "BlurredVision":
                BlurVision();
                break;
        }
    }

    void SpawnReinforcements()
    {
        Debug.Log("Spawn");
        //source.PlayOneShot(audios[2]);
        EnergyController.instance.SpawnUnit(true);
    }

    void PoisonKnights()
    {
        List<GameObject> clouds = new List<GameObject>();
        mouthCtrl.energyPerSoldier = 2;
        GroundUnit[] units = FindObjectsOfType<GroundUnit>();
        foreach (GroundUnit unit in units)
        {
            GameObject poison=Instantiate(poisonCloud, unit.transform.position,unit.transform.rotation);
            poison.transform.parent = unit.transform;
            clouds.Add(poison);
        }
        StartCoroutine(UnpoisonKnights(clouds));

    }

    IEnumerator UnpoisonKnights(List<GameObject> poison)
    {
        yield return new WaitForSeconds(5);
        foreach(GameObject poi in poison)
        {
            Destroy(poi);
        }
        mouthCtrl.energyPerSoldier = 10;
    }

    void BlurVision()
    {
        //source.PlayOneShot(audios[3]);
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
        //source.PlayOneShot(audios[0]);
        GroundUnit[] units = FindObjectsOfType<GroundUnit>();
        foreach (GroundUnit unit in units)
        {
            unit.unitDamage = 15;
        }
        StartCoroutine(DisableDamage(units));
        
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
