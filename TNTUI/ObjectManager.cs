using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ObjectManager : MonoBehaviour
{
    
    public static ObjectManager instance;
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
    
    
    //public SpriteRenderer sprite;
    public Color color;
    public Color color2;
    public Color color3;
    public Color color4;
    //TEXT
    public Color color5;
    public Color color6;
    private PlayerManager thePlayer;
    WaitForSeconds wait01 = new WaitForSeconds(0.01f);
    // Start is called before the first frame update

    void Start()
    {
        color = new Color (1,1,1,1);
        color2 = new Color (1,1,1,1);
        color3 = new Color (1,1,1,1);
        color4 = new Color (1,1,1,1);
        color5 = new Color (1,1,1,1);
        color6 = new Color (1,1,1,1);
        thePlayer = PlayerManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SAC(){
        StopAllCoroutines();
    }
    public void FadeOut(SpriteRenderer sprite, float speed=0.03f, bool activation = true, bool isUnknown = false){

        //StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(sprite, speed, activation, isUnknown));
    }
    public IEnumerator FadeOutCoroutine(SpriteRenderer sprite, float speed, bool activation, bool isUnknown){
        
        color3 = sprite.color;
        //fadeFlag = true;
        while(color3.a >0f){
            if(sprite==null) break;
            else{
                
            color3.a -= speed;
            sprite.color = color3;
            //yield return new WaitForSeconds(0.01f);
            yield return wait01;
            //Debug.Log(sprite.color.a);
            }
        }
        if(activation)
            sprite.gameObject.SetActive(false);
        if(isUnknown){
            sprite.gameObject.transform.position = new Vector3 (0f,20f,0f);
        }
        if(color3.a <=0f)
            color3 = new Color(1,1,1,1);
        //color = new Color(1,1,1,1);
        //ResetColor();
        //fadeFlag = false;
    }
    public void FadeIn(SpriteRenderer sprite, float speed = 0.03f){

        StartCoroutine(FadeInCoroutine(sprite, speed));
        
    }
    public IEnumerator FadeInCoroutine(SpriteRenderer sprite, float speed){
        
        color4.a = 0f;
        sprite.color = color4; 

        
        sprite.gameObject.SetActive(true);
        color4 = sprite.color;
        //fadeFlag = true;
        while(color4.a <1f ){
            if(sprite==null) break;
            
            else {
                
                color4.a += speed;
                sprite.color = color4;
                yield return wait01;
            }
        }
        
        //fadeFlag = false;
    } 
    public void ImageFadeIn(Image _image, float speed=0.03f){

        StartCoroutine(ImageFadeInCoroutine(_image, speed));
        
    }
    IEnumerator ImageFadeInCoroutine(Image _image, float speed){
        
        color2.a = 0f;
        _image.color = color2;

        _image.gameObject.SetActive(true);
        //_image.color = new Color(1,1,1,0);
        //color.a = 0f;
        color2 = _image.color;
        //fadeFlag = true;
        while(color2.a <1f){
            //Debug.Log("color2.a : "+color2.a);
            //Debug.Log("_image.color.a : "+_image.color.a);
            color2.a += speed;
            _image.color = color2;
            yield return wait01;
        }
        
        //fadeFlag = false;
    }
    public void ImageFadeOut(Image sprite, float speed=0.03f, bool activation = true){ //false면 페이드아웃 후 비활성화 안함

        //StopAllCoroutines();
        StartCoroutine(ImageFadeOutCoroutine(sprite, speed, activation));
    }
    IEnumerator ImageFadeOutCoroutine(Image sprite, float speed, bool activation){
        
        color = sprite.color;
        //fadeFlag = true;
        while(color.a >0f){
            if(sprite==null) break;
            else{
                
            color.a -= speed;
            sprite.color = color;
            //yield return new WaitForSeconds(0.01f);
            yield return wait01;

            }
        }
        if(activation)
            sprite.gameObject.SetActive(false);
        // if(color.a <=0f)
        //     color = new Color(1,1,1,1);
        //color = new Color(1,1,1,1);
        //ResetColor();
        //fadeFlag = false;
    }
    public void ResetColor(){
        Debug.Log("RESET");
        color = new Color(1,1,1,1);
    }

    
    public void TextFadeIn(Text text, float speed=0.03f){

        StartCoroutine(TextFadeInCoroutine(text, speed));
        
    }
    IEnumerator TextFadeInCoroutine(Text text, float speed){
        color5 = text.color;
        color5.a = 0f;
        text.color = color5;

        text.gameObject.SetActive(true);
        //_image.color = new Color(1,1,1,0);
        //color.a = 0f;
        color5 = text.color;
        //fadeFlag = true;
        while(color5.a <1f){
            //Debug.Log("color2.a : "+color2.a);
            //Debug.Log("_image.color.a : "+_image.color.a);
            color5.a += speed;
            text.color = color5;
            yield return wait01;
        }
        
        //fadeFlag = false;
    }
}
