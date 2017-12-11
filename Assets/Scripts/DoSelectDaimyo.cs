using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DoSelectDaimyo : MonoBehaviour {

	public int daimyoId = 0;
	public int daimyoBusyoId = 0;
	public bool busyoHaveFlg;
	public string heisyu = "";

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (name == "YesButton" || name == "YesButtonHard") {
            int senarioId = GameObject.Find("GameController").GetComponent<clearOrGameOver>().senarioId;            

            if (name == "YesButtonHard") {
                PlayerPrefs.SetBool("hardFlg",true);                
            }else {
                PlayerPrefs.SetBool("hardFlg", false);
            }
            
            PlayerPrefs.Flush();
            audioSources [5].Play ();

            //init data
            if(GameObject.Find("BGMController")) {
                AudioSource[] bgmSources = GameObject.Find("BGMController").GetComponents<AudioSource>();
                bgmSources[1].Stop();
                bgmSources[0].Play();
            }
            //same daimyo or not
            NewDaimyoDataMaker data = new NewDaimyoDataMaker();

			int preMyDaimyo = PlayerPrefs.GetInt("myDaimyo");
            if (preMyDaimyo == daimyoId) {
				data.dataMake(busyoHaveFlg, daimyoId, daimyoBusyoId, heisyu, true, senarioId);
			} else {
				data.dataMake(busyoHaveFlg, daimyoId, daimyoBusyoId, heisyu, false, senarioId);
			}


		} else {
			audioSources [1].Play ();

			//Back
			Destroy(GameObject.Find("DaimyoSelectConfirm"));
			Destroy(GameObject.Find("Back"));
		}
	}
}
