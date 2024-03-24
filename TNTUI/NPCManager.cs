using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

[System.Serializable]//인스펙터에 표시

public class NPCManager : MonoBehaviour
{
    public Queue<string> queue;
    public Transform parent;    //타겟
    public Transform npc1;      //본인
    public Queue<Vector2> npcPos;
    public Queue<Vector2> parentPos;
    public Vector2 currentPos;
    public Vector2 targetPos;
    public Vector2 vectorA;
    static public NPCManager instance;
    protected bool npccanmove = true;
    public Animator animator;
    public bool NPCWalkFlag = true;
    public bool Walking = false;

    private void Awake()
    {
        parentPos = new Queue<Vector2>();
        npcPos = new Queue<Vector2>();
    }
    void Start()
    {
        queue = new Queue<string>();
        //  if (instance == null)
        //  {
        //      DontDestroyOnLoad(this.gameObject);
        //      instance = this;
        //  }
        //  else
        //  {
        //      Destroy(this.gameObject);
        //  }
        parent = PlayerManager.instance.gameObject.transform;
        StartCoroutine(MoveCoroutine());
     }
 
    void Search()
    {
        npcPos.Enqueue(npc1.position);
        parentPos.Enqueue(parent.position);

        currentPos = npcPos.Dequeue();
        targetPos = parentPos.Dequeue();
        
        vectorA.Set(currentPos.x - targetPos.x, currentPos.y - targetPos.y);
    }

    private IEnumerator MoveCoroutine()
    {
        //animator.SetBool("Walking", true);
        npccanmove = false;
        Search();
        for (int i=0; i<10; i++)
        {
            Search();
            vectorA.Set(currentPos.x - targetPos.x, currentPos.y - targetPos.y);
            if (Mathf.Abs(vectorA.x) >= 0.3f)// && Mathf.Abs(vectorA.y) > 0.1f)
            {
                {
                    if (vectorA.x > 0.03f)
                    {
                        currentPos.x = currentPos.x - 0.03f;
                    }

                    else if (vectorA.x < -0.03f)
                    {
                        currentPos.x = currentPos.x + 0.03f;
                    }
                }
                //animator.SetFloat("Horizontal", -vectorA.x);
                //animator.SetFloat("Speed", vectorA.sqrMagnitude);
            }
            else// if(Mathf.Abs(vectorA.x) < 0.3f)//&& Mathf.Abs(vectorA.y) > 0.1f)
            {
                {
                    if (vectorA.y > 0.03f)
                    {
                        currentPos.y = currentPos.y - 0.03f;
                    }

                    else if (vectorA.y < -0.03f)
                    {
                        currentPos.y = currentPos.y + 0.03f;
                    }
                }
                //animator.SetFloat("Vertical", -vectorA.y);
                //animator.SetFloat("Speed", vectorA.sqrMagnitude);
            }
            transform.position = currentPos;
            animator.SetFloat("Horizontal", -vectorA.x);
            animator.SetFloat("Vertical", -vectorA.y);
            animator.SetFloat("Speed", vectorA.sqrMagnitude);
            yield return new WaitForSeconds(0.01f);
            npccanmove = true;
            Debug.Log(i);
            yield return new WaitUntil(()=> npccanmove);

            if(Mathf.Abs(vectorA.x) < 1.1f && Mathf.Abs(vectorA.y) < 1.1f)
            {
                Debug.Log("Game Over");
                LoadingTrig.instance.GameOver();
                //animator.SetBool("Walking", false);
                break;
            }

            else
            {
                i = -1;
            }
        }
    }
}

