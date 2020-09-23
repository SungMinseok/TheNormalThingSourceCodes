using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Control : MonoBehaviour
{
    public bool activateRelocate;
    public game2 game2;
    public Block[] blocks;
    public Slot1[] slots;
    public GameObject[] vessels;

    public bool[] checkAll;

    public int temp;   //잡은 블록 번호

    public int blockNum;
    public List<string> linkBlock = new List<string>(); //(0,1,2,5), ...
    public int[] linkBlockArray;

    public List<List<int>> lastNum = new List<List<int>>();
    void Start()
    {
        checkAll = new bool[blocks.Length];
        for(byte i=0; i<blocks.Length;i++){
            blocks[i].num = i;
            slots[i].num = i;
        }
        
        //자동으로 연결시키기 ..
        for(int a=0;a<blocks.Length;a++){
            blocks[a].linkedBlock.Clear();
        }
        for(int k=0; k<linkBlock.Count; k++){   //k번 째 블록모음의 {0,1,2,5}
            linkBlockArray = linkBlock[k].Split(',').Select(int.Parse).ToArray();
            for(int l=0;l<linkBlockArray.Length;l++){   //l번 째 블록에 다른 블록 집어넣어줌. {0}
                for(int m=0; m<linkBlockArray.Length;m++){                  // 0빼고 1,2,5
                    if(linkBlockArray[l]!=linkBlockArray[m]){
                        
                        blocks[linkBlockArray[l]].linkedBlock.Add(GameObject.Find("Blocks").
                        transform.Find("Slot ("+linkBlockArray[m].ToString()+")").gameObject.
                        GetComponent<Block>());
                        //Debug.Log(linkBlockArray[l]+"번 블록에 "+linkBlockArray[m]+"번 블록 연결 시킴");

                        blocks[linkBlockArray[l]].vessel = vessels[k];
                    }
                }
                //if(l==0){
                    blocks[linkBlockArray[0]].vessel = vessels[k];
                    
                    //Debug.Log(linkBlockArray[0]+"번 블록의 vessel은 "+k+"번");
                //}
            }
        }
        
        for(int j=0; j<blocks.Length;j++){
            
            for(int i=0; i<blocks[j].linkedBlock.Count;i++){
                blocks[j].linkedNum.Add(blocks[j].linkedBlock[i].num);
            }
        }


        for(int a=0; a<blocks.Length; a++){

            //고정되있는 블록이면 그자리 슬롯 채워진 상태로.
            if(blocks[a].fixedBlock){
                blocks[a].transform.SetAsFirstSibling();
                slots[blocks[a].num].check = true;
                blocks[a].enabled = false;
            }
            else{
                
                blocks[a].Relocate();
            }
        }




    }
    void Update()
    {
        // for(int i=0; i<slots.Length; i++){
        //     checkAll[i]=slots[i].check;
        // }
    }

    public void CheckGame(){
        for(int i=0; i<slots.Length; i++){
            if(slots[i].check){

                checkAll[i] = slots[i].check;
            }
            else return;
            
        }

        //Debug.Log("퍼즐 완성");
        //game2.passGame();
        
        AudioManager.instance.Play("success0");
        StartCoroutine(GameManager.instance.GameSuccess());
        game2.Invoke("passGame",GameManager.instance.successWaitTime);
    }
}
