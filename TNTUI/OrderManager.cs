using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    private PlayerManager thePlayer; //이벤트 도중 키입력 방지
    private List<MovingObject> characters;
    
    public static OrderManager instance;
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
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        //thePlayer = PlayerManager.instance;
    }

    public void PreLoadCharacter(){
        characters = ToList();
    }
    public List<MovingObject> ToList(){
        List<MovingObject> tempList = new List<MovingObject>();
        MovingObject[] temp = FindObjectsOfType<MovingObject>();

        for(int i=0; i< temp.Length ; i++){
            tempList.Add(temp[i]);
        }
        return tempList; 
    }

    //대화 이벤트 중 움직임 방지
    public void NotMove(){
        
        thePlayer.notMove = true;
        thePlayer.animator.SetFloat("Speed", 0f);
        //thePlayer.animator.SetFloat("Horizontal", 0f);
        //thePlayer.animator.SetFloat("Vertical", 0f);
        thePlayer.movement.x=0f;
        thePlayer.movement.y=0f;
        //characters.rb.MovePosition(characters.rb.position);
    }
    public void Move(){
        thePlayer.notMove = false;
    }

    //명령 : 이동시킴
    public void Move(string _name, string _dir){
        for(int i=0; i<characters.Count; i++){
            if(_name == characters[i].characterName){
                //characters[i].notCoroutine =false;
                characters[i].Move(_dir);
                Debug.Log(_dir);
            }
        }
    }

    //명령 : 정지한 채로 바라봄
    public void Turn(string _name, string _dir){
        for(int i=0; i<characters.Count; i++){
            if(_name == characters[i].characterName){
                //characters[i].notCoroutine =false;
                switch(_dir){
                    case "UP":
                        characters[i].movement.x=0f;
                        characters[i].movement.y=1f;
                        
                        break;
                    case "DOWN":
                        characters[i].movement.x=0f;
                        characters[i].movement.y=-1f;
                        
                        break;
                    case "RIGHT":
                        characters[i].movement.x=1f;
                        characters[i].movement.y=0f;
                        
                        break;
                    case "LEFT":
                        characters[i].movement.x=-1f;
                        characters[i].movement.y=0f;
                        
                        break;
                }
                //바라보고
                characters[i].animator.SetFloat("Horizontal", characters[i].movement.x);
                characters[i].animator.SetFloat("Vertical", characters[i].movement.y);
                
                //멈춤
                characters[i].animator.SetFloat("Speed", 0f);
                characters[i].movement.x=0f;
                characters[i].movement.y=0f;
            }
        }
    }
    //투명화
    public void SetTransparent (string _name){
        for(int i=0; i<characters.Count; i++){
            if(_name == characters[i].characterName){
                characters[i].gameObject.SetActive(false);
            }
        }
    }
    
    public void SetUnTransparent (string _name){
        for(int i=0; i<characters.Count; i++){
            if(_name == characters[i].characterName){
                characters[i].gameObject.SetActive(true);
            }
        }
    }
    //공중처리 (벽뚫기)
    public void SetAir (string _name){
        for(int i=0; i<characters.Count; i++){
            if(_name == characters[i].characterName){
                characters[i].boxCollider.enabled = false;
            }
        }
    }
    public void SetGround (string _name){
        for(int i=0; i<characters.Count; i++){
            if(_name == characters[i].characterName){
                characters[i].boxCollider.enabled = true;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
