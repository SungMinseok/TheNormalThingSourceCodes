using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{   
    public string clickSound;
    private Item mainItem;  //버튼 눌러 사용 할때 필요.
    public Image mainImage; //대표 아이템 이미지
    public Sprite nullImage; //빈 이미지
    public Sprite defaultVessel;
    public Sprite defaultName;
    public Sprite selectedVessel;
    public Sprite selectedName;
    public Text mainText;   //대표 아이템 설명
    public Text mainName;   //대표 아이템 이름
    public Button mainButton;   //대표 아이템 사용하기 (Clickable아이템만)


    public Button[] pageTab;  //페이지 버튼
    //private int page;
    private int slotCount;  //활성화 슬롯 개수
    private const int MAX_SLOTS_COUNT = 4;  //최대 슬롯 개수
    private int nowPage;




    public static Inventory instance;                   // 게임 중 오브젝트에서 GetItem 함수 사용하기 위해서 선언.
    public List<Item> inventoryItemList; // 플레이어가 소지한 아이템 리스트
    public Transform slotObject;    // slot의 부모객체 (grid slot)
    private InventorySlot[] slots; //인벤토리 슬롯들    slots = slotObject.GetComponentsInChildren<InventorySlot>();       을 통해 연결시킴.      

    
    ////////////////////////////////아이템 사용 대화/////////////////
    
    public Dialogue dialogue_1; // 3 이상한 나뭇잎
    public Dialogue wrongUse; // 잘못된 사용.
    
    
    
    
    
    public GameObject floatingText;
    public GameObject floatingCanvas;
    
    
    
    
    
    
    
    private DatabaseManager theDB;
    private BookManager theBook;
    private DialogueManager theDM;
    private PlayerManager thePlayer;
    AudioManager theAudio;

    public List<Item> SaveItem(){
        return inventoryItemList;
    }
    public void LoadItem(List<Item> _itemList){
        inventoryItemList = _itemList;
    }
    void Start()
    {

        instance=this;


        theDB=DatabaseManager.instance;
        theBook=BookManager.instance;
        theDM=DialogueManager.instance;
        theAudio=AudioManager.instance;
        thePlayer=PlayerManager.instance;
        inventoryItemList = new List<Item>();
        slots = slotObject.GetComponentsInChildren<InventorySlot>();
        inventoryItemList.Clear();
        RemoveSlot();
        
        //inventoryItemList.Add(new Item(10001, "빨간 포션"/*, "체력 50을 채워줌", Item.ItemType.Use)*/));
        //inventoryItemList.Add(new Item(10002, "파란 포션"/*, "마나 15을 채워줌", Item.ItemType.Use)*/));
        //ShowItem();

        //Debug.Log("inventoryItemList[slotNum].itemID " + inventoryItemList + "!!!");
    }

    public void ShowItem(){              //BookUpdate용, 실제로 가지고 있는 아이템 갯수 만큼 슬롯 0번부터 차례로 대입함. 초기화하고. 아이템 사용후 실행할 것.
        
        ShowPage(0);

        for(int i=0; i<pageTab.Length; i++){
            pageTab[i].interactable = true;
        }
        pageTab[0].interactable = false;

        
    }

    public void ShowPage(int pageNum){          //버튼 눌러서 페이지 넘김
        slotCount = 0;
        ResetVessels();
        nowPage = pageNum;
        theAudio.Play(clickSound);
        
        for(int i=0; i<pageTab.Length; i++){
            pageTab[i].interactable = true;
        }
        pageTab[pageNum].interactable = false;
       
        
        RemoveSlot();

        for(int i=pageNum*MAX_SLOTS_COUNT; i< slots.Length+pageNum*MAX_SLOTS_COUNT; i++){
            slotCount = i - (pageNum*MAX_SLOTS_COUNT);
            slots[slotCount].icon.GetComponent<Button>().interactable = false;
            if(slotCount == MAX_SLOTS_COUNT -1)
                break;
        }
        
        // for(int i=pageNum*MAX_SLOTS_COUNT; i< slots.Length+pageNum*MAX_SLOTS_COUNT; i++){
        //     if(inventoryItemList[i]==null) break;
        //     slotCount = i - (pageNum*MAX_SLOTS_COUNT);
        //     slots[slotCount].icon.GetComponent<Button>().interactable = true;       //아이템 가진 갯수만큼만 버튼 활성화
        //     if(slotCount == MAX_SLOTS_COUNT -1)
        //         break;
        // }
        
        for(int i=pageNum*MAX_SLOTS_COUNT; i< MAX_SLOTS_COUNT+(pageNum*MAX_SLOTS_COUNT); i++){
            if(i>=inventoryItemList.Count || inventoryItemList[i]==null) break;
            else{

                slotCount = i - (pageNum*MAX_SLOTS_COUNT);
                Debug.Log(i+"번 째 아이템 표시");
                slots[slotCount].AddItem(inventoryItemList[i]);
                slots[slotCount].icon.GetComponent<Button>().interactable = true; 

                    
                if(slotCount == MAX_SLOTS_COUNT -1)
                    break;
            }

        }
        
    }

    public void ShowMainItem(int slotNum){                              //아이템 개개 버튼 누르면 설명 창 활성화.  사용하기 위해 ID도 받아옴 
        int temp = slotNum + nowPage*MAX_SLOTS_COUNT;
        ResetVessels();
        if(inventoryItemList[temp]!=null){
            
           // if(mainItem.itemType!=Item.ItemType.Passive) 
            mainButton.gameObject.SetActive(true);
            theAudio.Play(clickSound);
            mainItem = inventoryItemList[temp];
            mainImage.sprite = inventoryItemList[temp].itemIcon;
            mainName.text = inventoryItemList[temp].itemName;
            mainText.text = inventoryItemList[temp].itemDescription;
            slots[slotNum].vessel.sprite = selectedVessel;
            slots[slotNum].nameVessel.sprite = selectedName;


            

            // if(mainItem.itemType != Item.ItemType.Clickable)            //clickable 아니면 사용하기 안눌리게
            //     mainButton.interactable = false;
            // else mainButton.interactable = true;

            if(mainItem.itemType == Item.ItemType.Clickable  || mainItem.itemType == Item.ItemType.Wearable|| mainItem.itemType == Item.ItemType.Readable){
                mainButton.interactable = true;
            }
            else{
                mainButton.gameObject.SetActive(false);
            }
        }
    }

    public void ResetVessels(){//선택 강조된 것들 원래대로 복구
        for(int i=0;i<MAX_SLOTS_COUNT;i++){
            slots[i].vessel.sprite = defaultVessel;
            slots[i].nameVessel.sprite = defaultName;
        }
    }

    public void GetItem(int _itemID){                               //다른 오브젝트에서 아이템 획득시 실행되는 함수. id만 받아와서 DB에 있는 이름과 이미지 불러옴.
        
        BookManager.instance.ActivateUpdateIcon(1);
        for(int i=0; i<theDB.itemList.Count; i++){
            if(_itemID == theDB.itemList[i].itemID){
                if(!SearchItem(_itemID)){

                    inventoryItemList.Add(theDB.itemList[i]);
                    Debug.Log("아이템등록");
                    return;
                }
                else
                Debug.Log("아이템 이미 보유중");
                return;
            }
        }
        Debug.Log("데이터베이스에 없음");
    }
    public void RemoveSlot(){                                       //아주 처음 초기화
        for(int i = 0; i<slots.Length ; i++){
            slots[i].RemoveItem();
        }
        
        mainImage.sprite = nullImage;
        mainName.text = "";
        mainText.text = "";
        //mainButton.interactable = false;
        mainButton.gameObject.SetActive(false);
    }

    public bool SearchItem(int _itemID){//아이템 목록에서 검색 : 찾아서 있으면 true 반환.
        for(int i=0; i<inventoryItemList.Count; i++){
            if(_itemID == inventoryItemList[i].itemID){
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(int _itemID){//퀘스트 아이템 사용 후 인벤토리에서 제거.
         for(int i=0; i<inventoryItemList.Count; i++){
            if(_itemID == inventoryItemList[i].itemID){
                inventoryItemList.RemoveAt(i);
            }
        }        
    }

    public void OnClickItem(){//아이템창 "사용하기" 슬롯 누를 때.
        if(mainItem.itemType==Item.ItemType.Clickable){
            UseItemByButton();
            CursorChange();
        }
        else if(mainItem.itemType==Item.ItemType.Wearable) WearItem();
        else if(mainItem.itemType==Item.ItemType.Readable) ReadItem();
    }
    public void UseItemByButton(){//DB의 아이템효과와 연결                  
        //if(mainItem.itemID!=null){
            theDB.ActivateItem(mainItem.itemID);    //사용 아이템이면 DB에서 OnActivated 켜줘야함.
        //}       
    }
    
    public void CursorChange(){          //Clickable 타입의 아이템만 아이템창에서 선택. 
        //if(/*mainItem.itemID!=null && */mainItem.itemType==Item.ItemType.Clickable){
            
            theAudio.Play(clickSound);
            BookManager.instance.BookOff();
            Debug.Log("마우스커서 변환성공");
            Cursor.SetCursor(CursorManager.instance.activatedCursor , Vector2.zero, CursorMode.ForceSoftware);
            CursorManager.instance._default = false;
        //}
    }

    
    public void OnClickItemEvent(int num){//커서 전환 후 버튼 클릭으로 아이템 사용    ex)puzzle0에서 햇빛 object 버튼에 주는 함수
        
        if(theDB.OnActivated[num]){                     //아이템 클릭한 상태
            theDB.OnActivated[num] = false;             
            
            // if(num==1 && PlayerManager.instance.currentMapName == "catwood2"){
            //     FindObjectOfType<Trig12>().AccessByButton();
            //     //Puzzle0.instance.GiveCandy();
            // }
            if(num==2){
                Puzzle0.instance.GiveCandy();
            }

            if(num==3){
                theDM.ShowDialogue(dialogue_1);
            }

            if(num==6){
                Puzzle1.instance.HangBulb();
            }
                
            CursorManager.instance.RecoverCursor();
        }
        else return;
    }
    //////////////////밑에는 백업용(슬롯마다 사용하기)
    /*
    public void OnClickItem(int slotNum){                                   //아이템창 슬롯 누를 때.
        UseItemByButton(slotNum);
        CursorChange(slotNum);
    }
    public void UseItemByButton(int slotNum){                       //DB의 아이템효과와 연결                  
        
        if(inventoryItemList[slotNum].itemID!=null){

            theDB.ActivateItem(inventoryItemList[slotNum].itemID); 
        }       

    }
    
    public void CursorChange(int slotNum){          //Clickable 타입의 아이템만 아이템창에서 선택. 
        if(inventoryItemList[slotNum].itemID!=null && inventoryItemList[slotNum].itemType==Item.ItemType.Clickable){
            BookManager.instance.BookOff();
            Debug.Log("마우스커서 변환성공");
            CursorManager.instance._default = false;
            Cursor.SetCursor(inventoryItemList[slotNum].itemTexture , Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    
    public void OnClickItemEvent(int num){              //커서 전환 후 버튼 클릭으로 아이템 사용
        
        if(theDB.OnActivated[num]){                     //아이템 클릭한 상태
            theDB.OnActivated[num] = false;             
            theDB.OnActivated[0] = false;
            
            theDM.ShowDialogue(dialogue_1);
            CursorManager.instance.RecoverCursor();
        }
    }*/
    public void WearItem(){
        RemoveItem(mainItem.itemID);
        BookManager.instance.BookOff();
        ShowPage(nowPage);
        switch(mainItem.itemID){
            case 13 :
                //Debug.Log("똥신 착용 성공");
                PrintFloatingText("달리기 속도 +1");
                AudioManager.instance.Play("mattouch");
                thePlayer.runSpeed=9;
                break;
            case 15 :
                //Debug.Log("똥신 착용 성공");
                AudioManager.instance.Play("mattouch");
                thePlayer.spriteRenderer.flipX = true;
                //thePlayer.spriteRenderer.flipY = true;
                break;
            case 17 :
                //Debug.Log("똥신 착용 성공");
                AudioManager.instance.Play("eat0");
                thePlayer.spriteRenderer.color = new Color(1,1,0,1);
                thePlayer.normalColor = new Color(1,1,0,1);
                //thePlayer.spriteRenderer.flipY = true;
                break;
        }
    }
    public void ReadItem(){
        //RemoveItem(mainItem.itemID);
        //BookManager.instance.BookOff();
        //ShowPage(nowPage);
        switch(mainItem.itemID){
            case 16 :
                //Debug.Log("똥신 착용 성공");
                AudioManager.instance.Play("pageflip");
                BookManager.instance.unknownManaul.SetActive(true);
                //thePlayer.runSpeed=9;
                break;
        }
    }
    //패시브 아이템 보유시 실행
    public void ActivatePassiveItem(int num){
        switch(num){
            case 14:
                PrintFloatingText("생명력 +1");
                thePlayer.life =1;
                break;
        }
    }
    public void PrintFloatingText(string text){

        var clone = Instantiate(floatingText, floatingCanvas.transform.position, Quaternion.identity);
        clone.GetComponent<FloatingText>().text.text = text;
        clone.transform.SetParent(floatingCanvas.transform);
    }
    void FixedUpdate(){
        if(!thePlayer.isInteracting){
            if(Input.GetKeyDown(KeyCode.Space)&&mainName.text != ""&&BookManager.instance.book.activeSelf){
                OnClickItem();
            }

        }
    }
    public IEnumerator WrongUse(){
        CursorManager.instance.RecoverCursor();
        DialogueManager.instance.ShowDialogue(wrongUse);
        yield return new WaitUntil(()=> !DialogueManager.instance.talking);
    }
}
