using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
public class Trig26 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    public static Trig26 instance;
    public int trigNum;

    // [Header ("텐드 진입 전")]
    // public Select select_1;
    // [Header ("거미줄 완료 후")]
    // public Select select_2;
    [Header ("깃털 줍기")]
    public Select select_1;
    [Header ("안 줍는다 선택")]
    public Dialogue dialogue_3;
    [Header ("앵무새 얼굴 크게")]
    public Dialogue dialogue_4;
    [Header ("앵무새 화면 꺼진 후 놀랄 때")]
    public Dialogue dialogue_1;
    [Header ("앵무새 날아간 후 in 중간숲")]
    public Dialogue dialogue_2;
    [Header ("앵무새 날아간 후 in 비오는숲")]
    public Dialogue dialogue_5;
    [Header ("앵무새 날아가기 직전 in 앵무히든")]
    public Dialogue dialogue_6;
    [Header ("앵무새 날아간 후 in 앵무히든")]
    public Dialogue dialogue_7;

    public Animator bird;
    public SpriteRenderer birdShadow;
    public Transform moveLocation;
    public Transform moveLocation_Ruby;
    public Transform moveLocation_Eat;
    public GameObject feather;

    public GameObject trig73;
    public GameObject paper73;
    
    
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
    
    public bool canMove;
    public bool rubyMove;
    
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
            bird.gameObject.SetActive(false);
            if(feather!=null)
                feather.SetActive(false);
        }

        if(thePlayer.currentMapName=="parrothidden"&&!theDB.trigOverList.Contains(73)&&theDB.gameOverList.Contains(22)){
            trig73.SetActive(true);
            paper73.SetActive(true);
        }

    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    private void OnTriggerStay2D(Collider2D collision){
        //if(theDB.trigOverList.Contains(24)&&!GameManager.instance.playing&&!theDB.gameOverList.Contains(6)){
            // if(!thePlayer.exc.GetBool("on")&&!flag){
            //     thePlayer.exc.SetBool("on",true);
            //     thePlayer.canInteractWith = trigNum;
            // }
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


    IEnumerator EventCoroutine(){
        thePlayer.isInteracting = true;
        theOrder.NotMove();  
        theOrder.PreLoadCharacter();  
        if(turn!="")      
            theOrder.Turn("Player",turn);
                                               //트리거 중 이동불가


        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작
        if(trigNum ==26){

            BGMManager.instance.FadeOutMusic();
            theSelect.ShowSelect(select_1);
            
            yield return new WaitUntil(() => !theSelect.selecting);
            if(theSelect.GetResult()==1){
                
                theDM.ShowDialogue(dialogue_3);
                yield return new WaitUntil(()=> !theDM.talking);
            }
                thePlayer.isInteracting = true;
            rubyMove = true;
            yield return new WaitUntil(()=> !rubyMove);


            thePlayer.animator.SetTrigger("grab");
            Inventory.instance.GetItem(12);
            feather.SetActive(false);
            yield return new WaitForSeconds(1f);


            Fade2Manager.instance.red.SetActive(true);
            Fade2Manager.instance.birdImage.gameObject.SetActive(true);



            bird.gameObject.SetActive(true);
            AudioManager.instance.Play("shout0");
            yield return new WaitForSeconds(2f);
            // theDM.ShowDialogue(dialogue_4);
            Fade2Manager.instance.red.SetActive(false);
            
            // yield return new WaitUntil(()=> !theDM.talking);
            //yield return new WaitForSeconds(3f);
            //Fade2Manager.instance.StartCoroutine("FadeOutImageCoroutine",Fade2Manager.instance.birdImage);
            //StartCoroutine(Fade2Manager.instance.FadeOutImageCoroutine(Fade2Manager.instance.birdImage));
            thePlayer.spriteRenderer.flipX = true;
            thePlayer.animator.SetTrigger("surprised");
            //yield return new WaitForSeconds(1f);
            yield return new WaitForSeconds(2.433f);
            thePlayer.spriteRenderer.flipX = false;

            Fade2Manager.instance.birdImage.gameObject.SetActive(false);
            theDM.ShowDialogue(dialogue_1);
            yield return new WaitUntil(()=> !theDM.talking);

            bird.SetTrigger("flyaway");
            AudioManager.instance.Play("wing1");
            bird.GetComponent<SpriteRenderer>().flipX = true;
            birdShadow.gameObject.SetActive(false);
            canMove= true;
            yield return new WaitForSeconds(1.5f);
            ObjectManager.instance.FadeOut(bird.GetComponent<SpriteRenderer>());
            yield return new WaitForSeconds(1f);

            theDM.ShowDialogue(dialogue_2);
            yield return new WaitUntil(()=> !theDM.talking);
            BGMManager.instance.FadeinMusic();
        }
        else if(trigNum==58){

            theDB.progress=6;
            BookManager.instance.ActivateUpdateIcon(2);
            bird.SetTrigger("flyaway");
            AudioManager.instance.Play("wing1");
            //bird.GetComponent<SpriteRenderer>().flipX = true;
            if(birdShadow!=null) ObjectManager.instance.FadeOut(birdShadow);
            canMove= true;
            yield return new WaitForSeconds(2f);
            ObjectManager.instance.FadeOut(bird.GetComponent<SpriteRenderer>());
            //yield return new WaitForSeconds(0.1f);

            theDM.ShowDialogue(dialogue_5);
            yield return new WaitUntil(()=> !theDM.talking);
        }
        else if(trigNum==60){
            thePlayer.transform.position = moveLocation_Ruby.position;
            theOrder.Turn("Player","RIGHT");
            bird.transform.position = moveLocation_Eat.position;
            //bird.GetComponent<PositionRendererSorter>().offset = 2;
            
            yield return new WaitForSeconds(0.2f);

            bird.SetTrigger("eat");
            
            yield return new WaitForSeconds(2f);
            AudioManager.instance.Play("laugh0");
            yield return new WaitForSeconds(1f);

            theDM.ShowDialogue(dialogue_6);
            yield return new WaitUntil(()=> !theDM.talking);

            bird.GetComponent<PositionRendererSorter>().offset = -1;
            birdShadow.gameObject.SetActive(false);
            bird.SetTrigger("flyaway");
            AudioManager.instance.Play("wing1");
            bird.GetComponent<SpriteRenderer>().flipX = true;
            canMove= true;
            yield return new WaitForSeconds(0.2f);
            trig73.SetActive(true);
            paper73.SetActive(true);
            yield return new WaitForSeconds(4f);
            paper73.GetComponent<PositionRendererSorter>().offset = 4;
            theDM.ShowDialogue(dialogue_7);
            yield return new WaitUntil(()=> !theDM.talking);
            //trig73.SetActive(true);

            Debug.Log("업적4 : 착한 친구");
            
            if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(4);
            ObjectManager.instance.FadeOut(bird.GetComponent<SpriteRenderer>());
        }

        theOrder.Move(); 
            
        thePlayer.isInteracting = false;

        if(onlyOnce)
            theDB.trigOverList.Add(trigNum);
        if(preserveTrigger)
            flag=false;
    }

    void FixedUpdate(){
        if(canMove&&bird.gameObject.activeSelf){
            //Debug.Log("!!");
            if(bird.gameObject.transform.position!=moveLocation.position){   //나무 앞까지 이동
                //Debug.Log("이동");
                bird.gameObject.transform.position = Vector3.MoveTowards(bird.gameObject.transform.position,
                moveLocation.position, 7f* Time.deltaTime); 
            }
            else canMove = false;

        }

        if(rubyMove){
            if(thePlayer.gameObject.transform.position!=moveLocation_Ruby.position){   //나무 앞까지 이동
                //Debug.Log("이동");
                thePlayer.animator.SetFloat("Horizontal", -1f);
                thePlayer.animator.SetFloat("Speed", 1f);
                thePlayer.gameObject.transform.position = Vector3.MoveTowards(thePlayer.gameObject.transform.position,
                moveLocation_Ruby.position, 3f* Time.deltaTime); 
            }
            else{
                rubyMove = false;
                //thePlayer.animator.SetFloat("Horizontal", 0f);
                thePlayer.animator.SetFloat("Speed", 0f);
            
            
            }
        }
    }
}

