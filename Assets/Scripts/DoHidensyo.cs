using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DoHidensyo : MonoBehaviour {

	public bool requiredItem = true;
	public bool requiredMoney = true;
	public string itemType;
	public int requiredItemQty;
	public int requiredMoneyAmt;
	public string busyoId;
	public int nextSenpouLv;

	// Use this for initialization
	public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		//Check
		Message msg = new Message(); 
		if (requiredItem == false || requiredMoney == false) {
			//Error
			audioSources [4].Play ();
			msg.makeMessage (msg.getMessage(52));
			
		} else {
			//OK
			audioSources [3].Play ();
			//Decrease Item Qty
			if(itemType == "low"){
				int hidensyoGeQty = PlayerPrefs.GetInt ("hidensyoGe");
				hidensyoGeQty = hidensyoGeQty - requiredItemQty;
				PlayerPrefs.SetInt ("hidensyoGe", hidensyoGeQty);
			}else if(itemType == "middle"){
				int hidensyoCyuQty = PlayerPrefs.GetInt ("hidensyoCyu");
				hidensyoCyuQty = hidensyoCyuQty - requiredItemQty;
				PlayerPrefs.SetInt ("hidensyoCyu", hidensyoCyuQty);
			}else if(itemType == "high"){
				int hidensyoJyoQty = PlayerPrefs.GetInt ("hidensyoJyo");
				hidensyoJyoQty = hidensyoJyoQty - requiredItemQty;
				PlayerPrefs.SetInt ("hidensyoJyo", hidensyoJyoQty);

			}

			//Decrease Money
			int money = PlayerPrefs.GetInt ("money");
			money = money - requiredMoneyAmt;
			PlayerPrefs.SetInt ("money", money);

			//Increase Lv of senpou
			string tmp = "senpou" + busyoId; 
			PlayerPrefs.SetInt (tmp, nextSenpouLv);
			PlayerPrefs.SetBool ("questDailyFlg24",true);

			PlayerPrefs.Flush();


			//Message
			string busyoName = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyoName;
            string OKtext = "";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                OKtext = "Gave skillbook to " + busyoName+".\n Passive skill Lv increased.";
            }else {
                OKtext = busyoName + "に秘伝書を授与しました。\n戦法効果が上がります。";
            }
			msg.makeMessage(OKtext);
			
			//Reload
			//Close Board
			GameObject.Find ("close").GetComponent<CloseBoard>().onClick();
			SenpouScene senpou = new SenpouScene();
			senpou.createSenpouStatusView(busyoId);
		}
	}	
}
