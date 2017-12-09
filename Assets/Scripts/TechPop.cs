using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class TechPop : MonoBehaviour {

	public void OnClick(){

		List<string> avlNaiseiList = new List<string> ();
		char[] delimiterChars = {':'};
		NaiseiController script = GameObject.Find ("NaiseiController").GetComponent<NaiseiController> ();
		string shigen = script.shigen;


		if(shigen != "null"){
			if(shigen.Contains(":")){
				avlNaiseiList = new List<string> (shigen.Split (delimiterChars));
			}else{
				avlNaiseiList.Add(shigen);
			}
		}
        Message msg = new Message();
        int langId = PlayerPrefs.GetInt("langId");
        if (avlNaiseiList.Contains ("tp") && avlNaiseiList.Contains ("kb") && avlNaiseiList.Contains ("snb")) {			            
            msg.makeMessage(msg.getMessage(283,langId));		
		} else {		
			BusyoStatusButton pop = new BusyoStatusButton ();
			pop.commonPopup (26);            
            msg.makeMessage(msg.getMessage(284, langId));            
        }        
	}
}
