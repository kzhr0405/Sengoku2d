using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class Ninmei : MonoBehaviour {

	//ninmei or kainin
	public bool kaininFlg = false;
	public int jyosyuId = 0;
	public string jyosyuName = "";

	public void OnClick(){
		
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
        int langId = PlayerPrefs.GetInt("langId");
        //Ninmei
        if (kaininFlg == false) {

			//Make Jyosyu List
			string openKuniString = PlayerPrefs.GetString ("openKuni");
			List<string> openKuniList = new List<string> ();
			char[] delimiterChars = {','};
			openKuniList = new List<string> (openKuniString.Split (delimiterChars));

			List<string> jyosyuList = new List<string> ();
			for (int i=0; i<openKuniList.Count; i++) {
				string temp = "jyosyu" + openKuniList [i];
				if (PlayerPrefs.HasKey (temp)) {
					int jyosyuId = PlayerPrefs.GetInt (temp);
					jyosyuList.Add (jyosyuId.ToString ());
				}
			}

			//Available Jyosyu List
			string myBusyoString = PlayerPrefs.GetString ("myBusyo");
			List<string> myBusyoList = new List<string> ();
			myBusyoList = new List<string> (myBusyoString.Split (delimiterChars));

			//Reduce MyBusyo - CurrentJyosyu
			myBusyoList.RemoveAll (jyosyuList.Contains);

            //Reduce MyDaimyo
            //int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
            //Daimyo daimyo = new Daimyo();
            //int myDaimyoBusyo = daimyo.getDaimyoBusyoId(myDaimyo);
            //myBusyoList.Remove(myDaimyoBusyo.ToString());
            
            if (myBusyoList.Count > 0){
				audioSources [0].Play ();
				BusyoStatusButton pop = new BusyoStatusButton ();
				pop.commonPopup (19);
                if (langId==2) {
                    GameObject.Find ("popText").GetComponent<Text> ().text = "Feudatory";
                }else {
                    GameObject.Find("popText").GetComponent<Text>().text = "城主任命";
                }
				//Set Scroll View
				string scrollPath = "Prefabs/Naisei/ScrollView";
				GameObject scroll = Instantiate (Resources.Load (scrollPath)) as GameObject;
				scroll.transform.SetParent (GameObject.Find ("board(Clone)").transform);
				scroll.transform.localScale = new Vector2 (1, 1);
				scroll.name = "ScrollView";
				RectTransform scrollTransform = scroll.GetComponent<RectTransform> ();
				scrollTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

                List<Busyo> baseBusyoList = new List<Busyo>();
                BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
                StatusGet sts = new StatusGet();
                foreach (string busyoIdString in myBusyoList) {
                    int busyoId = int.Parse(busyoIdString);
                    string busyoName = BusyoInfoGet.getName(busyoId, langId);
                    string rank = BusyoInfoGet.getRank(busyoId);
                    int lv = PlayerPrefs.GetInt(busyoId.ToString());
                    float dfcSts = (float)sts.getDfc(busyoId, lv);
                    float hpSts = (float)sts.getHp(busyoId, lv);
                    float atkSts = (float)sts.getAtk(busyoId, lv);
                    baseBusyoList.Add(new Busyo(busyoId, busyoName, rank,0, "", 0, 0, lv, hpSts, atkSts, dfcSts, 0,0,0));
                }
                List<Busyo> myBusyoSortList = new List<Busyo>(baseBusyoList);
                myBusyoSortList.Sort((a, b) => {
                    float result = b.dfc - a.dfc;
                    return (int)result;
                });


                //Show Available List
                foreach (Busyo Busyo in myBusyoSortList) {
					string slotPath = "Prefabs/Naisei/BusyoSlot";
					GameObject slot = Instantiate (Resources.Load (slotPath)) as GameObject;
					slot.transform.SetParent (scroll.transform.FindChild ("NaiseiContent").transform);
					slot.transform.localScale = new Vector2 (1, 1);

					string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
					GameObject busyo = Instantiate (Resources.Load (busyoPath)) as GameObject;
					busyo.name = Busyo.busyoId.ToString();
					busyo.transform.SetParent (slot.transform.FindChild ("Busyo").transform);
					busyo.transform.localScale = new Vector2 (3.5f, 3.5f);
					RectTransform busyo_transform = busyo.GetComponent<RectTransform> ();
					busyo_transform.anchoredPosition3D = new Vector3 (0, -300, 0);
					busyo_transform.sizeDelta = new Vector2 (200, 200);
					busyo.GetComponent<DragHandler> ().enabled = false;

					GameObject text = busyo.transform.FindChild ("Text").gameObject;
					text.transform.localScale = new Vector2 (0.6f, 0.6f);
					RectTransform text_transform = text.GetComponent<RectTransform> ();
					text_transform.anchoredPosition3D = new Vector3 (-200, 65, 0);

					GameObject rank = busyo.transform.FindChild ("Rank").gameObject;
					rank.transform.localScale = new Vector2 (1, 1);
					RectTransform rank_transform = rank.GetComponent<RectTransform> ();
					rank_transform.anchoredPosition3D = new Vector3 (30, -100, 0);

					//Status					
					float naiseiSts = Busyo.dfc / 2;
					float boubiStatusTemp = (Busyo.hp + Busyo.atk) / 2;
					float boubiStatus = boubiStatusTemp / 2;

					slot.transform.FindChild ("Busyo").transform.FindChild ("NaiseiEffectValue").GetComponent<Text> ().text = "+" + naiseiSts.ToString ("f1") + "%";
					slot.transform.FindChild ("Busyo").transform.FindChild ("BoubiEffectValue").GetComponent<Text> ().text = "+" + boubiStatus.ToString ("f1") + "%";

					//Lv
					string lvPath = "Prefabs/Naisei/Lv";
					GameObject lvObj = Instantiate (Resources.Load (lvPath)) as GameObject;
					lvObj.transform.SetParent (busyo.transform);
					lvObj.GetComponent<Text> ().text = "Lv" + Busyo.lv;
					lvObj.transform.localScale = new Vector2 (0.1f, 0.1f);
					RectTransform lv_transform = lvObj.GetComponent<RectTransform> ();
					lv_transform.anchoredPosition3D = new Vector3 (130, -70, 0);

					//Button
					slot.transform.FindChild ("Busyo").transform.FindChild ("NinmeiButton").GetComponent<DoNinmei> ().busyoId = Busyo.busyoId.ToString();
				}
                GameObject contents = scroll.transform.FindChild("NaiseiContent").gameObject;
                contents.transform.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0.0f;
            }else{
				audioSources [4].Play ();
				Message msg = new Message();
				msg.makeMessage(msg.getMessage(119));
			}
		} else {
			audioSources [0].Play ();
			//Kainin
			//Common Process
			//Back Cover
			string backPath = "Prefabs/Common/TouchBack";
			GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
			back.transform.SetParent(GameObject.Find ("Panel").transform);
			back.transform.localScale = new Vector2 (1, 1);
			RectTransform backTransform = back.GetComponent<RectTransform> ();
			backTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
			back.name = "TouchBack";

			//Message Box
			string msgPath = "Prefabs/Naisei/KaininConfirm";
			GameObject msg = Instantiate (Resources.Load (msgPath)) as GameObject;
			msg.transform.SetParent(back.transform);
			msg.transform.localScale = new Vector2 (1, 1);
			RectTransform msgTransform = msg.GetComponent<RectTransform> ();
			msgTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
			msgTransform.name = "kaininConfirm";

			//Message Text Mod
			GameObject msgObj = msg.transform.FindChild ("KaininText").gameObject;
			int myDaimyoBusyo = PlayerPrefs.GetInt ("myDaimyoBusyo");
            string msgText = "";
			if (myDaimyoBusyo == jyosyuId) {
                if (langId == 2) {
                    msgText = "My lord, do you want to resign the lord of this country?";
                }
                else {
                    msgText = "御館様、自らを城主から解任なさいますか？";
                }
            } else {
                if (langId == 2) {
                    msgText = "My lord, do you want to remove " + jyosyuName + " from the lord of this country?";
                }
                else {
                    msgText = "御館様、" + jyosyuName + "殿を城主から解任なさいますか？";
                }
            }



            msgObj.GetComponent<Text> ().text = msgText;

		}
	}
}
