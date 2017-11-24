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

        //Back
        //string backPath = "Prefabs/clearOrGameOver/Back";
        //GameObject backObj = Instantiate(Resources.Load(backPath)) as GameObject;
        //backObj.transform.SetParent(panel.transform);
        //backObj.transform.localScale = new Vector2(1, 1);

        string senarioPath = "Prefabs/Scenario/ScrollView";
        GameObject ScrollView = Instantiate(Resources.Load(senarioPath)) as GameObject;
        ScrollView.transform.SetParent(panel.transform);
        ScrollView.transform.localScale = new Vector2(1, 1.2f);
        ScrollView.transform.localPosition = new Vector2(0, 0);

        foreach(Transform chld in ScrollView.transform.FindChild("Content").transform) {
            chld.GetComponent<ScenarioSelect>().kuniIconView = kuniIconView;
            chld.GetComponent<ScenarioSelect>().KuniMap = KuniMap;
            chld.GetComponent<ScenarioSelect>().ScrollView = ScrollView;
            chld.GetComponent<ScenarioSelect>().FixedMessage = msg;
        }
    }
}
