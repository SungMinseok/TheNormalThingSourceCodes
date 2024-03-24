using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle2 : MonoBehaviour
{
    public static Puzzle2 instance;
    //public Animator animator;
    //public Rigidbody2D lizard;
    public GameObject book;
    private bool toggleBtns = true;
    public Button[] buttons;
    public GameObject cactus;
    public GameObject boxClosed;
    public GameObject cactusUsed;
    public GameObject boxOpened;
    public GameObject drawer0Closed;
    public GameObject handle;
    public GameObject handleUsed;
    public GameObject drawer0Opened;
    public GameObject key;
    public GameObject drawer1Closed;
    public GameObject drawer1Opened;
    public GameObject matches;
    public Image fire;
    public Image num5;

    public bool canExit;//처음 나갈때만 대사 침
    [Header ("진입 시")]
    public Dialogue dialogue_0; //선인장을 얻었다
   
    
    [Header ("선인장")]
    public Dialogue dialogue_1; //선인장을 얻었다
    [Header ("상자(선인장 꽂기 전)")]
    public Dialogue dialogue_2; //열리지 않는다
    [Header ("상자(선인장 꽂은 후)")]
    public Dialogue dialogue_3; //선인장을 꽂았다
    [Header ("상자(열기)")]
    public Dialogue dialogue_13;
    // [Header ("상자(상자 열기)")]
    // public Dialogue dialogue_4; //상자 열기

    [Header ("손잡이")]
    public Dialogue dialogue_4; //손잡이를 얻었다
    [Header ("왼쪽서랍(손잡이 달기 전)")]
    public Dialogue dialogue_5; //열 수 없다
    [Header ("왼쪽서랍(손잡이 단 후)")]
    public Dialogue dialogue_6; //손잡이를 꽂았다
    [Header ("왼쪽서랍(열기)")]
    public Dialogue dialogue_14;
    [Header ("키")]
    public Dialogue dialogue_7; //키를 얻었다.
    [Header ("오른쪽 서랍")]
    public Dialogue dialogue_8; 
    [Header ("성냥")]
    public Dialogue dialogue_9;
    [Header ("양초")]
    public Dialogue dialogue_10; 
    [Header ("불 켜기(양초)")]
    public Dialogue dialogue_11; 
    [Header ("출구(열쇠 없을 때)")]
    public Dialogue dialogue_12; 
    [Header ("출구(열쇠 있을 때)")]
    public Dialogue dialogue_21; 
    [Header ("출구(열기)")]
    public Dialogue dialogue_15;
    [Header ("10 액자")]
    public Dialogue dialogue_16;
    [Header ("11 화분")]
    public Dialogue dialogue_17;
    [Header ("12 유리병, 박스")]
    public Dialogue dialogue_18;
    [Header ("13 종이들")]
    public Dialogue dialogue_19;
    [Header ("14 커튼")]
    public Dialogue dialogue_20;




















    //////////////////////////////////////////////////////
    private DialogueManager theDM;
    //private SelectManager theSelect;
    //private OrderManager theOrder;
    private PlayerManager thePlayer;
    private DatabaseManager theDB;
    //private BookManager theBook;
    //private MapManager theMap; 
    //private CameraMovement theCamera;
    private PuzzleManager thePuzzle;
    private GameManager theGame;
    void Start()
    {
        instance = this;

        theDM = DialogueManager.instance;
        thePlayer = PlayerManager.instance;
        theDB = DatabaseManager.instance;
        thePuzzle= PuzzleManager.instance;
        theGame= GameManager.instance;

        thePlayer.isPlayingPuzzle = true;
        thePlayer.notMove = true;

        

        // if(theDB.gameOverList.Contains(7)) 
        //     cactus.SetActive(false);
        // if(theDB.gameOverList.Contains(8)){
        //     cactusUsed.SetActive(true);
        // }
        // if(theDB.gameOverList.Contains(9)){
        //     boxClosed.SetActive(false);
        //     cactusUsed.SetActive(false);
        //     boxOpened.SetActive(true);
        //     handle.SetActive(true);
        // }
        // if(theDB.gameOverList.Contains(10)){
        //     handle.SetActive(false);
        // }
        // if(theDB.gameOverList.Contains(11)){
        //     handleUsed.SetActive(true);
        // }
        // // if(theDB.gameOverList.Contains(12)){
        // //     drawer0Closed.SetActive(false);
        // //     handleUsed.SetActive(false);
        // //     drawer0Opened.SetActive(true);
        // //     key.SetActive(true);
        // // }
        // if(theDB.gameOverList.Contains(13)){
        //     key.SetActive(false);
        //     canExit = true;
        // }
        // // if(theDB.gameOverList.Contains(14)){
        // //     drawer1Closed.SetActive(false);
        // //     drawer1Opened.SetActive(true);
        // //     matches.SetActive(true);
        // // }
        // if(theDB.gameOverList.Contains(15)){
        //     matches.SetActive(false);
        // }
        // if(theDB.gameOverList.Contains(16)){
        //     fire.gameObject.SetActive(true);
        //     num5.gameObject.SetActive(true);
        // }
    }
    void OnEnable(){
        //서랍 일단 다 닫기+ 상자 닫기.
        drawer0Closed.SetActive(true);
        drawer0Opened.SetActive(false);
        drawer1Closed.SetActive(true);
        drawer1Opened.SetActive(false);
        boxClosed.SetActive(true);
        boxOpened.SetActive(false);


        
        if(DatabaseManager.instance.gameOverList.Contains(7)) 
            cactus.SetActive(false);
        else{

            cactus.SetActive(true);
        }
        if(DatabaseManager.instance.gameOverList.Contains(9)){
            boxClosed.SetActive(false);
            cactusUsed.SetActive(false);
            boxOpened.SetActive(true);
            if(!Inventory.instance.SearchItem(8)) handle.SetActive(true);
        }
        else{
            if(DatabaseManager.instance.gameOverList.Contains(8)){
                cactusUsed.SetActive(true);
            }
            else{
                cactusUsed.SetActive(false);
            }
        }
        if(DatabaseManager.instance.gameOverList.Contains(10)){
            handle.SetActive(false);
        }
        if(DatabaseManager.instance.gameOverList.Contains(11)){
            handleUsed.SetActive(true);
        }
        else{
            handleUsed.SetActive(false);
        }
        // if(theDB.gameOverList.Contains(12)){
        //     drawer0Closed.SetActive(false);
        //     handleUsed.SetActive(false);
        //     drawer0Opened.SetActive(true);
        //     key.SetActive(true);
        // }
        if(DatabaseManager.instance.gameOverList.Contains(13)){
            key.SetActive(false);
            canExit = true;
        }
        else{
            canExit = false;
        }
        // if(theDB.gameOverList.Contains(14)){
        //     drawer1Closed.SetActive(false);
        //     drawer1Opened.SetActive(true);
        //     matches.SetActive(true);
        // }
        if(DatabaseManager.instance.gameOverList.Contains(16)){
            Debug.Log("16클");
            fire.gameObject.SetActive(true);
            num5.gameObject.SetActive(true);
            fire.color = new Color(1,1,1,1);
            num5.color = new Color(1,1,1,1);
        }
        else{
            Debug.Log("16클x");
            fire.gameObject.SetActive(false);
            num5.gameObject.SetActive(false);
            // if(DatabaseManager.instance.gameOverList.Contains(15)){
            //     matches.SetActive(false);
            // }
            // else{
            //     matches.SetActive(true);
            // }
        }

        //PlayerManager.instance.GetComponent<Rigidbody2D>().isKinematic =true;
    }


    #region ButtonOn/Off
    public void BtnOnAccess(){
        Invoke("buttonOn",0.2f);
    }
    public void buttonOn(){
        for(int i =0 ; i<buttons.Length; i++){                  //트리거 후 버튼 활성화
            buttons[i].GetComponent<Button>().interactable = true;
            //buttons[i].interactable = true;
        }
    }

    public void buttonOff(){
        for(int i =0 ; i<buttons.Length; i++){                  //트리거 중 버튼 비활성화
            buttons[i].GetComponent<Button>().interactable = false;
            //buttons[i].interactable = false;
        }
    }
    #endregion

    #region EventBtns
    public void EventStart(int i)
    {   
        //if(!Inventory.instance.SearchItem(4)&&!Inventory.instance.SearchItem(6)){
            // StartCoroutine(EventCoroutine1(dialogue_1));
            // buttonOff();    
        //}  
        StartCoroutine(EventCoroutine(i));
    }
    IEnumerator EventCoroutine(int i){
    
// 선인장뽑기	Eve1
// 선인장꽂기(닫힌상자선택)	Eve2
// 선인장상자열기(닫힌상자선택)	Eve2
// 핸들먹기	Eve3
// 핸들꽂기(닫힌서랍0선택)	Eve4
// 서랍0열기(닫힌서랍0선택)	Eve4
// 키얻기	Eve5
// 서랍1열기	Eve6
// 성냥얻기	Eve7
// 초켜기(숫자5밝혀짐)	Eve8
        buttonOff();
        switch(i){
            case 0 :
                AudioManager.instance.Play("lockdoor");
                if(!canExit){
                    yield return new WaitForSeconds(2f);
                    theDM.ShowDialogue(dialogue_0);
                    yield return new WaitUntil(()=> !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)

                }
                break;
// 선인장뽑기	Eve1
            case 1 :
                theDB.gameOverList.Add(7);
                //yield return new WaitForSeconds(0.5f);
                theDM.ShowDialogue(dialogue_1);
                yield return new WaitUntil(()=> !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)
                Inventory.instance.GetItem(7);
                AudioManager.instance.Play("plant0");
                cactus.SetActive(false);
                break;
            case 2 :
                if(theDB.OnActivated[7]){//선인장꽂았다
                    theDB.gameOverList.Add(8);
                    theDB.OnActivated[7] = false; 
                    CursorManager.instance.RecoverCursor();
                    Inventory.instance.RemoveItem(7);
                    cactusUsed.SetActive(true);
                    AudioManager.instance.Play("key0");
                    theDM.ShowDialogue(dialogue_3);
                    yield return new WaitUntil(()=> !theDM.talking); 
                }
                else if(theDB.OnActivated[0]){
                    StartCoroutine(Inventory.instance.WrongUse());
                }
                else if(theDB.gameOverList.Contains(8)){
                    theDB.gameOverList.Add(9);
                    //상자 열림.
                    cactusUsed.SetActive(false);
                    boxClosed.SetActive(false);
                    boxOpened.SetActive(true);
                    handle.SetActive(true);
                    AudioManager.instance.Play("door2");
                    theDM.ShowDialogue(dialogue_13);
                    yield return new WaitUntil(()=> !theDM.talking); 
                }
                else{//열리지 않는다
                    //AudioManager.instance.Play("getitem2");
                    theDM.ShowDialogue(dialogue_2);
                    yield return new WaitUntil(()=> !theDM.talking); 
                }
                break;
            case 3 :
                theDB.gameOverList.Add(10);
                theDM.ShowDialogue(dialogue_4);
                yield return new WaitUntil(()=> !theDM.talking); 
                handle.SetActive(false);
                Inventory.instance.GetItem(8);
                AudioManager.instance.Play("getitem2");
                break;
            case 4 :
                if(theDB.OnActivated[8]){//핸들꽂
                    theDB.gameOverList.Add(11);
                    theDB.OnActivated[8] = false; 
                    CursorManager.instance.RecoverCursor();
                    Inventory.instance.RemoveItem(8);
                    handleUsed.SetActive(true);
                    AudioManager.instance.Play("wood0");
                    theDM.ShowDialogue(dialogue_6);
                    yield return new WaitUntil(()=> !theDM.talking); 
                }
                else if(theDB.OnActivated[0]){
                    StartCoroutine(Inventory.instance.WrongUse());
                }
                else if(theDB.gameOverList.Contains(11)){
                    theDB.gameOverList.Add(12);
                    //서랍 열림.
                    handleUsed.SetActive(false);
                    drawer0Closed.SetActive(false);
                    drawer0Opened.SetActive(true);
                    AudioManager.instance.Play("drawer0");
                    if(!theDB.gameOverList.Contains(13)){
                        key.SetActive(true);
                        theDM.ShowDialogue(dialogue_14);
                        yield return new WaitUntil(()=> !theDM.talking); 
                    }
                }
                else{//열리지 않는다
                    //AudioManager.instance.Play("getitem2");
                    theDM.ShowDialogue(dialogue_5);
                    yield return new WaitUntil(()=> !theDM.talking); 
                }
                break;
            case 5 :
                theDB.gameOverList.Add(13);
                theDM.ShowDialogue(dialogue_7);
                yield return new WaitUntil(()=> !theDM.talking); 
                key.SetActive(false);
                Inventory.instance.GetItem(9);
                AudioManager.instance.Play("getitem2");
                break;
            case 6 :
                theDB.gameOverList.Add(14);
                AudioManager.instance.Play("drawer0");
                drawer1Closed.SetActive(false);
                drawer1Opened.SetActive(true);
                if(!theDB.gameOverList.Contains(15)) matches.SetActive(true);
                theDM.ShowDialogue(dialogue_8);
                yield return new WaitUntil(()=> !theDM.talking); 
                break;
            case 7 :
                theDB.gameOverList.Add(15);
                theDM.ShowDialogue(dialogue_9);
                yield return new WaitUntil(()=> !theDM.talking); 
                matches.SetActive(false);
                Inventory.instance.GetItem(10);
                AudioManager.instance.Play("getitem2");
                break;
            case 8 :
                if(theDB.OnActivated[10]){//불켜기
                    theDB.gameOverList.Add(16);
                    theDB.OnActivated[10] = false; 
                    CursorManager.instance.RecoverCursor();
                    Inventory.instance.RemoveItem(10);
                    AudioManager.instance.Play("matches");
                    ObjectManager.instance.ImageFadeIn(fire);
                    yield return new WaitForSeconds(2f);
                    AudioManager.instance.Play("getitem0");
                    ObjectManager.instance.ImageFadeIn(num5);
                    yield return new WaitForSeconds(2f);
                    theDM.ShowDialogue(dialogue_11);
                    yield return new WaitUntil(()=> !theDM.talking);
//#if ADD_ACH
                    if(!canExit){
                        Debug.Log("업적17");
                        if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(17);
                    }
//#endif
                } 
                else if(theDB.OnActivated[0]){
                    StartCoroutine(Inventory.instance.WrongUse());
                }
                else if(!theDB.gameOverList.Contains(16)){
                    
                    //AudioManager.instance.Play("getitem2");
                    theDM.ShowDialogue(dialogue_10);
                    yield return new WaitUntil(()=> !theDM.talking);
                }
                break;
            case 9 :
                if(theDB.OnActivated[9]){//방 나가기
                    theDB.OnActivated[9] = false; 
                    CursorManager.instance.RecoverCursor();
                    Inventory.instance.RemoveItem(9);
                    if(!canExit){
                        canExit = true;
                        theDM.ShowDialogue(dialogue_15);
                        yield return new WaitUntil(()=> !theDM.talking);
                    }
                    AudioManager.instance.Play("escape0");
                    exitPuzzle();
                } 
                else if(theDB.OnActivated[0] && !canExit){
                    StartCoroutine(Inventory.instance.WrongUse());
                }
                else{
                    if(!canExit){
                        
                        AudioManager.instance.Play("clickdoor");
                        
                        if(Inventory.instance.SearchItem(9)){
                            
                            theDM.ShowDialogue(dialogue_21);
                            yield return new WaitUntil(()=> !theDM.talking);
                        }
                        else {

                            theDM.ShowDialogue(dialogue_12);
                            yield return new WaitUntil(()=> !theDM.talking);
                        }
                    }
                    else{
                            
                        AudioManager.instance.Play("escape0");
                        exitPuzzle();
                    }
                    //AudioManager.instance.Play("getitem2");
                }
                break;
            case 10 :
                AudioManager.instance.Play("bottle"+Random.Range(0,3).ToString());
                theDM.ShowDialogue(dialogue_16);
                yield return new WaitUntil(()=> !theDM.talking);
                break;
            case 11 :
                AudioManager.instance.Play("bottle"+Random.Range(0,3).ToString());
                theDM.ShowDialogue(dialogue_17);
                yield return new WaitUntil(()=> !theDM.talking);
                break;
            case 12 :
                AudioManager.instance.Play("bottle"+Random.Range(0,3).ToString());
                theDM.ShowDialogue(dialogue_18);
                yield return new WaitUntil(()=> !theDM.talking);
                break;
            case 13 :
                AudioManager.instance.Play("pageflip2");
                theDM.ShowDialogue(dialogue_19);
                yield return new WaitUntil(()=> !theDM.talking);
                break;
            case 14 :
                //AudioManager.instance.Play("bottle"+Random.Range(0,3).ToString());
                theDM.ShowDialogue(dialogue_20);
                yield return new WaitUntil(()=> !theDM.talking);
                break;
            case 15 : //왼쪽 서랍 닫기
                
                handleUsed.SetActive(true);
                drawer0Closed.SetActive(true);
                drawer0Opened.SetActive(false);
                key.SetActive(false);
                AudioManager.instance.Play("drawer1");
                break;
            case 16 : //오른쪽 서랍 닫기
                
                //handleUsed.SetActive(true);
                drawer1Closed.SetActive(true);
                drawer1Opened.SetActive(false);
                matches.SetActive(false);
                AudioManager.instance.Play("drawer1");
                break;
        }
        //buttonOn(); 
        Invoke("buttonOn",0.2f);
    }
    #endregion
    
    public void exitPuzzle(){
            AudioManager.instance.Play("button20");
        CursorManager.instance.RecoverCursor();
        StartCoroutine(exitPuzzleCoroutine());

    }
    
    IEnumerator exitPuzzleCoroutine(){

        Fade2Manager.instance.FadeOut();
        yield return new WaitForSeconds(1f);            
        thePuzzle.puzzleNum[2].SetActive(false);            
        Fade2Manager.instance.FadeIn();
        thePlayer.notMove = false;
        thePlayer.isPlayingPuzzle = false;
        theDB.isPlayingPuzzle2 = false;
    }

    void FixedUpdate(){
        //ToggleBtnsBox();
    }

    void ToggleBtnsBox(){

        if((book.activeSelf||thePlayer.isPlayingGame)&&buttons[0].GetComponent<BoxCollider2D>().enabled){
            //toggleBtns = false;
            for(int i=0;i<buttons.Length;i++){
                if(buttons[i].TryGetComponent(out BoxCollider2D temp))
                temp.enabled = false;
            }
        }
        else if((!book.activeSelf&&!thePlayer.isPlayingGame)&&!buttons[0].GetComponent<BoxCollider2D>().enabled){
            //toggleBtns = true;
            for(int i=0;i<buttons.Length;i++){
                if(buttons[i].TryGetComponent(out BoxCollider2D temp))
                temp.enabled = true;
            }
        }
    }

    
}
