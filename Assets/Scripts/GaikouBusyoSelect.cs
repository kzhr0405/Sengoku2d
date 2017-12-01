using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GaikouBusyoSelect : MonoBehaviour {

	public GameObject DoBtn;
	public GameObject Content;
	public string chiryaku = "";
	public int kuniDiff = 0;
	public int daimyoBusyoAtk = 0;
	public int daimyoBusyoDfc = 0;
	public int busyoId = 0;

	public void OnClick () {

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [2].Play ();

		string busyoName = "";
		foreach (Transform a in gameObject.transform) {
			foreach (Transform b in a) {
				if(b.name == "Chiryaku(Clone)"){
					chiryaku = b.Find("value").GetComponent<Text>().text;
				}
				if(b.name == "Text"){
					busyoName = b.GetComponent<Text>().text;
				}
			}
		}
		DoBtn.GetComponent<DoGaikou> ().busyoChiryaku = int.Parse(chiryaku);
		DoBtn.GetComponent<DoGaikou> ().busyoName = busyoName;
		DoBtn.GetComponent<DoGaikou> ().busyoId = busyoId;

		//Change Color
		Color Select = new Color (76f / 255f, 50f / 255f, 18f / 255f, 80f / 255f);
		Color unSelect = new Color (255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
		foreach (Transform obj in Content.transform) {
			obj.GetComponent<Image>().color = unSelect;
		}
		GetComponent<Image> ().color = Select;



		if (DoBtn.name == "DoDoumeiBtn") {
			//=友好度/20*知略/100
			string yukoudo = GameObject.Find ("YukouValue").GetComponent<Text> ().text;
			int ratio = (int.Parse (yukoudo) / 20 + int.Parse (chiryaku) / 100) * kuniDiff;
			if(ratio>80){
				ratio = 80;
			}else if(ratio<0) {
                ratio = 0;
            }
			GameObject.Find ("DoumeiRatio").transform.Find ("Value").GetComponent<Text> ().text = ratio.ToString () + "%";
			GameObject.Find ("DoDoumeiBtn").GetComponent<DoGaikou> ().doumeiRatio = ratio;
		
		} else if (DoBtn.name == "DoDoukatsuBtn") {
			//成功確率=(((友好度/10) * 知略/50)*国力差)*((100-大名武力)/100) *大名武力が100以上の時は99とする
			string yukoudo = GameObject.Find ("YukouValue").GetComponent<Text> ().text;
			float temp1 = ((int.Parse(yukoudo) / 10 * int.Parse (chiryaku) / 50)) * kuniDiff; 
			if(daimyoBusyoAtk > 99){
				daimyoBusyoAtk = 99;
			}
			float temp2 = 100 - daimyoBusyoAtk;
			float temp3 = temp2/100;
			float ratio = temp1 * temp3;
			if(ratio<1){
				ratio = 1;
			}else if(ratio > 80) {
                ratio = 80;
            }
			GameObject.Find ("DoDoukatsuBtn").GetComponent<DoGaikou> ().doukatsuRatio = ratio;
			GameObject.Find ("DoDoukatsuBtn").GetComponent<DoGaikou> ().moneyOKflg = true;

		}


	}
}
