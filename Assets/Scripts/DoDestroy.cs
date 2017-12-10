using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class DoDestroy : MonoBehaviour {

	public int areaId = 0;
	public int activeKuniId = 0;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (name == "YesButton") {
			//Destroy
			audioSources [6].Play ();

			string temp = "naisei" + activeKuniId.ToString ();
			if (PlayerPrefs.HasKey (temp)) {
				
				//Get Data
				string naiseiString = PlayerPrefs.GetString (temp);
				List<string> naiseiList = new List<string>();
				char[] delimiterChars = {','};
				naiseiList = new List<string>(naiseiString.Split (delimiterChars));

				string targetValue = "0:0";
				naiseiList[areaId] = targetValue;


				//Remake string
				string newNaiseiString = "";
				for(int i=0; i<naiseiList.Count; i++){
					if(i+1 != naiseiList.Count){
						newNaiseiString = newNaiseiString + naiseiList[i] + ",";
					}else{
						newNaiseiString = newNaiseiString + naiseiList[i];
					}
				}
				PlayerPrefs.SetString (temp,newNaiseiString);
				PlayerPrefs.Flush();

				Message msg = new Message();
                int langId = PlayerPrefs.GetInt("langId");

                msg.makeMessage (msg.getMessage(118,langId));

				//Close Tab
				GameObject.Find ("close").GetComponent<CloseBoard> ().onClick ();

				//Initialization
				NaiseiController naisei = new NaiseiController ();
				naisei.Start ();

				//Animation
				GameObject naiseiView = GameObject.Find("NaiseiView").gameObject;
				Vector2 areaPosition = naiseiView.transform.Find (areaId.ToString ()).transform.localPosition;
				string animPath = "Prefabs/Naisei/DestroyAnim";
				GameObject destroyObj = Instantiate (Resources.Load (animPath)) as GameObject;		
				destroyObj.transform.SetParent (naiseiView.transform);
				destroyObj.transform.localScale = new Vector2 (200,100);
				destroyObj.transform.localPosition = new Vector2 (areaPosition.x + 10, areaPosition.y + 65);

			}


		}else if(name == "NoButton"){
			//Close
			audioSources [1].Play ();

			GameObject.Find ("TouchBack").GetComponent<CloseBoard>().onClick();
		}
	}
}
