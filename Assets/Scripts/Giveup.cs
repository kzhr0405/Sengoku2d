using UnityEngine;
using System.Collections;

public class Giveup : MonoBehaviour {

	public void OnClick(){

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		GameObject canvas = GameObject.Find ("Canvas").gameObject;

		string pathOfBack = "Prefabs/Common/TouchBackForOne";
		GameObject back = Instantiate (Resources.Load (pathOfBack)) as GameObject;
		back.transform.SetParent (canvas.transform);
		back.transform.localScale = new Vector2 (1, 1);
		back.transform.localPosition = new Vector2 (0, 0);

		string pathOfGiveup = "Prefabs/PreKassen/GiveupConfirm";
		GameObject giveup = Instantiate (Resources.Load (pathOfGiveup)) as GameObject;
		giveup.transform.SetParent (back.transform);
		giveup.transform.localScale = new Vector2 (1, 1);
		giveup.transform.localPosition = new Vector2 (0, 0);

		back.GetComponent<CloseOneBoard> ().deleteObj = giveup;
		giveup.transform.FindChild ("YesButton").GetComponent<DoGiveup> ().backBoard = back;
		giveup.transform.FindChild ("NoButton").GetComponent<DoGiveup> ().backBoard = back;

	}
}
