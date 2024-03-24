using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class game25 : MonoBehaviour
{
    public GameObject beforeImage;
    public GameObject afterImage;
    public GameObject feather;


    private GameManager theGame;
    private DatabaseManager theDB;
    private PuzzleManager thePuzzle;
    private PlayerManager thePlayer;
    void Start()               
    {
        theGame= GameManager.instance;
        theDB = DatabaseManager.instance;
        thePuzzle = PuzzleManager.instance;
        thePlayer = PlayerManager.instance;

    }
    void OnEnable(){

        if(DatabaseManager.instance.gameOverList.Contains(25)){
            beforeImage.SetActive(false);
            afterImage.SetActive(true);
            feather.SetActive(true);
        }
        else{
            
            beforeImage.SetActive(true);
            afterImage.SetActive(false);
            feather.SetActive(false);
        }
    }
    public void exitGame(){
        Puzzle3.instance.inMain = true;
        Puzzle3.instance.SpritesOn();
            AudioManager.instance.Play("button20");
        gameObject.SetActive(false);
        thePlayer.isPlayingGame = false;
    }
    public void PassGame(){
        
        thePlayer.isPlayingGame = false;
        theDB.gameOverList.Add(25);
        //StartCoroutine(Puzzle3.instance.FinishCardGame());
        gameObject.SetActive(false);
    }
    public void ResetGame(){
        
    }
    public void PutFeather(){
        if(theDB.OnActivated[12]){//선인장꽂았다
            StartCoroutine(PF());
        }
    }

    IEnumerator PF(){

            theDB.gameOverList.Add(25);
            theDB.OnActivated[12] = false; 
            CursorManager.instance.RecoverCursor();
            Inventory.instance.RemoveItem(12);
            //cactusUsed.SetActive(true);
            AudioManager.instance.Play("key0");
            ObjectManager.instance.ImageFadeIn(feather.GetComponent<Image>());
            yield return new WaitForSeconds(2f);
            AudioManager.instance.Play("getitem0");
            ObjectManager.instance.ImageFadeIn(afterImage.GetComponent<Image>());
            ObjectManager.instance.ImageFadeOut(beforeImage.GetComponent<Image>());
            //theDM.ShowDialogue(dialogue_3);
            //yield return new WaitUntil(()=> !theDM.talking); 
            StartCoroutine(Puzzle3.instance.PutFeather());
    }

    
}
