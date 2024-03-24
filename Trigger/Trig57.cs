using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
[RequireComponent(typeof(BoxCollider2D))]
public class Trig57 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    //public static Trig27 instance;
    public int trigNum;
    [Header ("금붕어 첫 대사")]
    public Dialogue dialogue_0;
    
    public GameObject fish;
    public ParticleSystem fishFlash;
    public Transform moveLocations, createLocation;
    public bool moveFlag ;
    //public bool animFlag;
    
    
    //public GameObject centerView;
    

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
    private int temp;


    ///////////////////////////////////////////////////////////////////   flag는 실행여부 파악 : true이면 실행중/실행완료, false이면 실행전 //  bifer : 분기
    protected bool flag;    // true 이면 다시 실행 안됨.
    
    private int bifur;
    
    /////////////////////////////////////////////////////////////////////   inspector에서 체크 가능. 1: 닿으면 자동 실행, 2: 체크시 해당 트리거 무한 반복.
    
    [Header ("실행시 바라보는 방향")]public string turn;
    [Header ("트리거 진입 시 자동 실행")]public bool autoEnable;
    [Header ("여러번 실행 가능")]public bool preserveTrigger;
    [Header ("게임 중 딱 한번만 실행(영원히)")]public bool onlyOnce= true;
    [Header ("특정 분기만(부터) 반복")]public int repeatBifur;

    void Start()                                                            //Don't Touch
    {
        this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        //instance = this;
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
            if(repeatBifur!=0){
                flag = false;
                bifur = repeatBifur;
            }
        }

    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    private void OnTriggerStay2D(Collider2D collision){
        //if(theDB.trigOverList.Contains(24)&&!GameManager.instance.playing&&!theDB.gameOverList.Contains(6)){
        //if(theDB.trigOverList.Contains(3)){
        if(theDB.trigOverList.Contains(56)){

            if(!thePlayer.exc.GetBool("on")&&!flag&&!autoEnable){
                thePlayer.exc.SetBool("on",true);
                thePlayer.canInteractWith = trigNum;
            }
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
        //}
        //}


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


        thePlayer.isInteracting = true;
        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작
        moveFlag= true;
        yield return new WaitUntil(()=>!moveFlag);

        Fade2Manager.instance.FadeOut();
        yield return new WaitForSeconds(1.5f);
        fish.SetActive(true);
        //fish.GetComponent<SpriteRenderer>().flipX = true;
        thePlayer.animator.SetBool("onFish",false);
        thePlayer.transform.position = createLocation.transform.position;        
        theOrder.Turn("Player","RIGHT");
        yield return new WaitForSeconds(0.5f);
        Fade2Manager.instance.FadeIn();
        yield return new WaitForSeconds(1f);

        theDM.ShowDialogue(dialogue_0);
        yield return new WaitUntil(()=> !theDM.talking);

        // //붕어 사라지기
        // fish.GetComponent<Animator>().SetTrigger("disappear");
        // //fish.GetComponent<Animator>().SetFloat("reverse", -1f);
        // //fish.GetComponent<Animator>().Play("fish_appear");
        // yield return new WaitForSeconds(0.5f);
        // fishFlash.Play();
        // yield return new WaitForSeconds(1.5f);
        // ObjectManager.instance.FadeOut(fish.GetComponent<SpriteRenderer>());
        // //fish.SetActive(false);



        theOrder.Move(); 
            

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

    // IEnumerator RideFish(){
    //     thePlayer.notMove = true;
    //     Fade2Manager.instance.FadeOut();
    //     yield return new WaitForSeconds(2f);
    //     fish.SetActive(false);
    //     thePlayer.notMove = true;
    //     Fade2Manager.instance.FadeIn();
    //     thePlayer.animator.SetBool("onFish",true);
    //     thePlayer.transform.position = fish.transform.position;
    //     moveFlag[0]= true;
    //     yield return new WaitUntil(()=>!moveFlag[0]);
    //     moveFlag[1]= true;
    //     //yield return new WaitUntil(()=>!moveFlag[0]);

        
    // }

    void FixedUpdate(){
        if(moveFlag){
            if(thePlayer.transform.position!=moveLocations.position){
                thePlayer.transform.position = Vector3.MoveTowards(thePlayer.transform.position,
                moveLocations.position, 3f* Time.deltaTime); 
                thePlayer.animator.SetFloat("Horizontal", -1f);
                thePlayer.animator.SetFloat("Speed", 1f);
            }
            else{
                moveFlag=false;
            }
        }
    }
}

