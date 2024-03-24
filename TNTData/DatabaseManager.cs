using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class DatabaseManager : MonoBehaviour{

    public static DatabaseManager instance;
    // Start is called before the first frame update
    PaperManager thePaper;
    MapManager theMap;
    public Text text_Timer;

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
    
    
    
    /*
    public string[] var_name;
    public float[] var;

    public string[] switch_name;
    public bool[] switches;
*/
    //public int[] door_locked;

    public int phaseNum=0;                                        //0 : 로딩없이 시작.
                                                                //x>0: 로딩.(인트로 없음)
    /////////////////////////mapTransfer관련//////////////////
    public List<int> doorEnabledList = new List<int>();        //등록 되어있으면 열려있는거
    public List<int> doorLockedList = new List<int>();  //등록 되어있으면 잠겨있는거
    ////////////////////////////////////////////////////
    
    /////////////////////////UI관련//////////////////
    public bool bookActivated;                          //책 엔터 아이콘 등장
    //public bool uiHide;

    //public int where;           //map에 포인트 위치

    
    /////////////////////////trigger관련//////////////////
    
    public List<int> trigOverList = new List<int>();    //등록 되어있으면 다시 실행 안됨

    
    /////////////////////////story관련//////////////////
    public bool doneIntro;      //true면 일어나서 대화안함
    public bool firstOpen;      //책 첫대사
    public bool activateRandomAppear;   //언노운 출현시작여부
    public int progress;        //progress 숫자 에 해당하는 map blur 지움. : bookManager
    public List<int> puzzleOverList = new List<int>();    //등록 되어있으면 다시 실행 안됨
    public List<int> gameOverList = new List<int>();    //등록 되어있으면 다시 실행 안됨
    public int activatedPaper;  //획득한 찢어진 페이지
    public bool[] letterPaper = new bool[2];    //

    public bool isPlayingPuzzle2;//퍼즐2 탈출 전 일 때를 위해서...
    public int caughtCount; //언노운한데 잡힌 횟수
    public float gameTimer;
    /////////////////////////item관련//////////////////

    
    public List<Item> itemList = new List<Item>();
    public List<int> itemOverList = new List<int>();  //아이템 재생성 안하도록하기 위해서,

    public bool[] OnActivated = new bool[25];          //true 이면 n번 아이템 사용중.
    public void ActivateItem(int _itemID){
        DeactivateItem();
        OnActivated[_itemID] = true;
        OnActivated[0] = true;  //빨간손 체크용

    }
    public void DeactivateItem(){

        for(int i=0; i<OnActivated.Length; i++){
            OnActivated[i]=false;
        }
    }

    /////////////////////////////////SAVELOAD관련//////////////////////////////////
    
    public string[] saveName = new string[3];
    public string[] saveTime = new string[3];

    public int lastSaveNum;         //시작화면 로딩창에 불러올 데이터 번호

    void Start()
    {   
        instance =this;
        LocalizeItemNames(GameMultiLang.instance.nowLang);
        //bool[] OnActivated= new bool[itemList.Count+1]; 
    }
    // void OnGUI(){
        
    //     GUI.Box (new Rect (0,0,100,50), "Top-left");
    //     GUI.Box (new Rect (Screen.width - 100,0,100,50), "Top-right");
    //     GUI.Box (new Rect (0,Screen.height - 50,100,50), "Bottom-left");
    //     GUI.Box (new Rect (Screen.width - 100,Screen.height - 50,100,50), "Bottom-right");
    //     GUI.TextArea(new Rect(100,50,100,30), "Timer : "+Time.time.ToString("N2"));

    // }

    void Update(){
        if(BookManager.instance!=null){

            if(text_Timer==null) text_Timer = BookManager.instance.gameTimer;
            else{
                if(!PlayerManager.instance.isGameOver)
                gameTimer += Time.deltaTime;
                //text_Timer.text = Mathf.Round(gameTimer).ToString();
            }
        }
    }

    public void ResetDB(){//false 이면 맨 처음 로드시.
        
        //phaseNum=0;
        doorEnabledList.Clear();
        doorLockedList.Clear();
        bookActivated=false;
        trigOverList.Clear();
        doneIntro=false;
        progress=0;
        puzzleOverList.Clear();
        gameOverList.Clear();
        activatedPaper=0;
        itemOverList.Clear();
        //itemList.Clear();
        Inventory.instance.inventoryItemList.Clear();
        PaperManager.instance.ResetPapers();
        MapManager.instance.ResetMaps();
        //Debug.Log("???2");
    }

    public void LocalizeItemNames(string lang){
        //Debug.Log("아이템 번역 적용 "+GameMultiLang.instance.nowLang);
        itemList.Clear();
        if(lang == "en"){

            itemList.Add(new Item(1, "Spiky Leaf","It is a leaf with a pointed tip.", Item.ItemType.Clickable));
            itemList.Add(new Item(2, "Sweet Candy","When you're depressed, sweet candies sometimes help you.",Item.ItemType.Clickable));
            itemList.Add(new Item(3, "Strange Leaf","It's dark so it's hard to see...",Item.ItemType.Clickable));
            itemList.Add(new Item(4, "Light Bulb","It's an empty light bulb.\nMaybe it could contain something.",Item.ItemType.Clickable));
            itemList.Add(new Item(5, "Shiny Powder","It's a shiny powder on a rock. A subtle light shines",Item.ItemType.Clickable));
            itemList.Add(new Item(6, "Light Bulb with Water","It's a light bulb filled with water.",Item.ItemType.Clickable));
            itemList.Add(new Item(7, "Cactus Key","It is a cactus-shaped ornament.",Item.ItemType.Clickable));
            itemList.Add(new Item(8, "Handle","It is a bit broken, but it is a handle that can be used.",Item.ItemType.Clickable));
            itemList.Add(new Item(9, "House Key","It's like the key to this house.",Item.ItemType.Clickable));
            itemList.Add(new Item(10, "Match","It is a match that can ignite.",Item.ItemType.Clickable));
            itemList.Add(new Item(11, "Nipper","It's an old nipper. I think I can use it.",Item.ItemType.Clickable));
            itemList.Add(new Item(12, "Feather of a Parrot","It's the reddish feather of a parrot.\nIs there something to use...",Item.ItemType.Clickable));
            itemList.Add(new Item(13, "Wheeled Shoes","whoosh whoosh ",Item.ItemType.Wearable,true));
            itemList.Add(new Item(14, "Four Leaf Clover","If you have it, you will be lucky...\nMaybe you can defeat black thing.",Item.ItemType.Passive,true));
            itemList.Add(new Item(15, "Jackson Shoes","Maker has two left feet.",Item.ItemType.Wearable,true));
            itemList.Add(new Item(16, "Suspicious Note","There is something written on it.",Item.ItemType.Readable));
            itemList.Add(new Item(17, "Nana Milk","Maker's Favorite Milk",Item.ItemType.Wearable,true));
            itemList.Add(new Item(18, "Hedgehog's Note","I think something important is in this note.",Item.ItemType.Readable));
            itemList.Add(new Item(19, "Red Apple","At first glance, It looks like heart.", Item.ItemType.Clickable));
            itemList.Add(new Item(20, "Pumpkin Stone","It's a pumpkin stone with worms inside.\nIt feels like time has stopped.", Item.ItemType.Clickable));
            itemList.Add(new Item(21, "Eye-shaped Rice","It is rice obtained from a field.\nIt is shaped like an eye.", Item.ItemType.Clickable));
        }
        else{

            itemList.Add(new Item(1, "뾰족 잎사귀","끝 부분이 뾰족뾰족한 잎사귀다.", Item.ItemType.Clickable));
            itemList.Add(new Item(2, "달콤한 사탕","우울할 때는 달콤한 사탕이 위로가 되기도 하지.",Item.ItemType.Clickable));
            itemList.Add(new Item(3, "이상한 나뭇잎","어두워서 잘 보이지 않는다...",Item.ItemType.Clickable));
            itemList.Add(new Item(4, "전구","안이 비어있는 전구다.\n무언가 담을 수 있을지도.",Item.ItemType.Clickable));
            itemList.Add(new Item(5, "빛나는 가루","바위에 있던 빛나는 가루다. 은은한 빛이 난다",Item.ItemType.Clickable));
            itemList.Add(new Item(6, "물이 담긴 전구","물을 담은 전구다.",Item.ItemType.Clickable));
            itemList.Add(new Item(7, "선인장 모형","선인장 모양의 장식품이다.",Item.ItemType.Clickable));
            itemList.Add(new Item(8, "손잡이","조금 부서졌지만 쓸 수 있는 손잡이다.",Item.ItemType.Clickable));
            itemList.Add(new Item(9, "집의 열쇠","이 집의 열쇠 같다.",Item.ItemType.Clickable));
            itemList.Add(new Item(10, "성냥","불을 붙일 수 있는 성냥이다.",Item.ItemType.Clickable));
            itemList.Add(new Item(11, "니퍼","낡은 니퍼다. 사용은 할 수 있을 것 같다.",Item.ItemType.Clickable));
            itemList.Add(new Item(12, "앵무새의 깃털","앵무새의 붉은 색이 도는 깃털이다.\n쓸 데가 있으려나.",Item.ItemType.Clickable));
            itemList.Add(new Item(13, "바퀴 달린 신발","슝슝",Item.ItemType.Wearable,true));
            itemList.Add(new Item(14, "네잎클로버","가지고 있으면 행운이...\n검은 물체를 물리칠 수 있을지도.",Item.ItemType.Passive,true));
            itemList.Add(new Item(15, "잭슨슈즈","제작자는 몸치에요.",Item.ItemType.Wearable,true));
            itemList.Add(new Item(16, "수상한 쪽지","무언가 적혀있다.",Item.ItemType.Readable));
            itemList.Add(new Item(17, "나나 우유","제작자의 최애 우유",Item.ItemType.Wearable,true));
            itemList.Add(new Item(18, "남겨진 쪽지","누군가 중요한 내용을 적어 놓은 것 같다.",Item.ItemType.Readable));
            itemList.Add(new Item(19, "붉은 사과","언뜻 보면 심장처럼 보이는 모양의 사과다.", Item.ItemType.Clickable));
            itemList.Add(new Item(20, "호박석","안에 벌레가 간직된 호박석이다.\n시간이 멈춘 듯 보인다.", Item.ItemType.Clickable));
            itemList.Add(new Item(21, "눈모양 벼","밭에서 얻은 벼다.\n눈 모양을 하고있다.", Item.ItemType.Clickable));
        }
    }
}
