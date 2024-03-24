using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class game19 : MonoBehaviour
{



    private int nowPhase=0;
    public string[] answer = new string[3]{"213114","2321133","1212111221"};
    public string temp="";


    public Image first;
    public Image second;
    public GameObject firstBtn, secondBtn,mouth, peanut;
    public GameObject rubyHands, parrotHands;
    public Button[] thirdBtn;
    //public SpriteRenderer third;
    private GameManager theGame;
    private DatabaseManager theDB;
    private PuzzleManager thePuzzle;
    private PlayerManager thePlayer;
    public Button exitBtn;
    
    void Start()               
    {
        //instance = this;
        theGame= GameManager.instance;
        theDB = DatabaseManager.instance;
        thePuzzle = PuzzleManager.instance;
        thePlayer = PlayerManager.instance;

        answer = new string[3]{"213114","2321133","1212111221"};

        // if(theDB.gameOverList.Contains(21)){
        //     first.gameObject.SetActive(false);
        //     firstBtn.SetActive(false);
        //     second.gameObject.SetActive(false);
        //     secondBtn.SetActive(false);
        //     mouth.SetActive(true);
        //     peanut.SetActive(true);
        //     nowPhase=3;
        // }
        // else if(theDB.gameOverList.Contains(20)){
        //     first.gameObject.SetActive(false);
        //     firstBtn.SetActive(false);
        //     second.gameObject.SetActive(false);
        //     secondBtn.SetActive(false);
        //     nowPhase=2;
        // }
        // else if(theDB.gameOverList.Contains(19)){
        //     first.gameObject.SetActive(false);
        //     firstBtn.SetActive(false);
        //     nowPhase=1;
        // }
        // else{
            
        //     mouth.SetActive(false);
        //     peanut.SetActive(false);
        //     second.gameObject.SetActive(true);
        //     second.color = new Color(1,1,1,1);
        //     secondBtn.SetActive(true);
        //     first.gameObject.SetActive(true);
        //     first.color = new Color(1,1,1,1);
        //     firstBtn.SetActive(true);
        //     nowPhase=0;
        // }
    }
    void OnEnable(){
            thirdBtn[0].interactable = true;
            thirdBtn[1].interactable = true;
        parrotHands.SetActive(false);
        if(DatabaseManager.instance.gameOverList.Contains(21)){
            first.gameObject.SetActive(false);
            firstBtn.SetActive(false);
            second.gameObject.SetActive(false);
            secondBtn.SetActive(false);
            mouth.SetActive(true);
            peanut.SetActive(true);
            thirdBtn[0].interactable = false;
            thirdBtn[1].interactable = false;
            nowPhase=3;
        }
        else if(DatabaseManager.instance.gameOverList.Contains(20)){
            first.gameObject.SetActive(false);
            firstBtn.SetActive(false);
            second.gameObject.SetActive(false);
            secondBtn.SetActive(false);
            nowPhase=2;
        }
        else if(DatabaseManager.instance.gameOverList.Contains(19)){
            first.gameObject.SetActive(false);
            firstBtn.SetActive(false);
            nowPhase=1;
        }
        else{
            
            mouth.SetActive(false);
            peanut.SetActive(false);
            second.gameObject.SetActive(true);
            second.color = new Color(1,1,1,1);
            secondBtn.SetActive(true);
            first.gameObject.SetActive(true);
            first.color = new Color(1,1,1,1);
            firstBtn.SetActive(true);
            nowPhase=0;
        }
    }
    public void ExitGame(){
            AudioManager.instance.Play("button20");
        gameObject.SetActive(false);
        thePlayer.isPlayingGame = false;
        thePlayer.notMove = false;
    }
    public void PassGame(){

        theDB.gameOverList.Add(22);
        theDB.gameOverList.Add(21);
        theDB.gameOverList.Add(20);
        theDB.gameOverList.Add(19);
        gameObject.SetActive(false);
        thePlayer.isPlayingGame = false;
        thePlayer.notMove = false;

        Trig26.instance.StartCoroutine("EventCoroutine");
        //Trig59.instance.StartCoroutine("FinishGame");
    }
    public void RefreshGame(){
            AudioManager.instance.Play("button20");
        //nowPhase=0;
        //Debug.Log("초기화");
        temp="";
    }
    public void BtnClick(int i){
        AudioManager.instance.Play("puzzle0");
        temp += (i+1).ToString();
        //Debug.Log(temp + ", answer : "+answer[nowPhase]);
        if(answer[nowPhase]==temp){
            //Debug.Log("정답");
            temp="";
            //0, 1, 2        
            nowPhase++;
            switch(nowPhase){
                case 1:
                    ObjectManager.instance.ImageFadeOut(first,0.1f);
                    firstBtn.SetActive(false);
                    theDB.gameOverList.Add(19);
                    Trig59.instance.mats[0].SetActive(false);
                    Trig59.instance.mats[1].SetActive(true);
            AudioManager.instance.Play("mattouch");
                    //first.SetActive(false);
                    //second.SetActive(true);
                    break;
                case 2:
                    ObjectManager.instance.ImageFadeOut(second,0.1f);
                    secondBtn.SetActive(false);
                    theDB.gameOverList.Add(20);
                    Trig59.instance.mats[1].SetActive(false);
                    Trig59.instance.mats[2].SetActive(true);
            AudioManager.instance.Play("wood1");
                    //second.SetActive(false);
                    //third.SetActive(true);
                    break;
                case 3:
                    theDB.gameOverList.Add(21);
            AudioManager.instance.Play("matopen");
                    Trig59.instance.mats[2].SetActive(false);
                    Trig59.instance.mats[3].SetActive(true);
                    mouth.SetActive(true);
                    peanut.SetActive(true);
                    thirdBtn[0].interactable = false;
                    thirdBtn[1].interactable = false;
                    
                    //PassGame();
                    break;
            }
        
        }
    }
    public void PeanutClick(){
        //Debug.Log("땅콩");
                    theDB.gameOverList.Add(22);
        first.gameObject.SetActive(false);
        firstBtn.SetActive(false);
        second.gameObject.SetActive(false);
        secondBtn.SetActive(false);
        mouth.SetActive(true);
        peanut.SetActive(true);
        StartCoroutine(PeanutClickCoroutine());
    }
    IEnumerator PeanutClickCoroutine(){
        exitBtn.interactable = false;
        rubyHands.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        peanut.SetActive(false);
        parrotHands.SetActive(true);
        //yield return new WaitForSeconds(0.7f);
        peanut.SetActive(false);
        yield return new WaitForSeconds(1.8f);
        Fade2Manager.instance.FadeOut();
        ObjectManager.instance.FadeOut(rubyHands.GetComponent<SpriteRenderer>());
        yield return new WaitForSeconds(1f);

        PassGame();
        Fade2Manager.instance.FadeIn();
        //trig73.SetActive(true);
    }
    public void CheckStates(){
        
        
    }
}
