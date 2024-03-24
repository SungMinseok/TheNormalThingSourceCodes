using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaperManager : MonoBehaviour
{
    public string pageflipSound;
    public static PaperManager instance;
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
    //public GameObject[] pageBundle;
    //public GameObject nullLeftPaper;
    public GameObject nullRightPaper;
    public GameObject nullLeftPaper;
    public Button leftBtn;
    public Button rightBtn;
    public GameObject letter, letter0, letter1;
    public Image letter0front, letter1front;
    public Button letter0Btn, letter1Btn;
    public Button letterBtn;
    public GameObject letterBG, letterExitBtn;
    public Animator letterBtnFlash;
    public bool letterBtnUpdate;
    public GameObject disableBtn;
    //public bool activateLetter;
    [HideInInspector] public bool letterOn;
    
    public GameObject[] paperBundle;
    //private GameObject[] slots; //인벤토리 슬롯들   
    public GameObject[] paperUnlocked;
    public Text[] textUnlocked;
    public Image pageNumLeft;
    public Image pageNumRight;
    public GameObject frameLeft;
    public GameObject frameRight;
    public Sprite[] pageNums;
    public Sprite nullImage;

    public int activatedPaper;   //가지고 있는(활성화 된) 페이퍼, 0부터 시작.
    //private int activatedPage;   //가지고 있는(활성화 된) 페이지(2장의 paper per page)
    public int currentPage; //현재 보고 있는 페이지(2장의 paper per page)
    public bool blurAnim;
    private DatabaseManager theDB;
    public bool waitStory;
    
    void Start(){
        //activatedPaper = DatabaseManager.instance.activatedPaper;
        //activatedPage = activatedPaper/2-1;
        theDB = DatabaseManager.instance;
    }
    void FixedUpdate(){
        activatedPaper=theDB.activatedPaper;

        // if(letterBtnFlash){
        //     letterBtn.GetComponent<Animator>().enabled = true;
        // }
        // else
        //     letterBtn.GetComponent<Animator>().enabled = false;
        if(letterBtnUpdate&&!letterBtnFlash.GetBool("flash")){
            letterBtnFlash.SetBool("flash",true);
        }
        if(letter0.activeSelf&&!letter0Btn.gameObject.activeSelf) letter0Btn.gameObject.SetActive(true);
        if(letter1.activeSelf&&!letter1Btn.gameObject.activeSelf) letter1Btn.gameObject.SetActive(true);
    }
    public void PageBtnUpdate(){
        if(letterOn) ToggleLetterBtn();
        else{
            // if(activatedPaper>=2){
                
            //     //paperBundle[1].SetActive(true);
            //     if(currentPage==activatedPaper/2&&activatedPaper%2==0) // 맨처음
            //         nullRightPaper.SetActive(true);

            //     if(currentPage<activatedPaper/2){   //
            //         rightBtn.SetActive(true);
            //     }
            //     if(currentPage!=0){
            //         leftBtn.SetActive(true);
            //     }
            // }
            // else if(activatedPaper==1){
            //     paperBundle[0].SetActive(true);
            //     paperBundle[1].SetActive(true);
            //     nullRightPaper.SetActive(false);
            //     rightBtn.SetActive(false);
            //     leftBtn.SetActive(false);
            // }
            // else if(activatedPaper==0){
                
            //     paperBundle[0].SetActive(true);
            //     paperBundle[1].SetActive(false);
            //     nullRightPaper.SetActive(true);
            //     rightBtn.SetActive(false);
            //     leftBtn.SetActive(false);
            // }
            currentPage=0;

            rightBtn.interactable = activatedPaper >=3 ? true : false;
            leftBtn.interactable = false;
            

            nullLeftPaper.SetActive(false);
            nullRightPaper.SetActive(false);
            frameLeft.SetActive(false);
            frameRight.SetActive(false);

            for(int i=0; i<activatedPaper; i++){
                paperBundle[i].SetActive(false);
            }

            if(activatedPaper==0){
                //paperBundle[0].SetActive(true);
                nullLeftPaper.SetActive(true);
                nullRightPaper.SetActive(true);

                pageNumLeft.sprite = nullImage;
                pageNumRight.sprite = nullImage;
            }
            else if(activatedPaper==1){
                if(blurAnim){
                    blurAnim =false;
                    
        AudioManager.instance.Play("air0");
                    ObjectManager.instance.ImageFadeIn(paperBundle[0].GetComponent<Image>());
                    frameLeft.SetActive(true);
                    nullRightPaper.SetActive(true);
                    
                    pageNumLeft.sprite = pageNums[0];
                    pageNumRight.sprite = nullImage;
                }
                else{

                    paperBundle[0].SetActive(true);
                        frameLeft.SetActive(true);
                    nullRightPaper.SetActive(true);
                    
                    pageNumLeft.sprite = pageNums[0];
                    pageNumRight.sprite = nullImage;
                }
            }
            else if(activatedPaper==2){
                if(blurAnim){
                    blurAnim =false;
                 
        AudioManager.instance.Play("air0");   
                    //nullRightPaper.SetActive(false);
                    paperBundle[0].SetActive(true);
                    ObjectManager.instance.ImageFadeIn(paperBundle[1].GetComponent<Image>());
                        frameLeft.SetActive(true);
                        frameRight.SetActive(true);

                    pageNumLeft.sprite = pageNums[0];
                    pageNumRight.sprite = pageNums[1];
                }
                else{

                    //nullRightPaper.SetActive(false);
                    paperBundle[0].SetActive(true);
                    paperBundle[1].SetActive(true);
                        frameLeft.SetActive(true);
                        frameRight.SetActive(true);

                    pageNumLeft.sprite = pageNums[0];
                    pageNumRight.sprite = pageNums[1];
                }
            }
            else{
                //nullRightPaper.SetActive(false);
                paperBundle[0].SetActive(true);
                paperBundle[1].SetActive(true);
                    frameLeft.SetActive(true);
                    frameRight.SetActive(true);

                pageNumLeft.sprite = pageNums[0];
                pageNumRight.sprite = pageNums[1];
            }

            
            if(DatabaseManager.instance.letterPaper[0])
                PaperManager.instance.letter0.gameObject.SetActive(true);
            if(DatabaseManager.instance.letterPaper[1])
                PaperManager.instance.letter1.gameObject.SetActive(true);
        }
        
    }
    ////여기부터
    public void ShowRightPage(bool deleteArrow = false){    //우측버튼 클릭
        AudioManager.instance.Play(pageflipSound);
        currentPage +=1;

        for(int i=0; i<activatedPaper; i++){
            paperBundle[i].SetActive(false);
        }
////왼쪽페이지
        if(blurAnim && activatedPaper == currentPage*2 +1){
            blurAnim = false;
        AudioManager.instance.Play("air0");
            ObjectManager.instance.ImageFadeIn(paperBundle[currentPage*2].GetComponent<Image>());
            pageNumLeft.sprite = pageNums[currentPage*2];
            frameLeft.SetActive(true);
        }
        else{
            paperBundle[currentPage*2].SetActive(true);
            pageNumLeft.sprite = pageNums[currentPage*2];
            frameLeft.SetActive(true);
        }



////우측페이지
    //우측 비활성화
        if(activatedPaper==currentPage*2+1){//ex 5페이지까지 활성화되있다면 [4]나오고 [5]는 null
            nullRightPaper.SetActive(true);
            pageNumRight.sprite = nullImage;
            frameRight.SetActive(false);
        }         
    //우측 활성화
        else{
            if(blurAnim && activatedPaper == currentPage*2+2){

                blurAnim = false;
        AudioManager.instance.Play("air0");
                ObjectManager.instance.ImageFadeIn(paperBundle[currentPage*2+1].GetComponent<Image>());
                pageNumRight.sprite = pageNums[currentPage*2+1];
                frameRight.SetActive(true);
            }
            else{
                
                paperBundle[currentPage*2+1].SetActive(true);
                pageNumRight.sprite = pageNums[currentPage*2+1];
                frameRight.SetActive(true);
            }
        }
        //leftBtn.SetActive(true);
        //rightBtn.SetActive(false);
        if(!deleteArrow){
            
            leftBtn.interactable = true;
            rightBtn.interactable = (currentPage+1)*2<activatedPaper ? true : false;
        }

        // if(currentPage<activatedPaper/2){
        //     //rightBtn.SetActive(true);
        //     rightBtn.interactable = true;
        // }
        
    }
    public void ShowLeftPage(bool deleteArrow = false){    //좌측버튼 클릭
        AudioManager.instance.Play(pageflipSound);
        currentPage -=1;
        
        if(nullRightPaper.activeSelf)
            nullRightPaper.SetActive(false);
        for(int i=0; i<activatedPaper; i++){
            paperBundle[i].SetActive(false);
        }
        paperBundle[currentPage*2].SetActive(true);
        paperBundle[currentPage*2+1].SetActive(true);
    
        // leftBtn.SetActive(false);
        // rightBtn.SetActive(true);
        if(!deleteArrow){
                
            leftBtn.interactable = currentPage!=0 ? true : false;
            rightBtn.interactable = true;
        }

        // if(currentPage!=0){
        //     //leftBtn.SetActive(true);
        //     leftBtn.interactable = true;
        // }
            pageNumLeft.sprite = pageNums[currentPage*2];
            pageNumRight.sprite = pageNums[currentPage*2+1];
                    frameLeft.SetActive(true);
                    frameRight.SetActive(true);
    }

    public void ToggleLetterBtn(){

        if(letterBtnUpdate){
            letterBtnUpdate= false;
            letterBtnFlash.SetBool("flash",false);
        }
        AudioManager.instance.Play(pageflipSound);
        leftBtn.interactable = false;
        rightBtn.interactable = false;
                pageNumLeft.sprite = nullImage;
                pageNumRight.sprite = nullImage;
        if(!letterOn){
            letterOn=!letterOn;
            nullRightPaper.SetActive(true);
            nullLeftPaper.SetActive(true);
            letter.SetActive(true);
            for(int i=0; i<activatedPaper; i++){/////////////////////////////////////////////////////////////////////0731
                paperBundle[i].SetActive(false);
            }
        }
        else{
            letterOn=!letterOn;
            nullRightPaper.SetActive(false);
            nullLeftPaper.SetActive(false);
            letter.SetActive(false);
            PageBtnUpdate();
        }
    }

    public void ClickLetter(){//0:오른쪽(나중)
        disableBtn.SetActive(true);
        AudioManager.instance.Play("pageflip");
        if(letter0.activeSelf&&letter1.activeSelf){
            
            letter0Btn.enabled = false;
            letter0.GetComponent<Animator>().SetBool("appear", true);
            letter1Btn.enabled = false;
            letter1.GetComponent<Animator>().SetBool("appear", true);
            StartCoroutine(LetterAppear(1));
            StartCoroutine(LetterAppear(2));
        }
        else if(letter1.activeSelf){
            letter1Btn.enabled = false;
            letter1.GetComponent<Animator>().SetBool("appear", true);
            StartCoroutine(LetterAppear(1));
        }
        else if(letter0.activeSelf){
            letter0Btn.enabled = false;
            letter0.GetComponent<Animator>().SetBool("appear", true);
            StartCoroutine(LetterAppear(2));
        }
    }

    IEnumerator LetterAppear(int num){
        yield return new WaitForSeconds(2.5f);
                // letter1.GetComponent<Animator>().SetBool("appear", false);
                // letter0.GetComponent<Animator>().SetBool("appear", false);

        AudioManager.instance.Play("air0");
        letterBG.SetActive(true);
        switch(num){
            case 0:

                break;
            case 1:
                ObjectManager.instance.ImageFadeIn(letter1front);
                yield return new WaitForSeconds(1f);
                letter1.GetComponent<Animator>().SetBool("appear", false);
                if(!letterExitBtn.activeSelf){
                    letterExitBtn.SetActive(true);
                }

                // ObjectManager.instance.ImageFadeOut(letter1front);
                // yield return new WaitForSeconds(1f);
                // letter1front.gameObject.SetActive(false);
                //letter1.GetComponent<Animator>().SetBool("appear", false);
                letter1Btn.enabled = true;
                break;
            case 2:
                ObjectManager.instance.ImageFadeIn(letter0front);
                yield return new WaitForSeconds(1f);
                letter0.GetComponent<Animator>().SetBool("appear", false);
                if(!letterExitBtn.activeSelf){
                    letterExitBtn.SetActive(true);
                }
                // ObjectManager.instance.ImageFadeOut(letter0front);
                // yield return new WaitForSeconds(1f);
                // letter0front.gameObject.SetActive(false);
                //letter0.GetComponent<Animator>().SetBool("appear", false);
                letter0Btn.enabled = true;
                break;
        }

    }


    public void LetterExitBtn(){
        //StartCoroutine(LetterExitCoroutine);
            AudioManager.instance.Play("button20");
        disableBtn.SetActive(false);
        letterBG.SetActive(false);
        letterExitBtn.SetActive(false);
        letter0front.gameObject.SetActive(false);
        letter1front.gameObject.SetActive(false);
    }
    // IEnumerator LetterExitCoroutine(){

    // }

    public void ResetPapers(){
        
        for(int i=0; i<6; i++){
            paperBundle[i].SetActive(false);
        }
        letter0.SetActive(false);
        letter1.SetActive(false);
    }


    // public void BlurDisappearAnim(){
    //     for(int i=0; i<theDB.progress-1 ; i++){
    //         blur[i].SetActive(false);
    //     }
    //     AudioManager.instance.Play("air0");
    //     ObjectManager.instance.ImageFadeOut(blur[theDB.progress-1].GetComponent<Image>(),0.02f);
    // }
}
