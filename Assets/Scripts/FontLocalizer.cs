using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class FontLocalizer : MonoBehaviour {

    void Start() {
        int langId = PlayerPrefs.GetInt("langId");
        Text text = GetComponent<Text>();
        if (langId == 3) {
            text.font = (Font)Resources.Load("Fonts/simplifiedChinese");
        }
    }
}