#if UNITY_ANDROID || UNITY_IOS
#define DISABLEKEYBOARD
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    public string typeSound;
    #region Instance
    public static SelectManager instance;
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
    #endregion

    public Text name_Text;
    public Text question_Text;
    
    public Text[] answer_Text;
    
    public GameObject[] pointer;
    public GameObject Dwindow;
    
    public GameObject Swindow;

    //public SpriteRenderer rendererDialogueWindow;

    private string nameText;
    private string question;
    private List<string> listAnswers;

    
    public Animator Danimator;
    public Animator Sanimator;
    
    public bool questDone;  //질문대기용
    public bool selecting;  //대기용

    private bool keyInput;  //키처리 활성화

    private int count;  //배열의 크기

    public int result; //선택한 선택창
    
    private bool keyActivated = false;

    //private bool putActivated = false; // ㅋ
    private DialogueManager theDM;
    AudioManager theAudio;
    
    private WaitForSeconds waitTime = new WaitForSeconds(0.02f);
    
    private GameMultiLang langS;
    public Button[] selectBtnsMouse;
    [Header("MOBILE")]
    public GameObject[] selectBtns;
    public GameObject[] selectNums;
    public bool mobileTouch;
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theAudio=AudioManager.instance;
        listAnswers= new List<string>();

        Dwindow.SetActive(false);
        Swindow.SetActive(false);
        theDM.nameText.text = "";
        theDM.text.text = "";
        name_Text.text = "";
        question_Text.text = "";
        for(int i=0; i<answer_Text.Length; i++){
            answer_Text[i].text = "";
            pointer[i].SetActive(false);
        }
        for(int i=0; i<selectBtns.Length; i++){
            selectBtns[i].SetActive(false);
            selectNums[i].SetActive(false);
        }
        langS=GameMultiLang.instance;
    }


    public void ShowSelect(Select _select){
        
        LocalizeLanguages(_select);
            PlayerManager.instance.getSpace = false;
        PlayerManager.instance.isInteracting = true;
        Dwindow.SetActive(true);
        Danimator.SetBool("Appear", true);
        questDone=true;
        //go.SetActive(true);
        selecting =true;
        result =0;
        nameText = _select.nameText;
        question = _select.question_set;
            // if( _select.answers_set.Length == 0){
                
            //     for(int i =0 ; i < _select.answers.Length; i++){
            //         listAnswers.Add(_select.answers[i]);
            //         //pointer[i].SetActive(true);
            //         count = i;
            //     }
            // }
            // else{

                for(int i=0; i< _select.answers_set.Length; i++){
                    listAnswers.Add(_select.answers_set[i]);
                    
                    count = i;
                }
            //}

        Selection();
        
        StartCoroutine(QuestionCoroutine());
        //yield return new WaitUntil(() => questDone);
        
    }

    public int GetResult(){                                             //선택결과
        return result;
    }
    
    public void ExitSelect(){

        theAudio.Stop(typeSound);
        question_Text.text = "";
        name_Text.text = "";
        for(int i =0 ; i<=count ; i++){
            answer_Text[i].text = "";
            pointer[i].SetActive(false);
        }
        listAnswers.Clear();

        // Danimator.SetBool("Appear", false);
        // Sanimator.SetBool("Appear", false);
        
        Swindow.SetActive(false);
        Dwindow.SetActive(false);
        for(int i=0; i<selectBtns.Length;i++){
            
            selectBtns[i].SetActive(false);
            selectNums[i].SetActive(false);
        }
        selecting = false;
        PlayerManager.instance.isInteracting = false;
        for(int i=0;i<3;i++){
            selectBtnsMouse[i].interactable = false;
        }
        
    }

    IEnumerator QuestionCoroutine(){
        yield return new WaitForSeconds(0.01f);
        //rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count];
        name_Text.text = nameText;
        
        theAudio.Play(typeSound);
        for(int i=0; i<question.Length;i++){                        // 대화 출력 중 한 번 더 누르면 전체 출력.
#if DISABLEKEYBOARD
            if(((PlayerManager.instance.getSpace||mobileTouch)&&!keyActivated))
#else
            if(((Input.GetKeyDown(KeyCode.Space)||(Input.GetKeyDown(KeyCode.Return))&&!keyActivated))||(Input.GetMouseButtonDown(0)&&!keyActivated))

#endif
            //if(((Input.GetKeyDown(KeyCode.Space)||(Input.GetKeyDown(KeyCode.Return))&&!keyActivated))||(Input.GetMouseButtonDown(0)&&!keyActivated))
            {
        mobileTouch = false;
        PlayerManager.instance.getSpace = false;
                question_Text.text = question;
                //answer_Text[i].text = "";
                //answer_Text[i].text = listAnswers[count];
                break;
                
            }
            else{
                
                question_Text.text += question[i];//한글자씩 출력.                
                yield return waitTime;
                
            }
        }
        theAudio.Stop(typeSound);
        keyActivated = true;
        //questDone = true;
        
    }


    IEnumerator SelectCoroutine(){
        for(int i=0;i<3;i++){
            selectBtnsMouse[i].interactable = true;
        }
        
        Dwindow.SetActive(false);
        Swindow.SetActive(true);
#if DISABLEKEYBOARD
        for(int i=0; i<=count;i++){
            
            selectBtns[i].SetActive(true);
            selectNums[i].SetActive(true);
        }
#else
        for(int i =0 ; i <= count; i++){
            //listAnswers.Add(_select.answers[i]);
            pointer[i].SetActive(true);
            //count = i;
        }
#endif
        //yield return new WaitForSeconds(0.2f);

        //StartCoroutine(TypingQuestion());
        theAudio.Play(typeSound);
        StartCoroutine(TypingAnswer_0());
        if(count>=1) StartCoroutine(TypingAnswer_1());
        if(count>=2) StartCoroutine(TypingAnswer_2());
        if(count>=3) StartCoroutine(TypingAnswer_3());

        yield return new WaitForSeconds(0.01f);
        keyInput = true;
        
        //questDone = false;
    }
    /*
    IEnumerator TypingQuestion(){
        
        for(int i=0; i< question.Length; i++){
            question_Text.text += question[i];
            yield return waitTime;
        }
    }*/
    IEnumerator TypingAnswer_0(){
        // yield return new WaitForSeconds(0.1f);
        // for(int i=0; i< listAnswers[0].Length; i++){
        //     answer_Text[0].text += listAnswers[0][i];
        //     yield return waitTime;
        // }
        answer_Text[0].text = listAnswers[0];
        yield return null;
        
        if(count==0) theAudio.Stop(typeSound);
    }
    IEnumerator TypingAnswer_1(){
        // yield return new WaitForSeconds(0.11f);
        // for(int i=0; i< listAnswers[1].Length; i++){
        //     answer_Text[1].text += listAnswers[1][i];
        //     yield return waitTime;
        // }
        answer_Text[1].text = listAnswers[1];
        yield return null;
        if(count==1) theAudio.Stop(typeSound);
    }
    IEnumerator TypingAnswer_2(){
        // yield return new WaitForSeconds(0.12f);
        // for(int i=0; i< listAnswers[2].Length; i++){
        //     answer_Text[2].text += listAnswers[2][i];
        //     yield return waitTime;
        // }
        answer_Text[2].text = listAnswers[2];
        yield return null;
        if(count==2) theAudio.Stop(typeSound);
    }
    IEnumerator TypingAnswer_3(){
        // yield return new WaitForSeconds(0.13f);
        // for(int i=0; i< listAnswers[3].Length; i++){
        //     answer_Text[3].text += listAnswers[3][i];
        //     yield return waitTime;
        // }
        answer_Text[3].text = listAnswers[3];
        yield return null;
        
        if(count==3) theAudio.Stop(typeSound);
    }

    void Update()
    {
        
        if(questDone&&keyActivated){
#if DISABLEKEYBOARD              
            if(PlayerManager.instance.getSpace || mobileTouch){

        mobileTouch = false;
        PlayerManager.instance.getSpace = false;
                keyActivated =false;
                questDone =false;
                
                question_Text.text = "";
                name_Text.text = "";
                
                StopAllCoroutines();                
                        
                Swindow.SetActive(true);
                Sanimator.SetBool("Appear", true);
                
                Danimator.SetBool("Appear", false);
                StartCoroutine(SelectCoroutine());
                //Debug.Log("Z");
                //Progressing();
            }          
#else
            if(Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Return)||Input.GetMouseButtonDown(0)){

                keyActivated =false;
                questDone =false;
                
                question_Text.text = "";
                name_Text.text = "";
                
                StopAllCoroutines();                
                        
                Swindow.SetActive(true);
                Sanimator.SetBool("Appear", true);
                
                Danimator.SetBool("Appear", false);
                StartCoroutine(SelectCoroutine());
                //Debug.Log("Z");
                //Progressing();
            }
#endif
                
        }
        if(keyInput){
#if !DISABLEKEYBOARD 
            if(Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.W)){
                //theAudio.Play(keySound);
                if(result >0) result--;
                else result=count;
                Selection();
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow)||Input.GetKeyDown(KeyCode.S)){
                //theAudio.Play(keySound);
                if(result<count) result++;
                else result = 0;
                Selection();
            }
            else if(Input.GetKeyDown(KeyCode.Space)){
                //theAudio.Play(enterSound);
                keyInput = false;
                StopAllCoroutines();
                ExitSelect();
            }
