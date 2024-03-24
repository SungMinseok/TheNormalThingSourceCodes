#if UNITY_ANDROID || UNITY_IOS
#define DISABLEKEYBOARD
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class InGameVideo : MonoBehaviour
{
    public static InGameVideo instance;
    public VideoPlayer theVideo;
    [Header("0: 진엔딩, 1: 크레딧, 2: 끝로고, 3: 진엔딩(영어), 4: 크레딧(진엔딩)")]
    public VideoClip[] videoClips;
    public GameObject rawImage;
    public GameObject backGround;
    public GameObject skipBtn;
    public GameObject btnSet;
    public RenderTexture texture;
    public GameObject timer;
    
    public bool isPlaying;
    PlayerManager thePlayer;

    #region
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
        //instance = this;
    }
    #endregion

    void Start(){
        thePlayer = PlayerManager.instance;
        //theVideo.audioOutputMode = VideoAudioOutputMode.AudioSource;
#if DISABLEKEYBOARD
        videoClips[2]=CursorManager.instance.videoClips[3];
#else 
        videoClips[0]=CursorManager.instance.videoClips[3];
        videoClips[1]=CursorManager.instance.videoClips[4];
        videoClips[2]=CursorManager.instance.videoClips[5];
        videoClips[3]=CursorManager.instance.videoClips[6];
        videoClips[4]=CursorManager.instance.videoClips[7];
#endif

    }
    public void StartVideo(string videoName)
    {
        ClearOutRenderTexture(texture);
        StartCoroutine(VideoCoroutine(videoName));
    }
    IEnumerator VideoCoroutine(string videoName){
        yield return null;
        isPlaying = true;
        thePlayer.isWatching = true;
        switch(videoName){
            case "TrueEnding" : 
                theVideo.clip = videoClips[0];
                break;
            case "Credit" : 
                theVideo.clip = videoClips[1];
                break;
            case "LastLogo" : 
                theVideo.clip = videoClips[2];
                break;
            case "TrueEnding_EN" : 
                theVideo.clip = videoClips[3];
                break;
            case "Credit_TRUE" : 
                theVideo.clip = videoClips[4];
                break;
            default :
                Debug.Log("Error : No Video.");
                break;
        }
        theVideo.gameObject.SetActive(true);
        theVideo.Prepare();
        WaitForSeconds waitTime = new WaitForSeconds(0.1f);
        while(!theVideo.isPrepared){
            yield return waitTime;
        }
        //rawImage.GetComponent<RawImage>().texture = texture;
        theVideo.Play();
        backGround.SetActive(true);
        rawImage.SetActive(true);
#if DEV_MODE
        btnSet.SetActive(true);
        timer.SetActive(true);
#endif
        //theVideo.loopPointReached += OnMovieFinished;
        
        while(theVideo.isPlaying){
            yield return waitTime;
        }
        isPlaying = false;
#if DEV_MODE
        btnSet.SetActive(false);
        timer.SetActive(false);
#endif
        //theV.Pause();
    }
    // void OnMovieFinished(VideoPlayer player)
    // {
    //     //Debug.Log("Event for movie end called");
    //     isPlaying = false;
    //     player.Pause();
    // }
    public void ExitVideo(){

            theVideo.playbackSpeed = 1f;
        thePlayer.isWatching = false;
        backGround.SetActive(false);
        //rawImage.GetComponent<RawImage>().texture = null;
        //rawImage.GetComponent<RawImage>().texture = null;
        rawImage.SetActive(false);
        theVideo.gameObject.SetActive(false);
    }
#if DEV_MODE
    public void SKIP(){
        AudioManager.instance.Button22();
//#if DEV_MODE
        btnSet.SetActive(false);
        timer.SetActive(false);
//#endif
        if(thePlayer.isWatching){

            isPlaying = false;
        }
        else{//엔딩씬 책빨리넘기기용
            thePlayer.devMode = true;
        }
    }
#endif
    public void SetPlaybackSpeed(int mode){
        AudioManager.instance.Button22();
        if(mode==0){
            theVideo.playbackSpeed = 1f;
        }
        else if(mode==1){
            theVideo.playbackSpeed = 2f;
        }
        else{
            theVideo.playbackSpeed = 4f;
        }
    }
    public void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;
    }

#if DEV_MODE
    void FixedUpdate(){
        if(isPlaying)
        timer.GetComponentInChildren<Text>().text = theVideo.time.ToString("N2") + " / " + theVideo.length.ToString("N2");
    }
#endif
}
