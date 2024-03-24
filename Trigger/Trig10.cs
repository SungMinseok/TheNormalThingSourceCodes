using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
public class Trig10 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    
    public int trigNum;
    //public int test;
    //public string transferMapName;  //이동할 맵의 이름
    public Dialogue dialogue_1;

    public Dialogue dialogue_2;//완성후

    public string turn;
    //public GameObject centerView;
    public TransferMap door;

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
    public bool autoEnable;
    public bool preserveTrigger;
    public bool onlyOnce= true;

    void Start()                                                            //Don't Touch
    {
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
        if(theDB.puzzleOverList.Contains(0)){
            bifur = 1;
        }
    }

    void FixedUpdate(){
        if(theDB.trigOverList.Contains(trigNum)){
            flag = true;
        }
        if(theDB.puzzleOverList.Contains(0)){
            bifur = 1;
            door.dialogue_1.sentences[0] = "먼저 뾰족잎사귀를 전달해주자.";
            //door.locked = false;
        }
        else if(!theDB.puzzleOverList.Contains(0)){
            
            door.dialogue_1.sentences[0] = "먼저 뾰족잎사귀부터 찾아보자.";
        }
    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    private void OnTriggerStay2D(Collider2D collision){
        if(!thePlayer.isPlayingPuzzle/*&&!theDB.puzzleOverList.Contains(0)*/){

            // if(!thePlayer.exc.GetBool("on")&&!flag){
            //     thePlayer.exc.SetBool("on",true);
            //     thePlayer.canInteractWith = trigNum;
            // }
            // if(collision.gameObject.name == "Player" && !flag && !autoEnable && (Input.GetKeyDown(KeyCode.Space)||thePlayer.getSpace)&& !theDM.talking){
            //     flag = true;
            // thePlayer.exc.SetBool("on",false);
            // thePlayer.canInteractWith = 0;
            //     StartCoroutine(EventCoroutine());
            // }

            // if(collision.gameObject.name == "Player" && !flag && autoEnable&& !theDM.talking){
            //     flag = true;
            //     StartCoroutine(EventCoroutine());
            // }
            //외부에서 콜라이더에 첫 진입하거나(0) 자기 콜라이더 위에 있으면 해당 트리거 넘버 고정.
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
        if(turn!="")      
            theOrder.Turn("Player",turn);
                                               //트리거 중 이동불가


        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작
        if(bifur==0){

            theDM.ShowDialogue(dialogue_1);
            yield return new WaitUntil(()=> !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)
            
            Fade2Manager.instance.FadeOut();
            yield return new WaitForSeconds(1f);
            
            thePuzzle.puzzleNum[0].SetActive(true);


            
            thePlayer.isPlayingPuzzle = true;
            yield return new WaitForSeconds(0.01f);
            game1.instance.GoRandom();
            Fade2Manager.instance.FadeIn();
        }
        if(bifur==1){
            theDM.ShowDialogue(dialogue_2);
            yield return new WaitUntil(()=> !theDM.talking);  
            thePlayer.notMove =false;
        }

        
        //Wait();
        //SceneManager.LoadScene(transferMapName);
        
        //thePlayer.lastMapName = thePlayer.currentMapName;    //s
        //thePlayer.currentMapName = transferMapName;
        
        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 끝





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



}

