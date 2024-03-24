using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float successWaitTime = 3f;
    public GameObject game0;
    public GameObject game2;
    public GameObject game5;
    public GameObject game6;
    public GameObject game17;
    public GameObject game18;
    public GameObject game19;
    public GameObject game24;
    public GameObject game25;
    public Image disableBtns;

    public List<GameObject> GameList = new List<GameObject>();
    public GameObject[] testBtns;
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

        GameList.Add(game0);
        GameList.Add(game2);
        GameList.Add(game5);
        GameList.Add(game6);
        GameList.Add(game17);
        GameList.Add(game18);
        GameList.Add(game19);
        GameList.Add(game24);
        GameList.Add(game25);

        //#if UNITY_EDITOR
        //if(DebugManager.instance.devMode)
        
        //#endif
    }

    
    //public bool playing;    //true : 게임 창 on
    //public bool[] pass;   //true : 게임 통과



    void Start(){
        //pass = new bool[gameNum.Length];
    }
    //게임성공후 버튼 클릭방지
    public void GameSuccessTrig(){
        StartCoroutine(GameSuccess());
    }
    public IEnumerator GameSuccess(){//총 3초 : 1초후 페이드 아웃 2초후 페이드 인. 
        //AudioManager.instance.Play("success0");
        disableBtns.gameObject.SetActive(true);
        yield return new WaitForSeconds(successWaitTime-1f);
        Fade2Manager.instance.FadeOut();
        yield return new WaitForSeconds(0.95f);
        //yield return new WaitForSeconds(0.55f);
        Fade2Manager.instance.FadeIn();
        disableBtns.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.05f);
    }
}
