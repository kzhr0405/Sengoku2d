using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PopInfo : MonoBehaviour {

	//Busyo
	public int busyoId = 0;
	public string busyoName = "";
	public int hp = 0;
	public int atk = 0;
	public int dfc = 0;
	public int spd = 0;
	public string heisyu = "";
	public int daimyoId = 0;
	public int senpouId = 0;

	public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		string pathOfBack = "Prefabs/Common/TouchBack";
		GameObject back = Instantiate (Resources.Load (pathOfBack)) as GameObject;
		back.transform.parent = GameObject.Find ("Panel").transform;
		back.transform.localScale = new Vector2 (1, 1);
		back.transform.localPosition = new Vector2 (0, 0);

		string pathOfPop = "Prefabs/Zukan/busyoPop";
		GameObject pop = Instantiate (Resources.Load (pathOfPop)) as GameObject;
		pop.transform.parent = GameObject.Find ("Panel").transform;
		pop.transform.localScale = new Vector2 (1, 1);
		pop.transform.localPosition = new Vector2 (0, 0);

		//Kamon
		GameObject kamon = pop.transform.FindChild ("kamon").gameObject;
		string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString ();
		kamon.GetComponent<Image> ().sprite = 
			Resources.Load (imagePath, typeof(Sprite)) as Sprite;

		//Busyo Icon
		string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
		GameObject busyo = Instantiate (Resources.Load (busyoPath)) as GameObject;
		busyo.name = busyoId.ToString ();
		busyo.transform.SetParent (pop.transform);
		busyo.transform.localScale = new Vector2 (7, 7);
		busyo.GetComponent<DragHandler>().enabled = false;
		RectTransform busyoRect = busyo.GetComponent<RectTransform>();
		busyoRect.anchoredPosition3D = new Vector3(180,400,0);
		busyoRect.sizeDelta = new Vector2(40,40);
		busyo.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;

        //Ship Rank
        string shipPath = "Prefabs/Busyo/ShipSts";
        GameObject ShipObj = Instantiate(Resources.Load(shipPath)) as GameObject;
        ShipObj.transform.SetParent(busyo.transform);
        preKaisen kaisenScript = new preKaisen();
        int shipId = kaisenScript.getShipSprite(ShipObj, busyoId);
        ShipObj.transform.localPosition = new Vector3(-10, -15, 0);
        ShipObj.transform.localScale = new Vector2(0.2f, 0.2f);
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            if (shipId == 1) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "High";
            }else if (shipId == 2) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "Mid";
            }else if (shipId == 3) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "Low";
            }
        }else {
            if (shipId == 1) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "上";
            }
            else if (shipId == 2) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "中";
            }
            else if (shipId == 3) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "下";
            }
        }
        //Name
        GameObject.Find ("busyoNameValue").GetComponent<Text> ().text = busyoName;
		string heisyuName = "";
        Message msg = new Message();
        if (heisyu == "YR") {
            heisyuName = msg.getMessage(56);
        }
        else if (heisyu == "KB") {
            heisyuName = msg.getMessage(55);
        }
        else if (heisyu == "YM") {
            heisyuName = msg.getMessage(58);
        }
        else if (heisyu == "TP") {
            heisyuName = msg.getMessage(57);
        }

		GameObject.Find ("ChildNameValue").GetComponent<Text> ().text = heisyuName;
		int newHp = hp * 100;
		GameObject.Find ("TosotsuValue").GetComponent<Text> ().text = newHp.ToString();
		int newAtk = atk * 10;
		GameObject.Find ("BuyuuValue").GetComponent<Text> ().text = newAtk.ToString();
		int newDfc = dfc * 10;
		GameObject.Find ("ChiryakuValue").GetComponent<Text> ().text = newDfc.ToString();
		GameObject.Find ("SpeedValue").GetComponent<Text> ().text = spd.ToString();
			
		//Senpou
		Entity_senpou_mst senpouMst  = Resources.Load ("Data/senpou_mst") as Entity_senpou_mst;
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            GameObject.Find("SenpouValue").GetComponent<Text>().text = senpouMst.param[senpouId - 1].nameEng;
        } else {
            GameObject.Find("SenpouValue").GetComponent<Text>().text = senpouMst.param[senpouId - 1].name;
        }
		int senpouStatus = senpouMst.param [senpouId - 1].lv20;
		int each = (int)senpouMst.param [senpouId - 1].each;
		int ratio = (int)senpouMst.param [senpouId - 1].ratio;
		int term = (int)senpouMst.param [senpouId - 1].term;
        string senpouExp = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            senpouExp = senpouMst.param [senpouId - 1].effectionEng;
        }else {
            senpouExp = senpouMst.param[senpouId - 1].effection;
        }

        if (Application.systemLanguage != SystemLanguage.Japanese) {
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


		//Saku
		Saku saku = new Saku ();
		List<string> sakuList = new List<string>();
		sakuList = saku.getSakuInfoLvMax(busyoId);
		
		string sakuPath = "Prefabs/Saku/saku" + sakuList[0];
		GameObject sakuIcon = Instantiate (Resources.Load (sakuPath)) as GameObject;
		GameObject StatusSaku = GameObject.Find("StatusSaku").gameObject;
		foreach ( Transform n in StatusSaku.transform ){
			if(n.tag == "Saku"){
				GameObject.Destroy(n.gameObject);
			}
		}
		sakuIcon.transform.SetParent (StatusSaku.transform);
		sakuIcon.transform.localScale = new Vector2 (0.85f, 0.85f);
		sakuIcon.GetComponent<Button>().enabled = false;
		RectTransform sakuIcon_transform = sakuIcon.GetComponent<RectTransform>();
		sakuIcon_transform.anchoredPosition = new Vector3(-360,0,0);
		
		StatusSaku.transform.FindChild("SakuExp").transform.FindChild("SakuExpValue").GetComponent<Text>().text = sakuList[2];


				
				



	}	
}
