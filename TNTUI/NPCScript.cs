using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public static NPCScript instance;
    [Header ("이동 가능")]
    public bool canMove = true;    //이동 가능
    //public bool onPatrol;   //범위 내 있으면 이동 가능
    //public float onPatrolRange = 5f;
    [Header ("랜덤 이동")]
    public bool onJYD = true;   //정야독
    
    [Header ("랜덤 이동 쿨타임")]
    public float JYDCooldown = 3f;
    bool JYDFlag;
    [Header ("속도")]
    public float speed=0.5f;
    [Header ("목표 지정")]
    public Transform[] moveLocation;
    [Header ("대화 트리거")]
    public GameObject trig;
    [Header ("JYD구역")]
    public BoxCollider2D JYDArea;

    private PlayerManager thePlayer;

    private Animator animator;
    public SpriteRenderer spriteRenderer;
    //private BoxCollider2D boxCollider2D;
    private Rigidbody2D rb;
    private Transform target;
    private DatabaseManager theDB;
    private DialogueManager theDM;
    Vector2 movement;
    public Queue<Transform> que;
    public bool outOfArea;
    private Vector3 maxBound;
    private Vector3 minBound;

    void Start()
    {
        instance = this;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        theDB = DatabaseManager.instance;
        theDM = DialogueManager.instance;

        que = new Queue<Transform>();    //이동할 위치
        // for(int i =0 ; i<moveLocation.Length;i++){

        //     que.Enqueue(moveLocation[i]);
        // }
        thePlayer = PlayerManager.instance;

        maxBound = JYDArea.bounds.max;
        minBound = JYDArea.bounds.min;
        // Debug.Log("1 "+maxBound.x);
        // Debug.Log("2 "+ (-maxBound.x));
        //Debug.Log("2 "+maxBound.y);
        // target = this.gameObject.transform;
        // target = thePlayer.gameObject.transform;
    }

    void FixedUpdate()
    {
        if(canMove){
            //Debug.Log("0 "+transform.position.x);//-1
            // Debug.Log("1 "+maxBound.x);//1.7
            // Debug.Log("2 "+minBound.x);//-1.7
            // Debug.Log("3 "+(transform.position.x>maxBound.x));
            // Debug.Log("4 "+(transform.position.x<-maxBound.x));
            // Debug.Log("3 "+(transform.position.y>maxBound.y));
            // Debug.Log("4 "+(transform.position.y>-maxBound.y));
            if(!outOfArea&&(transform.position.x>maxBound.x||transform.position.x<minBound.x||transform.position.y>maxBound.y||transform.position.y<minBound.y)){
                outOfArea = true;
//                Debug.Log("범위 벗어남");
            }
            // if(target!=null){
                
            //     transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
            // }
            
            else if(outOfArea){
//                Debug.Log("중간으로 이동");
                JYDFlag = false;
                transform.position = Vector3.MoveTowards(transform.position, JYDArea.gameObject.transform.position, speed * Time.fixedDeltaTime);
                // if(transform.position.x - JYDArea.gameObject.transform.position.x >=0){
                //     spriteRenderer.flipX = false;
                // }
                // else{
                //     spriteRenderer.flipX = true;
                // }
                spriteRenderer.flipX= transform.position.x - JYDArea.gameObject.transform.position.x >=0? false : true;
                
                
                if(transform.position== JYDArea.gameObject.transform.position){
                    outOfArea = false;
                }
            }
            // if(que.Count>0){ //이동할 곳이 있다.
            //     Debug.Log("디큐");
            //     target = que.Dequeue();
            // }
            // if(target.position != transform.position){
                
            //     transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
            //     animator.SetBool("walk", true);
            //     if(transform.position.x - target.position.x >=0){
            //         spriteRenderer.flipX = false;
            //     }
            //     else{
                    
            //         spriteRenderer.flipX = true;
            //     }
            // }

            // else if(que.Count==0&&target.position == transform.position){
                
            if(onJYD&&!outOfArea){
//                Debug.Log("랜덤이동중");
                if(!JYDFlag){
                    StartCoroutine("JYD");
                }
                spriteRenderer.flipX = movement.x >=0 ? true : false;
                rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
                animator.SetBool("walk", true);
                animator.SetFloat("walkspeed", movement.sqrMagnitude * speed + 0.5f);
                //Debug.Log(movement.x +", "+ movement.y+", Speed : "+ speed + ", movement : "+movement);
            }

            
        }
        else{
            animator.SetBool("walk", false);
            movement = Vector2.zero;
            StopCoroutine("JYD");
            JYDFlag = false;
            //thePlayer.isChased = false;
        }

        trig.transform.position = this.transform.position;
    }

    public void WalkSound(){
        int temp = Random.Range(0,3);
        //AudioManager.instance.Play("unknownwalk"+temp.ToString());
    }

    IEnumerator JYD(){
        //Debug.Log("쿨");
        JYDFlag = true;
        movement = new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f));
        yield return new WaitForSeconds(JYDCooldown);
        JYDFlag = false;
    }

}
