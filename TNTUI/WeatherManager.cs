using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

// public class WeatherEditor : Editor
// {
//     WeatherManager mom;

//     void OnEnable(){
//         mom = target as WeatherManager;
//     }
//     public override void OnInspectorGUI(){
//         base.OnInspectorGUI();
//         //EditorGUILayout.
//         mom.rainAmount = EditorGUILayout.Slider("RainAmount", mom.rainAmount, 20f, 1000f);
//     }
// }


public class WeatherManager : MonoBehaviour
{
    [Header ("천둥")]public bool onLighting;
    private bool onLightingFlag;
    // [Header ("천둥 주기 (Min~Max)초")]public float lightingCooldownMin;
    // [Header ("천둥 주기 (Min~Max)초")]public float lightingCooldownMaX;
    [Header ("천둥 주기 (초)(+-1초)(최소 3초)")]public float lightingCooldown;
    [Header ("비")]public bool onRaining;
    public ParticleSystem rain;
    //private bool onLightingFlag;
    [Header ("비 량")][Range(50f, 1000f)]public float rainAmount =100f;
    


    private PlayerManager thePlayer;
    public SpriteRenderer white;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    public Color color2;
    public Color colorDefault = new Color(1,1,1,0);
    //public bool firstEnterMap;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(onLighting&&!onLightingFlag){
            onLightingFlag = true;
            StartCoroutine(CoolDown());
        }

        
        switch(thePlayer.currentMapName){
            case "lakeout" :
                rainAmount = 40f;
                onRaining = true;
                break;
            case "rainingforest" :
                rainAmount = 100f;
                onRaining = true;
                onLighting = false;
                break;
            case "parrothidden" :
                rainAmount = 100f;
                onRaining = true;
                break;
            case "thunderingforest" :
                rainAmount = 100f;
                onRaining = true;
                onLighting = true;
                break;
            case "mazein" :
                rainAmount = 80f;
                onRaining = true;
                onLighting = true;
                break;
            case "maze" :
                rainAmount = 60f;
                onRaining = true;
                onLighting = true;
                break;
            default :
                onRaining = false;
                onLighting = false;
                break;
        }
        
        
        if(onRaining/* && !rain.main.loop*/){
            rain.Stop();
            var main = rain.main;
            main.loop = true;
            var em = rain.emission;
            em.rateOverTime = rainAmount;
            rain.Play();
//            Debug.Log("비 on");
        }
        else if(!onRaining && rain.main.loop){
            
            //myParticle.Stop();
            var main = rain.main;
            main.loop = false;
            //Debug.Log("비 off");
            //myParticle.Play();
        } 
    }

    IEnumerator CoolDown(){
        yield return new WaitForSeconds(3f);
        //FadeManager.instance.Flash(0.3f);
        Flash(0.3f);
        yield return new WaitForSeconds(0.3f);
        Flash(0.5f);
        //FadeManager.instance.Flash(0.5f);
        yield return new WaitForSeconds(0.1f);
        Flash(0.5f);
        //FadeManager.instance.Flash(0.5f);
        yield return new WaitForSeconds(0.1f);
        //white.color = new Color(0,0,0,0);
        color2 = colorDefault;
        white.color = color2;
        //yield return new WaitForSeconds(Random.Range(lightingCooldownMin,lightingCooldownMaX));
        yield return new WaitForSeconds(Random.Range(lightingCooldown-4f,lightingCooldown-2f));
        onLightingFlag = false;
    }

    // IEnumerator TransferDelay(){
    //     yeildd return new WaitForSeconds(2f);
    // }

    public void Flash(float _speed = 0.01f){
        
        //StopAllCoroutines();
        StopCoroutine("FlashCoroutine");
        StartCoroutine(FlashCoroutine(_speed));
    }

    IEnumerator FlashCoroutine(float _speed){
        
        color2 = white.color;

        while(color2.a <0.5f){
            color2.a += _speed;
            white.color = color2;
            yield return waitTime;
        }
        
        while(color2.a >0f){
            color2.a -= _speed;
            white.color = color2;
            yield return waitTime;
        }

    }


    
}
