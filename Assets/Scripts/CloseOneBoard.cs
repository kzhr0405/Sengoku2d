using UnityEngine;
using System.Collections;

public class CloseOneBoard : MonoBehaviour {

	public GameObject deleteObj;

	public void OnClick(){

        if(Application.loadedLevelName == "pvp") {
            GameObject.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "Default";
        }

        AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [1].Play ();

		Destroy (deleteObj);
		Destroy (gameObject);
	}

}
