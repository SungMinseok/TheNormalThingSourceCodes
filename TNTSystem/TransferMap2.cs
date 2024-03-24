using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class TransferMap2 : MonoBehaviour
{
    
    //public static TransferMap2 instance;
    [Header ("문 소리(없으면 기본값)")]public string doorSound;
    [Header ("이동 방향 (시계방향 0,1,2,3)")]public int direction;
    [Header ("이동할 미로")]public TransferMap2 goTo;
    [Header ("이동할 미로 번호")]public int mazeNum;
    [Header ("해당 미로의 카메라 영역")]public BoxCollider2D myCameraBound;
    [Header ("해당 미로의 이동 불가 영역")]public GameObject myBoundary;
    [Header ("밑에서 나올 경우 체크")]public bool fromBottom;
    //[Header ("나올 위치")]public BoxCollider2D myCameraBound;
    
    public bool moveFlag;
    
    private PlayerManager thePlayer;

    private FadeManager theFade;
    private OrderManager theOrder;
    protected DatabaseManager theDB;
    private DialogueManager theDM;
    private BoxCollider2D thatBox;
    private UnknownScript theUnknown;
    private CameraMovement theCamera;
    AudioManager theAudio;

    [Header ("언노운 쫓길 때 바로 재 이동 가능 : true")]
    public bool onWaitingFlag = false;  //언노운 쫓길때 5초후 재 진입가능토록.


    //[Header ("이동 애니메이션 삭제 : true")]
    public bool activated = false; //이동 애니메이션
    private bool transportFlag;

    // public GameObject doorClosed;
    // public GameObject doorOpened;
    //private Transform transform;
    void Start()
    {
        //instance = this;
        thePlayer = FindObjectOfType<PlayerManager>();
        thatBox = goTo.GetComponent<BoxCollider2D>();
        theUnknown = FindObjectOfType<UnknownScript>();
        //transform = GetComponent<Transform>();
        //this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        thatBox.isTrigger = true;
        theFade = FadeManager.instance;
        theOrder = OrderManager.instance;
        theDB = DatabaseManager.instance;
        theDM = DialogueManager.instance;
        theAudio=AudioManager.instance;
        theCamera=CameraMovement.instance;
        
        //if(!thePlayer.isChased) onWaitingFlag =true;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player"&& !theDM.talking)
        { 
            if(doorSound!=""){
                theAudio.Play(doorSound);
            }
            else{
                theAudio.Play("run0");
//                Debug.Log("run0 플레이");
            } 
            
            StartCoroutine(TransferCoroutine());
           
        }
    }

    IEnumerator TransferCoroutine(){
        thePlayer.isTransporting = true;//언노운 떄문에 있음.

        thePlayer.isTransporting2 = true;//이동효과때문에 있음
        activated = true;
        game26.instance.moveCount++;
        theOrder.NotMove();
        theFade.FadeOut();
            switch(direction){
                case 0 :
                    thePlayer.animator.SetFloat("Vertical", 1f);
                    thePlayer.animator.SetFloat("Horizontal", 0f);
                    thePlayer.movementDirection = new Vector2(0f,1f);
                    break;
                case 1 :
                    thePlayer.animator.SetFloat("Vertical", 0f);
                    thePlayer.animator.SetFloat("Horizontal", 1f);
                    thePlayer.movementDirection = new Vector2(1f,0f);
                    break;
                case 2 :
                    thePlayer.animator.SetFloat("Vertical", -1f);
                    thePlayer.animator.SetFloat("Horizontal", 0f);
                    thePlayer.movementDirection = new Vector2(0f,-1f);
                    break;
                case 3 :
                    thePlayer.animator.SetFloat("Vertical", 0f);
                    thePlayer.animator.SetFloat("Horizontal", -1f);
                    thePlayer.movementDirection = new Vector2(-1f,0f);
                    break;
            }
        //thePlayer.lastMapName = thePlayer.currentMapName;    //s
        //thePlayer.currentMapName = transferMapName;
        thePlayer.pointInMaze = goTo.transform;
        thePlayer.mazeNum = mazeNum-1;
        //moveFlag = true;
        yield return new WaitForSeconds(1.5f);
        
        theCamera.SetBound(goTo.myCameraBound);
        yield return new WaitForSeconds(0.5f);
        //SceneManager.LoadScene(mazeNum);
        //
        StartCoroutine(DisableCollider());

        if(goTo.fromBottom) thePlayer.transform.position = new Vector3(goTo.transform.position.x, goTo.transform.position.y + 3.5f, goTo.transform.position.z);
        else thePlayer.transform.position = goTo.transform.position;
       // Debug.Log(gameObject.name);
    
        //EnableColliders();

        //moveFlag = true;    //도착 후 이동
        //Invoke("ArriveDone",2f);
        
        theFade.FadeIn();
        theOrder.Move();
        //onWaitingFlag = false;
        thePlayer.isTransporting = false;
        thePlayer.isTransporting2 = false;
        activated = false;
            //if(!myBoundary.activeSelf)
                myBoundary.SetActive(true);
        //EnableColliders();

    }
    void FixedUpdate(){
        // if(thePlayer.isChased&&thisBox.enabled&&!onWaitingFlag){
        //     onWaitingFlag = true;
        //     StartCoroutine(DisableCollider());
        // }
        // else{
        //     onWaitingFlag = true;
        // }
        // if(moveFlag){
        //     thePlayer.transform.Translate(new Vector3(thePlayer.movementDirection.x, thePlayer.movementDirection.y, 0)*Time.deltaTime*3f);
        // }
        //Debug.Log(moveFlag);
        TransportAnim();
        //if(moveFlag){
            //ArriveAnim();
        //}
        //Debug.Log(myBoundary);
    }
    // void ArriveAnim(){
    //     if(moveFlag){
    //         if(thePlayer.transform.position != goTo.transform.position)
         
    //         {
    //             DisableColliders();
    //             thePlayer.transform.position = Vector3.MoveTowards(thePlayer.transform.position,
    //             goTo.transform.position, 1f* Time.deltaTime); 
    //             thePlayer.animator.SetFloat("Vertical", 1f);
    //             thePlayer.animator.SetFloat("Speed", 1f);
    //         }
    //         else{
    //             EnableColliders();
    //             StartCoroutine(DisableCollider());
    //             moveFlag = false;
    //             thePlayer.animator.SetFloat("Vertical", 0f);
    //             thePlayer.animator.SetFloat("Speed", 0f);
    //         }    //thePlayer.transform.Translate(new Vector3(thePlayer.movementDirection.x, thePlayer.movementDirection.y, 0)*Time.deltaTime*3f);
    //     }
    // }
    // void ArriveDone(){
    //     moveFlag=false;
    // }
    IEnumerator EventCoroutine(Dialogue dialogue){

        //PlayerManager.instance.notMove = true;
        theOrder.NotMove();
        //theOrder.PreLoadCharacter();  
        //if(turn!="null")      
        //    theOrder.Turn("Player",turn);
                                               //트리거 중 이동불가


        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작

        theDM.ShowDialogue(dialogue);
        yield return new WaitUntil(()=> !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)
        
        PlayerManager.instance.notMove = false;
        // if(thePlayer.notMove)
        //     theOrder.Move();                                                        //트리거 완료 후 이동가능
        //Debug.Log("h1");


    }


    // public void ActivateUnknownToWait(){
    //     //UnknownScript.instance.onWaiting = true;
    //     //ObjectManager.instance.FadeOut(UnknownScript.instance.spriteRenderer, false);
    //     theUnknown.remainingCount --;
    //     theUnknown.onWaiting = true;
    //     //UnknownScript.instance.canMove = false;
    // }
    
    public IEnumerator DisableCollider(){
        //thisBox.enabled = false;
        //DisableColliders();
        thatBox.enabled = false;

        yield return new WaitForSeconds(4.5f);
        //onWaiting = false;
        //EnableColliders();
        //thisBox.enabled = true;
        thatBox.enabled = true;
    }
    public void DisableColliders(){

        BoxCollider2D[] collider2Ds=FindObjectsOfType(typeof(BoxCollider2D)) as BoxCollider2D[];
    
        foreach(BoxCollider2D col in collider2Ds){
            col.enabled = false;
        }
    }
    public void EnableColliders(){
        
        BoxCollider2D[] collider2Ds=FindObjectsOfType(typeof(BoxCollider2D)) as BoxCollider2D[];
    
        foreach(BoxCollider2D col in collider2Ds){
            col.enabled = true;
        }
    }

    void TransportAnim(){

        if(/*thePlayer.isTransporting2*/activated){
            //if(activated){   
            //if(!transportFlag){
                //transportFlag = true;
            // if(!moveFlag){
            //     moveFlag = true;
            //     DisableColliders();
            // }
            //}
            //thePlayer.animator.SetFloat("Vertical", 1f);
            myBoundary.SetActive(false);
            thePlayer.animator.SetFloat("Speed", 2f);
            // if(transform.position.x>thePlayer.transform.position.x){
            //     thePlayer.transform.Translate(Vector3.right *Time.deltaTime *3f);
            // }
            // else{
                
            //     thePlayer.transform.Translate(Vector3.left *Time.deltaTime *3f);
            // }
            thePlayer.transform.Translate(new Vector3(thePlayer.movementDirection.x, thePlayer.movementDirection.y, 0)*Time.deltaTime*1f);
            //thePlayer.transform.position = Vector3.MoveTowards(thePlayer.transform.position,transform.position, 2f* Time.deltaTime); 
            //Debug.Log(transform.position);
            //}
        }
        else{
            //activated = false;
            //EnableColliders();
            // if(moveFlag){
            //     moveFlag= false;
            //     EnableColliders();
            // }
        }
    }

}
