using UnityEngine;
using System.Collections;

public class GameSpeed : MonoBehaviour {

	public bool speed_ON;

	// Use this for initialization
	void Start () {
		if (speed_ON) {
			QualitySettings.vSyncCount = 1;
			Application.targetFrameRate = 60;
		} else {
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = 30;
		}

	}
}