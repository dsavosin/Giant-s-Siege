using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Transform[] soldiers;

    Transform playerTarget;
    Transform gazeTarget;

    // Start is called before the first frame update
    void Start()
    {
        //Fill in array of soldiers
        soldiers = GetComponentsInChildren<Transform>();

        playerTarget = Camera.main.transform;

        if(GameObject.FindObjectOfType<GazePoint>() != null)
        {
            gazeTarget = GameObject.FindObjectOfType<GazePoint>().transform;
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

        Vector3 playerFloorPos = new Vector3(playerTarget.position.x, 0.0f, playerTarget.position.z);
        Quaternion quarternion = Quaternion.LookRotation(playerFloorPos - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, quarternion, rotationSpeed * Time.deltaTime);

        //If distance between this unit and a player is more than attackRadius, run towars it.
        if(Vector3.Distance(playerFloorPos, transform.position) > attackRadius)
        {
            float step = unitSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerFloorPos, step);
        }
    }


    public void SmackEm()
    {
        foreach(Transform soldier in soldiers)
        {
            Rigidbody rb = soldier.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionLift);

                //Disable animation controllers per entity
            }
        }
    }

    public void ScareEm()
    {
        foreach (Transform soldier in soldiers)
        {
            Rigidbody rb = soldier.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = false;

                Vector3 randDir = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
                rb.velocity = randDir;

                //Disable animation controllers per entity
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.CompareTag("Hand"))
        {
            Debug.Log("Ground Unit collided with a hand!");
            SmackEm();
            StartCoroutine(DestroyUnit(4.0f));
        }

        if (other.gameObject.CompareTag("GazePoint"))
        {
            Debug.Log("Giant looks at this unit");
            ScareEm();
            StartCoroutine(ReturnToUnit(4.0f));
        }
    }

    IEnumerator ReturnToUnit(float delay = 0.0f)
    {
        if (delay != 0)
            yield return new WaitForSeconds(delay);

        // The rest of your coroutine here
        foreach (Transform soldier in soldiers)
        {
            Rigidbody rb = soldier.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = Vector3.zero;
            }

            SoldierUnit singleUnit = soldier.GetComponent<SoldierUnit>();
            if(singleUnit != null)
            {
                singleUnit.returnToPosition = true;
            }
        }
    }

    IEnumerator DestroyUnit(float delay = 0.0f)
    {
        if (delay != 0)
            yield return new WaitForSeconds(delay);

        // The rest of your coroutine here
        foreach (Transform soldier in soldiers)
        {
            Destroy(soldier.gameObject);
        }

        Destroy(gameObject);
    }
}
