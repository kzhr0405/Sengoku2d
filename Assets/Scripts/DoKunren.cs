using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Reflection;
using System.Collections.Generic;
using System;

public class DoKunren : MonoBehaviour {
	public bool moneyOK = true;
	Message msg = new Message(); 

	public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (moneyOK != true) {
			//Error
			audioSources [4].Play ();
			msg.makeMessage(msg.getMessage(6));


		}else{
			//OK
			audioSources [3].Play ();
			Slider lvSlider = GameObject.Find ("KunrenSlider").GetComponent<Slider>(); 
			int targetLv = (int)lvSlider.value;
			string payMoney = GameObject.Find ("RequiredMoneyValue").GetComponent<Text>().text;
			string busyoId = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyo;


			//reduce money
			int nowMoney = PlayerPrefs.GetInt ("money");
			int calcMoney = nowMoney - int.Parse(payMoney); 

			//increase target Lv
			string tmp = "hei" + busyoId;
			string chParam = PlayerPrefs.GetString (tmp,"0");
            StatusGet statusScript = new StatusGet();
            string chParamHeisyu = statusScript.getHeisyu(int.Parse(busyoId));
            if (chParam == "0" || chParam == "") {                
                chParam = chParamHeisyu + ":1:1:1";
                PlayerPrefs.SetString(tmp, chParam);
                PlayerPrefs.Flush();
            }

            char[] delimiterChars = {':'};
			string[] ch_list = chParam.Split (delimiterChars);

			//get pure status
			Entity_lvch_mst lvMst  = Resources.Load ("Data/lvch_mst") as Entity_lvch_mst;
			int startline = 0;
            string ch_type = chParamHeisyu;

			if(ch_type=="KB"){
				startline = 0;
			}else if(ch_type=="YR"){
				startline = 1;
			}else if(ch_type=="TP"){
				startline = 2;
			}else if(ch_type=="YM"){
				startline = 3;
			}

			object stslst = lvMst.param[startline];
			Type t = stslst.GetType();
			String param = "lv" + targetLv.ToString();
			FieldInfo f = t.GetField(param);
			int sts = (int)f.GetValue(stslst);

			string newParam = ch_list [0] + ":" + ch_list [1] + ":" + targetLv.ToString() + ":" + sts.ToString();

			PlayerPrefs.SetInt ("money",calcMoney);
			PlayerPrefs.SetString (tmp,newParam);
			PlayerPrefs.SetBool ("questDailyFlg23",true);

			PlayerPrefs.Flush();


			//Message
			string busyoName = GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyoName;
            string OKtext = "";
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                OKtext =busyoName+" trained their soldiers.";
            }else {
                OKtext = busyoName + "隊にて訓練を実施しました。";
            }
			msg.makeMessage(OKtext);
			
			//Reload
			//Close Board
			GameObject.Find ("close").GetComponent<CloseBoard>().onClick();
			RonkouScene ronkou = new RonkouScene();
			ronkou.createBusyoStatusView(busyoId);

		}
	}	
}
