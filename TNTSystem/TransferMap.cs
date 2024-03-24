using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class TransferMap : MonoBehaviour
{
    public static TransferMap instance;
    [Header ("문 소리(없으면 기본값)")]public string doorSound;
    //public GameObject Door;   //이동 로케이션 연계
    [Header ("문 번호")]public int doorNum;
    [Header ("이동 방향 (시계방향 0,1,2,3)")]public int direction;
    [Header ("이동할 맵")]public string transferMapName;  //이동할 맵의 이름
    //[Header ("둘 다 false가 기본값")]
    [Header ("열린상태")]public bool Enabled; 
    [Header ("잠긴상태")]public bool locked;             //door의 enable=false > locked=true
    
    //public bool single_use; //한번 사용.
    
    
    [Header ("대사1: 아직 갈 수 없다(기본값)")]public Dialogue dialogue_1;     //맨처음 대화
    
    [Header ("대사2: 문 잠겼을 때 대화")]public Dialogue dialogue_2;     //잠겼을 때 대화
    
    
    [Header ("대사1 반복가능")]public bool preserveTrigger1 = true;   
    private bool singleUse1;
    
    [Header ("대사2 반복가능")]public bool preserveTrigger2 = true;
    private bool singleUse2;
    
    
    private PlayerManager thePlayer;

    private FadeManager theFade;
    private OrderManager theOrder;
    protected DatabaseManager theDB;
    private DialogueManager theDM;
    private BoxCollider2D thisBox;
    private UnknownScript theUnknown;
    AudioManager theAudio;

    [Header ("언노운 쫓길 때 바로 재 이동 가능 : true")]
    public bool onWaitingFlag = false;  //언노운 쫓길때 5초후 재 진입가능토록.


    [Header ("이동 애니메이션 삭제 : true")]
    public bool activated = false; //이동 애니메이션
    private bool transportFlag;

    // public GameObject doorClosed;
    // public GameObject doorOpened;
    //private Transform transform;
    void Start()
    {
        instance = this;
        thePlayer = FindObjectOfType<PlayerManager>();
        thisBox = GetComponent<BoxCollider2D>();
        theUnknown = FindObjectOfType<UnknownScript>();
        //transform = GetComponent<Transform>();
        //this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        thisBox.isTrigger = true;
        theFade = FadeManager.instance;
        theOrder = OrderManager.instance;
        theDB = DatabaseManager.instance;
        theDM = DialogueManager.instance;
        theAudio=AudioManager.instance;
        
        if(theDB.doorEnabledList.Contains(doorNum)){//DB list에 있으면 문 열린 상태.
            Enabled = true;
        }
        if(theDB.doorLockedList.Contains(doorNum)){//DB lockedlist에 있으면 문 잠김.
            locked = true;
        }
        
        if(dialogue_1==null){
            dialogue_1.names[0] = "나";
            dialogue_1.sentences[0] = "아직 이곳을 잘 살펴보지 못했다.\n 아무것도 모른채로 다른 곳으로는 갈 수 없다.";
        }
        //if(!lockedDoor.GetComponent<TransferMap>().Enabled)
        //    locked = true;
        //else if(!theDB.doorList.Contains(doorNum)){
        //    Enabled = false;
            //theDB.doorList.Add(doorNum);
        //}    

        if(!thePlayer.isChased) onWaitingFlag =true;

        if(MapManager.instance.ForceEnableDoors) Enabled = true;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player" && Enabled && !theDM.talking && !locked)
        { 
            if(doorSound!=""){
                theAudio.Play(doorSound);
            }
            else{
                theAudio.Play("run0");
//                Debug.Log("run0 플레이");
            } 
//            Debug.Log("11");
            if(DebugManager.instance.isDemo==doorNum){
                StartCoroutine(LoadingTrig.instance.PopupDemo());
                //DemoPlay();
            }
            else StartCoroutine(TransferCoroutine());
           
        }
        else if(collision.gameObject.name == "Player" && !Enabled && !locked && !singleUse1  && !theDM.talking)     //들어갈 수 없고(!Enabled), 하지만 잠겨있진않고(!locked)
        { //Debug.Log("22");
            singleUse1 = true;
            if(dialogue_1.names.Length!=0)
                StartCoroutine(EventCoroutine(dialogue_1));
        }
        
        else if(collision.gameObject.name == "Player" && locked && !singleUse2  && !theDM.talking)
        { //Debug.Log("33");
            
            singleUse2 = true;
            
            if(dialogue_2.names.Length!=0)
                StartCoroutine(EventCoroutine(dialogue_2));
        }
    }

    IEnumerator TransferCoroutine(){
        thePlayer.isTransporting = true;//언노운 떄문에 있음.
        thePlayer.isTransporting2 = true;//이동효과때문에 있음
        activated = true;
        theOrder.NotMove();
        theFade.FadeOut();
        thePlayer.lastMapName = thePlayer.currentMapName;    //s
        thePlayer.currentMapName = transferMapName;
        thePlayer.transferMapCount +=1;

        Inventory.instance.ApplyExpiration();

        thePlayer.MapCountCheck(transferMapName);

        //UnknownManager.instance.RenewalLocation();

            // switch(direction){
            //     case 0 :
            //         thePlayer.animator.SetFloat("Vertical", 1f);
            //         thePlayer.animator.SetFloat("Horizontal", 0f);
            //         thePlayer.movementDirection = new Vector2(0f,1f);
            //         break;
            //     case 1 :
            //         thePlayer.animator.SetFloat("Vertical", 0f);
            //         thePlayer.animator.SetFloat("Horizontal", 1f);
            //         break;
            //     case 2 :
            //         thePlayer.animator.SetFloat("Vertical", -1f);
            //         thePlayer.animator.SetFloat("Horizontal", 0f);
            //         break;
            //     case 3 :
            //         thePlayer.animator.SetFloat("Vertical", 0f);
            //         thePlayer.animator.SetFloat("Horizontal", -1f);
            //         break;
            // }
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
        //언노운 맵 페이즈 체크//
        UnknownManager.instance.nowPhase = UnknownManager.instance.CheckMaps();

        //플레이어 색 변경//
        StartCoroutine(thePlayer.ChangeColor());



        //코너우드 언노운 첫 출현시//
        // if(doorNum==14&&!theDB.trigOverList.Contains(22)&&thePlayer.isChased){
        //     UnknownScript.instance.canMove = false;
        //     thePlayer.isChased = false;
        //     //BGMManager.instance.SetVolume(0f);
        //     yield return new WaitForSeconds(1f);
            
        //     Destroy(UnknownScript.instance.gameObject);
        //     BGMManager.instance.trackNum=1;
        //             //BGMManager.instance.FadeOutNPlay(1);

        // }
        
        
        //if(thePlayer.isChased){
        if(thePlayer.isChased){
            if(doorNum==35||doorNum==16||doorNum==2) UnknownScript.instance.DestroyUnknownTrig();
            //ActivateUnknownToWait();
            yield return new WaitForSeconds(1f);
            //UnknownScript.instance.spriteRenderer.color = new Color (1,1,1,0);
            Debug.Log("1");
            
            //SceneManager.LoadScene(transferMapName);
            StartCoroutine(LoadCoroutine(transferMapName));
            
            // theFade.FadeIn(0.02f);
            // theOrder.Move();
        }
        else{

            Debug.Log("2");
            yield return new WaitForSeconds(1f);
            
            //SceneManager.LoadScene(transferMapName);
            StartCoroutine(LoadCoroutine(transferMapName));
            
            // theFade.FadeIn();
            // theOrder.Move();
        }
//#if ADD_ACH
            
        if(doorNum==35&&game26.instance.moveCount==4){
            Debug.Log("업적16");        
            if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(16);
        }
            
//#endif
        //onWaitingFlag = false;
        thePlayer.isTransporting = false;
        thePlayer.isTransporting2 = false;
        //EnableColliders();

    }
    void FixedUpdate(){
        if(Enabled&&!theDB.doorEnabledList.Contains(doorNum)){     //진행 중 문 열리고, DB에 닫힌 상태라면> 문 오픈상태로 저장.
            //flag=true;
            theDB.doorEnabledList.Add(doorNum);
            //theDB.dooronList.Add(true);

        }
        else if (!Enabled&&theDB.doorEnabledList.Contains(doorNum)){          //puzzle0 때문에 만듦.
            Enabled = true;
            //theDB.doorEnabledList.Add(doorNum);
        }
        if(locked) Enabled = false;
        //locked = Enabled ? false : true;
        // if(thePlayer.isChased&&!onWaitingFlag){
        //     onWaitingFlag = true;
        //     StartCoroutine(DisableCollider());
        // }
        // else onWaiting = false;
        if(thePlayer.isChased&&thisBox.enabled&&!onWaitingFlag){
            onWaitingFlag = true;
            
            StartCoroutine(DisableCollider());
        }
        if(doorNum!=1) TransportAnim();
    }
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


        if(preserveTrigger1)
            singleUse1=false;
        if(preserveTrigger2)
            singleUse2=false;
    }


    // public void ActivateUnknownToWait(){
    //     //UnknownScript.instance.onWaiting = true;
    //     //ObjectManager.instance.FadeOut(UnknownScript.instance.spriteRenderer, false);
    //     theUnknown.remainingCount --;
    //     theUnknown.onWaiting = true;
    //     //UnknownScript.instance.canMove = false;
    // }
    
    public IEnumerator DisableCollider(){
        thisBox.enabled = false;

        yield return new WaitForSeconds(4.5f);
        //onWaiting = false;
        
        thisBox.enabled = true;
    }
    public void DisableColliders(){

Debug.Log("박스 무력화 : "+thePlayer.currentMapName);
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

        if(thePlayer.isTransporting2&&activated){   
            if(!transportFlag){
                transportFlag = true;
                //DisableColliders();
                thePlayer.boxCollider.enabled = false;
            }
            //thePlayer.animator.SetFloat("Vertical", 1f);
            thePlayer.animator.SetFloat("Speed", 1f);
            // if(transform.position.x>thePlayer.transform.position.x){
            //     thePlayer.transform.Translate(Vector3.right *Time.deltaTime *3f);
            // }
            // else{
                
            //     thePlayer.transform.Translate(Vector3.left *Time.deltaTime *3f);
            // }
            thePlayer.transform.Translate(new Vector3(thePlayer.movementDirection.x, thePlayer.movementDirection.y, 0)*Time.deltaTime*3f);
            //thePlayer.transform.position = Vector3.MoveTowards(thePlayer.transform.position,transform.position, 2f* Time.deltaTime); 
            //Debug.Log(transform.position);
        }
    }

    // void DoorSpriteChange(){
    //     //지름길 용
    //     doorClosed.SetActive(false);
    //     doorOpened.SetActive(true);
    // }

    // public void DemoPlay(){

        
    //                 Debug.Log("GameOver");
    //         LoadingTrig.instance.GameOver();
    //         thePlayer.isGameOver = true;
    //         UnknownManager.instance.activated = false;
    //         thePlayer.isChased = false;
    // }
    // public void StartLoading()
    // {
    //     StartCoroutine("load");
    // }
 
 
    AsyncOperation asyncOperation;
    IEnumerator LoadCoroutine(string transferMapName)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        asyncOperation = SceneManager.LoadSceneAsync(transferMapName, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = true;
        yield return asyncOperation;
            theFade.FadeIn();
            theOrder.Move();
        SceneManager.UnloadSceneAsync(currentScene);
 
    }
}
