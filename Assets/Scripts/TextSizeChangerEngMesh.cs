﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class TextSizeChangerEngMesh : MonoBehaviour {

    public int englishFontSize;
    public bool arialFg;

    // Use this for initialization
    void Start() {
        TextMesh text = GetComponent<TextMesh>();
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            if (englishFontSize != 0) {
                text.fontSize = englishFontSize;
            }
            if (arialFg) {
                text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            }
        }
    }
}
