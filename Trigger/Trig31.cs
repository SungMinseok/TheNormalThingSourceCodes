using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
public class Trig31 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    public static Trig31 instance;
    public int trigNum;
    [Header ("자물쇠 처음 들어갈 때")]
    public Select select_1;
    [Header ("자물쇠 들어갈 때")]
    public Select select_2;
    [Header ("자물쇠 맞춘 후1")]
    public Dialogue dialogue_1;
    [Header ("자물쇠 맞춘 후2")]
    public Dialogue dialogue_2;

    public GameObject closedBox;
    public GameObject openedBox0;
    public GameObject openedBox1;

    ///////////////////////////////////////////////////////////////////   Don't Touch
    private DialogueManager theDM;
    private SelectManager theSelect;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private DatabaseManager theDB;
    //private BookManager theBook;
    private MapManager theMap; 
    private CameraMovement theCamera;
    private PuzzleManager thePuzzle;


    ///////////////////////////////////////////////////////////////////   flag는 실행여부 파악 : true이면 실행중/실행완료, false이면 실행전 //  bifer : 분기
    protected bool flag;    // true 이면 다시 실행 안됨.
    
    private int bifur;
    
    /////////////////////////////////////////////////////////////////////   inspector에서 체크 가능. 1: 닿으면 자동 실행, 2: 체크시 해당 트리거 무한 반복.

    [Header ("실행시 바라보는 방향")]public string turn;
    [Header ("트리거 진입 시 자동 실행")]public bool autoEnable;
    [Header ("여러번 실행 가능")]public bool preserveTrigger;
    [Header ("게임 중 딱 한번만 실행(영원히)")]public bool onlyOnce= true;


    void Start()                                                            //Don't Touch
    {
        instance = this;
        theDM = DialogueManager.instance;
        theSelect = SelectManager.instance;
        theOrder = OrderManager.instance;
        thePlayer = PlayerManager.instance;
        theDB = DatabaseManager.instance;
        theCamera = CameraMovement.instance;
        theMap= MapManager.instance;
        thePuzzle= PuzzleManager.instance;
        if(theDB.trigOverList.Contains(trigNum)){//트리거 실행 후 맵에 다시 돌아왔을 때 DB list에 들어가 있으면 다시 실행 안됨.
            flag = true;
        }
        if(theDB.gameOverList.Contains(17)){//트리거 실행 후 맵에 다시 돌아왔을 때 DB list에 들어가 있으면 다시 실행 안됨.
            flag = true;
            openedBox1.SetActive(true);
            openedBox0.SetActive(false);
            closedBox.SetActive(false);
        }

    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    private void OnTriggerStay2D(Collider2D collision){
        if(!theDB.gameOverList.Contains(17)&&!thePlayer.isPlayingGame){ 
            // if(!thePlayer.exc.GetBool("on")&&!flag){
            //     thePlayer.exc.SetBool("on",true);
            //     thePlayer.canInteractWith = trigNum;
            // }
            // if(collision.gameObject.name == "Player" && !flag && !autoEnable && Input.GetKeyDown(KeyCode.Space)&& !theDM.talking){
            //     flag = true;
            //     thePlayer.exc.SetBool("on",false);
            //     thePlayer.canInteractWith = 0;
            //     StartCoroutine(EventCoroutine());
            // }

            // if(collision.gameObject.name == "Player" && !flag && autoEnable&& !theDM.talking){
            //     flag = true;
            //     StartCoroutine(EventCoroutine());
            // }
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
    }
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
        
        

        if(bifur==0){
            theSelect.ShowSelect(select_1);
            //bifur=1;
        }
        else{
            theSelect.ShowSelect(select_2);
        } 
        yield return new WaitUntil(() => !theSelect.selecting);



        if(theSelect.GetResult()==0){//게임 on
            GameManager.instance.game17.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.1f);  
                GameManager.instance.game17.GetComponent<game17>().ResetGame();
            thePlayer.isPlayingGame = true;
            thePlayer.notMove = true;
            if(bifur==0) bifur =1;
            //Fade2Manager.instance.FadeIn();
        }
        else if(theSelect.GetResult()==1){//그냥 나오기
            theOrder.Move(); 
            
        }

        // Fade2Manager.instance.FadeOut();
        // yield return new WaitForSeconds(1f);
        





        //theOrder.Move();                                                        //트리거 완료 후 이동가능
                //Debug.Log("h2");

        //theMap.blurList.Remove("blur1");
        //theMap.GetComponent<MapManager>().blur[0].SetActive(false);
        //thePlayer.GetComponent<PlayerManager>().speed = speed;

        
        if(onlyOnce)
            theDB.trigOverList.Add(trigNum);
        if(preserveTrigger)
            flag=false;
    }

    public IEnumerator FinishGame(){

        theOrder.NotMove();  
        theOrder.PreLoadCharacter();  
        if(turn!="null")      
            theOrder.Turn("Player",turn);
                                               //트리거 중 이동불가

        openedBox0.SetActive(true);
        openedBox1.SetActive(true);
        closedBox.SetActive(false);

        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작
    
        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(()=> !theDM.talking);
        openedBox0.SetActive(false);
        //openedBox1.SetActive(true);
        thePlayer.animator.SetTrigger("grab");
        AudioManager.instance.Play("pageflip2");


        
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.Play("getitem0");
        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(()=> !theDM.talking);
        //theDB.activatedPaper=2;
        //BookManager.instance.ActivateUpdateIcon(0);
        theDB.doorEnabledList.Add(20);
        theDB.activatedPaper=4;
        BookManager.instance.ActivateUpdateIcon(0);

        flag = true;
        theDB.trigOverList.Add(trigNum);

        theOrder.Move(); 
            

    }
}

