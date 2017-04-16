using UnityEngine;
using System.Collections;

public class Blinker : MonoBehaviour {

	private GameObject _Start;
	private float _Step = 0.025f;
	public float timer = 2.0f;
	public float defaultA = 0;

	void Start(){
		defaultA = GetComponent<SpriteRenderer> ().color.a;
	}

	void Update(){
		
		float toColor = GetComponent<SpriteRenderer> ().color.a;
		if (toColor < 0 || toColor > 1) {
			_Step = _Step * -1;
		}
		GetComponent<SpriteRenderer> ().color = new Color (255, 255, 255, toColor + _Step);

		timer -= Time.deltaTime;
		if (timer <= 0.0f) {
			GetComponent<SpriteRenderer> ().color = new Color (255, 255, 255, defaultA);
			Destroy (this);

		}
	}
}