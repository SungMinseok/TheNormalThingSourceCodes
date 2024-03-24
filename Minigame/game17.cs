using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class game17 : MonoBehaviour
{
    public static game17 instance;
    public Sprite[] numbers;
    public Image[] slots = new Image[4];
    public int[] numInSlot = new int[4]{0,0,0,0};
    public string answer = "3865";
    private string answerCheck;

    private DatabaseManager theDB;
    private GameManager theGame;
    private PlayerManager thePlayer;
    void Start()
    {
        instance = this;
        theDB = DatabaseManager.instance;
        theGame = GameManager.instance;
        thePlayer= PlayerManager.instance;
    }

    public void exitGame(){
            AudioManager.instance.Play("button20");
        gameObject.SetActive(false);
        thePlayer.notMove = false;
        thePlayer.isPlayingGame = false;

    }
    public void passGame(){
#if ADD_ACH
        Debug.Log("업적24");
        if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(24);
#endif
        theDB.gameOverList.Add(17);
        //theDB.doorEnabledList.Add(16);
        gameObject.SetActive(false);
        thePlayer.isPlayingGame = false;
        //thePlayer.notMove = false;
        Trig31.instance.StartCoroutine("FinishGame");
        //theGame.pass[2] = true;

        //Puzzle1.FinishGame();
    }
    public void ResetGame(){
        AudioManager.instance.Play("puzzle0");
        for(int i=0;i<4;i++){
            numInSlot[i] = 0;
            slots[i].sprite = numbers[0];
        }
    }
    
    public void SlotChange(int slotNum){
        AudioManager.instance.Play("puzzle0");
        numInSlot[slotNum] = numInSlot[slotNum]<9 ? ++numInSlot[slotNum] : 0;
        slots[slotNum].sprite = numbers[numInSlot[slotNum]];
        //Debug.Log(numInSlot[slotNum]);

        SlotCheck();
    }

    public void SlotCheck(){
        foreach(int a in numInSlot){
            answerCheck += a.ToString();
        }
        if(answer==answerCheck){
            AudioManager.instance.Play("bolt0");
            //passGame();
            AudioManager.instance.Play("success0");
            GameManager.instance.GameSuccessTrig();
            Invoke("passGame",GameManager.instance.successWaitTime);
        }
        else answerCheck = "";
    }
}

