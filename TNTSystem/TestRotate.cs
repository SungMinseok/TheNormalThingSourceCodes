﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestRotate : MonoBehaviour
{
    public float speed;
    public int[] values;

    float realRotation;

    void Start()
    {
        
    }

    void Update()
    {
        if (transform.root.eulerAngles.z != realRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, realRotation), speed);
        }
    }

    void OnMouseDown()
    {
        RotatePiece();
    }
   
    public void RotatePiece()
    {
        realRotation += 90;
        Debug.Log("ROTATING");
        if (realRotation == 360)
            realRotation = 0;

        RotateValues();
    }

    public void RotateValues()
    {
        int aux = values[0];

        for(int i = 0; i < values.Length-1; i++)
        {
            values[i] = values[i + 1];
        }

        values[3] = aux;

    }
}
