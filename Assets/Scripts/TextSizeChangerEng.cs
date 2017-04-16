using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSizeChangerEng : MonoBehaviour {

    public int englishFontSize;
    public bool arialFg;

    // Use this for initialization
    void Start () {
        Text text = GetComponent<Text>();
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            if (englishFontSize != 0) {
                text.fontSize = englishFontSize;
            }
            if (arialFg) {
                text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            }
        }
    }
}
