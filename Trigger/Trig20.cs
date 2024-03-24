using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
public class Trig20 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    public static Trig20 instance;
    public int trigNum;
    public Transform createLocation;
    public float walkingToTreeSpeed=3f;
    public float climbingTreeSpeed=3f;
    public float fallingSpeed=5f;
    //public int test;
    //public string transferMapName;  //이동할 맵의 이름
    public Dialogue dialogue_1;//올라가자
    public Dialogue dialogue_2;//올라가자
    public Dialogue dialogue_3;//올라가자
    public Dialogue dialogue_4;//올라가자
    public string turn;
    //public GameObject centerView;
    public Transform[] target = new Transform [3]; 
    public GameObject boundary;
    public bool[] moveStart = new bool [3];

    public GameObject unknown;
    public GameObject paper;
    public Transform moveLocation;
    //public Queue<Transform> tempQue = new Queue<Transform>();

    
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
        instance=this;
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
        // if(theDB.puzzleOverList.Contains(1)){//트리거 실행 후 맵에 다시 돌아왔을 때 DB list에 들어가 있으면 다시 실행 안됨.
        //     flag = true;
        // }

    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    // private void OnTriggerStay2D(Collider2D collision){
    //     if(theDB.trigOverList.Contains(19)){
    //         if(collision.gameObject.name == "Player" && !flag && !autoEnable && Input.GetKeyDown(KeyCode.Space)&& !theDM.talking){
    //             flag = true;
    //             StartCoroutine(EventCoroutine());
    //         }

    //         if(collision.gameObject.name == "Player" && !flag && autoEnable&& !theDM.talking){
    //             flag = true;
    //             StartCoroutine(EventCoroutine());
    //         }

    //     }        
    // }
    

    public IEnumerator EventCoroutine(){

        theOrder.NotMove();  
        theOrder.PreLoadCharacter();  
        if(turn!="null")      
            theOrder.Turn("Player",turn);
                                               //트리거 중 이동불가


        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작
        

        //theDM.ShowDialogue(dialogue_1); //이제 올라가자



        //yield return new WaitUntil(()=> !theDM.talking);  
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
        yield return new WaitUntil(()=> !moveStart[1]); //나무 위 끝까지 올라감

        BGMManager.instance.FadeOutMusic();
        paper.SetActive(false);
        AudioManager.instance.Play("pageflip2");
        thePlayer.animator.SetInteger("fallPhase",-2);
        yield return new WaitForSeconds(1f);
        AudioManager.instance.Play("getitem0");
        theDM.ShowDialogue(dialogue_2); //종이를 얻었다! 어어어
        theDB.activatedPaper=2;
        BookManager.instance.ActivateUpdateIcon(0);

        yield return new WaitUntil(()=> !theDM.talking);
            AudioManager.instance.Play("falldown0");
        thePlayer.spriteRenderer.flipX=true;
        moveStart[2] = true;
            thePlayer.isClimbing = false;  
            thePlayer.isFalling = true;
            // thePlayer.shadow_climbing.gameObject.SetActive(false);
        // AudioManager.instance.Play("falldown0");
        // yield return new WaitForSeconds(1.2f);
        // AudioManager.instance.Play("falldown1");
        // yield return new WaitForSeconds(1.2f);
        // AudioManager.instance.Play("falldown2");
        // yield return new WaitForSeconds(1.2f);
        // AudioManager.instance.Play("falldown3");
        yield return new WaitUntil(()=> !moveStart[2]); //떨어짐
            //AudioManager.instance.Play("unknownwalk0");
            AudioManager.instance.Play("falldown1");
        
            thePlayer.isFalling = false;
        yield return new WaitForSeconds(1f);
        thePlayer.animator.SetInteger("fallPhase",3);//standup
        yield return new WaitForSeconds(1f);
            AudioManager.instance.Play("falldown2");
        thePlayer.animator.SetInteger("fallPhase",4);//end
            //thePlayer.isClimbing = false;
            // thePlayer.shadow_normal.gameObject.SetActive(true);
        
        thePlayer.spriteRenderer.flipX=false;
        theCamera.SetBound(bound1);
        thePlayer.sorter.offset = temp;
            yield return new WaitForSeconds(1.5f);
            AudioManager.instance.Play("falldown3");
        theDM.ShowDialogue(dialogue_3); //" 으악!! "
// 내 비명소리와 함께 쿵 소리가 숲에 울려 퍼졌다.
// 그런데 안쪽에서 희미한 소리가 들린다.
// 쿵 . .. 쿵 .. 쿵 .. 
// 어라? 뭐지 내가 떨어진 소리의 메아리인가?
// 그런데 그 소리는 점점 가까워지고 커진다.
// 쿵.... 쿵.... 쿵.. 쿵.. 쿵.. 쿵쿵쿵쿵쿵쿵!!
        yield return new WaitUntil(()=> !theDM.talking);  
        
            yield return new WaitForSeconds(1f);
            //AudioManager.instance.Play("falldown3");
            //AudioManager.instance.Play("falldown3");

        theDB.doorEnabledList.Add(14);

///////////언노운 출현

        UnknownManager.instance.nowPhase = 3;
        Instantiate(unknown , createLocation.position , Quaternion.identity);
        UnknownScript.instance.moveLocation.Add(moveLocation);


        theCamera.target=GameObject.Find("Unknown(Clone)").gameObject;
        






        //unknown.SetActive(true);

        // tempQue.Enqueue(moveLocation);
        // unknown.GetComponent<UnknownScript>().que = tempQue;
        //De

        theDM.ShowDialogue(dialogue_4); //
// 뭔지모를 까만색 무언가가 나를 향해 달려온다. 너무 놀란 나는 뒤돌아 뛰기 시작했다.



        yield return new WaitUntil(()=> !theDM.talking);  

                 


        theCamera.target=thePlayer.gameObject;

        boundary.SetActive(true);
        


        thePlayer.notMove = false;
        
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

            if(thePlayer.transform.position!=target[1].position){   //나무 위로 이동
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
            }
        }
        
    }

}

