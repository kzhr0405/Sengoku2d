using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextLocalizer : MonoBehaviour {
    [Multiline]
    public string englishText;
    public int englishFontSize;
    public bool arialFg = false;

    void Start() {
        Text text = GetComponent<Text>();
        
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            text.text = englishText;
            if (englishFontSize != 0) {
                text.fontSize = englishFontSize;
            }
            if (arialFg) {
                text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            }
        }
    }
}