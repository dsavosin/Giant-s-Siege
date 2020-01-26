using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MouthController : MonoBehaviour
{
    [SerializeField]
    public float energyPerSoldier = 10;
    [SerializeField]
    UnityEvent OnEat;

    [SerializeField]
    UnityEvent OnEatGameOver;
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

            OnEat.Invoke();

        }
        if(other.tag== "GameOverFood")
        {
            OnEatGameOver.Invoke();
        }
    }
}
