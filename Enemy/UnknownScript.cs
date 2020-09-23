using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Collections;

public class UnknownScript : MonoBehaviour
{
    public static UnknownScript instance;
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
    public UnknownPhase myInfo;
    public bool canMove = true;    //이동 가능

    public bool activated;  //true 이면 출현.
    //[Header ("맵 이동 후 재생성 위치")]
    [HideInInspector]
    public Transform startPoint; //재생성 위치.
    public ParticleSystem myParticle;


    //public bool onWaiting;  //맵이동시 대기
    // [Header ("맵 이동 후 재생성 남은 시간")]
    // public float timer_wait = 3f; //재생성 남은 시간.
    [Header ("소멸 남은 시간")]
    public float timer;// = 12f; //소멸 남은 시간.
    [Header ("남은 맵 이동횟수")]
    public int remainingCount;// = 2; //소멸 남은 시간.


    // public bool onPatrol;   //범위 내 있으면 이동 가능
    // public float onPatrolRange = 5f;
    // public float speed=1.5f;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public GameObject shadow;
    //private Transform shadowBackup;
    private bool foundTarget;
    public Transform target;
    public List<Transform> moveLocation;
    private PlayerManager thePlayer;
    private UnknownManager mother;
    public Queue<Transform> que;

    void Start()
    {
        Debug.Log("생성성공!");
        //instance = this;
        que = new Queue<Transform>();    //이동할 위치
        ObjectManager.instance.FadeIn(spriteRenderer);

        if(moveLocation.Count!=0){
            
            for(int i =0 ; i<moveLocation.Count;i++){

                que.Enqueue(moveLocation[i]);
            }
        }
        thePlayer = PlayerManager.instance;
        target = this.gameObject.transform;
        //target = thePlayer.gameObject.transform;
        //animator.SetFloat("walkSpeed", (float)(myInfo.speed/1.5f));
        animator.SetFloat("walkSpeed", 1);
        //if(onPatrol) canMove =false;
        //shadowBackup = shadow.transform;
        mother = UnknownManager.instance;


        if(!mother.activated) mother.activated = true;

        //초기설정

        //spriteRenderer.color = new Color(1,1,1,1);
        thePlayer.isChased = true;
        myInfo = mother.phases[mother.nowPhase];
        // timer = mother.phase == 0 ? 12f : 20f;
        // remainingCount = mother.phase == 0 ? 2 : 3;


        // switch(mother.nowPhase){
        //     case 0 :
        //         myInfo = mother.phases[0];
        //         // timer = mother.phases[0].timer;
        //         // remainingCount = mother.phases[0].remainingCount;
        //         // speed = mother.phases[0].speed;
        //         break;
        //     case 1 :
        //         myInfo = mother.phases[1];
        //         break;
        // }

        //myParticle.duration = myInfo.timer;

        myParticle.Stop();
        var main = myParticle.main;
        main.duration = myInfo.timer;
        myParticle.Play();

        timer = myInfo.timer;
        remainingCount = myInfo.remainingCount;
        // timer = mother.nowPhase ==0 ? mother.phases[0].timer : mother.phases[1].timer;
        // remainingCount = mother.nowPhase ==0 ? mother.phases[0].remainingCount : mother.phases[1].remainingCount;



        //책 못열게
        //BookManager.instance.DisableBookOnBtn();
    }
    void OnTriggerStay2D(Collider2D collision){
        if(collision.gameObject.name == "Player"){
            
            if(thePlayer.isChased && canMove){

                if(thePlayer.life==0){
                    GameOver();
                }

                else{
                    thePlayer.life -= 1;
                    Inventory.instance.RemoveItem(14);
                    mother.activated = false;
                    StartCoroutine(DestroyUnknown());
                }

            }
        }
    }
    void FixedUpdate()
    {
        if(timer>=0) timer -= Time.deltaTime;
        //Debug.Log("timer : "+timer);
        // if(onPatrol){
        //     foundTarget = Physics2D.OverlapCircle((Vector2)transform.position, onPatrolRange, //있나?
        //     1<< LayerMask.NameToLayer("player"));
        //     canMove = foundTarget ? true : false;
        // }
        if(/*canMove&&onWaiting ||*/ thePlayer.isTransporting){
            canMove = false;
            thePlayer.isTransporting = false;
            remainingCount --;
            if((remainingCount<=0 && timer <=0f)/*&&mother.activated*/){
                //mother.activated = false;
                Debug.Log("AAA");
                StopCoroutine(ActivatedWaiting());
                StartCoroutine(DestroyUnknown());
            }
            else{ 
                
            StopCoroutine(ActivatedWaiting());
            StartCoroutine(ActivatedWaiting());

            }
    
            
        }

        
        if(canMove){
            if(que.Count>0){ //이동할 곳이 있다.
                Debug.Log("디큐");
                target = que.Dequeue();
            }
            //if(target.position!=transform.position){
                //Debug.Log("무브로케이션이동");
            transform.position = Vector3.MoveTowards(transform.position, target.position, 0.59375f*myInfo.speed * Time.fixedDeltaTime);
            animator.SetBool("walk", true);
            //spriteRenderer.flipX = transform.position.x - target.position.x >=0 ? false : true;
            if(transform.position.x - target.position.x >=0){
                spriteRenderer.flipX = false;
                //shadowSprite.flipX = false;
                //shadow.transform.position = new Vector3(shadowBackup.position.x, shadowBackup.position.y, shadowBackup.position.z);
            }
            else{
                
                spriteRenderer.flipX = true;
                //shadowSprite.flipX = true;
                //shadow.transform.position = new Vector3(-shadowBackup.position.x, shadowBackup.position.y, shadowBackup.position.z);
            }


            if(que.Count ==0 && target.position == transform.position)
                target = thePlayer.gameObject.transform;
            //}
            // else{   //이동할 곳이 없다. -> 플레이어에게 이동
                
            //     Debug.Log("플레이어에게 이동");
            //     transform.position = Vector3.MoveTowards(transform.position, 
            //     thePlayer.gameObject.transform.position, speed * Time.deltaTime);
            //     animator.SetBool("walk", true);
            //     spriteRenderer.flipX = transform.position.x - target.position.x >=0 ? false : true;
            //     thePlayer.isChased = true;
            // }
                
            if((remainingCount<=0 && timer <=0f)&&mother.activated){
                
                Debug.Log("BBB");
                mother.activated = false;
                StartCoroutine(DestroyUnknown());
            }


        }
        // else{
        //     animator.SetBool("walk", false);
        //     thePlayer.isChased = false;
        // }

        // if(transform.position == thePlayer.gameObject.transform.position && thePlayer.isChased && canMove){

        //     if(thePlayer.life==0){
        //         GameOver();
        //     }

        //     else{
        //         thePlayer.life -= 1;
        //         Inventory.instance.RemoveItem(14);
        //         mother.activated = false;
        //         StartCoroutine(DestroyUnknown());
        //     }

        // }

    
        

    }

