using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class MainVideo : MonoBehaviour
{
    // Start is called before the first frame up
    public GameObject menu,setting,lang;
    public VideoPlayer theVideo;
    //public Animator animator;
    public int playMusicTrack;
    public RenderTexture texture;



    BGMManager BGM;
    void Start()
    {
        menu.SetActive(false);
            setting.SetActive(false);
            lang.SetActive(false);
        ClearOutRenderTexture(texture);
        theVideo.clip = CursorManager.instance.videoClips[0];
        //theVideo.Play();
        StartCoroutine(Wait());    
        BGM = FindObjectOfType<BGMManager>();
        
    }

    IEnumerator Wait(){
        yield return new WaitForSeconds(0.01f);
        theVideo.Prepare();
        WaitForSeconds waitTime = new WaitForSeconds(0.1f);
        while(!theVideo.isPrepared){
            yield return waitTime;
        }
        //rawImage.GetComponent<RawImage>().texture = texture;
        theVideo.Play();
        BGM.Play(playMusicTrack);
        //yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(()=>theVideo.time >= 4f);
        
        //CursorManager.instance.SetCursorState(2);
        //yield return new WaitForSeconds(4.2f);
        menu.SetActive(true);
            setting.SetActive(true);
            lang.SetActive(true);
        //animator.SetBool("Appear",true);
        //yield return new WaitForSeconds(1.5f);
        
        //animator.SetBool("Swing",true);
        
        // yield return new WaitForSeconds(15f);
        // theVideo.Pause();
        
    }
    public void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;
    }

}
