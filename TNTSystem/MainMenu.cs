using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Video;
public class MainMenu : MonoBehaviour
{   
    public static MainMenu instance;
    public GameObject popup;
    public GameObject error;    
    private DatabaseManager theDB;
    private SaveNLoad theSL;
    // [Header("체크시 로고 활성화")]
    // public bool logoOn;
    // public GameObject logo;
    




    //private DatabaseManager theDB;
    
    void Start(){
        instance = this;
        theSL = FindObjectOfType<SaveNLoad>();
        theDB = FindObjectOfType<DatabaseManager>();
        
    }
    
    public void StartPopup(){
        Button22();
        if(theSL.SaveFilesCheck()==0){
            popup.SetActive(true);
        }
        else{
            StartFirst();
        }
    }
    public void ExitPopup(){
        Button22();
        popup.SetActive(false);
    }

    public void isExit(){
        Button22();
        Application.Quit();
    }


    public void StartFirst(){                           //처음부터 시작
        
        Button22();
        StartCoroutine(StartFirstCoroutine());
    }
    public void SKIP(){
        
        theDB.phaseNum = 0;
        Button22();
        Destroy(BGMManager.instance.gameObject);
        SceneManager.LoadScene("intro");
    }
    IEnumerator StartFirstCoroutine(){
            //Debug.Log("1");




        theDB.phaseNum = 0;
        Button22();
        BGMManager.instance.FadeOutMusic();
        Fade2Manager.instance.FadeOut(0.02f,1f);
        yield return new WaitForSeconds(2f);
        
            //Debug.Log("2");
        //Destroy(AudioManager.instance.gameObject);
        Destroy(BGMManager.instance.gameObject);
        SceneManager.LoadScene("intro");

    }

    public void StartLoad(int num){
        
        Button22();
        FileInfo file = new FileInfo(Application.persistentDataPath + "/" + Application.version + "/SaveFile" + num +".dat");

        if(file.Exists){
            BGMManager.instance.FadeOutMusic();
            StartCoroutine(StartLoadCoroutine(num));
        }
        
        else error.SetActive(true);
    }
    IEnumerator StartLoadCoroutine(int num){
        theDB.phaseNum = num;
        Fade2Manager.instance.FadeOut(0.02f,1f);
        yield return new WaitForSeconds(2f);
        
        //Destroy(AudioManager.instance.gameObject);
        Destroy(BGMManager.instance.gameObject);
        SceneManager.LoadScene("Loading");
//        Debug.Log(num + "번 파일 로드 준비");
        
        //yield return new WaitForSeconds(2f);
        //theSL.CallLoad(num);
    }
    public void QuitGame(){
        // if(theDB.phaseNum!=0)
        //     SaveNLoad.instance.CallSave(theDB.phaseNum);
        
        Button22();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else 
            Application.Quit();
        #endif
    }

    
    public void Button22(){
        AudioManager.instance.Play("button22");
    }
}
