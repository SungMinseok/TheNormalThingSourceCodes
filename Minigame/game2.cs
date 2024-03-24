using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class game2 : MonoBehaviour
{
    //public static game2 instance;
    public Puzzle1 Puzzle1;
    public Control control;
    //private Puzzle1 thePuzzle;
    private GameManager theGame;
    private DatabaseManager theDB;
    private PuzzleManager thePuzzle;
    private PlayerManager thePlayer;
    public GameObject counter;
    public Text countText;
    public float count;
    void Start()               
    {
        //instance = this;
        theGame= GameManager.instance;
        theDB = DatabaseManager.instance;
        thePuzzle = PuzzleManager.instance;
        thePlayer = PlayerManager.instance;
        //thePuzzle = Puzzle1.instance;

        //thePlayer.isPlayingGame = true;

    }
    void OnEnable(){
        PlayerManager.instance.isPlayingGame = true;
#if ADD_ACH
        counter.SetActive(true);
        count = 0;
        countText.text = "0";
#endif
    }
    public void exitGame(){
        Puzzle1.instance.inMain = true;
        thePlayer.isPlayingGame = false;
        if(theDB.puzzleOverList.Contains(1)) Puzzle1.instance.SpritesOn();
            AudioManager.instance.Play("button20");
        gameObject.SetActive(false);
        //thePuzzle.treeFace.SetActive(true);
        Puzzle1.buttonOn();
        //thePuzzle.StopAllCoroutines();

    }
    public void passGame(){

        Puzzle1.instance.inMain = true;
        Puzzle1.buttonOn();
        //thePuzzle.treeFace.SetActive(true);
#if ADD_ACH
            if(count<=14){
                
            Debug.Log("업적");
            //Debug.Log("s : "+count);
            
            if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(11);
            }
#endif
        
        gameObject.SetActive(false);
            thePlayer.isPlayingGame = false;
            //theGame.pass[0] = true;
            //theDB.gameOverList.Add(0);
        //theGame.pass[2] = true;
        
        theDB.gameOverList.Add(2);

        Puzzle1.FinishGame();
    }
    public void ResetGame(){
#if ADD_ACH
        count = 0;
        countText.text = "0";
#endif
            AudioManager.instance.Play("button20");
        for(int i=0;i<control.blocks.Length;i++){
            control.blocks[i].enabled = true;
            //if(control.blocks[i].nowNum!=-1){
                    
                control.blocks[i].Relocate();
                control.blocks[i].canvasGroup.blocksRaycasts = true;
                control.blocks[i].canvasGroup.alpha = 1f;
                control.blocks[i].nowNum=-1;
                control.blocks[i].check=false;
            //}

            if(!control.blocks[i].fixedBlock){
                
                control.slots[i].check=false;
            }
            else{
                
                control.blocks[i].enabled = false;
            }
        }
    }
        
#if ADD_ACH
    void FixedUpdate(){
        count += Time.deltaTime;
        countText.text = count.ToString("N0");
    }
#endif
    // void Update(){
        
    //     if(Input.GetKeyDown(KeyCode.Escape)&&!BookManager.instance.ReadableIsOn()){
    //         exitGame();
    //     }
    // }
}
