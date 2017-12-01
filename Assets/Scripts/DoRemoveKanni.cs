using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class DoRemoveKanni : MonoBehaviour {

    public bool hpFlg = false;
    public int effect = 0;
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
		kanni.transform.Find("Text").GetComponent<Text>().enabled = true;
		kanni.GetComponent<RonkouKousyoMenu>().kanniId = 0;

        if (hpFlg) {
            Jinkei Jinkei = new Jinkei();
            int baseHP = GameObject.Find("GameScene").GetComponent<NowOnBusyo>().HP;
            int var = (baseHP * effect) / 100;
            Jinkei.jinkeiHpUpda(false, var);
        }

    }
}
