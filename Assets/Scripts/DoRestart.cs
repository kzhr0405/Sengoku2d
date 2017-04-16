using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.SceneManagement;

public class DoRestart : MonoBehaviour {

	public GameObject back;
	public GameObject confirm;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (name == "YesButton") {
			int busyoDama = PlayerPrefs.GetInt ("busyoDama");
			if (busyoDama >= 100) {
				audioSources [5].Play ();
				int newBusyoDama = busyoDama - 100;
				PlayerPrefs.SetInt ("busyoDama",newBusyoDama);

				PlayerPrefs.SetBool("gameOverFlg",true);
				PlayerPrefs.Flush ();
				SceneManager.LoadScene ("clearOrGameOver");

			} else {
				Message msg = new Message ();
                msg.makeMessageOnBoard(msg.getMessage(2));
            }
		} else {
			audioSources [1].Play ();

			//Back
			Destroy(back);
			Destroy(confirm);
		}
	}
}
