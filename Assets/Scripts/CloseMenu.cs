using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class CloseMenu : MonoBehaviour {

	public GameObject obj;
	public bool cyouteiFlg;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [1].Play ();
		Destroy (obj);
        Message msg = new Message();
        int langId = PlayerPrefs.GetInt("langId");

        if (cyouteiFlg) {
			GameObject.Find ("Serihu").transform.Find ("Text").GetComponent<Text> ().text = msg.getMessage(30,langId);
		} else {
			GameObject.Find ("Serihu").transform.Find ("Text").GetComponent<Text> ().text = msg.getMessage(31,langId);
        }
	}
}
