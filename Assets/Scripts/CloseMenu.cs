using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CloseMenu : MonoBehaviour {

	public GameObject obj;
	public bool cyouteiFlg;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [1].Play ();
		Destroy (obj);
        Message msg = new Message();

		if (cyouteiFlg) {
			GameObject.Find ("Serihu").transform.FindChild ("Text").GetComponent<Text> ().text = msg.getMessage(30);
		} else {
			GameObject.Find ("Serihu").transform.FindChild ("Text").GetComponent<Text> ().text = msg.getMessage(31);
        }
	}
}
