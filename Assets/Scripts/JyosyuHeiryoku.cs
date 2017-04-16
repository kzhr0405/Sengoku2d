using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class JyosyuHeiryoku : MonoBehaviour {


	public int GetJyosyuHeiryoku (string busyoId) {
		string path = "jyosyuHei" + busyoId;
		int addHei = PlayerPrefs.GetInt (path);

		return addHei;
	}	
}