    public void WalkSound(){
        if(mother.activated){
            int temp = Random.Range(0,3);
            AudioManager.instance.Play("unknownwalk"+temp.ToString());

        }
    }


    public void RandomAppearance(){
    }

    //플레이어 맵 이동시 언노운 대기 후 출현 위치 정해줌.
    public void SetStartPoint(){

    }

    public IEnumerator ActivatedWaiting(){
        ObjectManager.instance.FadeOut(spriteRenderer, 0.03f, false);
        shadow.SetActive(false);
        
        yield return new WaitForSeconds(myInfo.timer_wait);
        if(thePlayer.currentMapName!="maze"){
                
            this.transform.position = startPoint.position;
        }
        else{
            this.transform.position = thePlayer.pointInMaze.position;

        }
        
        ObjectManager.instance.FadeIn(spriteRenderer);
        shadow.SetActive(true);
        mother.activated = true;
        //onWaiting = false;
        canMove = true;
    }

    public void DestroyUnknownTrig(){
        StartCoroutine(DestroyUnknown());
    }


    public IEnumerator DestroyUnknown(){



        thePlayer.isChased = false;
        mother.activated = false;
        Debug.Log("언노운 파괴");
        shadow.SetActive(false);
        ObjectManager.instance.SAC();
        ObjectManager.instance.FadeOut(spriteRenderer, 0.03f, false);
        //ObjectManager.instance.FadeOut(shadow.GetComponent<SpriteRenderer>(), false);
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
    }

    public void GameOver(){
        
        Debug.Log("GameOver");
        animator.SetBool("walk", false);

        DatabaseManager.instance.caughtCount +=1;
        
        if(DatabaseManager.instance.caughtCount>=10) Debug.Log("업적6 : 이제 그만");

        AudioManager.instance.Stop("pencil");
        DialogueManager.instance.ExitDialogue();
        canMove = false;
        LoadingTrig.instance.GameOver();
        thePlayer.isGameOver = true;
        UnknownManager.instance.activated = false;
        thePlayer.isChased = false;
        if(!DatabaseManager.instance.trigOverList.Contains(22)) mother.activateRandomAppear = false; //언노운 첫등장 모퉁이에서 게임오버일때.


        Destroy(this.gameObject);
    }
}
