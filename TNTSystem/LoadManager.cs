using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour                //필요한가?

{
    private Bound[] bounds;
    private PlayerManager thePlayer;
    private CameraMovement theCamera;

    public void LoadStart(){
        StopAllCoroutines();
        StartCoroutine(LoadWaitCoroutine());
    }

    IEnumerator LoadWaitCoroutine(){
        yield return new WaitForSeconds(0.5f);

        thePlayer= FindObjectOfType<PlayerManager>();
        bounds= FindObjectsOfType<Bound>();
        theCamera= FindObjectOfType<CameraMovement>();

        theCamera.target = GameObject.Find("Player");

        // for(int i=0; i<bounds.Length; i++){
        //     if(bounds[i].boundName == thePlayer.currentMapName){
        //         bounds[i].SetBound();
        //         break;
        //     }
        // }
    }
    
    public void Loading(){                  //startpoint 방지
        StartCoroutine(LoadingCoroutine());
        
    }
    
    IEnumerator LoadingCoroutine(){
        yield return new WaitForSeconds(2f);
    }

    
}
