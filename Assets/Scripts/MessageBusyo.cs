using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class MessageBusyo : MonoBehaviour {

	public void makeMessage (string Text, int busyoId, string type) {

        Message msg = new Message();

		string msgPath = "Prefabs/Common/MessageBusyo";
		GameObject messageObj = Instantiate (Resources.Load (msgPath)) as GameObject;
		messageObj.transform.SetParent(GameObject.Find ("Panel").transform);
		messageObj.transform.FindChild ("MessageBusyo").transform.GetComponent<Text> ().text = Text;
		messageObj.name = "MessageBusyo";	
		messageObj.transform.localScale = new Vector2 (1, 1);
		RectTransform messageTransform = messageObj.GetComponent<RectTransform> ();
		messageTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

		//Busyo View
		string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
		GameObject Busyo = Instantiate (Resources.Load (busyoPath)) as GameObject;	
		Busyo.name = busyoId.ToString();
		Busyo.transform.SetParent (messageObj.transform);
		Busyo.transform.localScale = new Vector2 (4, 4);
		Busyo.GetComponent<DragHandler>().enabled = false;
		RectTransform busyo_transform = Busyo.GetComponent<RectTransform>();
		busyo_transform.anchoredPosition3D = new Vector3(200,200,0);
		busyo_transform.sizeDelta = new Vector2( 70, 70);
		foreach ( Transform n in Busyo.transform ){
			GameObject.Destroy(n.gameObject);
		}
		messageObj.GetComponent<FadeOutBusyo> ().busyoImage = Busyo.GetComponent<Image> ();

		//Serihu
		string serihuPath = "Prefabs/Common/Serihu";
		GameObject Serihu = Instantiate (Resources.Load (serihuPath)) as GameObject;			
		Serihu.transform.SetParent (Busyo.transform);
		Serihu.transform.localScale = new Vector2 (0.2f, 0.2f);
		RectTransform Serihu_transform = Serihu.GetComponent<RectTransform>();
		Serihu_transform.anchoredPosition3D = new Vector3(100,0,0);
		Serihu.name = "Serihu";
        int langId = PlayerPrefs.GetInt("langId");

        string serihu = "";
		int myDaimyoBusyo = PlayerPrefs.GetInt ("myDaimyoBusyo");
		if (myDaimyoBusyo == busyoId) {

            Entity_serihu_mst serihuMst = Resources.Load("Data/serihu_mst") as Entity_serihu_mst;
            if (type == "tsuihou") {
                
                if (langId == 2) {
                    serihu = serihuMst.param[busyoId - 1].tsuihouMsgEng;
                }else {
                    serihu = serihuMst.param[busyoId - 1].tsuihouMsg;
                }
            }else if (type == "touyou") {
                if (langId == 2) {
                    serihu = serihuMst.param[busyoId - 1].touyouMsgEng;
                }else {
                    serihu = serihuMst.param[busyoId - 1].touyouMsg;
                }
            }else if (type == "ninmei") {
                serihu = msg.getMessage(89);
            }
            
        } else {

			//Serihu Contents
			Entity_serihu_mst serihuMst = Resources.Load ("Data/serihu_mst") as Entity_serihu_mst;

			if (type == "tsuihou") {
                if (langId == 2) {
                    serihu = serihuMst.param [busyoId - 1].tsuihouMsgEng;
                }else {
                    serihu = serihuMst.param[busyoId - 1].tsuihouMsg;
                }
			} else if (type == "touyou") {
                if (langId == 2) {
                    serihu = serihuMst.param [busyoId - 1].touyouMsgEng;
                }else {
                    serihu = serihuMst.param[busyoId - 1].touyouMsg;
                }
			} else if (type == "ninmei") {
				serihu = msg.getMessage(90);
			} 
		}
		Serihu.transform.FindChild("BusyoSerihu").GetComponent<Text>().text = serihu;
	}
}
