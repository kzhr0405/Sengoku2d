using UnityEngine;
using System.Collections;

public class Fadein : MonoBehaviour {

    public float fadeTime = 1f;
    private float timer;
    private TextMesh textScript;
    public GameObject destroyBoard;

    // Use this for initialization
    void Start () {
        timer = 0;
        textScript = GetComponent<TextMesh>();
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer >= fadeTime) {
            GameObject.Destroy(destroyBoard);
            return;
        }
        
        float alpha = timer / fadeTime;
        var color = textScript.color;
        color.a = alpha;
        textScript.color = color;
    }
}
