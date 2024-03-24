using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingTest : MonoBehaviour {

    //[SerializeField]
    //Image loadingBar;
    public SpriteRenderer loadAnim;
    public GameObject skipBtn_DEV;
    public Image loadBG;
    public float loadTime;
    public float loadingBar;
    IEnumerator animOff;
    IEnumerator animOn;
    IEnumerator bgOff;
    IEnumerator bgOn;
    Color color0;
    Color color1;
    bool fadeCheck;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    public Sprite help_mobile;
    public GameObject text_mobile;
    public GameObject text_normal;
    // public Text loadText;
    // public string loadTextSample;
    private void Start()
    {
        animOff = LoadAnimOff();
        animOn = LoadAnimOn();
        bgOff = LoadBGOff();
        bgOn = LoadBGOn();

#if UNITY_ANDROID || UNITY_IOS
        loadBG.sprite = help_mobile;
        text_mobile.SetActive(true);
        text_normal.SetActive(false);
#endif
        //loadTextSample = "가나다라마바사아자차카타파하";
        //loadingBar.fillAmount = 0;
        StartCoroutine(LoadAsyncScene());
        //StartCoroutine(LoadTexts());
        
    }
    public static void LoadScene(string sceneName)
    {  
        SceneManager.LoadScene("Loading");
    }
    public void SKIP(){

        SceneManager.LoadScene("start");
    }
    // IEnumerator LoadTexts(){
    //     for(int i=0;i<14;i++){
    //         loadText.text += loadTextSample[i];
    //         yield return waitTime;
    //     }
    // }
    IEnumerator LoadAsyncScene()
    {
        yield return null;
        //BGMManager.instance.Play(1);
        StartCoroutine(animOn);
        StartCoroutine(bgOn);
        AsyncOperation asyncScene = SceneManager.LoadSceneAsync("start");
        asyncScene.allowSceneActivation = false;
        fadeCheck = true;
        float timeC = 0;
        //Debug.Log(asyncScene.progress);


        yield return new WaitForSeconds(loadTime);
        
        //Debug.Log(asyncScene.progress);
        while (!asyncScene.isDone)
        {
            yield return null;
            //Debug.Log(asyncScene.progress);
            timeC += Time.deltaTime;
            if (asyncScene.progress >=0.9f && fadeCheck)
            {
                // loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1, timeC);
                loadingBar = Mathf.Lerp(loadingBar, 1, timeC);
                if (loadingBar == 1.0f)
                {
                    fadeCheck = false;
                    StopCoroutine(animOn);
                    StartCoroutine(animOff);
                    StopCoroutine(bgOn);
                    StartCoroutine(bgOff);
                    yield return new WaitUntil(()=> color0.a<=0f);
       // yield return new WaitForSeconds(1.5f);
                    asyncScene.allowSceneActivation = true;
                }         
            }
            else
            {
                loadingBar= Mathf.Lerp(loadingBar, asyncScene.progress, timeC);
                if (loadingBar >= asyncScene.progress){
                    timeC = 0f;
                }
            }
        }
    }
    IEnumerator LoadBGOff(){
        
        color1 = loadBG.color;
        while(color1.a > 0f){
            color1.a -= 0.02f;
            loadBG.color = color1;
            yield return waitTime;
        }

    }
    IEnumerator LoadAnimOff(){
        
        color0 = loadAnim.color;
        while(color0.a > 0f){
            color0.a -= 0.02f;
            loadAnim.color = color0;
            yield return waitTime;
        }

    }
    IEnumerator LoadBGOn(){

        color1 = loadBG.color;
        while(color1.a < 1f){
            color1.a += 0.02f;
            loadBG.color = color1;
            yield return waitTime;
        }
        
    }
    IEnumerator LoadAnimOn(){

        color0 = loadAnim.color;
        while(color0.a < 1f){
            color0.a += 0.02f;
            loadAnim.color = color0;
            yield return waitTime;
        }
        
    }
}