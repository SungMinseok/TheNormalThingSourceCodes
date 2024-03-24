using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
public class Trig8 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    
    public int trigNum;
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;

    public string turn;
    public GameObject door;
    public GameObject cat;
    //public float speed;

    ///////////////////////////////////////////////////////////////////   Don't Touch
    private DialogueManager theDM;
    private SelectManager theSelect;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private DatabaseManager theDB;
    //private BookManager theBook;
    private MapManager theMap;
    private BookManager theBook;


    ///////////////////////////////////////////////////////////////////   flag는 실행여부 파악 : true이면 실행중/실행완료, false이면 실행전 //  bifer : 분기
    protected bool flag;    // true 이면 다시 실행 안됨.
    
    private int bifur;
    
    /////////////////////////////////////////////////////////////////////   inspector에서 체크 가능. 1: 닿으면 자동 실행, 2: 체크시 해당 트리거 무한 반복.
    public bool autoEnable;
    public bool preserveTrigger;
    public bool onlyOnce = true;
    [Header ("특정 분기만(부터) 반복")]public int repeatBifur;

    void Start()                                                            //Don't Touch
    {
        theDM = DialogueManager.instance;
        theSelect = SelectManager.instance;
        theOrder = OrderManager.instance;
        thePlayer = PlayerManager.instance;
        theDB = DatabaseManager.instance;
        theBook= BookManager.instance;
        theMap= MapManager.instance;
        // if(theDB.trigOverList.Contains(9)){//트리거 실행 후 맵에 다시 돌아왔을 때 DB list에 들어가 있으면 다시 실행 안됨.
        //     bifur = 1;
        // }
        // else if(theDB.trigOverList.Contains(12)){
        //     bifur = 2;
        // }
        if(theDB.trigOverList.Contains(trigNum)){//트리거 실행 후 맵에 다시 돌아왔을 때 DB list에 들어가 있으면 다시 실행 안됨.
            flag = true;
            if(repeatBifur!=0){
                flag = false;
                bifur = repeatBifur;
            }
        }
    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    private void OnTriggerStay2D(Collider2D collision){
        //if(!theDB.puzzleOverList.Contains(0)){
        if(/*!theDB.trigOverList.Contains(12)*/cat.activeSelf){
        
            if(!thePlayer.exc.GetBool("on")&&!flag&&(thePlayer.canInteractWith==0||thePlayer.canInteractWith==trigNum)){
                thePlayer.exc.SetBool("on",true);
                thePlayer.canInteractWith = trigNum;
            }


            //그 넘버만 실행함.
            if(!flag && !autoEnable && (Input.GetKeyDown(KeyCode.Space)||thePlayer.getSpace)&& !theDM.talking &&thePlayer.canInteractWith==trigNum){
                flag = true;
                thePlayer.exc.SetBool("on",false);
                StartCoroutine(EventCoroutine());
            }

            if(!flag && autoEnable&& !theDM.talking){
                flag = true;
                StartCoroutine(EventCoroutine());
            }
        }
        //}

    }
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


    IEnumerator EventCoroutine(){

        theOrder.NotMove();  
        theOrder.PreLoadCharacter();  
        if(turn!="null")      
            theOrder.Turn("Player",turn);
                                               //트리거 중 이동불가


        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작
        
        //thePlayer.animator.SetBool("sad", false);
        //thePlayer.GetComponent<PlayerManager>().speed = speed;
        if(bifur==0){
Debug.Log("A");
            theDM.ShowDialogue(dialogue_1);
            yield return new WaitUntil(()=> !theDM.talking);    
            theDB.progress=1;
            theBook.ActivateUpdateIcon(2);

            //MapManager.instance.RemoveBlur(1);
            //AudioManager.instance.Play("air0");
            
            door.GetComponent<TransferMap>().Enabled = true;

            //bifur =1;
        }                //대화 끝날 때까지 대기 (마지막 제외 필수)
        else if(bifur==1){

Debug.Log("B");
            theDM.ShowDialogue(dialogue_2);
            yield return new WaitUntil(()=> !theDM.talking);    
        }                //대화 끝날 때까지 대기 (마지막 제외 필수)
        //Wait();
        else if(bifur==2){

Debug.Log("C");
            theDM.ShowDialogue(dialogue_3);
            yield return new WaitUntil(()=> !theDM.talking);    
        }           

        
        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 끝

        theOrder.Move();                                                        //트리거 완료 후 이동가능
                //Debug.Log("h2");
        //theDB.trigOverList.Add(trigNum);

        //theMap.blurList.Remove("blur1");
        //theMap.GetComponent<MapManager>().blur[0].SetActive(false);
        
        //thePlayer.GetComponent<PlayerManager>().speed = speed;
        if(onlyOnce)
            theDB.trigOverList.Add(trigNum);
        if(preserveTrigger)
            flag=false;
        if(repeatBifur!=0){
            theDB.trigOverList.Add(trigNum);
            flag=false;
            bifur=repeatBifur;
        }
    }

    void FixedUpdate(){
        if(bifur!=2&&theDB.trigOverList.Contains(12)) bifur =2;
    }



}

