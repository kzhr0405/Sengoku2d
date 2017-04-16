using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KousakuBusyoSelect : MonoBehaviour {

	public GameObject doBtnObj;
	public GameObject content;
	public int busyoId = 0;
	public float dfc = 0;

	public void OnClick(){

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [2].Play ();

		//Change Color
		Color Select = new Color (76f / 255f, 50f / 255f, 18f / 255f, 80f / 255f);
		Color unSelect = new Color (255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
		foreach (Transform obj in content.transform) {
			obj.GetComponent<Image>().color = unSelect;
		}
		GetComponent<Image> ().color = Select;

		doBtnObj.GetComponent<DoKousaku> ().busyoId = busyoId;
		doBtnObj.GetComponent<DoKousaku> ().dfc = dfc;

	}
}
