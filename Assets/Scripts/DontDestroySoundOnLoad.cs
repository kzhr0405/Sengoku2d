using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DontDestroySoundOnLoad : MonoBehaviour {
	public bool DestoryFlg = false;

	// Use this for initialization
	void Start () {		
		if (!DestoryFlg) {
			DontDestroyOnLoad (this);
		}
    }

	//アプリケーションクラッシュ対策用
	void OnApplicationQuit() {
		//強制保存
		PlayerPrefs.Flush();
    }
}