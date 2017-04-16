using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class ShowDaimyoSelect : MonoBehaviour {

	public bool gameOverFlg;
	public GameObject fin;
	public GameObject panel;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();


		if (!gameOverFlg) {
			//Clear and Re-srart
			makeDaimyoSeiryoku();
			audioSources [0].Play ();

		} else {
			audioSources [0].Play ();

			//GameOver and Re-srart
			NewDaimyoDataMaker data = new NewDaimyoDataMaker();

			//Check Once Cleared or Not
			string gameClearDaimyo = PlayerPrefs.GetString ("gameClearDaimyo");
			if(gameClearDaimyo!=null && gameClearDaimyo !=""){
				//Once Cleared >> Can Choose Daimyo
				GameObject kuniMap = GameObject.Find("KuniMap").gameObject;
				GameObject KuniIconView = GameObject.Find("KuniIconView").gameObject;
				Destroy(kuniMap.gameObject);
				Destroy(KuniIconView.gameObject);

				makeDaimyoSeiryoku();

                /*
				//Daimyo Busyo Data Clear in the case there is no gacya history
				string gacyaDaimyoHst = PlayerPrefs.GetString ("gacyaDaimyoHst");
				char[] delimiterChars = {','};
				List<string> gacyaDaimyoHstList = new List<string>();
				if(gacyaDaimyoHst!=null && gacyaDaimyoHst !=""){
					if(gacyaDaimyoHst.Contains(",")){
						gacyaDaimyoHstList = new List<string> (gacyaDaimyoHst.Split (delimiterChars));
					}else{
						gacyaDaimyoHstList.Add(gacyaDaimyoHst);
					}
				}
				Daimyo daimyo = new Daimyo();
				int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
				int daimyoBusyoId = daimyo.getDaimyoBusyoId(myDaimyo);

				if(!gacyaDaimyoHstList.Contains(daimyoBusyoId.ToString())){
					//delete daimyo busyo from my busyo
					string myBusyoString = PlayerPrefs.GetString ("myBusyo");
					List<string> myBusyoList = new List<string>();
					if(myBusyoString!=null && myBusyoString !=""){
						if(myBusyoString.Contains(",")){
							myBusyoList = new List<string> (myBusyoString.Split (delimiterChars));
						}else{
							myBusyoList.Add(myBusyoString);
						}
					}


					string newMyBusyo = "";
					for(int i=0; i<myBusyoList.Count; i++){
						int myBusyoId = int.Parse(myBusyoList[i]);

						if(myBusyoId != daimyoBusyoId){
							if(newMyBusyo == ""){
								newMyBusyo = myBusyoId.ToString();
							}else{
								newMyBusyo = newMyBusyo + "," + myBusyoId.ToString();
							}
						}
					}
					PlayerPrefs.SetString ("myBusyo",newMyBusyo);
					PlayerPrefs.Flush();

				}
                */

			}else{
				//Never Cleared >> Start From Oda Nobunaga
				data.dataMake(true, 1, 19, "TP", true);

			}
		}
	}


	public void makeDaimyoSeiryoku(){

		Destroy (fin.gameObject);
		
		string kuniMapPath = "Prefabs/clearOrGameOver/KuniMap";
		GameObject KuniMap = Instantiate (Resources.Load (kuniMapPath)) as GameObject;
		KuniMap.transform.SetParent (panel.transform);
		KuniMap.transform.localScale = new Vector2 (1, 1);
		
		string kuniIconViewPath = "Prefabs/clearOrGameOver/KuniIconView";
		GameObject kuniIconView = Instantiate (Resources.Load (kuniIconViewPath)) as GameObject;
		kuniIconView.transform.SetParent (panel.transform);
		kuniIconView.transform.localScale = new Vector2 (1, 1);
		
		string messagePath = "Prefabs/clearOrGameOver/FixedMessage";
		GameObject msg = Instantiate (Resources.Load (messagePath)) as GameObject;
		msg.transform.SetParent (panel.transform);
		msg.transform.localScale = new Vector2 (1, 1);
		msg.transform.localPosition = new Vector2 (0, 380);
		
		//Show
		Entity_kuni_mst kuniMst = Resources.Load ("Data/kuni_mst") as Entity_kuni_mst;
		Entity_daimyo_mst daimyoMst = Resources.Load ("Data/daimyo_mst") as Entity_daimyo_mst;
		
		string seiryoku = "1,2,3,4,5,6,7,8,3,4,9,10,12,11,13,14,15,16,3,17,18,17,19,8,19,19,20,21,22,23,24,25,26,27,28,29,30,31,31,32,33,34,35,35,36,37,38,38,38,38,31,31,31,39,40,41,41,41,41,42,43,44,45,45,46";
		List<string> seiryokuList = new List<string> ();
		char[] delimiterChars = {','};
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
		
		string kuniQtybyDaimyo = "1,1,3,2,1,1,1,2,1,1,1,1,1,1,1,1,2,1,3,1,1,1,1,1,1,1,1,1,1,1,5,1,1,1,2,1,1,4,1,1,4,1,1,1,2,1";
		List<string> KuniQtyList = new List<string> ();
		KuniQtyList = new List<string> (kuniQtybyDaimyo.Split (delimiterChars));
		
		string gameClearDaimyo = PlayerPrefs.GetString ("gameClearDaimyo");
		List<string> gameClearDaimyoList = new List<string> ();
		if (gameClearDaimyo != null && gameClearDaimyo != "") {
			if (gameClearDaimyo.Contains (",")) {
				gameClearDaimyoList = new List<string> (gameClearDaimyo.Split (delimiterChars));
			} else {
				gameClearDaimyoList.Add (gameClearDaimyo);
			}
		}
		
		string myBusyo = PlayerPrefs.GetString ("myBusyo");
		List<string> myBusyoList = new List<string> ();
		if (myBusyo != null && myBusyo != "") {
			if (myBusyo.Contains (",")) {
                myBusyoList = new List<string> (myBusyo.Split (delimiterChars));
			} else {
                myBusyoList.Add (myBusyo);
			}
		}		
		string kuniPath = "Prefabs/Map/kuni/";

        KuniInfo kuniScript = new KuniInfo();
        Daimyo daimyoScript = new Daimyo();
		for (int i=0; i<kuniMst.param.Count; i++) {
			int kuniId = kuniMst.param [i].kunId;
			
			string newKuniPath = kuniPath + kuniId.ToString ();
			int locationX = kuniMst.param [i].locationX;
			int locationY = kuniMst.param [i].locationY;
			
			GameObject kuni = Instantiate (Resources.Load (newKuniPath)) as GameObject;
			
			kuni.transform.SetParent (kuniIconView.transform);
			kuni.name = kuniId.ToString ();
			kuni.GetComponent<SendParam> ().kuniId = kuniId;
			kuni.GetComponent<SendParam> ().kuniName = kuniScript.getKuniName(kuniId);
			kuni.transform.localScale = new Vector2 (1, 1);
			kuni.GetComponent<SendParam> ().naiseiItem = kuniMst.param [i].naisei;
			
			//Seiryoku Handling
			int daimyoId = int.Parse (seiryokuList [kuniId - 1]);			
			string daimyoName = daimyoScript.getName(daimyoId);
			int daimyoBusyoIdTemp = daimyoMst.param [daimyoId - 1].busyoId;
			kuni.GetComponent<SendParam> ().daimyoId = daimyoId;
			kuni.GetComponent<SendParam> ().daimyoName = daimyoName;
			kuni.GetComponent<SendParam> ().daimyoBusyoId = daimyoBusyoIdTemp;
			kuni.GetComponent<SendParam> ().kuniQty = int.Parse (KuniQtyList [daimyoId - 1]);
			
			if (gameClearDaimyoList.Contains (daimyoId.ToString ())) {
				kuni.GetComponent<SendParam> ().gameClearFlg = true;
			}
			
			if (myBusyoList.Contains (daimyoBusyoIdTemp.ToString ())) {
				kuni.GetComponent<SendParam> ().busyoHaveFlg = true;
			}
			
			//Color Handling
			float colorR = (float)daimyoMst.param [daimyoId - 1].colorR;
			float colorG = (float)daimyoMst.param [daimyoId - 1].colorG;
			float colorB = (float)daimyoMst.param [daimyoId - 1].colorB;
			Color kuniColor = new Color (colorR / 255f, colorG / 255f, colorB / 255f, 255f / 255f);
			
			KuniMap.transform.FindChild (kuni.name).GetComponent<Image> ().color = kuniColor;
			
			//Daimyo Kamon Image
			string imagePath = "Prefabs/Kamon/" + daimyoId.ToString ();
			kuni.GetComponent<Image> ().sprite = 
				Resources.Load (imagePath, typeof(Sprite)) as Sprite;
			RectTransform kuniTransform = kuni.GetComponent<RectTransform> ();
			kuniTransform.anchoredPosition = new Vector3 (locationX, locationY, 0);
		}
	}

}
