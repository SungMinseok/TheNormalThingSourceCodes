using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle3 : MonoBehaviour
{
    public static Puzzle3 instance;
    public GameObject book;
    //public Animator animator;

    public bool inMain;
    public Button[] buttons;
    [Header ("오드아이 클릭시")]
    public Dialogue dialogue_0; //선인장을 얻었다
    [Header ("오드아이 재 클릭시")]
    public Dialogue dialogue_8; //선인장을 얻었다
    [Header ("퍼즐 완성시")]
    public Dialogue dialogue_1; //선인장을 얻었다
    [Header ("종이 획득시")]
    public Dialogue dialogue_9; //선인장을 얻었다
    [Header ("퍼즐 끝나고 오드아이 클릭시")]
    public Dialogue dialogue_2; //열리지 않는다
    [Header ("드림캐처 클릭시")]
    public Dialogue dialogue_3; //선인장을 꽂았다
    [Header ("앵무 깃털 사용시")]
    public Dialogue dialogue_4;
    [Header ("촛불 클릭시")]
    public Dialogue dialogue_5; //손잡이를 얻었다
    [Header ("병 클릭시")]
    public Dialogue dialogue_6; //열 수 없다
    [Header ("카드퍼즐 진입시")]
    public Dialogue dialogue_7; //손잡이를 꽂았다

    //////////////////////////////////////////////////////
    public SpriteRenderer[] sprites;
    private DialogueManager theDM;
    private PlayerManager thePlayer;
    private DatabaseManager theDB;
    private PuzzleManager thePuzzle;
    private GameManager theGame;
    BookManager theBook;
    
    private bool firstEnterCard;
    private bool firstEnterCircle;

    void Start()
    {
        instance = this;

        theDM = DialogueManager.instance;
        thePlayer = PlayerManager.instance;
        theDB = DatabaseManager.instance;
        thePuzzle= PuzzleManager.instance;
        theGame= GameManager.instance;
        theBook = BookManager.instance;

        thePlayer.isPlayingPuzzle = true;
        thePlayer.notMove = true;

    }

    void Update()
    {
        
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
        StartCoroutine(EventCoroutine(i));
    }
    IEnumerator EventCoroutine(int i){
        buttonOff();
        switch(i){
            case 0 ://오드아이
                if(!theDB.gameOverList.Contains(24)){
                        
                    if(!firstEnterCard){
                        firstEnterCard =true;
                        //AudioManager.instance.Play("drawer1");
                        theDM.ShowDialogue(dialogue_0);
                        yield return new WaitUntil(()=> !theDM.talking);
        inMain = false;

                        SpritesOff();

                        Fade2Manager.instance.FadeOut();
                        yield return new WaitForSeconds(1.2f);            
                        theGame.game24.SetActive(true);
                        yield return new WaitForSeconds(0.01f);   
                        //game24.instance.Shuffle();         
                        yield return new WaitForSeconds(0.1f);  
                        Fade2Manager.instance.FadeIn();
                        thePlayer.isPlayingGame = true;
                        theDM.ShowDialogue(dialogue_7);
                        yield return new WaitUntil(()=> !theDM.talking);
                        break;
                    }
                    else{
                        //AudioManager.instance.Play("drawer1");
                        theDM.ShowDialogue(dialogue_8);
                        yield return new WaitUntil(()=> !theDM.talking);
        inMain = false;
                        SpritesOff();
                        Fade2Manager.instance.FadeOut();
                        yield return new WaitForSeconds(1.2f);            
                        theGame.game24.SetActive(true);      
                        yield return new WaitForSeconds(0.01f);   
                        //game24.instance.Shuffle();           
                        yield return new WaitForSeconds(0.1f);  
                        Fade2Manager.instance.FadeIn();
                        thePlayer.isPlayingGame = true;
                        break;
                    }
                }
                else{
                        
                    //AudioManager.instance.Play("drawer1");
                    theDM.ShowDialogue(dialogue_2);
                    yield return new WaitUntil(()=> !theDM.talking);
        
                    break;
                }
            case 1 ://촛불
                //AudioManager.instance.Play("drawer1");
                AudioManager.instance.Play("climb"+Random.Range(0,4).ToString());
                theDM.ShowDialogue(dialogue_5);
                yield return new WaitUntil(()=> !theDM.talking);
        //buttonOn(); 
                break;
            case 2 ://병
                //AudioManager.instance.Play("drawer1");
                AudioManager.instance.Play("bottle"+Random.Range(0,3).ToString());
                theDM.ShowDialogue(dialogue_6);
                yield return new WaitUntil(()=> !theDM.talking);
        //buttonOn(); 
                break;
            case 3 ://드림캐처
                //if(!theDB.gameOverList.Contains(25)){
        inMain = false;
                        SpritesOff();
                    theGame.game25.SetActive(true);
                    thePlayer.isPlayingGame = true;
                    if(!firstEnterCircle){
                        firstEnterCircle=true;
                        theDM.ShowDialogue(dialogue_3);
                        yield return new WaitUntil(()=> !theDM.talking);
                    }
                //}
                break;

        }
        Invoke("buttonOn",0.2f);
        //buttonOn(); 
    }
    #endregion
    
    public void ExitPuzzle(){
        inMain = false;
        SpritesOff();
            AudioManager.instance.Play("button20");
        CursorManager.instance.RecoverCursor();
        StartCoroutine(ExitPuzzleCoroutine());
    }
    
    IEnumerator ExitPuzzleCoroutine(){
        Fade2Manager.instance.FadeOut();
        yield return new WaitForSeconds(1f);            
        thePuzzle.puzzleNum[3].SetActive(false);            
        Fade2Manager.instance.FadeIn();
        thePlayer.notMove = false;
        thePlayer.isPlayingPuzzle = false;
    }
    public void FinishCardGame(){
        StartCoroutine(FinishCardGameCoroutine());
    }
    public IEnumerator FinishCardGameCoroutine(){
        //Invoke("SpritesOn",1.5f);
        SpritesOn();
        //hePlayer.isPlayingGame=false;
                    theDB.activatedPaper=6;
                    BookManager.instance.ActivateUpdateIcon(0);
        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(()=> !theDM.talking);
                    //yield return new WaitForSeconds(1.5f);
        AudioManager.instance.Play("getitem0");
        theDM.ShowDialogue(dialogue_9);
        yield return new WaitUntil(()=> !theDM.talking);
#if ADD_ACH
        Debug.Log("업적25");
        if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(25);
#endif
        //Debug.Log("111");
                    //AudioManager.instance.Play("pageflip2");
    }
    public IEnumerator PutFeather(){
        yield return new WaitForSeconds(1f);     
        theDM.ShowDialogue(dialogue_4);
        yield return new WaitUntil(()=> !theDM.talking);
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
        // ObjectManager.instance.FadeIn(sprites[0]);
        // ObjectManager.instance.FadeIn(sprites[1]);
    }
    
    void OnEnable(){
        inMain = true;
        //Invoke("SpritesOn",1.5f);
        SpritesOn();
    }
    void FixedUpdate(){
        // if((theBook.book.activeSelf||theGame.game25.activeSelf)&&sprites[0].gameObject.activeSelf){
        //     SpritesOff();
        // }
        if(BookManager.instance.book.activeSelf&&sprites[0].gameObject.activeSelf){
            SpritesOff();
        }
        // else if(!theBook.book.activeSelf&&!sprites[0].gameObject.activeSelf&&inMain){
        //     //SpritesOn();
        //     Invoke("SpritesOn",1.5f);
        // }
        else if(!BookManager.instance.book.activeSelf&&!sprites[0].gameObject.activeSelf&&inMain){
            SpritesOn();
        }

        ToggleBtnsBox();
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
