using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class Slot1 : MonoBehaviour, IDropHandler
{    
    public Control control;
    public byte num;
    public bool check;
    public List<int> temp;
    public bool deny;   //해당 위치에 놓을 수 없음

    public List<int> getNum = new List<int>();  //불러온 번호       0,1,2
    public List<int> tempNum = new List<int>(); //처리 후 번호들    3,4,5

    public void OnDrop(PointerEventData eventData){

        control.blocks[control.temp].check = true;  //퍼즐에 들어갔는지 체크
        //1 들어온 블록에 연결된 번호 일단 저장
        //SaveBlock();
        getNum = control.blocks[control.temp].linkedNum;    //  0, 1, 2

        
        control.blocks[control.temp].nowNum = num;
        if(getNum.Count!=0 && num + getNum[getNum.Count-1] - control.temp < control.slots.Length 
        && num + getNum[0] - control.temp > -1 ){

            for(int i=0;i<control.blocks[control.temp].linkedBlock.Count;i++){
                control.blocks[control.temp].linkedBlock[i].nowNum = control.slots[num + getNum[i] - control.temp].num;
            }
        }
        else{
            
            if(getNum.Count!=0){
                
                control.blocks[control.temp].nowNum = -1;
                for(int i=0;i<control.blocks[control.temp].linkedBlock.Count;i++){
                    control.blocks[control.temp].linkedBlock[i].nowNum = -1;
                }
                
                //Debug.Log("슬롯을 벗어남");
                if(control.activateRelocate) control.blocks[control.temp].Relocate();
                return;
            }
        }
        //5 자리 체크   //nownum = -1 로 해야함
        //SlotCheck1();
           
        for(int i=0; i<control.blocks[control.temp].linkedBlock.Count;i++){
            //Debug.Log("control.blocks[control.temp].linkedBlock[i].nowNum : "+control.blocks[control.temp].linkedBlock[i].nowNum);
            if(control.slots[control.blocks[control.temp].linkedBlock[i].nowNum].check){
                //Debug.Log(i + "번 블록이 겹침");
                control.blocks[control.temp].nowNum = -1;
                for(int j=0;j<control.blocks[control.temp].linkedBlock.Count;j++){
                    control.blocks[control.temp].linkedBlock[j].nowNum = -1;
                }
                
                if(control.activateRelocate) control.blocks[control.temp].Relocate();
                return;
            }
        }
        //2 블록이 들어간 퍼즐 번호 다시 지정해주고, 체크 온    (여기서 인덱스 초과 오류)
        //SlotCheck2();
        control.slots[num].check = true;    //해당 슬롯 채워짐
        tempNum.Add(num);
        if(getNum.Count!=0 && num + getNum[getNum.Count-1] - control.temp < control.slots.Length 
        && num + getNum[0] - control.temp > -1 ){
            int t1=num + getNum[getNum.Count-1] - control.temp;
            int t2=control.slots.Length;
            for(int i=0; i<getNum.Count; i++){
                int t3=num + getNum[i] - control.temp;
                control.slots[num + getNum[i] - control.temp].check = true;
                tempNum.Add(num + getNum[i] - control.temp);
            }
        }
        else{   //그자리에 둘 수 없음.
            if(getNum.Count!=0){

                //Debug.Log("Can't build there");
                control.blocks[control.temp].check = false;
                control.temp=-1;
                return;
            }
            
        }
    

        //4 퍼즐에 들어간 블록 더미 추가해줌.
        GetIn(eventData);

    }
    public void SaveBlock(){
        
        getNum = control.blocks[control.temp].linkedNum;    //  0, 1, 2

        
        control.blocks[control.temp].nowNum = num;
        if(getNum.Count!=0 && num + getNum[getNum.Count-1] - control.temp < control.slots.Length 
        && num + getNum[0] - control.temp > -1 ){

            for(int i=0;i<control.blocks[control.temp].linkedBlock.Count;i++){
                control.blocks[control.temp].linkedBlock[i].nowNum = control.slots[num + getNum[i] - control.temp].num;
            }
        }
        else{
            
            if(getNum.Count!=0){
                
                control.blocks[control.temp].nowNum = -1;
                for(int i=0;i<control.blocks[control.temp].linkedBlock.Count;i++){
                    control.blocks[control.temp].linkedBlock[i].nowNum = -1;
                }
                
                //Debug.Log("슬롯을 벗어남");
                control.blocks[control.temp].Relocate();
                return;
            }
        }
    }
    public void SlotCheck1(){
        
        for(int i=0; i<control.blocks[control.temp].linkedBlock.Count;i++){
            //Debug.Log("control.blocks[control.temp].linkedBlock[i].nowNum : "+control.blocks[control.temp].linkedBlock[i].nowNum);
            if(control.slots[control.blocks[control.temp].linkedBlock[i].nowNum].check){
                //Debug.Log(i + "번 블록이 겹침");
                control.blocks[control.temp].nowNum = -1;
                for(int j=0;j<control.blocks[control.temp].linkedBlock.Count;j++){
                    control.blocks[control.temp].linkedBlock[j].nowNum = -1;
                }
                
                control.blocks[control.temp].Relocate();
                return;
            }
        }
    }
    public void SlotCheck2(){

        control.slots[num].check = true;    //해당 슬롯 채워짐
        tempNum.Add(num);
        if(getNum.Count!=0 && num + getNum[getNum.Count-1] - control.temp < control.slots.Length 
        && num + getNum[0] - control.temp > -1 ){
            int t1=num + getNum[getNum.Count-1] - control.temp;
            int t2=control.slots.Length;
            for(int i=0; i<getNum.Count; i++){
                int t3=num + getNum[i] - control.temp;
                control.slots[num + getNum[i] - control.temp].check = true;
                tempNum.Add(num + getNum[i] - control.temp);
            }
        }
        else{   //그자리에 둘 수 없음.
            if(getNum.Count!=0){

                //Debug.Log("Can't build there");
                control.blocks[control.temp].check = false;
                control.temp=-1;
                return;
            }
            
        }
    }

    public void GetIn(PointerEventData eventData){

        control.lastNum.Add(tempNum);
        for(int i=0; i< control.lastNum.Count;i++){
                string line = string.Join(",", control.lastNum[i]);
                //Debug.Log("control.lastNum.Count: "+control.lastNum.Count+", {"+line+"}"+"\n");
        }

        if(eventData.pointerDrag != null){
            //eventData.pointerDrag.transform.position = transform.position;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
            =GetComponent<RectTransform>().anchoredPosition;
        }
        int ranNum = UnityEngine.Random.Range(0,4);
        
        AudioManager.instance.Play("hammer"+ranNum.ToString());
        control.CheckGame();
    }
   
}
