using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Sprite nullImage;
    public Text itemName_Text;
    public Image vessel;
    public Image nameVessel;
    public Image eE;

    public void AddItem(Item _item){
        itemName_Text.text = _item.itemName;
        icon.sprite = _item.itemIcon;
    }

    public void RemoveItem(){
        itemName_Text.text = "";
        icon.sprite = nullImage;
    }
}
