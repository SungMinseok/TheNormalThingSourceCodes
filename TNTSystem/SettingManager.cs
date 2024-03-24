using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;
    public Slider bgmVolumeController;
    public Slider effVolumeController;
    public string clickSound;
    public string saveSound;
    public Transform slotObject;
    public SettingSlot[] slots;
    public GameObject error;
    public SpriteRenderer loadIcon;
    public GameObject alertPop_Save;
    public GameObject alertPop_Load;
    public GameObject alertPop_EXIT;
    public GameObject makersInfo;

    private string nameStr;
    private string timeStr;
    private string temp;

    private DatabaseManager theDB;
    private SaveNLoad theSL;
    public SaveNLoad testSL;
    private PlayerManager thePlayer;
    private LoadingTrig theLT;
    private BookManager theBook;
    AudioManager theAudio;
    BGMManager BGM;

    private string tempNum; //Save : 01,02,03 Load : 11,12,13

    //private PlayerManager thePlayer;

    void Start(){
        instance = this;
        slots = slotObject.GetComponentsInChildren<SettingSlot>();
        theDB = FindObjectOfType<DatabaseManager>();
        theSL = FindObjectOfType<SaveNLoad>();
        thePlayer = PlayerManager.instance;
        theLT = LoadingTrig.instance;
        theBook = BookManager.instance;
        theAudio=AudioManager.instance;
        BGM=BGMManager.instance;

        //theSL.loadList();
        bgmVolumeController.value = BGM.firstVolume;
        effVolumeController.value = theAudio.firstVolume;
        

    }
    public void ActivateController(){
        
        bgmVolumeController.onValueChanged.AddListener (delegate {BGMValueChangeCheck();});
        effVolumeController.onValueChanged.AddListener (delegate {EffValueChangeCheck();});
    }
    public void BGMValueChangeCheck(){              //세팅창 소리조절
        BGM.SetVolume(bgmVolumeController.value);
    }
    public void EffValueChangeCheck(){
        theAudio.SetAllVolume(effVolumeController.value);
    }

    public void WriteInform(int num){
        
        theAudio.Play(saveSound);
        
        GetMapInfo();   //맵이름 코드로 저장.
        // if(temp=="start") nameStr="시작의 숲";
        // else if(temp=="cabin") nameStr="시작의 숲";
        // else if(temp=="catwood") nameStr="시작의 숲";
        // else if(temp=="catwood2") nameStr="시작의 숲";
        // else if(temp=="ch2") nameStr="해가 드는 곳";
        // else if(temp=="ch3") nameStr="숲의 모퉁이";
        // else if(temp=="cornerwood") nameStr="숲의 모퉁이";
        // else if(temp=="camp") nameStr="빈 야영지";
        // else if(temp=="middlewood") nameStr="빈 야영지";
        // else if(temp=="village") nameStr="낡은 촌락";
        timeStr = DateTime.Now.ToString(("yyyy.MM.dd HH:mm"));
        //Debug.Log("name : "+str);
        //slots[num-1].name_text.text = nameStr;                                  //1번파일에 저장하면 0번슬롯에 표시
        slots[num-1].name_text.text = GameMultiLang.GetTraduction (nameStr);
        slots[num-1].time_text.text = timeStr;

        //theDB.saveName[num-1]=slots[num-1].name_text.text ;
        theDB.saveName[num-1]=nameStr ;
        theDB.saveTime[num-1]=slots[num-1].time_text.text ;

        theDB.lastSaveNum = num;    
    }

    public void LoadInform(){                   //저장목록 불러오기
        /*
        for(int i=0; i<theDB.saveName.Length; i++){
            if(theDB.saveName[i]!=null)
                slots[i].name_text.text = theDB.saveName[i];
        }
        for(int i=0; i<theDB.saveTime.Length; i++){
            if(theDB.saveTime[i]!=null)
                slots[i].time_text.text = theDB.saveTime[i];
        }*/
        theSL.loadListInPlaying();
    }

    public void SaveInform(){                                      //게임 종료할 때 한번 더 실행해줌.
        
    }

    
    public void StartLoad(int num){         //세팅창에서.
    
        theAudio.Play(clickSound);


            thePlayer.notMove=true;
        thePlayer.isPlayingPuzzle = false;
        thePlayer.isPlayingGame = false;
        PuzzleManager.instance.puzzleCheck = false;

        foreach(GameObject a in PuzzleManager.instance.puzzleNum){
            a.SetActive(false);
        }
        foreach(GameObject b in GameManager.instance.GameList){
            b.SetActive(false);
        }


        FileInfo file = new FileInfo(Application.persistentDataPath + "/" + Application.version + "/SaveFile" + num +".dat");

        if(file.Exists){
            
            StartCoroutine(StartLoadCoroutine(num));
        }
        
        else error.SetActive(true);
    }
    IEnumerator StartLoadCoroutine(int num){
        
        //Debug.Log(num + "번 파일 로드 준비");
        theDB.phaseNum = num;
        Fade2Manager.instance.FadeOut(0.02f,1f);
        yield return new WaitForSeconds(2f);
        //SceneManager.LoadScene("start");
        
        theLT.loadWindow.SetActive(true);  //로딩창 온
            thePlayer.isLoading = true;
        
        
        yield return new WaitForSeconds(0.01f);
        ObjectManager.instance.FadeIn(loadIcon);
        Fade2Manager.instance.FadeIn(0.02f);        
        
        yield return new WaitForSeconds(0.5f);   //오브젝트 불러오는 시간 필요 (개중요)
        
        thePlayer.notMove=true;
        
        
        theSL.CallLoad(num);
        yield return new WaitForSeconds(2f);
        theBook.BookOff();
        alertPop_Load.SetActive(false);
        
        thePlayer.exc.SetBool("on",false);
        thePlayer.canInteractWith = 0;
        
        thePlayer.animator.SetBool("onFish", false);
        FadeManager.instance.fog0.SetActive(false);
        
        ObjectManager.instance.FadeOut(loadIcon);
        Fade2Manager.instance.FadeOut(0.02f,1f);
        
        
        yield return new WaitForSeconds(2f);
        theLT.loadWindow.SetActive(false);
            thePlayer.isLoading = false;
        thePlayer.LetBegin();
        //yield return new WaitForSeconds(2f);
        //theSL.CallLoad(num);
    }

    public void QuitGame(){
        // if(theDB.phaseNum!=0)
        //     SaveNLoad.instance.CallSave(theDB.phaseNum);


        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else 
            Application.Quit();
        #endif
    }
    
    public void SetBtnClick(string num){    //저장or로드 클릭
        tempNum = num;
        
        theAudio.Play(clickSound);

        int realNum = 0;
        switch(tempNum){
            case "01" :
                realNum=1;
                break;
            case "02" :
                realNum=2;
                break;
            case "03" :
                realNum=3;
                break;
            case "11" :
                realNum=1;
                break;
            case "12" :
                realNum=2;
                break;
            case "13" :
                realNum=3;
                break;
        }

        FileInfo file = new FileInfo(Application.persistentDataPath + "/" + Application.version + "/SaveFile" + realNum +".dat");
        if(num=="01"||num=="02"||num=="03"){

            if(file.Exists){
                alertPop_Save.SetActive(true);
            }
            else{
                ExecuteSaveOrLoad();
            }
        }
        else{
            if(file.Exists){
                alertPop_Load.SetActive(true);
            }
            else{
                error.SetActive(true);
            }
        }
        //alertPop_SNL.SetActive(true);
        //Debug.Log(tempNum+"번 버튼 클릭");
    }
    public void ExecuteSaveOrLoad(){   //경고팝업 계속 클릭 
        
        alertPop_Save.SetActive(false);
        alertPop_Load.SetActive(false);
        switch(tempNum){
            case "01" :
                //testSL.CallSave(1);
                WriteInform(1);
                theSL.CallSave(1);
                //Debug.Log(tempNum+"번에 저장");
                break;
            case "02" :
                WriteInform(2);
                theSL.CallSave(2);
                //Debug.Log(tempNum+"번에 저장");
                break;
            case "03" :
                WriteInform(3);
                theSL.CallSave(3);
                //Debug.Log(tempNum+"번에 저장");
                break;
            case "11" :
                StartLoad(1);
                break;
            case "12" :
                StartLoad(2);
                break;
            case "13" :
                StartLoad(3);
                break;
        }
        tempNum = "";
    }
    public void ExitBtnClick(){
        alertPop_EXIT.SetActive(true);
        theAudio.Play(clickSound);
    }

    public void SetResolution(int num){
        if(num==0){
            
            Screen.SetResolution(1920, 1080, true);
            //Debug.Log("1920");        
        }
        else if(num==1){
            
            Screen.SetResolution(1680, 1050, true);
            //Debug.Log("1680"); 
        }
    }
    public void OpenMakersInfo(){
        makersInfo.SetActive(true);
    }
    public void LinkURL(string name){
        switch(name){
            case "mail" :
                Application.OpenURL("https://blog.naver.com/orata1996/");
                break;
            case "instagram" :
                Application.OpenURL("https://www.instagram.com/orata_1996_studio/");
                break;
            case "twitter" :
                Application.OpenURL("https://www.twitter.com/OrataStudio/");
                break;

        }
    }
    public void GetMapInfo(){

        switch(PlayerManager.instance.currentMapName){
            case "start" :
                nameStr="map0";
                break;
            case "cabin" :
                nameStr="map0";
                break;
            case "catwood" :
                nameStr="map0";
                break;
            case "catwood2" :
                nameStr="map1";
                break;
            case "ch2" :
                nameStr="map2";
                break;
            case "ch3" :
                nameStr="map3";
                break;
            case "cornerwood" :
                nameStr="map4";
                break;
            case "camp" :
                nameStr="map5";
                break;
            case "middlewood" :
                nameStr="map6";
                break;
            case "village" :
                nameStr="map7";
                break;
            case "lake" :
                nameStr="map8";
                break;
            case "lakein" :
                nameStr="map9";
                break;
            case "lakeout" :
                nameStr="map10";
                break;
            case "rainingforest" :
                nameStr="map11";
                break;
            case "parrothidden" :
                nameStr="map12";
                break;
            case "thunderingforest" :
                nameStr="map13";
                break;
            case "mazein" :
                nameStr="map14";
                break;
            case "maze" :
                nameStr="map15";
                break;
            case "mazeout" :
                nameStr="map16";
                break;
            case "end" :
                nameStr="map17";
                break;
        }

        //return nameStr;
    
        //nameStr = GameMultiLang.GetTraduction (nameStr);
        
    }
    // public void GetMapInfo(){
    //     if(GameMultiLang.instance.nowLang=="en"){

    //         switch(PlayerManager.instance.currentMapName){
    //             case "start" :
    //                 nameStr="Forest of Beginnings";
    //                 break;
    //             case "cabin" :
    //                 nameStr="Forest of Beginnings";
    //                 break;
    //             case "catwood" :
    //                 nameStr="Forest of Beginnings";
    //                 break;
    //             case "catwood2" :
    //                 nameStr="Silent Road";
    //                 break;
    //             case "ch2" :
    //                 nameStr="Sunny Forest";
    //                 break;
    //             case "ch3" :
    //                 nameStr="Big Rock Road";
    //                 break;
    //             case "cornerwood" :
    //                 nameStr="Corner Forest";
    //                 break;
    //             case "camp" :
    //                 nameStr="Empty Campsite";
    //                 break;
    //             case "middlewood" :
    //                 nameStr="Dense Forest";
    //                 break;
    //             case "village" :
    //                 nameStr="Old Village";
    //                 break;
    //             case "lake" :
    //                 nameStr="Into the Lake";
    //                 break;
    //             case "lakein" :
    //                 nameStr="Foggy Lake";
    //                 break;
    //             case "lakeout" :
    //                 nameStr="Beyond the Lake";
    //                 break;
    //             case "rainingforest" :
    //                 nameStr="Rainy North Forest";
    //                 break;
    //             case "parrothidden" :
    //                 nameStr="Parrot Forest";
    //                 break;
    //             case "thunderingforest" :
    //                 nameStr="Thunder North Forest";
    //                 break;
    //             case "mazein" :
    //                 nameStr="Altar Forest";
    //                 break;
    //             case "maze" :
    //                 nameStr="Tangled Forest";
    //                 break;
    //             case "mazeout" :
    //                 nameStr="The Way to the End";
    //                 break;
    //             case "end" :
    //                 nameStr="Rocking Bridge Cliff";
    //                 break;
    //         }
    //     }
    //     else{
    //         switch(PlayerManager.instance.currentMapName){
    //             case "start" :
    //                 nameStr="시작의 숲";
    //                 break;
    //             case "cabin" :
    //                 nameStr="시작의 숲";
    //                 break;
    //             case "catwood" :
    //                 nameStr="시작의 숲";
    //                 break;
    //             case "catwood2" :
    //                 nameStr="고요한 길";
    //                 break;
    //             case "ch2" :
    //                 nameStr="해가 드는 곳";
    //                 break;
    //             case "ch3" :
    //                 nameStr="큰 바위가 있는 길";
    //                 break;
    //             case "cornerwood" :
    //                 nameStr="숲의 모퉁이";
    //                 break;
    //             case "camp" :
    //                 nameStr="빈 야영지";
    //                 break;
    //             case "middlewood" :
    //                 nameStr="나무가 우거진 곳";
    //                 break;
    //             case "village" :
    //                 nameStr="낡은 촌락";
    //                 break;
    //             case "lake" :
    //                 nameStr="호수로 들어가는 길";
    //                 break;
    //             case "lakein" :
    //                 nameStr="안개 짙은 호수";
    //                 break;
    //             case "lakeout" :
    //                 nameStr="호수 건너편";
    //                 break;
    //             case "rainingforest" :
    //                 nameStr="비 오는 북쪽 숲";
    //                 break;
    //             case "parrothidden" :
    //                 nameStr="앵무의 숲";
    //                 break;
    //             case "thunderingforest" :
    //                 nameStr="천둥치는 북쪽 숲";
    //                 break;
    //             case "mazein" :
    //                 nameStr="제단의 숲";
    //                 break;
    //             case "maze" :
    //                 nameStr="어질러진 숲";
    //                 break;
    //             case "mazeout" :
    //                 nameStr="끝으로 가는 길";
    //                 break;
    //             case "end" :
    //                 nameStr="절벽의 흔들다리";
    //                 break;
    //         }

    //     }
    // }
}
