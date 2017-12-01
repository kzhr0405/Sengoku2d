using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class KanniInfo : MonoBehaviour {

	public void OnClick(){

		//Kanni Master
		string myKanni = PlayerPrefs.GetString ("myKanni");
		List<string> myKanniList = new List<string>();
		char[] delimiterChars = {','};
		if (myKanni != null && myKanni != "") {
			if(myKanni.Contains(",")){
				myKanniList = new List<string>(myKanni.Split (delimiterChars));
			}else{
				myKanniList.Add(myKanni);
			}
		}

        string myBusyo = PlayerPrefs.GetString("myBusyo");
        List<string> myBusyoList = new List<string>();
        List<string> givenKaniList = new List<string>();
        myBusyoList.AddRange(myBusyo.Split(delimiterChars));
        foreach (string busyoId in myBusyoList) {
            //gokui
            string kanniTmp = "kanni" + busyoId.ToString();
            int givenKanni = PlayerPrefs.GetInt(kanniTmp);
            if (givenKanni != 0) {
                givenKaniList.Add(givenKanni.ToString());
            }
        }
        myKanniList.AddRange(givenKaniList);

        if (myKanniList.Count == 0) {
			Message msg = new Message ();
			msg.makeMessageOnBoard (msg.getMessage(135));
		
		} else {
			
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

			//qa
			string qaPath = "Prefabs/Common/Question";
			GameObject qa = Instantiate (Resources.Load (qaPath)) as GameObject;
			qa.transform.SetParent(popup.transform);
			qa.transform.localScale = new Vector2 (1, 1);
			RectTransform qaTransform = qa.GetComponent<RectTransform> ();
			qaTransform.anchoredPosition = new Vector3 (-540, 285, 0);
			qa.name = "qa";
			qa.GetComponent<QA> ().qaId = 5;

			//Pop text
			string popTextPath = "Prefabs/Busyo/popText";
			GameObject popText = Instantiate (Resources.Load (popTextPath)) as GameObject;
			popText.transform.SetParent (popup.transform);
			popText.transform.localScale = new Vector2 (0.35f, 0.35f);
			RectTransform popTextTransform = popText.GetComponent<RectTransform> ();
			popTextTransform.anchoredPosition = new Vector3 (0, 260, 0);
			popText.name = "popText";
			popText.GetComponent<Text> ().text = "叙任済官位";

			//Scroll
			string scrollPath = "Prefabs/Map/kanni/ScrollView";
			GameObject scroll = Instantiate (Resources.Load (scrollPath)) as GameObject;
			scroll.transform.SetParent (popup.transform);
			scroll.transform.localScale = new Vector2 (1, 1);
			RectTransform scrollTransform = scroll.GetComponent<RectTransform> ();
			scrollTransform.anchoredPosition = new Vector3 (0, -35, 0);
			scroll.name = "ScrollView";

			string counterPath = "Prefabs/Map/kanni/Counter";
			GameObject counter = Instantiate (Resources.Load (counterPath)) as GameObject;
			counter.transform.SetParent (scroll.transform);
			counter.transform.localScale = new Vector2 (1, 1);
			counter.transform.localPosition = new Vector2 (0, 220);



			Entity_kanni_mst kanniMst = Resources.Load ("Data/kanni_mst") as Entity_kanni_mst;
			string slotPath = "Prefabs/Map/kanni/KanniSlot";
			GameObject content = scroll.transform.Find ("Content").gameObject;


			for (int i = 0; i < myKanniList.Count; i++) {

				int myKanniId = int.Parse (myKanniList [i]);

				string kanniName = kanniMst.param [myKanniId - 1].Ikai + " " + kanniMst.param [myKanniId - 1].Kanni;
				string effectLabel = kanniMst.param [myKanniId - 1].EffectLabel;
				string effectValue = "+" + kanniMst.param [myKanniId - 1].Effect + kanniMst.param [myKanniId - 1].EffectUnit;
				int kuniQty = kanniMst.param [myKanniId - 1].NeedKuniQty;
				int syoukaijyoRank = kanniMst.param [myKanniId - 1].SyoukaijyoRank;

				GameObject slot = Instantiate (Resources.Load (slotPath)) as GameObject;
				slot.transform.SetParent (content.transform);
				slot.transform.localScale = new Vector2 (1, 1);

				slot.transform.Find ("KanniNameValue").GetComponent<Text> ().text = kanniName;
				GameObject effect = slot.transform.Find ("Effect").gameObject;
				effect.transform.Find ("Label").GetComponent<Text> ().text = effectLabel;
				effect.transform.Find ("Value").GetComponent<Text> ().text = effectValue;
				GameObject kuni = slot.transform.Find ("KuniQty").gameObject;
				kuni.transform.Find ("Value").GetComponent<Text> ().text = kuniQty.ToString ();
				GameObject syoukaijyo = slot.transform.Find ("Syoukaijyo").gameObject;
				string syoukaisyaName = "";
				if (syoukaijyoRank == 1) {
					syoukaisyaName = " 上 中 下";
				} else if (syoukaijyoRank == 2) {
					syoukaisyaName = " 上 中";
				} else if (syoukaijyoRank == 3) {
					syoukaisyaName = " 上";
				}
				syoukaijyo.transform.Find ("Value").GetComponent<Text> ().text = syoukaisyaName;


			}

			//Counter
			counter.transform.Find ("NowValue").GetComponent<Text> ().text = myKanniList.Count.ToString ();
			counter.transform.Find ("TotalValue").GetComponent<Text> ().text = kanniMst.param.Count.ToString ();
		}
	}
}
