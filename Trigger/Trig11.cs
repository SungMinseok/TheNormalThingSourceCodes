using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
public class Trig11 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    
    public int trigNum;
    public Dialogue dialogue_1;

    public string turn;
    public float speed;

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
    private void OnTriggerStay2D(Collider2D collision){
        
        if(collision.gameObject.name == "Player" && !flag && !autoEnable && (Input.GetKeyDown(KeyCode.Space)||thePlayer.getSpace)&& !theDM.talking){
            flag = true;
            StartCoroutine(EventCoroutine());
        }

        if(collision.gameObject.name == "Player" && !flag && autoEnable&& !theDM.talking){
            flag = true;
            StartCoroutine(EventCoroutine());
        }

    }

    public void AccessByButton(){
        StartCoroutine(EventCoroutine());
    }

    IEnumerator EventCoroutine(){

        theOrder.NotMove();  
        theOrder.PreLoadCharacter();  
        if(turn!="null")      
            theOrder.Turn("Player",turn);
                                               //트리거 중 이동불가


        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작
        
        AudioManager.instance.Stop("shout1");
        FadeManager.instance.red.SetActive(false);
        thePlayer.animator.SetBool("sad", false);
        AudioManager.instance.Stop("wing0");
        //theDB.doorEnabledList.Add(1);
        theDB.doorEnabledList.Add(2);
        theDB.doorEnabledList.Add(3);
        theDB.doorEnabledList.Add(4);
        theDB.doorEnabledList.Add(6);
        theDB.doorEnabledList.Add(7);
        //theDB.doorLockedList.Remove(1);
        theDB.gameOverList.Add(-1);
        thePlayer.speed = speed;
        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(()=> !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)
        //Wait();

        
        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 끝

        theOrder.Move();                                                        //트리거 완료 후 이동가능

        if(onlyOnce)
            theDB.trigOverList.Add(trigNum);
        if(preserveTrigger)
            flag=false;
    }



}