#endif
        }
    }

    public void ClickResult(int num){
        result = num;
//Debug.Log(num+ result);
        keyInput = false;
        StopAllCoroutines();
        ExitSelect();
    }
    // public void Progressing(){

    //     keyActivated =false;
    //     questDone =false;
        
    //     question_Text.text = "";
    //     name_Text.text = "";
        
    //     StopAllCoroutines();                
                
    //     Swindow.SetActive(true);
    //     Sanimator.SetBool("Appear", true);
        
    //     Danimator.SetBool("Appear", false);
    //     StartCoroutine(SelectCoroutine());
            
                
    // }
    
    public void Selection(){
        Color color = pointer[0].GetComponent<Image>().color;   //선택 안된거.
        // color.r = 0f;
        // color.g = 0f;                                          
        // color.b = 0f;
        color.a = 0f;
        for(int i = 0; i<=count; i++){
            pointer[i].GetComponent<Image>().color = color;
        }
        
        color.a = 1f;
        color.r = 1.0f;
        color.g = 1.0f;
        color.b = 1.0f;
        pointer[result].GetComponent<Image>().color = color;
    }
    void LocalizeLanguages(Select select){
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
            
            for(int i=0; i<select.nameText.Length; i++){
                switch(select.nameText){
                    case "나" :
                        select.nameText = "Me";
                        break;
                    case "거북이" :
                        select.nameText = "Turtle";
                        break;
                    case "앵무새" :
                        select.nameText = "Parrot";
                        break;
                    case "꽃" :
                        select.nameText = "Flower";
                        break;
                    case "얼굴꽃" :
                        select.nameText = "Head flower";
                        break;
                    case "울부짖는 꽃" :
                        select.nameText = "Howling flower";
                        break;
                    case "고슴도치" :
                        select.nameText = "Hedgehog";
                        break;
                    case "제단" :
                        select.nameText = "Altar";
                        break;
                    case "주민" :
                        select.nameText = "Villager";
                        break;
                    case "금붕어" :
                        select.nameText = "Goldfish";
                        break;
                    case "오드아이" :
                        select.nameText = "Odd Eye";
                        break;
                    case "오시리스의 제단" :
                        select.nameText = "Altar of Ossiris";
                        break;
                    case "태양신의 제단" :
                        select.nameText = "Altar of Sol";
                        break;
                    case "아누비스의 제단" :
                        select.nameText = "Altar of Anubis";
                        break;
                    case "이상한 표지판" :
                        select.nameText = "Strange sign";
                        break;
                    case "표지판" :
                        select.nameText = "Sign";
                        break;
                }
            }

            select.question_set = select.question_en;

            if( select.answers_en.Length!= select.answers.Length){
                select.answers_set = select.answers;
                Debug.LogWarning("번역파일불완전함 : "+langS.nowLang);
            }
            else{
                select.answers_set = select.answers_en;
            }
        }
        else{
            select.question_set = select.question;
            select.answers_set = select.answers;
        }
    }
    public void ActivateMobileTouch(){
#if DISABLEKEYBOARD
        mobileTouch = true;
        //StartCoroutine(AMTCO());
#endif
    }
}
