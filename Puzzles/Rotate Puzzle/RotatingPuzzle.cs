using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPuzzle : MonoBehaviour
{
    //public GameObject GameManager;



    public float realRotation;
    //public float speed;
    public int value = 0;
//    private GameManager theGM;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (transform.rotation != Quaternion.Euler(0, 0, realRotation)){
            
            //Debug.Log(transform.rotation);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, realRotation), 0.5f);
            if (realRotation == 360)
            realRotation = 0;
        }
        
        
        
    }
    //void OnMouseDown()
    //{
    //    RotatePiece();
    //}
   
    public void RotatePiece()
    {
        //if (realRotation > 360)
            //realRotation = 0;
        
        if(DatabaseManager.instance.OnActivated[0]){
            StartCoroutine(RPCo());
        }
        else{

            realRotation += 90;
            value += 1;
            if(value>3)
                value =0;
        }
        
        //Debug.Log(realRotation);
    }
    IEnumerator RPCo(){

        CursorManager.instance.RecoverCursor();
        DialogueManager.instance.ShowDialogue(Inventory.instance.wrongUse);
        yield return new WaitUntil(()=> !DialogueManager.instance.talking);
    }
    /*IEnumerator Rotating(){
        //float a = Quaternion.Euler(0, 0, realRotation);
        Debug.Log("transform.root.eulerAngles.z : "+transform.root.eulerAngles.z);
        Debug.Log("realRotation : " + realRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0, 0, realRotation) , 0.1f);
        
        //bool check = false;
       
        yield return null;
        //yield return new WaitForSeconds(0.1f);
       //yield return new WaitUntil(()=> !check);  
    }*/
}
