using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GaikouShinobiSelect : MonoBehaviour {

	public GameObject DoBtn;
	public GameObject Content;
	public GameObject Gunzei;
	public int qty = 0;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [2].Play ();

		//Change Color
		Color Select = new Color (76f / 255f, 50f / 255f, 18f / 255f, 80f / 255f);
		Color unSelect = new Color (255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
		foreach (Transform obj in Content.transform) {
			obj.GetComponent<Image>().color = unSelect;
		}
		GetComponent<Image> ().color = Select;

		DoBtn.GetComponent<DoBouryaku> ().itemQty = qty;
		DoBtn.GetComponent<DoBouryaku> ().itemRank = name;
		DoBtn.GetComponent<DoBouryaku> ().Gunzei = Gunzei;

	}
}
