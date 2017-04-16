using UnityEngine;
using System.Collections;

public class TextLocalizerMesh : MonoBehaviour {
    [Multiline]
    public string englishText;
    public int englishFontSize;
    public bool arialFg = false;

    void Start() {
        TextMesh text = GetComponent<TextMesh>();
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