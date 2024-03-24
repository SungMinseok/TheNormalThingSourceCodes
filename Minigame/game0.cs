using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class game0 : MonoBehaviour
{
    public static game0 instance;
    public string answer;
    public GameObject whatGame;
    public RotatingPuzzle[] puzzles;
    private string temp = "";
    private GameManager theGame;
    private DatabaseManager theDB;
    private PlayerManager thePlayer;
    public Puzzle0 thePuzzle;
    public int[] checkValue;
    public GameObject counter;
    public Text countText;
    public int count;
    // Start is called before the first frame update
    void Start()                //퍼즐 랜덤위치로 돌려주자
    {
        instance = this;
        theGame= GameManager.instance;
        theDB = DatabaseManager.instance;
        thePlayer= PlayerManager.instance;

        //thePlayer.isPlayingGame = true;

        GoRandom();
        

    }
    void OnEnable(){
        PlayerManager.instance.isPlayingGame = true;
#if ADD_ACH
        counter.SetActive(true);
        count = 0;
        countText.text = "0";
#endif
    }
    public void GoRandom(){
        
        AudioManager.instance.Play("puzzle0");
        for(int i =0 ; i< puzzles.Length; i++){
            int ranNum = Random.Range(0,3);
            puzzles[i].realRotation = 90f*(float)ranNum;
            puzzles[i].value = ranNum;
        }
#if ADD_ACH
        count = 0;
        countText.text = "0";
#endif
    }

    // Update is called once per frame
    public void checkGame()
    {
#if ADD_ACH
        count ++;
        countText.text = count.ToString();
#endif
        for(int i=0; i<puzzles.Length; i++){
            
            AudioManager.instance.Play("puzzle0");
            //Debug.Log("i : " + i);
            checkValue[i] = puzzles[i].value;
            //Debug.Log("checkValue" + i +":" + checkValue[i]);
            //Debug.Log("puzzles.Length : " + i +":"  + puzzles.Length);
            //Debug.Log("puzzles[i].value : " + i +":"  +puzzles[i].value);
            if(temp.Length < puzzles.Length)                                // 숫자를 스트링으로 연결 "000000..."
                temp += checkValue[i].ToString();
            //Debug.Log("temp : " + temp);
        }
        
        if(answer == temp){
            AudioManager.instance.Play("success0");
            GameManager.instance.GameSuccessTrig();
            //StartCoroutine(GameManager.instance.GameSuccess());
            Invoke("passGame",GameManager.instance.successWaitTime);
            // thePuzzle.broken.SetActive(false);
            // thePuzzle.afterfixed.SetActive(true);
            // thePuzzle.buttonOn();


#if ADD_ACH
            if(count<=32){
                
            Debug.Log("업적");
            
            if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(9);
            }
#endif

            // 


            // //AudioManager.instance.Play("success0");
            // whatGame.SetActive(false);
            // Debug.Log(temp);
            // temp="";
            // Debug.Log("정답!");
            // thePlayer.isPlayingGame = false;
            // //theGame.pass[0] = true;
            // theDB.gameOverList.Add(0);
        }
        else if (answer != temp){
            
            //Debug.Log(temp);
            
            temp="";
            //Debug.Log("오답!");
        }


    }

    public void exitGame(){
        Puzzle0.instance.inMain = true;
            thePlayer.isPlayingGame = false;
        Puzzle0.instance.SpritesOn();
            AudioManager.instance.Play("button20");
        whatGame.SetActive(false);
        thePuzzle.buttonOn();
        //theGame.pass[0] = false;
        thePuzzle.StopAllCoroutines();

    }
    
    public void passGame(){
        
        Puzzle0.instance.inMain = true;
                //StartCoroutine(Puzzle0.instance.SuccessImplantation());
        Puzzle0.instance.StartCoroutine("SuccessImplantation");
        
        thePuzzle.broken.SetActive(false);
        thePuzzle.afterfixed.SetActive(true);
        //thePuzzle.buttonOn();
        
        whatGame.SetActive(false);
        //Debug.Log(temp);
        temp="";
        //Debug.Log("정답!(test)");
        // theGame.playing = false;
        // theGame.pass[0] = true;
        
            thePlayer.isPlayingGame = false;
            //theGame.pass[0] = true;
            theDB.gameOverList.Add(0);

    }
}
