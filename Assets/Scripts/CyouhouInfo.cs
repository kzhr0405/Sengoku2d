using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class CyouhouInfo : MonoBehaviour {

	public void OnClick(){

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		//Count cyouhou
		string cyouhou = PlayerPrefs.GetString("cyouhou");
		if (cyouhou != null && cyouhou != "") {

			//SE
			audioSources[0].Play();

			/*Popup*/
			string backPath = "Prefabs/Busyo/back";
			GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
			back.transform.SetParent (GameObject.Find ("Map").transform);
			back.transform.localScale = new Vector2 (1, 1);
			RectTransform backTransform = back.GetComponent<RectTransform> ();
			backTransform.anchoredPosition = new Vector3 (0, 0, 0);

			//Popup Screen
			string popupPath = "Prefabs/Busyo/board";
			GameObject popup = Instantiate (Resources.Load (popupPath)) as GameObject;
			popup.transform.SetParent (GameObject.Find ("Map").transform);
			popup.transform.localScale = new Vector2 (1, 1);
			RectTransform popupTransform = popup.GetComponent<RectTransform> ();
			popupTransform.anchoredPosition = new Vector3 (0, 0, 0);
			popup.name = "board";
			GameObject close = popup.transform.FindChild ("close").gameObject;

			//qa
			string qaPath = "Prefabs/Common/Question";
			GameObject qa = Instantiate (Resources.Load (qaPath)) as GameObject;
			qa.transform.SetParent(popup.transform);
			qa.transform.localScale = new Vector2 (1, 1);
			RectTransform qaTransform = qa.GetComponent<RectTransform> ();
			qaTransform.anchoredPosition = new Vector3 (-540, 285, 0);
			qa.name = "qa";
			qa.GetComponent<QA> ().qaId = 4;


			//Pop text
			string popTextPath = "Prefabs/Busyo/popText";
			GameObject popText = Instantiate (Resources.Load (popTextPath)) as GameObject;
			popText.transform.SetParent (popup.transform);
			popText.transform.localScale = new Vector2 (0.35f, 0.35f);
			RectTransform popTextTransform = popText.GetComponent<RectTransform> ();
			popTextTransform.anchoredPosition = new Vector3 (0, 275, 0);
			popText.name = "popText";

            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                popText.GetComponent<Text>().text = "Spy";
            }
            else {
                popText.GetComponent<Text>().text = "諜報";
            }

            //kuni board
            string boardPath = "Prefabs/Map/cyouhou/CyouhouKuniBoard";
			GameObject board = Instantiate (Resources.Load (boardPath)) as GameObject;
			board.transform.SetParent (popup.transform);
			board.transform.localScale = new Vector2 (1, 1);
			RectTransform boardRect = board.GetComponent<RectTransform> ();
			boardRect.anchoredPosition3D = new Vector3 (-257, -89, 0);


			string statusPath = "Prefabs/Map/cyouhou/CyouhouStatus";
			GameObject status = Instantiate (Resources.Load (statusPath)) as GameObject;
			status.transform.SetParent (popup.transform);
			status.transform.localScale = new Vector2 (1, 1);
			RectTransform statusRect = status.GetComponent<RectTransform> ();
			statusRect.anchoredPosition3D = new Vector3 (293, -92, 0);


			//Scroll Preparation
			string cyouhouString = PlayerPrefs.GetString ("cyouhou");
			List<string> cyouhouList = new List<string> ();
			char[] delimiterChars = { ',' };
			if (cyouhouString != null && cyouhouString != "") {
				if (cyouhouString.Contains (",")) {
					cyouhouList = new List<string> (cyouhouString.Split (delimiterChars));
				} else {
					cyouhouList.Add (cyouhouString);
				}
			}


			//Scroll
			string popScrollPath = "Prefabs/Map/cyouhou/CyouhouScrollView";
			GameObject scroll = Instantiate (Resources.Load (popScrollPath)) as GameObject;
			scroll.transform.SetParent (popup.transform);
			scroll.transform.localScale = new Vector2 (1, 1);
			RectTransform scrollRect = scroll.GetComponent<RectTransform> ();
			scrollRect.anchoredPosition3D = new Vector3 (0, 525, 0);

			//Seiryoku
			string seiryoku = PlayerPrefs.GetString ("seiryoku");
			List<string> seiryokuList = new List<string> ();
			seiryokuList = new List<string> (seiryoku.Split (delimiterChars));

			string slotPath = "Prefabs/Map/cyouhou/CyouhouSlot";
			GameObject content = scroll.transform.FindChild ("Content").gameObject;
			KuniInfo kuni = new KuniInfo ();
			Daimyo daimyo = new Daimyo ();

			for (int i = 0; i < cyouhouList.Count; i++) {
				GameObject slot = Instantiate (Resources.Load (slotPath)) as GameObject;
				slot.transform.SetParent (content.transform);
				slot.transform.localScale = new Vector2 (1, 1);

				string slotValue = "";
				//Kuni Name
				int kuniId = int.Parse (cyouhouList [i]);
				string kuniName = kuni.getKuniName (kuniId,langId);

				//Daimyo
				int daimyoId = int.Parse (seiryokuList [kuniId - 1]);
				string daimyoName = daimyo.getName (daimyoId,langId);

				//Rank of Shinobi
				string snbTmp = "cyouhou" + kuniId.ToString ();
				string rankName = "";
				int rank = PlayerPrefs.GetInt (snbTmp);
				if (rank == 1) {
                    if (langId == 2) {
                        rankName = "Ninja Low";
                    }else {
                        rankName = "下忍";
                    }
				} else if (rank == 2) {
                    if (langId == 2) {
                        rankName = "Ninja Mid";
                    }else {
                        rankName = "中忍";
                    }
				} else if (rank == 3) {
                    if (langId == 2) {
                        rankName = "Ninja High";
                    }else {
                        rankName = "上忍";
                    }
                }

				slotValue = kuniName + "\n" + daimyoName + "\n" + rankName;
				slot.transform.FindChild ("Value").GetComponent<Text> ().text = slotValue;

				GameObject kamon = slot.transform.FindChild ("Image").gameObject;
				string imagePath = "Prefabs/Kamon/" + daimyoId.ToString ();
				kamon.GetComponent<Image> ().sprite = 
					Resources.Load (imagePath, typeof(Sprite)) as Sprite;

				//Parametor Setting
				CyouhouSelect script = slot.GetComponent<CyouhouSelect> ();
				script.kuniId = kuniId;
				script.kuniName = kuniName;
				script.daimyoId = daimyoId;
				script.daimyoName = daimyoName;
				script.snbRank = rank;
				script.board = board;
				script.status = status;
				script.close = close;
				script.seiryokuList = seiryokuList;

				if (i == 0) {
					slot.GetComponent<CyouhouSelect> ().OnClick ();
				}
			}

		} else {
			//Error
			audioSources [4].Play ();

			Message msg = new Message();
			string txt = "";
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                txt = "You don't have ninja spying in other country.\n Please get Ninja via trading or development your country.";
            }else {
                txt = "他国に潜伏中の忍はおりませんぞ。\n忍は他国との交易か、内政開発にて取得できます。";
            }
            msg.makeMessage (txt);


		}

	}
}
