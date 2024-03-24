#if UNITY_ANDROID || UNITY_IOS
#define DISABLEKEYBOARD
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
    
    public static BookManager instance;
    public Text gameTimer;
    public Dialogue dialogue;
    public bool firstOpen;
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
    public string pageflipSound;
    [Header ("Readable")]
    public GameObject unknownManaul;
    public GameObject mazeManaul;
    public GameObject mazeManaul_en;
    public GameObject makersInfo;
    public GameObject[] readables;
    [Space]
    public GameObject help;

    [Header ("Basic")]
    public GameObject book;
    public GameObject updateIcon;
    public GameObject onButton;
    public GameObject paper;
    public GameObject paperbutton;
    public Text paperText;
    public GameObject item;
    public GameObject itembutton;
    public Text itemText;
    public GameObject map;
    public GameObject mapbutton;
    public Text mapText;
    public GameObject setting;
    public GameObject settingbutton;
    public Text settingText;
    public GameObject disableBtn;
    public GameObject[] btns;
    public GameObject nextBtn;
    [Header ("Update")]
    public GameObject[] updateIcons;

    private DatabaseManager theDB;
    private OrderManager theOrder;
    private MapManager theMap;
    private PlayerManager thePlayer;
    private SettingManager theSet;
    private PuzzleManager thePuzzle;
    private List<GameObject> uiList = new List<GameObject>();
    private List<GameObject> uiButtonList = new List<GameObject>();
    AudioManager theAudio;
    PaperManager thePaper;
    public GameObject redAlert;
    public Puzzle3 puzzle3;
    
    Color basicCol = new Color(0.2352941f,0.2313726f,0.2235294f,1);
    Color selectedCol = new Color(0.949f,0.937f,0.87f,1);

    void Start(){
        
        theDB = FindObjectOfType<DatabaseManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theMap = FindObjectOfType<MapManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        //thePlayer = FindObjectOfType<PlayerManager>();
        theSet = FindObjectOfType<SettingManager>();
        theAudio = AudioManager.instance;
        thePaper = PaperManager.instance;
        thePuzzle = PuzzleManager.instance;
#if DISABLEKEYBOARD
        help.SetActive(false);
#endif


        
        //uiList.Add(paper,paperbutton,item,itembutton,map,mapbutton,setting,settingbutton);
        uiList.Add(paper);
        uiList.Add(item);
        uiList.Add(map);
        uiList.Add(setting);
        uiButtonList.Add(paperbutton);
        uiButtonList.Add(itembutton);
        uiButtonList.Add(mapbutton);
        uiButtonList.Add(settingbutton);

        //if(theDB.firstOpen) firstOpen = true;
    }
    public void BookBtn(){
        if(!book.activeSelf){
            BookOn();
        }
        else{
            BookOff();
        }
    }
    public void BookOn(bool soundOn = true){

        if(!theDB.firstOpen){
            thePlayer.notMove = true;
            //onButton.GetComponent<Button>().interactable = false;
            StartCoroutine(FirstOpen());
        }
        else{
            puzzle3.SpritesOff();

            if(updateIcon.activeSelf)                   //updateIcon 켜져있으면 책 열 때 무조건 꺼줌.
                updateIcon.SetActive(false);

            for(int i=0;i<3;i++){
                if(uiList[i].activeSelf && updateIcons[i].activeSelf){
                    updateIcons[i].SetActive(false);
                    break;
                }
            }

            if(!book.activeSelf&&!thePlayer.isInteracting){
                if(soundOn) theAudio.Play(pageflipSound);
                book.SetActive(true);
                BookUpdate();
                //thePuzzle.treeFace.SetActive(false);
                //onButton.GetComponent<Button>().interactable = false;
                theOrder.NotMove();
                //bookOnButton.GetComponent<Button>().interactable = false;

            }
        
        }


        thePlayer.canInteractWith = -3;
        thePlayer.exc.SetBool("on",false);
    }
    public void BookOff(){
        if(book.activeSelf){
            theAudio.Play(pageflipSound);
            book.SetActive(false);
            // if(thePuzzle.puzzleNum[1].activeSelf)
            //     thePuzzle.treeFace.SetActive(true);
            //onButton.GetComponent<Button>().interactable = true;
            if(!DialogueManager.instance.talking&&!SelectManager.instance.selecting&&!thePlayer.isPlayingPuzzle)//대화중이 아닐 때만 다시 이동가능
                theOrder.Move();
            
            //bookOnButton.GetComponent<Button>().interactable = true;
        }

        
        Invoke("DelayBookOff", 0.01f);
    }
    public void DelayBookOff(){
        thePlayer.canInteractWith = 0;
    }
    public void BookUpdate(){                                                               //지도 블러 제거, 지도 포인트 설정
        
        ///////////////////////////////////////////Item/////////////////////////////
        Inventory.instance.ShowItem();


        ///////////////////////////////////////////Map/////////////////////////////
        if(map.activeInHierarchy) theMap.MapUpdate();
        //theMap.blur[0].SetActive(false);
        /*
        for(int i=0; i<=theDB.progress ; i++){
            theMap.blur[i].SetActive(false);
        }*/
        
        /*
        for(int i=0; i<theMap.point.Length ; i++){
            theMap.point[i].SetActive(false);
        }
        if(thePlayer.currentMapName == "start"){
            theMap.point[0].SetActive(true);
        }
        else if(thePlayer.currentMapName == "catwood" || thePlayer.currentMapName == "catwood2" ){
            
            theMap.point[1].SetActive(true);
        }
        else if(thePlayer.currentMapName == "ch2" ){
            
            theMap.point[2].SetActive(true);
        }*/
        ///////////////////////////////////////////Setting/////////////////////////////
        theSet.LoadInform();
        ///////////////////////////////////////////Paper/////////////////////////////
        if(paper.activeInHierarchy) thePaper.PageBtnUpdate();
        
    }
    public void OnPaper(bool mute = false){
        if(!paper.activeSelf){
            for(int i=0 ; i <uiList.Count; i++){
                uiList[i].SetActive(false);
                uiButtonList[i].GetComponent<Button>().interactable = true;
            }
            ChangeTextColor(0);

            if(!mute) theAudio.Play(pageflipSound);
            paper.SetActive(true);

            if(paper.activeInHierarchy) thePaper.PageBtnUpdate();
            paperbutton.GetComponent<Button>().interactable = false;
            //theOrder.NotMove();
            //bookOnButton.GetComponent<Button>().interactable = false;
            if(theDB.letterPaper[0])
                PaperManager.instance.letter0.gameObject.SetActive(true);
            if(theDB.letterPaper[1])
                PaperManager.instance.letter1.gameObject.SetActive(true);

                
            updateIcons[0].SetActive(false);
        }
    }
    public void OnItem(){
        if(!item.activeSelf){
            for(int i=0 ; i <uiList.Count; i++){
                uiList[i].SetActive(false);
                uiButtonList[i].GetComponent<Button>().interactable = true;
            }
            ChangeTextColor(1);
        Inventory.instance.ShowItem();
            theAudio.Play(pageflipSound);
            item.SetActive(true);
            itembutton.GetComponent<Button>().interactable = false;
            //paperbutton.GetComponent<Button>().interactable = false;
            //theOrder.NotMove();
            //bookOnButton.GetComponent<Button>().interactable = false;

            updateIcons[1].SetActive(false);
        }
    }
    public void OnMap(){
        if(!map.activeSelf){
            for(int i=0 ; i <uiList.Count; i++){
                uiList[i].SetActive(false);
                uiButtonList[i].GetComponent<Button>().interactable = true;
            }
            ChangeTextColor(2);
        // //if(theDB.progress!=0){
        //     for(int i=0; i<theDB.progress ; i++){
        //         theMap.blur[i].SetActive(false);
        //     }
        //}
            theAudio.Play(pageflipSound);
            map.SetActive(true);
        if(map.activeInHierarchy) theMap.MapUpdate();
            mapbutton.GetComponent<Button>().interactable = false;
            //paperbutton.GetComponent<Button>().interactable = false;
            //theOrder.NotMove();
            //bookOnButton.GetComponent<Button>().interactable = false;

            updateIcons[2].SetActive(false);
        }
    }
    public void OnSetting(){
        if(!setting.activeSelf){
            for(int i=0 ; i <uiList.Count; i++){
                uiList[i].SetActive(false);
                uiButtonList[i].GetComponent<Button>().interactable = true;
            }
            ChangeTextColor(3);
        theSet.LoadInform();
            theAudio.Play(pageflipSound);
            setting.SetActive(true);
            settingbutton.GetComponent<Button>().interactable = false;
            theSet.ActivateController();
            //paperbutton.GetComponent<Button>().interactable = false;
            //theOrder.NotMove();
            //bookOnButton.GetComponent<Button>().interactable = false;

        }
    }

    void FixedUpdate(){
        if(theDB!=null&&theDB.bookActivated&&!onButton.activeSelf)
            onButton.SetActive(true);

        if((!book.activeSelf&& thePlayer.notMove&&!thePlayer.isPlayingGame&&!thePlayer.isPlayingPuzzle)||thePlayer.isChased||redAlert.activeSelf)
            onButton.GetComponent<Button>().interactable = false;
        else
            onButton.GetComponent<Button>().interactable = true;
        // else if(!thePlayer.isChased)
        //     onButton.GetComponent<Button>().interactable = true;
    }

    public void ActivateUpdateIcon(int i=-1){   //0 page, 1 item, 2 map
        
        if(!updateIcon.activeSelf)
            updateIcon.SetActive(true);
        if(i!=-1)
            updateIcons[i].SetActive(true);
        if(i==2){
            theMap.blurAnim = true;
        }
        else if(i==0){
            
            thePaper.blurAnim = true;
        }
    }

    IEnumerator FirstOpen(){
        
        firstOpen = true;
        theDB.firstOpen = true;
        thePlayer.boxCollider.enabled = false;
        //onButton.GetComponent<Button>().interactable=false;
        DialogueManager.instance.ShowDialogue(dialogue);
        yield return new WaitUntil(()=> !DialogueManager.instance.talking);
        //DialogueManager.instance.ForceAlphaOut();
        BookOn();
        thePlayer.boxCollider.enabled = true;
    }

    // public void DisableBookOnBtn(){
    //     onButton.GetComponent<Button>().interactable = false;
    //     Debug.Log("책 잠금");
    //     Debug.Log(onButton.GetComponent<Button>().interactable);
    // }
    // public void EnableBookOnBtn(){
    //     onButton.GetComponent<Button>().interactable = true;
    // }
    public void ClickSound(string name){
        theAudio.Play(name);
    }

    public void ChangeTextColor(int num){
        // paperText.color = new Color(0.2352941f,0.2313726f,0.2235294f,1);
        // itemText.color = new Color(0.2352941f,0.2313726f,0.2235294f,1);
        // mapText.color = new Color(0.2352941f,0.2313726f,0.2235294f,1);
        // settingText.color = new Color(0.2352941f,0.2313726f,0.2235294f,1);
        paperText.color = basicCol;
        itemText.color = basicCol;
        mapText.color = basicCol;
        settingText.color = basicCol;

        switch(num){
            case 0:
                paperText.color = selectedCol;
                break;
            case 1:
                itemText.color = selectedCol;
                break;
            case 2:
                mapText.color = selectedCol;
                break;
            case 3:
                settingText.color = selectedCol;
                break;
        }
    }
    public void StoryNextBtn(){
        nextBtn.SetActive(false);
        PaperManager.instance.waitStory = false;
    }
    public bool ReadableIsOn(){
        for(int i=0; i<readables.Length; i++){
            if(readables[i].activeSelf){
                return true;
            }
        }
        return false;
    }
    
    public void SetRes(int num){
        switch(num){
            case 0 :
                Screen.SetResolution(1920, 1080, true);

                break;
            case 1 :
                Screen.SetResolution(1920, 1080, false);

                break;
            case 2 :
                Screen.SetResolution(1600, 900, false);

                break;
            case 3 :
                Screen.SetResolution(1280, 720, false);

                break;
        }
    }
}
