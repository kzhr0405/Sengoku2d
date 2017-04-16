using UnityEngine;
using System.Collections;

public class SyoukaijyoSelect : MonoBehaviour {

	public void OnClick(){
		if (GameObject.Find ("PassButton").GetComponent<CyouteiPop> ()) {
			GameObject.Find ("PassButton").GetComponent<CyouteiPop> ().syoukaijyoRank = name;
		} else {
			GameObject.Find ("PassButton").GetComponent<SyouninPop> ().syoukaijyoRank = name;
		}
	}
}
