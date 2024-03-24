using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MainMenuLoad : MonoBehaviour
{
    public static MainMenuLoad instance;
    public Transform slotObject;
    public SettingSlot[] slots;

    private string nameStr;
    private string timeStr;
    private string temp;
    //private int lastSaveNum;

    private DatabaseManager theDB;
    private SaveNLoad theSL;
    private FileStream file;

    //private int fileCount = 3;                              //세이브 저장 가능 갯수
    

    //private PlayerManager thePlayer;

    void Start(){

        slots = slotObject.GetComponentsInChildren<SettingSlot>();
        theDB = FindObjectOfType<DatabaseManager>();
        theSL = FindObjectOfType<SaveNLoad>();
//        Debug.Log(theSL.data.saveName);

        theSL.loadList();
        // if(theSL.loadList()==0){

        // }
        //for(int i=1; i<=3;i++){
        //    slots[i].name_text.text =theSL.data.saveName[i];
        //    slots[i].time_text.text =theSL.data.saveTime[i];
        //}
        //세이브파일 다 열어서 lastSaveNum 비교 가 아니라 하나열어서 그대로 걍 대입

        /*
        for(int i=1; i<=fileCount; i++){

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/SaveFile" + i +".dat", FileMode.Open);
            
            if(file != null && file.Length >0){
                data =(Data)bf.Deserialize(file);
                
                slots[i].name_text.text = data.saveName[i-1];
                slots[i].time_text.text = data.saveTime[i-1];

            }
            else{
                
                slots[i].name_text.text = "";
                slots[i].time_text.text = "";
                Debug.Log(i +"번에 저장된 파일없음");  
            } 

            file.Close();
        }*/
        /*
        Debug.Log(theDB.lastSaveNum);
        lastSaveNum = theDB.lastSaveNum;
        if(lastSaveNum!=0){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/SaveFile" + lastSaveNum +".dat", FileMode.Open);

            if(file != null && file.Length >0){
                data =(Data)bf.Deserialize(file);
                


        }   */
        //
        //FileStream file = File.Open(Application.dataPath + "/SaveFile1.dat", FileMode.Open);
        //

        //if(file != null && file.Length >0){






        

    }


}
