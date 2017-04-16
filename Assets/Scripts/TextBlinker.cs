using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextBlinker : MonoBehaviour {

	private GameObject _Start;
	public float _Step = 0.012f;
	public float timer = 2.0f;
	public float defaultA = 0;
    public bool blinkFlg = false;

	void Awake(){
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start () {
		defaultA = GetComponent<Text> ().color.a;
	}
	
	// Update is called once per frame
	void Update () {
        if(!blinkFlg) {
		    float toColor = GetComponent<Text> ().color.a;
		    if (toColor < 0 || toColor > 1) {
			    _Step = _Step * -1;
		    }
		    GetComponent<Text> ().color = new Color (255, 255, 255, toColor + _Step);
        }
    }
}
