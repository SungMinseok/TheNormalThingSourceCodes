using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
public class Trig2 : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    public int trigNum;
    
    public Dialogue dialogue_1;
    //public Select select_1;
    public Dialogue dialogue_2;
    public Select select_2;
    //public Dialogue dialogue_3;
    //public Dialogue dialogue_4;

    public GameObject door;
   // public GameObject destination;

    /////////////////////////////////////////////////////////////////////  
    private DialogueManager theDM;
    private SelectManager theSelect;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private DatabaseManager theDB;
    private FadeManager theFade;


    /////////////////////////////////////////////////////////////////////   flag는 실행여부 파악 : true이면 실행중/실행완료, false이면 실행전
    protected bool flag;

    private int bifur;

    /////////////////////////////////////////////////////////////////////   inspector에서 체크 가능. 체크시 해당 트리거 무한 반복.
    public bool preserveTrigger;
    public bool onlyOnce = true;

    void Start()
    {
        theDM = DialogueManager.instance;
        theSelect = SelectManager.instance;
        theOrder = OrderManager.instance;
        //thePlayer = PlayerManager.instance;
        theDB = DatabaseManager.instance;
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FadeManager.instance;

        
        if(theDB.trigOverList.Contains(trigNum)){
            flag = true;
        }
        
    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    
    
    private void OnTriggerStay2D(Collider2D collision){
        if(!theDB.trigOverList.Contains(2)){
            
            if(!thePlayer.exc.GetBool("on")&&!flag&&(thePlayer.canInteractWith==0||thePlayer.canInteractWith==trigNum)){
                thePlayer.exc.SetBool("on",true);
                thePlayer.canInteractWith = trigNum;
            }

            if(!flag &&(Input.GetKeyDown(KeyCode.Space)||thePlayer.getSpace)&&!theDB.doorLockedList.Contains(1)&&
                thePlayer.canInteractWith == trigNum){
                flag = true;
            thePlayer.exc.SetBool("on",false);
            //thePlayer.canInteractWith = 0;
                StartCoroutine(EventCoroutine());
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D collision){
        thePlayer.exc.SetBool("on",false);
        thePlayer.canInteractWith = 0;
    }

/*
    private void Wait(){
        StartCoroutine(_Wait());
    }
    IEnumerator _Wait(){
        yield return new WaitForSeconds(0.01f);
    }*/
    IEnumerator EventCoroutine(){

        theOrder.PreLoadCharacter();        
        theOrder.Turn("Player","UP");

        //thePlayer.exc.SetTrigger("on");

        theOrder.NotMove();                                                 //트리거 중 이동불가
        if(bifur==0){
            bifur=1;
            theDM.ShowDialogue(dialogue_1);
            yield return new WaitUntil(()=> !theDM.talking);
            bifur=1;
        }
        AudioManager.instance.Play("knock");
        yield return new WaitForSeconds(1.2f);
        theSelect.ShowSelect(select_2);
        yield return new WaitUntil(() => !theSelect.selecting);  
        if(theSelect.GetResult()==0){

            preserveTrigger=false;
            AudioManager.instance.Play("metaldoor");
            theFade.FadeOut();
            thePlayer.lastMapName = thePlayer.currentMapName;    //s
            thePlayer.currentMapName = "cabin";
            yield return new WaitForSeconds(1f);
            theDB.trigOverList.Add(trigNum);
            theDB.doorEnabledList.Add(1);
            SceneManager.LoadScene("cabin");
            
            theFade.FadeIn();
            theOrder.NotMove(); 
        }
        else{

            theDM.ShowDialogue(dialogue_2);
            yield return new WaitUntil(()=> !theDM.talking);
        }
        
        theOrder.Move(); 

        if(preserveTrigger)
            flag=false;
//         if(bifur==0){

//             theSelect.ShowSelect(select_1);
//             yield return new WaitUntil(() => !theSelect.selecting);             //선택지 선택 끝날 때까지 대기 
//             //Wait();

// //            Debug.Log(theSelect.GetResult());

            
//         }


        //yield return new WaitForSeconds(0.01f);
        // if(theSelect.GetResult()==0||bifur==1){
        //     theOrder.NotMove();
        //     bifur=1;
        //     AudioManager.instance.Play("knock");
        //     yield return new WaitForSeconds(1.2f);
        //     theSelect.ShowSelect(select_2);
        //     yield return new WaitUntil(() => !theSelect.selecting);
                
        //     if(theSelect.GetResult()==0){
                
        //         //AudioManager.instance.Play("metaldoor");
        //         //thePlayer.transform.position = door.transform.position;
                
        //         //theDM.ShowDialogue(dialogue_3);
        //         //yield return new WaitUntil(()=> !theDM.talking);
        //         door.GetComponent<TransferMap>().Enabled = true;
        //         //yield return new WaitForSeconds(0.3f);
        //         //Debug.Log(thePlayer.transform.position);
        //         //Debug.Log(door.transform.position);
        //         //Debug.Log("문이 열렸다");


        //         if(onlyOnce)
        //             theDB.trigOverList.Add(trigNum);

        //         preserveTrigger=false;

                
        //         AudioManager.instance.Play("metaldoor");
        //         //theOrder.NotMove();
        //         theFade.FadeOut();
        //         thePlayer.lastMapName = thePlayer.currentMapName;    //s
        //         thePlayer.currentMapName = "cabin";
                
                
        //         yield return new WaitForSeconds(1f);
        //         theDB.doorEnabledList.Add(1);
        //         SceneManager.LoadScene("cabin");
                
        //         theFade.FadeIn();
        //         theOrder.Move();






        //          //thePlayer.transform.position = door.transform.position; 

        //                         //문활성화
        //         //key=true;

        //         //yield return new WaitForSeconds(0.2f);
        //         //thePlayer.transform.position = destination.transform.position;
        //         //thePlayer.transform.position = destination.transform.position;
        //         //SceneManager.LoadScene("cabin");
        //         theOrder.NotMove(); 
        //         //yield return new WaitForSeconds(0.01f);
        //     }
                            
//             else if(theSelect.GetResult()==1){

//             }
//                 //theDM.ShowDialogue(dialogue_4);
//                 //yield return new WaitUntil(()=> !theDM.talking);
                
//         }
            
        
//         else if(bifur==0&&theSelect.GetResult()==1){
//             //theDM.ShowDialogue(dialogue_2);
//             //yield return new WaitUntil(()=> !theDM.talking);
             
//             // thePlayer.transform.position = destination.transform.position; 
//         }

                                                                //트리거 완료 후 이동가능

    }


    

}

