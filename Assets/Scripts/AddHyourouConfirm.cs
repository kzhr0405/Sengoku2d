using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class AddHyourouConfirm : MonoBehaviour {

    //PvP
    //public GameObject lightning;
    //public GameObject vs;

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

			string pathOfBoard = "Prefabs/Map/common/AddHyourouBoard";
			GameObject board = Instantiate (Resources.Load (pathOfBoard)) as GameObject;
			board.transform.SetParent (panel.transform);
			if (Application.loadedLevelName != "shisya" && Application.loadedLevelName != "naisei" && Application.loadedLevelName != "pvp") {
                board.transform.localScale = new Vector2(1, 1);            
            }else { 
                board.transform.localScale = new Vector2(1, 0.8f);
            }
            board.transform.localPosition = new Vector2 (0, 0);

            //PvP
            if (Application.loadedLevelName == "pvp") {
                GameObject.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "UI";
            }


                //qa
                string qaPath = "Prefabs/Common/Question";
			GameObject qa = Instantiate (Resources.Load (qaPath)) as GameObject;
			qa.transform.SetParent(board.transform);
			qa.transform.localScale = new Vector2 (1, 1);
			RectTransform qaTransform = qa.GetComponent<RectTransform> ();
			qaTransform.anchoredPosition = new Vector3 (-258, 258, 0);
			qaTransform.sizeDelta = new Vector2 (25,33);
			qa.name = "qa";
			qa.GetComponent<QA> ().qaId = 7;

			back.GetComponent<CloseOneBoard> ().deleteObj = board;
			board.transform.Find ("YesButton").GetComponent<AddHyourou> ().touchBackObj = back;
			board.transform.Find ("NoButton").GetComponent<AddHyourou> ().touchBackObj = back;
		
		} else {
			audioSources [4].Play ();
			Message msg = new Message ();
			msg.makeMessage (msg.getMessage(2));		
		}
	}
}
