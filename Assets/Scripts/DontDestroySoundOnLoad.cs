using UnityEngine;
using System.Collections;

public class DontDestroySoundOnLoad : MonoBehaviour {
	public bool DestoryFlg = false;

	// Use this for initialization
	void Start () {
		
		if (!DestoryFlg) {
			DontDestroyOnLoad (this);
		}
	}
}
