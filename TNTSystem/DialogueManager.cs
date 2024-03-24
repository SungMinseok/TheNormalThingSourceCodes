#if UNITY_ANDROID || UNITY_IOS
#define DISABLEKEYBOARD
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour        // 
{   
    /*HOW TO USE

    names와 sentences의 size는 반드시 같게 설정해야하고, 넣을 image가 0개일 경우 images의 size는 건들지 않아도됨.
    단, image를 하나라도 넣는 경우 반드시 null_image 추가해야함!
    
    */ 
    public string typeSound;
    public static DialogueManager instance;
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
        //instance = this;
    }
    public GameObject window;
    public GameObject signWindow;
    public Image background;
    //public Image image;
    public SpriteRenderer image; 
    public SpriteRenderer frame; 
    //public SpriteRenderer null_image;
    public Color color_background;
    public Color color_image;
    public Text nameText;
    public Text text;
    //public string tempStr;
    public int colorCnt;
    private bool null_check;              // 그림 널 체크 ,  널이면 true
    
    //public SpriteRenderer rendererSprite;
    //public SpriteRenderer rendererDialogueWindow;
    //public GameObject go;

    private List<string> listNames;
    private List<string> listSentences;
    //private List<Sprite> listImages;
    private List<Sprite> listImages;
    private List<bool> listCheck;
    //private List<Sprite> listSprites;
    //private List<Sprite> listDialogueWindows;
    private int count; //대화 진행상황

    //public Animator animSprite;
    public Animator animDialogueWindow;

    private FadeManager theFade; 
    AudioManager theAudio;

    //public string typeSound;
    //public string enterSound;

    //private EventManager theEvent;
    
    public bool talking = false;
    private bool keyActivated = false;
    
    private WaitForSeconds waitTime = new WaitForSeconds(0.02f);
    private WaitForSeconds waitTime2 = new WaitForSeconds(0.005f);

    public GameObject nextPointer;
    
    private GameMultiLang langS;
    public SpriteRenderer imageColor;
    public SpriteRenderer frameColor;
    public GameObject dialogueImage;
    [Header("MOBILE")]
    public bool mobileTouch;

    //PlayerManager thePlayer;
    void Start(){
        count = 0 ;
        window.SetActive(false);
        dialogueImage.SetActive(false);
        //name.SetActive(false);
        nameText.text = "";
        text.text = "";
        listNames = new List<string>();
        listSentences = new List<string>();
        listImages = new List<Sprite>();
        listCheck = new List<bool>();
        theFade=FadeManager.instance;
        theAudio=AudioManager.instance;
        langS=GameMultiLang.instance;
        //thePlayer=PlayerManager.instance;
#if DISABLEKEYBOARD
        imageColor.GetComponent<RectTransform>().localScale = new Vector2(20.23455f,20.23455f);
#endif
    }

    public void ShowDialogue(Dialogue dialogue,bool mute = false){
        if(dialogue.names.Length == 0&&!dialogue.isSign){
            return;
            //talking = false;
        }
        else{
            LocalizeLanguages(dialogue);

            PlayerManager.instance.getSpace = false;
            PlayerManager.instance.isInteracting = true;

            window.gameObject.SetActive(true);            
            dialogueImage.SetActive(true);
            if(mute) nextPointer.SetActive(false);
            if(dialogue.images.Length==0) null_check = true;    // image가 아예 없으면 아예 실행 안하도록

            talking = true;
            if(!null_check){                                // image가 하나라도 있으면 실행하도록
                for(int i=0; i< dialogue.images.Length; i++){
                    if(dialogue.images[i]!=null){
                            
                        listImages.Add(dialogue.images[i]);
                        listCheck.Add(false);                           //i번째에는 이미지가 있다.
                    }
                    else
                    {
                        listImages.Add(null);
                        listCheck.Add(true);                            //i번째에는 이미지가 없다.(널이미지가 있다)
                    }    
                    
                }
            }


            int mode = 0;
            if(dialogue.isSign) mode = 1;
            else if(dialogue.isSignWithName) mode = 2;

            if(mode!=1){
                for(int i=0; i< dialogue.names.Length; i++){
                    listNames.Add(dialogue.names[i]);
                }
            }

            for(int i=0; i< dialogue.sentences_set.Length; i++){
                listSentences.Add(dialogue.sentences_set[i]);
            }

            if(mode != 1) animDialogueWindow.SetBool("Appear", true);
            else signWindow.SetActive(true);
            
            StopAllCoroutines();
            StartCoroutine(StartDialogueCoroutine(mute, mode));
        }

    }

    public void ExitDialogue(){
        //if(listImages[count]!=null)
        ImageOut();
        count = 0;
        nameText.text = "";
        text.text = "";

        listNames.Clear();
        listSentences.Clear();
        if(!null_check)
            listImages.Clear();
        listCheck.Clear();
//        listDialogueWindows.Clear();
        animDialogueWindow.SetBool("Appear", false);
        signWindow.SetActive(false);
        window.SetActive(false);
        dialogueImage.SetActive(false);
        nextPointer.SetActive(true);
        talking = false;
        null_check = false;
        PlayerManager.instance.isInteracting = false;
        
    }

    IEnumerator StartDialogueCoroutine(bool mute = false, int mode = 0){    //0 : default, 1 : isSign, 2 : isSignWithName
        
        
        if(!null_check){
            if(count >0){
                if(listImages[count] != listImages[count-1] && listCheck[count] ==false){   //해당 순서에 이미지가 있고, 이미지가 전과 다름
                    //Debug.Log("count = " + count);
                   
//                    Debug.Log("listchech = " + listCheck[count-1]);
                    if(listCheck[count-1]==false){  //전 번째에는 이미지가 있다.
                        
                        ImageOut();
                        
                        yield return new WaitForSeconds(0.2f);
                        
                            
                       
                    }
                    image.GetComponent<SpriteRenderer>().sprite = listImages[count];
                    ImageOn();
                    
                
                }
                else if(listImages[count] != listImages[count-1] && listCheck[count] ==true){//해당 순서에 이미지가 없고, 이미지가 전과 다름

                    ImageOut();
                    yield return new WaitForSeconds(0.05f);
                }
                else{
                        
                    yield return new WaitForSeconds(0.05f);  
                                                    
                }
            }
            else{
                
                if(listCheck[count]==false){   
                    image.GetComponent<SpriteRenderer>().sprite = listImages[count]; 
                    ImageOn();
                }      
                else {
                     
                    yield return new WaitForSeconds(0.05f);
                }
                
            }

        }
        
        
        yield return new WaitForSeconds(0.001f);
        nameText.text = "";
        if(mode==0){ //일반 대화
            nameText.text = listNames[count];
            
            text.alignment = TextAnchor.UpperLeft;
        }
        else if(mode==1){
            
            text.alignment = TextAnchor.MiddleCenter;
        }
        else if(mode==2){
            
            nameText.text = listNames[count];
            text.alignment = TextAnchor.MiddleCenter;
        }
        if(!mute){
            theAudio.Play(typeSound);
        }



        //name.text = listNames[count];
        for(int i=0; i<listSentences[count].Length;i++){                        // 대화 출력 중 한 번 더 누르면 전체 출력.
#if DISABLEKEYBOARD
            if(mode!=0||((PlayerManager.instance.getSpace||mobileTouch)&&!keyActivated))
#else
            if(mode!=0||((Input.GetKeyDown(KeyCode.Space)||(Input.GetKeyDown(KeyCode.Return))&&!keyActivated)) ||(Input.GetMouseButtonDown(0)&&!keyActivated))

#endif
            //if(mode!=0||((Input.GetKeyDown(KeyCode.Space)||(Input.GetKeyDown(KeyCode.Return))&&!keyActivated)) ||(Input.GetMouseButtonDown(0)&&!keyActivated))
            {
                
        mobileTouch = false;
        PlayerManager.instance.getSpace = false;
                // color_background.a = 0.6f;
                // color_image.a = 1f;
                // background.color = color_background;
                // image.color = color_image;
                text.text = "";
                text.text = listSentences[count];
                if(mode!=0) theAudio.Play("woodtouch0");
                break;
                //yield return new WaitForSeconds(0.01f);
                
            }
            else{
                //////////////////////
                if(listSentences[count][i]=='<'){
                    string tempStr = "<";
                    int j=0;
                    while(listSentences[count][i+j]!='>'){
                        j++;
                        tempStr += listSentences[count][i+j];
                    }
                    while(listSentences[count][i+j+1]!='>'){
                        j++;
                        tempStr += listSentences[count][i+j];
                    }
                    tempStr += '>';
                    text.text += tempStr;
                    //Debug.Log(tempStr);
                    i+=j+1;
                }
                else{
                    //Debug.Log("listSentences[count][i] : "+listSentences[count][i]);
                    text.text += listSentences[count][i];//한글자씩 출력. 
                    //Debug.Log(waitTime);
                    yield return waitTime;
                    //yield return new WaitForSeconds(0.0000001f);
                }


                //////////////////////


                // text.text += listSentences[count][i];//한글자씩 출력. 
                // yield return waitTime;
                
            }
        }
#if DISABLEKEYBOARD
        if(!DatabaseManager.instance.doneIntro){ 
            DatabaseManager.instance.doneIntro=true;
            HelpManager.instance.PopUpHelper(0);
        }
#endif
        if(!mute) theAudio.Stop(typeSound);
        keyActivated = true;
    }
    void FixedUpdate()
    {
        if(talking && keyActivated){

            //if(Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Return)||PlayerManager.instance.getSpace||Input.GetMouseButtonDown(0)){
#if DISABLEKEYBOARD            
            if(PlayerManager.instance.getSpace || mobileTouch){
        mobileTouch = false;
        PlayerManager.instance.getSpace = false;
                keyActivated =false;
                
                count++;
                /*if(count>=1&&listNames[count]!=listNames[count-1])*/ 
                nameText.text="";
                text.text="";
                if(count==listSentences.Count){
                    StopAllCoroutines();                
                    ExitDialogue();
                }
                else{
                    StopAllCoroutines();                
                    StartCoroutine(StartDialogueCoroutine());
                }
            }
#else
            if(Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Return)||Input.GetMouseButtonDown(0)){
                keyActivated =false;
                
                count++;
                /*if(count>=1&&listNames[count]!=listNames[count-1])*/ 
                nameText.text="";
                text.text="";
                if(count==listSentences.Count){
                    StopAllCoroutines();                
                    ExitDialogue();
                }
                else{
                    StopAllCoroutines();                
                    StartCoroutine(StartDialogueCoroutine());
                }
            }
#endif
        }
    }

    public void MouseClick(){
    }
    public void ImageOn(){                   //speed 0.1f보다 작은값으로 하면 버그걸림 ㄷㄷ
        //StopAllCoroutines();
        //Debug.Log("ImageOn");
        StopCoroutine(ImageOutCoroutine1());
        StopCoroutine(ImageOutCoroutine2());
        //theFade.FadeOut(0.1f, 0.6f);
        StartCoroutine(ImageOnCoroutine1());
        StartCoroutine(ImageOnCoroutine2());
    }
    IEnumerator ImageOnCoroutine1(){        //image On
        color_image = image.color;
        while(color_image.a <1f){
            //Debug.Log("ImageOnCoroutine1");
            color_image.a += 0.1f;
            image.color = color_image; 
            frame.color = color_image;               
            yield return waitTime2;
        }
    }
    IEnumerator ImageOnCoroutine2(){        //background On
        color_background = background.color;
        while(color_background.a <0.6f){
            color_background.a += 0.1f;
            background.color = color_background;      
            yield return waitTime2;
        }
        //color_background = 0.6;
        //background.color = color_background;
    }        
    public void ImageOut(){
        
        //Debug.Log("ImageOut");
        //StopCoroutine(ImageOnCoroutine1());
        //StopCoroutine(ImageOnCoroutine2());
        //StopAllCoroutines();
        //StartCoroutine(FadeInCoroutine(_speed));
        StartCoroutine(ImageOutCoroutine1());
        StartCoroutine(ImageOutCoroutine2());
    }
    IEnumerator ImageOutCoroutine1(){
        
        color_background = background.color;
        while(color_background.a >0f){
        //Debug.Log("ImageOutCoroutine1");
            color_background.a -= 0.2f;
            background.color = color_background;      
            yield return waitTime2;
        }
    }
    IEnumerator ImageOutCoroutine2(){
        color_image = image.color;
        while(color_image.a >0f){
            color_image.a -= 0.2f;
            image.color = color_image;  
            frame.color = color_image;            
            yield return waitTime2;
        }
    }

    public void RenderTest(){
        
            window.gameObject.SetActive(true);
    }
    
    void LocalizeLanguages(Dialogue dialogue){
    //     Debug.Log("1111"+GameMultiLang.instance.nowLang);
    //     switch(GameMultiLang.instance.nowLang){
    //         case "kr" :
    //             sentences_set = sentences;
    //             break;
    //         case "en" :
    //             sentences_set = sentences_en;
    //             break;
    //     }
    //Debug.Log("로컬라이즈");
        if(langS.nowLang=="en"){
            
            //언어별 자막속도 설정(영어 2배)
            
            waitTime = new WaitForSeconds(0.001f);
            
            for(int i=0; i<dialogue.names.Length; i++){
                switch(dialogue.names[i]){
                    case "나" :
                        dialogue.names[i] = "Me";
                        break;
                    case "거북이" :
                        dialogue.names[i] = "Turtle";
                        break;
                    case "앵무새" :
                        dialogue.names[i] = "Parrot";
                        break;
                    case "꽃" :
                        dialogue.names[i] = "Flower";
                        break;
                    case "얼굴꽃" :
                        dialogue.names[i] = "Head flower";
                        break;
                    case "울부짖는 꽃" :
                        dialogue.names[i] = "Howling flower";
                        break;
                    case "고슴도치" :
                        dialogue.names[i] = "Hedgehog";
                        break;
                    case "제단" :
                        dialogue.names[i] = "Altar";
                        break;
                    case "주민" :
                        dialogue.names[i] = "Villager";
                        break;
                    case "금붕어" :
                        dialogue.names[i] = "Goldfish";
                        break;
                    case "오드아이" :
                        dialogue.names[i] = "Odd Eye";
                        break;
                    case "오시리스의 제단" :
                        dialogue.names[i] = "Altar of Ossiris";
                        break;
                    case "태양신의 제단" :
                        dialogue.names[i] = "Altar of Sol";
                        break;
                    case "아누비스의 제단" :
                        dialogue.names[i] = "Altar of Anubis";
                        break;
                    case "이상한 표지판" :
                        dialogue.names[i] = "Strange sign";
                        break;
                    case "표지판" :
                        dialogue.names[i] = "Sign";
                        break;
                    case "이름 모를 제단" :
                        dialogue.names[i] = "Unknown altar";
                        break;
                }
            }

            if(dialogue.sentences_en.Length != dialogue.sentences.Length || dialogue.sentences_en[0]==""){
                //dialogue.sentences_set = new string[dialogue.sentences.Length];
                dialogue.sentences_set = dialogue.sentences;
                Debug.Log("번역파일불완전함 : "+langS.nowLang);
            }
            else{
                dialogue.sentences_set = dialogue.sentences_en;
            }
            
        }
        else{
            dialogue.sentences_set = dialogue.sentences;
        }
    }

    // public void ForceAlphaOut(){
    //     imageColor.color = new Color(1,1,1,0);
    //     frameColor.color = new Color(1,1,1,0);
    // }
    public void ActivateMobileTouch(){
#if DISABLEKEYBOARD
        mobileTouch = true;
        //StartCoroutine(AMTCO());
#endif
    }
    IEnumerator AMTCO(){
        mobileTouch = true;
        yield return new WaitForSeconds(0.001f);
        mobileTouch = false;
    }
}
