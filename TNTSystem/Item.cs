using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Item
{
    public int itemID;  //고유 ID값, 중복불가
    public string itemName; //이름, 중복가능
    public string itemDescription; //설명
    //public int itemCount; //소지 갯수
    public Sprite itemIcon;
    public Texture2D itemTexture;   //커서용
    public ItemType itemType;
    public bool isEE;

    public enum ItemType{
        Clickable,         //클릭시 커서 전환
        Wearable,
        Passive,
        Readable,
        ETC
    }
    //데이터베이스에서 가져오는 값들
    public Item(int _itemID, string _itemName, string _itemDes, ItemType _itemType, bool _isEE = false/*, string _itemDes, ItemType _itemType, int _itemCount =1*/){
        itemID= _itemID;
        itemName= _itemName;
        itemDescription= _itemDes;
        itemType= _itemType;
        //itemCount= _itemCount;
        itemIcon= Resources.Load("ItemIcon/"+_itemID.ToString(), typeof(Sprite)) as Sprite;
        itemTexture= Resources.Load("ItemIcon/"+_itemID+"_cursor".ToString(), typeof(Texture2D)) as Texture2D;
        isEE= _isEE;
    }
}
