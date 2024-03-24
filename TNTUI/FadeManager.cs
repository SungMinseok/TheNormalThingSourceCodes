using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    //public SpriteRenderer white; 
    public SpriteRenderer black; 

    public SpriteRenderer white;
    //public Image black1;
    //public GameObject white;
    //public GameObject black;
    public Color color;
    public Color color2;
    public GameObject fog0;
    public GameObject red;
    public GameObject unknownFog;
    public AnimationClip ufIng;
    public AnimationClip ufOff;

    

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    private bool fadeFlag;  //빠르게 다시 나가면 다시 Fadeout.
    private PlayerManager thePlayer;
    public static FadeManager instance;
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
    void Start(){
        thePlayer = FindObjectOfType<PlayerManager>();
    }
    // void FixedUpdate(){
    //     if(thePlayer.isChased){
    //         unknownFog.SetActive(true);
    //     }
    //     else{
    //         unknownFog.SetActive(false);
    //     }
    // }
    
    public void FadeOut(float _speed = 0.03f, float _max = 1f){
        StopAllCoroutines();
        //StopCoroutine("FadeOutCoroutine");
        //StopCoroutine("FadeInCoroutine");
        StartCoroutine(FadeOutCoroutine(_speed, _max));
    }

    IEnumerator FadeOutCoroutine(float _speed, float _max){
        
        color = black.color;
        while(color.a <_max){
            color.a += _speed;
            black.color = color;
            yield return waitTime;
        }
    }
    public void FadeIn(float _speed = 0.03f){
        
        StopAllCoroutines();
        //StopCoroutine("FadeOutCoroutine");
        //StopCoroutine("FadeInCoroutine");
        StartCoroutine(FadeInCoroutine(_speed));
    }

    IEnumerator FadeInCoroutine(float _speed){
        
        color = black.color;

        while(color.a >0f){
            color.a -= _speed;
            black.color = color;
            yield return waitTime;
        }
    }
    public void FlashOut(float _speed = 0.03f){
        
        StopAllCoroutines();
        StartCoroutine(FlashOutCoroutine(_speed));
    }

    IEnumerator FlashOutCoroutine(float _speed){
        
        color = white.color;
        fadeFlag = true;
        while(color.a <1f){
            color.a += _speed;
            white.color = color;
            yield return waitTime;
        }
        
        fadeFlag = false;
    }
    /*
    public void FlashIn(float _speed = 0.03f){
        
        StopAllCoroutines();
        StartCoroutine(FlashInCoroutine(_speed));
    }

    IEnumerator FlashInCoroutine(float _speed){
        
        color = white.color;

        while(color.a >0f&&!fadeFlag){
            color.a -= _speed;
            white.color = color;
            yield return waitTime;
        }
    }
// */
//     public void Flash(float _speed = 0.01f){
        
//         StopAllCoroutines();
//         StartCoroutine(FlashCoroutine(_speed));
//     }

//     IEnumerator FlashCoroutine(float _speed){
        
//         color2 = white.color;

//         while(color2.a <1f){
//             color2.a += _speed;
//             white.color = color2;
//             yield return waitTime;
//         }
        
//         while(color2.a >0f){
//             color2.a -= _speed;
//             white.color = color2;
//             yield return waitTime;
//         }
//     }

    // void FixedUpdate(){
    //     if(fog0.activeSelf){
    //         fog0.transform.position = thePlayer.transform.position;
    //     }
    // }
    public void UnknownFogOff(){
        //StartCoroutine(UnknownFogOffCR());
        unknownFog.GetComponent<Animator>().SetTrigger("off");
        Invoke("UFSetOff",ufIng.length+ufOff.length);
    }
    // public IEnumerator UnknownFogOffCR(){
    //     //Debug.Log(ufIng.length+ufOff.length);
    //     unknownFog.GetComponent<Animator>().SetTrigger("off");
    //     yield return new WaitForSeconds(ufIng.length+ufOff.length);
    //     //Debug.Log("여기 왜안돼");
    //     unknownFog.SetActive(false);
    // }
    public void UFSetOff() => unknownFog.SetActive(false);

    
}
