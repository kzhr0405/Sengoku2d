using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShisyaSyoukaijyoSelect : MonoBehaviour {

	public int qty = 0;
	public GameObject contentObj;
	public GameObject qtyObj;

	public void Start(){
		
		if (name == "Ge") {
			qty = 100;
		} else if (name == "Cyu") {
			qty = 200;
		} else if (name == "Jyo") {
			qty = 500;
		}
	}


	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [2].Play ();

		if (gameObject.transform.parent.name == "CyouteiContent") {
			
			contentObj = GameObject.Find ("CyouteiContent").gameObject;

			qtyObj = GameObject.Find ("CyouteiQty").gameObject;
			GameObject.Find ("YesButton").GetComponent<DoShisya> ().cyouteiFlg = true;
			GameObject.Find ("YesButton").GetComponent<DoShisya> ().syouninFlg = false;

		} else {
			contentObj = GameObject.Find ("SyouninContent").gameObject;

			qtyObj = GameObject.Find ("SyouninQty").gameObject;
			GameObject.Find ("YesButton").GetComponent<DoShisya> ().cyouteiFlg = false;
			GameObject.Find ("YesButton").GetComponent<DoShisya> ().syouninFlg = true;

		}

		qtyObj.GetComponent<Text> ().text = "x " + qty.ToString ();
		GameObject.Find ("YesButton").GetComponent<DoShisya> ().busyoDamaQty = qty;

		Color Select = new Color (76f / 255f, 50f / 255f, 18f / 255f, 80f / 255f);
		Color unSelect = new Color (255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
		foreach (Transform obj in contentObj.transform) {
			obj.GetComponent<Image>().color = unSelect;
		}
		GetComponent<Image> ().color = Select;

	}
}
