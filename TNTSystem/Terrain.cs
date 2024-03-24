using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Terrain : MonoBehaviour
{
    public bool isMud;
    public bool isPond;
    public bool nearMachine;
    public bool nearFire;
    public bool flag;
    private PlayerManager thePlayer;
    private AudioManager theAudio;

    void Start(){
        thePlayer = PlayerManager.instance;
        theAudio = AudioManager.instance;
    }


    private void OnTriggerStay2D(Collider2D collision){
        //if(theDB.trigOverList.Contains(24)&&!GameManager.instance.playing&&!theDB.gameOverList.Contains(6)){
        //if(theDB.trigOverList.Contains(3)){\
        if(collision.gameObject.name == "Player"){
            if(isMud&&!thePlayer.onMud){

                ResetTerrainState();
                thePlayer.onMud = true;
                
            }
            else if(isPond&&!thePlayer.onPond){
                
                ResetTerrainState();
                thePlayer.onPond = true;
                
            }
            else if(nearMachine&&!flag){
                
                ResetTerrainState();
                theAudio.Play("machine0");
                flag = true;
                //thePlayer.nearMachine = true;
                
            }

            else if(nearFire&&!flag){
                
                ResetTerrainState();
                theAudio.Play("bonfire");
                flag = true;
                //thePlayer.nearFire = true;
                
            }


        }


    }
    void OnTriggerExit2D(Collider2D collision){
        //flag = false;
        ResetTerrainState();
    }

    public void ResetTerrainState(){
        thePlayer.onMud = false;
        thePlayer.onPond = false;
        flag = false;
        theAudio.Stop("machine0");
        theAudio.Stop("bonfire");
    }
}
