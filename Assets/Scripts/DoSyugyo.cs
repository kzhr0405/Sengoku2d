using UnityEngine;
using System.Collections;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class DoSyugyo : MonoBehaviour {

	public bool itemOKFlg = false;
	public bool moneyOKFlg = false;

	public int busyoId = 0;
	public int nextLv = 0;

	public bool kengouFlg = false;
	public int kengouId = 0;
	public string kahouType = "";

	public int kahouId = 0;

	public int paiedMoney = 0;


	public void OnClick(){
		Message msg = new Message ();
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
        int langId = PlayerPrefs.GetInt("langId");
        if (itemOKFlg) {
			if (moneyOKFlg) {
				//Process
				audioSources [3].Play ();
				if (kengouFlg) {
					//kengou
					string kengouString = PlayerPrefs.GetString ("kengouItem");
					char[] delimiterChars = { ',' };
					List<string> kengouList = new List<string> ();
					kengouList = new List<string> (kengouString.Split (delimiterChars));

					int qty = int.Parse (kengouList [kengouId - 1]);

					int remainQty = qty - 1;
					kengouList [kengouId - 1] = remainQty.ToString ();

					string newKengouString = "";
					for (int i = 0; i < kengouList.Count; i++) {

						if (i == 0) {
							newKengouString = kengouList [i];
						} else {
							newKengouString = newKengouString + "," + kengouList [i];
						}
					}

					PlayerPrefs.SetString ("kengouItem", newKengouString);

				} else {
					//kahou
					DoSell reduce = new DoSell ();

					string target = "";
					if (kahouType == "chishikisyo") {
						target = "availableChishikisyo";
					} else {
						target = "availableHeihousyo";
					}
					reduce.reduceKahou (target,kahouId,1);
				}

				//reduce Money
				int nowMoney = PlayerPrefs.GetInt("money");
				int newMoney = nowMoney - paiedMoney;
				PlayerPrefs.SetInt ("money",newMoney);

				//Update Lv
				string sakuLvTmp = "saku" + busyoId;
                if(nextLv == 0) {
                    nextLv = 1;
                }
				PlayerPrefs.SetInt (sakuLvTmp,nextLv);
				PlayerPrefs.SetBool ("questDailyFlg26",true);

				PlayerPrefs.Flush ();
				msg.makeMessage(msg.getMessage(65,langId));
				Destroy (GameObject.Find ("board(Clone)"));
				Destroy (GameObject.Find ("Back(Clone)"));

			} else {
				audioSources [4].Play ();
				msg.makeMessage (msg.getMessage(6,langId));
			}
		} else {
			audioSources [4].Play ();
			msg.makeMessage (msg.getMessage(66,langId));
		}

	}
}
