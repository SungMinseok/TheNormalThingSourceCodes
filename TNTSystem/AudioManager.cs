using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; //사운드의 이름

    public AudioClip clip;  //사운드 파일
    private AudioSource source;     //사운드 플레이어(볼륨조절등)

    public float Volume;
    [HideInInspector] public float volumeAfter;
    public bool loop;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.volume = Volume;
        volumeAfter = Volume;
    }

    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void SetLoop()
    {
        source.loop = true;
    }

    public void SetLoopCancel()
    {
        source.loop = false;
    }

    public void SetVolume()
    {
        source.volume = Volume;
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public float firstVolume = 0.3f;
    public bool flag;

    [SerializeField]
    public Sound[] sounds;

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
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        for(int i = 0; i<sounds.Length; i++)
        {
            GameObject soundObject = new GameObject("사운드파일:" + i + "=" + sounds[i].name);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);
        }
        FirstSetAllVolume(0.3f);
    }

    
    public void Play(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if(_name == sounds[i].name)
            {
                sounds[i].Play();
                return;
            }
        }
    }

    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public void SetLoop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoop();
                return;
            }
        }
    }

    public void SetLoopCancel(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].SetLoopCancel();
                return;
            }
        }
    }

    public void SetVolume(string _name, float _Volume)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].name)
            {
                sounds[i].Volume = _Volume;
                sounds[i].SetVolume();
                return;
            }
        }
    }
    public void SetAllVolume(float _Volume)         //세팅창에서 조절
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Volume = _Volume*sounds[i].volumeAfter;
            sounds[i].SetVolume();
            
        }
    }
    public void FirstSetAllVolume(float _Volume)         //세팅창에서 조절
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Volume *= _Volume;
            sounds[i].SetVolume();
        }
    }

    public void Button22(){
        Play("button22");
    }

    void FixedUpdate(){
        if(PlayerManager.instance!=null ){

            if(PlayerManager.instance.currentMapName == "cornerwood" && !flag){
                flag= true;
                Play("treecreak");
            }
            else if(PlayerManager.instance.currentMapName != "cornerwood" && flag){
                flag=false;
                Stop("treecreak");
            }
        }
    }

}
