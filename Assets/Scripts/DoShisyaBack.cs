using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DoShisyaBack : MonoBehaviour {

	public GameObject back;

	public void OnClick(){
		
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (name == "YesButton") {
			audioSources [0].Play ();
			PlayerPrefs.DeleteKey("shisyaFlg");
            int langId = PlayerPrefs.GetInt("langId");
            GameObject content = GameObject.Find ("Content").gameObject;
			DoShisya doShisyaScript = new DoShisya ();
			foreach (Transform slotObj in content.transform) {
				int shisyaId = slotObj.GetComponent<ShisyaSelect> ().shisyaId;

				if (shisyaId == 1) {
					string msg = doShisyaScript.rejectDoumei (slotObj.gameObject);
				} else if (shisyaId == 3) {
					string msg = doShisyaScript.rejectEngun (slotObj.gameObject);
				} else if (shisyaId == 4) {
					string msg = doShisyaScript.rejectDoukatsu (slotObj.gameObject);
				} else if (shisyaId == 5) {
					string msg = doShisyaScript.rejectKoueki (slotObj.gameObject);
				} else if (shisyaId == 7) {
					string msg = doShisyaScript.rejectMitsugimono (slotObj.gameObject);
				} else if (shisyaId == 8) {
					string msg = doShisyaScript.rejectKyouhaku(slotObj.gameObject);
				} else if (shisyaId == 9) {
					string msg = doShisyaScript.rejectCyakai (slotObj.gameObject);
				} else if (shisyaId == 10) {
					string msg = doShisyaScript.rejectCyouteiCyusai (slotObj.gameObject,false);
				} else if (shisyaId == 11) {
					string msg = doShisyaScript.rejecToubatsurei (slotObj.gameObject);
				} else if (shisyaId == 12) {
					string msg = doShisyaScript.rejectBoueirei (slotObj.gameObject);
				} else if (shisyaId == 13) {
					string msg = doShisyaScript.rejectSyogunApproval (slotObj.gameObject, langId);
				} else if (shisyaId == 14) {
					string msg = doShisyaScript.rejectMusin (slotObj.gameObject, true);
				} else if (shisyaId == 15) {
					string msg = doShisyaScript.rejectMusin (slotObj.gameObject, false);
				} else if (shisyaId == 16) {
					string msg = doShisyaScript.rejectKyucyuGyouji (slotObj.gameObject);
				} else if (shisyaId == 17) {
					string msg = doShisyaScript.rejectCyouteiCyusai (slotObj.gameObject,true);
				} else if (shisyaId == 18) {
					string msg = doShisyaScript.rejectKoueki (slotObj.gameObject);
				} else if (shisyaId == 19) {
					string msg = doShisyaScript.rejectKoueki (slotObj.gameObject);
				} else if (shisyaId == 20) {
					PlayerPrefs.DeleteKey ("shisya20");
				} else if (shisyaId == 21) {
					PlayerPrefs.DeleteKey ("shisya21");
				}


			}


			PlayerPrefs.DeleteKey("shisya1");
			PlayerPrefs.DeleteKey("shisya2");
			PlayerPrefs.DeleteKey("shisya3");
			PlayerPrefs.DeleteKey("shisya4");
			PlayerPrefs.DeleteKey("shisya5");
			PlayerPrefs.DeleteKey("shisya6");
			PlayerPrefs.DeleteKey("shisya7");
			PlayerPrefs.DeleteKey("shisya8");
			PlayerPrefs.DeleteKey("shisya9");
			PlayerPrefs.DeleteKey("shisya10");
			PlayerPrefs.DeleteKey("shisya11");
			PlayerPrefs.DeleteKey("shisya12");
			PlayerPrefs.DeleteKey("shisya13");
			PlayerPrefs.DeleteKey("shisya14");
			PlayerPrefs.DeleteKey("shisya15");
			PlayerPrefs.DeleteKey("shisya16");
			PlayerPrefs.DeleteKey("shisya17");
			PlayerPrefs.DeleteKey("shisya18");
			PlayerPrefs.DeleteKey("shisya19");
			PlayerPrefs.DeleteKey("shisya20");
			PlayerPrefs.DeleteKey("shisya21");
            PlayerPrefs.SetBool("fromShisyaFlg", true);

            Application.LoadLevel("mainStage");

		} else {
			audioSources [1].Play ();
			Destroy (back);

		}
	}
}
