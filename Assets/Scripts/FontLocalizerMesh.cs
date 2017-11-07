using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class FontLocalizerMesh : MonoBehaviour {

    void Start() {
        int langId = PlayerPrefs.GetInt("langId");
        TextMesh text = GetComponent<TextMesh>();
        if (langId == 3) {
            text.font = (Font)Resources.Load("Fonts/simplifiedChinese");
           
        }
    }
}