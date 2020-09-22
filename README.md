# TheNormalThingSourceCodes
The Normal Thing Unity Project Source Codes

## INDEX
[1.Various Puzzles](#various-puzzles)

[2.Enemy Randomly Appearing and Chasing](#enemy-randomly-appearing-and-chasing)

[3.Save and Load](#save-and-load)

[4.Acquisition and Use of Items](#acquisition-and-use-of-items)


## Various Puzzles

1.Rotate Puzzle

<img src="https://user-images.githubusercontent.com/70127676/93859850-25d51800-fcf9-11ea-8b1a-316555aded4d.gif" width="100%" height="100%">

Implemented functions

- 블록 회전 경우의 수 : 블록 당 4가지.
- 모든 블록 랜덤 회전(정답 제외)

2.Polyomino

<img src="https://user-images.githubusercontent.com/70127676/93859916-37b6bb00-fcf9-11ea-9232-cd3fbf2ea2ac.gif" width="100%" height="100%">

3.Jigsaw Puzzle

<img src="https://user-images.githubusercontent.com/70127676/93859883-2c638f80-fcf9-11ea-8388-b35f7d833470.gif" width="100%" height="100%">

4.Sliding Puzzle

<img src="https://user-images.githubusercontent.com/70127676/93860447-173b3080-fcfa-11ea-9341-8224531c3690.gif" width="100%" height="100%">

## Enemy Randomly Appearing and Chasing
```
    public class UnknownPhase
    {
      [Header ("단계")]public int phase;
      [Header ("맵 이동 횟수")]public int remainingCount;
      [Header ("생존 시간")]public float timer;
      [Header ("이동 속도")]public float speed;
      [Header ("맵 이동 후 재생성 시간")]public float timer_wait = 3f;
      [Header ("출현 확률 (0~99)")]public int prob;
      [Header ("출현 맵")]public string[] maps;    
    }
```
    public class UnknownManager : MonoBehaviour
    {
      public bool activateRandomAppear;   //true 이면 랜덤 출몰 (적 출현 스위치)

    [Header ("초반:0, 중반이후:1, Maze:2, 첫등장:3")]
    public int nowPhase= -1;
    
    [Header ("단계별 설정")]
    [SerializeField]
    public UnknownPhase[] phases;
    public float coolDown = 3f;
    public bool coolDownFlag;
    public bool activated;
    public GameObject unknown;
    public List<Transform> locationList = new List<Transform>();
    public static UnknownManager instance;

```
쿨다운 체크
    void FixedUpdate()
    {
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
            else if(((thePlayer.notMove&&thePlayer.currentMapName!="maze"&&!thePlayer.isWakingup)||thePlayer.isInteracting||thePlayer.isPlayingPuzzle||thePlayer.isPlayingGame||thePlayer.isWakingup||theBook.book.activeSelf)){
//Debug.Log("쿨다운 무효화");
                coolDownFlag= false;
                activated =false;
            }
            else if(activated&&coolDownFlag&&!thePlayer.isTransporting){
//Debug.Log("언노운 출현");
                coolDownFlag = false;
            }
        }
        else{
            coolDownFlag = false;
                StopAllCoroutines();
        } 
    }
    ```
    IEnumerator CoolDown(){
        yield return new WaitForSeconds(coolDown);
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
         
    }


    public int CheckMaps(){    //맵이동시 실행. 그맵이름 잇으면 그 페이즈로 설정.
        for(int i=0; i<phases.Length;i++){
                
            for(int j=0; j<phases[i].maps.Length; j++){
                if(thePlayer.currentMapName==phases[i].maps[j]){
                    return i;
                }
            }
        }
        return -1;
    }
}


## Save and Load
## Acquisition and Use of Items
