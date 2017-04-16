using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class BakuhuInfo : MonoBehaviour {

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		GameObject mapObj = GameObject.Find ("Map").gameObject;

		string backPath = "Prefabs/Busyo/back";
		GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
		back.transform.SetParent(mapObj.transform);
		back.transform.localScale = new Vector2 (1, 1);
		RectTransform backTransform = back.GetComponent<RectTransform> ();
		backTransform.anchoredPosition = new Vector3 (0, 0, 0);

		//Popup Screen
		string popupPath = "Prefabs/Bakuhu/board";
		GameObject popup = Instantiate (Resources.Load (popupPath)) as GameObject;
		popup.transform.SetParent(mapObj.transform);
		popup.transform.localScale = new Vector2 (1, 1);
		RectTransform popupTransform = popup.GetComponent<RectTransform> ();
		popupTransform.anchoredPosition = new Vector3 (0, 0, 0);
		popup.name = "board";

		//Param
		GameObject scrollObj = popup.transform.FindChild("ScrollView").gameObject;
		GameObject contentObj = scrollObj.transform.FindChild ("Content").gameObject;

		foreach(Transform btnObj in contentObj.transform){
			btnObj.GetComponent<BakuhuMenu> ().board = popup;
			btnObj.GetComponent<BakuhuMenu> ().scrollView = scrollObj;

		}


		//Soubujirei Check
		bool soubujireiFlg = PlayerPrefs.GetBool("soubujireiFlg");
		if (!soubujireiFlg) {
			updateAtkOrderBtnStatus (contentObj);
		} else {
			kessen (contentObj);
		}

	}

	public void updateAtkOrderBtnStatus(GameObject contentObj){
		int daimyoId = PlayerPrefs.GetInt ("bakuhuTobatsuDaimyoId");
		if (daimyoId != null && daimyoId != 0) {
			GameObject atkBtn = contentObj.transform.FindChild ("AtkOrderBtn").gameObject;
			atkBtn.GetComponent<Button> ().enabled = false;
			atkBtn.GetComponent<Image> ().color = new Color (120f / 255f, 120f / 255f, 120f / 255f, 150f / 255f);
			atkBtn.transform.FindChild ("HyourouIcon").gameObject.SetActive (false);
			Daimyo daimyo = new Daimyo ();
			string toubatsuDaiymoName = daimyo.getName (daimyoId);
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                atkBtn.transform.FindChild ("Exp").GetComponent<Text> ().text = "You've already issued attack order to " + toubatsuDaiymoName + " in this season.\n You can order for 1 time in a season.";
            }else {
                atkBtn.transform.FindChild("Exp").GetComponent<Text>().text = "今季は既に" + toubatsuDaiymoName + "に討伐令が出ています。\n討伐令は季節に一回のみ実施可能です。";
            }
		}
	}


	public void kessen(GameObject contentObj){
		//disable
		contentObj.transform.FindChild ("SobujiKessenBtn").gameObject.SetActive(false);
		contentObj.transform.FindChild ("AtkOrderBtn").gameObject.SetActive(false);
		contentObj.transform.FindChild ("DfcOrderBtn").gameObject.SetActive(false);
		contentObj.transform.FindChild ("RelationshipBtn").gameObject.SetActive(false);

		Daimyo daimyo = new Daimyo ();
		string seiryoku = PlayerPrefs.GetString ("seiryoku");
		List<string> seiryokuList = new List<string> ();
		char[] delimiterChars = { ',' };
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
		List<int> kuniQtyByDaimyoId = new List<int> () {
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};

        List<int> openDaimyoList = new List<int>();
        string openKuni = PlayerPrefs.GetString("openKuni");
        List<string> openKuniList = new List<string>();
        if (openKuni != null && openKuni != "") {
            if (openKuni.Contains(",")) {
                openKuniList = new List<string>(openKuni.Split(delimiterChars));
            }else {
                openKuniList.Add(openKuni);
            }
        }

        int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
        for (int i = 0; i < seiryokuList.Count; i++) {
			int daimyoId = int.Parse(seiryokuList [i]);
			kuniQtyByDaimyoId [daimyoId - 1] = kuniQtyByDaimyoId [daimyoId - 1] + 1;

            int kuniId = i + 1;
            if(openKuniList.Contains(kuniId.ToString())) {
                if(!openDaimyoList.Contains(daimyoId)) {
                    openDaimyoList.Add(daimyoId);
                }
            }

		}




        string kessenBtnPath = "Prefabs/Bakuhu/KessenBtn";		
		List<int> checkedDaimyoId = new List<int> ();
		for(int i=0; i<seiryokuList.Count;i++){
			int daimyoId = int.Parse(seiryokuList [i]);
			if (daimyoId != myDaimyo) {
				if (!checkedDaimyoId.Contains (daimyoId)) {

                    //Open Kuni
                    if(openDaimyoList.Contains(daimyoId)) {

					    checkedDaimyoId.Add (daimyoId);
					    string daimyoName = daimyo.getName (daimyoId);
					    int busyoId = daimyo.getDaimyoBusyoId (daimyoId);
					    int kuniId = i + 1;

					    GameObject slotObj = Instantiate(Resources.Load (kessenBtnPath)) as GameObject;
					    slotObj.transform.SetParent(contentObj.transform);
					    slotObj.transform.localScale = new Vector2 (1,1);
					    slotObj.name = "KessenBtn";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            slotObj.transform.FindChild ("Title").GetComponent<Text> ().text = daimyoName + "\n Final War";
                        }else {
                            slotObj.transform.FindChild("Title").GetComponent<Text>().text = daimyoName + "\n決戦";
                        }
					    string imagePath = "Prefabs/Player/Sprite/unit" + busyoId.ToString ();
					    slotObj.transform.FindChild ("Mask").transform.FindChild("Image").GetComponent<Image>().sprite = 
						    Resources.Load (imagePath, typeof(Sprite)) as Sprite;
					    slotObj.GetComponent<BakuhuMenu> ().daimyoId = daimyoId;
					    slotObj.GetComponent<BakuhuMenu> ().daimyoName = daimyoName;
					    slotObj.GetComponent<BakuhuMenu> ().kuniId = kuniId;
					    int hyourou = kuniQtyByDaimyoId [daimyoId - 1] * 20;
					    if (hyourou > 80) {
						    hyourou = 80;
					    }
					    slotObj.GetComponent<BakuhuMenu> ().hyourouNo = hyourou;
					    slotObj.transform.FindChild ("HyourouIcon").transform.FindChild ("HyourouValue").GetComponent<Text> ().text = hyourou.ToString ();
                    }
                }
			}
			
		}


	}
}
