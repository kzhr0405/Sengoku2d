using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class TabibitoItemGetter : MonoBehaviour {

	public bool isNanbansenFlg = false;
	public string itemCd = ""; //money or hyourou or item
	public int itemId = 0;
	public int itemQty = 0;
	public GameObject popButton;

	//Nanbansen
	public bool moneyCheckFlg = false;
	public int paiedMoney = 0;
	public GameObject shipObj;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		string Text = "";
		Kahou kahou = new Kahou ();
        Message msg = new Message();

		if (!isNanbansenFlg) {

            audioSources[3].Play ();
            Text = msg.getMessage(122);

            if (itemCd == "money") {
				//Money
				int nowMoney = PlayerPrefs.GetInt ("money");
				nowMoney = nowMoney + itemQty;
                if (nowMoney < 0) {
                    nowMoney = int.MaxValue;
                }
                PlayerPrefs.SetInt ("money", nowMoney);

				
				//Label
				Text nowMoneyLabelText = GameObject.Find ("MoneyValue").GetComponent<Text> ();
				int nowMoneyLabel = int.Parse (nowMoneyLabelText.text) + itemQty;
				nowMoneyLabelText.text = nowMoneyLabel.ToString ();

			} else if (itemCd == "hyourou") {
				//Hyourou

				//Check
				int maxHyourou = PlayerPrefs.GetInt ("hyourouMax");
				int nowHyourou = PlayerPrefs.GetInt ("hyourou");
				nowHyourou = nowHyourou + itemQty;

				if (maxHyourou <= nowHyourou) {
					nowHyourou = maxHyourou;
					PlayerPrefs.SetInt ("hyourou", nowHyourou);

					Text = msg.getMessage(123);

					//Label
					Text nowHyourouLabelText = GameObject.Find ("HyourouCurrentValue").GetComponent<Text> ();
					nowHyourouLabelText.text = nowHyourou.ToString ();

				} else {

					PlayerPrefs.SetInt ("hyourou", nowHyourou);

					//Label
					Text nowHyourouLabelText = GameObject.Find ("HyourouCurrentValue").GetComponent<Text> ();
					nowHyourouLabelText.text = nowHyourou.ToString ();

				}
			} else if (itemCd == "kengou") {

				string kengouString = PlayerPrefs.GetString ("kengouItem");
				List<string> kengouList = new List<string> ();
				char[] delimiterChars = {','};
				kengouList = new List<string> (kengouString.Split (delimiterChars));

				string qty = kengouList [itemId - 1];
				int newQty = int.Parse (qty) + itemQty;
				kengouList [itemId - 1] = newQty.ToString ();

				string newKengouString = "";
				for (int i=0; i<kengouList.Count; i++) {

					if (i == 0) {
						newKengouString = kengouList [i];
					} else {
						newKengouString = newKengouString + "," + kengouList [i];
					}
				}

				PlayerPrefs.SetString ("kengouItem", newKengouString);


			} else if (itemCd == "gokui") {
				string gokuiString = PlayerPrefs.GetString ("gokuiItem");
				List<string> gokuiList = new List<string> ();
				char[] delimiterChars = {','};
				gokuiList = new List<string> (gokuiString.Split (delimiterChars));
				
				string qty = gokuiList [itemId - 1];
				int newQty = int.Parse (qty) + itemQty;
				gokuiList [itemId - 1] = newQty.ToString ();
				
				string newGokuiString = "";
				for (int i=0; i<gokuiList.Count; i++) {
					
					if (i == 0) {
						newGokuiString = gokuiList [i];
					} else {
						newGokuiString = newGokuiString + "," + gokuiList [i];
					}
				}
				
				PlayerPrefs.SetString ("gokuiItem", newGokuiString);
			
			
			} else if (itemCd == "CyouheiTP") {

				registerTP();

			} else if (itemCd.Contains ("nanban")) {
				registerNanban();

			} else if (itemCd == "bugu") {
				kahou.registerBugu (itemId);
			} else if (itemCd == "gusoku") {
				kahou.registerGusoku (itemId);
			} else if (itemCd == "kabuto") {
				kahou.registerKabuto (itemId);
			} else if (itemCd == "meiba") {
				kahou.registerMeiba (itemId);
			} else if (itemCd == "cyadougu") {
				kahou.registerCyadougu (itemId);
			} else if (itemCd == "chishikisyo") {
				kahou.registerChishikisyo (itemId);
			} else if(itemCd == "heihousyo"){                
				kahou.registerHeihousyo (itemId);

			}else if(itemCd == "cyoutei"||itemCd == "koueki"){
				registerKouekiOrCyoutei(itemCd, itemId);
			}

			PlayerPrefs.SetBool ("questDailyFlg21",true);
			PlayerPrefs.Flush();
			PlayerPrefs.Flush ();

			//Delete Tap Button
			Destroy (popButton);
		
		} else {
			//Nanbansen
			if(moneyCheckFlg){

                audioSources[3].Play ();
				//Money Handling
				int money = PlayerPrefs.GetInt ("money");
				money = money - paiedMoney;
				PlayerPrefs.SetInt ("money",money);
				PlayerPrefs.Flush();
				GameObject.Find("MoneyValue").GetComponent<Text>().text = money.ToString();
				
				//Fadeout & Button false
				shipObj.GetComponent<FadeoutImage>().enabled = true;
				shipObj.GetComponent<Button>().enabled = false;

				//nanbansen flg change
				GameObject.Find("NaiseiController").GetComponent<NaiseiController>().isNanbansenFlg = false;

				//Register Item
				if(itemCd.Contains("CyouheiTP")){
					registerTP();
				}else if(itemCd.Contains("nanban")){
					registerNanban();
				} else if (itemCd == "bugu") {
					kahou.registerBugu (itemId);
				} else if (itemCd == "gusoku") {
					kahou.registerGusoku (itemId);
				} else if (itemCd == "kabuto") {
					kahou.registerKabuto (itemId);
				} else if (itemCd == "meiba") {
					kahou.registerMeiba (itemId);
				} else if (itemCd == "cyadougu") {
					kahou.registerCyadougu (itemId);
				} else if (itemCd == "chishikisyo") {
					kahou.registerChishikisyo (itemId);				
                }else if (itemCd == "heihousyo") {
                    kahou.registerHeihousyo(itemId);
                }

            Text = msg.getMessage(124);
			}else{
				audioSources [4].Play ();
                Text = msg.getMessage(125);
            }
		}

		//Message
		msg.makeMessage(Text);

		//Close
		GameObject.Find ("close").GetComponent<CloseBoard> ().onClick ();

	}



	public void registerTP(){

		string cyouheiString = PlayerPrefs.GetString ("cyouheiTP");
		char[] delimiterChars = {','};
		string[] cyouheiList = cyouheiString.Split (delimiterChars);
		string newCyouheiString = "";
		if (itemId == 1) {
			int tempQty = int.Parse (cyouheiList [0]);
			tempQty = tempQty + itemQty;
			newCyouheiString = tempQty.ToString () + "," + cyouheiList [1] + "," + cyouheiList [2];
			
		} else if (itemId == 2) {
			int tempQty = int.Parse (cyouheiList [1]);
			tempQty = tempQty + itemQty;
			newCyouheiString = cyouheiList [0] + "," + tempQty.ToString () + "," + cyouheiList [2];
			
		} else if (itemId == 3) {
			int tempQty = int.Parse (cyouheiList [2]);
			tempQty = tempQty + itemQty;
			newCyouheiString = cyouheiList [0] + "," + cyouheiList [1] + "," + tempQty.ToString ();
		}
		
		PlayerPrefs.SetString ("cyouheiTP", newCyouheiString);
	
	}

	public void registerNanban(){

		string nanbanString = PlayerPrefs.GetString ("nanbanItem");
		List<string> nanbanList = new List<string> ();
		char[] delimiterChars = {','};
		nanbanList = new List<string> (nanbanString.Split (delimiterChars));
		
		string qty = nanbanList [itemId - 1];
		int newQty = int.Parse (qty) + 1;
		nanbanList [itemId - 1] = newQty.ToString ();
		
		string newNanbanString = "";
		for (int i=0; i<nanbanList.Count; i++) {
			
			if (i == 0) {
				newNanbanString = nanbanList [i];
			} else {
				newNanbanString = newNanbanString + "," + nanbanList [i];
			}
		}
		
		PlayerPrefs.SetString ("nanbanItem", newNanbanString);

	}

	public void registerKouekiOrCyoutei(string itemCd, int itemId){
		string nowQty = PlayerPrefs.GetString (itemCd);
		List<string> nowQtyList = new List<string> ();
		char[] delimiterChars = {','};
		nowQtyList = new List<string> (nowQty.Split (delimiterChars));

		string newQty = "";
		if (itemId == 1) {
			//Ge
			int nowNo = int.Parse(nowQtyList[0]);
			int newNo = nowNo + 1;
			newQty = newNo.ToString() + "," + nowQtyList[1] + "," + nowQtyList[2];

		} else if (itemId == 2) {
			//Cyu
			int nowNo = int.Parse(nowQtyList[1]);
			int newNo = nowNo + 1;
			newQty = nowQtyList[0] + "," + newNo.ToString()  + "," + nowQtyList[2];

		} else if (itemId == 3) {
			//Jyo
			int nowNo = int.Parse(nowQtyList[2]);
			int newNo = nowNo + 1;
			newQty = nowQtyList[0] + "," + nowQtyList[1]  + "," + newNo.ToString();
		}

		PlayerPrefs.SetString (itemCd, newQty);

		PlayerPrefs.Flush ();
		
	}
}
