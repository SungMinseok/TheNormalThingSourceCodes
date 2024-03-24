using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingTrig : MonoBehaviour
{
    //public GameObject newDB;
    public GameObject[] destroyObject;
    public static LoadingTrig instance;
    public Dialogue forLoadDialogue;
    public GameObject demoHomeBtn;
    
    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else{
            Destroy(this.gameObject);
        }
    }
    public GameObject gameOverWindow;
    public GameObject loadWindow;
    public GameObject demoPop;
    public SpriteRenderer loadIcon; 
    SaveNLoad theSL;
    PlayerManager thePlayer;
    DatabaseManager theDB;
    //FadeManager theFade;
    //private OrderManager theOrder;
    void Start(){

        //instance = this;
        theSL = FindObjectOfType<SaveNLoad>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theDB = FindObjectOfType<DatabaseManager>();
        //StopAllCoroutines();
            //theSL.CallLoad(theDB.phaseNum);
        if(!DebugManager.instance.onDebug){
            StartCoroutine(Loading());
        }
    }
    void FixedUpdate(){
        if(loadWindow.activeSelf){
            thePlayer.notMove=true;
        }
    }
    IEnumerator Loading(){
        yield return null;
        // yield return new WaitForSeconds(0.01f);
        // ObjectManager.instance.FadeIn(loadIcon);
        // Fade2Manager.instance.FadeIn(0.02f);     
        
        //yield return new WaitForSeconds(0.5f);   //오브젝트 불러오는 시간 필요 (개중요)


        // DialogueManager.instance.ShowDialogue(forLoadDialogue, true);
        // yield return new WaitForSeconds(1f);
        // DialogueManager.instance.ExitDialogue();


        thePlayer.notMove=true;
        if(theDB.phaseNum != 0){
            theSL.CallLoad(theDB.phaseNum);
        }
        // yield return new WaitForSeconds(3f);     // 로딩창 켜져있는 시간 조절
        
        

                //Fade2Manager.instance.FadeOut(0.02f,1f);
        
        // ObjectManager.instance.FadeOut(loadIcon);
        //yield return new WaitForSeconds(2f);
                //loadWindow.SetActive(false);

        thePlayer.LetBegin();
    }

    public void GameOver(){ //게임오버창 활성화
        thePlayer.isGameOver=true;
        StartCoroutine(GameOverCoroutine());
    }
    IEnumerator GameOverCoroutine(){
        
        thePlayer.notMove=true;       
        Fade2Manager.instance.FadeOut(0.2f,1f);    //검은창으로.

        yield return new WaitForSeconds(2f);
        gameOverWindow.SetActive(true);

        Fade2Manager.instance.FadeIn(0.02f);    //게임오버화면
    }
    public void GoMainMenu(){   //메인 메뉴로 돌아감
        AudioManager.instance.Play("button22");
        StartCoroutine(GoMainMenuCoroutine());
    }

    IEnumerator GoMainMenuCoroutine(){
        
        Fade2Manager.instance.FadeOut(0.02f,1f);
        yield return new WaitForSeconds(2f);
        AudioManager.instance.Stop("treecreak");
        SceneManager.LoadScene("MainMenu");
        Destroy(theDB.gameObject);
        //Destroy(FindObjectOfType<BGMManager>());
        for(int i=0;i<destroyObject.Length;i++){
            Destroy(destroyObject[i]);
        }
    }
    public void GoLastSaved(){  //마지막 저장지점으로 돌아감
        AudioManager.instance.Play("button22");
        StartCoroutine(GoLastSavedCoroutine());
    }

    IEnumerator GoLastSavedCoroutine(){

        Fade2Manager.instance.FadeOut(0.02f,1f);
        yield return new WaitForSeconds(2f);
        if(DatabaseManager.instance.phaseNum==0){   //새로시작으로 겜 시작시
            
            //Instantiate (newDB, new Vector3(0,0,0), Quaternion.identity);
            //yield return new WaitForSeconds(2f);
            
            theDB.ResetDB();
            thePlayer.ResetPlayer();
            thePlayer.isGameOver = false;
            SceneManager.LoadScene("Start");
            //Destroy(theDB.gameObject);
            for(int i=0;i<destroyObject.Length;i++){
                Destroy(destroyObject[i]);
            }
        }
        else{   //로드로 겜 시작시
            theDB.ResetDB();
            thePlayer.ResetPlayer();
            gameOverWindow.SetActive(false);
            thePlayer.isGameOver = false;
            SettingManager.instance.StartLoad(DatabaseManager.instance.lastSaveNum);
            
        } 
    }


    public IEnumerator PopupDemo(){
        thePlayer.notMove = true;
        thePlayer.isDemoOver = true;
                //BGMManager.instance.Play(15);
                AudioManager.instance.Stop("treecreak");
                thePlayer.animator.SetFloat("Speed", 0f);
        StartCoroutine(UnknownScript.instance.DestroyUnknown());
        FadeManager.instance.FadeOut();
        yield return new WaitForSeconds(2f);
        demoPop.SetActive(true);
        FadeManager.instance.FadeIn();
        //Time.timeScale = 0f;
        Invoke("ActivateDemoHomeBtn", 2.5f);
        //if(DebugManager.instance.isDemo==trigNum) LoadingTrig.instance.PopupDemo();

        yield return new WaitUntil(()=>!BGMManager.instance.source.isPlaying);
        DemoHomeBtn();
    }
    void ActivateDemoHomeBtn(){
        demoHomeBtn.SetActive(true);
    }
    public void DemoHomeBtn(){
        
        //Time.timeScale = 1f;
        GoMainMenu();
    }
}
