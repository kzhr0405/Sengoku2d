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
        Message msg = new Message();

        //Shinobi Check
        int shinobiGe = PlayerPrefs.GetInt ("shinobiGe");
		int shinobiCyu = PlayerPrefs.GetInt ("shinobiCyu");
		int shinobiJyo = PlayerPrefs.GetInt ("shinobiJyo");

		int total = shinobiGe + shinobiCyu + shinobiJyo;
        int langId = PlayerPrefs.GetInt("langId");

        if (total == 0) {
			audioSources [4].Play ();
			msg.makeMessage (msg.getMessage(13, langId));

		}else{

			CloseBoard close = GameObject.Find ("close").GetComponent<CloseBoard> ();
			close.layer = close.layer + 1;

			int nowHyourou = PlayerPrefs.GetInt ("hyourou");
			bool hyourouOKflg = false;

			int daimyoId = GameObject.Find ("close").GetComponent<CloseBoard>().daimyoId;            

            if (name == "Cyouhou") {
				audioSources [0].Play ();

                GameObject.Find("kuniName").GetComponent<Text>().text = msg.getMessage(204, langId);
                
				OffBouryakuMenuList();
				
				string path = "Prefabs/Map/bouryaku/bouryakuObj";
				GameObject obj= Instantiate (Resources.Load (path)) as GameObject;
				obj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
				obj.transform.localScale = new Vector3 (1, 1, 1);
				GameObject btn = obj.transform.Find("DoBouryakuBtn").gameObject;
				btn.name = "DoCyouhouBtn";
                
                btn.transform.Find("Text").GetComponent<Text>().text = msg.getMessage(204, langId);
                shinobiScroll(obj, shinobiGe, shinobiCyu, shinobiJyo, btn);

				GameObject.Find ("return").GetComponent<MenuReturn> ().layer2 = obj;

			} else if (name == "Ryugen") {
				Daimyo daimyo = new Daimyo ();
				int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
				bool remain1DaimyoFlg = daimyo.checkRemain1DaimyoOnMain (myDaimyo);
				if (remain1DaimyoFlg) {
					audioSources [4].Play ();
					
					msg.makeMessage (msg.getMessage(14, langId));
                    close.layer = close.layer - 1;

                } else {
					audioSources [0].Play ();

                    GameObject.Find("kuniName").GetComponent<Text>().text = msg.getMessage(205, langId);
                    OffBouryakuMenuList ();

					string path = "Prefabs/Map/bouryaku/bouryakuObj";
					GameObject obj = Instantiate (Resources.Load (path)) as GameObject;
					obj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
					obj.transform.localScale = new Vector3 (1, 1, 1);
					GameObject btn = obj.transform.Find ("DoBouryakuBtn").gameObject;
					btn.name = "DoRyugenBtn";
                    btn.transform.Find("Text").GetComponent<Text>().text = msg.getMessage(205, langId);

                    shinobiScroll (obj, shinobiGe, shinobiCyu, shinobiJyo, btn);

					GameObject.Find ("return").GetComponent<MenuReturn> ().layer2 = obj;
				}
			} else if (name == "Goudatsu") {
				audioSources [0].Play ();
                
                GameObject.Find("kuniName").GetComponent<Text>().text = msg.getMessage(206, langId);
                OffBouryakuMenuList();
				
				string path = "Prefabs/Map/bouryaku/bouryakuObj";
				GameObject obj= Instantiate (Resources.Load (path)) as GameObject;
				obj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
				obj.transform.localScale = new Vector3 (1, 1, 1);
				GameObject btn = obj.transform.Find("DoBouryakuBtn").gameObject;
				btn.name = "DoGoudatsuBtn";
                btn.transform.Find("Text").GetComponent<Text>().text = msg.getMessage(206, langId);

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
                    GameObject.Find("kuniName").GetComponent<Text>().text = msg.getMessage(207, langId);

                    OffBouryakuMenu ();

					string path = "Prefabs/Map/bouryaku/GihouObj";
					GameObject obj = Instantiate (Resources.Load (path)) as GameObject;
					obj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
					obj.transform.localScale = new Vector3 (1, 1, 1);

					//Label
					int dstKuni = targetGunzei.GetComponent<Gunzei>().dstKuni;
					int hei = targetGunzei.GetComponent<Gunzei>().myHei;
					KuniInfo kuni = new KuniInfo();
					string kuniName = kuni.getKuniName(dstKuni,langId);
                    if (langId == 2) {
                        obj.transform.Find("GunzeiInfo").transform.Find("DaimyoNameValue").GetComponent<Text>().text = "To "+ kuniName;
                    }else if (langId == 3) {
                        obj.transform.Find("GunzeiInfo").transform.Find("DaimyoNameValue").GetComponent<Text>().text = "向" + kuniName + "进军中";
                    }else {
                        obj.transform.Find("GunzeiInfo").transform.Find("DaimyoNameValue").GetComponent<Text>().text = kuniName + "に進軍中";
                    }

                    obj.transform.Find("Heiryoku").transform.Find("HeiryokuValue").GetComponent<Text>().text = hei.ToString();

					GameObject btn = obj.transform.Find("DoGihouBtn").gameObject;

					shinobiScroll(obj, shinobiGe, shinobiCyu, shinobiJyo, btn);

					GameObject.Find ("return").GetComponent<MenuReturn> ().layer2 = obj;

				}else{
					audioSources [4].Play ();                    
					msg.makeMessage (msg.getMessage(15, langId));
					close.layer = close.layer - 1;
				}
			}
		}
	}


	public void makeUnderObject(string btnName, int langId){
		string path = "Prefabs/Map/bouryaku/bouryakuObj";
		GameObject obj= Instantiate (Resources.Load (path)) as GameObject;
		obj.transform.SetParent (GameObject.Find ("smallBoard(Clone)").transform);
		obj.transform.localScale = new Vector3 (1, 1, 1);
		GameObject btn = obj.transform.Find("DoBouryakuBtn").gameObject;
		btn.name = btnName;
        Message msg = new Message();
		if (btnName == "DoRyugenBtn") {
            btn.transform.Find("Text").GetComponent<Text>().text = msg.getMessage(205, langId);            
		}
	}

	
	public void shinobiScroll(GameObject obj, int shinobiGe, int shinobiCyu, int shinobiJyo, GameObject btn){
		string slotPath = "Prefabs/Map/common/ShinobiSlot";
		string shinobiItemPath = "Prefabs/Item/Shinobi/Shinobi";
        Message msg = new Message();
        GameObject content = obj.transform.Find("ScrollView").transform.Find("Content").gameObject;
		bool clickFlg = false;
        int langId = PlayerPrefs.GetInt("langId");
        if (shinobiGe!=0){
			GameObject slot = Instantiate (Resources.Load (slotPath)) as GameObject;			
			slot.transform.SetParent (content.transform);
			slot.transform.localScale = new Vector3 (1, 1, 1);
			slot.name = "Ge";
			
			GameObject shinobi = Instantiate (Resources.Load (shinobiItemPath)) as GameObject;	
			shinobi.transform.SetParent (slot.transform,false);
			shinobi.transform.localScale = new Vector3 (1, 1, 1);
			
			Color lowColor = new Color (0f / 255f, 0f / 255f, 219f / 255f, 255f / 255f);
			shinobi.GetComponent<Image>().color = lowColor;
            shinobi.transform.Find("ShinobiRank").GetComponent<Text>().text = msg.getMessage(181, langId);            
			shinobi.transform.Find("Qty").GetComponent<Text>().text = shinobiGe.ToString();

            RectTransform shinobiTransform = shinobi.transform.Find("Shinobi").GetComponent<RectTransform> ();
            shinobiTransform.sizeDelta = new Vector2( 155, 190);

            GameObject Qty = shinobi.transform.Find("Qty").gameObject;
            Qty.GetComponent<Text>().fontSize = 150;
            Qty.transform.localPosition = new Vector2(-40,-60);

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
            shinobi.transform.Find("ShinobiRank").GetComponent<Text>().text = msg.getMessage(182, langId);
            
            shinobi.transform.Find("Qty").GetComponent<Text>().text = shinobiCyu.ToString();
			
			RectTransform shinobiTransform = shinobi.transform.Find("Shinobi").GetComponent<RectTransform> ();
			shinobiTransform.sizeDelta = new Vector2( 155, 190);
            GameObject Qty = shinobi.transform.Find("Qty").gameObject;
            Qty.GetComponent<Text>().fontSize = 150;
            Qty.transform.localPosition = new Vector2(-40, -60);

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
            shinobi.transform.Find("ShinobiRank").GetComponent<Text>().text = msg.getMessage(183, langId);
            
            shinobi.transform.Find("Qty").GetComponent<Text>().text = shinobiJyo.ToString();
			
			RectTransform shinobiTransform = shinobi.transform.Find("Shinobi").GetComponent<RectTransform> ();
			shinobiTransform.sizeDelta = new Vector2( 155, 190);
            GameObject Qty = shinobi.transform.Find("Qty").gameObject;
            Qty.GetComponent<Text>().fontSize = 150;
            Qty.transform.localPosition = new Vector2(-40, -60);

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
				n.Find("Text").GetComponent<Text>().enabled = false;
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
		GameObject CyouhouBtn = newMenu.transform.Find ("Cyouhou").gameObject;
		GameObject RyugenBtn = newMenu.transform.Find ("Ryugen").gameObject;
		GameObject GoudatsuBtn = newMenu.transform.Find ("Goudatsu").gameObject;
		GameObject GihouBtn = newMenu.transform.Find ("Gihou").gameObject;
		
		CyouhouBtn.GetComponent<Image> ().enabled = false;
		CyouhouBtn.GetComponent<Button> ().enabled = false;
		CyouhouBtn.transform.Find ("Text").GetComponent<Text> ().enabled = false;
		
		RyugenBtn.GetComponent<Image> ().enabled = false;
		RyugenBtn.GetComponent<Button> ().enabled = false;
		RyugenBtn.transform.Find ("Text").GetComponent<Text> ().enabled = false;
		
		GoudatsuBtn.GetComponent<Image> ().enabled = false;
		GoudatsuBtn.GetComponent<Button> ().enabled = false;
		GoudatsuBtn.transform.Find ("Text").GetComponent<Text> ().enabled = false;
		
		GihouBtn.GetComponent<Image> ().enabled = false;
		GihouBtn.GetComponent<Button> ().enabled = false;
		GihouBtn.transform.Find ("Text").GetComponent<Text> ().enabled = false;

		
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
				n.Find("Text").GetComponent<Text>().enabled = true;
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
		GameObject CyouhouBtn = newMenu.transform.Find ("Cyouhou").gameObject;
		GameObject RyugenBtn = newMenu.transform.Find ("Ryugen").gameObject;
		GameObject GoudatsuBtn = newMenu.transform.Find ("Goudatsu").gameObject;
		GameObject GihouBtn = newMenu.transform.Find ("Gihou").gameObject;
		
		CyouhouBtn.GetComponent<Image> ().enabled = true;
		CyouhouBtn.GetComponent<Button> ().enabled = true;
		CyouhouBtn.transform.Find ("Text").GetComponent<Text> ().enabled = true;
		
		RyugenBtn.GetComponent<Image> ().enabled = true;
		RyugenBtn.GetComponent<Button> ().enabled = true;
		RyugenBtn.transform.Find ("Text").GetComponent<Text> ().enabled = true;
		
		GoudatsuBtn.GetComponent<Image> ().enabled = true;
		GoudatsuBtn.GetComponent<Button> ().enabled = true;
		GoudatsuBtn.transform.Find ("Text").GetComponent<Text> ().enabled = true	;
		
		GihouBtn.GetComponent<Image> ().enabled = true;
		GihouBtn.GetComponent<Button> ().enabled = true;
		GihouBtn.transform.Find ("Text").GetComponent<Text> ().enabled = true;
		
		
		
	}

	public void OffBouryakuMenuList(){

		GameObject newMenu = GameObject.Find ("return").GetComponent<MenuReturn>().NewMenu;
		GameObject CyouhouBtn = newMenu.transform.Find ("Cyouhou").gameObject;
		GameObject RyugenBtn = newMenu.transform.Find ("Ryugen").gameObject;
		GameObject GoudatsuBtn = newMenu.transform.Find ("Goudatsu").gameObject;
		GameObject GihouBtn = newMenu.transform.Find ("Gihou").gameObject;
		
		CyouhouBtn.GetComponent<Image> ().enabled = false;
		CyouhouBtn.GetComponent<Button> ().enabled = false;
		CyouhouBtn.transform.Find ("Text").GetComponent<Text> ().enabled = false;
		
		RyugenBtn.GetComponent<Image> ().enabled = false;
		RyugenBtn.GetComponent<Button> ().enabled = false;
		RyugenBtn.transform.Find ("Text").GetComponent<Text> ().enabled = false;
		
		GoudatsuBtn.GetComponent<Image> ().enabled = false;
		GoudatsuBtn.GetComponent<Button> ().enabled = false;
		GoudatsuBtn.transform.Find ("Text").GetComponent<Text> ().enabled = false;
		
		GihouBtn.GetComponent<Image> ().enabled = false;
		GihouBtn.GetComponent<Button> ().enabled = false;
		GihouBtn.transform.Find ("Text").GetComponent<Text> ().enabled = false;
		
		
	}


}
