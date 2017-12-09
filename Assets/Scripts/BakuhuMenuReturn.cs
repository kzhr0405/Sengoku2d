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
        Message Message = new Message();
        int langId = PlayerPrefs.GetInt("langId");
        Destroy (deleteObj);
        board.transform.Find("popText").GetComponent<Text>().text = Message.getMessage(203,langId);
        
		scrollView.SetActive (true);

		BakuhuInfo bakuhu = new BakuhuInfo ();
		GameObject contentObj = scrollView.transform.Find ("Content").gameObject;
		bakuhu.updateAtkOrderBtnStatus (contentObj);



		Destroy (gameObject);
	}


}
