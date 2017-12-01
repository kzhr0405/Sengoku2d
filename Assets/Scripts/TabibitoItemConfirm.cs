using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class TabibitoItemConfirm : MonoBehaviour {



	public void OnClick(){
		
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		int meiseiItem = PlayerPrefs.GetInt ("meisei");
		if (meiseiItem > 0) {
			audioSources [0].Play ();
			GameObject panel = GameObject.Find ("Panel").gameObject;

			string pathOfBack = "Prefabs/Common/TouchBackForOne";
			GameObject back = Instantiate (Resources.Load (pathOfBack)) as GameObject;
			back.transform.SetParent (panel.transform);
			back.transform.localScale = new Vector2 (1, 1);
			back.transform.localPosition = new Vector2 (0, 0);

			string pathOfBoard = "Prefabs/Naisei/TabibitoUpdateBoard";
			GameObject board = Instantiate (Resources.Load (pathOfBoard)) as GameObject;
			board.transform.SetParent (panel.transform);
			board.transform.localScale = new Vector2 (1, 0.85f);
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
			qa.GetComponent<QA> ().qaId = 14;

			back.GetComponent<CloseOneBoard> ().deleteObj = board;
			board.transform.Find ("YesButton").GetComponent<TabibitoItem> ().touchBackObj = back;
			board.transform.Find ("NoButton").GetComponent<TabibitoItem> ().touchBackObj = back;

		} else {
			audioSources [4].Play ();

			Message msg = new Message ();
			msg.makeMessage (msg.getMessage(117));		
		}

	}
}
