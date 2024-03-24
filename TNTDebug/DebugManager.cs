using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;
    // [Header ("빌드 전 확인(배포용 : false)")]
    // public bool devMode;
    [HideInInspector]
    public bool onDebug;
    [HideInInspector]
    public bool onCol;
    [HideInInspector]
    public bool speedUp;
    // public int count_play;
    // public int count_clear;
    [Header ("데모 버전이면 마지막 문 번호 입력 (정식 : 0, 데모 : 14)")]
    public int isDemo;
    [Header ("체크 시 언노운 프리 버전")]
    public bool dobbyIsFree = false;
    //[Header ("버전(저장폴더설정)")]
    //public string ver;
    [Header ("수정 날짜 (테스트시)")]
    public string update ;

    private PlayerManager thePlayer;

    public GameObject warningDevMode;
    
    [System.Serializable]
    public class PlayerInfo{
        public int count_play;
        public int count_clear;
    }
    [SerializeField]
    public PlayerInfo playerInfo;
    public PlayerInfo playerInfoTemp;
    void Start(){
        if(instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else{
            Destroy(this.gameObject);
        }
        thePlayer = PlayerManager.instance;

        Directory.CreateDirectory(Application.persistentDataPath + "/" + Application.version + "/");

        LoadPlayerInfo("play");

        ShowInfo();
        
        #if DEV_MODE
        warningDevMode.SetActive(true);
        warningDevMode.GetComponent<Text>().text += "\nVER." + Application.version + " " + update;
        #endif
    }

    public void ShowInfo(){
        
        Debug.Log("Ver."+Application.version+"\nPlayCount : "+playerInfo.count_play+"\nClearCount : "+playerInfo.count_clear);
    }
    //게임 실행시 불러와서 플레이횟수 +1 해주고 바로 씀.
    //게임 클리어시 불러와서 클리어횟수 +1 해주고 바로 씀.
    public void LoadPlayerInfo(string what){


        BinaryFormatter bf = new BinaryFormatter();

        FileInfo file = new FileInfo(Application.persistentDataPath + "/PlayerInfo" +".dat");

        //if(file != null && file.Length >0){ //2번 이상 실행시
        if(file.Exists){
            FileStream openedFile = File.Open(Application.persistentDataPath + "/PlayerInfo" +".dat", FileMode.Open);

            playerInfo =(PlayerInfo)bf.Deserialize(openedFile);

        //Debug.Log("플레이횟수 더하기 전 : "+playerInfo.count_play);
            switch(what){
                case "play":
                    playerInfo.count_play += 1;
                    break;
                case "clear":
                    playerInfo.count_clear += 1;
                    break;
            }
            
        //Debug.Log("플레이횟수 : "+playerInfo.count_play);
            openedFile.Close();

            SavePlayerInfo();
        }
        else{   //처음 실행시

            FileStream openedFile = File.Create(Application.persistentDataPath + "/PlayerInfo" +".dat");

            playerInfo.count_play += 1;
            
            openedFile.Close();

            SavePlayerInfo();
        }
        
        // Debug.Log("플레이횟수 : "+playerInfo.count_play);
        // Debug.Log("클리어횟수 : "+playerInfo.count_clear);
    }

public void SavePlayerInfo(){
    
    BinaryFormatter bf = new BinaryFormatter();
    FileStream openedFile = File.Create(Application.persistentDataPath + "/PlayerInfo" +".dat");
    bf.Serialize(openedFile, playerInfo);
    openedFile.Close();
}
#if UNITY_EDITOR

    #region Debugging
    // [UnityEditor.MenuItem("DebugMode/Transportation", true, 1)]
    // [UnityEditor.MenuItem("DebugMode/Colliders", true, 2)]

    [UnityEditor.MenuItem("DebugMode/Transportation/ToStart #1")]
    static void ToStart(){
        //PlayerManager.instance.StopToTest();
        DebugManager.instance.onDebug = true;
        //if(DebugManager.instance.onDebug)
            Transportation("start");
        // ToggleColliders();
        // ToggleColliders();
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToCabin #2")]
    static void ToCabin(){
        //PlayerManager.instance.StopToTest();
        Transportation("cabin");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToCabin #2", true)]
    static bool Toggle1()
    {
        return PlayerManager.instance != null;
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToCatwood #3")]
    static void ToCatwood(){
        //PlayerManager.instance.StopToTest();
        Transportation("catwood");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToCatwood #3", true)]
    static bool Toggle2()
    {
        return PlayerManager.instance != null;
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToCatwood2 #4")]
    static void ToCatwood2(){
        //PlayerManager.instance.StopToTest();
        Transportation("catwood2");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToCatwood2 #4", true)]
    static bool Toggle3()
    {
        return PlayerManager.instance != null;
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToCh2 #5")]
    static void ToCh2(){
        //PlayerManager.instance.StopToTest();
        Transportation("ch2");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToCh2 #5", true)]
    static bool Toggle4()
    {
        return PlayerManager.instance != null;
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToCh3 #6")]
    static void ToCh3(){
        //PlayerManager.instance.StopToTest();
        Transportation("ch3");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToCh3 #6", true)]
    static bool Toggle5()
    {
        return PlayerManager.instance != null;
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToCornerwood #7")]
    static void ToCornerwood(){
        //PlayerManager.instance.StopToTest();
        Transportation("cornerwood");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToCornerwood #7", true)]
    static bool Toggle6()
    {
        return PlayerManager.instance != null;
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToCamp #8")]
    static void ToCamp(){
        //PlayerManager.instance.StopToTest();
        Transportation("camp");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToCamp #8", true)]
    static bool Toggle7()
    {
        return PlayerManager.instance != null;
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToMiddlewood #9")]
    static void ToMiddlewood(){
        //PlayerManager.instance.StopToTest();
        Transportation("middlewood");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToMiddlewood #9", true)]
    static bool Toggle8()
    {
        return PlayerManager.instance != null;
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToVillage #0")]
    static void ToVillage(){
        //PlayerManager.instance.StopToTest();
        Transportation("village");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToVillage #0", true)]
    static bool Toggle9()
    {
        return PlayerManager.instance != null;
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToLake #q")]
    static void ToLake(){
        //PlayerManager.instance.StopToTest();
        Transportation("lake");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToLake #q", true)]
    static bool Toggle10()
    {
        return PlayerManager.instance != null;
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToLakeIn #w")]
    static void ToLakeIn(){
        //PlayerManager.instance.StopToTest();
        Transportation("lakein");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToLakeIn #w", true)]
    static bool Toggle11()
    {
        return PlayerManager.instance != null;
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToLakeOut #e")]
    static void ToLakeOut(){
        //PlayerManager.instance.StopToTest();
        Transportation("lakeout");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToLakeOut #e", true)]
    static bool Toggle12()
    {
        return PlayerManager.instance != null;
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToRainingForest #r")]
    static void ToRainingForest(){
        //PlayerManager.instance.StopToTest();
        Transportation("rainingforest");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToRainingForest #r", true)]
    static bool Toggle13()
    {
        return PlayerManager.instance != null;
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToParrotHidden #t")]
    static void ToParrotHidden(){
        //PlayerManager.instance.StopToTest();
        Transportation("parrothidden");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToParrotHidden #t", true)]
    static bool Toggle14()
    {
        return PlayerManager.instance != null;
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToThunderingForest #y")]
    static void ToThunderingForest(){
        //PlayerManager.instance.StopToTest();
        Transportation("thunderingforest");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToThunderingForest #y", true)]
    static bool Toggle15()
    {
        return PlayerManager.instance != null;
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToMazeIn #u")]
    static void ToMazeIn(){
        //PlayerManager.instance.StopToTest();
        Transportation("mazein");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToMazeIn #u", true)]
    static bool Toggle16()
    {
        return PlayerManager.instance != null;
    }

    [UnityEditor.MenuItem("DebugMode/Transportation/ToMaze #i")]
    static void ToMaze(){
        //PlayerManager.instance.StopToTest();
        Transportation("maze");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToMaze #i", true)]
    static bool Toggle17()
    {
        return PlayerManager.instance != null;
    }

    [UnityEditor.MenuItem("DebugMode/Transportation/ToMazeOut #o")]
    static void ToMazeOut(){
        //PlayerManager.instance.StopToTest();
        Transportation("mazeout");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToMazeOut #o", true)]
    static bool Toggle18()
    {
        return PlayerManager.instance != null;
    }

    [UnityEditor.MenuItem("DebugMode/Transportation/ToEnd #p")]
    static void ToEnd(){
        //PlayerManager.instance.StopToTest();
        Transportation("end");
    }
    [UnityEditor.MenuItem("DebugMode/Transportation/ToEnd #p", true)]
    static bool Toggle19()
    {
        return PlayerManager.instance != null;
    }

    #endregion



    [UnityEditor.MenuItem("DebugMode/Control/ToggleColliders %#z")]
    static void ToggleColliders(){
        bool toggle=!DebugManager.instance.onCol;
        DebugManager.instance.onCol=toggle;
        if(toggle) Debug.Log("Enable colliders");
        else Debug.Log("Disable colliders");

        BoxCollider2D[] collider2Ds=FindObjectsOfType(typeof(BoxCollider2D)) as BoxCollider2D[];
        EdgeCollider2D[] collider2Ds2=FindObjectsOfType(typeof(EdgeCollider2D)) as EdgeCollider2D[];
        CircleCollider2D[] collider2Ds3=FindObjectsOfType(typeof(CircleCollider2D)) as CircleCollider2D[];
        PolygonCollider2D[] collider2Ds4=FindObjectsOfType(typeof(PolygonCollider2D)) as PolygonCollider2D[];
    
        foreach(BoxCollider2D col in collider2Ds){
            col.enabled = toggle;
        }
        foreach(EdgeCollider2D col2 in collider2Ds2){
            col2.enabled = toggle;
        }
        foreach(CircleCollider2D col3 in collider2Ds3){
            col3.enabled = toggle;
        }
        foreach(PolygonCollider2D col4 in collider2Ds4){
            col4.enabled = toggle;
        }
        //return PlayerManager.instance != null;
        
    }

    [UnityEditor.MenuItem("DebugMode/Control/TogglePlayerSpeed %#x")]
    static void ToggleSpeed(){
        if(DebugManager.instance.onDebug){
            DebugManager.instance.speedUp = !DebugManager.instance.speedUp;
            PlayerManager.instance.runSpeed = DebugManager.instance.speedUp ? 20f : 8f;
        }
    }
    [UnityEditor.MenuItem("DebugMode/Control/IAmGod %#v")]
    static void IAmGod(){
        if(DebugManager.instance.onDebug){
            // for(int i=0; i<12;i++){
                
            //     Inventory.instance.GetItem(i+1);
            // }
            Inventory.instance.GetItem(19);
            Inventory.instance.GetItem(20);
            Inventory.instance.GetItem(21);
            Inventory.instance.GetItem(2);
            Inventory.instance.GetItem(5);
            Inventory.instance.GetItem(6);
        }
    }



    
//#else 
//#endif
//#if DEV_MODE

    static void Transportation(string mapName){
        DebugManager.instance.StartCoroutine(TPC(mapName));
        if(mapName!="start"){
            PlayerManager.instance.currentMapName = mapName;
        PlayerManager.instance.ChangeColor();
        PlayerManager.instance.CheckPassive();
        }
        Debug.Log(mapName);
        // SceneManager.LoadScene(mapName);
        // PlayerManager.instance.debuggingMode = true;
        // PlayerManager.instance.transform.position = new Vector3 (0,0,0);
        // DisableColliders();
    }
    public static IEnumerator TPC(string mapName){
        //SceneManager.LoadScene("start");
        //yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(mapName);
        yield return new WaitForSeconds(0.2f);
        PlayerManager.instance.transform.position = new Vector3 (0,0,0);
        //PlayerManager.instance.debuggingMode = true;
        
        //StopToTest();
        //DisableColliders();
        if(mapName == "start"){

            LoadingTrig.instance.loadWindow.SetActive(false);
            Fade2Manager.instance.color = Fade2Manager.instance.black.color;
            Fade2Manager.instance.color.a = 0f;
            Fade2Manager.instance.black.color = Fade2Manager.instance.color;
            Fade2Manager.instance.go.SetActive(false);

        }
        PlayerManager.instance.shadow_laydown.GetComponent<Animator>().SetTrigger("off");
        PlayerManager.instance.shadow_laydown.gameObject.SetActive(false);
                    
        PlayerManager.instance.notMove = false;
        PlayerManager.instance.isWakingup = false;

        DatabaseManager.instance.bookActivated = true;
    }

    void StopToTest(){
        
        StopCoroutine(thePlayer.WakingUp());
        DatabaseManager.instance.doneIntro=true;
        thePlayer.animator.SetBool("wake_up",false);
        thePlayer.shadow_laydown.gameObject.SetActive(false);
        thePlayer.notMove = false;
        thePlayer.isWakingup = false;
    }
    

    
#endif
}
