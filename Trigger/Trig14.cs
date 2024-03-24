using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
public class Trig14 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    public static Trig14 instance;
    public int trigNum;
    public float walkingToTreeSpeed=3f;
    public float climbingTreeSpeed=3f;
    public float fallingSpeed=5f;
    public GameObject band;
    public GameObject bugs;
    public GameObject rope;
    public GameObject hangedBulb;
    public GameObject shiningBulb;
    //public int test;
    //public string transferMapName;  //이동할 맵의 이름
    //public Dialogue dialogue_1;//올라가자
    public Dialogue dialogue_2;
    [Header ("화남")]
    public Select select_1;
    [Header ("보통")]
    public Select select_2;
    [Header ("해피")]
    public Select select_3;

    public string turn;
    //public GameObject centerView;
    
    public Transform[] target = new Transform [4]; 
    public GameObject boundary;
    public bool[] moveStart = new bool [3];
    public Animator animator;
    public Animator shake;

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
    public BoxCollider2D bound1;
    public BoxCollider2D bound2;
    private int temp;


    ///////////////////////////////////////////////////////////////////   flag는 실행여부 파악 : true이면 실행중/실행완료, false이면 실행전 //  bifer : 분기
    protected bool flag;    // true 이면 다시 실행 안됨.
    
    private int bifur;
    
    /////////////////////////////////////////////////////////////////////   inspector에서 체크 가능. 1: 닿으면 자동 실행, 2: 체크시 해당 트리거 무한 반복.
    public bool autoEnable;
    public bool preserveTrigger;
    public bool onlyOnce= true;

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

        if(theDB.gameOverList.Contains(2)){ //상처치료
            band.SetActive(true);
            bugs.SetActive(false);
            animator.SetInteger("state", 1);
            //thePuzzle.treeFace.GetComponent<Animator>().SetInteger("state", 1);
        }
        if(theDB.puzzleOverList.Contains(1)){   //
            animator.SetInteger("state", 2);
            //thePuzzle.treeFace.GetComponent<Animator>().SetInteger("state", 2);
        }
    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    private void OnTriggerStay2D(Collider2D collision){
        //Debug.Log("나는 "+trigNum+"번 트리거 위에 있다 + flag : "+flag);
        if(!theDB.trigOverList.Contains(20)){

            if(!thePlayer.exc.GetBool("on")&&!flag){
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

    }
    void OnTriggerEnter2D(Collider2D collision){

        if(!thePlayer.exc.GetBool("on")&&!flag){
            thePlayer.exc.SetBool("on",true);
            thePlayer.canInteractWith = trigNum;
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
        thePlayer.boxCollider.enabled = false;

        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작
        //두문제 성공
        if(theDB.gameOverList.Contains(2)&&theDB.gameOverList.Contains(4)){
            
            theSelect.ShowSelect(select_3);
            bifur=2;
        }
        //1문제 성공 (물든전구달기or상처메꾸기)
        else if(theDB.gameOverList.Contains(2)||theDB.gameOverList.Contains(4)){
            
            theSelect.ShowSelect(select_2);
            bifur=1;
        }
        //0문제 성공
        else{
            theSelect.ShowSelect(select_1); // 0올라가자 , 1말자
            bifur=0;
        }
        yield return new WaitUntil(() => !theSelect.selecting);

        if(theSelect.GetResult()==0){//올라가는데 3가지경우
            if(bifur==0||bifur==1){//올라가다 떨어짐

                boundary.SetActive(false);
                theCamera.SetBound(bound2);
                temp = thePlayer.sorter.offset;
                thePlayer.sorter.offset = -100;
                
                moveStart[0] = true;
                yield return new WaitUntil(()=> !moveStart[0]); //나무 앞까지감
                moveStart[1] = true;
            thePlayer.isClimbing = true;
            // thePlayer.shadow_normal.gameObject.SetActive(false);
            // thePlayer.shadow_climbing.gameObject.SetActive(true);
                thePlayer.animator.SetInteger("fallPhase",-1);
                yield return new WaitUntil(()=> !moveStart[1]); //나무 위 중간까지 올라감
                AudioManager.instance.Play("whip");
                shake.SetTrigger("shake");
                yield return new WaitForSeconds(0.5f);
                moveStart[2] = true;
            thePlayer.isClimbing = false;
            thePlayer.isFalling = true;
            // thePlayer.shadow_climbing.gameObject.SetActive(false);
                thePlayer.animator.SetInteger("fallPhase",-2);
                yield return new WaitUntil(()=> !moveStart[2]); //떨어짐
                AudioManager.instance.Play("rubyfall");
            thePlayer.isFalling = false;
                yield return new WaitForSeconds(0.6f);
                thePlayer.animator.SetInteger("fallPhase",3);//standup
                yield return new WaitForSeconds(1f);
                thePlayer.animator.SetInteger("fallPhase",4);//end
            // thePlayer.shadow_normal.gameObject.SetActive(true);
                theCamera.SetBound(bound1);
                thePlayer.sorter.offset = temp;
                theDM.ShowDialogue(dialogue_2);//아직 못올라가나보다
                yield return new WaitUntil(()=> !theDM.talking);
                boundary.SetActive(true);
                theOrder.Move();    
            }
            else if(bifur==2){
                Trig20.instance.StartCoroutine("EventCoroutine");
                preserveTrigger = false;
            }
        }
        else if(theSelect.GetResult()==1){
            //bifur=-1;
            theOrder.Move(); 
            //올라가지 말자 대화끝   
            
        }

        
        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 끝

        // Fade2Manager.instance.FadeOut();
        // yield return new WaitForSeconds(1f);
        
        // thePuzzle.puzzleNum[1].SetActive(true);
        // thePlayer.isPlayingPuzzle = true;
        // thePlayer.notMove = true;
        // Fade2Manager.instance.FadeIn();





        //theOrder.Move();                                                        //트리거 완료 후 이동가능
                //Debug.Log("h2");

        //theMap.blurList.Remove("blur1");
        //theMap.GetComponent<MapManager>().blur[0].SetActive(false);
        //thePlayer.GetComponent<PlayerManager>().speed = speed;

        thePlayer.boxCollider.enabled = true;
        
        if(onlyOnce)
            theDB.trigOverList.Add(trigNum);
        if(preserveTrigger)
            flag=false;
    }


    void FixedUpdate(){
        if(moveStart[0]){

            if(thePlayer.transform.position!=target[0].position){   //나무 앞까지 이동
                thePlayer.animator.SetFloat("Vertical", 1f);
                thePlayer.animator.SetFloat("Speed", 1f);
                thePlayer.transform.position = Vector3.MoveTowards(thePlayer.transform.position,
                target[0].position, walkingToTreeSpeed* Time.deltaTime); 
            }

            else{
                moveStart[0] = false;
                thePlayer.animator.SetFloat("Vertical", 0f);
                thePlayer.animator.SetFloat("Speed", 0f);
            }
        }

        if(moveStart[1] && !thePlayer.isHalting){

            if(thePlayer.transform.position!=target[1].position){   //나무 앞까지 이동
                //thePlayer.animator.SetBool("isClimbing", true);
                thePlayer.transform.position = Vector3.MoveTowards(thePlayer.transform.position,
                target[1].position, climbingTreeSpeed* Time.deltaTime); 
            }

            else{
                moveStart[1] = false;
                //thePlayer.animator.SetBool("isClimbing", false);
            }
        } 
        if(moveStart[2]){

            if(thePlayer.transform.position!=target[2].position){   //나무 앞까지 이동
                thePlayer.animator.SetInteger("fallPhase",1);
                thePlayer.transform.position = Vector3.MoveTowards(thePlayer.transform.position,
                target[2].position, fallingSpeed* Time.deltaTime); 
            }

            else{
                moveStart[2] = false;
                thePlayer.animator.SetInteger("fallPhase",2);
                thePlayer.animator.SetFloat("Vertical", 1f);
            }
        }
        



        
        if((theDB.gameOverList.Contains(2) || theDB.gameOverList.Contains(4)) && animator.GetInteger("state")==0 ){ //상처치료
            animator.SetInteger("state", 1);
            if(theDB.gameOverList.Contains(2)){

                band.SetActive(true);
                bugs.SetActive(false);
            }
            //thePuzzle.treeFace.GetComponent<Animator>().SetInteger("state", 1);
        }
        if((theDB.gameOverList.Contains(2) && theDB.gameOverList.Contains(4)) && animator.GetInteger("state")<=1){
            animator.SetInteger("state", 2);
            //thePuzzle.treeFace.GetComponent<Animator>().SetInteger("state", 2);
        }

        if(theDB.gameOverList.Contains(3)&&!theDB.gameOverList.Contains(4)){
            if(!hangedBulb.activeSelf){
                hangedBulb.SetActive(true);
                rope.SetActive(false);
            }
        }
        if(theDB.gameOverList.Contains(4)){
            if(!shiningBulb.activeSelf){
                shiningBulb.SetActive(true);
                hangedBulb.SetActive(false);
            }
        }
    }

}

