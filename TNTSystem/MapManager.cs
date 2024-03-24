using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
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
    }
    public GameObject[] blur;
    public GameObject[] point;
    public GameObject[] text;
    public GameObject shortcut;
    //protected List<GameObject> blurList = new List<GameObject>();
    private PlayerManager thePlayer;
    private DatabaseManager theDB;
    private BookManager theBook;
    //[HideInInspector]
    public bool[] doorStateBackup;
    public List<int> doorStateBackupList;
    //[HideInInspector]
    public bool ForceEnableDoors;

    public bool blurAnim;
    void Start(){
        thePlayer = FindObjectOfType<PlayerManager>();
        theDB = FindObjectOfType<DatabaseManager>();
        theBook= FindObjectOfType<BookManager>();
        /*
        for(int i=0;i<blur.Length;i++){
            blurList.Add(blur[i]);
        }*/
        doorStateBackup = new bool[50];
        doorStateBackup[3] = true;
        doorStateBackup[5] = true;
        doorStateBackup[9] = true;
        doorStateBackup[11] = true;
        doorStateBackup[13] = true;
        doorStateBackup[15] = true;
        doorStateBackup[17] = true;
        doorStateBackup[18] = true;
        doorStateBackup[19] = true;
        doorStateBackup[22] = true;
        doorStateBackup[23] = true;
        doorStateBackup[25] = true;
        doorStateBackup[26] = true;
        doorStateBackup[28] = true;
        doorStateBackup[29] = true;
        doorStateBackup[30] = true;
        doorStateBackup[31] = true;
        doorStateBackup[32] = true;
        doorStateBackup[34] = true;
        doorStateBackup[35] = true;
        doorStateBackup[37] = true;
        doorStateBackup[39] = true;
        doorStateBackup[40] = true;
    }

    public void MapUpdate(){
        BlurUpdate();
        WhereUpdate();
    }
    /*public void BlurUpdate(){               //DB의 진행수 만큼 블러 제거함.
        //if(!blur[theDB.progress].activeSelf){ 
            for(int i=0; i<=theDB.progress ; i++){
                blur[i].SetActive(false);
                Debug.Log("theDB.progress : "+theDB.progress);
                Debug.Log("blur["+ i +"] false");
            }
        //}   
    }*/

    public void WhereUpdate(){              //map 포인트 위치 설정
        
        for(int i=0; i<point.Length ; i++){
            point[i].SetActive(false);
        }
        switch(thePlayer.currentMapName){
            case "start" :
                point[0].SetActive(true);
                break;
            case "cabin" :
                point[0].SetActive(true);
                break;
            case "catwood" :
                point[1].SetActive(true);
                break;
            case "catwood2" :
                point[1].SetActive(true);
                break;
            case "ch2" :
                point[2].SetActive(true);
                break;
            case "ch3" :
                point[3].SetActive(true);
                break;
            case "cornerwood" :
                point[4].SetActive(true);
                break;
            case "camp" :
                point[5].SetActive(true);
                break;
            case "middlewood" :
                point[6].SetActive(true);
                break;
            case "village" :
                point[7].SetActive(true);
                break;
            case "lake" :
                point[8].SetActive(true);
                break;
            case "lakein" :
                point[9].SetActive(true);
                break;
            case "lakeout" :
                point[9].SetActive(true);
                break;
            case "rainingforest" :
                point[10].SetActive(true);
                break;
            case "parrothidden" :
                point[10].SetActive(true);
                break;
            case "thunderingforest" :
                point[11].SetActive(true);
                break;
            case "mazein" :
                point[11].SetActive(true);
                break;
            case "maze" :
                point[12].SetActive(true);
                break;
            case "mazeout" :
                point[12].SetActive(true);
                break;
            case "end" :
                point[13].SetActive(true);
                break;
            
        }
    }

    // public void RemoveBlur(int blurNum){            // 맵 여는 트리거에서
    //     if(!theBook.updateIcon.activeSelf)
    //         theBook.updateIcon.SetActive(true);                         //update느낌표 온
        
    //     for(int i=0; i<=blurNum; i++){
    //         blur[i].SetActive(false);
    //     }

    //     //theDB.progress = blurNum;
    // }
    public void BlurUpdate(){

        if(blurAnim){
            blurAnim = false;
            BlurDisappearAnim();
        }
        else{

            if(theDB.progress<=7){
                
                if(blur[theDB.progress].activeSelf){
                    // if(!theBook.updateIcon.activeSelf)
                    //     theBook.updateIcon.SetActive(true);
                    for(int i=0; i<theDB.progress ; i++){
                        blur[i].SetActive(false);
                        text[i].SetActive(true);
                    }
                }
            }
            else if(theDB.progress ==8){
                for(int i=0; i<theDB.progress ; i++){
                    blur[i].SetActive(false);
                    text[i].SetActive(true);
                }
            }
        }


        if(theDB.trigOverList.Contains(43)){
            shortcut.SetActive(true);
        }
    }

    public void ResetMaps(){
        for(int i=0; i<blur.Length; i++){
            blur[i].SetActive(true);
        }
        for(int i=0; i<text.Length; i++){
            text[i].SetActive(false);
        }
    }

    public void BlurDisappearAnim(){
        text[0].SetActive(true);
        for(int i=0; i<theDB.progress-1 ; i++){
            blur[i].SetActive(false);
            text[i+1].SetActive(true);
        }
        AudioManager.instance.Play("air0");
        ObjectManager.instance.ImageFadeOut(blur[theDB.progress-1].GetComponent<Image>(),0.02f);
    }
}
