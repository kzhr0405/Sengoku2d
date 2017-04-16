using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class HyourouTimer : MonoBehaviour {

	public float startTime; // seconds
	public float timer;

	private void Start()
	{
		reset();
	}
	
	private void reset()
	{
		timer = startTime;
	}
	
	// Update is called once per frame
	private void Update () {
		timer -= Time.deltaTime;

		if (timer > 0.0f) {
			//On Play
			GetComponent<Text> ().text = ((int)timer).ToString ();
			
		} else {

			//Add Hyourou
			int hyourou = int.Parse(GameObject.Find ("HyourouCurrentValue").GetComponent<Text>().text);
			hyourou = hyourou + 1;
			GameObject.Find ("HyourouCurrentValue").GetComponent<Text>().text = hyourou.ToString();
			PlayerPrefs.SetInt ("hyourou",hyourou);
			System.DateTime now = System.DateTime.Now;
			PlayerPrefs.SetString("lasttime", now.ToString());
			PlayerPrefs.Flush();

			//Reset
			GetComponent<Text> ().text = "300";
			timer = 300;
			startTime = 300;
		}

	}
}
