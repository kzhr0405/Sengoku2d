using UnityEngine;
using System.Collections;

public class PopupController : MonoBehaviour {
	public GameObject TargetPopup;

	public void ClickIcon (int stageNo) {
		//引数を基に対象のpopupを移動する
		string target = "popup" + stageNo;
		TargetPopup = GameObject.Find(target);
		TargetPopup.transform.position = new Vector3 (0,0,0);

	}
}
