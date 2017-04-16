using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageBlinker : MonoBehaviour {

	private GameObject _Start;
	public float _Step = 0.012f;
	public float timer = 2.0f;
	public float defaultA = 0;

	// Use this for initialization
	void Start () {
		defaultA = GetComponent<Image> ().color.a;
	}
	
	// Update is called once per frame
	void Update () {
		float toColor = GetComponent<Image> ().color.a;
		if (toColor < 0 || toColor > 1) {
			_Step = _Step * -1;
		}
		GetComponent<Image> ().color = new Color (255, 255, 255, toColor + _Step);
	}
}
