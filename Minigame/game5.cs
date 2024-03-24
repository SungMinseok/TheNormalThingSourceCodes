using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class game5 : MonoBehaviour
{
    //public Puzzle1 Puzzle1;
    //public Control control;
    //private Puzzle1 thePuzzle;
    public Control3 control;
    private GameManager theGame;
    private DatabaseManager theDB;
    private PuzzleManager thePuzzle;
    private PlayerManager thePlayer;
    void Start()               
    {
        //instance = this;
        theGame= GameManager.instance;
        theDB = DatabaseManager.instance;
        thePuzzle = PuzzleManager.instance;
        thePlayer = PlayerManager.instance;
        //thePuzzle = Puzzle1.instance;


    }
    public void exitGame(){
        AudioManager.instance.Play("button20");
        gameObject.SetActive(false);
        thePlayer.isPlayingGame = false;
        thePlayer.notMove = false;
    }
    public void passGame(){

        theDB.gameOverList.Add(5);
        if(theDB.gameOverList.Contains(5)&&theDB.gameOverList.Contains(6)){
            theDB.doorEnabledList.Add(16);
        }
        gameObject.SetActive(false);
        thePlayer.isPlayingGame = false;
        Trig23.instance.bifur=1;
        Trig23.instance.flag = true;
        thePlayer.exc.SetBool("on",false);
        thePlayer.canInteractWith = 0;
        Trig23.instance.StartCoroutine("EventCoroutine");   //겜 통과후 선택지 발생
    }
    public void ResetGame(){
        AudioManager.instance.Play("puzzle0");
        for(int i=0; i<control.blocks.Length; i++){
            control.blocks[i].nowNum = -1;
            control.blocks[i].check = false;
            control.blocks[i].Relocate();
            control.blocks[i].canvasGroup.blocksRaycasts = true;
            control.blocks[i].canvasGroup.alpha = 1f;
            control.slots[i].check = false;
        }
    }
    public void Update(){
    }
}
