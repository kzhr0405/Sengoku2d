using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GaikouView : MonoBehaviour {

	public bool doumeiFlg = false;
	//Sound
	public AudioSource sound;


	public void OnClick () {

		//SE
		sound = GameObject.Find ("SEController").GetComponent<AudioSource> ();
		sound.PlayOneShot (sound.clip); 

		//Disable Previous Button
		GameObject AttackButton = GameObject.Find ("AttackButton").gameObject;
		GameObject GaikouButton = GameObject.Find ("GaikouButton").gameObject;
		GameObject BouryakuButton = GameObject.Find ("BouryakuButton").gameObject;

		AttackButton.GetComponent<Image> ().enabled = false;
		AttackButton.GetComponent<Button> ().enabled = false;
		AttackButton.transform.Find ("Text").GetComponent<Text> ().enabled = false;

		GaikouButton.GetComponent<Image> ().enabled = false;
		GaikouButton.GetComponent<Button> ().enabled = false;
		GaikouButton.transform.Find ("Text").GetComponent<Text> ().enabled = false;

		BouryakuButton.GetComponent<Image> ().enabled = false;
		BouryakuButton.GetComponent<Button> ().enabled = false;
		BouryakuButton.transform.Find ("Text").GetComponent<Text> ().enabled = false;

		bool doumeiFlg = GameObject.Find ("close").GetComponent<CloseBoard>().doumeiFlg;

		//Add Back to Main Button
		string returnPath = "Prefabs/Map/return";
		GameObject returnBtn = Instantiate(Resources.Load (returnPath)) as GameObject;
		returnBtn.transform.parent = GameObject.Find ("smallBoard(Clone)").transform;
		returnBtn.transform.localScale = new Vector2(1,1);
		returnBtn.name = "return";
		RectTransform returnBtn_transform = returnBtn.GetComponent<RectTransform> ();
		returnBtn_transform.anchoredPosition = new Vector3 (-290, 290, 0);

		CloseBoard close = GameObject.Find ("close").GetComponent<CloseBoard> ();
		close.layer = close.layer + 1;

		//Normal Gaikou Menu or Doumei Menu
		if (!doumeiFlg) {
			//Add Gaikou Button
			string gaikouMenuPath = "Prefabs/Map/gaikou/GaikouMenu";
			GameObject gaikouMenu = Instantiate (Resources.Load (gaikouMenuPath)) as GameObject;
			gaikouMenu.transform.parent = GameObject.Find ("smallBoard(Clone)").transform;
			gaikouMenu.transform.localScale = new Vector2 (1, 1);
			gaikouMenu.name = "GaikouMenu";
			returnBtn.GetComponent<MenuReturn> ().NewMenu = gaikouMenu;
		} else {
			//Add Doumei Gaikou Button
			string gaikouMenuPath = "Prefabs/Map/gaikou/DoumeiGaikouMenu";
			GameObject gaikouMenu = Instantiate (Resources.Load (gaikouMenuPath)) as GameObject;
			gaikouMenu.transform.parent = GameObject.Find ("smallBoard(Clone)").transform;
			gaikouMenu.transform.localScale = new Vector2 (1, 1);
			gaikouMenu.name = "DoumeiGaikouMenu";
			returnBtn.GetComponent<MenuReturn> ().NewMenu = gaikouMenu;
		}


		//Disable Cyoutei in Yamashiro
		int kuniId = close.kuniId;
		if (kuniId == 16) {
			GameObject CyouteiIcon = GameObject.Find ("CyouteiIcon").gameObject;
			CyouteiIcon.GetComponent<Image> ().enabled = false;
			CyouteiIcon.GetComponent<Button> ().enabled = false;
			CyouteiIcon.transform.Find ("Text").GetComponent<Text> ().enabled = false;
			CyouteiIcon.transform.Find ("Image").GetComponent<Image> ().enabled = false;

		} else if (kuniId == 38 || kuniId == 39 || kuniId == 58) {
			GameObject SyouninIcon = GameObject.Find ("SyouninIcon").gameObject;
			SyouninIcon.GetComponent<Image> ().enabled = false;
			SyouninIcon.GetComponent<Button> ().enabled = false;
			SyouninIcon.transform.Find ("Text").GetComponent<Text> ().enabled = false;
			SyouninIcon.transform.Find ("Image").GetComponent<Image> ().enabled = false;

		}

	}
}
