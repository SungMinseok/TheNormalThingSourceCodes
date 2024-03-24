using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
[RequireComponent(typeof(BoxCollider2D))]
public class Trig45 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    //public static Trig27 instance;
    public int trigNum;

    // [Header ("텐드 진입 전")]
    // public Select select_1;
    // [Header ("거미줄 완료 후")]
    // public Select select_2;
    [Header ("처음 대사")]
    public Dialogue dialogue_0;
    [Header ("이후 짤막 대사(없어도 됨)")]
    public Dialogue dialogue_1;
    [Header ("발전기 선택지")]
    public Select select_0;
    [Header ("발전기 돌리고 대사")]
    public Dialogue dialogue_2;
    [Header ("문 열기 실패 후 대사")]
    public Dialogue dialogue_3;
    [Header ("대사 카운트")]
    public int dialogueCount;
    public Trig45 trig53;
    public SpriteRenderer defaultHandle;
    public SpriteRenderer activatedHandle;
    public GameObject closed, opened;
    public bool doorOpen;
    public Transform rubyMovePoint0;
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

        if(theDB.gameOverList.Contains(23)&&trigNum==52){
            activatedHandle.gameObject.SetActive(true);
            defaultHandle.gameObject.SetActive(false);
            closed.SetActive(false);
            opened.SetActive(true);
        }

        if(trigNum==50&&!theDB.trigOverList.Contains(93)){
            gameObject.SetActive(false);
        }

    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    private void OnTriggerStay2D(Collider2D collision){
        //if(theDB.trigOverList.Contains(24)&&!GameManager.instance.playing&&!theDB.gameOverList.Contains(6)){
            if(!thePlayer.exc.GetBool("on")&&!flag&&!autoEnable){
                //if(trigNum!=50){
                // if(trigNum==50&&theDB.gameOverList.Contains(20)){

                // }
                // else{

                    thePlayer.exc.SetBool("on",true);
                    thePlayer.canInteractWith = trigNum;
                //}
                //}
                
                
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
        //}


    }
    void OnTriggerExit2D(Collider2D collision){
        thePlayer.exc.SetBool("on",false);
        thePlayer.canInteractWith = 0;
    }
    public void OuterAccess(){
                flag = true;
        thePlayer.exc.SetBool("on",false);
        thePlayer.canInteractWith = 0;
        StartCoroutine(EventCoroutine());
    }


    IEnumerator EventCoroutine(){

        theOrder.NotMove();  
        theOrder.PreLoadCharacter();  
        if(turn!="")      
            theOrder.Turn("Player",turn);
                                               //트리거 중 이동불가


        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작
//#if ADD_ACH
        if(trigNum!=50&&trigNum!=52&&trigNum!=56){
            trig53.dialogueCount ++;
            if(trig53.dialogueCount>=8){
                Debug.Log("업적11");        
                if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(11);
            }
        }
//#endif


        if(bifur==0){
            if(trigNum==47){//기계소리가 난다
                AudioManager.instance.Play("boosruck");
            }
            else if(trigNum==51){//기계소리
                    
                AudioManager.instance.Play("boosruck");
            }
            else if(trigNum==56){//탈출
                thePlayer.isInteracting = true;
                ObjectManager.instance.ImageFadeOut(FadeManager.instance.fog0.GetComponent<Image>(),0.015f);
                StartCoroutine(RubyWalk());

                yield return new WaitUntil(()=>rubyMovePoint0.position==thePlayer.transform.position);
                
            }

            theDM.ShowDialogue(dialogue_0);
            yield return new WaitUntil(()=> !theDM.talking);
            switch(trigNum){
                case 50 ://문앞 열기 전.
                    if(!theDB.gameOverList.Contains(23)){
                        AudioManager.instance.Play("boosruck");
                        theDM.ShowDialogue(dialogue_3);
                        yield return new WaitUntil(()=> !theDM.talking);
                    }
                    break;
                case 52 ://손잡이
                    theSelect.ShowSelect(select_0);
                    yield return new WaitUntil(()=> !theSelect.selecting);
                    if(theSelect.GetResult()==0){
                        ObjectManager.instance.FadeIn(activatedHandle,0.015f);
                        AudioManager.instance.Play("machine1");
                        yield return new WaitForSeconds(4f);
                        //ObjectManager.instance.FadeOut(defaultHandle);
                        defaultHandle.gameObject.SetActive(false);
                        closed.SetActive(false);
                        opened.SetActive(true);
                        theDM.ShowDialogue(dialogue_2);
                        yield return new WaitUntil(()=> !theDM.talking);
                        preserveTrigger = false;
                        //doorOpen =true;
                        theDB.gameOverList.Add(23);
                        GameObject.Find("Trig50").GetComponent<Trig45>().flag = true;
                        break;
                    }
                    else{
                        break;
                    }
                default : 
                    break;
            }
            


        }
        else{
            switch(trigNum){
                case 50 :
                    if(!theDB.gameOverList.Contains(23)){
                        theDM.ShowDialogue(dialogue_3);
                        yield return new WaitUntil(()=> !theDM.talking);
                    }
                    break;
                default :
                    theDM.ShowDialogue(dialogue_1);
                    yield return new WaitUntil(()=> !theDM.talking);
                    break;
            }
        }

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
    IEnumerator RubyWalk(){
        
        while(thePlayer.gameObject.transform.position!=rubyMovePoint0.position){
                thePlayer.animator.SetFloat("Vertical", 0f);
                thePlayer.animator.SetFloat("Horizontal", -1f);
                thePlayer.animator.SetFloat("Speed", 1f);
                thePlayer.gameObject.transform.position = Vector3.MoveTowards(thePlayer.gameObject.transform.position,
                rubyMovePoint0.position, 3f* Time.deltaTime); 
                yield return null;
        }
        thePlayer.animator.SetFloat("Speed", 0f);
    }    
}

