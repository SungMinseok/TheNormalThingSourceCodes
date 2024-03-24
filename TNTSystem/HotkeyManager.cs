using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class HotkeyManager : MonoBehaviour
{
    public static HotkeyManager instance;
    private BookManager theBook;
    private SaveNLoad theSL;
    private DatabaseManager theDB;
    private PlayerManager thePlayer;
    public GameObject manual;
    public GameObject makerInfo;
    public GameObject redAlert;
    public GameObject devMode;
    public GameObject isDevMode;
    public GameObject BGM;
    public Text playCount;
    public Text clearCount;
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

        if(BGMManager.instance == null){
            BGM.SetActive(true);
        }
    }
    
    void Start()
    {
        
        theDB=DatabaseManager.instance;
        theBook= BookManager.instance;
        thePlayer = PlayerManager.instance;
        theSL=FindObjectOfType<SaveNLoad>();
#if DEV_MODE
        isDevMode.SetActive(true);
        isDevMode.GetComponent<Text>().text += " VER." + Application.version + " " + DebugManager.instance.update;
        playCount.text += DebugManager.instance.playerInfo.count_play;
        clearCount.text += DebugManager.instance.playerInfo.count_clear;


#endif
    }

    // Update is called once per frame
    void Update()
    {
#if DEV_MODE
        if(Input.GetKeyDown(KeyCode.F2)){
        AudioManager.instance.Play("button22");
            devMode.SetActive(!devMode.activeSelf);
        }
#endif
        
        if(theDB.bookActivated&&!(thePlayer.notMove&&!theBook.book.activeSelf&&!(thePlayer.isPlayingGame||thePlayer.isPlayingPuzzle))&&!thePlayer.isChased&&!redAlert.activeSelf){
        //if(!((thePlayer.notMove&&!thePlayer.isPlayingGame&&!thePlayer.isPlayingPuzzle)||thePlayer.isChased||redAlert.activeSelf)){
        //if(theDB.bookActivated&&!thePlayer.isWakingup&&!thePlayer.isInteracting&&!thePlayer.isChased){

            if(Input.GetKeyDown(KeyCode.Q)){
                if(!theBook.book.activeSelf) theBook.BookOn();
                else if(theBook.paper.activeSelf) theBook.BookOff();
                theBook.OnPaper();
            }
            if(Input.GetKeyDown(KeyCode.I)){
                if(!theBook.book.activeSelf) theBook.BookOn();
                else if(theBook.item.activeSelf) theBook.BookOff();
                theBook.OnItem();
            }
            if(Input.GetKeyDown(KeyCode.M)){
                if(!theBook.book.activeSelf) theBook.BookOn();
                else if(theBook.map.activeSelf) theBook.BookOff();
                theBook.OnMap();
            }
            if(Input.GetKeyDown(KeyCode.Escape)){
                if(manual.activeSelf) PopUpHelp();
                else if(makerInfo.activeSelf) makerInfo.SetActive(false);
                else if(!theBook.book.activeSelf) theBook.BookOn();
                else if(theBook.setting.activeSelf) theBook.BookOff();
                theBook.OnSetting();
            }
            if(Input.GetKeyDown(KeyCode.Tab)){
                if(theBook.book.activeSelf){

                    if(theBook.paper.activeSelf){
                        theBook.OnItem();
                    }
                    else if(theBook.item.activeSelf){
                        theBook.OnMap();
                    }
                    else if(theBook.map.activeSelf){
                        theBook.OnSetting();
                    }
                    else if(theBook.setting.activeSelf){
                        theBook.OnPaper();
                    }
                }
            }
            
            if(Input.GetKeyDown(KeyCode.F1)){
                PopUpHelp();
            }
                /*
            if(theBook.book.activeSelf && Input.GetKeyDown(KeyCode.Tab)){
                if(theBook.paper.activeSelf){
                    theBook.OnItem();
                }
                else if(theBook.item.activeSelf){
                    theBook.OnMap();
                }
                else if(theBook.map.activeSelf){
                    theBook.OnSetting();
                }
                else if(theBook.setting.activeSelf){
                    theBook.OnPaper();
                }
            }*/
            
        }
        // if(Input.GetKeyDown(KeyCode.F8)){
        //     loadFading();
        // }
    }
    /*
    public void loadFading(){
        StartCoroutine(loadFadingCoroutine());

    }
    
    IEnumerator loadFadingCoroutine(){
        Fade2Manager.instance.FadeOut();
        yield return new WaitForSeconds(1f);      
        theSL.CallLoad();          
        Fade2Manager.instance.FadeIn();
    }*/
        
    public void PopUpHelp(){
        //thePlayer.notMove = !manual.activeSelf;
        manual.SetActive(!manual.activeSelf);
        AudioManager.instance.Play("pageflip");
    }

    #if DEV_MODE

    void Transportation(string mapName){
        StartCoroutine(TPC(mapName));
        if(mapName!="start") PlayerManager.instance.currentMapName = mapName;
        PlayerManager.instance.ChangeColor();
        PlayerManager.instance.CheckPassive();
//        Debug.Log(mapName);
        // SceneManager.LoadScene(mapName);
        // PlayerManager.instance.debuggingMode = true;
        // PlayerManager.instance.transform.position = new Vector3 (0,0,0);
        // DisableColliders();
    }
    public IEnumerator TPC(string mapName){
        //SceneManager.LoadScene("start");
        //yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(mapName);
        yield return new WaitForSeconds(0.2f);
        PlayerManager.instance.transform.position = new Vector3 (0,0,0);
        //PlayerManager.instance.debuggingMode = true;
        
        //StopToTest();
        //DisableColliders();
        if(mapName == "start"){

            LoadingTrig.instance.loadWindow.SetActive(false);
            Fade2Manager.instance.color = Fade2Manager.instance.black.color;
            Fade2Manager.instance.color.a = 0f;
            Fade2Manager.instance.black.color = Fade2Manager.instance.color;
            Fade2Manager.instance.go.SetActive(false);

        }
        PlayerManager.instance.shadow_laydown.GetComponent<Animator>().SetTrigger("off");
        PlayerManager.instance.shadow_laydown.gameObject.SetActive(false);
                    
        PlayerManager.instance.notMove = false;
        PlayerManager.instance.isWakingup = false;
        PlayerManager.instance.exc.SetBool("on",false);
        PlayerManager.instance.canInteractWith = 0;

        DatabaseManager.instance.bookActivated = true;
    }

    void StopToTest(){
        
        StopCoroutine(thePlayer.WakingUp());
        DatabaseManager.instance.doneIntro=true;
        thePlayer.animator.SetBool("wake_up",false);
        thePlayer.shadow_laydown.gameObject.SetActive(false);
        thePlayer.notMove = false;
        thePlayer.isWakingup = false;
    }
    
    public void MapKey(int num){
        FloatingText();

        HotkeyManager.instance.devMode.SetActive(false);
        AudioManager.instance.Play("button22");

        switch(num){
            case 0 :
                Transportation("start");
                break;
            // case 1 :
            //     Transportation("cabin");
            //     break;
            // case 2 :
            //     Transportation("catwood");
            //     break;
            case 1 :
                Transportation("catwood2");
                break;
            case 2 :
                Transportation("ch2");
                break;
            case 3 :
                Transportation("ch3");
                break;
            case 4 :
                Transportation("cornerwood");
                break;
            case 5 :
                Transportation("camp");
                break;
            case 6 :
                Transportation("middlewood");
                break;
            case 7 :
                Transportation("village");
                break;
            case 8 :
                Transportation("lake");
                break;
            case 9 :
                Transportation("lakein");
                break;
            case 10 :
                Transportation("lakeout");
                break;
            case 11 :
                Transportation("rainingforest");
                break;
            case 12 :
                Transportation("parrothidden");
                break;
            case 13 :
                Transportation("thunderingforest");
                break;
            case 14 :
                Transportation("mazein");
                break;
            case 15 :
                Transportation("maze");
                break;
            case 16 :
                Transportation("mazeout");
                break;
            case 17 :
                Transportation("end");
                break;
        }
    }
    public void ToggleSpeed(){
        FloatingText();
        AudioManager.instance.Play("button22");
        HotkeyManager.instance.devMode.SetActive(false);
        //DebugManager.instance.speedUp = !DebugManager.instance.speedUp;
        PlayerManager.instance.runSpeed = PlayerManager.instance.runSpeed == 8f ? 20f : 8f;        
    }
    public void EnableUnknown(){
        FloatingText();
        AudioManager.instance.Play("button22");
        HotkeyManager.instance.devMode.SetActive(false);
        UnknownManager.instance.activateRandomAppear = true;
    }
    public void DisableUnknown(){
        FloatingText();
        AudioManager.instance.Play("button22");
        HotkeyManager.instance.devMode.SetActive(false);
        UnknownManager.instance.activateRandomAppear = false;
    }
    public void GetItems(int num){
        FloatingText();
        AudioManager.instance.Play("button22");
        HotkeyManager.instance.devMode.SetActive(false);
        BookManager.instance.BookOff();
        for(int i=0;i<24;i++){
            Inventory.instance.RemoveItem(i+1);
        }
        for(int j=(num-1)*12;j<num*12;j++){
            Inventory.instance.GetItem(j+1);
        }
    }

    public void TogglePassBtn(){
        FloatingText();
        AudioManager.instance.Play("button22");
        HotkeyManager.instance.devMode.SetActive(false);
        foreach(GameObject a in GameManager.instance.testBtns){
            a.SetActive(!a.activeSelf);
        }
    }
    public void ResetAll(){
        FloatingText();
        AudioManager.instance.Play("button22");
        HotkeyManager.instance.devMode.SetActive(false);
        UnknownManager.instance.nowPhase = UnknownManager.instance.CheckMaps();
        PlayerManager.instance.runSpeed = 8f;
        foreach(GameObject a in GameManager.instance.testBtns){
            a.SetActive(false);
        }
        ToggleColliders(true);
        // TransferMap[] transferMaps=FindObjectsOfType(typeof(TransferMap)) as TransferMap[];
        // foreach(TransferMap door in transferMaps){
        //     MapManager.instance.doorStateBackupList = DatabaseManager.instance.doorEnabledList;
        //     door.Enabled = false;
        //     DatabaseManager.instance.doorEnabledList.Clear();
        //     DatabaseManager.instance.doorEnabledList = MapManager.instance.doorStateBackupList;
        //     //DatabaseManager.instance.doorEnabledList.Remove(door.doorNum);
        //     //door.Enabled = MapManager.instance.doorStateBackup[door.doorNum] ? true : false;
        // }
    }
    public void FloatingText(){
        //Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
        Inventory.instance.PrintFloating(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text, null);
    }
    public void ToggleColliders(bool reset){
        FloatingText();
        AudioManager.instance.Play("button22");
        HotkeyManager.instance.devMode.SetActive(false);

        BoxCollider2D[] collider2Ds=FindObjectsOfType(typeof(BoxCollider2D)) as BoxCollider2D[];
        EdgeCollider2D[] collider2Ds2=FindObjectsOfType(typeof(EdgeCollider2D)) as EdgeCollider2D[];
        CircleCollider2D[] collider2Ds3=FindObjectsOfType(typeof(CircleCollider2D)) as CircleCollider2D[];
        PolygonCollider2D[] collider2Ds4=FindObjectsOfType(typeof(PolygonCollider2D)) as PolygonCollider2D[];
    
        foreach(BoxCollider2D col in collider2Ds){
            col.enabled = reset ? true : !col.enabled;
        }
        foreach(EdgeCollider2D col2 in collider2Ds2){
            col2.enabled = reset ? true : !col2.enabled;
        }
        foreach(CircleCollider2D col3 in collider2Ds3){
            col3.enabled = reset ? true : !col3.enabled;
        }
        foreach(PolygonCollider2D col4 in collider2Ds4){
            col4.enabled = reset ? true : !col4.enabled;
        }
    }
    public void ForceToMove(){
        FloatingText();
        AudioManager.instance.Play("button22");
        HotkeyManager.instance.devMode.SetActive(false);
        PlayerManager.instance.notMove = false;
    }
    public void ForceEnableDoors(){
        
        FloatingText();
        AudioManager.instance.Play("button22");
        HotkeyManager.instance.devMode.SetActive(false);

        MapManager.instance.doorStateBackupList=DatabaseManager.instance.doorEnabledList;

        MapManager.instance.ForceEnableDoors = true;
        TransferMap[] transferMaps=FindObjectsOfType(typeof(TransferMap)) as TransferMap[];

        foreach(TransferMap door in transferMaps){
            door.Enabled = true;
        }
    }
    public void GetBook(){
        FloatingText();
        AudioManager.instance.Play("button22");
        HotkeyManager.instance.devMode.SetActive(false);
        DatabaseManager.instance.bookActivated = true;

    }
    public void GetLetters(){
        FloatingText();
        AudioManager.instance.Play("button22");
        HotkeyManager.instance.devMode.SetActive(false);
        
        PaperManager.instance.letter0.gameObject.SetActive(true);
        theDB.letterPaper[0] = true;
        PaperManager.instance.letter1.gameObject.SetActive(true);
        theDB.letterPaper[1] = true;
        PaperManager.instance.letterBtnUpdate = true;//.SetBool("flash",true);
        BookManager.instance.ActivateUpdateIcon(0);
    }

#endif

}
