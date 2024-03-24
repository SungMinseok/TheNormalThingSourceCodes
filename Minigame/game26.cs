using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game26 : MonoBehaviour
{
    public static game26 instance;
    public BoxCollider2D[] bounds;
    public int moveCount;

    void Start(){
        instance = this;

#if UNITY_ANDROID || UNITY_IOS 
//Debug.Log(bound.size.x* transform.parent.localScale.x);
        //if(bound.size.x*transform.parent.localScale.x<22.6)
        for(int i=0;i<bounds.Length;i++){
            bounds[i].size = new Vector2(22.6f/transform.localScale.x,bounds[i].size.y);

        }
#endif
        CameraMovement.instance.SetBound(bounds[PlayerManager.instance.mazeNum]);
    }

    public void SetBoundInMaze(){
        
        if(PlayerManager.instance.mazeNum!=0){
        }
    }
}
