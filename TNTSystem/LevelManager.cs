using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public static void Load(string name) {
        GameObject go = new GameObject("LevelManager");
        LevelManager instance = go.AddComponent<LevelManager>();
        instance.StartCoroutine(instance.InnerLoad(name));
    }
 
    IEnumerator InnerLoad(string name) {
        //load transition scene
        Object.DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("2");
 
        //wait one frame (for rendering, etc.)
        yield return null;
 
        //load the target scene
        SceneManager.LoadScene(name);
        Destroy(this.gameObject);
    }

    public void Btn(){
        Invoke("TestLoad",0f);
    }
    void TestLoad(){
        Load("start");
    }
}

