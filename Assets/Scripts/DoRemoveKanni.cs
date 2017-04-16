using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class DoRemoveKanni : MonoBehaviour {

	public string busyoId = "";

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (name == "YesButton") {
			audioSources [4].Play ();

			//Add to myKanni
			removeKanni(busyoId);


			//Screen Optimization
			//Remove Previous Screen
			Destroy(GameObject.Find("KanniRemoveConfirm"));
			Destroy(GameObject.Find("Back(Clone)"));




		} else {
			audioSources [1].Play ();

			Destroy(GameObject.Find("KanniRemoveConfirm"));
			Destroy(GameObject.Find("Back(Clone)"));
		}
	}

	public void removeKanni(string busyoId){

		string tmp = "kanni" + busyoId;
		int kanniId = PlayerPrefs.GetInt(tmp);
		string myKanni = PlayerPrefs.GetString("myKanni");
		string newMyKanni = "";
		if(myKanni != null && myKanni !=""){
			newMyKanni = myKanni + "," + kanniId;
		}else{
			newMyKanni = kanniId.ToString();
		}
		PlayerPrefs.SetString ("myKanni", newMyKanni);
		
		//Remove from myBusyoKanni
		string myKanniWithBusyo = PlayerPrefs.GetString ("myKanniWithBusyo");
		List<string> myKanniWithBusyoList = new List<string> ();
		if(myKanniWithBusyo.Contains(",")){
			char[] delimiterChars = {','};
			myKanniWithBusyoList = new List<string> (myKanniWithBusyo.Split (delimiterChars));
		}else{
			myKanniWithBusyoList.Add(myKanniWithBusyo);
		}
		myKanniWithBusyoList.Remove (kanniId.ToString());
		
		string newMyKanniWithBusyo = "";
		for (int i=0; i<myKanniWithBusyoList.Count; i++) {
			if(i==0){
				newMyKanniWithBusyo = myKanniWithBusyoList[i];
			}else{
				newMyKanniWithBusyo = newMyKanni + "," + myKanniWithBusyoList[i];
			}
		}
		PlayerPrefs.SetString ("myKanniWithBusyo", newMyKanniWithBusyo);
		PlayerPrefs.DeleteKey(tmp);
		PlayerPrefs.Flush ();

		//Remove Kanni
		GameObject kanni = GameObject.Find("kanni").gameObject;
		foreach(Transform n in kanni.transform){
			if(n.name == "KanniName"){
				Destroy(n.gameObject);
			}
		}
		//Activate
		kanni.transform.FindChild("Text").GetComponent<Text>().enabled = true;
		kanni.GetComponent<RonkouKousyoMenu>().kanniId = 0;
	}
}
