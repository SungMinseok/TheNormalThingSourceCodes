using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour
{
    RaycastHit2D hitTemp;

    BoxCollider2D my_collider;
    
    public bool movingX;    //true 면 좌우로만

    //public bool toggle;

    public Vector3 originPos;

    float temp;

    void Awake(){
        my_collider= GetComponent<BoxCollider2D>();
        //originPos = this.transform;
        //originPos=this.transform.position;
        originPos= GetComponent<RectTransform>().anchoredPosition;
    }

    public void MoveCheck(){
        if(this.gameObject.name == "MainBlock") AudioManager.instance.Play("boosruck");
        else AudioManager.instance.Play("wood"+Random.Range(0,2).ToString());
        
        my_collider.enabled = false;

        if(movingX){    

            hitTemp = Physics2D.BoxCast(GetComponent<RectTransform>().position,new Vector2(0.01f,0.01f),0,new Vector2(-1,0),10000f);
            
            temp = GetComponent<RectTransform>().anchoredPosition.x-hitTemp.transform.GetComponent<RectTransform>().anchoredPosition.x
            -hitTemp.transform.GetComponent<RectTransform>().rect.width/2-transform.GetComponent<RectTransform>().rect.width/2;
            Debug.Log("GetComponent<RectTransform>().anchoredPosition.x : "+GetComponent<RectTransform>().anchoredPosition.x);
            Debug.Log("hitTemp.transform.GetComponent<RectTransform>().anchoredPosition.x : "+hitTemp.transform.GetComponent<RectTransform>().anchoredPosition.x);
            Debug.Log("temp : "+temp);
            if(temp<43f){//왼쪽이동 불가시 오른쪽으로 이동
                hitTemp = Physics2D.BoxCast(GetComponent<RectTransform>().position,new Vector2(0.01f,0.01f),0,new Vector2(1,0),10000f);
                GetComponent<RectTransform>().anchoredPosition =new Vector2(hitTemp.transform.GetComponent<RectTransform>().anchoredPosition.x 
                - hitTemp.transform.GetComponent<RectTransform>().rect.width/2 - transform.GetComponent<RectTransform>().rect.width/2 , 
                GetComponent<RectTransform>().anchoredPosition.y);
                Debug.Log("왼쪽막힘 오른쪽이동");
            }
            else{
                GetComponent<RectTransform>().anchoredPosition = new Vector2(hitTemp.transform.GetComponent<RectTransform>().anchoredPosition.x 
                + hitTemp.transform.GetComponent<RectTransform>().rect.width/2 + transform.GetComponent<RectTransform>().rect.width/2 , 
                GetComponent<RectTransform>().anchoredPosition.y);
            
                Debug.Log("오른쪽막힘 왼쪽이동");
            }
            
        }

        else{   //상하

            hitTemp = Physics2D.BoxCast(GetComponent<RectTransform>().position,new Vector2(0.01f,0.01f),0,new Vector2(0,1),10000f);
            //Physics2D.BoxCast(GetComponent<RectTransform>().position,new Vector2(1,1),0,new Vector2(0,-1),10000f);

            temp = hitTemp.transform.GetComponent<RectTransform>().anchoredPosition.y - hitTemp.transform.GetComponent<RectTransform>()
            .rect.height/2 - transform.GetComponent<RectTransform>().rect.height/2-GetComponent<RectTransform>().anchoredPosition.y;
            if(temp<43f){//위쪽 이동 불가시 아래로 이동
                hitTemp = Physics2D.BoxCast(GetComponent<RectTransform>().position,new Vector2(0.01f,0.01f),0,new Vector2(0,-1),10000f);
                GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, 
                hitTemp.transform.GetComponent<RectTransform>().anchoredPosition.y + hitTemp.transform.GetComponent<RectTransform>()
                .rect.height/2 + transform.GetComponent<RectTransform>().rect.height/2);
                
                Debug.Log("위쪽막힘 아래로이동");
            }
            else{ 
                GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x ,
                hitTemp.transform.GetComponent<RectTransform>().anchoredPosition.y - hitTemp.transform.GetComponent<RectTransform>()
                .rect.height/2 - transform.GetComponent<RectTransform>().rect.height/2);
                Debug.Log("아래쪽막힘 위로이동");
            }
            
            // GetComponent<RectTransform>().anchoredPosition = toggle ? new Vector2(GetComponent<RectTransform>().anchoredPosition.x ,
            // hitTemp.transform.GetComponent<RectTransform>().anchoredPosition.y - hitTemp.transform.GetComponent<RectTransform>()
            // .rect.height/2 - transform.GetComponent<RectTransform>().rect.height/2)
            // : new Vector2(GetComponent<RectTransform>().anchoredPosition.x, 
            // hitTemp.transform.GetComponent<RectTransform>().anchoredPosition.y + hitTemp.transform.GetComponent<RectTransform>()
            // .rect.height/2 + transform.GetComponent<RectTransform>().rect.height/2);

            // toggle = !toggle;
        }

        my_collider.enabled = true;

        if(this.gameObject.name =="MainBlock"&&GetComponent<RectTransform>().anchoredPosition.x>80){
            //game6.instance.passGame();
            AudioManager.instance.Play("success0");
            StartCoroutine(GameManager.instance.GameSuccess());
            game6.instance.Invoke("passGame",GameManager.instance.successWaitTime);
        }
    }

    public void ResetPos(){
        this.GetComponent<RectTransform>().anchoredPosition = originPos;
    }
    // private void OnTriggerEnter2D(Collider2D collision){
    //     if(this.gameObject.name == "Goal" && collision.gameObject.name == "MainBlock"){
    //         
    //     }
    // }
}
