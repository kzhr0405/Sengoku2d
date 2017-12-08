using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class DoKessen : MonoBehaviour {

	public int daimyoId = 0;
	public string daimyoName = "";
	public GameObject confirm;
	public GameObject back;
	public int kuniId = 0;
	public int needHyourouNo = 0;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (name == "YesButton") {
			audioSources [0].Play ();

			GameObject kuniView = GameObject.Find("KuniIconView");
			SendParam param = kuniView.transform.Find(kuniId.ToString()).GetComponent<SendParam>();

            //adjust
            int busyoQty = param.busyoQty;
            int jinkeiBusyoQty = PlayerPrefs.GetInt("jinkeiBusyoQty");
            if (busyoQty< jinkeiBusyoQty) {
                busyoQty = jinkeiBusyoQty;
            }
            
			int busyoLv = param.busyoLv;

            //adjsut
            int butaiQty = param.butaiQty;
            int jinkeiAveChQty = PlayerPrefs.GetInt("jinkeiAveChQty");
            if (butaiQty < jinkeiAveChQty) {
                butaiQty = jinkeiAveChQty;
            }
            int butaiLv = param.butaiLv;

			//Dummy
			PlayerPrefs.SetInt("activeStageId", 0);
            int activeStageMoney = busyoQty * busyoLv * 100;
            int activeStageExp = busyoQty * busyoLv * 10;
            if (activeStageMoney > 20000) activeStageMoney = 20000;
            if (activeStageExp > 2000) activeStageExp = 2000;

            PlayerPrefs.SetInt("activeStageMoney", activeStageMoney);
			PlayerPrefs.SetInt("activeStageExp", activeStageExp);
			PlayerPrefs.SetString("activeItemType", "");
			PlayerPrefs.SetInt("activeItemId", 0);
			PlayerPrefs.SetFloat("activeItemRatio", 0);
			PlayerPrefs.SetInt("activeItemQty", 0);
			PlayerPrefs.SetString("activeItemGrp", "no");

			//Actual
			KuniInfo kuni = new KuniInfo ();
            string kassenName = "";
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                kassenName = " Final Battle with " + daimyoName;
            }else {
                kassenName = daimyoName + "決戦";
            }
			PlayerPrefs.SetString("activeStageName", kassenName);

			PlayerPrefs.SetInt("activeDaimyoId", daimyoId);
			PlayerPrefs.SetInt ("activeBusyoQty", busyoQty);
			PlayerPrefs.SetInt ("activeBusyoLv", busyoLv);
			PlayerPrefs.SetInt ("activeButaiQty", butaiQty);
			PlayerPrefs.SetInt ("activeButaiLv", butaiLv);


			//Flag
			PlayerPrefs.DeleteKey ("isAttackedFlg");
			PlayerPrefs.SetBool ("isKessenFlg",true);
			PlayerPrefs.SetInt ("kessenHyourou",needHyourouNo);

			//Player Doumei Flg
			PlayerPrefs.DeleteKey("playerEngunList");

			//Enemy Doumei Handling
			PlayerPrefs.DeleteKey("enemyEngunList");
			string doumeiTemp = "doumei" + daimyoId;
			string enemyDoumeiString = PlayerPrefs.GetString (doumeiTemp);
			char[] delimiterChars = {','};
			List<string> doumeiList = new List<string>();
			if(enemyDoumeiString != null && enemyDoumeiString !=""){
				if(enemyDoumeiString.Contains(",")){
					doumeiList = new List<string> (enemyDoumeiString.Split (delimiterChars));
				}else{
					doumeiList.Add(enemyDoumeiString);
				}
			}
			string seiryoku = PlayerPrefs.GetString ("seiryoku");
			List<string> seiryokuList = new List<string> ();
			seiryokuList = new List<string> (seiryoku.Split (delimiterChars));

			Doumei doumei = new Doumei();
			Gaikou gaikou = new Gaikou ();
			List<string> okDaimyoList = new List<string> ();
			List<string> checkedList = new List<string> ();
			string dstEngunDaimyoId = "";
			string dstEngunSts = ""; //BusyoId-BusyoLv-ButaiQty-ButaiLv:
            int senarioId = PlayerPrefs.GetInt("senarioId");
            okDaimyoList = doumei.traceNeighborDaimyo(kuniId, daimyoId, doumeiList, seiryokuList, checkedList, okDaimyoList);
			if (okDaimyoList.Count != 0) {
				for (int k = 0; k < okDaimyoList.Count; k++) {
					string engunDaimyo = okDaimyoList [k];
					int yukoudo = gaikou.getExistGaikouValue (int.Parse (engunDaimyo), daimyoId);

					//engun check
					MainEventHandler main = new MainEventHandler ();
					bool dstEngunFlg = main.CheckByProbability (yukoudo);
					if (dstEngunFlg) {
						//Engun OK
						dstEngunFlg = true;
						if (dstEngunDaimyoId != null && dstEngunDaimyoId != "") {
							dstEngunDaimyoId = dstEngunDaimyoId + ":" + engunDaimyo;
							string tempEngunSts = main.getEngunSts (engunDaimyo, senarioId);
							dstEngunSts = dstEngunSts + ":" + engunDaimyo + "-" + tempEngunSts;

						} else {
							dstEngunDaimyoId = engunDaimyo;
							string tempEngunSts = main.getEngunSts (engunDaimyo,senarioId);
							dstEngunSts = engunDaimyo + "-" + tempEngunSts;

						}
					}
				}
				PlayerPrefs.SetString ("enemyEngunList", dstEngunSts);

			}

			//Delete "Start Kassen Flg"
			PlayerPrefs.DeleteKey("activeLink");
			PlayerPrefs.SetInt("activePowerType",3);
            PlayerPrefs.SetBool("lastOneFlg", true);

            PlayerPrefs.Flush();
			Application.LoadLevel("preKassen");



		} else {
			audioSources [1].Play ();
			Destroy (confirm.gameObject);
			Destroy (back.gameObject);

		}
	}
}
