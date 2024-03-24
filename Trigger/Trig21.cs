using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
public class Trig21 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    public static Trig21 instance;
    public int trigNum;

    [Header ("화남")]
    public Dialogue dialogue_1;
    [Header ("보통")]
    public Dialogue dialogue_2;
    [Header ("해피")]
    public Dialogue dialogue_3;
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
        instance =this;
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
   

    public void OnMouseDown(){
         
             
        if(clickable&&!PuzzleManager.instance.puzzleNum[1].activeSelf){
            int temp = Random.Range(0,2);
            AudioManager.instance.Play("woodtouch"+temp.ToString());
            //StartCoroutine(EventCoroutine());
        }
    }


    public IEnumerator EventCoroutine(){

        theOrder.NotMove();  
        int temp = Random.Range(0,2);
        AudioManager.instance.Play("woodtouch"+temp.ToString());

        if(theDB.gameOverList.Contains(2)&&theDB.gameOverList.Contains(4)){

            theDM.ShowDialogue(dialogue_3);
        }
        else if(theDB.gameOverList.Contains(2)||theDB.gameOverList.Contains(4)){

            theDM.ShowDialogue(dialogue_2);
        }
        else{

            theDM.ShowDialogue(dialogue_1);
        }

    
        yield return new WaitUntil(()=> !theDM.talking);  
    
    
        Invoke("BtnOn",0.51f);
    
    }

    public void BtnOn(){
        Puzzle1.instance.buttonOn();
    }

}

