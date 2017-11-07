using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class TextLocalizer : MonoBehaviour {
    [Multiline]
    public string englishText;
    public string chineseText;
    public int englishFontSize;
    public bool arialFg = false;

    void Start() {
        Text text = GetComponent<Text>();
        int langId = PlayerPrefs.GetInt("langId");
        if (langId==2) {
            text.text = englishText;
            if (englishFontSize != 0) {
                text.fontSize = englishFontSize;
            }
            if (arialFg) {
                text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            }
        }else if(langId==3) {
            text.text = chineseText;            
            if (arialFg) {
                text.font = (Font)Resources.Load("Fonts/simplifiedChinese");
            }
        }
    }
}