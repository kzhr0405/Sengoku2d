using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class MyDaimyoWasAttacked : MonoBehaviour {

	public void wasAttacked(string key, int srcKuni, int dstKuni, int srcDaimyoId, int dstDaimyoId, bool dstEngunFlg, string dstEngunDaimyoId, string dstEngunSts){

		//In the case of My Damyo was Attacked

		//For Dramatic Enemy Creation
		GameObject kuniView = GameObject.Find("KuniIconView");
        int senarioId = PlayerPrefs.GetInt("senarioId");
        if (kuniView.transform.Find (srcKuni.ToString ())) {
			SendParam param = kuniView.transform.Find (srcKuni.ToString ()).GetComponent<SendParam> ();
            Gaikou gaikou = new Gaikou();
            int busyoQty = param.busyoQty;
			int busyoLv = param.busyoLv;
			int butaiQty = param.butaiQty;
			int butaiLv = param.butaiLv;

			//Dummy
			PlayerPrefs.SetInt ("activeStageId", 0);
			PlayerPrefs.SetInt ("activeStageMoney", busyoQty  * 500);
			PlayerPrefs.SetInt ("activeStageExp", busyoQty  * 100);
			PlayerPrefs.SetString ("activeItemType", "");
			PlayerPrefs.SetInt ("activeItemId", 0);
			PlayerPrefs.SetFloat ("activeItemRatio", 0);
			PlayerPrefs.SetInt ("activeItemQty", 0);

            //Actual
            int langId = PlayerPrefs.GetInt("langId");
            PlayerPrefs.SetInt ("activeKuniId", dstKuni);
			KuniInfo kuni = new KuniInfo ();
			string kuniName = kuni.getKuniName (dstKuni,langId);
            string kassenName = "";
            if (langId == 2) {
                kassenName = kuniName + " Defence";
            }else {
                 kassenName = kuniName + "防衛";
            }
			PlayerPrefs.SetString ("activeStageName", kassenName);

			PlayerPrefs.SetInt ("activeDaimyoId", srcDaimyoId);
			PlayerPrefs.SetInt ("activeBusyoQty", busyoQty);
			PlayerPrefs.SetInt ("activeBusyoLv", busyoLv);
			PlayerPrefs.SetInt ("activeButaiQty", butaiQty);
			PlayerPrefs.SetInt ("activeButaiLv", butaiLv);

			//Passive only
			PlayerPrefs.SetBool ("isAttackedFlg", true);
			PlayerPrefs.DeleteKey ("isKessenFlg");
			PlayerPrefs.SetString ("activeKey", key);
			PlayerPrefs.SetInt ("activeSrcDaimyoId", srcDaimyoId);
			PlayerPrefs.SetInt ("activeDstDaimyoId", dstDaimyoId);

			//Engun
			if (dstEngunFlg) {
				PlayerPrefs.SetString ("playerEngunList", dstEngunSts);
				PlayerPrefs.DeleteKey ("enemyEngunList");

			} else {
				PlayerPrefs.DeleteKey ("playerEngunList");
				PlayerPrefs.DeleteKey ("enemyEngunList");
			}

            //Enemy Rengou
            bool rengouFlg = PlayerPrefs.GetBool("rengouFlg");
            if(rengouFlg) {
                string rengouDaimyo = PlayerPrefs.GetString("rengouDaimyo");
                char[] delimiterChars = { ',' };
                List<string> rengouDaimyoList = new List<string>();
                rengouDaimyoList = new List<string>(rengouDaimyo.Split(delimiterChars));

                //Target Kuni List Prep.
                List<int> baseKuni = new List<int>();
                string seiryoku = PlayerPrefs.GetString("seiryoku");
                List<string> seiryokuList = new List<string>();
                seiryokuList = new List<string>(seiryoku.Split(delimiterChars));

                baseKuni.AddRange(kuni.getMappingKuni(dstKuni));
                for(int i=0; i<seiryokuList.Count; i++) {
                    string daimyoId = seiryokuList[i];
                    if (int.Parse(daimyoId) == srcDaimyoId) {
                        int kuniId = i + 1;
                        baseKuni.AddRange(kuni.getMappingKuni(kuniId));
                    }
                }
                List<int> engunDaimyoList = new List<int>();
                for (int i = 0; i < baseKuni.Count; i++) {
                    int tmpDaimyoId = int.Parse(seiryokuList[baseKuni[i] - 1]);
                    if(tmpDaimyoId != srcDaimyoId) {
                        if (!engunDaimyoList.Contains(tmpDaimyoId)) {
                            if(rengouDaimyoList.Contains(tmpDaimyoId.ToString())) {
                                engunDaimyoList.Add(tmpDaimyoId);
                            }
                        }
                    }
                }
                if (engunDaimyoList.Count != 0) {
                    Doumei doumei = new Doumei();
                    for (int k = 0; k < engunDaimyoList.Count; k++) {
                        string engunDaimyo = engunDaimyoList[k].ToString();
                        int yukoudo = gaikou.getExistGaikouValue(int.Parse(engunDaimyo), srcDaimyoId);

                        //mydaimyo doumei check
                        bool myDoumeiFlg = false;
                        myDoumeiFlg = doumei.myDoumeiExistCheck(int.Parse(engunDaimyo));
                        if (myDoumeiFlg) {
                            yukoudo = yukoudo / 2;
                        }

                        //engun check
                        MainEventHandler main = new MainEventHandler();
                        bool engunFlg = main.CheckByProbability(yukoudo);                        
                        if (engunFlg) {
                            //rengou check
                            bool hardFlg = PlayerPrefs.GetBool("hardFlg");
                            int count = 1;//engun busyo count
                            if (rengouFlg && rengouDaimyoList.Contains(engunDaimyo)) {
                                if(hardFlg) {
                                    count = UnityEngine.Random.Range(4, 7);//3-6
                                }else {
                                    count = UnityEngine.Random.Range(2, 5);//2-4
                                }
                            }

                            for (int i = 0; i < count; i++) {
                                //Engun OK
                                engunFlg = true;
                                if (dstEngunDaimyoId != null && dstEngunDaimyoId != "") {
                                    dstEngunDaimyoId = dstEngunDaimyoId + ":" + engunDaimyo;
                                    string tempEngunSts = main.getSomeEngunSts(engunDaimyo, dstEngunSts, seiryokuList,senarioId);
                                    dstEngunSts = dstEngunSts + ":" + engunDaimyo + "-" + tempEngunSts;
                                }
                                else {
                                    dstEngunDaimyoId = engunDaimyo;
                                    string tempEngunSts = main.getSomeEngunSts(engunDaimyo, dstEngunSts, seiryokuList,senarioId);
                                    dstEngunSts = engunDaimyo + "-" + tempEngunSts;

                                }
                            }
                        }
                    }
                    PlayerPrefs.SetString("enemyEngunList", dstEngunSts);
                }


            }

            //Gaikou Down
			gaikou.downGaikouByAttack (srcDaimyoId, dstDaimyoId);

			//Delete "Start Kassen Flg"
			PlayerPrefs.DeleteKey ("activeLink");
			PlayerPrefs.DeleteKey ("activePowerType");

			List<int> powerTypeList = new List<int> (){ 1, 2, 3 };
			int random = UnityEngine.Random.Range (1, powerTypeList.Count + 1);
			PlayerPrefs.SetInt ("activePowerType", random);


			//Boubi effect
			string boubiTmp = "boubi" + dstKuni.ToString ();
			int boubi = PlayerPrefs.GetInt (boubiTmp, 0);
			boubi = boubi / 10;
			PlayerPrefs.SetInt ("activeBoubi", boubi);



			PlayerPrefs.Flush ();
			Application.LoadLevel ("preKassen");

		}
	}



}
