using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System;

public class TouyouController : MonoBehaviour {

	public int freeGacyaCount = 0;
	public string freeGacyaTimeString = "";


	// Use this for initialization
	void Start () {
		int busyoDama = PlayerPrefs.GetInt("busyoDama");
		GameObject.Find ("BusyoDamaValue").GetComponent<Text> ().text = busyoDama.ToString();

		//QA
		GameObject.Find("FreeQuestion").GetComponent<QA>().qaId = 9;
		GameObject.Find("DamaQuestion").GetComponent<QA>().qaId = 10;


		/*Free Gacya Count*/
		freeGacyaTimeString = PlayerPrefs.GetString ("freeGacyaDate");
		if (freeGacyaTimeString == null || freeGacyaTimeString == "") {
			freeGacyaTimeString = System.DateTime.Today.ToString ();
			PlayerPrefs.SetString ("freeGacyaDate",freeGacyaTimeString);
			PlayerPrefs.Flush();
		}
		System.DateTime loginTime = System.DateTime.Parse (freeGacyaTimeString);
		System.TimeSpan span = System.DateTime.Today - loginTime;
		double spanDay = span.TotalDays;

		if (spanDay >= 1) {
			//Reset
			PlayerPrefs.SetInt ("freeGacyaCounter",0);
			PlayerPrefs.Flush();
			freeGacyaCount = 0;
			
		}else{
			//Get Counted No
			freeGacyaCount = PlayerPrefs.GetInt ("freeGacyaCounter");
		}
		int remain = 5 - freeGacyaCount;
		GameObject.Find("Count").GetComponent<Text>().text = remain.ToString();


        /*View Last Hit Busyo*/
        //Get History
        bool tutorialDoneFlg = PlayerPrefs.GetBool("tutorialDoneFlg");
        if (tutorialDoneFlg && Application.loadedLevelName != "tutorialTouyou") {
            string gacyaHst = PlayerPrefs.GetString("gacyaHst");
		    if (gacyaHst != null && gacyaHst != "") {
			    //View History
			    char[] delimiterChars = {','};
			    string[] tokens = gacyaHst.Split(delimiterChars);		
			    int[] hitBusyo = Array.ConvertAll<string, int>(tokens, int.Parse);

			    Gacya viewBusyo = new Gacya();
			    viewBusyo.viewBusyo(hitBusyo,false);

		    } else {
			    //View Message for only 1st time
			    Message msg = new Message();
                string Text = msg.getMessage(53);
			    msg.makeMessage(Text);
			    GameObject messageObj = GameObject.Find ("MessageObject");
			    messageObj.transform.SetParent(GameObject.Find ("CenterView").transform);
			    RectTransform messageTransform = messageObj.GetComponent<RectTransform> ();
			    messageTransform.anchoredPosition = new Vector3 (0, 0, 0);

			    messageObj.GetComponent<FadeuGUI>().enabled = false;
		    }
        }else {
            //View Message for only 1st time
            Message msg = new Message();
            string Text = msg.getMessage(53);
            msg.makeMessage(Text);
            GameObject messageObj = GameObject.Find("MessageObject");
            messageObj.transform.SetParent(GameObject.Find("CenterView").transform);
            RectTransform messageTransform = messageObj.GetComponent<RectTransform>();
            messageTransform.anchoredPosition = new Vector3(0, 0, 0);

            messageObj.GetComponent<FadeuGUI>().enabled = false;
        }
	}
}
