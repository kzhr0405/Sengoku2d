using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class ZukanScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ZukanMenu zkn = new ZukanMenu ();
		GameObject Content = GameObject.Find ("Content1");
		zkn.changeTabColor("Busyo");
        int senarioId = PlayerPrefs.GetInt("senarioId");
        zkn.showBusyoZukan (Content,senarioId);
	}
}
