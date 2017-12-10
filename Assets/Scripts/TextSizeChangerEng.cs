using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class TextSizeChangerEng : MonoBehaviour {

    public int englishFontSize;
    public int chineseFontSize;
    public bool arialFg;

    // Use this for initialization
    void Start () {
        Text text = GetComponent<Text>();
        int langId = PlayerPrefs.GetInt("langId");        
        if (langId == 2) {
            if (englishFontSize != 0) {
                text.fontSize = englishFontSize;
            }
            if (arialFg) {
                text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            }
        }else if(langId==3) {
            if(chineseFontSize!=0) text.fontSize = chineseFontSize;
            if (arialFg) {
               text.font = (Font)Resources.Load("Fonts/simplifiedChinese");
            }
        }
    }
}
