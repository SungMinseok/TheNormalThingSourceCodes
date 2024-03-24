using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class game6 : MonoBehaviour
{
    public static game6 instance;
    // Start is called before the first frame update
    public BoxCollider2D[] blocks;

    
    //private Transform[] originPos;
    private DatabaseManager theDB;
    private GameManager theGame;
    private PlayerManager thePlayer;
    public GameObject counter;
    public Text countText;
    public int count;
    void Awake()
    {
        instance = this;
        theDB = DatabaseManager.instance;
        theGame = GameManager.instance;
        thePlayer= PlayerManager.instance;
        
        //Debug.Log("가능?");
        //DisableColliders();
        thePlayer.isPlayingGame = true;
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
            AudioManager.instance.Play("button20");
        gameObject.SetActive(false);
        thePlayer.notMove = false;
        thePlayer.isPlayingGame = false;
        EnableColliders();
        //Puzzle1.buttonOn();
        //thePuzzle.StopAllCoroutines();

    }
    public void passGame(){

#if ADD_ACH
            if(count<=13){
                
            Debug.Log("업적");
            
            if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(14);
            }
#endif
        //Puzzle1.buttonOn();
        
        theDB.gameOverList.Add(6);
        if(theDB.gameOverList.Contains(5)&&theDB.gameOverList.Contains(6)){
            theDB.doorEnabledList.Add(16);
        }
        gameObject.SetActive(false);
        //theGame.playing = false;
        thePlayer.isPlayingGame = false;
        //thePlayer.notMove = false;
        Trig25.instance.StartCoroutine("FinishGame");
        //theGame.pass[2] = true;
        EnableColliders();
        

        //Puzzle1.FinishGame();
    }
    public void ResetGame(){
#if ADD_ACH
        count = 0;
        countText.text = "0";
#endif
            AudioManager.instance.Play("button20");
        for(int i=0;i<11;i++){
            blocks[i].GetComponent<Ray>().ResetPos();
        }
    }
    
    public void EnableColliders(){
        
        BoxCollider2D[] collider2Ds=FindObjectsOfType(typeof(BoxCollider2D)) as BoxCollider2D[];
        EdgeCollider2D[] collider2Ds2=FindObjectsOfType(typeof(EdgeCollider2D)) as EdgeCollider2D[];
        CircleCollider2D[] collider2Ds3=FindObjectsOfType(typeof(CircleCollider2D)) as CircleCollider2D[];
        PolygonCollider2D[] collider2Ds4=FindObjectsOfType(typeof(PolygonCollider2D)) as PolygonCollider2D[];
    
        foreach(BoxCollider2D col in collider2Ds){
            col.enabled = true;
        }
        foreach(EdgeCollider2D col2 in collider2Ds2){
            col2.enabled = true;
        }
        foreach(CircleCollider2D col3 in collider2Ds3){
            col3.enabled = true;
        }
        foreach(PolygonCollider2D col4 in collider2Ds4){
            col4.enabled = true;
        }

        // foreach(BoxCollider2D colRe in blocks){
        //     colRe.enabled =false;
        // }
    }

    public void DisableColliders(){

        BoxCollider2D[] collider2Ds=FindObjectsOfType(typeof(BoxCollider2D)) as BoxCollider2D[];
        EdgeCollider2D[] collider2Ds2=FindObjectsOfType(typeof(EdgeCollider2D)) as EdgeCollider2D[];
        CircleCollider2D[] collider2Ds3=FindObjectsOfType(typeof(CircleCollider2D)) as CircleCollider2D[];
        PolygonCollider2D[] collider2Ds4=FindObjectsOfType(typeof(PolygonCollider2D)) as PolygonCollider2D[];
    
        foreach(BoxCollider2D col in collider2Ds){
            col.enabled = false;
        }
        foreach(EdgeCollider2D col2 in collider2Ds2){
            col2.enabled = false;
        }
        foreach(CircleCollider2D col3 in collider2Ds3){
            col3.enabled = false;
        }
        foreach(PolygonCollider2D col4 in collider2Ds4){
            col4.enabled = false;
        }

        foreach(BoxCollider2D colRe in blocks){
            colRe.enabled =true;
        }
    }
}

