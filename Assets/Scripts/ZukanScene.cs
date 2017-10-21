using UnityEngine;
using System.Collections;

public class ZukanScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ZukanMenu zkn = new ZukanMenu ();
		GameObject Content = GameObject.Find ("Content1");
		zkn.changeTabColor("Busyo");
		zkn.showBusyoZukan (Content);
	}
}
