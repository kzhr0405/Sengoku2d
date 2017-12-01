using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class BakuhuMenuReturn : MonoBehaviour {

	//Param
	public bool myKuniQtyIsBiggestFlg = false;
	public GameObject board;
	public GameObject deleteObj;
	public GameObject scrollView;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [1].Play ();

		Destroy (deleteObj);
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            board.transform.Find("popText").GetComponent<Text>().text = "Shogunate";
        } else {
            board.transform.Find("popText").GetComponent<Text>().text = "幕府";
        }
		scrollView.SetActive (true);

		BakuhuInfo bakuhu = new BakuhuInfo ();
		GameObject contentObj = scrollView.transform.Find ("Content").gameObject;
		bakuhu.updateAtkOrderBtnStatus (contentObj);



		Destroy (gameObject);
	}


}
