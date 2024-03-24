using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle0 : MonoBehaviour
{
    public static Puzzle0 instance;
    public GameObject book;
    public GameObject[] sprites;
    public bool inMain;
    ///////////////////////////////////////////////////
    public GameObject whatPuzzle;
    public GameObject[] buttons;
    public Dialogue dialogue_1; //얼굴꽃
    public Dialogue dialogue_2; //나뭇잎
    public Select select_1; //쓰러진꽃
    public Dialogue dialogue_6;
    public Select select_2;
    public Dialogue dialogue_3;
    public Dialogue dialogue_4;
    public Dialogue dialogue_5;
    public Dialogue dialogue_7;
    public Dialogue dialogue_8;
    [Header("땅바닥")]
    public Dialogue dialogue_9;

    [Header("햇빛에 비추기")]
    public Dialogue dialogue_10;

    [Header("왼쪽 꽃")]
    public Dialogue dialogue_11;


    public GameObject candy;
    public GameObject leaf;
    public GameObject broken;
    public GameObject afterfixed;
    //public GameObject door;
    public GameObject full;
    public GameObject edgeLeaf;

    //////////////////////////////////////////////////////
    private DialogueManager theDM;
    private SelectManager theSelect;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private DatabaseManager theDB;
    //private BookManager theBook;
    private MapManager theMap;
    private CameraMovement theCamera;
    private PuzzleManager thePuzzle;
    private GameManager theGame;
    public bool[] talkCount;

    void Start()                                                            //Don't Touch
    {
        instance = this;

        theDM = DialogueManager.instance;
        theSelect = SelectManager.instance;
        theOrder = OrderManager.instance;
        thePlayer = PlayerManager.instance;
        theDB = DatabaseManager.instance;
        theCamera = CameraMovement.instance;
        theMap = MapManager.instance;
        thePuzzle = PuzzleManager.instance;
        theGame = GameManager.instance;

        thePlayer.isPlayingPuzzle = true;
        talkCount = new bool[3] { false, false, false };
    }
    void OnEnable()
    {
        //초기화
        leaf.SetActive(true);
        edgeLeaf.SetActive(true);

        broken.SetActive(true);
        full.SetActive(false);
        afterfixed.SetActive(false);

        //
        if (Inventory.instance.SearchItem(3))
        {
            leaf.SetActive(false);
        }

        if (DatabaseManager.instance.gameOverList.Contains(0))
        {//뿌리퍼즐 맞췄으면 
            broken.SetActive(false);
            afterfixed.SetActive(true);
            //게임1을 깬 상태에서, 캔디를 가지고 있어
            // if(theDB.gameOverList.Contains(1)){
            //     if(Inventory.instance.SearchItem(1)){

            //     }
            // }




            // if(Inventory.instance.SearchItem(1)){
            //     leaf.SetActive(false);
            // }
            // else{
            //     leaf.SetActive(true);
            // }
        }
        //Invoke("SpritesOn",0.5f);


        inMain = true;
        SpritesOn();


    }
    public void BtnOnAccess()
    {
        Invoke("buttonOn", 0.2f);
    }
    public void buttonOn()
    {
        for (int i = 0; i < buttons.Length; i++)
        {                  //트리거 후 버튼 활성화
            buttons[i].GetComponent<Button>().interactable = true;
        }
    }

    public void buttonOff()
    {
        for (int i = 0; i < buttons.Length; i++)
        {                  //트리거 중 버튼 비활성화
            buttons[i].GetComponent<Button>().interactable = false;
        }
    }
    // Update is called once per frame
    public void dialogueStart_1()
    {
        if (!talkCount[0])
        {
            talkCount[0] = true;
            TalkCheck();
        }
        //if(CursorManager.instance._default){
        //OnActivatedReset();
        StartCoroutine(EventCoroutine1(dialogue_1));
        buttonOff();
        //}
    }
    public void dialogueStart_2()
    {
        //if(CursorManager.instance._default){

        //OnActivatedReset();
        StartCoroutine(EventCoroutine2(dialogue_2));
        buttonOff();
        //}
    }
    public void dialogueStart_3()
    {
        //if(CursorManager.instance._default){

        //OnActivatedReset();
        StartCoroutine(EventCoroutine1(dialogue_9));
        buttonOff();
        //}
    }
    public void dialogueStart_4()
    {
        //if(CursorManager.instance._default){

        if (!talkCount[1])
        {
            talkCount[1] = true;
            TalkCheck();
        }
        //OnActivatedReset();
        StartCoroutine(EventCoroutine1(dialogue_11));
        buttonOff();
        //}
    }
    public void SelectBroken()
    {
        //if(CursorManager.instance._default){

        //OnActivatedReset();
        //if(afterfixed.activeSelf){

        StartCoroutine(Broken());
        //buttonOff();

        // if(full.activeSelf)
        //     buttonOn();
        //}
        //}
    }
    public void SelectFixed()
    {
        if (!talkCount[2])
        {
            talkCount[2] = true;
            TalkCheck();
        }
        StartCoroutine(Fixed());
    }
    IEnumerator EventCoroutine1(Dialogue dialogue)
    {
        if (theDB.OnActivated[0])
        {
            CursorManager.instance.RecoverCursor();
            theDM.ShowDialogue(Inventory.instance.wrongUse);
            yield return new WaitUntil(() => !theDM.talking);
        }
        else
        {

            theDM.ShowDialogue(dialogue);
            yield return new WaitUntil(() => !theDM.talking);
        }                    //대화 끝날 때까지 대기 (마지막 제외 필수)
        Invoke("buttonOn", 0.2f);
    }
    IEnumerator EventCoroutine2(Dialogue dialogue)
    {

        theOrder.NotMove();
        theOrder.PreLoadCharacter();
        //if(turn!="null")
        //    theOrder.Turn("Player",turn);
        //트리거 중 이동불가


        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작

        theDM.ShowDialogue(dialogue);
        yield return new WaitUntil(() => !theDM.talking);                    //대화 끝날 때까지 대기 (마지막 제외 필수)
        Inventory.instance.GetItem(3);
        AudioManager.instance.Play("getitem2");
        // Debug.Log("3번?");

        leaf.SetActive(false);
                CursorManager.instance.interactable = false;
        Invoke("buttonOn", 0.2f);
    }
    IEnumerator Broken()
    {   //쓰러진 꽃 터치 > 심어줄까 > 게임or바로심어지기

        buttonOff();
        if (theDB.OnActivated[0])
        {
            CursorManager.instance.RecoverCursor();
            theDM.ShowDialogue(Inventory.instance.wrongUse);
            yield return new WaitUntil(() => !theDM.talking);
            Invoke("buttonOn", 0.2f);
        }
        else
        {
            theSelect.ShowSelect(select_1);       //심어줄까?
            yield return new WaitUntil(() => !theSelect.selecting);

            if (theSelect.GetResult() == 0)
            {       //응

                if (!theDB.gameOverList.Contains(0))
                {

                    inMain = false;
                    SpritesOff();

                    Fade2Manager.instance.FadeOut();
                    yield return new WaitForSeconds(1f);
                    theGame.game0.SetActive(true);
                    yield return new WaitForSeconds(0.01f);
                    game0.instance.GoRandom();
                    Fade2Manager.instance.FadeIn();
                }
                else
                {
                    StartCoroutine(SuccessImplantation());
                }
            }
            else if (theSelect.GetResult() == 1)
            {      //아니
                theDM.ShowDialogue(dialogue_3);
                yield return new WaitUntil(() => !theDM.talking);
                Invoke("buttonOn", 0.2f);
            }

        }

    }
    public IEnumerator SuccessImplantation()
    {   //심기 성공시or다시뽑기후 심기 성공시 바로 진행되는 멘트.
        buttonOff();
        Invoke("SpritesOn", 1f);
        broken.SetActive(false);
        afterfixed.SetActive(true);
        AudioManager.instance.Play("flowereyes0");
        theDM.ShowDialogue(dialogue_8);     //꽃을 심어줬다!

        yield return new WaitUntil(() => !theDM.talking);
        // theDM.ShowDialogue(dialogue_6);
        // yield return new WaitUntil(()=> !theDM.talking);
        // theSelect.ShowSelect(select_2);
        // yield return new WaitUntil(() => !theSelect.selecting);
        // if(theSelect.GetResult()==0){                       // 말을 건다

        //     theDM.ShowDialogue(dialogue_4);
        //     yield return new WaitUntil(()=> !theDM.talking);
        // }
        // else if(theSelect.GetResult()==1){                  // 다시 뽑는다
        //     /////처음으로
        //     AudioManager.instance.Play("mattouch");
        //     broken.SetActive(true);
        //     afterfixed.SetActive(false);
        // }
        Invoke("buttonOn", 0.2f);
    }
    public IEnumerator Fixed()
    { //심은 꽃 터치
        buttonOff();
        if (theDB.OnActivated[2])
        {//사탕 주기

            CursorManager.instance.RecoverCursor();
            Inventory.instance.RemoveItem(2);

            afterfixed.SetActive(false);
            full.SetActive(true);

            AudioManager.instance.Play("candy0");
            AudioManager.instance.Play("eat0");

            //theDB.doorEnabledList.Add(9);

        }
        else if (theDB.OnActivated[0])
        {

            CursorManager.instance.RecoverCursor();
            DialogueManager.instance.ShowDialogue(Inventory.instance.wrongUse);
            yield return new WaitUntil(() => !DialogueManager.instance.talking);
        }
        else
        {

            theDM.ShowDialogue(dialogue_6);
            yield return new WaitUntil(() => !theDM.talking);
            theSelect.ShowSelect(select_2);
            yield return new WaitUntil(() => !theSelect.selecting);
            if (theSelect.GetResult() == 0)
            {                       // 말을 건다

                theDM.ShowDialogue(dialogue_4);
                yield return new WaitUntil(() => !theDM.talking);
            }
            else if (theSelect.GetResult() == 1)
            {                  // 다시 뽑는다
                /////처음으로

                AudioManager.instance.Play("mattouch");
                broken.SetActive(true);
                afterfixed.SetActive(false);
            }
        }







        Invoke("buttonOn", 0.2f);
    }


    public void getCandy()
    {
        candy.SetActive(false);
    }
    public void getLeaf()
    {
        leaf.SetActive(false);
    }

    public void exitPuzzle()
    {
        inMain = false;
        SpritesOff();
        AudioManager.instance.Play("button20");
        CursorManager.instance.RecoverCursor();
        StartCoroutine(exitPuzzleCoroutine());
        //whatPuzzle.SetActive(false);
        //thePuzzle.buttonOn();
        //theGame.pass[0] = false;
        //thePuzzle.StopAllCoroutines();

    }

    IEnumerator exitPuzzleCoroutine()
    {

        Fade2Manager.instance.FadeOut();
        yield return new WaitForSeconds(1f);
        thePuzzle.puzzleNum[0].SetActive(false);
        Fade2Manager.instance.FadeIn();
        theOrder.Move();
        thePlayer.isPlayingPuzzle = false;
    }

    public void GiveCandy()
    {

        StartCoroutine(GiveCandyCoroutine());
    }

    IEnumerator GiveCandyCoroutine()
    {

        if (Inventory.instance.SearchItem(2))
        {       //사탕이 있으면 성공

            CursorManager.instance.RecoverCursor();
            afterfixed.SetActive(false);
            full.SetActive(true);

            AudioManager.instance.Play("candy0");

            AudioManager.instance.Play("eat0");
            theDB.puzzleOverList.Add(0);

            theDB.doorEnabledList.Add(9);
            theOrder.Move();
            Inventory.instance.RemoveItem(2);

            Invoke("buttonOn", 0.2f);

        }
        else
        {                                                         //사탕 없으면 다쉬
            theDM.ShowDialogue(dialogue_7);
            yield return new WaitUntil(() => !theDM.talking);
        }

    }

    public void getEdgeLeaf()
    {
        StartCoroutine(getEdgeLeafCoroutine());
    }
    IEnumerator getEdgeLeafCoroutine()
    {
        theDB.puzzleOverList.Add(0);
        AudioManager.instance.Play("getitem2");
        theDM.ShowDialogue(dialogue_5);
        yield return new WaitUntil(() => !theDM.talking);
#if ADD_ACH
        Debug.Log("업적22");
        if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(22);
#endif
    }






    public void OnActivatedReset()
    {                 // 선택된 템 초기화(databaseManager : OnActivated) 일단 사용안함

        for (int i = 0; i < 10; i++)
        {

            theDB.OnActivated[i] = false;
        }
    }


    public void GetLeafHint()
    {
        StartCoroutine(GetLeafHintCR());
    }

    IEnumerator GetLeafHintCR()
    {
        buttonOff();
        if (theDB.OnActivated[3])
        {//불켜기
            theDB.OnActivated[3] = false;
            CursorManager.instance.RecoverCursor();
            theDM.ShowDialogue(dialogue_10);
            yield return new WaitUntil(() => !theDM.talking);
        }
        Invoke("buttonOn", 0.2f);
    }


    public void SpritesOff()
    {
        sprites[0].gameObject.SetActive(false);
        sprites[1].gameObject.SetActive(false);
        sprites[2].gameObject.SetActive(false);
        // ObjectManager.instance.FadeOut(sprites[0]);
        // ObjectManager.instance.FadeOut(sprites[1]);
    }
    public void SpritesOn()
    {

        sprites[0].gameObject.SetActive(true);
        sprites[1].gameObject.SetActive(true);
        sprites[2].gameObject.SetActive(true);
        // ObjectManager.instance.FadeIn(sprites[0].GetComponent<SpriteRenderer>());
        // ObjectManager.instance.FadeIn(sprites[1].GetComponent<SpriteRenderer>());
        // ObjectManager.instance.FadeIn(sprites[2].GetComponent<SpriteRenderer>());
    }

    void FixedUpdate()
    {
        //ToggleBtnsBox();
        if (BookManager.instance.book.activeSelf && sprites[0].activeSelf)
        {
            SpritesOff();
        }
        else if (!BookManager.instance.book.activeSelf && !sprites[0].activeSelf && inMain)
        {
            SpritesOn();
        }
    }
    void ToggleBtnsBox()
    {

        if ((book.activeSelf || thePlayer.isPlayingGame) && buttons[0].GetComponent<BoxCollider2D>().enabled)
        {
            //toggleBtns = false;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].TryGetComponent(out BoxCollider2D temp))
                    temp.enabled = false;
            }
        }
        else if ((!book.activeSelf && !thePlayer.isPlayingGame) && !buttons[0].GetComponent<BoxCollider2D>().enabled)
        {
            //toggleBtns = true;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].TryGetComponent(out BoxCollider2D temp))
                    temp.enabled = true;
            }
        }
    }
    void TalkCheck()
    {

        //#if ADD_ACH
        if (!theDB.gameOverList.Contains(27) && talkCount[0] && talkCount[1] && talkCount[2])
        {
            theDB.gameOverList.Add(27);
            Debug.Log("업적9");

            if (SteamAchievement.instance != null) SteamAchievement.instance.ApplyAchievements(9);
        }
        //#endif
    }
}
