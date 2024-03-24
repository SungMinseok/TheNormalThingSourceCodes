using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingObject : MonoBehaviour
{
    [HideInInspector]public BoxCollider2D boxCollider;
    
    
    //public LayerMask layerMask;
    [HideInInspector]public Queue<string> queue;
    private bool notCoroutine=false;
    [HideInInspector]public string characterName;
    
    public float speed = 4f;
    public float runSpeed = 5f;

    [HideInInspector]public Vector2 movement;
    [HideInInspector]public Vector2 movementDirection;
    //protected Vector3 dirVec;

    [HideInInspector]public Rigidbody2D rb;

    [HideInInspector]public Animator animator;

    void Start(){

        DontDestroyOnLoad(this.gameObject); //dd
        //boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }


    public void Move(string _dir){
        queue.Enqueue(_dir);
        if(!notCoroutine){
            notCoroutine=true;
            StartCoroutine(MoveCoroutine(_dir));
        }
    }

    IEnumerator MoveCoroutine(string _dir){
        while(queue.Count !=0){
            string direction = queue.Dequeue();
            //Debug.Log(direction);
            
            switch(direction){
                case "UP":
                    movement.x=0f;
                    movement.y=1f;
                    break;
                case "DOWN":
                    movement.x=0f;
                    movement.y=-1f;
                    break;
                case "RIGHT":
                    movement.x=1f;
                    movement.y=0f;
                    break;
                case "LEFT":
                    movement.x=-1f;
                    movement.y=0f;
                    break;
                case "STOP":
                    movement.x=0f;
                    movement.y=0f;
                    break;
            }

            animator.SetFloat("Speed", movement.sqrMagnitude);
        
        //이동완료 후 방향 고정
            movementDirection = new Vector2(movement.x, movement.y);
            if (movementDirection != Vector2.zero){
                animator.SetFloat("Horizontal", movementDirection.x);
                animator.SetFloat("Vertical", movementDirection.y);
            }
            /*
            while(true){

                bool checkCollisionFlag= CheckCollision();
                if (checkCollisionFlag) {
                    
                        animator.SetFloat("Speed", 0f);
                        movement.x=0f;
                        movement.y=0f;
                        yield return new WaitForSeconds(1f);
                }
                else{
                    break;
                }

            }*/
            //animator.SetFloat("Speed", 0f);
            
            yield return new WaitForSeconds(0.01f);
            notCoroutine = false;

        }
        
    }

    void Update(){
        //이동시 위치 설정

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        
        /*bool checkCollisionFlag= CheckCollision();
        if (checkCollisionFlag) {
            
                animator.SetFloat("Speed", -1f);
                movement.x=0f;
                movement.y=0f;
        }*/
    }
    //충돌판정 : 앞에 장애물 있으면 true
    /*protected bool CheckCollision(){
        RaycastHit2D hit; 

        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(movement.x * speed * Time.fixedDeltaTime, movement.y * speed * Time.fixedDeltaTime);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, layerMask);
        boxCollider.enabled = true;

        if (hit.transform != null)  //start에서 end로 갈 때 layermask에 해당하는 벽이 있으면 널 값이 아님 -> 트루
            return true;
        return false;
    }*/
}
