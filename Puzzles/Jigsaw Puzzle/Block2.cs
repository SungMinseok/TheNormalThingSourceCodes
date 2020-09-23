using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;


public class Block2 : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    public bool[] corner = new bool[4];
    
    public Control2 control;
    public int num;
    public int nowNum = -1;
    public bool check;  //퍼즐 내부에 들어가면 슬롯 onDrop에서 true > endDrag에서 이 체크가 false 이면 nowNum = -1
    //public List<Block> linkedBlock = new List<Block>();
    //public List<int> t = new List<int>();
    //public List<int> linkedNum = new List<int>();
    //public GameObject vessel;
    public bool isMoving;

    Vector2 temp;


    //[SerializeField] private CaseInsensitiveHashCodeProvider caseInsensitiveHash;
    private CanvasGroup canvasGroup;
    //private Control con;


    void Awake(){
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        //con = Control.instance;
    }

    void Start(){
        if(corner[0]) temp.y = corner[2] ? 0.5f : 0.373f;
        //else if(corner[2]) temp.y = 0.6275f;
        else temp.y = corner[2] ? 0.6275f : 0.5f;
        if(corner[1]) temp.x = corner[3] ? 0.5f : 0.371f;
        //else if(corner[3]) temp.x = 0.625f;
        else temp.x = corner[3] ? 0.625f : 0.5f;
        rectTransform.pivot = new Vector2(temp.x, temp.y);
    }
    public void OnBeginDrag(PointerEventData eventData){
        isMoving =true;
        
        AudioManager.instance.Play("puzzle0");
        // for(int i=0; i<linkedBlock.Count; i++){ //연결된거 넣어줌.
        //     //Debug.Log(linkedBlockNum[i]);
        //     linkedBlock[i].isMoving = true;
        // }  



        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = .6f;

        // for(int i=0; i<linkedBlock.Count; i++){ //연결된거 넣어줌.
        //     linkedBlock[i].transform.parent = this.transform;
        // }  
        control.temp = num;

        // for(int i=0; i<control.lastNum.Count; i++){ //등록된 퍼즐 수만큼
        //     if(control.lastNum[i].Contains(nowNum)){    //i번째 퍼즐에 클릭한 블록의 번호가 있으면
        //         for(int j=0; j<control.lastNum[i].Count; j++){ //i번째 퍼즐 길이 만큼
        //             control.slots[control.lastNum[i][j]].check =false;  //i번째 퍼즐 체크 다 풀어줌.
        //         }
        //         control.lastNum[i].Clear();
        //         control.lastNum.RemoveAt(i);
        //     }
        
        // }
        if(nowNum!=-1) control.slots[nowNum].check = false;
    }
    public void OnDrag(PointerEventData eventData){
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        //rectTransform.anchoredPosition = eventData.position;// / canvas.scaleFactor;
        //transform.position = eventData.;
    }    
    public void OnEndDrag(PointerEventData eventData){//취소
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        //Debug.Log("OnEndDrag");
        //transform.SetAsFirstSibling();
        // for(byte i=0; i<linkedBlock.Count; i++){
        //     linkedBlock[i].transform.parent = GameObject.Find("Blocks").transform;
        //     linkedBlock[i].transform.SetAsFirstSibling();
        // }
        transform.SetAsLastSibling();
        if(check){

            check = false;
        }
        else{               //퍼즐 밖으로 뺄 때... 
            nowNum = -1;
            // for(int i=0; i<linkedBlock.Count; i++){
                
            //     linkedBlock[i].nowNum = -1;

            // }
            if(control.activateRelocating) Relocate();

        }
        isMoving = false;
        
        // for(int i=0; i<linkedBlock.Count; i++){ //연결된거 넣어줌.
        //     //Debug.Log(linkedBlockNum[i]);
        //     linkedBlock[i].isMoving = false;
        // }  


//        Debug.Log(this.transform.position);
    }
    public void OnPointerDown(PointerEventData eventData){
        
        //isMoving =true;
    }

    void Update(){
        
    }

    public void Relocate(){
        Debug.Log("Relocate");
        //if(nowNum == -1 && !isMoving){   //연결시키고 원래 자리로 이동 시킴.
            
            // for(int i=0; i<linkedBlock.Count; i++){ //연결된거 넣어줌.
            //     linkedBlock[i].transform.parent = this.transform;
            // }  

            //transform.position = control.vessels[0].transform.position;
            
            rectTransform.anchoredPosition = new Vector3(UnityEngine.Random.Range(141,336),UnityEngine.Random.Range(-102,102), 90f);
            
            // for(byte i=0; i<linkedBlock.Count; i++){
            //     linkedBlock[i].transform.parent = GameObject.Find("Blocks").transform;
            // }
        //}
    }
    public void RelocateAtFirst(){
        //Debug.Log("RelocateAtFirst");
        //if(nowNum == -1 && !isMoving){   //연결시키고 원래 자리로 이동 시킴.
            
            // for(int i=0; i<linkedBlock.Count; i++){ //연결된거 넣어줌.
            //     linkedBlock[i].transform.parent = this.transform;
            // }  
            // int ranNum = UnityEngine.Random.Range(0,control.vessels.Length);
            // transform.position = control.vessels[ranNum].transform.position;
        int ranNum = UnityEngine.Random.Range(-102,102);
        rectTransform.anchoredPosition = new Vector3(UnityEngine.Random.Range(141,336),ranNum, 90f);
        if(ranNum>=0)
            transform.SetAsLastSibling();
        else
            transform.SetAsFirstSibling();
            // for(byte i=0; i<linkedBlock.Count; i++){
            //     linkedBlock[i].transform.parent = GameObject.Find("Blocks").transform;
            // }
        //}
    }
}
 