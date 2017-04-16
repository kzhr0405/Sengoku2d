using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class MenuReturn : MonoBehaviour {

	public GameObject NewMenu;
	public GameObject layer2;

	public void OnClick(){

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [1].Play ();

		//Open Current Menu
		GameObject.Find ("kuniName").GetComponent<Text>().text = GameObject.Find ("close").GetComponent<CloseBoard>().title;
		CloseBoard close = GameObject.Find ("close").GetComponent<CloseBoard> ();
		close.layer = close.layer - 1;
		bool soubujireiFlg = PlayerPrefs.GetBool ("soubujireiFlg");
        if (close.layer == 0) {

			//Layer 1 -> 0
			Destroy (NewMenu);

			GameObject AttackButton = GameObject.Find ("AttackButton").gameObject;
			GameObject GaikouButton = GameObject.Find ("GaikouButton").gameObject;
			GameObject BouryakuButton = GameObject.Find ("BouryakuButton").gameObject;
			
			AttackButton.GetComponent<Image> ().enabled = true;
            if(AttackButton.GetComponent<AttackNaiseiView>().openFlg) {
			    AttackButton.GetComponent<Button> ().enabled = true;
            }
            AttackButton.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;

			if(close.doumeiFlg){
				AttackButton.GetComponent<Button>().enabled = false;
			}

			if (soubujireiFlg) {
				Color NGClorBtn = new Color (133 / 255f, 133 / 255f, 80 / 255f, 255f / 255f);
				Color NGClorTxt = new Color (90 / 255f, 90 / 255f, 40 / 255f, 255f / 255f);
				GaikouButton.GetComponent<Image> ().enabled = true;
				GaikouButton.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
				GaikouButton.GetComponent<Image> ().color = NGClorBtn;
				GaikouButton.GetComponent<Button> ().enabled = false;
				GaikouButton.transform.FindChild ("Text").GetComponent<Text> ().color = NGClorTxt;

			} else {
				GaikouButton.GetComponent<Image> ().enabled = true;
				GaikouButton.GetComponent<Button> ().enabled = true;
				GaikouButton.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
			}

			BouryakuButton.GetComponent<Image> ().enabled = true;
			BouryakuButton.GetComponent<Button> ().enabled = true;
			BouryakuButton.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
			
			Destroy (gameObject);	

			int kuniId = close.kuniId;
			if (kuniId == 16) {
				GameObject CyouteiIcon = GameObject.Find ("CyouteiIcon").gameObject;
				CyouteiIcon.GetComponent<Image>().enabled = true;
				CyouteiIcon.GetComponent<Button>().enabled = true;
				CyouteiIcon.transform.FindChild("Text").GetComponent<Text>().enabled = true;
				CyouteiIcon.transform.FindChild("Image").GetComponent<Image>().enabled = true;
				
			}else if (kuniId == 38 || kuniId == 39 || kuniId == 58) {
				GameObject SyouninIcon = GameObject.Find ("SyouninIcon").gameObject;
				SyouninIcon.GetComponent<Image> ().enabled = true;
				SyouninIcon.GetComponent<Button> ().enabled = true;
				SyouninIcon.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
				SyouninIcon.transform.FindChild ("Image").GetComponent<Image> ().enabled = true;
				
			}


		} else if (close.layer == 1) {
			//Layer 2 -> 1
			Destroy(layer2);

			if (NewMenu.name == "GaikouMenu") {


				GaikouMenu gaikouMenu = new GaikouMenu ();
				gaikouMenu.OnGaikouMenu ();

			}else if(NewMenu.name == "DoumeiGaikouMenu"){
				
				DoumeiGaikouMenu gaikouMenu = new DoumeiGaikouMenu ();
				gaikouMenu.OnGaikouMenu ();

			}else if(NewMenu.name == "BouryakuMenu"){
				BouryakuMenu bouryakuMenu = new BouryakuMenu();
				bouryakuMenu.OnBouryakuMenu();
			}
		}
	}
}
