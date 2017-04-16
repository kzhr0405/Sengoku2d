using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class SenpouScene : MonoBehaviour {

	public Color OKClorBtn = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
	public Color OKClorTxt = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
	public Color NGClorBtn = new Color (133 / 255f, 133 / 255f, 80 / 255f, 255f / 255f);
	public Color NGClorTxt = new Color (90 / 255f, 90 / 255f, 40 / 255f, 255f / 255f);

	public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		TabHandler tab = new TabHandler ();
		tab.tabButtonColor ("Senpou");

		GameObject mainController = GameObject.Find ("GameScene");
		mainController.GetComponent<NowOnButton> ().onButton = "Senpou";

		if (GameObject.Find ("GameScene").GetComponent<NowOnButton> ().lastButton != "Senpou") {
			/*CenterView*/
			//Delete Previous View
			Destroy(GameObject.Find ("BusyoStatus"));
			Destroy (GameObject.Find ("KahouStatus"));
			Destroy (GameObject.Find ("SyoguMenu"));

			/*Busyo View*/
			string busyoId = mainController.GetComponent<NowOnBusyo>().OnBusyo;

			//Make Senpou Status
			string senpouPath = "Prefabs/Busyo/SenpouStatus";
			GameObject Senpou = Instantiate (Resources.Load (senpouPath)) as GameObject;
			Senpou.transform.SetParent(GameObject.Find ("CenterView").transform);
			Senpou.transform.localScale = new Vector2 (1, 1);
			Senpou.name = "SenpouStatus";

			RectTransform senpou_transform = Senpou.GetComponent<RectTransform> ();
			senpou_transform.anchoredPosition3D = new Vector3 (240, 31, 0);


			/*Centeral View*/
			createSenpouStatusView(busyoId);
			createSakuStatusView(busyoId);

			//Controller Setting
			mainController.GetComponent<NowOnButton> ().lastButton = "Senpou";

		}
	}


	public void createSenpouStatusView(string busyoId){
		StatusGet sts = new StatusGet();
		ArrayList senpouArray = sts.getSenpou(int.Parse(busyoId),false);

		int senpouId = (int)senpouArray[0];
		string senpouTyp = senpouArray[1].ToString();
		string senpouName = senpouArray[2].ToString();
		string senpouExp = senpouArray[3].ToString();
		float senpouEach = (float)senpouArray[4];
		float senpouRatio = (float)senpouArray[5];
		float senpouTerm = (float)senpouArray[6];
		int senpouStatus = (int)senpouArray[7];
		int senpouLv = (int)senpouArray[8];

        //Kahou Adjustment
        KahouStatusGet kahouSts = new KahouStatusGet ();
		string[] KahouSenpouArray =kahouSts.getKahouForSenpou (busyoId,senpouStatus);
		string kahouTyp = KahouSenpouArray [0];
		string adjSenpouStatus = senpouStatus.ToString();

		if (kahouTyp != null) {
			if (kahouTyp == "Attack") {
				int kahouStatus = int.Parse (KahouSenpouArray [1]);
				adjSenpouStatus = adjSenpouStatus + "<color=#35d74bFF>(+" + kahouStatus.ToString() + ")</color>";
			} else {
				Debug.Log ("Not Yet except for Attack");
			}
		}

        //Explanation Modification
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            senpouExp = senpouExp.Replace("ABC", adjSenpouStatus);
		    senpouExp = senpouExp.Replace("DEF", senpouEach.ToString());
		    senpouExp = senpouExp.Replace("GHI", senpouRatio.ToString());
		    senpouExp = senpouExp.Replace("JKL", senpouTerm.ToString());
        }else {
            senpouExp = senpouExp.Replace("A", adjSenpouStatus);
            senpouExp = senpouExp.Replace("B", senpouEach.ToString());
            senpouExp = senpouExp.Replace("C", senpouRatio.ToString());
            senpouExp = senpouExp.Replace("D", senpouTerm.ToString());
        }
		//Fill fields by got Senpou Value
		GameObject.Find ("SenpouValue").GetComponent<Text> ().text = senpouName;
		GameObject.Find ("SenpouLvValue").GetComponent<Text> ().text = senpouLv.ToString();
		GameObject.Find ("SenpouExpValue").GetComponent<Text> ().text = senpouExp;


		GameObject btn = GameObject.Find("SenpouStatus").transform.FindChild("ButtonHeihousyo").gameObject;
		if (senpouLv < 20) {
			btn.GetComponent<Image> ().color = OKClorBtn;
			btn.transform.FindChild ("Text").GetComponent<Text> ().color = OKClorTxt;
			btn.GetComponent<Button>().enabled = true;
		} else {
			btn.GetComponent<Image> ().color = NGClorBtn;
			btn.transform.FindChild ("Text").GetComponent<Text> ().color = NGClorTxt;
			btn.GetComponent<Button>().enabled = false;
		}
	}


	public void createSakuStatusView(string busyoId){
		
		/*Saku Fields*/
		Saku saku = new Saku ();
		List<string> sakuList = new List<string>();
		string tmp = "gokui" + busyoId;
		if (PlayerPrefs.HasKey (tmp)) {
			int gokuiId = PlayerPrefs.GetInt (tmp);
			sakuList = saku.getGokuiInfoForLabel(int.Parse(busyoId), gokuiId);
		} else {
			sakuList = saku.getSakuInfoForLabel (int.Parse (busyoId));
		}

		//Icon
		string sakuPath = "Prefabs/Saku/saku" + sakuList[0];
		GameObject sakuIcon = Instantiate (Resources.Load (sakuPath)) as GameObject;
		GameObject StatusSaku = GameObject.Find("SenpouStatus").transform.FindChild("StatusSaku").gameObject;
		foreach ( Transform n in StatusSaku.transform ){
			if(n.tag == "Saku"){
				GameObject.Destroy(n.gameObject);
			}
		}
		sakuIcon.transform.SetParent (StatusSaku.transform);
		sakuIcon.transform.localScale = new Vector2 (1, 1);
		sakuIcon.GetComponent<Button>().enabled = false;
		RectTransform sakuIcon_transform = sakuIcon.GetComponent<RectTransform>();
		sakuIcon_transform.anchoredPosition3D = new Vector3(-260,0,0);
		
		StatusSaku.transform.FindChild("SakuName").transform.FindChild("SakuNameValue").GetComponent<Text>().text = sakuList[1];
		StatusSaku.transform.FindChild("SakuExp").transform.FindChild("SakuExpValue").GetComponent<Text>().text = sakuList[2];
		StatusSaku.transform.FindChild("SakuLv").transform.FindChild("SakuLvValue").GetComponent<Text>().text = sakuList[3];
		
		
	}
}
