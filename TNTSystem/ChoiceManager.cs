using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    public static ChoiceManager instance;
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
    
    private string question;
    private List<string> answerList;

    public GameObject go; //평소 비활성화, SetActive

    public Text question_Text;
    public Text[] answer_Text;

    public GameObject[] answer_Panel;
    public Animator animator;

    public string keySound;
    public string enterSound;
    public bool choiceIng;  //대기용

    private bool keyInput;  //키처리 활성화

    private int count;  //배열의 크기

    private int result; //선택한 선택창
    

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    void Start()
    {
        answerList= new List<string>();

        for(int i=0; i<answer_Text.Length; i++){
            answer_Text[i].text = "";
            answer_Panel[i].SetActive(false);
        }
        question_Text.text = "";
    }

    public void ShowChoice(Choice _choice){

        go.SetActive(true);
        choiceIng =true;
        result =0;
        question = _choice.question;
        for(int i =0 ; i < _choice.answers.Length; i++){
            answerList.Add(_choice.answers[i]);
            answer_Panel[i].SetActive(true);
            count = i;
        }
        animator.SetBool("Appear", true);
        
        Selection();
        StartCoroutine(ChoiceCoroutine());
    }

    public int GetResult(){                                             //선택결과
        return result;
    }
    public void ExitChoice(){

        
        question_Text.text = "";
        for(int i =0 ; i<=count ; i++){
            answer_Text[i].text = "";
            answer_Panel[i].SetActive(false);
        }
        answerList.Clear();

        choiceIng = false;
        animator.SetBool("Appear", false);
                                                                                    //이 사이에 대기 넣어서 사라지는 애니재생
        go.SetActive(false);
    }

    IEnumerator ChoiceCoroutine(){

        
        yield return new WaitForSeconds(0.2f);

        StartCoroutine(TypingQuestion());
        StartCoroutine(TypingAnswer_0());
        if(count>=1) StartCoroutine(TypingAnswer_1());
        if(count>=2) StartCoroutine(TypingAnswer_2());
        if(count>=3) StartCoroutine(TypingAnswer_3());

        yield return new WaitForSeconds(0.5f);
        keyInput = true;
        
    }
    
    IEnumerator TypingQuestion(){
        
        for(int i=0; i< question.Length; i++){
            question_Text.text += question[i];
            yield return waitTime;
        }
    }
    IEnumerator TypingAnswer_0(){
        yield return new WaitForSeconds(0.4f);
        for(int i=0; i< answerList[0].Length; i++){
            answer_Text[0].text += answerList[0][i];
            yield return waitTime;
        }
    }
    IEnumerator TypingAnswer_1(){
        yield return new WaitForSeconds(0.5f);
        for(int i=0; i< answerList[1].Length; i++){
            answer_Text[1].text += answerList[1][i];
            yield return waitTime;
        }
    }
    IEnumerator TypingAnswer_2(){
        yield return new WaitForSeconds(0.6f);
        for(int i=0; i< answerList[2].Length; i++){
            answer_Text[2].text += answerList[2][i];
            yield return waitTime;
        }
    }
    IEnumerator TypingAnswer_3(){
        yield return new WaitForSeconds(0.7f);
        for(int i=0; i< answerList[3].Length; i++){
            answer_Text[3].text += answerList[3][i];
            yield return waitTime;
        }
    }
    void Update()
    {
        if(keyInput){
            if(Input.GetKeyDown(KeyCode.UpArrow)){
                //theAudio.Play(keySound);
                if(result >0) result--;
                else result=count;
                Selection();
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow)){
                //theAudio.Play(keySound);
                if(result<count) result++;
                else result = 0;
                Selection();
            }
            else if(Input.GetKeyDown(KeyCode.Space)){
                //theAudio.Play(enterSound);
                keyInput = false;
                StopAllCoroutines();
                ExitChoice();
            }
        }
    }

    public void Selection(){
        Color color = answer_Panel[0].GetComponent<Image>().color;
        color.a = 0.75f;                                                        //모든 패널 투명하게
        for(int i = 0; i<=count; i++){
            answer_Panel[i].GetComponent<Image>().color = color;
        }
        color.a = 1f;
        answer_Panel[result].GetComponent<Image>().color = color;
    }
}
