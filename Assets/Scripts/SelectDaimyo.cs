using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class SelectDaimyo : MonoBehaviour {

	public string daimyoName = "";
	public int daimyoId = 0;
	public int daimyoBusyoId = 0;
	public bool busyoHaveFlg;
	public string heisyu = "";

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		//Confirm Button
		string backPath = "Prefabs/Busyo/Back";
		GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
		back.transform.SetParent(GameObject.Find ("Panel").transform);
		back.transform.localScale = new Vector2 (1, 1);
		RectTransform backTransform = back.GetComponent<RectTransform> ();
		backTransform.anchoredPosition = new Vector3 (0, 0, 0);
        back.name = "Back";

        //Message Box
        string msgPath = "Prefabs/clearOrGameOver/DaimyoSelectConfirm";
		GameObject msg = Instantiate (Resources.Load (msgPath)) as GameObject;
		msg.transform.SetParent(GameObject.Find ("Panel").transform);
		msg.transform.localScale = new Vector2 (0.8f, 1.0f);
		RectTransform msgTransform = msg.GetComponent<RectTransform> ();
		msgTransform.anchoredPosition = new Vector3 (0, 0, 0);
		msgTransform.name = "DaimyoSelectConfirm";
		
		GameObject msgObj = GameObject.Find ("DaimyoSelectText");
		string msgText = msgObj.GetComponent<Text> ().text;
		
		
		//Message Text Mod
		msgText = msgText.Replace ("A", daimyoName);
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msgText = "Would you start the samurai world with " + daimyoName + "?";
        }else if(langId==3) {
            msgText = "是否要用" + daimyoName + "开始战国乱世？";
        }
        else {
            msgText = daimyoName + "で戦国の世を始めますか？";
        }
        msgObj.GetComponent<Text> ().text = msgText;
		
		//Add busyoId
		GameObject.Find ("YesButton").GetComponent<DoSelectDaimyo> ().daimyoId = daimyoId;
		GameObject.Find ("YesButton").GetComponent<DoSelectDaimyo> ().daimyoBusyoId = daimyoBusyoId;
		GameObject.Find ("YesButton").GetComponent<DoSelectDaimyo> ().busyoHaveFlg = busyoHaveFlg;
		GameObject.Find ("YesButton").GetComponent<DoSelectDaimyo> ().heisyu = heisyu;

        GameObject.Find("YesButtonHard").GetComponent<DoSelectDaimyo>().daimyoId = daimyoId;
        GameObject.Find("YesButtonHard").GetComponent<DoSelectDaimyo>().daimyoBusyoId = daimyoBusyoId;
        GameObject.Find("YesButtonHard").GetComponent<DoSelectDaimyo>().busyoHaveFlg = busyoHaveFlg;
        GameObject.Find("YesButtonHard").GetComponent<DoSelectDaimyo>().heisyu = heisyu;
    }
}
