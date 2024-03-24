using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
[RequireComponent(typeof(BoxCollider2D))]
public class Trig32 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    //public static Trig27 instance;
    public int trigNum;

    // [Header ("텐드 진입 전")]
    // public Select select_1;
    // [Header ("거미줄 완료 후")]
    // public Select select_2;
    [Header("처음 대사//제단: 활성화 후 대사")]
    public Dialogue dialogue_0;
    [Header("이후 짤막 대사(없어도 됨)//제단: 활성화 전 대사")]
    public Dialogue dialogue_1;
    [Header("처음 질문")]
    public Select select_0;
    [Header("제단: 성공시 대사")]
    public Dialogue dialogue_2;
    [Header("제단: 모두 성공시 대사")]
    public Dialogue dialogue_3;
    [Header("제단: 문 열릴시 대사")]
    public Dialogue dialogue_4;

    [Space()]
    public SpriteRenderer dog;
    public SpriteRenderer hamster;
    public GameObject paper71;
    [Space()]
    //public GameObject paper73;
    [Header("제단 오브젝트-------")]
    public SpriteRenderer tribute;
    public GameObject lockedAltar;
    public GameObject unlockedAltar;
    public GameObject redLight;
    public Trig32 trig88;
    public Trig32 trig89;
    public GameObject fenceClosed;
    public GameObject fenceOpened;
    [Space()]


    public SpriteRenderer letter73;
    //public GameObject centerView;
    public Trig45 trig50;

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

    private int bifur;
    [Space()]
    public GameObject cameraPoint0;
    public GameObject cameraPoint1;
    public GameObject cameraPoint2;
    public GameObject cameraPoint3;
    public Transform rubyMovePoint0;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    private IEnumerator eventCo;

    /////////////////////////////////////////////////////////////////////   inspector에서 체크 가능. 1: 닿으면 자동 실행, 2: 체크시 해당 트리거 무한 반복.

    [Header("실행시 바라보는 방향")] public string turn;
    [Header("트리거 진입 시 자동 실행")] public bool autoEnable;
    [Header("여러번 실행 가능")] public bool preserveTrigger;
    [Header("게임 중 딱 한번만 실행(영원히)")] public bool onlyOnce = true;
    [Header("특정 분기만(부터) 반복")] public int repeatBifur;
    public bool clickable = false;          // 클릭시 실행.

    void Start()                                                            //Don't Touch
    {
        this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        //instance = this;
        eventCo = EventCoroutine();
        theDM = DialogueManager.instance;
        theSelect = SelectManager.instance;
        theOrder = OrderManager.instance;
        thePlayer = PlayerManager.instance;
        theDB = DatabaseManager.instance;
        theCamera = CameraMovement.instance;
        theMap = MapManager.instance;
        thePuzzle = PuzzleManager.instance;
        if (trigNum == 86 || trigNum == 87 || trigNum == 88)
        {
            if (theDB.trigOverList.Contains(trigNum))
            {
                redLight.SetActive(true);
                tribute.gameObject.SetActive(true);
            }
            else
            {
                tribute.gameObject.SetActive(false);
            }

            if (theDB.trigOverList.Contains(89))
            {
                unlockedAltar.SetActive(true);
                lockedAltar.SetActive(false);
                clickable = true;
            }
            else
            {
                unlockedAltar.SetActive(false);
                lockedAltar.SetActive(true);
                clickable = false;
            }
        }
        if (theDB.trigOverList.Contains(trigNum))
        {//트리거 실행 후 맵에 다시 돌아왔을 때 DB list에 들어가 있으면 다시 실행 안됨.
            flag = true;
            clickable = false;
            if (repeatBifur != 0)
            {
                flag = false;
                bifur = repeatBifur;
            }
        }
        if (trigNum == 71)
        {
            if (theDB.trigOverList.Contains(71))
            {
                paper71.SetActive(false);
            }
        }
        else if (trigNum == 75)
        {
            if (theDB.trigOverList.Contains(11))
            {
                bifur = 1;
            }
            if (theDB.trigOverList.Contains(44))
            {
                flag = true;
            }
        }
        else if (trigNum == 89)
        {
            if (theDB.trigOverList.Contains(86) && theDB.trigOverList.Contains(87) && theDB.trigOverList.Contains(88))
            {
                flag = true;
                fenceClosed.SetActive(false);
                fenceOpened.SetActive(true);
            }
        }
        else if (trigNum == 80)
        {
            if (theDB.trigOverList.Contains(11))
            {
                flag = true;
            }
        }

    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (!thePlayer.exc.GetBool("on") && !flag && !autoEnable && (thePlayer.canInteractWith == 0 || thePlayer.canInteractWith == trigNum))
        {
            thePlayer.exc.SetBool("on", true);
            thePlayer.canInteractWith = trigNum;
        }


        //그 넘버만 실행함.
        if (!flag && !autoEnable && (Input.GetKeyDown(KeyCode.Space) || thePlayer.getSpace) && !theDM.talking && thePlayer.canInteractWith == trigNum)
        {
            flag = true;
            thePlayer.exc.SetBool("on", false);
            StartCoroutine(EventCoroutine());
        }

        if (!flag && autoEnable && !theDM.talking)
        {
            flag = true;
            StartCoroutine(EventCoroutine());
        }

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        thePlayer.exc.SetBool("on", false);
        thePlayer.canInteractWith = 0;
    }

    public void OnMouseDown()
    {
        if (clickable&&theDB.OnActivated[0])
        {
            switch (trigNum)
            {
                case 86:
                    thePlayer.exc.SetBool("on", false);
                    thePlayer.canInteractWith = -1;
                    UseItemToAltar(21);
                    break;
                case 87:
                    thePlayer.exc.SetBool("on", false);
                    thePlayer.canInteractWith = -1;
                    UseItemToAltar(20);
                    break;
                case 88:
                    thePlayer.exc.SetBool("on", false);
                    thePlayer.canInteractWith = -1;
                    UseItemToAltar(19);
                    break;

            }
        }
    }

    IEnumerator EventCoroutine()
    {

        theOrder.NotMove();
        theOrder.PreLoadCharacter();
        if (turn != "")
            theOrder.Turn("Player", turn);

        switch (trigNum)
        {
            case 30:
                theDB.progress = 4;
                BookManager.instance.ActivateUpdateIcon(2);

                // //카메라무빙
                // theCamera.isManual = true;

                // //기존 무빙 : 이동
                // // theCamera.moveSpeed = 1000f;
                // // theCamera.target = cameraPoint0;
                // // yield return new WaitForSeconds(0.01f);
                // // theCamera.moveSpeed = 1f;
                // // theCamera.target = cameraPoint1;
                // // yield return new WaitForSeconds(3f);
                // // //yield return new WaitUntil(()=> CameraMovement.instance.target.transform.position == cameraPoint1.transform.position);
                // // StartCoroutine(RubyWalk());
                // // theCamera.target = cameraPoint2;
                // // yield return new WaitForSeconds(3f);

                // //플랜비 : 줌아웃>줌인
                // theCamera.SetZoom(7f);
                // theCamera.moveSpeed = 1000f;
                // theCamera.target = cameraPoint3;
                // //yield return new WaitForSeconds(0.01f);
                // yield return new WaitForSeconds(3f);
                // theCamera.moveSpeed = 1f;
                // theCamera.AdjustZoom(5.5f,0.05f);
                // StartCoroutine(RubyWalk());
                // theCamera.target = cameraPoint2;
                // //CameraMovement.instance.target = thePlayer.gameObject;
                // yield return new WaitForSeconds(3f);
                break;
            case 32:
                dog.flipX =
                (this.transform.position.x >= thePlayer.transform.position.x) ? true : false;
                break;
            case 33:
                hamster.flipX =
                (this.transform.position.x >= thePlayer.transform.position.x) ? false : true;
                break;
            case 34:
                AudioManager.instance.Play("bowl0");
                break;
            case 61:
                AudioManager.instance.Play("mattouch");
                break;
            case 62:
                AudioManager.instance.Play("stonetouch");
                break;
            case 63:
                AudioManager.instance.Play("treetouch");
                break;
            case 68:
                theDB.progress = 6;
                BookManager.instance.ActivateUpdateIcon(2);
                break;
            case 69:
                theDB.progress = 7;
                BookManager.instance.ActivateUpdateIcon(2);
                break;
            case 70:
                theDB.progress = 8;
                BookManager.instance.ActivateUpdateIcon(2);
                CameraMovement.instance.isManual = true;


                /////////////////////////////////////1 번////////////
                CameraMovement.instance.SetZoom(12f);
                CameraMovement.instance.moveSpeed = 1000f;
                CameraMovement.instance.target = cameraPoint0;
                yield return new WaitForSeconds(0.01f);
                CameraMovement.instance.moveSpeed = 1f;
                CameraMovement.instance.target = cameraPoint1;
                yield return new WaitForSeconds(4f);
                CameraMovement.instance.AdjustZoom(9f, 0.03f);
                //CameraMovement.instance.target = thePlayer.gameObject;
                CameraMovement.instance.target = cameraPoint2;
                yield return new WaitForSeconds(2f);
                StartCoroutine(RubyWalk());
                //CameraMovement.instance.target = thePlayer.gameObject;
                yield return new WaitForSeconds(2f);
                //yield return new WaitUntil(()=> Mathf.Abs( theCamera.gameObject.transform.position.x - thePlayer.gameObject.transform.position.x)<1f);
                //yield return new WaitUntil(() => !CameraMovement.instance.isZooming);

                ///////////////////////////////////////////////////////
                break;
            case 82:
                theDB.progress = 5;
                BookManager.instance.ActivateUpdateIcon(2);
                break;
            //     case 100 :
            //         AudioManager.instance.Play("boosruck");
            //         Inventory.instance.GetItem(13);
            //         thePlayer.animator.SetTrigger("grab");
            //             BookManager.instance.ActivateUpdateIcon(1);
            // theOrder.Move(); 
            //             gameObject.SetActive(false);
            //         break;
            case 101:
                AudioManager.instance.Play("boosruck");
                Inventory.instance.GetItem(14);
                thePlayer.animator.SetTrigger("grab");
                BookManager.instance.ActivateUpdateIcon(1);
                theOrder.Move();
                gameObject.SetActive(false);
                break;

        }



        switch (trigNum)
        {
            case 30:

                // CameraMovement.instance.moveSpeed = 1f;
                if(theDB.trigOverList.Contains(44)){

                    theDM.ShowDialogue(dialogue_1);
                    yield return new WaitUntil(()=> !theDM.talking);
                }
                else{
                    
                    theDM.ShowDialogue(dialogue_0);
                    yield return new WaitUntil(()=> !theDM.talking);
                }   
                // theDM.ShowDialogue(dialogue_0);
                // yield return new WaitUntil(() => !theDM.talking);
                // CameraMovement.instance.moveSpeed = 5f;
                // CameraMovement.instance.isManual = false;
                // CameraMovement.instance.target = thePlayer.gameObject;
                break;
            case 70:

                //CameraMovement.instance.moveSpeed = 1f;
                theDM.ShowDialogue(dialogue_0);
                yield return new WaitUntil(() => !theDM.talking);
                CameraMovement.instance.moveSpeed = 5f;
                CameraMovement.instance.target = thePlayer.gameObject;
                CameraMovement.instance.isManual = false;
                break;
            case 71:



                theSelect.ShowSelect(select_0);
                yield return new WaitUntil(() => !theSelect.selecting);

                if (theSelect.GetResult() == 0)
                {


                    AudioManager.instance.Play("pageflip2");
                    thePlayer.animator.SetTrigger("grab");
                    theDB.activatedPaper = 5;
                    BookManager.instance.ActivateUpdateIcon(0);
                    ObjectManager.instance.FadeOut(paper71.GetComponent<SpriteRenderer>());
                    theDB.trigOverList.Add(71);
                    theDB.doorEnabledList.Add(27);
                    yield return new WaitForSeconds(1.5f);
                    AudioManager.instance.Play("getitem0");
                    theDM.ShowDialogue(dialogue_0);
                    yield return new WaitUntil(() => !theDM.talking);
                    preserveTrigger = false;
                }
                break;

            case 72:
                //if(UnknownScript.instance) StartCoroutine(UnknownScript.instance.DestroyUnknown());
                if (bifur == 0)
                {
                    theDM.ShowDialogue(dialogue_0);
                    yield return new WaitUntil(() => !theDM.talking);
                }

                break;

            case 73:

                AudioManager.instance.Play("pageflip2");
                thePlayer.animator.SetTrigger("grab");
                PaperManager.instance.letter0.gameObject.SetActive(true);
                theDB.letterPaper[0] = true;
                PaperManager.instance.letterBtnUpdate = true;//.SetBool("flash",true);
                BookManager.instance.ActivateUpdateIcon(0);
                letter73.gameObject.SetActive(false);
                yield return new WaitForSeconds(1.5f);
                theDM.ShowDialogue(dialogue_0);
                yield return new WaitUntil(() => !theDM.talking);
                //ObjectManager.instance.FadeOut(letter73);
                break;

            case 74:

                break;

            case 75:
                if (!theDB.trigOverList.Contains(43))
                {
                    if (FadeManager.instance.red.activeSelf)
                    {
                        theDM.ShowDialogue(dialogue_0);
                        yield return new WaitUntil(() => !theDM.talking);
                    }
                    else
                    {
                        theDM.ShowDialogue(dialogue_1);
                        yield return new WaitUntil(() => !theDM.talking);
                    }
                }
                break;

            case 86:
                if (clickable)
                {
                    theDM.ShowDialogue(dialogue_0);
                    yield return new WaitUntil(() => !theDM.talking);
                }
                else
                {
                    theDM.ShowDialogue(dialogue_1);
                    yield return new WaitUntil(() => !theDM.talking);
                }
                break;

            case 87:
                if (clickable)
                {
                    theDM.ShowDialogue(dialogue_0);
                    yield return new WaitUntil(() => !theDM.talking);
                }
                else
                {
                    theDM.ShowDialogue(dialogue_1);
                    yield return new WaitUntil(() => !theDM.talking);
                }
                break;

            case 88:
                if (clickable)
                {
                    theDM.ShowDialogue(dialogue_0);
                    yield return new WaitUntil(() => !theDM.talking);
                }
                else
                {
                    theDM.ShowDialogue(dialogue_1);
                    yield return new WaitUntil(() => !theDM.talking);
                }
                break;
            case 89:
                if (fenceClosed.activeSelf)
                {
                    if (bifur == 0)
                    {

                        theDM.ShowDialogue(dialogue_0);
                        yield return new WaitUntil(() => !theDM.talking);

                        yield return new WaitForSeconds(1f);
                        theOrder.Turn("Player", "UP");
                        trig88.unlockedAltar.SetActive(true);
                        trig88.lockedAltar.SetActive(false);
                        theDM.ShowDialogue(dialogue_2);
                        yield return new WaitUntil(() => !theDM.talking);
                        //if(trig88.lockedAltar.activeSelf){
                        trig88.clickable = true;
                        //}

                    }
                    else
                    {
                        theDM.ShowDialogue(dialogue_1);
                        yield return new WaitUntil(() => !theDM.talking);

                    }
                }
                break;

            case 91:
                if (theDB.trigOverList.Contains(86) && theDB.trigOverList.Contains(87) && theDB.trigOverList.Contains(88))
                {
                    fenceClosed.SetActive(false);
                    fenceOpened.SetActive(true);
                }
                break;

            case 92:
                BGMManager.instance.FadeOutMusic();
                break;
            case 93:
                StartCoroutine(RubyWalk("UP"));
                yield return new WaitForSeconds(4f);
                trig50.gameObject.SetActive(true);
                trig50.OuterAccess();
                break;
            default:
                if (bifur == 0)
                {
                    theDM.ShowDialogue(dialogue_0);
                    yield return new WaitUntil(() => !theDM.talking);
                }
                else
                {
                    theDM.ShowDialogue(dialogue_1);
                    yield return new WaitUntil(() => !theDM.talking);
                }
                break;
        }



        theOrder.Move();


        if (onlyOnce)
            if (!theDB.trigOverList.Contains(trigNum)) theDB.trigOverList.Add(trigNum);
        if (preserveTrigger)
            flag = false;
        if (repeatBifur != 0)
        {
            if (!theDB.trigOverList.Contains(trigNum)) theDB.trigOverList.Add(trigNum);
            flag = false;
            bifur = repeatBifur;
        }
    }

    //IEnumerator OpenBook(){

    //}
    IEnumerator RubyWalk(string dir = "LEFT")
    {

        while (thePlayer.gameObject.transform.position != rubyMovePoint0.position)
        {
            switch (dir)
            {
                case "LEFT":
                    thePlayer.animator.SetFloat("Horizontal", -1f);
                    break;
                case "UP":
                    thePlayer.animator.SetFloat("Vertical", 1f);
                    break;
            }
            thePlayer.animator.SetFloat("Speed", 1f);
            thePlayer.gameObject.transform.position = Vector3.MoveTowards(thePlayer.gameObject.transform.position,
            rubyMovePoint0.position, 3f * Time.deltaTime);
            yield return null;
        }
        thePlayer.animator.SetFloat("Speed", 0f);
    }
    IEnumerator WrongTribute()
    {

        theOrder.NotMove();
        DialogueManager.instance.ShowDialogue(Inventory.instance.wrongTribute);
        yield return new WaitUntil(() => !DialogueManager.instance.talking);
        theDB.gameOverList.Add(28);
        thePlayer.canInteractWith = 0;
        theOrder.Move();
    }
    public void UseItemToAltar(int itemNum)
    {

        if (theDB.OnActivated[itemNum])
        {
            theDB.OnActivated[itemNum] = false;
            Inventory.instance.RemoveItem(itemNum);
            clickable = false;
            preserveTrigger = false;
            //StopCoroutine(eventCo);
            flag = true;
            theDB.trigOverList.Add(trigNum);
            CursorManager.instance.RecoverCursor();
            StartCoroutine(ActivateAltar());
        }
        else if (theDB.OnActivated[0])
        {
            theDB.DeactivateItem();
            CursorManager.instance.RecoverCursor();
            StartCoroutine(WrongTribute());
        }
    }
    IEnumerator ActivateAltar()
    {
        theOrder.NotMove();
        AudioManager.instance.Play("key0");
        ObjectManager.instance.FadeIn(tribute);
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.Play("getitem0");
        ObjectManager.instance.FadeIn(redLight.GetComponent<SpriteRenderer>());
        yield return new WaitForSeconds(1.5f);
        lockedAltar.SetActive(false);
        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(() => !theDM.talking);

        if (theDB.trigOverList.Contains(86) && theDB.trigOverList.Contains(87) && theDB.trigOverList.Contains(88))
        {
            //#if ADD_ACH
            if (!theDB.gameOverList.Contains(28))
            {
                Debug.Log("업적15");
                if (SteamAchievement.instance != null) SteamAchievement.instance.ApplyAchievements(15);
            }
            //#endif
            if (trigNum == 88) trig89.flag = true;
            thePlayer.canInteractWith = 0;
            DialogueManager.instance.ShowDialogue(Inventory.instance.checkAllTributes);
            yield return new WaitUntil(() => !DialogueManager.instance.talking);
        }

        thePlayer.canInteractWith = 0;
        if (trigNum == 88)
        {
            StartCoroutine(ActivateMazeDoor());
        }
        else
        {

            theOrder.Move();
        }
    }
    IEnumerator ActivateMazeDoor()
    {
        if (theDB.trigOverList.Contains(86) && theDB.trigOverList.Contains(87))
        {
            trig89.flag = true;
            yield return new WaitForSeconds(0.5f);
            theOrder.Turn("Player", "LEFT");
            AudioManager.instance.Play("door2");
            //fenceClosed.SetActive(false);
            fenceOpened.SetActive(true);
            fenceClosed.SetActive(false);
            //ObjectManager.instance.FadeIn(fenceOpened.GetComponent<SpriteRenderer>());
            //ObjectManager.instance.FadeIn(fenceClosed.GetComponent<SpriteRenderer>());

            //yield return new WaitForSeconds(1f);
            theDM.ShowDialogue(dialogue_4);
            yield return new WaitUntil(() => !theDM.talking);
        }

        theOrder.Move();
    }










}

