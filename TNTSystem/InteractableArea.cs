using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum InteractableType
{
    CursorAccess,
    UIBtn,
}


//[RequireComponent(typeof(BoxCollider2D))]
public class InteractableArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler//,IPointerHandler
{    
    [SerializeField]
    public InteractableType type;
    [Header ("클릭시 이미지 사라지면 true")]
    public bool forcedOff;//클릭시 이미지가 사라지는 경우에 true해줌.
    void Start(){
        // box=GetComponent<BoxCollider2D>();
        // if(TryGetComponent(out RectTransform rect)){
            
        //     box.size = new Vector2(rect.sizeDelta.x,rect.sizeDelta.y);
        // }
        // box.isTrigger = true;
    }
    // public InteractableType CheckType()
    // {
    //     switch (type)
    //     {
    //         case InteractableType.CursorAccess:
    //             break;

    //         case InteractableType.UIBtn:
    //             break;

    //         default:
    //             break;
    //     }
    //     return type;
    // }
    public void OnPointerEnter (PointerEventData eventData) 
    {
//        Debug.Log("OnMouseEnter : "+gameObject.name);
        if(type==InteractableType.CursorAccess){

            CursorManager.instance.interactable = true;
        }
        else if(type==InteractableType.UIBtn){
            AudioManager.instance.Play("button20");
        }
    }
    public void OnPointerExit (PointerEventData eventData) 
    {
//        Debug.Log("OnMouseExit : "+gameObject.name);
        if(type==InteractableType.CursorAccess){

            CursorManager.instance.interactable = false;
        }
    }
    public void OnPointerUp (PointerEventData eventData) 
    {
        if(type==InteractableType.CursorAccess){

            if(forcedOff){
                //gameObject.SetActive(false);
    //       Debug.Log("OnPointerUp : "+gameObject.name);
                CursorManager.instance.interactable = false;
            }
        }
    }
    // void OnMouseEnter(){
    //     Debug.Log("OnMouseEnter : "+gameObject.name);
    //     CursorManager.instance.interactable = true;
    //     //CursorManager.instance.ActivateInteractable();
    // }
    // void OnMouseExit(){
    //     Debug.Log("OnMouseExit : "+gameObject.name);
    //     CursorManager.instance.interactable = false;
    //     //CursorManager.instance.DeactivateInteractable();
    // }
}
