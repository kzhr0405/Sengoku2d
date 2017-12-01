using UnityEngine;
using System.Collections;

public class NaiseiDestroy : MonoBehaviour {

	public int areaId = 0;
	public int activeKuniId = 0;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		string Text = "";
		string backPath = "Prefabs/Common/TouchBack";
		GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
		back.transform.SetParent(GameObject.Find ("Panel").transform);
		back.transform.localScale = new Vector2 (1, 1);
		RectTransform backTransform = back.GetComponent<RectTransform> ();
		backTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
		back.name = "TouchBack";

		//Message Box
		string msgPath = "Prefabs/Naisei/DestroyConfirm";
		GameObject msg = Instantiate (Resources.Load (msgPath)) as GameObject;
		msg.transform.SetParent(back.transform);
		msg.transform.localScale = new Vector2 (1, 1);
		RectTransform msgTransform = msg.GetComponent<RectTransform> ();
		msgTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
		msg.name = "DestroyConfirm";
		msg.transform.Find ("YesButton").GetComponent<DoDestroy> ().areaId = areaId;
		msg.transform.Find ("YesButton").GetComponent<DoDestroy> ().activeKuniId = activeKuniId;


	}
}
