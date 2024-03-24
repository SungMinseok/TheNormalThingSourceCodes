using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
[RequireComponent(typeof(BoxCollider2D))]
public class Trig43 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    //public static Trig27 instance;
    public int trigNum;

    // [Header ("텐드 진입 전")]
    // public Select select_1;
    // [Header ("거미줄 완료 후")]
    // public Select select_2;
    [Header ("처음 문열기 대사")]
    public Dialogue dialogue_0;
    [Header ("문 두 개 다 열고 대사")]
    public Dialogue dialogue_1;
    [Header ("잠겼을 때 대사")]
    public Dialogue dialogue_2;
    [Header ("위에서 아래로 왔을때 잠김 대사")]
    public Dialogue dialogue_3;
    [Header ("위에는 뚫고 밑에서 접근시 대사")]
    public Dialogue dialogue_4;
    public GameObject trig75;
    
    
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
    public GameObject doorClosed;
    public GameObject doorOpened0;
    public GameObject doorOpened1;
    public SpriteRenderer wire;
    //public BoxCollider2D doorCollider;


    ///////////////////////////////////////////////////////////////////   flag는 실행여부 파악 : true이면 실행중/실행완료, false이면 실행전 //  bifer : 분기
    protected bool flag;    // true 이면 다시 실행 안됨.
    
    public int bifur;
    
    /////////////////////////////////////////////////////////////////////   inspector에서 체크 가능. 1: 닿으면 자동 실행, 2: 체크시 해당 트리거 무한 반복.
    
    [Header ("실행시 바라보는 방향")]public string turn;
    [Header ("트리거 진입 시 자동 실행")]public bool autoEnable;
    [Header ("여러번 실행 가능")]public bool preserveTrigger;
    [Header ("게임 중 딱 한번만 실행(영원히)")]public bool onlyOnce= true;
    [Header ("특정 분기만(부터) 반복")]public int repeatBifur;
    [Header ("마우스 클릭 실행")]public bool clickable = false;          // 클릭시 실행.

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
            wire.gameObject.SetActive(false);
            if(trigNum==44) trig75.SetActive(false);
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
        if(doorClosed.activeSelf/*&&theDB.trigOverList.Contains(30)*/){

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
        //}
        //}


    }
    void OnTriggerExit2D(Collider2D collision){
        thePlayer.exc.SetBool("on",false);
        thePlayer.canInteractWith = 0;
    }
    public void OnMouseDown(){
        
             
        if(clickable && theDB.OnActivated[11]){
                thePlayer.exc.SetBool("on",false);
            theDB.OnActivated[11] = false; 
            clickable =false;
            flag = true;
            StartCoroutine(OpenCoroutine());
            CursorManager.instance.RecoverCursor();
        }
    }

    IEnumerator OpenCoroutine(){
        theOrder.NotMove();  
        if(bifur==0){
            //if(thePlayer.currentMapName == "village"){
                //촌락 도착전(돌아서 캣우드부터 뚫는 경우)
                if(!theDB.trigOverList.Contains(30)){   
                    //촌락에서 : 캣우드뚫고 촌락은 뚫을수 없다.
                    if(thePlayer.currentMapName == "village"){  
                                
                        theDM.ShowDialogue(dialogue_3);
                        yield return new WaitUntil(()=> !theDM.talking);
                        flag=false;
                        clickable = true;
                    }
                    //캣우드에서 : 캣우드는 뚫을 수 있다.
                    else{
                            trig75.SetActive(false);
                        ObjectManager.instance.FadeOut(wire);
                        StopCoroutine(EventCoroutine());
                        theDM.ShowDialogue(dialogue_0);
                        yield return new WaitUntil(()=> !theDM.talking);
                        AudioManager.instance.Play("boosruck");
                        // if(thePlayer.currentMapName == "village")
                        //     theDB.doorEnabledList.Add(21);
                        // else if(thePlayer.currentMapName == "catwood"){
                            theDB.doorEnabledList.Add(21);
                        //}

                        // if(theDB.trigOverList.Contains(43)){
                            
                        //     Debug.Log("업적1 : 도구를 이용해야지");
                        //     if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(1);
                        //     theDB.doorEnabledList.Add(5);
                        //     Inventory.instance.RemoveItem(11);
                        //     //trig75.SetActive(false);
                        //     theDM.ShowDialogue(dialogue_1);
                        //     yield return new WaitUntil(()=> !theDM.talking);
                        // }


                        bifur =1;
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
                }
                //촌락 방문 후 : 촌락 or 캣우드
                else
                {
                        ObjectManager.instance.FadeOut(wire);
                        StopCoroutine(EventCoroutine());
                        theDM.ShowDialogue(dialogue_0);
                        yield return new WaitUntil(()=> !theDM.talking);
                        AudioManager.instance.Play("boosruck");
                            theDB.doorEnabledList.Add(5);
                            theDB.doorEnabledList.Add(21);
                        //wire.SetActive(false);
                        if(thePlayer.currentMapName == "village"){
                            if(theDB.trigOverList.Contains(44)){
                            
                                Debug.Log("업적1 : 도구를 이용해야지");
                                if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(1);
                                Inventory.instance.RemoveItem(11);
                                theDM.ShowDialogue(dialogue_1);
                                yield return new WaitUntil(()=> !theDM.talking);
                            }

                        }
                        else if(thePlayer.currentMapName == "catwood"){
                            //theDB.doorEnabledList.Add(21);
                            trig75.SetActive(false);
                            if(theDB.trigOverList.Contains(43)){
                            
                                Debug.Log("업적1 : 도구를 이용해야지");
                                if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(1);
                                Inventory.instance.RemoveItem(11);
                                theDM.ShowDialogue(dialogue_1);
                                yield return new WaitUntil(()=> !theDM.talking);
                            }
                        }

                        // if(/*theDB.trigOverList.Contains(43)&&*/theDB.trigOverList.Contains(44)){
                            
                        //     Debug.Log("업적1 : 도구를 이용해야지");
                        //     if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(1);
                        //     theDB.doorEnabledList.Add(5);
                        //     Inventory.instance.RemoveItem(11);
                        //     //trig75.SetActive(false);
                        //     theDM.ShowDialogue(dialogue_1);
                        //     yield return new WaitUntil(()=> !theDM.talking);
                        // }

                        //템 삭제.
                        // if(theDB.doorEnabledList.Contains(5)&&theDB.doorEnabledList.Contains(21)&&Inventory.instance.SearchItem(11)){
                        //     Inventory.instance.RemoveItem(11);
                        //     theDM.ShowDialogue(dialogue_1);
                        //     yield return new WaitUntil(()=> !theDM.talking);
                        // }

                        bifur =1;
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
                
            //}

        }
    
        theOrder.Move(); 
            
    
    
    }

    IEnumerator EventCoroutine(){

        theOrder.NotMove();  
        theOrder.PreLoadCharacter();  
        if(turn!="")      
            theOrder.Turn("Player",turn);
                                               //트리거 중 이동불가


        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작

        if(bifur==0&&wire.gameObject.activeSelf){
            if(thePlayer.currentMapName == "village"){
                if(theDB.trigOverList.Contains(30)&&theDB.trigOverList.Contains(44)){
                    theDM.ShowDialogue(dialogue_4);
                    yield return new WaitUntil(()=> !theDM.talking);
                }
                else{
                        
                    theDM.ShowDialogue(dialogue_2);
                    yield return new WaitUntil(()=> !theDM.talking);
                }

            }
            else{
                    
                theDM.ShowDialogue(dialogue_2);
                yield return new WaitUntil(()=> !theDM.talking);
            }
            theOrder.Move(); 

            flag = false;


        }

        

        else if(bifur==1){
            if(doorClosed.activeSelf){
                AudioManager.instance.Play("boosruck");
                doorClosed.SetActive(false);
                doorClosed.GetComponent<BoxCollider2D>().enabled = false;
                if((thePlayer.currentMapName == "village"&&thePlayer.transform.position.y<7.8f)||(thePlayer.currentMapName == "catwood"&&thePlayer.transform.position.y<-4f)){
                    doorOpened0.SetActive(true);
                    theOrder.Turn("Player","UP");
                }
                else{
                    doorOpened1.SetActive(true);
                    theOrder.Turn("Player","DOWN");
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

    }
}

