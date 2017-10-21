using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;


public class Doumei : MonoBehaviour {


	public bool myDoumeiExistCheck(int targetDaimyoId){
		bool doumeiExistFlg = false;
		string doumeiString = PlayerPrefs.GetString ("doumei");
		List<string> doumeiDaimyoList = new List<string> ();
		char[] delimiterChars = {','};

		if (doumeiString != null && doumeiString != "") {
			if (doumeiString.Contains (",")) {
				doumeiDaimyoList = new List<string> (doumeiString.Split (delimiterChars));
			} else {
				doumeiDaimyoList.Add (doumeiString);
			}
		}

		if (doumeiDaimyoList.Count != 0) {
			if (doumeiDaimyoList.Contains (targetDaimyoId.ToString ())) {
				doumeiExistFlg = true;
			}
		}

		return doumeiExistFlg;
	}



	public List<string> doumeiExistCheck(int daimyoId, string eDaimyo){
		List<string> doumeiDaimyoList = new List<string> ();
		char[] delimiterChars = {','};
		int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
		string doumeiString = "";

		if (daimyoId == myDaimyo) {
			if(PlayerPrefs.HasKey("doumei")){
				doumeiString = PlayerPrefs.GetString ("doumei");
			}
		} else {
			string temp = "doumei" + daimyoId.ToString();
			if(PlayerPrefs.HasKey(temp)){
				doumeiString = PlayerPrefs.GetString (temp);
			}
		}

		if (doumeiString != null && doumeiString != "") {
			if (doumeiString.Contains (",")) {
				doumeiDaimyoList = new List<string> (doumeiString.Split (delimiterChars));
			} else {
				doumeiDaimyoList.Add (doumeiString);
			}
		}

		//If Attacking Daimyo = Doumei Daimyo
		doumeiDaimyoList.Remove (eDaimyo);

		return doumeiDaimyoList;
	}

	//Check whether can attacked Kuni trace to doumei Diamyo
	public List<string> traceNeighborDaimyo(int attackedKuniId, int attackedDaimyoId, List<string> doumeiDaimyoList, List<string>seiryokuList, List<string>checkedList, List<string> okDaimyoList){

		List<int> nKuniList = new List<int>();
		KuniInfo kuni = new KuniInfo ();
		nKuniList = kuni.getMappingKuni(attackedKuniId);

		for (int i=0; i<nKuniList.Count; i++) {
			string tempDaimyoId = seiryokuList[nKuniList[i]-1];

			if(!checkedList.Contains(nKuniList[i].ToString())){
				checkedList.Add(nKuniList[i].ToString());
				if(doumeiDaimyoList.Contains(tempDaimyoId)){
					//found
					if(!okDaimyoList.Contains(tempDaimyoId)){
						//no duplication
						okDaimyoList.Add(tempDaimyoId);
					}
				}else{
					//not found
					if(int.Parse(tempDaimyoId) == attackedDaimyoId){
						okDaimyoList = traceNeighborDaimyo(nKuniList[i], attackedDaimyoId, doumeiDaimyoList, seiryokuList, checkedList,okDaimyoList);
						
					}
				}
			}
		}
		return okDaimyoList;
	}

	public void deleteDoumei(string daimyo1, string daimyo2){

		int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");

		if (daimyo1 == myDaimyo.ToString()) {
			deleteDoumeiMyDaimyo(myDaimyo.ToString(), daimyo2);

		} else if (daimyo2 == myDaimyo.ToString()) {
			deleteDoumeiMyDaimyo(myDaimyo.ToString(), daimyo1);

		} else {
			//enemy attacked enemy
			deleteDoumeiEnemyDaimyo(daimyo1, daimyo2);
			deleteDoumeiEnemyDaimyo(daimyo2, daimyo1);
		}
	
	}

	public void deleteDoumeiMyDaimyo(string myDaimyo, string otherDaimyo){

		char[] delimiterChars = {','};

		//My Dimyo Handling
		string doumei1 = PlayerPrefs.GetString ("doumei");
		List<string> doumeiList1 = new List<string>();
		if(doumei1.Contains(",")){
			doumeiList1 = new List<string> (doumei1.Split (delimiterChars));
		}else{
			doumeiList1.Add(doumei1);
		}
		doumeiList1.Remove(otherDaimyo);

		string newDoumei1 = "";
		for(int i=0; i<doumeiList1.Count; i++){
			if(i==0){
				newDoumei1 = doumeiList1[i];
			}else{
				newDoumei1 = newDoumei1 + "," + doumeiList1[i];
			}
		}
		
		//Other Daimyo Handling
		string temp2 = "doumei" + otherDaimyo;
		string doumei2 = PlayerPrefs.GetString (temp2);
		List<string> doumeiList2 = new List<string>();
        Debug.Log(doumei2);
        if(doumei2 != "") {
		    if(doumei2.Contains(",")){
			    doumeiList2 = new List<string> (doumei2.Split (delimiterChars));
		    }else{
			    doumeiList2.Add(doumei2);
		    }
		    doumeiList2.Remove(myDaimyo);
		    string newDoumei2 = "";
		
		    for(int i=0; i<doumeiList2.Count; i++){
			    if(i==0){
				    newDoumei2 = doumeiList2[i];
			    }else{
				    newDoumei2 = newDoumei2 + "," + doumeiList2[i];
			    }
		    }

		    PlayerPrefs.SetString ("doumei",newDoumei1);
		    PlayerPrefs.SetString (temp2,newDoumei2);
		    PlayerPrefs.Flush ();
        }
    }

	public void deleteDoumeiEnemyDaimyo(string daimyo1, string daimyo2){

		char[] delimiterChars = {','};
		string temp = "doumei" + daimyo1;
		string doumei = PlayerPrefs.GetString (temp);
		List<string> doumeiList = new List<string>();
		if(doumei.Contains(",")){
			doumeiList = new List<string> (doumei.Split (delimiterChars));
		}else{
			doumeiList.Add(doumei);
		}
		doumeiList.Remove(daimyo2);
		string newDoumei = "";
		
		for(int i=0; i<doumeiList.Count; i++){
			if(i==0){
				newDoumei = doumeiList[i];
			}else{
				newDoumei = newDoumei + "," + doumeiList[i];
			}
		}

		PlayerPrefs.SetString (temp,newDoumei);
		PlayerPrefs.Flush ();
		
	}

}
