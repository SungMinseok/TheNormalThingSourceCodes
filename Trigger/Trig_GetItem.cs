using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
[RequireComponent(typeof(BoxCollider2D))]
public class Trig_GetItem : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    //public static Trig27 instance;
    [Header ("습득 아이템 번호")]
    public int itemNum;
    [Header ("습득 사운드")]
    public string getSound;
    [Header ("습득 전 대사")]
    public Dialogue dialogue_0;
    [Header ("습득 후 대사")]
    public Dialogue dialogue_1;
    [Header ("습득 조건(맵 넘버)")]
    [Tooltip ("0start\n1cabin\n2catwood\n3catwood2\n4ch2\n5ch3\n6cornerwood\n7camp\n8middlewood\n9village\n10lake\n11lakein\n12lakeout\n13rainingforest\n14parrothidden\n15thunderingforest\n16mazein\n17maze\n18mazeout\n19end\n")]
    public int mapNum = -1;
    [Header ("습득 조건(맵 진입 횟수)")]
    public int mapCount = -1;
    [Header ("아이템")]
    public GameObject item;
    [Header ("체크시 사용 후 재생성")]
    public bool recreatable;
    [Header ("체크시 한번만 생성")]
    public bool onlyOnce = true;
    [Header ("체크시 페이드아웃")]
    public bool shrara;
    [Header ("플로팅 이미지")]
    public Sprite sprite;
    

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
    
    
    /////////////////////////////////////////////////////////////////////   inspector에서 체크 가능. 1: 닿으면 자동 실행, 2: 체크시 해당 트리거 무한 반복.
    

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
        item = this.gameObject;

        if(mapNum==0) mapNum = -1;
        if(mapCount==0) mapCount = -1;

        if(getSound=="") getSound = "boosruck";

        if(recreatable){
            if(Inventory.instance.SearchItem(itemNum)){
                gameObject.SetActive(false);
            }
        }

        if(onlyOnce){
            if(!theDB.itemOverList.Contains(itemNum)&&MapCountCheck()){
                gameObject.SetActive(true);
            }
            else{
                gameObject.SetActive(false);
            }
        }
    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    private void OnTriggerStay2D(Collider2D collision){

        if(!thePlayer.exc.GetBool("on")&&!flag){
            thePlayer.exc.SetBool("on",true);
        }

        if(!flag&& (Input.GetKeyDown(KeyCode.Space)||thePlayer.getSpace)&& !theDM.talking){
            flag = true;
            thePlayer.exc.SetBool("on",false);
            StartCoroutine(EventCoroutine());
        }


    }
    void OnTriggerExit2D(Collider2D collision){
        thePlayer.exc.SetBool("on",false);
    }


    IEnumerator EventCoroutine(){

        theOrder.NotMove();  

        //습득전 대사 있을 때
        if(dialogue_0.names.Length != 0){

            //Debug.Log("111");
            theDM.ShowDialogue(dialogue_0);
            yield return new WaitUntil(()=> !theDM.talking);

            AudioManager.instance.Play(getSound);
            Inventory.instance.GetItem(itemNum);
            thePlayer.animator.SetTrigger("grab");
            BookManager.instance.ActivateUpdateIcon(1);
            
            if(shrara){
                ObjectManager.instance.FadeOut(gameObject.GetComponent<SpriteRenderer>());
            }
            else{
                gameObject.SetActive(false);      
            }
        }
        else{
            //Debug.Log("222");
            AudioManager.instance.Play(getSound);
            Inventory.instance.GetItem(itemNum);
            thePlayer.animator.SetTrigger("grab");
            BookManager.instance.ActivateUpdateIcon(1);
            
            ObjectManager.instance.FadeOut(gameObject.GetComponent<SpriteRenderer>(),0.03f,false);
            yield return new WaitForSeconds(1.5f);
            theDM.ShowDialogue(dialogue_1);
            yield return new WaitUntil(()=> !theDM.talking);
        }



            //Debug.Log("11111");
        if(theDB.itemList[itemNum-1].itemType==Item.ItemType.Passive){
            //Debug.Log("칼");
            Inventory.instance.ActivatePassiveItem(itemNum, sprite);
        }
        if(onlyOnce){
            theDB.itemOverList.Add(itemNum);
        }
        theOrder.Move();  
        gameObject.SetActive(false);
    }
    //출현 카운트 횟수만족시 true
    public bool MapCountCheck(){
        if(mapNum==-1||thePlayer.mapCheckList[mapNum]>=mapCount){
            return true;
        }
        else{
            return false;
        }
    }







}

