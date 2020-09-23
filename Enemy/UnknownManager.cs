using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnknownPhase{
    [Header ("단계")]public int phase;
    [Header ("맵 이동 횟수")]public int remainingCount;
    [Header ("생존 시간")]public float timer;
    [Header ("이동 속도")]public float speed;
    [Header ("맵 이동 후 재생성 시간")]public float timer_wait = 3f; //재생성 남은 시간.
    [Header ("출현 확률 (0~99)")]public int prob;
    [Header ("출현 맵")]public string[] maps;

    
}

public class UnknownManager : MonoBehaviour
{
    public bool activateRandomAppear;   //true 이면 랜덤 생성 가능. >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>CAMP 맵 진입 부터 켜짐. trig22
    
    [Header ("초반:0, 중반이후:1, Maze:2, 첫등장:3")]
    public int nowPhase= -1;   //초반 0 : 맵 2번이상 이동, 12초    중반 1 : 맵 3번이상 이동 + 20초
    
    [Header ("단계별 설정")]
    [SerializeField]
    public UnknownPhase[] phases;
    public float coolDown = 3f;
    //public bool delayCoolDown;  //쿨다운정지
    //[HideInInspector]
    public bool coolDownFlag;
    public bool activated;
    public GameObject unknown;
    public List<Transform> locationList = new List<Transform>();
    public static UnknownManager instance;
    private PlayerManager thePlayer;
    private BookManager theBook;
    
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
    
    // Start is called before the first frame update
    void Start()
    {
        //thePlayer = 
        thePlayer = FindObjectOfType<PlayerManager>();
        theBook = FindObjectOfType<BookManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //coolDownFlag = delayCoolDown ? true : false;
        // switch(thePlayer.currentMapName){
        //     case "maze" :
        //         if(!activateRandomAppear&&!UnknownScript.instance)
        //             activateRandomAppear = true;
        //             nowPhase = 2;
        //         break;
        // }

        
        if(activateRandomAppear && nowPhase != -1 && locationList.Count!=0 && !thePlayer.isGameOver ){
            if(UnknownScript.instance==null&&!activated&&!coolDownFlag&&!thePlayer.notMove&&!thePlayer.isPlayingGame&&!thePlayer.isPlayingPuzzle&&
            !thePlayer.isInteracting&&!thePlayer.isWakingup&&!theBook.book.activeSelf){

//Debug.Log("쿨다운시작");
                coolDownFlag= true;
                //StopCoroutine(CoolDown());
                StopAllCoroutines();
                StartCoroutine(CoolDown());
                
            }
            //쿨다운 취소 조건들
            else if(((thePlayer.notMove&&thePlayer.currentMapName!="maze"&&!thePlayer.isWakingup)||thePlayer.isInteracting||thePlayer.isPlayingPuzzle||thePlayer.isPlayingGame||thePlayer.isWakingup||theBook.book.activeSelf)/*&&!delayCoolDown*/){
//Debug.Log("쿨다운 무효화");
                //delayCoolDown = true;
                //coolDownFlag= false;
                //StopCoroutine(CoolDown());

                //StopCoroutine(CoolDown());


                
                coolDownFlag= false;
                activated =false;
            }
            else if(activated&&coolDownFlag&&!thePlayer.isTransporting){
//Debug.Log("언노운 출현");
                coolDownFlag = false;
                //if(locationList!=null){
                    //Debug.Log("1");
                    Instantiate(unknown , locationList[Random.Range(0,locationList.Count)].transform.position , Quaternion.identity);

                //}
                //else{

                //    Instantiate(unknown , new Vector3(0,0,0), Quaternion.identity);
                //}
            }
        }
        else{
            coolDownFlag = false;
            //StopCoroutine(CoolDown());
                StopAllCoroutines();
        } 
    }
    IEnumerator CoolDown(){
        yield return new WaitForSeconds(coolDown);
        //if((!thePlayer.isInteracting||!thePlayer.isPlayingPuzzle||!thePlayer.isPlayingGame||!thePlayer.isWakingup)){
        if(coolDownFlag){

            int ranNum = Random.Range(0,100);
            int myProb = 0;
            if(nowPhase!=-1){
                myProb = phases[nowPhase].prob;
                Debug.Log(ranNum);
                if(ranNum<=myProb){
                    activated = true;
                }
                else{
                    coolDownFlag = false;
                }
            }
            else{
                coolDownFlag = false;
                
            }
        }
            // switch(nowPhase){
            //     case 0 :
            //         myProb = phases[0].prob;
            //         break;
            //     case 1 :
            //         myProb = phases[1].prob;
            //         break;

            // }
            // Debug.Log(ranNum);
            // if(ranNum<=myProb){
            //     activated = true;
            // }
            // else{
            //     coolDownFlag = false;
            // }
        //}
    }

    // public void RenewalLocation(){
    //     locationList.Clear();
    //     Transform[] locations=FindObjectsOfType(typeof(Transform)) as Transform[];
    //     if(locations!=null){

    //         for(int i=0; i<locations.Length; i++){
    //             locationList.Add(locations[i]);
    //         }

    //     }

    // }

    public int CheckMaps(){    //맵이동시 실행. 그맵이름 잇으면 그 페이즈로 설정.
        for(int i=0; i<phases.Length;i++){
                
            for(int j=0; j<phases[i].maps.Length; j++){
                if(thePlayer.currentMapName==phases[i].maps[j]){
                    //activateRandomAppear = true;
                    return i;
                }
                //else 
                    //activateRandomAppear = false;
            }
        }
        
        
        return -1;
    }
}
