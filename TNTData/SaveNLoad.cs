using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveNLoad : MonoBehaviour
{
    public static SaveNLoad instance;
    // Start is called before the first frame update
    
    private void Awake()

    /////////////////////////////////////////////////////////로드할 때 loadFadingN 함수 실행하시오.


    {
        instance = this;
    }
    
    //public bool loading;
    //private Fade2Manager theFade;
    [System.Serializable]   //SL에 필수적 속성 : 직렬화. 컴퓨터가 읽고쓰기 쉽게.
    public class Data{
        public float playerX;
        public float playerY;
        public float playerZ;

        public List<int> playerItemInventory;   //
        
        public string currentMapName;
        //public string lastMapName;

        public int mazeNum;

        /////////////////////////mapTransfer관련//////////////////
        public List<int> doorEnabledList;       //등록 되어있으면 열려있는거
        public List<int> doorLockedList;  //등록 되어있으면 잠겨있는거
        ////////////////////////////////////////////////////
        
        /////////////////////////UI관련//////////////////
        public bool bookActivated;                          //책 엔터 아이콘 등장
        //public bool uiHide;

        public int where;           //map에 포인트 위치
        
        /////////////////////////trigger관련//////////////////
        
        public List<int> trigOverList;    //등록 되어있으면 다시 실행 안됨

        
        /////////////////////////story관련//////////////////
        public bool doneIntro;
        public bool firstOpen;
        public bool activateRandomAppear;
        public int progress;        //progress 숫자 에 해당하는 map blur 지움. : bookManager
        public List<int> puzzleOverList;    //등록 되어있으면 다시 실행 안됨
        public List<int> gameOverList = new List<int>();    //등록 되어있으면 다시 실행 안됨
        public int activatedPaper;  //획득한 찢어진 페이지
        public bool[] letterPaper = new bool[2]; 

            
        public bool isPlayingPuzzle2;//퍼즐2 탈출 전 일 때를 위해서...

        public int caughtCount;//언노운 잡힌 횟수
        public float gameTimer;

        //ITEM

        
        public List<int> itemOverList = new List<int>(); 

        /////////////////////////////////SAVELOAD관련//////////////////////////////////
        
        public string[] saveName;
        public string[] saveTime;
        public int lastSaveNum;
    
    }
    private PlayerManager thePlayer;
    private DatabaseManager theDB;
    private Inventory theInven;
    private MainMenuLoad MML;
    private SettingManager theSet;
    private UnknownManager theUnknown;

    public Data data;
    private Vector3 vector;


    public void CallSave(int num){
        Debug.Log(num+"번 파일 저장 성공");


        theDB=FindObjectOfType<DatabaseManager>();
        thePlayer=FindObjectOfType<PlayerManager>();
        theInven=FindObjectOfType<Inventory>();

        theDB.phaseNum = num;



        data.playerX = thePlayer.transform.position.x;
        data.playerY = thePlayer.transform.position.y;
        data.playerZ = thePlayer.transform.position.z;

        data.currentMapName = thePlayer.currentMapName;
        //data.lastMapName = thePlayer.lastMapName;
        data.mazeNum = thePlayer.mazeNum;

        data.doorEnabledList = theDB.doorEnabledList;
        data.doorLockedList = theDB.doorLockedList;

        data.bookActivated = theDB.bookActivated;

        data.trigOverList = theDB.trigOverList;
        ///////////////////////////////////////////////////////////////
        data.doneIntro = theDB.doneIntro;
        data.firstOpen = theDB.firstOpen;
        data.activateRandomAppear = theDB.activateRandomAppear;
        
//        Debug.Log("SAVE : theDB.doneIntro : "+ data.doneIntro);
        data.progress = theDB.progress;
        data.puzzleOverList = theDB.puzzleOverList;
        data.gameOverList = theDB.gameOverList;
        data.activatedPaper= theDB.activatedPaper;
        data.letterPaper = theDB.letterPaper;


        data.isPlayingPuzzle2 = theDB.isPlayingPuzzle2;
        data.caughtCount = theDB.caughtCount;
        data.gameTimer = theDB.gameTimer;
/////////////////////////////////////////////////////////////////////////


        data.itemOverList = theDB.itemOverList;

/////////////////////////////////////////////////////////////////////////



        data.saveName = theDB.saveName;
        data.saveTime = theDB.saveTime;
        data.lastSaveNum = theDB.lastSaveNum;
        
        for(int i=0; i<3; i++){
            
            //Debug.Log("Name : "+theDB.saveName[i]);
        }

//        Debug.Log("기초 데이터 입력성공");

        data.playerItemInventory.Clear();

        List<Item> itemList = theInven.SaveItem();

        for(int i=0; i<itemList.Count ; i++){
 //           Debug.Log("인벤토리 아이템 저장완료 : "+ itemList[i].itemID);
            data.playerItemInventory.Add(itemList[i].itemID);
        }

        BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(Application.dataPath + "/SaveFile1.dat");
        //FileStream file = File.Create(Application.dataPath + "/SaveFile" + num +".dat");
//Application.persistentDataPath
        FileStream file = File.Create(Application.persistentDataPath + "/" + Application.version + "/SaveFile" + num +".dat");
        bf.Serialize(file, data);
        file.Close();   //파일 내보내기 끝

//        Debug.Log("세이브 "+num+" 파일 저장 성공");

    }

    public void CallLoad(int num){

        BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Open(Application.dataPath + "/SaveFile1.dat", FileMode.Open);
        FileStream file = File.Open(Application.persistentDataPath + "/" + Application.version + "/SaveFile" + num +".dat", FileMode.Open);

        if(file != null && file.Length >0){

            DatabaseManager.instance.ResetDB();

            data =(Data)bf.Deserialize(file);

            theDB=FindObjectOfType<DatabaseManager>();
            thePlayer=FindObjectOfType<PlayerManager>();
            theInven=FindObjectOfType<Inventory>();
            theUnknown=FindObjectOfType<UnknownManager>();

            thePlayer.currentMapName = data.currentMapName;
            //thePlayer.lastMapName = data.lastMapName;
            thePlayer.lastMapName = "";

            thePlayer.mazeNum = data.mazeNum;
            
            UnknownManager.instance.nowPhase = UnknownManager.instance.CheckMaps();

            vector.Set(data.playerX, data.playerY, data.playerZ);
            thePlayer.transform.position = vector;

            
            theDB.doorEnabledList =data.doorEnabledList;
            theDB.doorLockedList =data.doorLockedList;

            theDB.bookActivated = data.bookActivated;

            theDB.trigOverList = data.trigOverList;

            theDB.doneIntro = data.doneIntro;
            theDB.firstOpen = data.firstOpen;
            theDB.activateRandomAppear = data.activateRandomAppear;
            theUnknown.activateRandomAppear = theDB.activateRandomAppear;//바로연동
//            Debug.Log("LOAD : theDB.doneIntro : "+ theDB.doneIntro);
            theDB.progress =data.progress;
            theDB.puzzleOverList =data.puzzleOverList;
            theDB.gameOverList =data.gameOverList;
            theDB.activatedPaper=data.activatedPaper;
            theDB.letterPaper= data.letterPaper;

            
            theDB.isPlayingPuzzle2 = data.isPlayingPuzzle2;
            if(theDB.lastSaveNum!=num) theDB.caughtCount = data.caughtCount;
            if(theDB.lastSaveNum!=num) theDB.gameTimer = data.gameTimer;


            //////////////////////////////////////////////////ITEM

            theDB.itemOverList = data.itemOverList;



            //////////////////////////////////////////////////SETTING
            theDB.saveName = data.saveName;
            theDB.saveTime = data.saveTime;
            theDB.lastSaveNum = data.lastSaveNum;

            List<Item> itemList = new List<Item>();

            for(int i=0; i<data.playerItemInventory.Count; i++){
                for(int j=0; j<theDB.itemList.Count;j++){
                    if(data.playerItemInventory[i] == theDB.itemList[j].itemID){
                        itemList.Add(theDB.itemList[j]);
                        //Debug.Log("인벤토리 item 로드했습니다 : "+theDB.itemList[j].itemID);
                        break;
                    }
                }
            }

            theInven.LoadItem(itemList);
            
            /*LoadManager theLoad = FindObjectOfType<LoadManager>();        //필요한가?
            theLoad.LoadStart();*/

            SelectManager.instance.ExitSelect();
            DialogueManager.instance.ExitDialogue();
            OrderManager.instance.Move();
            PlayerManager.instance.ResetPlayer();
            
            PuzzleManager.instance.SetPuzzle2();

            SceneManager.LoadScene(data.currentMapName);

        //CameraMovement.instance.SetBound(bound);
            //theLoad.Loading();
            //loading =false;
        }
        else{
            //Debug.Log("저장된 세이브 파일이 없습니다");
        
        }
        
        file.Close();
    }    
 


    public void loadFading(int num){
        StartCoroutine(loadFadingCoroutine(num));

    }
    
    IEnumerator loadFadingCoroutine(int num){
        Fade2Manager.instance.FadeOut();
        yield return new WaitForSeconds(1f);      
        CallLoad(num);          
        Fade2Manager.instance.FadeIn();
    }

    public void loadList(){                                         //메인 메뉴에서 로드창
        MML = FindObjectOfType<MainMenuLoad>();
        //int saveFiles = 0;
        for(int i=0; i<3; i++){

            FileInfo fileCheck = new FileInfo(Application.persistentDataPath + "/" + Application.version + "/SaveFile" + (i+1) +".dat");

            if(fileCheck.Exists){
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + Application.version + "/SaveFile" + (i+1) +".dat", FileMode.Open);
                
                if(file != null && file.Length >0){
                    data =(Data)bf.Deserialize(file);
                    
//                    Debug.Log("name : "+data.saveName[i]);
                    //MML.slots[i].name_text.text = data.saveName[i];
                    MML.slots[i].name_text.text = GameMultiLang.GetTraduction (data.saveName[i]);
                    MML.slots[i].time_text.text = data.saveTime[i];
                    //saveFiles += 1;

                }
                else{
                    if(GameMultiLang.instance.nowLang=="en"){
                    
                        MML.slots[i].name_text.text = "Blank note";
                    }
                    else{

                        MML.slots[i].name_text.text = "빈 쪽지";
                    }
                    MML.slots[i].time_text.text = "";
                   // Debug.Log(i+1 +"번에 저장된 파일없음");  
                } 

                file.Close();
            
            }
            
            else{
                
                if(GameMultiLang.instance.nowLang=="en"){
                
                    MML.slots[i].name_text.text = "Blank note";
                }
                else{

                    MML.slots[i].name_text.text = "빈 쪽지";
                }
                MML.slots[i].time_text.text = "";
//                Debug.Log(i+1 +"번에 저장된 파일없음");  
            } 

        
           
        }
        //return saveFiles;
    }
    
    
    public void loadListInPlaying(){                            //게임 중 저장 페이지 로드
        theSet = FindObjectOfType<SettingManager>();
        for(int i=0; i<3; i++){

            FileInfo fileCheck = new FileInfo(Application.persistentDataPath + "/" + Application.version + "/SaveFile" + (i+1) +".dat");

            if(fileCheck.Exists){
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + Application.version + "/SaveFile" + (i+1) +".dat", FileMode.Open);
                
                if(file != null && file.Length >0){
                    data =(Data)bf.Deserialize(file);
                    
                    //theSet.slots[i].name_text.text = data.saveName[i];
                    
                    theSet.slots[i].name_text.text = GameMultiLang.GetTraduction (data.saveName[i]);
                    theSet.slots[i].time_text.text = data.saveTime[i];
                }
                else{
                    
                    if(GameMultiLang.instance.nowLang=="en"){
                    
                        theSet.slots[i].name_text.text = "Blank note";
                    }
                    else{

                        theSet.slots[i].name_text.text = "빈 쪽지";
                    }
                    theSet.slots[i].time_text.text = "";
                    //Debug.Log(i+1 +"번에 저장된 파일없음");  
                } 
                
//                    Debug.Log("SaveName["+i+"] : "+data.saveName[i]);
                file.Close();
            
            }
            
            else{
            
                if(GameMultiLang.instance.nowLang=="en"){
                
                    theSet.slots[i].name_text.text = "Blank note";
                }
                else{

                    theSet.slots[i].name_text.text = "빈 쪽지";
                }
                theSet.slots[i].time_text.text = "";
                //Debug.Log(i+1 +"번에 저장된 파일없음");  
            } 

            
           
        }
    }

    public int SaveFilesCheck(){
        for(int i=0; i<3; i++){

            FileInfo fileCheck = new FileInfo(Application.persistentDataPath + "/" + Application.version + "/SaveFile" + (i+1) +".dat");

            if(fileCheck.Exists){
                return 0;
            }
        }
        return -1;
    }
    // public string GetMapInfo(){
    //     string nameStr = "";
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

    //     return nameStr;
    // }
}
