using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class GiveGokui : MonoBehaviour {

	public int busyoId = 0;
	public int sakuId = 0;
	public string sakuName = "";
	public GameObject confirmObj;
	public GameObject backObj;
	public GameObject boardObj;
	public GameObject boardBackObj;


	public void OnClick(){

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (name == "YesButton") {
			audioSources [3].Play ();

			int gokuId = sakuId - 11;
			string gokuiItem = PlayerPrefs.GetString ("gokuiItem");
			List<string> gokuiItemList = new List<string> ();
			char[] delimiterChars = {','};
			gokuiItemList = new List<string> (gokuiItem.Split (delimiterChars));
			List<string> newGokuiItemList = new List<string> (gokuiItemList);

			newGokuiItemList [gokuId] = (int.Parse (gokuiItemList [gokuId]) - 1).ToString();

			string newGokuiItem = "";
			for (int i = 0; i < newGokuiItemList.Count; i++) {
				string qty = newGokuiItemList [i];

				if (i == 0) {
					newGokuiItem = qty;
				} else {
					newGokuiItem = newGokuiItem + "," + qty;
				}
			}
			PlayerPrefs.SetString ("gokuiItem",newGokuiItem);

			string tmp = "gokui" + busyoId;
			PlayerPrefs.SetInt (tmp,sakuId);

			PlayerPrefs.Flush ();

			Message msg = new Message();
            string Text = "";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                Text = sakuName + " was transmitted. \n I hope to see his active skill asap.";
            }else {
                Text = sakuName + "を伝授致しました。\n次の合戦が楽しみですな。";
            }
			msg.makeMessage (Text);
			Destroy(boardObj);
			Destroy(boardBackObj);

		}else if(name == "NoButton"){
			audioSources [1].Play ();
		}
		Destroy(confirmObj);
		Destroy(backObj);
		
	}



}
