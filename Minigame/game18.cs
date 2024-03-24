using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class game18 : MonoBehaviour
{
    //public Puzzle1 Puzzle1;
    public Control2 control;
    //private Puzzle1 thePuzzle;
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
    void OnEnable(){

        PlayerManager.instance.isPlayingPuzzle = true;
    }
    public void exitGame(){
            AudioManager.instance.Play("button20");
        gameObject.SetActive(false);
        thePlayer.isPlayingPuzzle = false;
        thePlayer.notMove = false;
        //thePuzzle.treeFace.SetActive(true);
        //Puzzle1.buttonOn();
        //thePuzzle.StopAllCoroutines();

    }
    public void passGame(){

        //Puzzle1.buttonOn();
        //thePuzzle.treeFace.SetActive(true);
        
        gameObject.SetActive(false);
        thePlayer.isPlayingPuzzle = false;
        theDB.gameOverList.Add(18);
        thePlayer.notMove = false;

        Trig38.instance.StartCoroutine("FinishGame");
    }
    public void ResetGame(){
        
        AudioManager.instance.Play("puzzle0");
        for(int i=0; i<control.blocks.Length; i++){
            control.slots[i].check = false;
            //if(control.blocks[i].nowNum != -1){
                control.blocks[i].nowNum = -1;
                control.blocks[i].check = false;
                control.blocks[i].canvasGroup.blocksRaycasts = true;
                control.blocks[i].canvasGroup.alpha = 1f;
                control.blocks[i].RelocateAtFirst();
            //}
        }
        for(int j=0; j<control.fakeBlocks.Length; j++){
            //if(control.fakeBlocks[j].nowNum != -1){
                control.fakeBlocks[j].nowNum = -1;
                control.fakeBlocks[j].check = false;
                control.fakeBlocks[j].canvasGroup.blocksRaycasts = true;
                control.fakeBlocks[j].canvasGroup.alpha = 1f;
                control.fakeBlocks[j].RelocateAtFirst();
            //control.slots[i].check = false;
            //}
        }
        
    }

    
}
