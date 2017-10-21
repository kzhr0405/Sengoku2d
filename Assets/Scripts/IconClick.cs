using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class IconClick : MonoBehaviour {

	// Use this for initialization
	public void OnClick(){
		
		//Under Button
		if (name == "KuniLv") {
			//Popup
			ViewKuniInfo();
			AudioSource sound = GameObject.Find ("SEController").GetComponent<AudioSource> ();
			sound.Play ();
		}
	}

	public void IconPopup(){
		string backPath = "Prefabs/Common/Back";
		GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
		back.transform.SetParent(GameObject.Find ("Panel").transform);
		back.transform.localScale = new Vector2 (1, 1);
		RectTransform backTransform = back.GetComponent<RectTransform> ();
		backTransform.anchoredPosition = new Vector3 (0, 0, 0);
		back.name = "Back";
		
	}

	public void ViewKuniInfo(){
		string kuniPath = "Prefabs/Common/Kakejiku";
		GameObject kakejiku = Instantiate (Resources.Load (kuniPath)) as GameObject;
        kakejiku.transform.SetParent(GameObject.Find("Panel").transform);
        kakejiku.transform.localScale = new Vector2 (1, 1);
        kakejiku.name = "Kakejiku";
        GameObject kuni = kakejiku.transform.FindChild("Kakejiku").transform.FindChild("KuniInfo").gameObject;

        //Kanni
        int syogunDaimyoId = PlayerPrefs.GetInt("syogunDaimyoId");
		int myDaimyoId = PlayerPrefs.GetInt("myDaimyo");
		string kanniName = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (syogunDaimyoId == myDaimyoId) {
            if (langId == 2) {
                kanniName = "Shogun";
            }else {
                kanniName = "征夷大将軍";
            }
		} else {
			int myDaimyoBusyo = PlayerPrefs.GetInt("myDaimyoBusyo");
			string kanniTmp = "kanni" + myDaimyoBusyo.ToString ();
			if (PlayerPrefs.HasKey (kanniTmp)) {
				int kanniId = PlayerPrefs.GetInt (kanniTmp);
				Kanni kanni = new Kanni ();
				kanniName = kanni.getKanniName (kanniId);
			} else {
                if (langId == 2) {
                    kanniName = "No Royal Court Rank";
                }else { 
                    kanniName = "官位なし";
                }
            }
		}
		kuni.transform.FindChild ("KaniValue").GetComponent<Text> ().text = kanniName;

        //Daimyo Name
        Daimyo daimyoScript = new Daimyo();
        int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
        string myDaimyoName = daimyoScript.getName(myDaimyo,langId);
        kuni.transform.FindChild("DaimyoName").GetComponent<Text>().text = myDaimyoName;

        //Daimyo busyo image
        int daimyoBusyoId = daimyoScript.getDaimyoBusyoId(myDaimyo);
        string daimyoPath = "Prefabs/Player/Sprite/unit" + daimyoBusyoId.ToString();
        kuni.transform.FindChild("Mask").transform.FindChild("BusyoImage").GetComponent<Image>().sprite =
            Resources.Load(daimyoPath, typeof(Sprite)) as Sprite;



        //Current Kokuryoku
        int kuniExp = PlayerPrefs.GetInt ("kuniExp");
        //kuni.transform.FindChild ("PopStatusLv").transform.FindChild("PopLvValue").GetComponent<Text> ().text = kuniExp.ToString ();

        //Now Lv
        int nowLv = PlayerPrefs.GetInt("kuniLv");
        kuni.transform.FindChild("PopStatusLv").transform.FindChild("PopLvValue").GetComponent<Text>().text = nowLv.ToString();
        
        //Exp for Next Lv
        Exp exp = new Exp ();
		int totalExp = exp.getKuniExpforNextLv (nowLv);
		int diff = totalExp - kuniExp;
        kuni.transform.FindChild("PopStatusLv").transform.FindChild("ExpValue").GetComponent<Text> ().text = diff.ToString ();

        //Slider
        Slider sliderScript = kuni.transform.FindChild("PopStatusLv").transform.FindChild("ExpSlider").GetComponent<Slider>();
        sliderScript.maxValue = totalExp;
        int totalExpOfNowLv = 0;
        if(nowLv != 1) {
            totalExpOfNowLv = exp.getKuniExpforNextLv(nowLv - 1);
        }
        sliderScript.minValue = totalExpOfNowLv;
        sliderScript.value = kuniExp;


        //Now Kuni Qty
        string clearedKuni = PlayerPrefs.GetString ("clearedKuni");
		if (clearedKuni != null && clearedKuni != "") {
			if (clearedKuni.Contains (",")) {
				char[] delimiterChars = {','};
				string[] clearedKuniList = clearedKuni.Split (delimiterChars);
				kuni.transform.FindChild ("ShiroQtyValue").GetComponent<Text> ().text = clearedKuniList.Length.ToString ();

			} else {
				kuni.transform.FindChild ("ShiroQtyValue").GetComponent<Text> ().text = "1";
			}
		} else {
			kuni.transform.FindChild ("ShiroQtyValue").GetComponent<Text> ().text = "0";
		}

		//Syutujin Limit
		int jinkeiLimit = PlayerPrefs.GetInt ("jinkeiLimit");

		int addNo = 0;
		bool addJinkei1Flg = PlayerPrefs.GetBool ("addJinkei1");
		if (addJinkei1Flg) {
			addNo = addNo + 1;
		}
		bool addJinkei2Flg = PlayerPrefs.GetBool ("addJinkei2");
		if (addJinkei2Flg) {
			addNo = addNo + 1;
		}
		bool addJinkei3Flg = PlayerPrefs.GetBool ("addJinkei3");
		if (addJinkei3Flg) {
			addNo = addNo + 1;
		}
		bool addJinkei4Flg = PlayerPrefs.GetBool ("addJinkei4");
		if (addJinkei4Flg) {
			addNo = addNo + 1;
		}
		kuni.transform.FindChild ("SyutsujinQtyValue").GetComponent<Text> ().text = jinkeiLimit.ToString () + "<Color=#35D74BFF>+" + addNo + "</Color>";

		//Stock Limit
		int stockLimit = PlayerPrefs.GetInt ("stockLimit");
        int addSpace = PlayerPrefs.GetInt("space");
        int myBusyoQty = PlayerPrefs.GetInt ("myBusyoQty");
		string value = myBusyoQty.ToString () + "/" + stockLimit.ToString () + "<Color=#35D74BFF>+" + addSpace + "</Color>";
        kuni.transform.FindChild ("TouyouQtyValue").GetComponent<Text> ().text = value;


        //SyutsujinQtyUpLvValue
        kuni.transform.FindChild("SyutsujinQtyUpLvValue").GetComponent<Text>().text = nextAvailableSamuraiUpLv(nowLv).ToString();
        
        //SyutsujinQtyValue
        int TrackTotalKassenNo = PlayerPrefs.GetInt("TrackTotalKassenNo");
        int TrackWinNo = PlayerPrefs.GetInt("TrackWinNo");
        kuni.transform.FindChild("BattleNoValue").GetComponent<Text>().text = TrackTotalKassenNo.ToString();

        //WinRatioValue        
        int winRatio = Mathf.CeilToInt((float)TrackWinNo / (float)TrackTotalKassenNo * 100);
        if (TrackWinNo == 0) {
            winRatio = 0;
        }
        kuni.transform.FindChild("WinRatioValue").GetComponent<Text>().text = winRatio.ToString();

        //DevNo
        int HstNanbansen = PlayerPrefs.GetInt("HstNanbansen");
        kuni.transform.FindChild("ShipNo").GetComponent<Text>().text = HstNanbansen.ToString();
        //VisitorNo
        int TrackTabibitoNo = PlayerPrefs.GetInt("TrackTabibitoNo");
        kuni.transform.FindChild("VisitorNo").GetComponent<Text>().text = TrackTabibitoNo.ToString();
        //BouryakuNo
        int TrackBouryakuNo = PlayerPrefs.GetInt("TrackBouryakuNo");
        kuni.transform.FindChild("BouryakuNo").GetComponent<Text>().text = TrackBouryakuNo.ToString();
        //GaikoNo
        int TrackGaikouNo = PlayerPrefs.GetInt("TrackGaikouNo");
        kuni.transform.FindChild("GaikoNo").GetComponent<Text>().text = TrackGaikouNo.ToString();
    }


    public int nextAvailableSamuraiUpLv(int currentLv) {

        int nextLv = 0;
        Entity_kuni_lv_mst kuniLvMst = Resources.Load("Data/kuni_lv_mst") as Entity_kuni_lv_mst;
        int currentMax = 0;
        currentLv = currentLv - 1; //adjust line
        for (int i=currentLv; i< kuniLvMst.param.Count;i++) {
            if(i==currentLv) {
                currentMax = kuniLvMst.param[i].busyoJinkeiLimit;
            }else {
                int tmpMax = kuniLvMst.param[i].busyoJinkeiLimit;
                if(currentMax < tmpMax) {
                    nextLv = i + 1;
                    break;
                }
            }
        }
        return nextLv;
    }
}
