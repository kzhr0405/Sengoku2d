using UnityEngine;
using System.Collections;

public class Soudaisyo : MonoBehaviour {

	public void OnClick(){

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		//Delete Previous Soudaisyo
		Destroy (GameObject.Find ("soudaisyo"));

		string path = "Prefabs/Jinkei/soudaisyo";
		GameObject soudaisyo = Instantiate (Resources.Load (path)) as GameObject;
		soudaisyo.transform.SetParent (gameObject.transform);
		soudaisyo.transform.localScale = new Vector2 (27, 12);
		soudaisyo.name = "soudaisyo";
		soudaisyo.transform.localPosition = new Vector3 (0, 11, 0);

		if (Application.loadedLevelName == "hyojyo") {
			JinkeiConfirmButton KakuteiScript = GameObject.Find ("KakuteiButton").GetComponent<JinkeiConfirmButton> ();
			KakuteiScript.soudaisyo = int.Parse (name);
		} else if (Application.loadedLevelName == "preKassen" || Application.loadedLevelName == "preKaisen") {
			startKassen2 startScript = GameObject.Find ("StartBtn").GetComponent<startKassen2> ();
			startScript.soudaisyo = int.Parse (name);
		}

	}
}
