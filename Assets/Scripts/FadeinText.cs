using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeinText : MonoBehaviour {

    public float fadeTime = 2f;
    private float timer;
    Text textScript;
    public bool doneFlg = false;

    // Use this for initialization
    void Start() {
        timer = 0;
        textScript = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
        if(!doneFlg) {
            timer += Time.deltaTime;

            float alpha = timer / fadeTime;
            var color = textScript.color;
            color.a = alpha;
            textScript.color = color;

            if (alpha > 0.7) {
                doneFlg = true;
            }

        }
    }
}
