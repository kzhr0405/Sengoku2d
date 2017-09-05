using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DoCyusai : MonoBehaviour {

	public string daimyoId = "";
	public string daimyoId2 = "";
	public string daimyoName = "";
	public string daimyoName2 = "";
	public GameObject confirm;
	public GameObject back;
	public GameObject btnContent;
	public GameObject uprContent;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		if(name == "YesButton"){
			audioSources [3].Play ();

			//Hyourou
			int hyourou = PlayerPrefs.GetInt ("hyourou");
			int newHyourou = hyourou - 5;
			PlayerPrefs.SetInt ("hyourou", newHyourou);
			GameObject.Find ("HyourouCurrentValue").GetComponent<Text> ().text = newHyourou.ToString ();

			Gaikou gaikou = new Gaikou ();
			int addYukoudo = 0;
			if (GameObject.Find ("GameController").GetComponent<MainStageController> ().myKuniQtyIsBiggestFlg) {
				addYukoudo = 10;
			}
			int upYukoudo = UnityEngine.Random.Range(5,20) + addYukoudo;
			gaikou.upOtherGaikouValue (int.Parse(daimyoId),int.Parse(daimyoId2),upYukoudo);

			//Message & Close
			GameObject.Find("bakuhuReturn").GetComponent<BakuhuMenuReturn>().OnClick();
			Message msg = new Message ();
            string OKtext = "";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                OKtext = "Friendship between " + daimyoName + " and " + daimyoName2 + " increased " + upYukoudo.ToString() + " point.";
            }else {
                OKtext = daimyoName + "殿と" + daimyoName2 + "殿の友好関係が" + upYukoudo.ToString() + "上昇しましたぞ。";
            }
			msg.makeMessageOnBoard (OKtext);


		}else if(name == "NoButton"){
			audioSources [1].Play ();

			//Reset
			foreach (Transform obj in btnContent.transform) {
				Destroy (obj.gameObject);
			}

			Color unSelect = new Color (255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
			foreach (Transform obj in uprContent.transform) {
				obj.GetComponent<Image> ().color = unSelect;
			}

			string cyusaiTxtPath = "Prefabs/Bakuhu/CyusaiText";
			GameObject cTxt = Instantiate (Resources.Load (cyusaiTxtPath)) as GameObject;
			cTxt.transform.SetParent (btnContent.transform);
			cTxt.transform.localScale = new Vector2 (0.12f, 0.15f);
			cTxt.name = "CyusaiText";



		}

		Destroy(confirm.gameObject);
		Destroy(back.gameObject);

	}
}
