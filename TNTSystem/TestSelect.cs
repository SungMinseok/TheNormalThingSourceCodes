using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSelect : MonoBehaviour
{
    [SerializeField]    
    public Select select;

    private OrderManager theOrder;
    private SelectManager theSelect;

    public bool flag;

    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();    
        theSelect = FindObjectOfType<SelectManager>();   
        
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(!flag){
            StartCoroutine(ACoroutine());
        }
    }

    
    IEnumerator ACoroutine(){
        yield return new WaitForSeconds(0.01f); //키 중복방지 대기
        flag =true;
        theOrder.NotMove();
        theSelect.ShowSelect(select);

        yield return new WaitUntil(() => !theSelect.selecting); // 선택지 선택 끝날 때까지

        theOrder.Move();

        Debug.Log(theSelect.GetResult());
        flag = false;
    }
}
