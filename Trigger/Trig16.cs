using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
public class Trig16 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    
    public int trigNum;
    public Dialogue dialogue_1;

    public string turn;

    public Animator animator;


    ///////////////////////////////////////////////////////////////////   Don't Touch
    private DialogueManager theDM;
    private SelectManager theSelect;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private DatabaseManager theDB;
    //private BookManager theBook;
    private MapManager theMap;




    ///////////////////////////////////////////////////////////////////   flag는 실행여부 파악 : true이면 실행중/실행완료, false이면 실행전 //  bifer : 분기
    protected bool flag;    // true 이면 다시 실행 안됨.
    
    private int bifur;
    
    /////////////////////////////////////////////////////////////////////   inspector에서 체크 가능. 1: 닿으면 자동 실행, 2: 체크시 해당 트리거 무한 반복.
    public bool autoEnable;
    public bool preserveTrigger;            //  반복 실행.
    public bool onlyOnce = true;            //  게임 중 딱 한번 실행.
    public bool clickable = false;          // 클릭시 실행.

    void Start()                                                            //Don't Touch
    {
        theDM = DialogueManager.instance;
        theSelect = SelectManager.instance;
        theOrder = OrderManager.instance;
        thePlayer = PlayerManager.instance;
        theDB = DatabaseManager.instance;
//        theBook= BookManager.instance;
        theMap= MapManager.instance;
        if(theDB.trigOverList.Contains(trigNum)){//트리거 실행 후 맵에 다시 돌아왔을 때 DB list에 들어가 있으면 다시 실행 안됨.
            flag = true;
        }
        
    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    // private void OnTriggerStay2D(Collider2D collision){
    //     if(theDB.puzzleOverList.Contains(0)){ 
    //     if(!thePlayer.exc.GetBool("on")&&!flag){
    //         thePlayer.exc.SetBool("on",true);
    //         thePlayer.canInteractWith = trigNum;
    //     }
                
    //         if(collision.gameObject.name == "Player" && !flag && !autoEnable && Input.GetKeyDown(KeyCode.Space)&& !theDM.talking){
    //             flag = true;
    //     thePlayer.exc.SetBool("on",false);
    //     thePlayer.canInteractWith = 0;
    //             StartCoroutine(EventCoroutine());
    //         }

    //         if(collision.gameObject.name == "Player" && !flag && autoEnable&& !theDM.talking){
    //             flag = true;
    //             StartCoroutine(EventCoroutine());
    //         }

    //     }
    // }
    // void OnTriggerEnter2D(Collider2D collision){

    //     if(!thePlayer.exc.GetBool("on")&&!flag){
    //         thePlayer.exc.SetBool("on",true);
    //         thePlayer.canInteractWith = trigNum;
    //     }
    // }
    void OnTriggerExit2D(Collider2D collision){
        thePlayer.exc.SetBool("on",false);
        thePlayer.canInteractWith = 0;
    }

    

    public void OnMouseDown(){
         
             
        if(clickable && theDB.OnActivated[4]){
            theDB.OnActivated[4] = false; 
            clickable =false;
            //StopCoroutine(Trig18.instance.EventCoroutine());
            thePlayer.canInteractWith = trigNum;
            StartCoroutine(EventCoroutine());
            CursorManager.instance.RecoverCursor();
        }
    }
    IEnumerator EventCoroutine(){

        theOrder.NotMove();  
        theOrder.PreLoadCharacter();  
        if(turn!="null")      
            theOrder.Turn("Player",turn);
        
                                               //트리거 중 이동불가
        

        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작
        
        AudioManager.instance.Play("watersplash");
        yield return new WaitForSeconds(1f);
        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(()=> !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)
        //Wait();
        
        
        Inventory.instance.RemoveItem(4);
        Inventory.instance.GetItem(6);
        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 끝

        theOrder.Move();                                                        //트리거 완료 후 이동가능
                //Debug.Log("h2");
        

        //theMap.blurList.Remove("blur1");
        //theMap.GetComponent<MapManager>().blur[0].SetActive(false);
        //theDB.progress=1;
        //thePlayer.GetComponent<PlayerManager>().speed = speed;
        if(onlyOnce)
            theDB.trigOverList.Add(trigNum);
        if(preserveTrigger)
            flag=false;
            
        thePlayer.exc.SetBool("on",false);
        thePlayer.canInteractWith = 0;
            //StopCoroutine(Trig18.instance.EventCoroutine());
    }



}

