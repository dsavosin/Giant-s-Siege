﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReloadScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Load());
    }
    IEnumerator Load()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
