using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class TouyouView : MonoBehaviour {

	public int busyoId;
	public bool daimyoFlg;
	public string busyoRank = "";

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

        //Panel
        GameObject.Find("Touyou").GetComponent<Canvas>().sortingLayerName = "unit";

		//Pop View
		BusyoStatusButton pop = new BusyoStatusButton ();
		GameObject board = pop.commonPopup (27);
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            GameObject.Find ("popText").GetComponent<Text> ().text = "Samurai Recruitment";
        }else {
            GameObject.Find("popText").GetComponent<Text>().text = "武将登用";
        }
		//Kamon
		string kamonPath = "Prefabs/Touyou/kamon";
		GameObject kamon = Instantiate (Resources.Load (kamonPath)) as GameObject;			
		kamon.transform.SetParent (board.transform);
		kamon.transform.localScale = new Vector2 (1, 1);
		kamon.transform.localPosition = new Vector2 (-310, 0);
		BusyoInfoGet busyoScript = new BusyoInfoGet ();
        int senarioId = PlayerPrefs.GetInt("senarioId");
        int daimyoId = busyoScript.getDaimyoId (busyoId,senarioId);
		if (daimyoId == 0) {
			daimyoId = busyoScript.getDaimyoHst (busyoId,senarioId);
		}
		string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString ();
		kamon.GetComponent<Image> ().sprite = 
			Resources.Load (imagePath, typeof(Sprite)) as Sprite;

		//Busyo View
		string path = "Prefabs/Player/Unit/BusyoUnit";
		GameObject Busyo = Instantiate (Resources.Load (path)) as GameObject;
		Busyo.name = busyoId.ToString ();
		Busyo.transform.SetParent (board.transform);
		Busyo.transform.localScale = new Vector2 (3.5f, 3.5f);
		Busyo.GetComponent<DragHandler>().enabled = false;
		RectTransform busyo_transform = Busyo.GetComponent<RectTransform>();
		busyo_transform.anchoredPosition = new Vector3(350,300,0);
		busyo_transform.sizeDelta = new Vector2( 100, 100);

        //Ship Rank
        string shipPath = "Prefabs/Busyo/ShipSts";
        GameObject ShipObj = Instantiate(Resources.Load(shipPath)) as GameObject;
        ShipObj.transform.SetParent(Busyo.transform);
        preKaisen kaisenScript = new preKaisen();
        int shipId = kaisenScript.getShipSprite(ShipObj, busyoId);
        ShipObj.transform.localPosition = new Vector3(-40, -40, 0);
        ShipObj.transform.localScale = new Vector2(0.5f, 0.5f);
        if (langId == 2) {
            if (shipId == 1) {
            ShipObj.transform.Find("Text").GetComponent<Text>().text = "High";
            }
            else if (shipId == 2) {
                ShipObj.transform.Find("Text").GetComponent<Text>().text = "Mid";
            }
            else if (shipId == 3) {
                ShipObj.transform.Find("Text").GetComponent<Text>().text = "Low";
            }
        }else {
            if (shipId == 1) {
                ShipObj.transform.Find("Text").GetComponent<Text>().text = "上";
            }
            else if (shipId == 2) {
                ShipObj.transform.Find("Text").GetComponent<Text>().text = "中";
            }
            else if (shipId == 3) {
                ShipObj.transform.Find("Text").GetComponent<Text>().text = "下";
            }
        }


        //Text Modification
        Busyo.transform.Find ("Text").gameObject.GetComponent<Text>().enabled = false;

		//Rank Text Modification
		GameObject rank = Busyo.transform.Find ("Rank").gameObject;
		RectTransform rank_transform = rank.GetComponent<RectTransform>();
		rank_transform.anchoredPosition = new Vector3 (0,-50,0);
		rank_transform.sizeDelta = new Vector2( 200, 200);
		rank.GetComponent<Text>().fontSize = 200;

		/*Status*/
		string statusPath = "Prefabs/Touyou/busyoStatus";
		GameObject status = Instantiate (Resources.Load (statusPath)) as GameObject;			
		status.transform.SetParent (board.transform);
		status.transform.localScale = new Vector2 (1, 1);
		RectTransform status_transform = status.GetComponent<RectTransform>();
		status_transform.anchoredPosition = new Vector3(245,-40,0);


		Entity_busyo_mst busyoMst  = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
		Entity_senpou_mst senpouMst  = Resources.Load ("Data/senpou_mst") as Entity_senpou_mst;
        string busyoName = busyoScript.getName(busyoId,langId);
        GameObject.Find ("busyoNameValue").GetComponent<Text>().text = busyoName;
		GameObject.Find ("TosotsuValue").GetComponent<Text>().text = busyoMst.param [busyoId-1].minHp.ToString() + "00";
		GameObject.Find ("BuyuuValue").GetComponent<Text> ().text = busyoMst.param [busyoId - 1].minAtk.ToString () + "0";
		GameObject.Find ("ChiryakuValue").GetComponent<Text>().text = busyoMst.param [busyoId-1].minDfc.ToString() + "0";
		GameObject.Find ("SpeedValue").GetComponent<Text>().text = busyoMst.param [busyoId-1].minSpd.ToString();

		string heisyuType = busyoMst.param [busyoId - 1].heisyu;
		string heisyu = "";
        Message msg = new Message();
		if (heisyuType == "KB") {
            heisyu = msg.getMessage(55);
		} else if (heisyuType == "YR") {
			heisyu = msg.getMessage(56);
        } else if (heisyuType == "TP") {
			heisyu = msg.getMessage(57);
        } else if (heisyuType == "YM") {
			heisyu = msg.getMessage(58);
        }

		GameObject.Find ("ChildNameValue").GetComponent<Text>().text = heisyu;

		int senpouId = busyoMst.param [busyoId-1].senpou_id;
        if (langId == 2) {
            GameObject.Find ("SenpouValue").GetComponent<Text>().text = senpouMst.param[senpouId-1].nameEng;
        }else {
            GameObject.Find("SenpouValue").GetComponent<Text>().text = senpouMst.param[senpouId - 1].name;
        }
		int senpouStatus = senpouMst.param [senpouId - 1].lv1;
		int each = (int)senpouMst.param [senpouId - 1].each;
		int ratio = (int)senpouMst.param [senpouId - 1].ratio;
		int term = (int)senpouMst.param [senpouId - 1].term;
        string senpouExp = "";
        if (langId == 2) {
            senpouExp = senpouMst.param [senpouId - 1].effectionEng;
        }else {
            senpouExp = senpouMst.param[senpouId - 1].effection;
        }
        if (langId == 2) {
            senpouExp = senpouExp.Replace("ABC", senpouStatus.ToString());
            senpouExp = senpouExp.Replace("DEF", each.ToString());
            senpouExp = senpouExp.Replace("GHI", ratio.ToString());
            senpouExp = senpouExp.Replace("JKL", term.ToString());
        }else {
            senpouExp = senpouExp.Replace("A", senpouStatus.ToString());
		    senpouExp = senpouExp.Replace("B", each.ToString());
		    senpouExp = senpouExp.Replace("C", ratio.ToString());
		    senpouExp = senpouExp.Replace("D", term.ToString());
        }
		GameObject.Find ("SenpouExpValue").GetComponent<Text>().text = senpouExp;


		/*Saku*/
		Saku saku = new Saku ();
		List<string> sakuList = new List<string>();
		sakuList = saku.getSakuInfo (busyoId);
		
		//Icon
		string sakuPath = "Prefabs/Saku/saku" + sakuList[0];
		GameObject sakuIcon = Instantiate (Resources.Load (sakuPath)) as GameObject;
		GameObject StatusSaku = status.transform.Find("StatusSaku").gameObject;
		foreach ( Transform n in StatusSaku.transform ){
			if(n.tag == "Saku"){
				GameObject.Destroy(n.gameObject);
			}
		}
		sakuIcon.transform.SetParent (StatusSaku.transform);
		sakuIcon.transform.localScale = new Vector2 (0.7f, 0.7f);
		sakuIcon.GetComponent<Button>().enabled = false;
		RectTransform sakuIcon_transform = sakuIcon.GetComponent<RectTransform>();
		sakuIcon_transform.anchoredPosition = new Vector3(-235,0,0);
		
		StatusSaku.transform.Find("SakuExp").transform.Find("SakuExpValue").GetComponent<Text>().text = sakuList[2];

		/*daimyo busyo check*/
		Daimyo daimyo = new Daimyo ();
		daimyoFlg = daimyo.daimyoBusyoCheck (busyoId);

		//pass data to button
		GameObject touyouBtn = GameObject.Find ("TouyouButton").gameObject;
		touyouBtn.GetComponent<DoTouyou> ().busyoId = busyoId;
		touyouBtn.GetComponent<DoTouyou> ().busyoName = busyoName;
		touyouBtn.GetComponent<DoTouyou> ().heisyu = heisyuType;
		touyouBtn.GetComponent<DoTouyou> ().sequence = int.Parse(name);
		touyouBtn.GetComponent<DoTouyou> ().rank = busyoRank;
		touyouBtn.GetComponent<DoTouyou> ().daimyoFlg = daimyoFlg;


        //Tutorial
        if (Application.loadedLevelName == "tutorialTouyou") {
            TutorialController tutorialScript = new TutorialController();
            Vector2 vect = new Vector2(0,50);
            GameObject btn = tutorialScript.SetPointer(touyouBtn, vect);
            btn.transform.localScale = new Vector2(150,150);

        }

        //Hired Check
        if (Application.loadedLevelName != "tutorialTouyou") {
            string myBusyo = PlayerPrefs.GetString("myBusyo");
            char[] delimiterChars = { ',' };

            if(myBusyo != null && myBusyo != "") {
                List<string> myBusyoList = new List<string>();
                if (myBusyo.Contains(",")) {
                    myBusyoList = new List<string>(myBusyo.Split(delimiterChars));
                }else {
                    myBusyoList.Add(myBusyo);
                }

                if (myBusyoList.Contains(busyoId.ToString())) {
                    msg.makeMessage(msg.getMessage(137));

                }
            }
            //Zukan Check
            string zukanBusyoHst = PlayerPrefs.GetString("zukanBusyoHst");
            if (zukanBusyoHst != null && zukanBusyoHst != "") {
                List<string> myZukanList = new List<string>();
                if (zukanBusyoHst.Contains(",")) {
                    myZukanList = new List<string>(zukanBusyoHst.Split(delimiterChars));
                }else {
                    myZukanList.Add(zukanBusyoHst);
                }

                if (myZukanList.Contains(busyoId.ToString())) {
                    string zukanPath = "Prefabs/Touyou/Zukan";
                    GameObject zukan = Instantiate(Resources.Load(zukanPath)) as GameObject;
                    zukan.transform.SetParent(board.transform);
                    zukan.transform.localScale = new Vector2(1, 1);
                    zukan.transform.localPosition = new Vector2(-41, 167);                    
                }
            }
        }
    }
}
