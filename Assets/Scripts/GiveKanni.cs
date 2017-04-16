using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class GiveKanni : MonoBehaviour {

	public int kanniId = 0;
	public string busyoId = "";
	public GameObject board;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [3].Play ();

		string tmp = "kanni" + busyoId;
		PlayerPrefs.SetInt (tmp, kanniId);


		//Reduce from My Kanni
		string myKanni = PlayerPrefs.GetString("myKanni");
		List<string> myKanniList = new List<string> ();
		if(myKanni.Contains(",")){
			char[] delimiterChars = {','};
			myKanniList = new List<string> (myKanni.Split (delimiterChars));
		}else{
			myKanniList.Add(myKanni);
		}
		myKanniList.Remove (kanniId.ToString());

		string newMyKanni = "";
		for (int i=0; i<myKanniList.Count; i++) {
			if(i==0){
				newMyKanni = myKanniList[i];
			}else{
				newMyKanni = newMyKanni + "," + myKanniList[i];
			}
		}
		PlayerPrefs.SetString ("myKanni", newMyKanni);

		//Add Busyo Kanni
		string newMyKanniWithBusyo = "";
		string myKanniWithBusyo = PlayerPrefs.GetString ("myKanniWithBusyo");
		if (myKanniWithBusyo != null && myKanniWithBusyo != "") {
			newMyKanniWithBusyo = myKanniWithBusyo + "," + kanniId;
		} else {
			newMyKanniWithBusyo = kanniId.ToString();		
		}
		PlayerPrefs.SetString ("myKanniWithBusyo", newMyKanniWithBusyo);

		PlayerPrefs.SetBool ("questSpecialFlg8",true);
		PlayerPrefs.Flush ();

		SyoguScene syogu = new SyoguScene();
		syogu.createSyoguView(busyoId);
		
		//Close Board
		CloseBoard close = new CloseBoard ();
		close.onClick();

	}
}
