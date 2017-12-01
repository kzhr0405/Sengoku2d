using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BGMSESwitch : MonoBehaviour {

	public bool OffFlg = false;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [2].Play ();

		GameObject parentObj = transform.parent.gameObject;
		if (name == "OnButton") {
			ChangeButtonColorByConfig(false, parentObj);
			ChangeVolumeByConfig(true, parentObj.name);
		} else {
			ChangeButtonColorByConfig(true, parentObj);
			ChangeVolumeByConfig(false, parentObj.name);
		}


	}

	public void ChangeButtonColorByConfig(bool OffFlg, GameObject parentObj){
		
		Color onBtnColor = new Color (85f / 255f, 85f / 255f, 85f / 255f, 160f / 255f);
		Color onTxtColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 40f / 255f);
		Color offBtnColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 200f / 255f);
		Color offTxtColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 175f / 255f);

		if(OffFlg){
			GameObject offObj = parentObj.transform.Find ("OffButton").gameObject;
			offObj.GetComponent<Image> ().color = onBtnColor;
			offObj.transform.Find("Text").GetComponent<Text> ().color = onTxtColor;
			offObj.GetComponent<Button> ().enabled = false;

			GameObject onObj = parentObj.transform.Find ("OnButton").gameObject;
			onObj.GetComponent<Image> ().color = offBtnColor;
			onObj.transform.Find("Text").GetComponent<Text> ().color = offTxtColor;
			onObj.GetComponent<Button> ().enabled = true;

		}else{
			GameObject onObj = parentObj.transform.Find ("OnButton").gameObject;
			onObj.GetComponent<Image> ().color = onBtnColor;
			onObj.transform.Find("Text").GetComponent<Text> ().color = onTxtColor;
			onObj.GetComponent<Button> ().enabled = false;

			GameObject offObj = parentObj.transform.Find ("OffButton").gameObject;
			offObj.GetComponent<Image> ().color = offBtnColor;
			offObj.transform.Find("Text").GetComponent<Text> ().color = offTxtColor;
			offObj.GetComponent<Button> ().enabled = true;

		}
	}


	public void ChangeVolumeByConfig(bool volumeFlg, string parentObjname){

		string tmpName = parentObjname + "Controller";
		AudioSource[] audioSources = GameObject.Find (tmpName).GetComponents<AudioSource> ();
		string path = "Prefabs/Common/SoundController/" + tmpName;
		GameObject soundController = Instantiate (Resources.Load (path)) as GameObject;		

		for(int i=0; i<audioSources.Length;i++){
			if (volumeFlg) {
				
				AudioSource[] tmpAudioSources = soundController.GetComponents<AudioSource> ();
				audioSources [i].volume = tmpAudioSources [i].volume;

			} else {
				audioSources [i].volume = 0;
			}
		}

		Destroy (soundController);


		string tmpDataName = parentObjname + "OffFlg";
		if (volumeFlg) {
			PlayerPrefs.SetBool (tmpDataName,false);
		}else{
			PlayerPrefs.SetBool (tmpDataName,true);
		}
		PlayerPrefs.Flush ();

	}

	public void StopSEVolume(){
		bool SEOffFlg = PlayerPrefs.GetBool ("SEOffFlg");
		if(SEOffFlg){
			AudioSource[] SEAudioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
			for(int i=0; i<SEAudioSources.Length;i++){				
				SEAudioSources [i].volume = 0;
			}
		}
	}

    public void StopKassenBGMVolume() {
        bool BGMOffFlg = PlayerPrefs.GetBool("BGMOffFlg");
        AudioSource[] SEAudioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        if (BGMOffFlg) {
            SEAudioSources[12].volume = 0;
        }else {
            SEAudioSources[12].volume = 1;
        }
    }

    public void StopBGMVolume(){
		bool BGMOffFlg = PlayerPrefs.GetBool ("BGMOffFlg");
		if(BGMOffFlg){
			AudioSource[] BGMAudioSources = null;
			if (SceneManager.GetActiveScene ().name == "naisei" || SceneManager.GetActiveScene().name == "tutorialNaisei") {
				BGMAudioSources = GameObject.Find ("NaiseiBGMController").GetComponents<AudioSource> ();
			} else {
                if(GameObject.Find("BGMController")) {
				    BGMAudioSources = GameObject.Find ("BGMController").GetComponents<AudioSource> ();
                }
            }

			for(int i=0; i<BGMAudioSources.Length;i++){				
			    BGMAudioSources [i].volume = 0;
			}
		}
	}

}
