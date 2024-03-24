using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class game24 : MonoBehaviour
{
    public static game24 instance;
    public Transform[] cards;
    public int[] cardOrder = new int[5]{0,1,2,3,4};
    private int[] answer = new int[5]{0,1,2,3,4};
    private Vector3 tempPos = new Vector3(0,0,0);
    private int tempNum = 0;
    //private GameObject[] listedCards;   //랜덤셔플 된 카드
    private int stack;  //1되면 카드체인지

    int cardNum0 = -1;
    int cardNum1 = -1;







    private GameManager theGame;
    private DatabaseManager theDB;
    private PuzzleManager thePuzzle;
    private PlayerManager thePlayer;
    void Start()               
    {
        instance = this;
        theGame= GameManager.instance;
        theDB = DatabaseManager.instance;
        thePuzzle = PuzzleManager.instance;
        thePlayer = PlayerManager.instance;



    }
    void OnEnable(){

        Shuffle();
    }
    public void ExitGame(){
        Puzzle3.instance.inMain = true;
        Puzzle3.instance.SpritesOn();
            AudioManager.instance.Play("button20");
        gameObject.SetActive(false);
        thePlayer.isPlayingGame = false;
    }
    public void PassGame(){
//#if ADD_ACH
            if(!theDB.gameOverList.Contains(25)){
                
            Debug.Log("업적14");
            
            if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(14);
            }
//#endif
        
        Puzzle3.instance.inMain = true;
        thePlayer.isPlayingGame = false;
        theDB.gameOverList.Add(24);
        theDB.doorEnabledList.Add(33);
        //StartCoroutine(Puzzle3.instance.FinishCardGame());
        Puzzle3.instance.FinishCardGame();
        gameObject.SetActive(false);
        //Debug.Log("222");
    }
    public void ResetGame(){
        
    }

    public void Shuffle(){

        //int tempNum = Random.Range(0,5);

            AudioManager.instance.Play("button20");
        for(int i=0;i<5;i++){
            //listedCards[i] = cards[Random.Range(0,5)];
            tempPos = cards[i].position;
            tempNum = cardOrder[i];
            int ranNum = Random.Range(0,5);
            cards[i].position = cards[ranNum].position;
            cardOrder[i] = cardOrder[ranNum];
            cards[ranNum].position = tempPos;
            cardOrder[ranNum] = tempNum;

            //Debug.Log(i+"와 "+ranNum);

        }

        if(cardOrder.SequenceEqual(answer)){
            Shuffle();
        }
        //Debug.Log(cardOrder);
    }

    public void CardClick(int cardNum){
        //int cardNum1=0;
                AudioManager.instance.Play("card"+Random.Range(0,3).ToString());


        if(cardNum0!=cardNum){
            //Debug.Log(cardNum+"번 카드 클릭");
            if(stack==1){
                cards[cardNum0].GetComponent<Image>().color=new Color(1,1,1,1);
                cardNum1=cardNum;
                stack=0;
                CardChange();
                AnswerCheck();
            }
            else{
                cardNum0=cardNum;
                cards[cardNum0].GetComponent<Image>().color=new Color(0.5f,0.5f,0.5f,0.7f);
                stack++;
            }
        }
        else{
            
            cards[cardNum0].GetComponent<Image>().color=new Color(1,1,1,1);
            stack=0;
            cardNum0 = -1;
            cardNum1 = -1;
        }
    }
    public void CardChange(){
        tempPos=cards[cardNum0].position;
        cards[cardNum0].position = cards[cardNum1].position;
        cards[cardNum1].position = tempPos;

        tempNum=cardOrder[cardNum0];
        cardOrder[cardNum0] = cardOrder[cardNum1];
        cardOrder[cardNum1] = tempNum;

        cardNum0 = -1;
        cardNum1 = -1;

    }
    public void AnswerCheck(){
        if(cardOrder.SequenceEqual(answer)){
            AudioManager.instance.Play("success0");
            GameManager.instance.GameSuccessTrig();
            Invoke("PassGame",GameManager.instance.successWaitTime);
        }
    }
    
}
