using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade2Manager : MonoBehaviour
{
    //public SpriteRenderer white; 
    public Image black; 
    public Image white;
    //public Image white1;
    //public Image black1;
    //public GameObject white;
    //public GameObject black;
    public Color color;
    public Color color1;
    public Color color2;
    
    public GameObject go;
    public Image birdImage;
    public GameObject red;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    private bool fadeFlag;  //빠르게 다시 나가면 다시 Fadeout.

    public static Fade2Manager instance;
    public bool animFlag;
    public Image image0, image1, image2, frame, blackBG;
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


    void Start(){
        color = new Color(1,1,1,0);
        color1 = new Color(0,0,0,0);

    }
    
    public void FadeOut(float _speed = 0.03f, float _max = 1f){
        go.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(_speed, _max));
    }

    IEnumerator FadeOutCoroutine(float _speed, float _max){
        color = black.color;
        //fadeFlag = true;
        while(color.a <_max){
            color.a += _speed;
            black.color = color;
            yield return waitTime;
        }

        //go.SetActive(false);
    }
    public void FadeIn(float _speed = 0.03f){
        
        //go.SetActive(true);
        //black.color = color2;
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(_speed));
    }

    IEnumerator FadeInCoroutine(float _speed){
        if(!go.activeSelf)
            go.SetActive(true);
        color = black.color;

        while(color.a >0f){
            color.a -= _speed;
            black.color = color;
            yield return waitTime;
        }
        go.SetActive(false);
    }
    public void FadeKeep(){                     //꺼멓게 유지, 이후에 꼭 FadeIn()넣을 것
        
        //go.SetActive(true);
        //black.color = color2;
        StopAllCoroutines();
        StartCoroutine(FadeKeepCoroutine());
    }

    IEnumerator FadeKeepCoroutine(){
        if(!go.activeSelf)
            go.SetActive(true);
        color = black.color;
        yield return null;
/*
        while(color.a >0f){
            color.a -= _speed;
            black.color = color;
            yield return waitTime;
        }
        go.SetActive(false);*/
    }
    /*public void FlashOut(float _speed = 0.03f){
        
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
    }*/
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

    public void Flash(float _speed = 0.01f){
        
        StopAllCoroutines();
        StartCoroutine(FlashCoroutine(_speed));
    }

    IEnumerator FlashCoroutine(float _speed){
        
        color = white.color;

        while(color.a <1f){
            color.a += _speed;
            white.color = color;
            yield return waitTime;
        }
        
        while(color.a >0f){
            color.a -= _speed;
            white.color = color;
            yield return waitTime;
        }
    }*/

    public void FadeOutOI(){                    //인트로용 (인자없이)    
        
        go.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeOutOICoroutine());
    }

    IEnumerator FadeOutOICoroutine(){
        
        
        color = black.color;
        while(color.a <1f){
            color.a += 0.01f;
            black.color = color;
            yield return waitTime;
        }

        //go.SetActive(false);
    }
    // public void FadeOutImageCoroutine(Image _image){
    //     StartCoroutine((FadeOutImageCoroutine(_image));
    // }

    public IEnumerator FadeOutImageCoroutine(Image _image, float speed = 0.1f, bool activation = true){
        color = _image.color;
        //fadeFlag = true;
        while(color.a >0){
            color.a -= speed;
            _image.color = color;
            yield return waitTime;
        }
        if(activation)
            _image.gameObject.SetActive(false);
    }
    IEnumerator FadeInImageCoroutine(Image[] _image, float speed = 0.1f, bool activation = true){
        
        if(activation){
            for(int i=0; i<_image.Length; i++){

                _image[i].gameObject.SetActive(true);
            }
        }
        // for(int i=0; i<_image.Length; i++){
        //     color1 = _image[i].color;
        // }
        color1 = new Color(1,1,1,0);
        //fadeFlag = true;
        while(color1.a <1f){
            color1.a += speed;
            for(int i=0; i<_image.Length; i++){
                _image[i].color = color1;
            }
            yield return waitTime;
        }
    }

    public IEnumerator ImageAnim(){
        animFlag = true;
        blackBG.gameObject.SetActive(true);
        Image[] temp = new Image[2]{image0, frame};
        StartCoroutine(FadeInImageCoroutine(temp,0.06f));

        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeOutImageCoroutine(image0,0.02f));
        image1.gameObject.SetActive(true);
        // temp = new Image[1]{image1};
        // StartCoroutine(FadeInImageCoroutine(temp,0.02f));

        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeOutImageCoroutine(image1,0.02f));
        image2.gameObject.SetActive(true);
        // temp = new Image[1]{image2};
        // StartCoroutine(FadeInImageCoroutine(temp,0.02f));
        
        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeOutImageCoroutine(image2,0.06f));
        StartCoroutine(FadeOutImageCoroutine(frame,0.06f));


        yield return new WaitForSeconds(0.6f);
        
        blackBG.gameObject.SetActive(false);
        animFlag= false;
        
        image0.color = new Color (1,1,1,1);
        image1.color = new Color (1,1,1,1);
        image2.color = new Color (1,1,1,1);
    }


    public void FlashIn(float _speed = 0.03f){
        
        StopAllCoroutines();
        StartCoroutine(FlashInCoroutine(_speed));
    }

    IEnumerator FlashInCoroutine(float _speed){
        //color2 = new Color(1,1,1,0);
        color2 = white.color;

        while(color2.a <1f){
            color2.a += _speed;
            white.color = color2;
            yield return waitTime;
        }
    }










}
