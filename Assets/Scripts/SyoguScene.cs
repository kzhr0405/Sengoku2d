using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class SyoguScene : MonoBehaviour {

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();
	
		TabHandler tab = new TabHandler ();
		tab.tabButtonColor ("Syogu");
		
		GameObject mainController = GameObject.Find ("GameScene");
		mainController.GetComponent<NowOnButton> ().onButton = "Syogu";

		if (mainController.GetComponent<NowOnButton> ().lastButton != "Syogu") {
			Destroy(GameObject.Find ("BusyoStatus"));
			Destroy(GameObject.Find ("SenpouStatus"));
			Destroy (GameObject.Find ("KahouStatus"));

			/*Busyo View*/
			string busyoId = mainController.GetComponent<NowOnBusyo>().OnBusyo;

			//Make Syogu Status
			string path = "Prefabs/Busyo/SyoguMenu";
			GameObject syogu = Instantiate (Resources.Load (path)) as GameObject;
			syogu.transform.SetParent(GameObject.Find ("CenterView").transform);
			syogu.transform.localScale = new Vector2 (1, 1);
			syogu.name = "SyoguMenu";
			RectTransform syogu_transform = syogu.GetComponent<RectTransform> ();
			syogu_transform.anchoredPosition3D = new Vector3 (240, 31, 0);

			createSyoguView(busyoId);

			//Controller Setting
			mainController.GetComponent<NowOnButton> ().lastButton = "Syogu";
		}
	}

	public void createSyoguView(string busyoId){

		int lv = PlayerPrefs.GetInt (busyoId);

		Color ngImageColor = new Color (40f / 255f, 40f / 255f, 40f / 255f, 180f / 255f);
		Color ngTextColor = new Color (90f / 255f, 90f / 255f, 90f / 255f, 90f / 255f);
		Color okImageColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 150f / 255f);
		Color okTextColor = new Color (40f / 255f, 40f / 255f, 0f / 255f, 255f / 255f);
		
		GameObject kanjyo = GameObject.Find("kanjyo").gameObject;
		GameObject tsuihou = GameObject.Find("tsuihou").gameObject;

		int daimyoBusyoId = PlayerPrefs.GetInt ("myDaimyoBusyo");
        
		if (busyoId == daimyoBusyoId.ToString ()) {
			kanjyo.GetComponent<Image> ().color = ngImageColor; 
			kanjyo.transform.FindChild ("Text").GetComponent<Text> ().color = ngTextColor; 
			kanjyo.GetComponent<Button> ().enabled = false;

			tsuihou.GetComponent<Image> ().color = ngImageColor; 
			tsuihou.transform.FindChild ("Text").GetComponent<Text> ().color = ngTextColor; 
			tsuihou.GetComponent<Button> ().enabled = false;

		} else {
            string addLvTmp = "addlv" + busyoId.ToString();
            int maxLv = 100 + PlayerPrefs.GetInt(addLvTmp);

            if (lv != maxLv) {
				kanjyo.GetComponent<BusyoStatusButton> ().pa_lv = lv;
				kanjyo.GetComponent<Image> ().color = okImageColor; 
				kanjyo.transform.FindChild ("Text").GetComponent<Text> ().color = okTextColor;
				kanjyo.GetComponent<Button> ().enabled = true;
			} else {
				kanjyo.GetComponent<Image> ().color = ngImageColor; 
				kanjyo.transform.FindChild ("Text").GetComponent<Text> ().color = ngTextColor; 
				kanjyo.GetComponent<Button> ().enabled = false;
			}

			tsuihou.GetComponent<Image> ().color = okImageColor;
			tsuihou.transform.FindChild ("Text").GetComponent<Text> ().color = okTextColor; 
			tsuihou.GetComponent<Button> ().enabled = true;

		}
		
		GameObject kanni = GameObject.Find("kanni").gameObject;
		GameObject jyosyu = GameObject.Find("jyosyu").gameObject;
		GameObject syugyo = GameObject.Find("syugyo").gameObject;
		GameObject gokui = GameObject.Find("gokui").gameObject;
		kanni.GetComponent<RonkouKousyoMenu>().busyoId = busyoId;
		jyosyu.GetComponent<RonkouKousyoMenu>().busyoId = busyoId;
		syugyo.GetComponent<RonkouKousyoMenu>().busyoId = busyoId;
		gokui.GetComponent<RonkouKousyoMenu>().busyoId = busyoId;

		//Kanni
		string kanniTmp = "kanni" + busyoId;
		Kanni kanniScript = new Kanni();
		if (PlayerPrefs.HasKey (kanniTmp)) {
			int kanniId = PlayerPrefs.GetInt (kanniTmp);
			if (kanniId != 0) {
				foreach(Transform n in kanni.transform){
					if(n.name == "KanniName"){
						Destroy(n.gameObject);
					}
				}
				kanni.GetComponent<RonkouKousyoMenu> ().kanniId = kanniId;
				string path = "Prefabs/Busyo/KanniName";
				GameObject kanniName = Instantiate (Resources.Load (path)) as GameObject;
				kanniName.transform.SetParent (kanni.transform);
				kanniName.transform.localScale = new Vector2 (0.12f, 0.12f);
				kanniName.transform.localPosition = new Vector2 (0, 0);
				kanniName.name = "KanniName";

				string kanniNameString = kanniScript.getKanni(kanniId);
				string kanniIkai = kanniScript.getIkai(kanniId);
				kanniName.transform.FindChild("value").GetComponent<Text>().text = kanniIkai + "\n" + kanniNameString; 

				string effectLabel = kanniScript.getEffectLabel(kanniId);
				int effect = kanniScript.getEffect(kanniId);
				kanniName.transform.FindChild("effectLabel").GetComponent<Text>().text = effectLabel;
				kanniName.transform.FindChild("effectValue").GetComponent<Text>().text = "+" + effect.ToString() + "%"; 

				kanni.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
			}
		} else {
			foreach(Transform n in kanni.transform){
				if(n.name == "KanniName"){
					Destroy(n.gameObject);
				}
			}


			kanni.GetComponent<RonkouKousyoMenu> ().kanniId = 0;
			kanni.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
		}

		//Jyosyu
		string jyosyuTmp = "jyosyuBusyo" + busyoId;

		if (PlayerPrefs.HasKey (jyosyuTmp)) {
			BusyoInfoGet busyoInfo = new BusyoInfoGet ();
			string busyoName = busyoInfo.getName (int.Parse(busyoId));
			jyosyu.GetComponent<RonkouKousyoMenu>().jyosyuName = busyoName;

			int kuniId = PlayerPrefs.GetInt(jyosyuTmp);

			if(kuniId !=0){
				foreach(Transform n in jyosyu.transform){
					if(n.name == "JyosyuName"){
						Destroy(n.gameObject);
					}
				}

				KuniInfo kuni = new KuniInfo();
				string kuniName = kuni.getKuniName(kuniId);

				string jyosyuHeiTmp = "jyosyuHei" + busyoId;
				int jyosyuHei = PlayerPrefs.GetInt(jyosyuHeiTmp);

				jyosyu.GetComponent<RonkouKousyoMenu>().jyosyuKuniId = kuniId;
				string jyosyuPath = "Prefabs/Busyo/JyosyuName";
				GameObject jyosyuName = Instantiate (Resources.Load (jyosyuPath)) as GameObject;
				jyosyuName.transform.SetParent (jyosyu.transform);
				jyosyuName.transform.localScale = new Vector2 (0.12f, 0.12f);
				jyosyuName.transform.localPosition = new Vector2 (0, 0);
				jyosyuName.name = "JyosyuName";

				jyosyuName.transform.FindChild("value").GetComponent<Text>().text = kuniName; 
				jyosyuName.transform.FindChild("effectValue").GetComponent<Text>().text = "+" + jyosyuHei.ToString();
				
				jyosyu.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
			}			
		} else {
			foreach(Transform n in jyosyu.transform){
				if(n.name == "JyosyuName"){
					Destroy(n.gameObject);
				}
			}
			jyosyu.GetComponent<RonkouKousyoMenu> ().jyosyuKuniId = 0;
			jyosyu.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
		}




	}

}
