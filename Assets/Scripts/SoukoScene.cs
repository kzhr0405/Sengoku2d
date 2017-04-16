using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class SoukoScene : MonoBehaviour {

	// Use this for initialization

	public string currentTab = "kahouScenes";

	void Start () {

		//Money
		int money = PlayerPrefs.GetInt ("money");
		GameObject.Find ("MoneyValue").GetComponent<Text> ().text = money.ToString();

		GameObject.Find ("Kahou").GetComponent<KahouSoukoScene> ().OnClick ();

	}
}
