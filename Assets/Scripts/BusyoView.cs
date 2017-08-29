using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class BusyoView : MonoBehaviour {

	public Color OKClorBtn = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
	public Color OKClorTxt = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
	public Color NGClorBtn = new Color (133 / 255f, 133 / 255f, 80 / 255f, 255f / 255f);
	public Color NGClorTxt = new Color (90 / 255f, 90 / 255f, 40 / 255f, 255f / 255f);
    public bool jinkeiFlg;

	public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [2].Play ();

		/*Busyo View*/
		//Delete Previous
		foreach ( Transform n in GameObject.Find ("BusyoView").transform ){
			GameObject.Destroy(n.gameObject);
		}
        //Jinkei Flg
        if (jinkeiFlg) {
            string iconPath = "Prefabs/Busyo/Jinkei";
            GameObject jinkei = Instantiate(Resources.Load(iconPath)) as GameObject;
            jinkei.transform.SetParent(GameObject.Find("BusyoView").transform);
            jinkei.transform.localScale = new Vector2(0.3f, 0.3f);
            jinkei.transform.localPosition = new Vector2(220, 200);
            jinkei.name = "jinkei";
        }

        //Make New Busyo
        string busyoId;
		busyoId = this.name.Remove (0, 4);
		string path = "Prefabs/Player/Unit/BusyoUnit";
		GameObject Busyo = Instantiate (Resources.Load (path)) as GameObject;
		Busyo.name = busyoId.ToString ();
		Busyo.transform.SetParent(GameObject.Find ("BusyoView").transform);
		Busyo.transform.localScale = new Vector2 (4, 4);
		Busyo.GetComponent<DragHandler> ().enabled = false;	

		RectTransform rect_transform = Busyo.GetComponent<RectTransform>();
		rect_transform.anchoredPosition3D = new Vector3(300,200,0);
		rect_transform.sizeDelta = new Vector2( 100, 100);

        //Ship Rank
        string shipPath = "Prefabs/Busyo/ShipSts";
        GameObject ShipObj = Instantiate(Resources.Load(shipPath)) as GameObject;
        ShipObj.transform.SetParent(Busyo.transform);
        preKaisen kaisenScript = new preKaisen();
        int shipId = kaisenScript.getShipSprite(ShipObj, int.Parse(busyoId));
        ShipObj.transform.localPosition = new Vector3(-40,-40,0);
        ShipObj.transform.localScale = new Vector2(0.4f, 0.4f);
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            if (shipId==1) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "High";
            }else if(shipId==2) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "Mid";
            }else if(shipId==3) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "Low";
            }
        }else {
            if (shipId == 1) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "上";
            }else if (shipId == 2) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "中";
            }else if (shipId == 3) {
                ShipObj.transform.FindChild("Text").GetComponent<Text>().text = "下";
            }
        }
        //Text Modification
        GameObject text = Busyo.transform.FindChild ("Text").gameObject;
		text.GetComponent<Text> ().color = new Color(255,255,255,255);
		RectTransform text_transform = text.GetComponent<RectTransform>();
		text_transform.anchoredPosition3D = new Vector3 (-70,30,0);
		text_transform.sizeDelta = new Vector2( 630, 120);
		text.transform.localScale = new Vector2 (0.2f,0.2f);

		//Rank Text Modification
		GameObject rank = Busyo.transform.FindChild ("Rank").gameObject;
		RectTransform rank_transform = rank.GetComponent<RectTransform>();
		rank_transform.anchoredPosition3D = new Vector3 (20,-50,0);
		rank_transform.sizeDelta = new Vector2( 200, 200);
		rank.GetComponent<Text>().fontSize = 200;
		

		/*Busyo Status*/
		NowOnBusyo NowOnBusyoScript = GameObject.Find ("GameScene").GetComponent<NowOnBusyo> ();
		BusyoInfoGet busyoInfoGetScript = new BusyoInfoGet ();
		if (GameObject.Find ("GameScene").GetComponent<NowOnButton> ().onButton == "Ronkou") {
			int lv = PlayerPrefs.GetInt (busyoId);
			StatusGet sts = new StatusGet ();
			int hp = sts.getHp (int.Parse (busyoId), lv);
			int atk = sts.getAtk (int.Parse (busyoId), lv);
			int dfc = sts.getDfc (int.Parse (busyoId), lv);
			int spd = sts.getSpd (int.Parse (busyoId), lv);

			int adjHp = hp * 100;
			int adjAtk = atk * 10;
			int adjDfc = dfc * 10;

            //add lv
            string addLvTmp = "addlv" + busyoId.ToString();
            if (PlayerPrefs.HasKey(addLvTmp)) {
                string addLvValue = "+" + PlayerPrefs.GetString(addLvTmp);
                GameObject.Find("addLvValue").GetComponent<Text>().text = addLvValue.ToString();
            }else {
                GameObject.Find("addLvValue").GetComponent<Text>().text = "";
            }
            int maxLv = 100 + PlayerPrefs.GetInt(addLvTmp);

            GameObject.Find ("LvValue").GetComponent<Text> ().text = lv.ToString ();
			GameObject.Find ("TosotsuValue").GetComponent<Text> ().text = adjHp.ToString ();
			GameObject.Find ("BuyuuValue").GetComponent<Text> ().text = adjAtk.ToString ();
			GameObject.Find ("ChiryakuValue").GetComponent<Text> ().text = adjDfc.ToString ();
			GameObject.Find ("SpeedValue").GetComponent<Text> ().text = spd.ToString ();

			//Exp
			string expId = "exp" + busyoId.ToString ();
			string expString = "";
			int nowExp = PlayerPrefs.GetInt(expId);
			Exp exp = new Exp ();
			int requiredExp= 0;
			if (lv != maxLv) {
				requiredExp = exp.getExpforNextLv (lv);
			} else {
				requiredExp = exp.getExpLvMax(maxLv);
			}


			expString = nowExp + "/" + requiredExp;
			GameObject.Find ("ExpValue").GetComponent<Text> ().text = expString;

			//Kahou status
			KahouStatusGet kahouSts = new KahouStatusGet ();
			string[] KahouStatusArray =kahouSts.getKahouForStatus (busyoId,adjHp,adjAtk,adjDfc,spd);
			int totalBusyoHp =0;


			//Kanni
			string kanniTmp = "kanni" + busyoId;
			float addAtkByKanni = 0;
			float addHpByKanni = 0;
			float addDfcByKanni = 0;

			if (PlayerPrefs.HasKey (kanniTmp)) {
				int kanniId = PlayerPrefs.GetInt (kanniTmp);
                if (kanniId != 0) {
                    Kanni kanni = new Kanni ();
				    string kanniIkai = kanni.getIkai (kanniId);
				    string kanniName = kanni.getKanni (kanniId);
				    GameObject.Find ("StatusKanni").transform.FindChild ("Value").GetComponent<Text> ().text = kanniIkai + "\n" + kanniName;

				    //Status
				    string kanniTarget = kanni.getEffectTarget(kanniId);
				    int effect = kanni.getEffect(kanniId);
				    if(kanniTarget=="atk"){
					    addAtkByKanni = ((float)adjAtk * (float)effect)/100;
				    }else if(kanniTarget=="hp"){
					    addHpByKanni = ((float)adjHp * (float)effect)/100;
				    }else if(kanniTarget=="dfc"){
					    addDfcByKanni = ((float)adjDfc * (float)effect)/100;
				    }
                }else {
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        GameObject.Find("StatusKanni").transform.FindChild("Value").GetComponent<Text>().text = "No Rank";
                    }else {
                        GameObject.Find("StatusKanni").transform.FindChild("Value").GetComponent<Text>().text = "官位無し";
                    }
                }
            } else {
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    GameObject.Find ("StatusKanni").transform.FindChild ("Value").GetComponent<Text> ().text = "No Rank";
                }else {
                    GameObject.Find("StatusKanni").transform.FindChild("Value").GetComponent<Text>().text = "官位無し";
                }                
			}

			//Jyosyu
			string jyosyuTmp = "jyosyuBusyo" + busyoId;
			if (PlayerPrefs.HasKey (jyosyuTmp)) {
				int kuniId = PlayerPrefs.GetInt(jyosyuTmp);
				KuniInfo kuni = new KuniInfo();
				string kuniName = kuni.getKuniName(kuniId);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    GameObject.Find ("StatusJyosyu").transform.FindChild ("Value").GetComponent<Text> ().text = kuniName + "\nLord";
                }else {
                    GameObject.Find("StatusJyosyu").transform.FindChild("Value").GetComponent<Text>().text = kuniName + "\n城主";
                }
			} else {
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    GameObject.Find("StatusJyosyu").transform.FindChild("Value").GetComponent<Text>().text = "No Feud";
                }else {
                    GameObject.Find("StatusJyosyu").transform.FindChild("Value").GetComponent<Text>().text = "城無し";
                }
			}

			//Show Additional Status
			int finalAtk = int.Parse (KahouStatusArray [0]) + Mathf.FloorToInt (addAtkByKanni);
			int finalHp = int.Parse (KahouStatusArray [1]) + Mathf.FloorToInt (addHpByKanni);
			int finalDfc= int.Parse (KahouStatusArray [2]) + Mathf.FloorToInt (addDfcByKanni);
			int finalSpd = int.Parse (KahouStatusArray [3]);
			
			GameObject.Find ("KahouAtkValue").GetComponent<Text> ().text = "+" + finalAtk.ToString ();
			GameObject.Find ("KahouHpValue").GetComponent<Text>().text = "+" + finalHp.ToString();
			totalBusyoHp = adjHp + finalHp;
			GameObject.Find ("KahouDfcValue").GetComponent<Text>().text = "+" + finalDfc.ToString();
			GameObject.Find ("KahouSpdValue").GetComponent<Text>().text = "+" + finalSpd.ToString();

			//Butai Status
			string heiId = "hei" + busyoId.ToString ();
			string chParam = PlayerPrefs.GetString (heiId,"0");
            
            if(chParam == "0") {
                StatusGet statusScript = new StatusGet();
                string heisyu = statusScript.getHeisyu(int.Parse(busyoId));
                chParam = heisyu + ":1:1:1";
                PlayerPrefs.SetString(heiId, chParam);
                PlayerPrefs.Flush();
            }


            if (chParam.Contains(":")) {
			    char[] delimiterChars = {':'};
			    string[] ch_list = chParam.Split (delimiterChars);
			
			    string ch_type = ch_list [0];
			    int ch_num = int.Parse (ch_list [1]);
			    int ch_lv = int.Parse (ch_list [2]);
			    float ch_status = float.Parse (ch_list [3]);
            
			    string heisyu = "";
                Message msg = new Message();
                if (ch_type == "KB") {
                    heisyu = msg.getMessage(55);
                }else if (ch_type == "YR") {
                    heisyu = msg.getMessage(56);
                }else if (ch_type == "TP") {
                    heisyu = msg.getMessage(57);
                }else if (ch_type == "YM") {
                    heisyu = msg.getMessage(58);
                }
            
                GameObject.Find ("ChildNameValue").GetComponent<Text> ().text = heisyu;
			    GameObject.Find ("ChildQtyValue").GetComponent<Text> ().text = ch_num.ToString ();
			    GameObject.Find ("ChildLvValue").GetComponent<Text> ().text = ch_lv.ToString ();

			    //Jyosyu Handling
			    JyosyuHeiryoku jyosyuHei = new JyosyuHeiryoku ();
			    float addHei = (float)jyosyuHei.GetJyosyuHeiryoku (busyoId);
			    float hei = ch_status * 10;
                GameObject.Find("ChildHeiryokuValue").GetComponent<Text>().text = hei.ToString();
                float newHei = finalHp + addHei;
                GameObject.Find("KahouHpValue").GetComponent<Text>().text = "+" + newHei.ToString();

                int chAtkDfc = (int)sts.getChAtkDfc ((int)hei, totalBusyoHp);
			    string chAtkDfcString = chAtkDfc.ToString () + "/" + chAtkDfc.ToString (); 
			    GameObject.Find ("ChildStatusValue").GetComponent<Text> ().text = chAtkDfcString;
            

			    //Child Image
			    foreach (Transform n in GameObject.Find ("Img").transform) {
				    GameObject.Destroy (n.gameObject);
			    }
			    string chPath = "Prefabs/Player/Unit/" + ch_type;
			    GameObject chObj = Instantiate (Resources.Load (chPath)) as GameObject;
			    chObj.transform.SetParent (GameObject.Find ("Img").transform);
			    RectTransform chTransform = chObj.GetComponent<RectTransform> ();
			    chTransform.anchoredPosition3D = new Vector3 (-200, -50, 0);
			    chTransform.sizeDelta = new Vector2 (40, 40);
			    chObj.transform.localScale = new Vector2 (4, 4);
            

			    GameObject chigyo = GameObject.Find ("ButtonCyouhei");
			    if (ch_num < 20) {
				    chigyo.GetComponent<Image> ().color = OKClorBtn;
				    chigyo.transform.FindChild ("Text").GetComponent<Text> ().color = OKClorTxt;
				    chigyo.GetComponent<Button>().enabled = true;

				    chigyo.GetComponent<BusyoStatusButton> ().ch_type = ch_type;
				    chigyo.GetComponent<BusyoStatusButton> ().ch_heisyu = heisyu;
				    chigyo.GetComponent<BusyoStatusButton> ().ch_num = ch_num;
				    chigyo.GetComponent<BusyoStatusButton> ().ch_status = chAtkDfc;
				    chigyo.GetComponent<BusyoStatusButton> ().ch_hp = hei;
				    chigyo.GetComponent<BusyoStatusButton> ().pa_hp = totalBusyoHp/100;
			    } else {
				    //MAX
				    chigyo.GetComponent<Image> ().color = NGClorBtn;
				    chigyo.transform.FindChild ("Text").GetComponent<Text> ().color = NGClorTxt;
				    chigyo.GetComponent<Button>().enabled = false;
			    }
			    GameObject kunren = GameObject.Find ("ButtonKunren");
			    if (ch_lv < 100) {
				    kunren.GetComponent<Image> ().color = OKClorBtn;
				    kunren.transform.FindChild ("Text").GetComponent<Text> ().color = OKClorTxt;
				    kunren.GetComponent<Button>().enabled = true;

				    kunren.GetComponent<BusyoStatusButton> ().ch_type = ch_type;
				    kunren.GetComponent<BusyoStatusButton> ().ch_heisyu = heisyu;
				    kunren.GetComponent<BusyoStatusButton> ().ch_lv = ch_lv;
				    kunren.GetComponent<BusyoStatusButton> ().ch_num = ch_num;
				    kunren.GetComponent<BusyoStatusButton> ().ch_status = chAtkDfc;
				    kunren.GetComponent<BusyoStatusButton> ().ch_hp = hei;
				    kunren.GetComponent<BusyoStatusButton> ().pa_hp = totalBusyoHp/100;
			    } else {
				    //MAX
				    kunren.GetComponent<Image> ().color = NGClorBtn;
				    kunren.transform.FindChild ("Text").GetComponent<Text> ().color = NGClorTxt;
				    kunren.GetComponent<Button>().enabled = false;
			    }
            }

			//Parametor Setting
			NowOnBusyoScript.OnBusyo = busyoId;
			NowOnBusyoScript.OnBusyoName = busyoInfoGetScript.getName (int.Parse(busyoId));

		} else if (GameObject.Find ("GameScene").GetComponent<NowOnButton> ().onButton == "Senpou") {
			NowOnBusyoScript.OnBusyo = busyoId;
			NowOnBusyoScript.OnBusyoName = busyoInfoGetScript.getName (int.Parse(busyoId));
			SenpouScene scene = new SenpouScene ();
			scene.createSenpouStatusView (busyoId);
			scene.createSakuStatusView(busyoId);
			
		} else if (GameObject.Find ("GameScene").GetComponent<NowOnButton> ().onButton == "Kahou") {
			NowOnBusyoScript.OnBusyo = busyoId;
			NowOnBusyoScript.OnBusyoName = busyoInfoGetScript.getName (int.Parse(busyoId));
			KahouScene kahou = new KahouScene();
			kahou.createKahouStatusView(busyoId);
		} else if (GameObject.Find ("GameScene").GetComponent<NowOnButton> ().onButton == "Syogu") {
			NowOnBusyoScript.OnBusyo = busyoId;
			NowOnBusyoScript.OnBusyoName = busyoInfoGetScript.getName (int.Parse(busyoId));
			SyoguScene syogu = new SyoguScene();
			syogu.createSyoguView(busyoId);
		}
        
        

    }	
}
