using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GaikouKuniSelect : MonoBehaviour {

	public int targetKuniId = 0;
	public int targetDaimyoId = 0;
	public int doumeiDaimyoId = 0;
	public string kuniName = "";
	public GameObject Content;
	public GameObject Btn;

	//Ratio Calc
	public int myYukoudo = 0;
	public int chiryaku = 0;
	public int kuniDiff = 0;
	public int theirYukoudo = 0;

	//Syuppei
	public int srcKuniId = 0;
	public int srcDaimyoId = 0;
	public string srcDaimyoName = "";
	public string targetDaimyoName = "";

	// Use this for initialization
	public void OnClick () {

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [2].Play ();

		//Change Color
		Color Select = new Color (76f / 255f, 50f / 255f, 18f / 255f, 80f / 255f);
		Color unSelect = new Color (255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);


		foreach (Transform obj in Content.transform) {
			obj.GetComponent<Image>().color = unSelect;
		}
		GetComponent<Image> ().color = Select;

		//Seikoudo
		//成功確率 = (((友好度/20) * 知略/50)*国力差)*((100-他大名間友好度)/100)
		if (Btn.name == "DoKyoutouBtn") {
			float tempResult = (myYukoudo / 20 * chiryaku / 5) * kuniDiff;
			float tempCalc3 = (100 - theirYukoudo);
			float tempCalc4 = tempCalc3 / 100;
			float ratio = (tempResult * tempCalc4);

			int ratioInt = 0;
			if (ratio > 80) {
				ratioInt = 80;
			} else if(ratio < 0){
                ratioInt = 0;
            }else {
                ratioInt = (int)ratio;
            }


            GameObject.Find ("KyoutouRatio").transform.Find ("Value").GetComponent<Text> ().text = ratioInt.ToString () + "%";
			GameObject.Find ("DoKyoutouBtn").GetComponent<DoGaikou> ().kyoutouRatio = ratioInt;
			GameObject.Find ("DoKyoutouBtn").GetComponent<DoGaikou> ().myYukoudo = myYukoudo;
			GameObject.Find ("DoKyoutouBtn").GetComponent<DoGaikou> ().kuniName = kuniName;
			GameObject.Find ("DoKyoutouBtn").GetComponent<DoGaikou> ().targetKuniId = targetKuniId;
		
		} else if (Btn.name == "DoSyuppeiBtn") {
			//Ratio = (((My Yukoudo/20) *DFC/50)*Diff)*((100-their Yukoudo)/100)
			float tempResult = (myYukoudo / 20 * chiryaku / 5) * kuniDiff;
			float tempCalc3 = (100 - theirYukoudo);
			float tempCalc4 = tempCalc3 / 100;
			float ratio = (tempResult * tempCalc4);

            int ratioInt = 0;
			if (ratio > 80) {
				ratioInt = 80;
			}else if (ratio < 0) {
                ratioInt = 0;
            }else {
				ratioInt = (int)ratio;
			}

			GameObject.Find ("KyoutouRatio").transform.Find ("Value").GetComponent<Text> ().text = ratioInt.ToString () + "%";
			DoGaikou DoSyuppeiBtnScript = GameObject.Find ("DoSyuppeiBtn").GetComponent<DoGaikou>();
			DoSyuppeiBtnScript.kyoutouRatio = ratioInt;
			DoSyuppeiBtnScript.myYukoudo = myYukoudo;
			DoSyuppeiBtnScript.kuniName = kuniName;
			DoSyuppeiBtnScript.targetKuniId = targetKuniId;
			DoSyuppeiBtnScript.srcKuniId = srcKuniId;
			DoSyuppeiBtnScript.srcDaimyoId = srcDaimyoId;
			DoSyuppeiBtnScript.srcDaimyoName = srcDaimyoName;
			DoSyuppeiBtnScript.targetDaimyoId = targetDaimyoId;
			DoSyuppeiBtnScript.targetDaimyoName = targetDaimyoName;


		}
		






	}	
}
