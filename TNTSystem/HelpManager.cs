#if UNITY_ANDROID || UNITY_IOS
#define DISABLEKEYBOARD
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpManager : MonoBehaviour
{
    public static HelpManager instance;
    public GameObject bundle;
    public GameObject[] helper;
    public bool onClick;
#if DISABLEKEYBOARD
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }

        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    void Start()
    {
        for(int i=0;i<helper.Length;i++){
            helper[i].SetActive(false);
        }
    }
    public void ClickHelper(){
        onClick = true;
    }
    public void PopUpHelper(int num){
        PlayerManager.instance.notMove = true;
        PlayerManager.instance.animator.SetFloat("Speed",0f);
        bundle.SetActive(true);
        helper[num].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(onClick){
            onClick = false;
            PlayerManager.instance.notMove = false;
            PlayerManager.instance.getSpace = true;
            for(int i=0;i<helper.Length;i++){
                helper[i].SetActive(false);
            }
            bundle.SetActive(false);
        }
    }
#endif
}
