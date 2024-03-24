using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Bound : MonoBehaviour
{
    private BoxCollider2D bound;
    public string boundName;    //SL

    private CameraMovement theCamera;
    //private PlayerManager thePlayer;

    void Start()
    {
        bound = GetComponent<BoxCollider2D>();
        
#if UNITY_ANDROID || UNITY_IOS 
//Debug.Log(bound.size.x* transform.localScale.x);
        if(bound.size.x*transform.localScale.x<22.6)
            bound.size = new Vector2(22.6f/transform.localScale.x,bound.size.y);
#endif
        theCamera = FindObjectOfType<CameraMovement>();
        //thePlayer = FindObjectOfType<PlayerManager>();
        theCamera.SetBound(bound);
        bound.isTrigger = true;
        //thePlayer.SetBound(bound);
    }

    public void SetBound(){             //SL
        // if(theCamera != null)
        // {
        //     theCamera.SetBound(bound);
        // }

        // if(PlayerManager.instance.currentMapName=="maze"){
        //     game26.instance.SetBoundInMaze();
        // }
        // else 
        if(theCamera != null)
        {
            theCamera.SetBound(bound);
        }

    }
    
}
