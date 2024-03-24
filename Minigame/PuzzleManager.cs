using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;
    private PlayerManager thePlayer;
    
    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else{
            Destroy(this.gameObject);
        }
        //thePlayer = PlayerManager.instance;
    }
    //public GameObject treeFace;
    public GameObject[] puzzleNum;
    public bool puzzleCheck;
    
    void Start(){
    }

    // void FixedUpdate(){
    //     // if(puzzleNum[1].activeSelf){
    //     //     treeFace.GetComponent<Animator>().Set
    //     // }
    //     if(!puzzleCheck&&DatabaseManager.instance.isPlayingPuzzle2&&!puzzleNum[2].activeSelf){
    //         //Debug.Log("인식성공");
    //         puzzleCheck = true;
    //         PlayerManager.instance.isPlayingPuzzle = true;
    //         PlayerManager.instance.notMove = true;
    //         puzzleNum[2].gameObject.SetActive(true);
    //         //theDB.isPlayingPuzzle2 = true;
    //     }
        
    // }

    public void SetPuzzle2(){
        
        if(!puzzleCheck&&DatabaseManager.instance.isPlayingPuzzle2&&!puzzleNum[2].activeSelf){
            //Debug.Log("인식성공");
            puzzleCheck = true;
            PlayerManager.instance.isPlayingPuzzle = true;
            PlayerManager.instance.notMove = true;
            puzzleNum[2].gameObject.SetActive(true);
            //theDB.isPlayingPuzzle2 = true;
        }
    }


}
