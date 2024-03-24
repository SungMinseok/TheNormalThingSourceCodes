using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle1 : MonoBehaviour
{
    public static Puzzle1 instance;
    public game2 game2;
    public GameObject book;
    //public Animator animator;
    public bool inMain;
    public GameObject[] sprites;

    public Button[] buttons;
    public GameObject bulb;
    public GameObject normalFace;
    public GameObject happyFace;
    public GameObject shiningFace;


    public GameObject rope;
    public GameObject hangedBulb;
    public GameObject shiningBulb;
    public GameObject bugs;
    public GameObject band;

   
    
    [Header ("전구")]
    public Dialogue dialogue_1; //전구
    [Header ("얼굴")]
    public Dialogue dialogue_2; //얼굴
    [Header ("밧줄에 빈 전구 달기")]
    public Dialogue dialogue_8; //밧줄에 전구 달기
    [Header ("밧줄에 전구 달기")]
    public Dialogue dialogue_3; //밧줄에 전구 달기
    [Header ("전구에 가루 주기")]
    public Dialogue dialogue_4; //전구에 가루 주기
    [Header ("game 완성")]
    public Dialogue dialogue_5; //game 완성
    [Header ("힌트")]
    public Dialogue dialogue_6; //힌트
    [Header("밧줄")]
    public Dialogue dialogue_7; //밧줄




















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
        //theSelect = SelectManager.instance;
        //theOrder = OrderManager.instance;
        thePlayer = PlayerManager.instance;
        theDB = DatabaseManager.instance;
        //theCamera = CameraMovement.instance;
        //theMap= MapManager.instance;
        thePuzzle= PuzzleManager.instance;
        theGame= GameManager.instance;

        thePlayer.isPlayingPuzzle = true;
        thePlayer.notMove = true;

        //if(Inventory.instance.inventoryItemList.Contains())
        
    }
    void OnEnable(){
        //Invoke("SpritesOn",0.5f);
        inMain = true;

        if(DatabaseManager.instance.puzzleOverList.Contains(1))
            SpritesOn();

        if(DatabaseManager.instance.gameOverList.Contains(2)){ //상춰 메꿔줌 - 노말표정
            band.SetActive(true);
            bugs.SetActive(false);
            //normalFace.SetActive(true);
        }
        else{
            band.SetActive(false);
            bugs.SetActive(true);
            //normalFace.SetActive(true);
        }

        if(DatabaseManager.instance.gameOverList.Contains(4)){ //반짝전구 - 해피표정 // 전구에 가루 뿌린 후
            rope.SetActive(false);
            hangedBulb.SetActive(false);
            //hangedBulb.GetComponent<Button>().interactable = false;
            shiningBulb.SetActive(true);
            shiningFace.SetActive(true);
            //normalFace.SetActive(false);
            //happyFace.SetActive(true);
            bulb.SetActive(false);
        }
        else if(DatabaseManager.instance.gameOverList.Contains(3)){
            rope.SetActive(false);
            hangedBulb.SetActive(true);
            shiningBulb.SetActive(false);
            shiningFace.SetActive(false);
            bulb.SetActive(false);
        }
        else{
            
            rope.SetActive(true);
            hangedBulb.SetActive(false);
            shiningBulb.SetActive(false);
            shiningFace.SetActive(false);
            if(!Inventory.instance.SearchItem(4) && !Inventory.instance.SearchItem(6) ){
                
                bulb.SetActive(true);
            }
            else{
                
                bulb.SetActive(false);
            }
        }

        if(DatabaseManager.instance.gameOverList.Contains(2)&&DatabaseManager.instance.gameOverList.Contains(4)){ 
            
            normalFace.SetActive(false);
            happyFace.SetActive(true);
        }
        else if(DatabaseManager.instance.gameOverList.Contains(2) || DatabaseManager.instance.gameOverList.Contains(4)){

            // if(DatabaseManager.instance.gameOverList.Contains(2)){
            //     game2.ResetGame();
            // }
            normalFace.SetActive(true);
            happyFace.SetActive(false);
        }
        else{

            normalFace.SetActive(false);
            happyFace.SetActive(false);
        }
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
    public void GetBulb()
    {   
        if(!Inventory.instance.SearchItem(4)&&!Inventory.instance.SearchItem(6)){
                
            StartCoroutine(EventCoroutine1(dialogue_1));
            buttonOff();    
        }  
    }
    IEnumerator EventCoroutine1(Dialogue dialogue){

        thePlayer.notMove = true;
        Inventory.instance.GetItem(4);
        AudioManager.instance.Play("getitem2");
        bulb.SetActive(false);
        theDM.ShowDialogue(dialogue);
        yield return new WaitUntil(()=> !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)
        
        
        Invoke("buttonOn",0.2f); 
    }
    public void TalkFace(){
        
        if(!theDB.gameOverList.Contains(2)){
            StartCoroutine(EventCoroutine2(dialogue_2));
            buttonOff();     
        }
    }
    IEnumerator EventCoroutine2(Dialogue dialogue){
        thePlayer.notMove = true;
        AudioManager.instance.Play("bugsound");
        theDM.ShowDialogue(dialogue);
        yield return new WaitUntil(()=> !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)
        
        
        //theGame.gameNum[2].SetActive(true); //game2
        //theGame.game2.SetActive(true);
        //thePuzzle.treeFace.SetActive(false);
        
        inMain = false;
        SpritesOff();
        Fade2Manager.instance.FadeOut();
        yield return new WaitForSeconds(1f);            
        theGame.game2.SetActive(true);
        yield return new WaitForSeconds(0.1f); 
            //if(!DatabaseManager.instance.gameOverList.Contains(2)){
                game2.ResetGame();
            //}
        Fade2Manager.instance.FadeIn();



        
        AudioManager.instance.Play("bugsound");
        theDB.doorEnabledList.Add(10);
        theDB.doorEnabledList.Add(11);
        theDB.doorEnabledList.Add(12);
        Invoke("buttonOn",0.2f); 
    }

    public void HangBulb(){
        if(theDB.OnActivated[6]){
            theDB.OnActivated[6] = false; 
            theDB.gameOverList.Add(3);
            
            Inventory.instance.RemoveItem(6);
            rope.SetActive(false);
            hangedBulb.SetActive(true);
            
            CursorManager.instance.RecoverCursor();


            StartCoroutine(EventCoroutine3(dialogue_3));
            buttonOff();      



        }
        else if(theDB.OnActivated[4]){
            theDB.OnActivated[4] = false; 
            //theDB.gameOverList.Add(3);
            
            //Inventory.instance.RemoveItem(6);
            //rope.SetActive(false);
            //hangedBulb.SetActive(true);
            
            CursorManager.instance.RecoverCursor();


            //StartCoroutine(EventCoroutine3(dialogue_3));
            buttonOff();      
            StartCoroutine(EventCoroutine8(dialogue_8));

        Invoke("buttonOn",0.2f); 

        }
        else if(theDB.OnActivated[0]){
            StartCoroutine(Inventory.instance.WrongUse());
            buttonOff();      
        }
        else TouchRope();
    }
    IEnumerator EventCoroutine3(Dialogue dialogue){
        thePlayer.notMove = true;
        AudioManager.instance.Play("rope");
        theDM.ShowDialogue(dialogue);
        yield return new WaitUntil(()=> !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)
        
        
        Invoke("buttonOn",0.2f); 
    }
    IEnumerator EventCoroutine8(Dialogue dialogue){
        //thePlayer.notMove = true;
        //AudioManager.instance.Play("rope");
        theDM.ShowDialogue(dialogue);
        yield return new WaitUntil(()=> !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)
        
        
        Invoke("buttonOn",0.2f); 
    }
    public void GiveAsh(){  //게임3끝
        if(theDB.OnActivated[5]){
            theDB.OnActivated[5] = false; 
            //animator.SetInteger("state",2);
            theDB.gameOverList.Add(4);
            Inventory.instance.RemoveItem(5);
            hangedBulb.SetActive(false);
            //hangedBulb.GetComponent<Button>().interactable = false;
            shiningBulb.SetActive(true);
            shiningFace.SetActive(true);
            normalFace.SetActive(true);
            //happyFace.SetActive(true);
            //bulb.SetActive(true);
            //theDB.trigOverList.Add(19);
            AudioManager.instance.Play("getitem0");
        
            
            CursorManager.instance.RecoverCursor();
            
            StartCoroutine(EventCoroutine4(dialogue_4));

            
#if ADD_ACH
        if(!theDB.gameOverList.Contains(2)){

            Debug.Log("업적12");
            if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(12);
        }
#endif


            buttonOff();      

        }
        else if(theDB.OnActivated[0]){
            StartCoroutine(Inventory.instance.WrongUse());
            buttonOff();      
        }
        else return;
    }
    IEnumerator EventCoroutine4(Dialogue dialogue){
        thePlayer.notMove = true;
        AudioManager.instance.Play("ice");
        theDM.ShowDialogue(dialogue);
        yield return new WaitUntil(()=> !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)
        
        //theDB.puzzleOverList.Add(1);   
        
        Invoke("buttonOn",0.2f); 
    }
    public void FinishGame(){   //게임2끝
        StartCoroutine(EventCoroutine5(dialogue_5));
        buttonOff();      
    }
    IEnumerator EventCoroutine5(Dialogue dialogue){
        thePlayer.notMove = true;
        //animator.SetInteger("state",1);
        bugs.SetActive(false);
        band.SetActive(true);
        Trig14.instance.bugs.SetActive(false);
        Trig14.instance.band.SetActive(true);
        normalFace.SetActive(true);
        theDM.ShowDialogue(dialogue);
        yield return new WaitUntil(()=> !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)
        
        theDB.doorEnabledList.Add(13);
        //theDB.puzzleOverList.Add(1);   
        
        Invoke("buttonOn",0.2f); 
    }
    public void GetHint(){
        StartCoroutine(EventCoroutine6(dialogue_6));
        buttonOff();      
    }
    IEnumerator EventCoroutine6(Dialogue dialogue){
        thePlayer.notMove = true;
        theDM.ShowDialogue(dialogue);
        yield return new WaitUntil(()=> !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)
        
        //theDB.puzzleOverList.Add(1);   
        
        //buttonOn();
        Invoke("buttonOn",0.2f); 
    }
    public void TouchRope(){
        StartCoroutine(EventCoroutine7(dialogue_7));
        buttonOff();      
    }
    IEnumerator EventCoroutine7(Dialogue dialogue){
        thePlayer.notMove = true;
        theDM.ShowDialogue(dialogue);
        yield return new WaitUntil(()=> !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)
        
        //theDB.puzzleOverList.Add(1);   
        
        Invoke("buttonOn",0.2f); 
    }
    public void TouchFace(){
        Trig21.instance.StartCoroutine("EventCoroutine");
        buttonOff();
    }
    
    #endregion
    
    public void exitPuzzle(){
        inMain = false;
        SpritesOff();
            AudioManager.instance.Play("button20");
        CursorManager.instance.RecoverCursor();
        StartCoroutine(exitPuzzleCoroutine());

    }
    
    IEnumerator exitPuzzleCoroutine(){

        Fade2Manager.instance.FadeOut();
        yield return new WaitForSeconds(1f);            
        thePuzzle.puzzleNum[1].SetActive(false);            
        Fade2Manager.instance.FadeIn();
        thePlayer.notMove = false;
        thePlayer.isPlayingPuzzle = false;
    }

    void FixedUpdate(){
        if(theDB.gameOverList.Contains(4)&&theDB.gameOverList.Contains(2)&&!happyFace.activeSelf){
            happyFace.SetActive(true);
            theDB.puzzleOverList.Add(1);   
#if ADD_ACH
            Debug.Log("업적23");
            if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(23);
#endif
            SpritesOn();

            thePlayer.autoSave = true;

                    thePlayer.exc.SetBool("on",false);
                    thePlayer.canInteractWith = 0;
        }
        
        if(theDB.puzzleOverList.Contains(1)){
                
            if(BookManager.instance.book.activeSelf&&sprites[0].activeSelf){
                SpritesOff();
            }
            else if(!BookManager.instance.book.activeSelf&&!sprites[0].activeSelf&&inMain){
                SpritesOn();
            }
        }

        
        //ToggleBtnsBox();
    }

    public void SpritesOff(){
        sprites[0].gameObject.SetActive(false);
        sprites[1].gameObject.SetActive(false);
        // ObjectManager.instance.FadeOut(sprites[0]);
        // ObjectManager.instance.FadeOut(sprites[1]);
    }
    public void SpritesOn(){

        sprites[0].gameObject.SetActive(true);
        sprites[1].gameObject.SetActive(true);
        // ObjectManager.instance.FadeIn(sprites[0].GetComponent<SpriteRenderer>());
        // ObjectManager.instance.FadeIn(sprites[1].GetComponent<SpriteRenderer>());
        // ObjectManager.instance.FadeIn(sprites[2].GetComponent<SpriteRenderer>());
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
    
    // void Update(){
        
    //     if(Input.GetKeyDown(KeyCode.Escape)&&!thePlayer.isPlayingGame&&!BookManager.instance.ReadableIsOn()){
    //         exitPuzzle();
    //     }
    // }
}
