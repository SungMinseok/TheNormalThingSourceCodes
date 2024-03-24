using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;
#if UNITY_ANDROID || UNITY_IOS
    [Header("0: 타이틀, 1: 달리기, 2: 눈, 3: 끝로고")]
    public VideoClip[] videoClips_MOBILE;
#else
    [Header("0: 타이틀, 1: 달리기, 2: 눈, 3: 진엔딩, 4: 크레딧, 5: 끝로고, 6: 진엔딩(영어), 7: 크레딧(진엔딩)")]
    public VideoClip[] videoClips_PC;
#endif
    public VideoClip[] videoClips;
     private void Awake()



    {
#if UNITY_ANDROID || UNITY_IOS 
        videoClips = videoClips_MOBILE;
#else
        videoClips = videoClips_PC;
#endif
        if(instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else{
            Destroy(this.gameObject);
        }


        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = 30;
        
        //Screen.SetResolution(1280, 960, false);
        

#if !UNITY_EDITOR
        SetCursorState(2);
#endif
    }
    
    public Texture2D defaultCursor;
    public Texture2D activatedCursor;
    public Texture2D grabCursor;
    public Texture2D interactableCursor;
    public Texture2D interactableGrabCursor;
    public bool _default;       //true면 기본 커서
    public bool interactable;
    //public bool canGrab;

    //private bool onCheck;
    //private bool onUsing;
    // Start is called before the first frame update
    private DatabaseManager theDB;
    PlayerManager thePlayer;
    public 
    void Start()
    {
        theDB=DatabaseManager.instance;
        //thePlayer=PlayerManager.instance;
        _default = true;
        //Cursor.visible =false;
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        //SetCursorState(1);


    }

    void Update(){
        if(thePlayer==null){
            thePlayer = PlayerManager.instance;
        }
        
        //if(theDB.OnActivated[0]!=null){
            if(Input.GetMouseButtonUp(1)&& !_default){  //템선택 상태에서 오른쪽 버튼 클릭하면 복구
                RecoverCursor();
                for(int i=0; i<theDB.OnActivated.Length; i++)
                    theDB.OnActivated[i]=false;
            }

            if(Input.GetMouseButtonDown(0)&&!_default){
                SetToCursor(grabCursor);
            }
            if(Input.GetMouseButtonUp(0)&&!_default){
                SetToCursor(activatedCursor);
            }

        //}
        if(interactable&&_default&&!thePlayer.isInteracting){
                
            //Cursor.SetCursor(interactableCursor, Vector2.zero, CursorMode.ForceSoftware);
            
            if(Input.GetMouseButton(0)){
                SetToCursor(interactableGrabCursor);
            }
            // else if(Input.GetMouseButtonUp(0)){
            //     SetToCursor(interactableCursor);
            // }
            else{

                SetToCursor(interactableCursor);
            }
        }
        else if(_default){

            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
    
    public void RecoverCursor(){
        _default = true;
        interactable = false;
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        //theDB.OnActivated[0] = false;
        theDB.DeactivateItem();
//        Debug.Log("마우스커서 복구");
    }
    public void SetToCursor(Texture2D ct){
        //_default = true;
                
        Cursor.SetCursor(ct, Vector2.zero, CursorMode.ForceSoftware);

        //Debug.Log("마우스커서 복구");
    }
    public void SetCursorState(int num){
        if(num==0){//커서 정상
            Cursor.lockState = CursorLockMode.None;
        }
        else if(num==1){//커서 숨김
            Cursor.lockState = CursorLockMode.Locked;

        }
        else{//밖으로 못나가게
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    /*
    public void CursorWait(){
        StartCoroutine(CursorWaitCoroutine());
        
    }
    /*

    
    public void OnClickItemEvent(int num){
        //Debug.Log("??13131313????????");
        //Debug.Log(DatabaseManager.instance.OnActivated[num]);
        //onCheck = DatabaseManager.instance.OnActivated[num];
        //if(onCheck){
        //    Debug.Log("이건 어떄");
           //DatabaseManager.instance.OnActivated[0]=false;
           //DatabaseManager.instance.OnActivated[num]=false;
        //}
        /*
        
        if(DatabaseManager.instance.OnActivated!=null){
        if(DatabaseManager.instance.OnActivated[num])
            //theDB.ActivateItem(num)
           DatabaseManager.instance.OnActivated[0]=false;
           DatabaseManager.instance.OnActivated[num]=false;
           Debug.Log("아이템 사용중 클릭 인식 성공"); 

        }
        StartCoroutine(OnClickCoroutine(num));
    }

    IEnumerator OnClickCoroutine(int num){
        yield return new WaitForSeconds(0.2f);
        
        if(theDB.OnActivated[num]){
            theDB.OnActivated[num] = false;
            theDB.OnActivated[0] = false;
        }
    }*/
    // public void ActivateInteractable(){
    //     if(_default)
    //         Cursor.SetCursor(interactableCursor, Vector2.zero, CursorMode.ForceSoftware);
    // }
    // public void DeactivateInteractable(){
    //     if(_default)
    //         Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    // }

    public void SetRes(int num){
        switch(num){
            case 0 :
                Screen.SetResolution(1920, 1080, true);

                break;
            case 1 :
                Screen.SetResolution(1920, 1080, false);

                break;
            case 2 :
                Screen.SetResolution(1600, 900, false);

                break;
            case 3 :
                Screen.SetResolution(1280, 720, false);

                break;
        }
    }
}
