using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DontDestroySoundOnLoad : MonoBehaviour {
	public bool DestoryFlg = false;

	// Use this for initialization
	void Start () {
		
		if (!DestoryFlg) {
			DontDestroyOnLoad (this);
<<<<<<< HEAD
		}

    }

    //アプリケーションクラッシュ対策用
    void OnApplicationQuit() {
        //強制保存
        PlayerPrefs.Flush();
    }    
=======
		}
	}

	//アプリケーションクラッシュ対策用
	void OnApplicationQuit() {
		//強制保存
		PlayerPrefs.Flush();
    }
>>>>>>> c83f2d30f6644cc7d70669053dcd8180e994fb9a
}
