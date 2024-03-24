using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
public class Trig7 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    
    public int trigNum;
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;
    public Animator bird;
    public GameObject birdObject;
    public SpriteRenderer shadow;

    public string turn;

    public float speed;
    

    ///////////////////////////////////////////////////////////////////   Don't Touch
    private DialogueManager theDM;
    private SelectManager theSelect;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private DatabaseManager theDB;


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
        if(theDB.trigOverList.Contains(11)){
            birdObject.SetActive(false);
        }

        if(theDB.trigOverList.Contains(trigNum)){//트리거 실행 후 맵에 다시 돌아왔을 때 DB list에 들어가 있으면 다시 실행 안됨.
            flag = true;
        }
    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    private void OnTriggerStay2D(Collider2D collision){
        
        if(!thePlayer.exc.GetBool("on")&&!flag&&!autoEnable&&(thePlayer.canInteractWith==0||thePlayer.canInteractWith==trigNum)){
            thePlayer.exc.SetBool("on",true);
            thePlayer.canInteractWith = trigNum;
        }

        if(collision.gameObject.name == "Player" && !flag && !autoEnable && (Input.GetKeyDown(KeyCode.Space)||thePlayer.getSpace)&& !theDM.talking){
            flag = true;
        thePlayer.exc.SetBool("on",false);
            StartCoroutine(EventCoroutine());
        }

        if(collision.gameObject.name == "Player" && !flag && autoEnable&& !theDM.talking){
            flag = true;
            StartCoroutine(EventCoroutine());
        }

    }
    void OnTriggerExit2D(Collider2D collision){
        thePlayer.exc.SetBool("on",false);
        thePlayer.canInteractWith = 0;
    }

    void FixedUpdate(){
        if(thePlayer.transform.position.x>=transform.position.x){
            bird.gameObject.GetComponent<SpriteRenderer>().flipX=true;
        }
        else{
            bird.gameObject.GetComponent<SpriteRenderer>().flipX=false;
        }
    }


    IEnumerator EventCoroutine(){

        theOrder.NotMove();  
        theOrder.PreLoadCharacter();  
        if(turn!="null")      
            theOrder.Turn("Player",turn);
                                               //트리거 중 이동불가

        if(thePlayer.transform.position.x>=transform.position.x){
            //bird.gameObject.GetComponent<SpriteRenderer>().flipX=true;
            theOrder.Turn("Player","LEFT");
        }
        else{
            theOrder.Turn("Player","RIGHT");
        }

        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(()=> !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)
        //Wait();
        BGMManager.instance.trackNum=5;
        BGMManager.instance.Play(5);
        AudioManager.instance.Play("shout1");
        FadeManager.instance.red.SetActive(true);
        bird.SetBool("fly", true);
        ObjectManager.instance.FadeOut(shadow);
        AudioManager.instance.Play("wing0");
        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(()=> !theDM.talking); 
        thePlayer.animator.SetBool("sad", true);
        theDM.ShowDialogue(dialogue_3);
        yield return new WaitUntil(()=> !theDM.talking); 
        
        theDB.doorEnabledList.Add(6);
                // Inventory.instance.GetItem(1);
                // Inventory.instance.GetItem(2);
                // Inventory.instance.GetItem(3);
                // Inventory.instance.GetItem(4);
                // Inventory.instance.GetItem(5);
                // Inventory.instance.GetItem(6);
        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 끝

        theOrder.Move();                                                        //트리거 완료 후 이동가능
                //Debug.Log("h2");
        if(onlyOnce)
            theDB.trigOverList.Add(trigNum);
        thePlayer.GetComponent<PlayerManager>().speed = speed;

        if(preserveTrigger)
            flag=false;
    }



}

