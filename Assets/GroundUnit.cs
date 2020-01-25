using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GroundUnit : MonoBehaviour
{
    [SerializeField]
    float explosionForce;

    [SerializeField]
    float explosionRadius;

    [SerializeField]
    float explosionLift;

    [SerializeField]
    float rotationSpeed;

    [SerializeField]
    float attackRadius;

    [SerializeField]
    float unitSpeed;
    [SerializeField]
    float unitDamage;
    public Transform[] soldiers;
    [SerializeField]
    UnityEvent OnScare;
    [SerializeField]
    UnityEvent OffScare;
    Transform playerTarget;
    Transform gazeTarget;
    Transform regroupPoint;
    
    bool killed=false;
    // Start is called before the first frame update
    void Start()
    {
        //Fill in array of soldiers
        soldiers = GetComponentsInChildren<Transform>();

        //foreach(Transform soldier in soldiers)
        //{
        //    transform.parent = null;
        //}

        playerTarget = Camera.main.transform;

        if (GameObject.FindObjectOfType<GazePoint>() != null)
        {
            gazeTarget = GameObject.FindObjectOfType<GazePoint>().transform;
        }

        regroupPoint = GameObject.FindGameObjectWithTag("RegroupPoint").transform;
        if(regroupPoint == null)
        {
            Debug.LogAssertion("No retreat point on the level!!!!");
        }

        // Default unit rotation to start looking at the player despite its spawned rotation
        Vector3 playerFloorPos = new Vector3(playerTarget.position.x, 0.0f, playerTarget.position.z);
        Quaternion quarternion = Quaternion.LookRotation(playerFloorPos - transform.position);
        transform.rotation = quarternion;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SmackEm();
            StartCoroutine(DestroyUnit(4.0f));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ScareEm();
            StartCoroutine(ReturnToUnit(1.0f));
        }

        if (!killed)
        {
            Vector3 playerFloorPos = new Vector3(playerTarget.position.x, 0.0f, playerTarget.position.z);
            Quaternion quarternion = Quaternion.LookRotation(playerFloorPos - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, quarternion, rotationSpeed * Time.deltaTime);

            //If distance between this unit and a player is more than attackRadius, run towars it.
            if (Vector3.Distance(playerFloorPos, transform.position) > attackRadius)
            {
                float step = unitSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, playerFloorPos, step);
            }
            else
            {
                EnergyController.instance.SubtractEnergy(unitDamage * Time.deltaTime);
            }
        }
    }


    public void SmackEm()
    {
        killed = true;
        foreach(Transform soldier in soldiers)
        {
            if (soldier != this.transform && soldier != null)
            {

                Rigidbody rb = soldier.GetComponent<Rigidbody>();
                soldier.GetComponent<SoldierUnit>().eaten = true;
                if (rb != null)
                {
                    rb.constraints = RigidbodyConstraints.None;
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionLift);

                    //Disable animation controllers per entity
                }
            }
        }
    }

    public void ScareEm()
    {
        OnScare.Invoke();
        foreach (Transform soldier in soldiers)
        {
            if (soldier != this.transform && soldier != null)
            {
                Rigidbody rb = soldier.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = false;

                    rb.constraints = RigidbodyConstraints.FreezeRotation;

                    rb.AddExplosionForce(10f, transform.position, 5f, 0.0f);
                    //Vector3 randDir = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
                    //rb.velocity = randDir;

                    //Disable animation controllers per entity
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.CompareTag("LeftHand")||other.gameObject.CompareTag("RightHand"))
        {
            float velo = 0;
            if (other.gameObject.CompareTag("LeftHand"))
            {
                velo = EnergyController.instance.leftHandVelo;
            }
            else
            {
                velo = EnergyController.instance.rightHandVelo;
            }
            if (velo > 3)
            {
                Debug.Log("Ground Unit collided with a hand!");
                SmackEm();
                StartCoroutine(DestroyUnit(4.0f));
            }
        }
        if (other.tag == "Interactable")
        {
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            if (rb.velocity.magnitude > 3)
            {
                Destroy(other.gameObject);
                SmackEm();
                StartCoroutine(DestroyUnit(4.0f));
            }

        }
    }

    public IEnumerator ReturnToUnit(float delay = 0.0f)
    {
        if (delay != 0)
            yield return new WaitForSeconds(delay);


       
        //if(regroupPoint != null)
        //transform.position = regroupPoint.position;

        // The rest of your coroutine here
        foreach (Transform soldier in soldiers)
        {
            if (soldier != this.transform && soldier != null)
            {
                Rigidbody rb = soldier.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.constraints = RigidbodyConstraints.None;
                   // rb.velocity = Vector3.zero;
                    rb.isKinematic = true;
                }

                SoldierUnit singleUnit = soldier.GetComponent<SoldierUnit>();
                if (singleUnit != null)
                {
                    singleUnit.returnToPosition = true;
                }
            }
        }
        OffScare.Invoke();
    }

    IEnumerator DestroyUnit(float delay = 0.0f)
    {
        if (delay != 0)
            yield return new WaitForSeconds(delay);

        // The rest of your coroutine here
        foreach (Transform soldier in soldiers)
        {
            if(soldier != null)
            Destroy(soldier.gameObject);
        }

        Destroy(gameObject);
    }
}
