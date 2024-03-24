
#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

using UnityEngine;
#if !DISABLESTEAMWORKS

using System.Collections;
using System.Collections.Generic;
using Steamworks;
#endif
public class SteamAchievement : MonoBehaviour
{
#if !DISABLESTEAMWORKS
    public bool unlockTest = false;
    public CGameID m_GameID;
    public AppId_t appID;
#endif

    public static SteamAchievement instance;
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
    public void ApplyAchievements(int num){
        if(!SteamManager.Initialized) { return ; }

#if !DISABLESTEAMWORKS
        TestSteamAchievement(num);
        if(!unlockTest){
            SteamUserStats.SetAchievement("NEW_ACHIEVEMENT_1_"+num.ToString());
            SteamUserStats.StoreStats();
        }
#endif
    }
    
#if !DISABLESTEAMWORKS
    void Start()
    {
        appID = SteamUtils.GetAppID();
        m_GameID = new CGameID(SteamUtils.GetAppID());
    }
    // public void UnlockSteamAchievement(int num){
    //     TestSteamAchievement(num);
    //     if(!unlockTest){

    //     }
    // }

    // public void ApplyAchievements(int num){
    //     if(!SteamManager.Initialized) { return ; }

    //     TestSteamAchievement(num);
    //     if(!unlockTest){
    //         SteamUserStats.SetAchievement("NEW_ACHIEVEMENT_1_"+num.ToString());
    //         SteamUserStats.StoreStats();
    //     }
    // }
    public void DEBUG_LockSteamAchievement(int num){
        
        TestSteamAchievement(num);
        if(unlockTest){
            SteamUserStats.ClearAchievement("NEW_ACHIEVEMENT_1_"+num.ToString());
        }
    }
    public bool GetSteamAchievementStatus(int num){
        TestSteamAchievement(num);
        return unlockTest;
    }

    void Update(){
        //if(!m_bInitialized){
#if UNITY_EDITOR
            if(Input.GetKey(KeyCode.F5)){
                for(int i=1; i<=17; i++){
                    
                DEBUG_LockSteamAchievement(i);
                Debug.Log(i+"번 업적 잠금");
                }
            }
#endif
        //}
        SteamAPI.RunCallbacks();
    }

    void TestSteamAchievement(int num){
        SteamUserStats.GetAchievement("NEW_ACHIEVEMENT_1_"+num.ToString(), out unlockTest);
    }
#endif

}
