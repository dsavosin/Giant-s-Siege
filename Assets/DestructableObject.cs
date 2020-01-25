using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{

    [SerializeField]
    float health;

    [SerializeField]
    float damageReceive;

    public Transform hitFX;


    public float DestructionThreshold = 1;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            iTween.PunchPosition(gameObject, iTween.Hash("amount", new Vector3(0.5f, 0.5f, 0.5f), "time", 1, "delay", 0.1, "loopType", "none"));

            if(hitFX != null)
                hitFX.GetComponentInChildren<ParticleSystem>().Play();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        bool dealDamage = false;


        if (collision.gameObject.CompareTag("LeftHand"))
        {
            if (EnergyController.instance.leftHandVelo > DestructionThreshold)
            {
                dealDamage = true;
            }
        }

        if (collision.gameObject.CompareTag("RightHand"))
        {
            if (EnergyController.instance.rightHandVelo > DestructionThreshold)
            {
                dealDamage = true;
            }
        }


        if (collision.gameObject.CompareTag("Interactable"))
        {
            Rigidbody rb = collision.transform.parent.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if(rb.velocity.magnitude > 3.0f)
                {
                    dealDamage = true;
                }
            }
        }


        if (dealDamage)
        {
            health -= damageReceive;

            if (hitFX != null)
                hitFX.GetComponentInChildren<ParticleSystem>().Play();

            iTween.PunchPosition(gameObject, iTween.Hash("amount", new Vector3(0.5f, 0.5f, 0.5f), "time", 1, "delay", 0.1, "loopType", "none"));
            if (health <= 0.0f)
            {
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
