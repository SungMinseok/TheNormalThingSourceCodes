using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRendererSorter : MonoBehaviour
{
    [HideInInspector]public int sortingOrderBase = 5000;
    
    [Header ("키가 큰 것:0~1")]
    [Header ("(높이기준) 인물:2")]
    [Header ("짧은 것:3")]
    [Header ("매우 짧은 것:4")]
    [Header ("배경 맨 앞나무:-10")]
    public int offset =0;
    private Renderer rend;
    void Awake()
    {
        rend = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rend.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
    }
}
