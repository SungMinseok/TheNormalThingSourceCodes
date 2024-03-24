using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    static public BGMManager instance;
    public float firstVolume = 0.3f;

    public AudioClip[] clips;

    public AudioSource source;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    PlayerManager thePlayer;
    public string mapName; //플레이어 맵위치 저장
    public bool mainBGMOn; //트루면 메인브금 켜져있는상태
    public int trackNum;    //재생중인 트랙 넘버
    private int mapTypeNum;
    private float savedVolume = 1f; //설정저장된 볼륨값
    private float backupVolume; //ctrl+M으로 배경음 온오프
    //private bool mapChangeCheck;    //맵이동 체크
    public bool firstPlay = true;//맨 처음시작시 페이드 아웃 안함.
    

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }

        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        thePlayer = PlayerManager.instance;
        SetVolume(firstVolume);
        /*
        if(thePlayer != null){
                   //메인 브금 온
                mainBGMOn = true;
                Play(1);
                    
            
        }*/




    }

    public void Play(int _playMusicTrack)
    {
        source.volume = savedVolume;
        source.clip = clips[_playMusicTrack];
        source.Play();
        //trackNum = _playMusicTrack;
    }

    public void Stop()
    {
        source.Stop();
    }
    public void Pause(){
        source.Pause();
    }
    public void Unpause(){
        source.UnPause();
    }

    public void SetVolume(float _volume){
        source.volume = _volume;
        savedVolume = source.volume;
    }
    public void FadeOutMusic()
    {
        StopCoroutine(FadeOutMusicCoroutine());
        StopCoroutine(FadeInMusicCoroutine());
        StartCoroutine(FadeOutMusicCoroutine());
    }

    IEnumerator FadeOutMusicCoroutine()                 //2.5초 대기해야함
    {
        for(float i = savedVolume; i>=0f; i-=0.02f)
        {
            source.volume = i;
            yield return waitTime;
        }    
    }

    public void FadeinMusic()
    {
        StopCoroutine(FadeOutMusicCoroutine());
        StopCoroutine(FadeInMusicCoroutine());
        StartCoroutine(FadeInMusicCoroutine());
    }

    IEnumerator FadeInMusicCoroutine()
    {
        for (float i = 0f; i <= savedVolume; i += 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }
    /*
    Track List
    0:Thenormalthing
    1:ThemeSong_Spirit_of_the_Dead.mp3  //mainTheme
    2:puzzle_theme_part_1 
    3:puzzle_theme_part_2
    4:Midnight_in_the_Graveyard.mp3 + Day_of_Chaos.mp3
    5:04_aurora_currents(parrot)
    6:488376__wobesound__deathgameoverv1



    */
    void FixedUpdate(){
        
        if(Input.GetKey(KeyCode.LeftControl)&&Input.GetKeyDown(KeyCode.M)){
            if(savedVolume>0.1f){
                backupVolume = savedVolume;
                SetVolume(0f);
            }
                
            else if(savedVolume<0.1f){
                SetVolume(backupVolume);
            }
        }







        if(thePlayer == null){
            
            //Debug.Log("Player checking...");
            thePlayer = PlayerManager.instance;
        }
        
        if(thePlayer != null){
            
            mapName = thePlayer.currentMapName;
            if(!thePlayer.isPlayingPuzzle&&!thePlayer.isGameOver&&!thePlayer.isDemoOver&&!thePlayer.isChased&&!thePlayer.finishGame&&!thePlayer.isWatching){       //특정 "상황"이 아닐 때 메인 브금 or 특정 브금 (특정상황 &&로 추가가능)
                if((mapName == "start"||mapName == "cabin"||mapName == "catwood2"||mapName == "ch2"||
                mapName == "ch3"||mapName == "cornerwood"||mapName == "camp"||mapName == "middlewood"
                ||mapName == "village") && trackNum!=1){       // 메인 브금 온
                    trackNum=1;
                    if(firstPlay){
                        firstPlay = false;
                        Play(1);
                    }
                    else{
                        FadeOutNPlay(1);
                    }
                }


                
                
                // else if(mapName == "catwood" && trackNum!=5 && !DatabaseManager.instance.gameOverList.Contains(-1)){  //특정 "맵"일 때 특정 브금
                //     trackNum=5;
                //     if(firstPlay){
                //         firstPlay = false;
                //         Play(5);
                //     }
                //     else{
                //         FadeOutNPlay(5);
                //     }
                // // }
                // else if(mapName == "catwood" && trackNum==1 && !DatabaseManager.instance.gameOverList.Contains(-1)){  //특정 "맵"일 때 특정 브금
                //     FadeOutMusic();
                // }

                else if((mapName=="lakein"||mapName == "lake") && trackNum!=7){  //특정 "맵"일 때 특정 브금
                    trackNum=7;
                    if(firstPlay){
                        firstPlay = false;
                        Play(7);
                    }
                    else{
                        FadeOutNPlay(7);
                    }
                }
                else if((mapName=="lakeout"||mapName=="parrothidden") && trackNum!=8){  //특정 "맵"일 때 특정 브금
                    Stop();
                    trackNum=8;
        StopAllCoroutines();
                    Play(8);
                    //FadeOutNPlay(8);
                }
                else if(mapName == "rainingforest" && trackNum!=9){  //특정 "맵"일 때 특정 브금
                    Stop();
                    trackNum=9;
        StopAllCoroutines();
                    Play(9);
                    //FadeOutNPlay(9);
                }
                else if((mapName == "thunderingforest"||mapName == "mazein"||mapName == "maze") && trackNum!=10){  //특정 "맵"일 때 특정 브금
                    Stop();
                    trackNum=10;
        StopAllCoroutines();
                    Play(10);
                    //FadeOutNPlay(10);
                }
                else if((mapName == "mazeout"||mapName =="end")&&trackNum!=14){  //특정 "맵"일 때 특정 브금
                    trackNum=14;
                    if(firstPlay){
                        firstPlay = false;
                        Play(14);
                    }
                    else{
                        FadeOutNPlay(14);
                    }
                    //trackNum=10;
                   // Play(10);
                    //FadeOutNPlay(10);
                }
               

                //else if(mapName == "catwood2" && )

            }

            // else if(thePlayer.isPlayingPuzzle && trackNum!=2){  //특정 "상황"일 때 특정 브금
            //     trackNum=2;
            //     PuzzleBGM();
            // }
            else if(thePlayer.isPlayingPuzzle){  //특정 "상황"일 때 특정 브금
                if(PuzzleManager.instance.puzzleNum[3].activeSelf){
                    if(trackNum!=11){
                            
                        trackNum=11;
                        FadeOutNPlay(11);
                    }
                }
                else{
                    // if(trackNum!=2){
                        
                    //     trackNum=2;
                    //     PuzzleBGM();
                    // }

                    
                    if(trackNum!=13){
                        trackNum=13;
                        FadeOutNPlay(13);
                    }
                }
                
            }

            else if(thePlayer.isGameOver && trackNum!=6){
                trackNum=6;
                FadeOutNPlay(6,false);
            }
            else if(thePlayer.isDemoOver && trackNum!=15){
                trackNum=15;
                FadeOutNPlay(15,false);
                source.loop = false;
            }

            else if(thePlayer.isChased && !thePlayer.isGameOver && trackNum!=4){
                //Debug.Log("언노운테마온");
                trackNum=4;
                FadeinMusic();
                Play(4);
               // FadeOutNPlay(4);
            }
            // else if(thePlayer.finishGame && trackNum!=12){
            //     trackNum=12;
            //     FadeOutNPlay(12);
            //     source.loop = false;
            //     //EndingBGMCoroutine();
            // }

            else if(thePlayer.isWatching&&source.isPlaying){
                source.Stop();
            }

            



/*
            if(thePlayer.playingPuzzle && mainBGMOn){                //퍼즐 중에는 브금 아웃.
                mainBGMOn = false;
                PuzzleBGM();
            }
            else if(!thePlayer.playingPuzzle && !mainBGMOn){        //퍼즐 끄면 다시 브금온
                
                mainBGMOn =true;
                MainBGM();
            }*/

        }
    }

    public void PuzzleBGM(){
        StopAllCoroutines();
        StartCoroutine(PuzzleBGMCoroutine());
    }
    IEnumerator PuzzleBGMCoroutine(){
        
        FadeOutMusic();
        //yield return new WaitUntil(() => source.volume==0f);
        
        yield return new WaitForSeconds(2.5f);
        Play(2);
        yield return new WaitForSeconds(54f);
        Play(3);
    }
    public IEnumerator EndingBGMCoroutine(){
        
        //yield return new WaitForSeconds(54.465f);
        yield return new WaitUntil(()=>!source.isPlaying);
        Stop();
    }
    /*
    public void MainBGM(){
        
        StopAllCoroutines();
        StartCoroutine(MainBGMCoroutine());
        
    }
    IEnumerator MainBGMCoroutine(){
        
        FadeOutMusic();
        yield return new WaitForSeconds(2f);
        Play(1);
    }
    */
    public void FadeOutNPlay(int num, bool fadeIn = true){
        StopAllCoroutines();
        StartCoroutine(FadeOutNPlayCoroutine(num, fadeIn));
    }
    IEnumerator FadeOutNPlayCoroutine(int num, bool fadeIn){
        FadeOutMusic();
        //yield return new WaitUntil(() => source.volume==0f);
        
        yield return new WaitForSeconds(2.5f);
                if(fadeIn) FadeinMusic();
        Play(num);
        //FadeinMusic();
    }
    /*
    int MapNameCheck(){
        int mapType = 0;
        mapName = thePlayer.currentMapName;
        switch(mapName){
            case "start" :
                mapType = 1;
                break;
            case "cabin" :
                mapType = 1;
                break;
            case "catwood2" :
                mapType = 1;
                break;
            case "ch2" :
                mapType = 1;
                break;


        }
        return mapType;
    }
    */
}
