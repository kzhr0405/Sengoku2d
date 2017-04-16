using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class MyDaimyoWasAttacked : MonoBehaviour {

	public void wasAttacked(string key, int srcKuni, int dstKuni, int srcDaimyoId, int dstDaimyoId, bool dstEngunFlg, string dstEngunDaimyoId, string dstEngunSts){

		//In the case of My Damyo was Attacked

		//For Dramatic Enemy Creation
		GameObject kuniView = GameObject.Find("KuniIconView");
		if (kuniView.transform.FindChild (srcKuni.ToString ())) {
			SendParam param = kuniView.transform.FindChild (srcKuni.ToString ()).GetComponent<SendParam> ();
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
			PlayerPrefs.SetInt ("activeKuniId", dstKuni);
			KuniInfo kuni = new KuniInfo ();
			string kuniName = kuni.getKuniName (dstKuni);
            string kassenName = "";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
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

			//Gaikou Down
			Gaikou gaikou = new Gaikou ();
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
