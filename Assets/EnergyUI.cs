using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    public GameObject slider;

    // Start is called before the first frame update
    void Start()
    {
        //TODO check sloder for null obj reference
    }

    // Update is called once per frame
    void Update()
    {
        slider.GetComponent<Slider>().value = EnergyController.instance.energy;
    }
}
