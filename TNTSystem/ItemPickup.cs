using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour         
{
    public int itemID;
    
    public void GetIt(){                            //버튼 클릭으로 아이템 획득.
        if(itemID==2)
            AudioManager.instance.Play("getitem2");
        Inventory.instance.GetItem(itemID);
        Debug.Log("아이템획득");
        this.gameObject.SetActive(false);

    }
}

