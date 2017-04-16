using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class BouryakuMenu : MonoBehaviour {

	public GameObject targetGunzei;

	// Use this for initialization
	public void OnClick () {

		//SE
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();


		//Shinobi Check
		int shinobiGe = PlayerPrefs.GetInt ("shinobiGe");
		int shinobiCyu = PlayerPrefs.GetInt ("shinobiCyu");
		int shinobiJyo = PlayerPrefs.GetInt ("shinobiJyo");

		int total = shinobiGe + shinobiCyu + shinobiJyo;

		if (total == 0) {
			audioSources [4].Play ();
			Message msg = new Message ();
			msg.makeMessage (msg.getMessage(13));

		}else{

			CloseBoard close = GameObject.Find ("close").GetComponent<CloseBoard> ();
			close.layer = close.layer + 1;

			int nowHyourou = PlayerPrefs.GetInt ("hyourou");
			bool hyourouOKflg = false;

			int daimyoId = GameObject.Find ("close").GetComponent<CloseBoard>().daimyoId;


			if (name == "Cyouhou") {
				audioSources [0].Play ();
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    GameObject.Find ("kuniName").GetComponent<Text> ().text = "Spy";
                }else {
                    GameObject.Find("kuniName").GetComponent<Text>().text = "諜報";
                }
				OffBouryakuMenuList();
				
				string path = "Prefabs/Map/bouryaku/bouryakuObj";
				GameObject obj= Instantiate (Resources.Load (path)) as GameObject;
				obj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
				obj.transform.localScale = new Vector3 (1, 1, 1);
				GameObject btn = obj.transform.FindChild("DoBouryakuBtn").gameObject;
				btn.name = "DoCyouhouBtn";
				
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    btn.transform.FindChild("Text").GetComponent<Text>().text = "Spy";
                }
                else {
                    btn.transform.FindChild("Text").GetComponent<Text>().text = "諜報";
                }
                shinobiScroll(obj, shinobiGe, shinobiCyu, shinobiJyo, btn);

				GameObject.Find ("return").GetComponent<MenuReturn> ().layer2 = obj;

			} else if (name == "Ryugen") {
				Daimyo daimyo = new Daimyo ();
				int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
				bool remain1DaimyoFlg = daimyo.checkRemain1DaimyoOnMain (myDaimyo);
				if (remain1DaimyoFlg) {
					audioSources [4].Play ();

					Message msg = new Message ();
					msg.makeMessage (msg.getMessage(14));

				} else {
					audioSources [0].Play ();

					
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        GameObject.Find("kuniName").GetComponent<Text>().text = "Bad Rumor";
                    }else {
                        GameObject.Find("kuniName").GetComponent<Text>().text = "流言";
                    }
                    OffBouryakuMenuList ();

					string path = "Prefabs/Map/bouryaku/bouryakuObj";
					GameObject obj = Instantiate (Resources.Load (path)) as GameObject;
					obj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
					obj.transform.localScale = new Vector3 (1, 1, 1);
					GameObject btn = obj.transform.FindChild ("DoBouryakuBtn").gameObject;
					btn.name = "DoRyugenBtn";
					
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        btn.transform.FindChild("Text").GetComponent<Text>().text = "Bad Rumor";
                    }
                    else {
                        btn.transform.FindChild("Text").GetComponent<Text>().text = "流言";
                    }
                    shinobiScroll (obj, shinobiGe, shinobiCyu, shinobiJyo, btn);

					GameObject.Find ("return").GetComponent<MenuReturn> ().layer2 = obj;
				}
			} else if (name == "Goudatsu") {
				audioSources [0].Play ();

				
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    GameObject.Find("kuniName").GetComponent<Text>().text = "Theft";
                }
                else {
                    GameObject.Find("kuniName").GetComponent<Text>().text = "強奪";
                }
                OffBouryakuMenuList();
				
				string path = "Prefabs/Map/bouryaku/bouryakuObj";
				GameObject obj= Instantiate (Resources.Load (path)) as GameObject;
				obj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
				obj.transform.localScale = new Vector3 (1, 1, 1);
				GameObject btn = obj.transform.FindChild("DoBouryakuBtn").gameObject;
				btn.name = "DoGoudatsuBtn";
				
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    btn.transform.FindChild("Text").GetComponent<Text>().text = "Theft";
                }
                else {
                    btn.transform.FindChild("Text").GetComponent<Text>().text = "強奪";
                }
                shinobiScroll(obj, shinobiGe, shinobiCyu, shinobiJyo, btn);

				GameObject.Find ("return").GetComponent<MenuReturn> ().layer2 = obj;
			
			} else if (name == "Gihou") {
				//Gunzei Check
				bool gunzeiFlg = false;
				foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Gunzei")){
					int checkDaimyoId = obs.GetComponent<Gunzei>().srcDaimyoId;
					if(checkDaimyoId == daimyoId){
						gunzeiFlg = true;
						targetGunzei = obs;
						break;
					}
				}

				if(gunzeiFlg){
					audioSources [0].Play ();

					//Menu Handling
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        GameObject.Find("kuniName").GetComponent<Text>().text = "Misreport";
                    }
                    else {
                        GameObject.Find("kuniName").GetComponent<Text>().text = "偽報";
                    }
                    OffBouryakuMenu ();

					string path = "Prefabs/Map/bouryaku/GihouObj";
					GameObject obj = Instantiate (Resources.Load (path)) as GameObject;
					obj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
					obj.transform.localScale = new Vector3 (1, 1, 1);

					//Label
					int dstKuni = targetGunzei.GetComponent<Gunzei>().dstKuni;
					int hei = targetGunzei.GetComponent<Gunzei>().myHei;
					KuniInfo kuni = new KuniInfo();
					string kuniName = kuni.getKuniName(dstKuni); if (Application.systemLanguage != SystemLanguage.Japanese) {
                        obj.transform.FindChild("GunzeiInfo").transform.FindChild("DaimyoNameValue").GetComponent<Text>().text = "To "+ kuniName;
                    }else {
                        obj.transform.FindChild("GunzeiInfo").transform.FindChild("DaimyoNameValue").GetComponent<Text>().text = kuniName + "に進軍中";
                    }

                    obj.transform.FindChild("Heiryoku").transform.FindChild("HeiryokuValue").GetComponent<Text>().text = hei.ToString();

					GameObject btn = obj.transform.FindChild("DoGihouBtn").gameObject;

					shinobiScroll(obj, shinobiGe, shinobiCyu, shinobiJyo, btn);

					GameObject.Find ("return").GetComponent<MenuReturn> ().layer2 = obj;

				}else{
					audioSources [4].Play ();

					Message msg = new Message();
					msg.makeMessage (msg.getMessage(15));
					close.layer = close.layer - 1;
				}
			}
		}
	}


	public void makeUnderObject(string btnName){
		string path = "Prefabs/Map/bouryaku/bouryakuObj";
		GameObject obj= Instantiate (Resources.Load (path)) as GameObject;
		obj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
		obj.transform.localScale = new Vector3 (1, 1, 1);
		GameObject btn = obj.transform.FindChild("DoBouryakuBtn").gameObject;
		btn.name = btnName;

		if (btnName == "DoRyugenBtn") {
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                btn.transform.FindChild ("Text").GetComponent<Text> ().text = "Misreport";
            }else {
                btn.transform.FindChild("Text").GetComponent<Text>().text = "流言";
            }
		}
	}

	
	public void shinobiScroll(GameObject obj, int shinobiGe, int shinobiCyu, int shinobiJyo, GameObject btn){
		string slotPath = "Prefabs/Map/common/ShinobiSlot";
		string shinobiItemPath = "Prefabs/Item/Shinobi/Shinobi";
		
		GameObject content = obj.transform.FindChild("ScrollView").transform.FindChild("Content").gameObject;
		bool clickFlg = false;

		if(shinobiGe!=0){
			GameObject slot = Instantiate (Resources.Load (slotPath)) as GameObject;			
			slot.transform.SetParent (content.transform);
			slot.transform.localScale = new Vector3 (1, 1, 1);
			slot.name = "Ge";
			
			GameObject shinobi = Instantiate (Resources.Load (shinobiItemPath)) as GameObject;	
			shinobi.transform.SetParent (slot.transform);
			shinobi.transform.localScale = new Vector3 (1, 1, 1);
			
			Color lowColor = new Color (0f / 255f, 0f / 255f, 219f / 255f, 255f / 255f);
			shinobi.GetComponent<Image>().color = lowColor;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                shinobi.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "Low";
            } else {
                shinobi.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "下";
            }
			shinobi.transform.FindChild("Qty").GetComponent<Text>().text = shinobiGe.ToString();
			
			RectTransform shinobiTransform = shinobi.transform.FindChild("Shinobi").GetComponent<RectTransform> ();
			shinobiTransform.sizeDelta = new Vector2( 155, 190);
			RectTransform qtyTransform = shinobi.transform.FindChild("Qty").GetComponent<RectTransform> ();
			qtyTransform.anchoredPosition = new Vector3 (-50, -65, 0);
			
			slot.GetComponent<GaikouShinobiSelect>().Gunzei = targetGunzei;
			slot.GetComponent<GaikouShinobiSelect>().Content = content;
			slot.GetComponent<GaikouShinobiSelect>().DoBtn = btn;
			slot.GetComponent<GaikouShinobiSelect>().qty = shinobiGe;

			slot.GetComponent<GaikouShinobiSelect>().OnClick();
			clickFlg=true;
		}
		
		if(shinobiCyu!=0){
			GameObject slot = Instantiate (Resources.Load (slotPath)) as GameObject;			
			slot.transform.SetParent (content.transform);
			slot.transform.localScale = new Vector3 (1, 1, 1);
			slot.name = "Cyu";
			
			GameObject shinobi = Instantiate (Resources.Load (shinobiItemPath)) as GameObject;	
			shinobi.transform.SetParent (slot.transform);
			shinobi.transform.localScale = new Vector3 (1, 1, 1);
			
			Color midColor = new Color (94f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
			shinobi.GetComponent<Image>().color = midColor;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                shinobi.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "Mid";
            }
            else {
                shinobi.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "中";
            }
            shinobi.transform.FindChild("Qty").GetComponent<Text>().text = shinobiCyu.ToString();
			
			RectTransform shinobiTransform = shinobi.transform.FindChild("Shinobi").GetComponent<RectTransform> ();
			shinobiTransform.sizeDelta = new Vector2( 155, 190);
			RectTransform qtyTransform = shinobi.transform.FindChild("Qty").GetComponent<RectTransform> ();
			qtyTransform.anchoredPosition = new Vector3 (-50, -65, 0);
			
			slot.GetComponent<GaikouShinobiSelect>().Gunzei = targetGunzei;
			slot.GetComponent<GaikouShinobiSelect>().Content = content;
			slot.GetComponent<GaikouShinobiSelect>().DoBtn = btn;
			slot.GetComponent<GaikouShinobiSelect>().qty = shinobiCyu;

			if(!clickFlg){
				slot.GetComponent<GaikouShinobiSelect>().OnClick();
				clickFlg=true;
			}
		}
		
		if(shinobiJyo!=0){
			GameObject slot = Instantiate (Resources.Load (slotPath)) as GameObject;			
			slot.transform.SetParent (content.transform);
			slot.transform.localScale = new Vector3 (1, 1, 1);
			slot.name = "Jyo";
			
			GameObject shinobi = Instantiate (Resources.Load (shinobiItemPath)) as GameObject;	
			shinobi.transform.SetParent (slot.transform);
			shinobi.transform.localScale = new Vector3 (1, 1, 1);
			
			Color highColor = new Color (84f / 255f, 103f / 255f, 0f / 255f, 255f / 255f);
			shinobi.GetComponent<Image>().color = highColor;
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                shinobi.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "High";
            }
            else {
                shinobi.transform.FindChild("ShinobiRank").GetComponent<Text>().text = "上";
            }
            shinobi.transform.FindChild("Qty").GetComponent<Text>().text = shinobiJyo.ToString();
			
			RectTransform shinobiTransform = shinobi.transform.FindChild("Shinobi").GetComponent<RectTransform> ();
			shinobiTransform.sizeDelta = new Vector2( 155, 190);
			RectTransform qtyTransform = shinobi.transform.FindChild("Qty").GetComponent<RectTransform> ();
			qtyTransform.anchoredPosition = new Vector3 (-50, -65, 0);
			
			slot.GetComponent<GaikouShinobiSelect>().Gunzei = targetGunzei;
			slot.GetComponent<GaikouShinobiSelect>().Content = content;
			slot.GetComponent<GaikouShinobiSelect>().DoBtn = btn;
			slot.GetComponent<GaikouShinobiSelect>().qty = shinobiJyo;

			if(!clickFlg){
				slot.GetComponent<GaikouShinobiSelect>().OnClick();
				clickFlg=true;
			}
		}



	}


	public void OffBouryakuMenu(){
		
		GameObject kamon = GameObject.Find ("KamonBack").gameObject;
		GameObject daimyoName = GameObject.Find ("DaimyoName").gameObject;
		GameObject Heiryoku = GameObject.Find ("Heiryoku").gameObject;
		GameObject Yukoudo = GameObject.Find ("Yukoudo").gameObject;
		
		kamon.GetComponent<Image> ().enabled = false;
		daimyoName.GetComponent<Image> ().enabled = false;
		Heiryoku.GetComponent<Image> ().enabled = false;
		Yukoudo.GetComponent<Image> ().enabled = false;
		
		foreach (Transform n in kamon.transform) {
			n.gameObject.GetComponent<Image>().enabled = false;
			if(n.name == "Doumei"){
				n.FindChild("Text").GetComponent<Text>().enabled = false;
			}
		}
		foreach (Transform n in daimyoName.transform) {
			n.gameObject.GetComponent<Text>().enabled = false;
		}
		foreach (Transform n in Heiryoku.transform) {
			n.gameObject.GetComponent<Text>().enabled = false;
		}
		foreach (Transform n in Yukoudo.transform) {
			n.gameObject.GetComponent<Text>().enabled = false;
		}
		
		GameObject newMenu = GameObject.Find ("return").GetComponent<MenuReturn>().NewMenu;
		GameObject CyouhouBtn = newMenu.transform.FindChild ("Cyouhou").gameObject;
		GameObject RyugenBtn = newMenu.transform.FindChild ("Ryugen").gameObject;
		GameObject GoudatsuBtn = newMenu.transform.FindChild ("Goudatsu").gameObject;
		GameObject GihouBtn = newMenu.transform.FindChild ("Gihou").gameObject;
		
		CyouhouBtn.GetComponent<Image> ().enabled = false;
		CyouhouBtn.GetComponent<Button> ().enabled = false;
		CyouhouBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
		
		RyugenBtn.GetComponent<Image> ().enabled = false;
		RyugenBtn.GetComponent<Button> ().enabled = false;
		RyugenBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
		
		GoudatsuBtn.GetComponent<Image> ().enabled = false;
		GoudatsuBtn.GetComponent<Button> ().enabled = false;
		GoudatsuBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
		
		GihouBtn.GetComponent<Image> ().enabled = false;
		GihouBtn.GetComponent<Button> ().enabled = false;
		GihouBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;

		
	}
	
	
	public void OnBouryakuMenu(){
		
		GameObject kamon = GameObject.Find ("KamonBack").gameObject;
		GameObject daimyoName = GameObject.Find ("DaimyoName").gameObject;
		GameObject Heiryoku = GameObject.Find ("Heiryoku").gameObject;
		GameObject Yukoudo = GameObject.Find ("Yukoudo").gameObject;
		
		kamon.GetComponent<Image> ().enabled = true;
		daimyoName.GetComponent<Image> ().enabled = true;
		Heiryoku.GetComponent<Image> ().enabled = true;
		Yukoudo.GetComponent<Image> ().enabled = true;
		
		foreach (Transform n in kamon.transform) {
			n.gameObject.GetComponent<Image>().enabled = true;
			if(n.name == "Doumei"){
				n.FindChild("Text").GetComponent<Text>().enabled = true;
			}
		}
		foreach (Transform n in daimyoName.transform) {
			n.gameObject.GetComponent<Text>().enabled = true;
		}
		foreach (Transform n in Heiryoku.transform) {
			n.gameObject.GetComponent<Text>().enabled = true;
		}
		foreach (Transform n in Yukoudo.transform) {
			n.gameObject.GetComponent<Text>().enabled = true;
		}
		
		GameObject newMenu = GameObject.Find ("return").GetComponent<MenuReturn>().NewMenu;
		GameObject CyouhouBtn = newMenu.transform.FindChild ("Cyouhou").gameObject;
		GameObject RyugenBtn = newMenu.transform.FindChild ("Ryugen").gameObject;
		GameObject GoudatsuBtn = newMenu.transform.FindChild ("Goudatsu").gameObject;
		GameObject GihouBtn = newMenu.transform.FindChild ("Gihou").gameObject;
		
		CyouhouBtn.GetComponent<Image> ().enabled = true;
		CyouhouBtn.GetComponent<Button> ().enabled = true;
		CyouhouBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
		
		RyugenBtn.GetComponent<Image> ().enabled = true;
		RyugenBtn.GetComponent<Button> ().enabled = true;
		RyugenBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
		
		GoudatsuBtn.GetComponent<Image> ().enabled = true;
		GoudatsuBtn.GetComponent<Button> ().enabled = true;
		GoudatsuBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = true	;
		
		GihouBtn.GetComponent<Image> ().enabled = true;
		GihouBtn.GetComponent<Button> ().enabled = true;
		GihouBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = true;
		
		
		
	}

	public void OffBouryakuMenuList(){

		GameObject newMenu = GameObject.Find ("return").GetComponent<MenuReturn>().NewMenu;
		GameObject CyouhouBtn = newMenu.transform.FindChild ("Cyouhou").gameObject;
		GameObject RyugenBtn = newMenu.transform.FindChild ("Ryugen").gameObject;
		GameObject GoudatsuBtn = newMenu.transform.FindChild ("Goudatsu").gameObject;
		GameObject GihouBtn = newMenu.transform.FindChild ("Gihou").gameObject;
		
		CyouhouBtn.GetComponent<Image> ().enabled = false;
		CyouhouBtn.GetComponent<Button> ().enabled = false;
		CyouhouBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
		
		RyugenBtn.GetComponent<Image> ().enabled = false;
		RyugenBtn.GetComponent<Button> ().enabled = false;
		RyugenBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
		
		GoudatsuBtn.GetComponent<Image> ().enabled = false;
		GoudatsuBtn.GetComponent<Button> ().enabled = false;
		GoudatsuBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
		
		GihouBtn.GetComponent<Image> ().enabled = false;
		GihouBtn.GetComponent<Button> ().enabled = false;
		GihouBtn.transform.FindChild ("Text").GetComponent<Text> ().enabled = false;
		
		
	}


}
