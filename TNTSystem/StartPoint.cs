using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class StartPoint : MonoBehaviour
{
    public string startPoint;
    private PlayerManager thePlayer;
    private CameraMovement theCamera;

    private FadeManager theFade;
    private OrderManager theOrder;
    private UnknownScript theUnknown;
    
    public string lastPoint;    //s



    void Start()
    {
        theCamera = FindObjectOfType<CameraMovement>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theUnknown = FindObjectOfType<UnknownScript>();
        this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        //theOrder.Move();
        if ((startPoint == thePlayer.currentMapName)&&(lastPoint == thePlayer.lastMapName))//s
        {   
            //thePlayer.boxCollider.enabled = false;
            theCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, theCamera.transform.position.z);
            thePlayer.transform.position = this.transform.position;
            //thePlayer.boxCollider.enabled = true;
            
            if(theUnknown != null){
                theUnknown.startPoint = this.transform;
            }
            //theFade.FadeIn();
            Invoke("DelayEnableBox",0.1f);
                //thePlayer.boxCollider.enabled = true;
        }
    }

    void DelayEnableBox(){
        
                thePlayer.boxCollider.enabled = true;
    }
    /*
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            StartCoroutine(TransferCoroutine());
        }
    }*/
    
}
