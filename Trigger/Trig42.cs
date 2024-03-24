using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
[RequireComponent(typeof(BoxCollider2D))]
public class Trig42 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    //public static Trig27 instance;
    public int trigNum;
    [Header ("첫 대사")]
    public Dialogue dialogue_7;
    
    [Header ("삽화 끝난 후")]
    public Dialogue dialogue_8;

    // [Header ("텐드 진입 전")]
    // public Select select_1;
    // [Header ("거미줄 완료 후")]
    // public Select select_2;
    [Header ("금붕어 첫 대사")]
    public Dialogue dialogue_0;
    [Header ("틀렸을 때")]
    public Dialogue dialogue_1;
    [Header ("다 맞췄을 때")]
    public Dialogue dialogue_2;
    [Header ("질문1")]
    public Select select_0;
    [Header ("정답")]
    public Dialogue dialogue_3;
    [Header ("질문2")]
    public Select select_1;
    [Header ("정답")]
    public Dialogue dialogue_4;
    [Header ("질문3")]
    public Select select_2;
    [Header ("정답")]
    public Dialogue dialogue_5;
    [Header ("질문4")]
    public Select select_3;
    [Header ("정답")]
    public Dialogue dialogue_6;

    public GameObject fish;
    public ParticleSystem fishFlash;
    public Transform[] moveLocations = new Transform[2];
    public bool[] moveFlag = new bool[2];
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
                fish.SetActive(true);
            }
        }
        if(theDB.trigOverList.Contains(57)){
            flag = true;
            fish.SetActive(true);
        }

    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    private void OnTriggerStay2D(Collider2D collision){
        //if(theDB.trigOverList.Contains(24)&&!GameManager.instance.playing&&!theDB.gameOverList.Contains(6)){
        //if(theDB.trigOverList.Contains(3)){
        //if(!theDB.trigOverList.Contains(trigNum)){
        if(!theDB.trigOverList.Contains(45)){

            if(!thePlayer.exc.GetBool("on")&&!flag&&!autoEnable&&(thePlayer.canInteractWith==0||thePlayer.canInteractWith==trigNum)){
                thePlayer.exc.SetBool("on",true);
                thePlayer.canInteractWith = trigNum;
            }

            if(collision.gameObject.name == "Player" && !flag && !autoEnable && (Input.GetKeyDown(KeyCode.Space)||thePlayer.getSpace)&& !theDM.talking &&thePlayer.canInteractWith==trigNum){
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
        //}


    }
    void OnTriggerExit2D(Collider2D collision){
        thePlayer.exc.SetBool("on",false);
        thePlayer.canInteractWith = 0;
    }


    IEnumerator EventCoroutine(){
        thePlayer.isInteracting = true;
        theOrder.NotMove();  
        theOrder.PreLoadCharacter();  
        if(turn!="")      
            theOrder.Turn("Player",turn);
                                               //트리거 중 이동불가


        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작
    


        if(bifur==0){
            theDB.progress=5;
            BookManager.instance.ActivateUpdateIcon(2);

            theDM.ShowDialogue(dialogue_7);
            yield return new WaitUntil(()=> !theDM.talking);
            StartCoroutine(Fade2Manager.instance.ImageAnim());
            //animFlag =true;
            yield return new WaitUntil(()=> !Fade2Manager.instance.animFlag);
            fish.SetActive(true);
            fish.GetComponent<Animator>().SetTrigger("appear");
                AudioManager.instance.Play("water0");
            //yield return new WaitForSeconds(0.5f);
            fishFlash.Play();
            yield return new WaitForSeconds(2.5f);

            theDM.ShowDialogue(dialogue_8);
            yield return new WaitUntil(()=> !theDM.talking);
            // theDM.ShowDialogue(dialogue_0);
            // yield return new WaitUntil(()=> !theDM.talking);
            bifur =1;
        }
        else if(bifur==1){
            theDM.ShowDialogue(dialogue_0);
            yield return new WaitUntil(()=> !theDM.talking);
            
            theSelect.ShowSelect(select_0);
            yield return new WaitUntil(()=> !theSelect.selecting);
            if(theSelect.GetResult()==0){//정답
                theDM.ShowDialogue(dialogue_3);
                yield return new WaitUntil(()=> !theDM.talking);
                theSelect.ShowSelect(select_1);
                yield return new WaitUntil(()=> !theSelect.selecting);
                if(theSelect.GetResult()==1){//ㅈㄷ
                    theDM.ShowDialogue(dialogue_4);
                    yield return new WaitUntil(()=> !theDM.talking);
                    theSelect.ShowSelect(select_2);
                    yield return new WaitUntil(()=> !theSelect.selecting);
                    if(theSelect.GetResult()==1){//ㅈㄷ
                        theDM.ShowDialogue(dialogue_5);
                        yield return new WaitUntil(()=> !theDM.talking);
                        theSelect.ShowSelect(select_3);
                        yield return new WaitUntil(()=> !theSelect.selecting);
                        if(theSelect.GetResult()==1){//ㅈㄷ
                            theDM.ShowDialogue(dialogue_6);
                            yield return new WaitUntil(()=> !theDM.talking);
                            theDM.ShowDialogue(dialogue_2);
                            yield return new WaitUntil(()=> !theDM.talking);
                            
                            StartCoroutine(RideFish());

                            theDB.trigOverList.Add(trigNum);
                            preserveTrigger = false;
                            repeatBifur =0 ;
                        }
                        else{//ㅇㄷ
                            theDM.ShowDialogue(dialogue_1);
                            yield return new WaitUntil(()=> !theDM.talking);
                        }
                    }
                    else{//ㅇㄷ
                        theDM.ShowDialogue(dialogue_1);
                        yield return new WaitUntil(()=> !theDM.talking);
                    }
                }
                else{//ㅇㄷ
                    theDM.ShowDialogue(dialogue_1);
                    yield return new WaitUntil(()=> !theDM.talking);
                }
            }
            else{//오답
                //bifur=0;
                theDM.ShowDialogue(dialogue_1);
                yield return new WaitUntil(()=> !theDM.talking);
            }
        }



        theOrder.Move(); 
            

        thePlayer.isInteracting = false;
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

    IEnumerator RideFish(){
        thePlayer.notMove = true;
        thePlayer.isInteracting = true;
        Fade2Manager.instance.FadeOut();
        yield return new WaitForSeconds(1.5f);
        fish.SetActive(false);
                AudioManager.instance.Play("water1");
        thePlayer.notMove = true;
        thePlayer.animator.SetBool("onFish",true);
        thePlayer.transform.position = fish.transform.position;
        yield return new WaitForSeconds(0.5f);
        Fade2Manager.instance.FadeIn();
        moveFlag[0]= true;
        yield return new WaitUntil(()=>!moveFlag[0]);
        ObjectManager.instance.ImageFadeIn(FadeManager.instance.fog0.GetComponent<Image>(),0.015f);
        moveFlag[1]= true;
        //yield return new WaitUntil(()=>!moveFlag[0]);

        
    }

    void FixedUpdate(){
        if(moveFlag[0]){
            if(thePlayer.transform.position!=moveLocations[0].position){
                thePlayer.transform.position = Vector3.MoveTowards(thePlayer.transform.position,
                moveLocations[0].position, 3f* Time.deltaTime); 
                thePlayer.animator.SetFloat("Horizontal", -1f);
                thePlayer.animator.SetFloat("Speed", 1f);
            }
            else{
                moveFlag[0]=false;
            }
        }
        else if(moveFlag[1]){
            if(thePlayer.transform.position!=moveLocations[1].position){
                thePlayer.transform.position = Vector3.MoveTowards(thePlayer.transform.position,
                moveLocations[1].position, 3f* Time.deltaTime); 
                thePlayer.animator.SetFloat("Vertical", 1f);
                thePlayer.animator.SetFloat("Speed", 1f);
            }
            else{
                moveFlag[1]=false;
            }
        
        }
    }
}

