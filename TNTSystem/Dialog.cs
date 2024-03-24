using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Dialogue
{
    public string[] names;

    
[Header("------------------------------------------------------------------|<color=red></color>")]
    [TextArea(1,2)]
    public string[] sentences ;
[Header("------------------------------------------------------------------|<color=red></color>")]
    [TextArea(1,2)]
    public string[] sentences_en = {""};
    [HideInInspector]
    public string[] sentences_set ;

    public Sprite[] images;

    public bool isSign = false;
    public bool isSignWithName = false;
    //public Sprite null_image;
    //public Image[] images;      //그림 필요 없는 곳에 그림 안넣어도되고 대신 투명한 널이미지 하나 추가해줘서 그때는 그림효과 작용안하도록      //위의 널 체크, 널이면 true
    //public Sprite[] dialogueWindows;
}
