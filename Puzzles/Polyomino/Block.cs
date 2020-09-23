using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;


public class Block : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    
    public Control control;
    public byte num;
    public int nowNum = -1;
    public bool check;  //퍼즐 내부에 들어가면 슬롯 onDrop에서 true > endDrag에서 이 체크가 false 이면 nowNum = -1
    public List<Block> linkedBlock = new List<Block>();
    //public List<int> t = new List<int>();
    public List<int> linkedNum = new List<int>();
    public GameObject vessel;
    public bool isMoving;
    public bool fixedBlock;

    //[SerializeField] private CaseInsensitiveHashCodeProvider caseInsensitiveHash;
    private CanvasGroup canvasGroup;
    //private Control con;


    void Awake(){
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        //con = Control.instance;
    }
    public void OnBeginDrag(PointerEventData eventData){
        if(!fixedBlock){

            isMoving =true;
            
            AudioManager.instance.Play("puzzle0");
            for(int i=0; i<linkedBlock.Count; i++){ //연결된거 넣어줌.
                //Debug.Log(linkedBlockNum[i]);
                linkedBlock[i].isMoving = true;
            }  



            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = .6f;

            transform.SetAsLastSibling();
            for(int i=0; i<linkedBlock.Count; i++){ //연결된거 넣어줌.
                linkedBlock[i].transform.parent = this.transform;
                linkedBlock[i].transform.SetAsLastSibling();
                //linkedBlock[i].transform.SetParent(this.transform,false);
            }  
            control.temp = num;

            for(int i=0; i<control.lastNum.Count; i++){ //등록된 퍼즐 수만큼
                if(control.lastNum[i].Contains(nowNum)){    //i번째 퍼즐에 클릭한 블록의 번호가 있으면
                    for(int j=0; j<control.lastNum[i].Count; j++){ //i번째 퍼즐 길이 만큼
                        control.slots[control.lastNum[i][j]].check =false;  //i번째 퍼즐 체크 다 풀어줌.
                    }
                    control.lastNum[i].Clear();
                    control.lastNum.RemoveAt(i);
                }
                
            }
        }
        
    }
    public void OnDrag(PointerEventData eventData){
        if(!fixedBlock){
                
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }    
    public void OnEndDrag(PointerEventData eventData){//취소
    
        if(!fixedBlock){

            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            //Debug.Log("OnEndDrag");
            transform.SetAsFirstSibling();
            for(byte i=0; i<linkedBlock.Count; i++){
                linkedBlock[i].transform.parent = GameObject.Find("Blocks").transform;
                linkedBlock[i].transform.SetAsFirstSibling();
            }

            if(check){

                check = false;
            }
            else{               //퍼즐 밖으로 뺄 때... 
                nowNum = -1;
                for(int i=0; i<linkedBlock.Count; i++){
                    
                    linkedBlock[i].nowNum = -1;

                }
                
                if(control.activateRelocate) Relocate();

            }
            isMoving = false;
            
            for(int i=0; i<linkedBlock.Count; i++){ //연결된거 넣어줌.
                //Debug.Log(linkedBlockNum[i]);
                linkedBlock[i].isMoving = false;
            }  
        }
    }
    public void OnPointerDown(PointerEventData eventData){
        
        //isMoving =true;
    }

    void Update(){
        
    }

    public void Relocate(){
        if(!fixedBlock){

            Debug.Log("Relocate");
            //if(nowNum == -1 && !isMoving){   //연결시키고 원래 자리로 이동 시킴.
                
                for(int i=0; i<linkedBlock.Count; i++){ //연결된거 넣어줌.
                    linkedBlock[i].transform.parent = this.transform;
                    //linkedBlock[i].transform.SetParent(this.transform,false);
                }  

                transform.position = vessel.transform.position;

                
                for(byte i=0; i<linkedBlock.Count; i++){
                    linkedBlock[i].transform.parent = GameObject.Find("Blocks").transform;
                    //linkedBlock[i].transform.SetParent(GameObject.Find("Blocks").transform, false);// = GameObject.Find("Blocks").transform;
                    
                }
            //}
        }


    }
}
 