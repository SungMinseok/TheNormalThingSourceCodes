using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game1 : MonoBehaviour
{   
    public static game1 instance;
    public string answer;
    public GameObject whatGame;
    public GameObject checkButton;
    public GameObject afterCheckImage;
    public GameObject candy;
    public RotatingPuzzle[] puzzles;
    private string temp = "";
    private GameManager theGame;
    private DatabaseManager theDB;
    public int[] checkValue;
    // Start is called before the first frame update
    void Start()                //퍼즐 랜덤위치로 돌려주자
    {
        instance = this;
        theGame= GameManager.instance;
        theDB = DatabaseManager.instance;

        //theGame.playing = true;
        //GoRandom();
        // if(theDB.gameOverList.Contains(1)) {
            
        //         checkButton.SetActive(false);
        //         afterCheckImage.SetActive(true);
        // }

    }
    void OnEnable(){
        
        if(DatabaseManager.instance.gameOverList.Contains(1)){
            checkButton.SetActive(false);
            afterCheckImage.SetActive(true);
            if(Inventory.instance.SearchItem(2)){
                candy.SetActive(false);
            }
            else{
                candy.SetActive(true);
            }
        }
        else{
            checkButton.SetActive(true);
            afterCheckImage.SetActive(false);
        }
    }
    public void GoRandom(){

        for(int i =0 ; i< puzzles.Length; i++){
            int ranNum = Random.Range(0,3);
            puzzles[i].realRotation = 90f*(float)ranNum;
            puzzles[i].value = ranNum;
        }
    }

    // Update is called once per frame
    public void checkGame()
    {
        for(int i=0; i<puzzles.Length; i++){
            int ranNum = Random.Range(0,2);
            AudioManager.instance.Play("flowereyes"+ranNum.ToString());
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
            if(whatGame!=null)
                whatGame.SetActive(false);


            if(checkButton!=null){

                checkButton.SetActive(false);
                afterCheckImage.SetActive(true);
                if(!theDB.gameOverList.Contains(1)){
                    
                    AudioManager.instance.Play("success0");
                    candy.SetActive(true);
                }
            }






            //Debug.Log(temp);
            temp="";
            //Debug.Log("정답!");
            theDB.gameOverList.Add(1);
            //theGame.playing = false;
            //theGame.pass[0] = true;
        }
        else if (answer != temp){
            
            //Debug.Log(temp);
            
            temp="";
            //Debug.Log("오답!");
        }


    }

    public void exitGame(){
        
        if(whatGame!=null)
            whatGame.SetActive(false);

    }
    /*
    public void passGame(){
        
        if(whatGame!=null)
            whatGame.SetActive(false);
        Debug.Log(temp);
        temp="";
        Debug.Log("정답!(test)");
        theGame.playing = false;
        theGame.pass[0] = true;
    }*/
}
