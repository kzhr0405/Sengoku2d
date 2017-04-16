using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class AddMoneyConfirm : MonoBehaviour {

	public void OnClick(){

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		int busyoDama = PlayerPrefs.GetInt ("busyoDama");
		if (busyoDama >= 100) {

			//SE
			audioSources [0].Play ();

			GameObject panel = GameObject.Find ("Panel").gameObject;

			string pathOfBack = "Prefabs/Common/TouchBackForOne";
			GameObject back = Instantiate (Resources.Load (pathOfBack)) as GameObject;
			back.transform.SetParent (panel.transform);
			back.transform.localScale = new Vector2 (1, 1);
			back.transform.localPosition = new Vector2 (0, 0);

			string pathOfBoard = "Prefabs/Map/common/AddMoneyBoard";
			GameObject board = Instantiate (Resources.Load (pathOfBoard)) as GameObject;
			board.transform.SetParent (panel.transform);
            if (Application.loadedLevelName != "shisya") {
                board.transform.localScale = new Vector2 (1, 1);
            }else {
                board.transform.localScale = new Vector2(1, 0.8f);
            }
            board.transform.localPosition = new Vector2 (0, 0);

			//qa
			string qaPath = "Prefabs/Common/Question";
			GameObject qa = Instantiate (Resources.Load (qaPath)) as GameObject;
			qa.transform.SetParent(board.transform);
			qa.transform.localScale = new Vector2 (1, 1);
			RectTransform qaTransform = qa.GetComponent<RectTransform> ();
			qaTransform.anchoredPosition = new Vector3 (-258, 258, 0);
			qaTransform.sizeDelta = new Vector2 (25,33);
			qa.name = "qa";
			qa.GetComponent<QA> ().qaId = 13;

			back.GetComponent<CloseOneBoard> ().deleteObj = board;
			board.transform.FindChild ("YesButton").GetComponent<AddMoney> ().touchBackObj = back;
			board.transform.FindChild ("NoButton").GetComponent<AddMoney> ().touchBackObj = back;
		
		}else {
			audioSources [4].Play ();
			Message msg = new Message ();
			msg.makeMessage (msg.getMessage(2));		
		}
	}
}
