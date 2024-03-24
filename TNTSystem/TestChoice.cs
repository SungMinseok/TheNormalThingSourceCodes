using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChoice : MonoBehaviour
{
    [SerializeField]
    public Choice choice;
    private OrderManager theOrder;

    private ChoiceManager theChoice;

    public bool flag;
    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();    
        theChoice = FindObjectOfType<ChoiceManager>();    
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(!flag){
            StartCoroutine(ACoroutine());
        }
    }
    
    IEnumerator ACoroutine(){
        flag =true;
        theOrder.NotMove();

        theChoice.ShowChoice(choice);



        yield return new WaitUntil(() => !theChoice.choiceIng); // 선택지 선택 끝날 때까지


        theOrder.Move();

        Debug.Log(theChoice.GetResult());
        flag = false;
    }
}
