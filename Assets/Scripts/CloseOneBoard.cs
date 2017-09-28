using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CloseOneBoard : MonoBehaviour {

	public GameObject deleteObj;

	public void OnClick(){

        if(GameObject.Find("Canvas")) {
            if (SceneManager.GetActiveScene().name != "kassen" && SceneManager.GetActiveScene().name != "kaisen") {
                GameObject.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "Default";
            }
        }else if(GameObject.Find("Map")) {
            GameObject.Find("Map").GetComponent<Canvas>().sortingLayerName = "Default";
        }else if (GameObject.Find("Jinkei")) {
            GameObject.Find("Jinkei").GetComponent<Canvas>().sortingLayerName = "Default";
        }


        AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [1].Play ();


		Destroy (deleteObj);
		Destroy (gameObject);
	}

}
