using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component1 : MonoBehaviour
{
    void Awake(){

    }

    void Start()
    {
        Debug.Log("Hello world!");
        Debug.Log(this);
    }

    void Update()
    {
        //Debug.Log("u");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
        }
    }
}
