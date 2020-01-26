using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DestructableObject : MonoBehaviour
{

    [SerializeField]
    float health;

    [SerializeField]
    float damageReceive;


    public Transform hitFX;

    float waitForSeconds;

    public float DestructionThreshold = 1;

    [SerializeField]
    UnityEvent OnHit;
    // Start is called before the first frame update
    void Start()
    {
        waitForSeconds = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        waitForSeconds += Time.deltaTime;
        if(waitForSeconds >= 3.0f)
        {
            waitForSeconds = 3.0f;
        }


        if (Input.GetKeyDown(KeyCode.P))
        {
            iTween.PunchPosition(gameObject, iTween.Hash("amount", new Vector3(0.5f, 0.5f, 0.5f), "time", 1, "delay", 0.1, "loopType", "none"));

            //if(hitFX != null)
                //hitFX.GetComponentInChildren<ParticleSystem>().Play();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        bool dealDamage = false;
        //Debug.Log(collision.transform.tag);

        if (collision.gameObject.CompareTag("LeftHand"))
        {
            if (EnergyController.instance.leftHandVelo > DestructionThreshold && waitForSeconds >= 3.0f)
            {
                dealDamage = true;
                waitForSeconds = 0.0f;
            }
        }

        if (collision.gameObject.CompareTag("RightHand"))
        {
            if (EnergyController.instance.rightHandVelo > DestructionThreshold && waitForSeconds >= 3.0f)
            {
                dealDamage = true;
                waitForSeconds = 0.0f;
            }
        }


        if (collision.gameObject.CompareTag("Interactable"))
        {
            Rigidbody rb = collision.gameObject.GetComponentInParent<Rigidbody>();
           
            if (rb != null)
            {
                Debug.Log(rb.velocity.magnitude);
                if(rb.velocity.magnitude > 0.7f)
                {
                    dealDamage = true;
                }
            }
            else
            {
                Debug.Log("NoRB");
            }
        }


        if (dealDamage)
        {
            health -= damageReceive;
            OnHit.Invoke();
            if (EnergyController.instance.hitCastleFirstTime == false)
            {
                EnergyController.instance.SpawnUnit();
                EnergyController.instance.hitCastleFirstTime = true;
                //EnergyController.instance.canSpawn = true;
            }
            EnergyController.instance.AddScore();
            iTween.PunchPosition(gameObject, iTween.Hash("amount", new Vector3(0.5f, 0.5f, 0.5f), "time", 1, "delay", 0.1, "loopType", "none"));
            if (health <= 0.0f)
            {
                if (hitFX != null)
                    hitFX.GetComponentInChildren<ParticleSystem>().Play();

                StartCoroutine(DestroyObject(1.5f));
            }
        }
    }

    IEnumerator DestroyObject(float delay = 0.0f)
    {
        if (delay != 0)
            yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
