using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Select
{
    public string nameText;
    
    [TextArea(1,2)]
    public string question;
    
    [TextArea(1,2)]
    public string question_en;
    [HideInInspector]
    public string question_set ;
    
    [TextArea(1,2)]
    public string[] answers;
    
    [TextArea(1,2)]
    public string[] answers_en = {""};
    [HideInInspector]
    public string[] answers_set ;
}
