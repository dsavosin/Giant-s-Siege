using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tobii.G2OM;
namespace Tobii.XR.Examples
{
    public class ActivateAtGaze : MonoBehaviour, IGazeFocusable
    {
        [SerializeField]
        GroundUnit unit;
        [SerializeField]
        float fleeTime=5;
        [SerializeField]
        float multiplier;

        float gazeTime;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (focused)
            {
                gazeTime += Time.deltaTime * multiplier;
                if (gazeTime >= fleeTime)
                {
                    unit.ScareEm();
                    unit.StartCoroutine(unit.ReturnToUnit(4));
                }
            }
        }
        bool focused=false;
        public void GazeFocusChanged(bool hasFocus)
        {
            //If this object received focus, fade the object's color to highlight color
            if (hasFocus)
            {
                focused = true;
            }
            //If this object lost focus, fade the object's color to it's original color
            else
            {
                focused = false;
                gazeTime = 0;
            }
        }
    }
}

