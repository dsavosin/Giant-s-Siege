using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMeshControl : MonoBehaviour
{
    [SerializeField]
    Renderer rend;
    [SerializeField]
    List<Material> knightMats;
    // Start is called before the first frame update
    void Start()
    {
        rend.material = knightMats[Random.Range(0, knightMats.Count - 1)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
