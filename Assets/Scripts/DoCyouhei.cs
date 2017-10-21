using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class DoCyouhei : MonoBehaviour {

	public bool itemOK = true;
	public bool moneyOK = true;
	public int nowItem;
	public int requiredItem;
	public string itemType;
	public string ch_type;

	public int nowMoney;
	public int requiredMoney;

	public void OnClick () {
		Message msg = new Message(); 
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (itemOK == false || moneyOK == false) {
			//Error
			audioSources [4].Play ();
			msg.makeMessage(msg.getMessage(52));

		} else {
			audioSources [3].Play ();

			/*Do Cyouhei*/
			//Reduce Current Item
			string itemColumn = "cyouhei" + ch_type;
			string itemString = PlayerPrefs.GetString (itemColumn);
			char[] delimiterChars = {','};
			string[] itemList = itemString.Split (delimiterChars);

			string newItemListString = "0,0,0";
			if(itemType == "low"){
				int calc = int.Parse(itemList[0]) - requiredItem;
				newItemListString = calc.ToString() + "," + itemList[1] + "," + itemList[2];

			}else if(itemType == "middle"){
				int calc = int.Parse(itemList[1]) - requiredItem;
				newItemListString = itemList[0] + "," + calc.ToString() + "," + itemList[2];

			}else if(itemType == "hight"){
				int calc = int.Parse(itemList[2]) - requiredItem;
				newItemListString = itemList[0] + "," + itemList[1] + "," + calc.ToString();

			}
			PlayerPrefs.SetString (itemColumn,newItemListString);

			//Reduce Money
			int calcMoney = nowMoney - requiredMoney; 
			PlayerPrefs.SetInt ("money",calcMoney);

			//Add Child QTY
			string busyoId = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyo;
			string temp = "hei" + busyoId;
			string childStsString = PlayerPrefs.GetString (temp,"0");
            if (childStsString == "0" || childStsString == "") {
                StatusGet statusScript = new StatusGet();
                string chParamHeisyu = statusScript.getHeisyu(int.Parse(busyoId));
                childStsString = chParamHeisyu + ":1:1:1";
                PlayerPrefs.SetString(temp, childStsString);
                PlayerPrefs.Flush();
            }

            char[] delimiterChars2 = {':'};
			string[] childStsList = childStsString.Split (delimiterChars2);
		
			int bfrChildQty = int.Parse(childStsList[1]);
			int aftChildQty = bfrChildQty + 1;

            string newChildStsString = childStsList[0] + ":" + aftChildQty.ToString() + ":" + childStsList[2] + ":" + childStsList[3];
            bool tutorialDoneFlg = PlayerPrefs.GetBool("tutorialDoneFlg");
            if (!tutorialDoneFlg || Application.loadedLevelName != "tutorialBusyo") {
                PlayerPrefs.SetString (temp,newChildStsString);
            }

			PlayerPrefs.SetBool ("questDailyFlg22",true);
			PlayerPrefs.Flush();


			//Message
			string busyoName = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyoName;
            string OKtext = "";
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                OKtext =busyoName+" recruited new soldiers.";
            }else {
                OKtext = busyoName + "隊にて徴兵を実施しました。";
            }
			msg.makeMessage(OKtext);

            //Reload
            //Close Board
            if (Application.loadedLevelName != "tutorialBusyo") {
                GameObject.Find ("close").GetComponent<CloseBoard>().onClick();
			    RonkouScene ronkou = new RonkouScene();
			    ronkou.createBusyoStatusView(busyoId);
            }else {
                Destroy(GameObject.Find("Back(Clone)").gameObject);
                Destroy(GameObject.Find("board(Clone)").gameObject);
                GameObject ChildQtyValue = GameObject.Find("ChildQtyValue").gameObject;
                ChildQtyValue.GetComponent<Text>().text = "2";
                //Set Parametor
                PlayerPrefs.SetInt("tutorialId", 11);
                PlayerPrefs.Flush();
                TutorialController tutorialScript = new TutorialController();
                Vector2 vect = new Vector2(50, 200);
                GameObject anim = tutorialScript.SetFadeoutPointer(ChildQtyValue, vect);
                anim.transform.localScale = new Vector2(500, 500);

                Color enabledColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
                GameObject btn = GameObject.Find("ButtonKunren").gameObject;
                btn.GetComponent<Button>().enabled = true;
                btn.GetComponent<Image>().color = enabledColor;
                btn.transform.FindChild("Text").GetComponent<Text>().color = enabledColor;

                GameObject ButaiStatus = GameObject.Find("ButaiStatus").gameObject;
                GameObject BusyoStatus = GameObject.Find("BusyoStatus").gameObject;
                ButaiStatus.transform.SetParent(BusyoStatus.transform);
                
                GameObject Button = GameObject.Find("Panel").transform.FindChild("Button").gameObject;
                GameObject tBack = GameObject.Find("tBack").gameObject;
                Button.transform.SetParent(tBack.transform);
                Vector2 vect2 = new Vector2(0, 50);
                GameObject animPoint = tutorialScript.SetPointer(Button, vect2);
                animPoint.transform.localScale = new Vector2(200, 200);
            }

        }
	}	
}
