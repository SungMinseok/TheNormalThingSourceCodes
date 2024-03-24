using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestMove{
    public string name;
    public string direction;
}

[System.Serializable]
public class TestTurn{
    public string name;
    public string direction;
}

[System.Serializable]
public class TestTransparent{
    
    public bool remove ;
    //public bool create ;
    public string name;
}
public class Test : MonoBehaviour
{
    //public string name;
    [SerializeField]    
    public TestMove[] move;
    
    [SerializeField]    
    public TestTurn[] turn;
    
    [SerializeField]    
    public TestTransparent[] transparent;
    private OrderManager theOrder;
    // Start is called before the first frame update
    void Start()
    {
        theOrder= FindObjectOfType<OrderManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision){

        if(collision.gameObject.name == "Player"){

            theOrder.PreLoadCharacter();
            for(int i=0; i<move.Length; i++){
                theOrder.Move(move[i].name, move[i].direction);
                //Debug.Log(move[i].direction);
            }
            for(int j=0; j<turn.Length; j++){
                theOrder.Turn(turn[j].name, turn[j].direction);
                //Debug.Log(move[i].direction);
            }
            
            for(int j=0; j<transparent.Length; j++){
                if(transparent[j].remove)
                    theOrder.SetTransparent(transparent[j].name);
                else if (!transparent[j].remove)
                    theOrder.SetUnTransparent(transparent[j].name);
                //Debug.Log(move[i].direction);
            }
            //theOrder.Turn("npc1", direction);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
