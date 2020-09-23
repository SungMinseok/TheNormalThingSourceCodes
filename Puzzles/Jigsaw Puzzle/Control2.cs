using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Control2 : MonoBehaviour
{
    public game18 game18;
    public bool activateRelocating;
    public Block2[] blocks;
    public Block2[] fakeBlocks;
    public Slot2[] slots;
    public GameObject[] vessels;

    public bool[] checkAll;

    public int temp;   //잡은 블록 번호

    //private byte t = 15;

    void Start()
    {
        checkAll = new bool[slots.Length];
        for(int i=0; i<15;i++){   //찐 블록 num = 0~14
            blocks[i].num = i;
            slots[i].num = i;
            //slots[i].transform.position = blocks[i].transform.position;
        }
        for(int j=0; j<fakeBlocks.Length; j++){    //가짜 블록 num = 15~23
            fakeBlocks[j].num = j+15;
        }
        
        //자동으로 연결시키기 ..
        // for(int a=0;a<blocks.Length;a++){
        //     blocks[a].linkedBlock.Clear();
        // }
        // for(int k=0; k<linkBlock.Count; k++){   //k번 째 블록모음의 {0,1,2,5}
        //     linkBlockArray = linkBlock[k].Split(',').Select(int.Parse).ToArray();
        //     for(int l=0;l<linkBlockArray.Length;l++){   //l번 째 블록에 다른 블록 집어넣어줌. {0}
        //         for(int m=0; m<linkBlockArray.Length;m++){                  // 0빼고 1,2,5
        //             if(linkBlockArray[l]!=linkBlockArray[m]){
                        
        //                 blocks[linkBlockArray[l]].linkedBlock.Add(GameObject.Find("Blocks").
        //                 transform.Find("Slot ("+linkBlockArray[m].ToString()+")").gameObject.
        //                 GetComponent<Block>());
        //                 //Debug.Log(linkBlockArray[l]+"번 블록에 "+linkBlockArray[m]+"번 블록 연결 시킴");

        //                 blocks[linkBlockArray[l]].vessel = vessels[k];
        //             }
        //         }
        //         //if(l==0){
        //             blocks[linkBlockArray[0]].vessel = vessels[k];
                    
        //             //Debug.Log(linkBlockArray[0]+"번 블록의 vessel은 "+k+"번");
        //         //}
        //     }
        // }
        
        // for(int j=0; j<blocks.Length;j++){
            
        //     for(int i=0; i<blocks[j].linkedBlock.Count;i++){
        //         blocks[j].linkedNum.Add(blocks[j].linkedBlock[i].num);
        //     }
        // }

        //if(activateRelocating){

            for(int a=0; a<blocks.Length; a++){
                blocks[a].RelocateAtFirst();
            }
            foreach(Block2 b in fakeBlocks){
                b.RelocateAtFirst();
            }
        //}

        

    }
    // void FixedUpdate()
    // {
    //     for(int i=0; i<slots.Length; i++){
    //         checkAll[i]=slots[i].check;
    //     }
    // }

    public void CheckGame(){
        for(int i=0; i<slots.Length; i++){
            if(slots[i].check){

                checkAll[i] = slots[i].check;
            }
            else return;
            
        }

        Debug.Log("퍼즐 완성");

        //game18.passGame();
        AudioManager.instance.Play("success0");
        StartCoroutine(GameManager.instance.GameSuccess());
        game18.Invoke("passGame",GameManager.instance.successWaitTime);
    }
}
