#if UNITY_ANDROID || UNITY_IOS
#define DISABLEKEYBOARD
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
[RequireComponent(typeof(BoxCollider2D))]
public class Trig74 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    //public static Trig27 instance;
    public int trigNum;
    [Header ("다리 앞 대사")]
    public Dialogue dialogue_0;
    [Header ("다리 통과 후 대사")]
    public Dialogue dialogue_1;
    [Header ("눈감고 대사")]
    public Dialogue dialogue_2;
    
    public bool moveFlag ;
    //public bool animFlag;
    
    public GameObject fish;
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
    
    private int bifur;
    public bool[] rubyMove = new bool[3];
    public Transform[] moveLocation_Ruby = new Transform[3];
    
    /////////////////////////////////////////////////////////////////////   inspector에서 체크 가능. 1: 닿으면 자동 실행, 2: 체크시 해당 트리거 무한 반복.
    
    [Header ("실행시 바라보는 방향")]public string turn;
    [Header ("트리거 진입 시 자동 실행")]public bool autoEnable;
    [Header ("여러번 실행 가능")]public bool preserveTrigger;
    [Header ("게임 중 딱 한번만 실행(영원히)")]public bool onlyOnce= true;
    [Header ("특정 분기만(부터) 반복")]public int repeatBifur;

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
        if(theDB.trigOverList.Contains(trigNum)){//트리거 실행 후 맵에 다시 돌아왔을 때 DB list에 들어가 있으면 다시 실행 안됨.
            flag = true;
            if(repeatBifur!=0){
                flag = false;
                bifur = repeatBifur;
            }
        }
        // if(theDB.trigOverList.Contains(57)){
        //     fish.SetActive(true);
        // }
    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    private void OnTriggerStay2D(Collider2D collision){
        //if(theDB.trigOverList.Contains(24)&&!GameManager.instance.playing&&!theDB.gameOverList.Contains(6)){
        //if(theDB.trigOverList.Contains(3)){
        //if(theDB.trigOverList.Contains(57)){

            if(!thePlayer.exc.GetBool("on")&&!flag&&!autoEnable){
                thePlayer.exc.SetBool("on",true);
                thePlayer.canInteractWith = trigNum;
            }
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
        //}
        //}


    }
    void OnTriggerExit2D(Collider2D collision){
        thePlayer.exc.SetBool("on",false);
        thePlayer.canInteractWith = 0;
    }


    IEnumerator EventCoroutine(){

        theOrder.NotMove();  
        theOrder.PreLoadCharacter();  
        if(turn!="null")      
            theOrder.Turn("Player",turn);
                                               //트리거 중 이동불가


        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작
//대사 후 책 열기 애니메이션
        theDM.ShowDialogue(dialogue_0);
        yield return new WaitUntil(()=> !theDM.talking);
        thePlayer.animator.SetBool("openbook", true);
        yield return new WaitForSeconds(3.2f);
        AudioManager.instance.Play("pageflip");
        yield return new WaitForSeconds(0.3f);
        //StartCoroutine("OpenBook");


        #if UNITY_EDITOR
        
            BookManager.instance.firstOpen = true;
            theDB.firstOpen = true;
            // PaperManager.instance.letter0.gameObject.SetActive(true);
            // PaperManager.instance.letter1.gameObject.SetActive(true);
            theDB.activatedPaper=6;
        #endif

//페이드아웃 했다가 밝아지면서 책 펼쳐지고 책아이콘 삭제.
        // thePlayer.joyBundle.SetActive(false);
        // thePlayer.joyStick.SetActive(false);

        Fade2Manager.instance.FadeOut(0.02f);
        yield return new WaitForSeconds(2f);
        thePlayer.isPlayingGame = true;
        Fade2Manager.instance.FadeIn(0.02f);
        BookManager.instance.BookOn(false);
        BookManager.instance.OnPaper();
        for(int i=0;i<BookManager.instance.btns.Length;i++){
            BookManager.instance.btns[i].SetActive(false);
        }
        BookManager.instance.OnPaper();
        theDB.bookActivated = false;
        BookManager.instance.onButton.SetActive(false);
        thePlayer.notMove = true;
        PaperManager.instance.blurAnim = false;
#if DEV_MODE
        InGameVideo.instance.skipBtn.SetActive(true);
#endif
        //연출부
        for(int j=0; j<3; j++){
            yield return new WaitForSeconds(1f);
            for(int i=0; i<2; i++){
        AudioManager.instance.Play("air0");
                ObjectManager.instance.ImageFadeOut(PaperManager.instance.paperBundle[2*j+i].GetComponent<Image>(),0.01f);
                ObjectManager.instance.ImageFadeIn(PaperManager.instance.paperUnlocked[2*j+i].GetComponent<Image>(),0.01f);
                ObjectManager.instance.TextFadeIn(PaperManager.instance.textUnlocked[2*j+i],0.01f);
                
                if(thePlayer.devMode){

                    yield return new WaitForSeconds(0.1f);//6
                }
                else{
                    
                    yield return new WaitForSeconds(6f);//6
                }

                if(i==1&&j!=2){
                    if(thePlayer.devMode){
                        yield return new WaitForSeconds(0.1f);
                    }
                    else{
                        yield return new WaitForSeconds(3f);//3
                    }
                    PaperManager.instance.waitStory = true;
                    BookManager.instance.nextBtn.SetActive(true);
                    yield return new WaitUntil(()=> !PaperManager.instance.waitStory);
                    PaperManager.instance.ShowRightPage(true);

                    PaperManager.instance.paperUnlocked[2*j+i-1].SetActive(false);
                    PaperManager.instance.paperUnlocked[2*j+i].SetActive(false);
                    PaperManager.instance.textUnlocked[2*j+i-1].gameObject.SetActive(false);
                    PaperManager.instance.textUnlocked[2*j+i].gameObject.SetActive(false);
                }
            }
            // if(j<2) {
            //     PaperManager.instance.waitStory = true;
            //     BookManager.instance.nextBtn.SetActive(true);
            //     yield return new WaitUntil(()=> !PaperManager.instance.waitStory);
            //     PaperManager.instance.ShowRightPage(true);
            // }
        }
        PaperManager.instance.waitStory = true;
        BookManager.instance.nextBtn.SetActive(true);
        yield return new WaitUntil(()=> !PaperManager.instance.waitStory);



#if DEV_MODE
        InGameVideo.instance.skipBtn.SetActive(false);
#endif





#if !DISABLEKEYBOARD

#region (영상) 진엔딩 언노운

        if(PaperManager.instance.letter0.activeSelf&&PaperManager.instance.letter1.activeSelf){
            BGMManager.instance.FadeOutMusic();
            Fade2Manager.instance.FadeOut(0.02f);
            yield return new WaitForSeconds(2f);
            BookManager.instance.BookOff();
            thePlayer.notMove = true;

            thePlayer.animator.SetBool("openbook", false);
            yield return new WaitForSeconds(3f);
            if(GameMultiLang.instance.nowLang=="kr"){

                InGameVideo.instance.StartVideo("TrueEnding");
            }
            else if(GameMultiLang.instance.nowLang=="en"){

                InGameVideo.instance.StartVideo("TrueEnding_EN");
            }

            yield return new WaitUntil(()=> InGameVideo.instance.theVideo.isPlaying);
            Fade2Manager.instance.FadeIn(0.01f);

            yield return new WaitUntil(()=> !InGameVideo.instance.isPlaying);


            FadeManager.instance.FadeOut(0.01f);

            Debug.Log("업적6 : 안녕, 안녕(진엔딩)");
            
            if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(6);


            yield return new WaitForSeconds(4f);
            InGameVideo.instance.ExitVideo();
            thePlayer.finishGame = true;
        }
#endregion
        
//#region (인게임 애니메이션) 노멀엔딩
        //yield return new WaitForSeconds(2f);
        else
#endif  //진엔딩 모바일 끄기.
        {
    //페이드아웃 했다가 밝아지면서 책 닫고 책 닫는 애니메이션.
            Fade2Manager.instance.FadeOut(0.02f);
            yield return new WaitForSeconds(2f);
            BookManager.instance.BookOff();
            thePlayer.notMove = true;
            //yield return new WaitForSeconds(0.2f);
            thePlayer.animator.SetBool("openbook", false);
            yield return new WaitForSeconds(1f);
            Fade2Manager.instance.FadeIn(0.02f);
            yield return new WaitForSeconds(4f);

    //눈 감는 애니메이션 후 다리 중간까지 이동.
            //thePlayer.animator.SetTrigger("close_eye");
            //yield return new WaitForSeconds(2.3f);
            thePlayer.animator.SetBool("close_eye_bool", true);
            yield return new WaitForSeconds(1f);

            theDM.ShowDialogue(dialogue_2);
            yield return new WaitUntil(()=> !theDM.talking);
            thePlayer.animator.SetBool("close_eye_bool", false);
            yield return new WaitForSeconds(0.583f);
            rubyMove[0] = true;
            yield return new WaitUntil(()=> !rubyMove[0]);

            rubyMove[1] = true;
            yield return new WaitUntil(()=> !rubyMove[1]);

            //Fade2Manager.instance.FlashIn(0.02f);

    //화면 밝아지면서 대사 진행.
            rubyMove[2] = true;
            FadeManager.instance.FlashOut(0.02f);
            
            yield return new WaitForSeconds(2f);
            rubyMove[2] = false;
            thePlayer.animator.SetFloat("Speed", 0f);
            theDM.ShowDialogue(dialogue_1);
            yield return new WaitUntil(()=> !theDM.talking);

    //화면 어두워지면서 나가리.
            FadeManager.instance.FadeOut(0.01f);
            Debug.Log("업적8 : 숲의 끝(노멀엔딩)");
            
            if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(8);
            yield return new WaitForSeconds(4f);
            thePlayer.finishGame = true;

            
        }
//#endregion

#region (업적) 시간

        DebugManager.instance.LoadPlayerInfo("clear");
        
        if(theDB.gameTimer<=3600f){
            Debug.Log("업적3 : 빨라도 너무 빨라");
            
            if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(3);
        } 
#endregion

// #region (영상) 진엔딩 헨리

//         if(PaperManager.instance.letter0.activeSelf&&PaperManager.instance.letter1.activeSelf){
            
//             yield return new WaitForSeconds(1f);
//             InGameVideo.instance.StartVideo("Henry");
//             yield return new WaitUntil(()=> InGameVideo.instance.theVideo.isPlaying);
//             FadeManager.instance.FadeIn(0.01f);

//             yield return new WaitUntil(()=> !InGameVideo.instance.isPlaying);
//             FadeManager.instance.FadeOut(0.01f);
//             yield return new WaitForSeconds(4f);
//             InGameVideo.instance.ExitVideo();
//         }
// #endregion

#if !DISABLEKEYBOARD
#region (영상) 크레딧


        if(theDB.caughtCount==0){
            Debug.Log("업적2 : 발 빠른 강아지");
            if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(2);
        }

        yield return new WaitForSeconds(1f);
        if(PaperManager.instance.letter0.activeSelf&&PaperManager.instance.letter1.activeSelf){
            InGameVideo.instance.StartVideo("Credit_TRUE");
        }
        else{
            InGameVideo.instance.StartVideo("Credit");
        }
        yield return new WaitUntil(()=> InGameVideo.instance.theVideo.isPlaying);
        FadeManager.instance.FadeIn(0.01f);

        yield return new WaitUntil(()=> !InGameVideo.instance.isPlaying);
        FadeManager.instance.FadeOut(0.01f);
            yield return new WaitForSeconds(4f);
            InGameVideo.instance.ExitVideo();
#endregion
#endif  //모바일 크레딧 끄기

#region (영상) 로고

        //FadeManager.instance.FadeIn(0.1f);
            yield return new WaitForSeconds(1f);
        InGameVideo.instance.StartVideo("LastLogo");
        yield return new WaitUntil(()=> InGameVideo.instance.theVideo.isPlaying);
        FadeManager.instance.FadeIn(0.01f);
        // InGameVideo.instance.theVideo.Pause();
        //     yield return new WaitForSeconds(1f);
        // InGameVideo.instance.theVideo.Play();

        yield return new WaitUntil(()=> !InGameVideo.instance.isPlaying);
        FadeManager.instance.FadeOut(0.01f);
            yield return new WaitForSeconds(4f);
            InGameVideo.instance.ExitVideo();

#endregion



//메인메뉴로 돌아감

            yield return new WaitForSeconds(1f);
            // yield return new WaitUntil(()=> !BGMManager.instance.source.isPlaying);

        LoadingTrig.instance.GoMainMenu();
        // SceneManager.LoadScene("MainMenu");
        // Destroy(theDB.gameObject);
        // for(int i=0;i<LoadingTrig.instance.destroyObject.Length;i++){
        //     Destroy(LoadingTrig.instance.destroyObject[i]);
        // }
//

        // theOrder.Move();

        // if(onlyOnce)
        //     theDB.trigOverList.Add(trigNum);
        // if(preserveTrigger)
        //     flag=false;
        // if(repeatBifur!=0){
        //     theDB.trigOverList.Add(trigNum);
        //     flag=false;
        //     bifur=repeatBifur;
        // }
    }

    void FixedUpdate(){

        if(rubyMove[0]){
            if(thePlayer.gameObject.transform.position!=moveLocation_Ruby[0].position){   
                //Debug.Log("이동");
                thePlayer.animator.SetFloat("Horizontal", -1f);
                thePlayer.animator.SetFloat("Speed", 1f);
                thePlayer.gameObject.transform.position = Vector3.MoveTowards(thePlayer.gameObject.transform.position,
                moveLocation_Ruby[0].position, 2f* Time.deltaTime); 
            }
            else{
                rubyMove[0] = false;
                //thePlayer.animator.SetFloat("Horizontal", 0f);
                //thePlayer.animator.SetFloat("Speed", 0f);
            
            
            }
        }
        else if(rubyMove[1]){
            if(thePlayer.gameObject.transform.position!=moveLocation_Ruby[1].position){   
                //Debug.Log("이동");
                thePlayer.animator.SetFloat("Horizontal", -1f);
                thePlayer.animator.SetFloat("Speed", 1f);
                thePlayer.gameObject.transform.position = Vector3.MoveTowards(thePlayer.gameObject.transform.position,
                moveLocation_Ruby[1].position, 2f* Time.deltaTime); 
            }
            else{
                rubyMove[1] = false;
                //thePlayer.animator.SetFloat("Horizontal", 0f);
                //thePlayer.animator.SetFloat("Speed", 0f);
            
            
            }
        }
        else if(rubyMove[2]){
            if(thePlayer.gameObject.transform.position!=moveLocation_Ruby[2].position){   
                //Debug.Log("이동");
                thePlayer.animator.SetFloat("Horizontal", -1f);
                thePlayer.animator.SetFloat("Speed", 1f);
                thePlayer.gameObject.transform.position = Vector3.MoveTowards(thePlayer.gameObject.transform.position,
                moveLocation_Ruby[2].position, 2f* Time.deltaTime); 
            }
            else{
                rubyMove[2] = false;
                //thePlayer.animator.SetFloat("Horizontal", 0f);
                //thePlayer.animator.SetFloat("Speed", 0f);
            
            
            }
        }
    }
}

