using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		//Confirm Button
		string backPath = "Prefabs/Busyo/back";
		GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
		back.transform.SetParent(GameObject.Find ("Panel").transform);
		back.transform.localScale = new Vector2 (1, 1);
		RectTransform backTransform = back.GetComponent<RectTransform> ();
		backTransform.anchoredPosition = new Vector3 (0, 0, 0);

		//Message Box
		string msgPath = "Prefabs/Common/RestartConfirm";
		GameObject msg = Instantiate (Resources.Load (msgPath)) as GameObject;
		msg.transform.SetParent(GameObject.Find ("Panel").transform);
		msg.transform.localScale = new Vector2 (0.8f, 1.0f);
		RectTransform msgTransform = msg.GetComponent<RectTransform> ();
		msgTransform.anchoredPosition = new Vector3 (0, 0, 0);
		msgTransform.name = "RestartConfirm";

		msg.transform.FindChild ("YesButton").GetComponent<DoRestart> ().back = back;
		msg.transform.FindChild ("YesButton").GetComponent<DoRestart> ().confirm = msg;
		msg.transform.FindChild ("NoButton").GetComponent<DoRestart> ().back = back;
		msg.transform.FindChild ("NoButton").GetComponent<DoRestart> ().confirm = msg;

	}
}
