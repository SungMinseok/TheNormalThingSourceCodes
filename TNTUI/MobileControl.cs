using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum ControllerType
{
    Joystick,
    JoyPad,
    Shift,
    Space,
    Btn1,
    Btn2,
    Btn3,
    TouchPad,

}

//조이스틱 : 조이패드 + 쉬프트 (비활성화 : 퍼즐, 게임, 책, 게임오버 // 반투명 : 대화)
//상호작용 : 스페이스 (반투명 : 퍼즐, 게임)
#if UNITY_ANDROID || UNITY_IOS
public class MobileControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerClickHandler
{

    [SerializeField] private ControllerType type;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform bg;
    [SerializeField] private RectTransform js;
    public GameObject shiftBtn;
    public GameObject spaceBtn;
    public bool isTouch;
    public bool isDragging;
    private float r;
    public PlayerManager thePlayer;
    public GameObject mobileController;
    public Camera theCamera;
    public GameObject eventSystem;
    void Start()
    {
        //thePlayer = PlayerManager.instance;
        theCamera = GameObject.Find("Camera").GetComponent<Camera>();
        mobileController.SetActive(true);
        
        if (type != ControllerType.TouchPad){
            r = bg.rect.width * 0.5f;
        }
        
        if (type == ControllerType.JoyPad){
            eventSystem.SetActive(false);
        }
    }

    void Update()
    {
        if (/*type == ControllerType.Joystick ||*/ type == ControllerType.JoyPad)
        {
            if (!thePlayer.notMove)
            {
                if (isTouch)
                {
                    if (!thePlayer.isRunning)
                    {
        //Debug.Log("걷기");
                        //씬 전환후 이동 유지하기
                        if(thePlayer.movement == Vector2.zero){
                            thePlayer.movement = new Vector2(thePlayer.animator.GetFloat("Horizontal"),thePlayer.animator.GetFloat("Vertical"));
                        }
                        //
                        thePlayer.rb.MovePosition(thePlayer.rb.position + thePlayer.movement * thePlayer.speed * Time.fixedDeltaTime);
                        thePlayer.animator.SetFloat("Speed", 1f);
                    }
                    else if (thePlayer.isRunning)
                    {
        //Debug.Log("달리기");
                        //씬 전환후 이동 유지하기
                        if(thePlayer.movement == Vector2.zero){
                            thePlayer.movement = new Vector2(thePlayer.animator.GetFloat("Horizontal"),thePlayer.animator.GetFloat("Vertical"));
                        }
                        //
                        thePlayer.rb.MovePosition(thePlayer.rb.position + thePlayer.movement * thePlayer.runSpeed * Time.fixedDeltaTime);
                        thePlayer.animator.SetFloat("Speed", 2f);
                    }
                }
                else
                {
                    if(js.localPosition == Vector3.zero){
        //Debug.Log("멈춤");

                        thePlayer.animator.SetFloat("Speed", 0f);
                    }

                }
            }
            else if (thePlayer.notMove)
            {

                //isTouch = false;
                //js.localPosition = Vector3.zero;

                //mobileController.SetActive(false);
                //thePlayer.animator.SetFloat("Speed", 0f);
            }
        }

    }
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("ondrag");
        if (/*type == ControllerType.Joystick ||*/ type == ControllerType.JoyPad )
        {
            if (!thePlayer.notMove)
            {
                //if(type == ControllerType.JoyPad ){
                    
                    Vector3 temp = theCamera.ScreenToWorldPoint(eventData.position);
                    js.gameObject.transform.position = new Vector3(temp.x, temp.y,0);
                    js.anchoredPosition = Vector2.ClampMagnitude(js.anchoredPosition, r);
                //}
                // else{
                    
                //     js.anchoredPosition += eventData.delta / canvas.scaleFactor;
                //     js.anchoredPosition = Vector2.ClampMagnitude(js.anchoredPosition, r);
                // }

                Vector3 v = js.anchoredPosition.normalized;
                // Debug.Log("local"+v);
                // Debug.Log("anchoredPosition"+js.anchoredPosition);

                if (v.y < 0.7 && v.y > 0)
                {
                    if (js.localPosition.x > 0)
                    {
                        thePlayer.movement.x = 1f;
                        thePlayer.movement.y = 0;
                    }
                    else if (js.localPosition.x < 0)
                    {
                        thePlayer.movement.x = -1f;
                        thePlayer.movement.y = 0;
                    }
                }
                else if (v.y >= 0.7)
                {

                    thePlayer.movement.x = 0;
                    thePlayer.movement.y = 1f;
                }

                else if (v.y > -0.7 && v.y < 0)
                {
                    if (js.localPosition.x > 0)
                    {
                        thePlayer.movement.x = 1f;
                        thePlayer.movement.y = 0;
                    }
                    else if (js.localPosition.x < 0)
                    {
                        thePlayer.movement.x = -1f;
                        thePlayer.movement.y = 0;
                    }
                }
                else if (v.y <= -0.7)
                {

                    thePlayer.movement.x = 0;
                    thePlayer.movement.y = -1f;
                }

                thePlayer.animator.SetFloat("Horizontal", thePlayer.movement.x);
                thePlayer.animator.SetFloat("Vertical", thePlayer.movement.y);

            }
        }


    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
        //Vibration.Vibrate(200);
        if (/*type == ControllerType.Joystick ||*/ type == ControllerType.JoyPad)
        {

            isTouch = true;
            OnDrag(eventData);
        }
        else if (type == ControllerType.Shift)
        {
            if(BookManager.instance.book.activeSelf){
                BookManager.instance.BookOff();
            }
            else{

                thePlayer.isRunning = true;
            }
        }
        // else if (type == ControllerType.Space)
        // {
        //     //thePlayer.getSpace = true;
        //     Invoke("DelayClick",0.2f);
        // }
        else if (type == ControllerType.Space)
        {
            //if(!thePlayer.isInteracting) 
            StartCoroutine(DelayClick());
            //Debug.Log("누름");
        }
        else if (type == ControllerType.TouchPad)
        {
            HelpManager.instance.ClickHelper();
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("OnPointerUp");
        if (/*type == ControllerType.Joystick||*/ type == ControllerType.JoyPad)
        {
            isTouch = false;
            js.localPosition = Vector3.zero;
            thePlayer.animator.SetFloat("Speed", 0f);
        }
        else if (type == ControllerType.Shift)
        {
            thePlayer.isRunning = false;
        }
        // else if (type == ControllerType.Space)
        // {
        //     thePlayer.getSpace = false;
        // }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //Vibration.Vibrate(200);


        // if (type == ControllerType.Space)
        // {
        //     //if(!thePlayer.isInteracting) 
        //     StartCoroutine(DelayClick());
        // }
        // else 
        if (type == ControllerType.Btn1){
            SelectManager.instance.ClickResult(0);
        }
        else if (type == ControllerType.Btn2){
            SelectManager.instance.ClickResult(1);
        }
        else if (type == ControllerType.Btn3){
            SelectManager.instance.ClickResult(2);
        }
    }
    IEnumerator DelayClick(){
        //yield return new WaitForSeconds(0.001f);
        thePlayer.getSpace = true;
        yield return null;
       // yield return new WaitForSeconds(0.001f);
        thePlayer.getSpace = false;
    }

}
#endif
