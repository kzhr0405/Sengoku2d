using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

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

		if (avlNaiseiList.Contains ("tp") && avlNaiseiList.Contains ("kb") && avlNaiseiList.Contains ("snb")) {
			Message msg = new Message();
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                msg.makeMessage("You have already transferred technology of Gun, Hourse, Ninja.");
            }else {
                msg.makeMessage("この国に鉄砲、騎馬、忍技術は伝達済みです。");
            }
		
		} else {
		
			BusyoStatusButton pop = new BusyoStatusButton ();
			pop.commonPopup (26);
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                GameObject.Find ("popText").GetComponent<Text> ().text = "Tech Transfer";
            }else {
                GameObject.Find("popText").GetComponent<Text>().text = "技術伝達";
            }






		}




	}
}
