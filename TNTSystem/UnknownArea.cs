using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class UnknownArea : MonoBehaviour
{
    // Start is called before the first frame update
    private UnknownManager mother;
    
    void Start()
    {
        mother = UnknownManager.instance;

        mother.locationList.Add(this.transform);
    }

    void OnDestroy(){
        if(mother.locationList!=null)
            mother.locationList.Clear();
    }
}
