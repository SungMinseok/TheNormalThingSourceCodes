using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
public class Trig22 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    
    public int trigNum;
    [Header ("도망친 직후")]
    public Dialogue dialogue_1;
    
    [Header ("숨 다 쉰 후")]
    public Dialogue dialogue_2;

    public string turn;
    public GameObject centerView;
    public Transform rubyMovePoint0;
    public Transform rubyMovePointf;
    

    ///////////////////////////////////////////////////////////////////   Don't Touch
    private DialogueManager theDM;
    private SelectManager theSelect;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private DatabaseManager theDB;
    //private BookManager theBook;
    private MapManager theMap; 
    private CameraMovement theCamera;


    ///////////////////////////////////////////////////////////////////   flag는 실행여부 파악 : true이면 실행중/실행완료, false이면 실행전 //  bifer : 분기
    protected bool flag;    // true 이면 다시 실행 안됨.
    
    private int bifur;
    
    /////////////////////////////////////////////////////////////////////   inspector에서 체크 가능. 1: 닿으면 자동 실행, 2: 체크시 해당 트리거 무한 반복.
    public bool autoEnable;
    public bool preserveTrigger;
    public bool onlyOnce = true;

    void Start()                                                            //Don't Touch
    {
        theDM = DialogueManager.instance;
        theSelect = SelectManager.instance;
        theOrder = OrderManager.instance;
        thePlayer = PlayerManager.instance;
        theDB = DatabaseManager.instance;
        theCamera = CameraMovement.instance;
        theMap= MapManager.instance;
        if(theDB.trigOverList.Contains(trigNum)){//트리거 실행 후 맵에 다시 돌아왔을 때 DB list에 들어가 있으면 다시 실행 안됨.
            flag = true;
        }
    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    private void OnTriggerStay2D(Collider2D collision){
        if(collision.gameObject.name == "Player" && !flag && !autoEnable && (Input.GetKeyDown(KeyCode.Space)||thePlayer.getSpace)&& !theDM.talking){
            flag = true;
        thePlayer.exc.SetBool("on",false);
        thePlayer.canInteractWith = 0;
            StartCoroutine(EventCoroutine());
        }

        if(collision.gameObject.name == "Player" && !flag && autoEnable&& !theDM.talking){
            flag = true;
            StartCoroutine(EventCoroutine());
        }

    }


    IEnumerator EventCoroutine(){

        theOrder.NotMove();  
        theOrder.PreLoadCharacter();  
        if(turn!="null")      
            theOrder.Turn("Player",turn);
                                               //트리거 중 이동불가
// ///////////루비이동 애니메이션
//         thePlayer.boxCollider.enabled=false;
//         thePlayer.transform.position = rubyMovePointf.position;
//         //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작
//         StartCoroutine(RubyWalk());

//         yield return new WaitUntil(()=>rubyMovePoint0.position==thePlayer.transform.position);
//         thePlayer.boxCollider.enabled=true;
//         //UnknownManager.instance.nowPhase = -1;
//         //thePlayer.exc.SetTrigger("on");
//         //yield return new WaitForSeconds(2f);
// /////////////////
        thePlayer.animator.SetTrigger("fastbreath");
        AudioManager.instance.Play("fastbreathing");
        yield return new WaitForSeconds(3.5f);
        AudioManager.instance.Stop("fastbreathing");
        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(()=> !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)
        //Wait();
        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(()=> !theDM.talking);  
        theDB.doorEnabledList.Add(1);
        theDB.doorLockedList.Remove(1);
        theDB.progress=3;
        BookManager.instance.ActivateUpdateIcon(2);



        if(!DebugManager.instance.dobbyIsFree){
            
            UnknownManager.instance.activateRandomAppear = true;
            theDB.activateRandomAppear = true;
        }
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
    }


    IEnumerator RubyWalk(){
        
        while(thePlayer.gameObject.transform.position!=rubyMovePoint0.position){
                thePlayer.animator.SetFloat("Horizontal", -1f);
                thePlayer.animator.SetFloat("Speed", 1f);
                thePlayer.gameObject.transform.position = Vector3.MoveTowards(thePlayer.gameObject.transform.position,
                rubyMovePoint0.position, 5f* Time.deltaTime); 
                yield return null;
        }
        thePlayer.animator.SetFloat("Speed", 0f);
    }    

}

