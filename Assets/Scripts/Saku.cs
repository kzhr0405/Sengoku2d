using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System;
using UnityEngine.UI;
using System.Reflection;

public class Saku : MonoBehaviour {

	public int sakuId;
	public int sakuEffect;
	public string sakuHeisyu;
	public float sakuHeiSts;
	public int sakuBusyoId;
	public float sakuBusyoSpeed;
	public int kengouQty;
	public string kengouCd;
	public string kengouName;
    public GameObject Content;
    public bool selectFlg;

    private void Start() {
        if(GameObject.Find("ScrollView")) {
            Content = GameObject.Find("ScrollView").transform.FindChild("Content").gameObject;
        }
    }


	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [2].Play ();

        Color Select = new Color(76f / 255f, 50f / 255f, 18f / 255f, 80f / 255f);
        Color unSelect = new Color(255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
        if (selectFlg) {
            //Cancel
            GetComponent<Image>().color = unSelect;
            selectFlg = false;

            if (Application.loadedLevelName == "kaisen") {
                KaisenScene KaisenScene = GameObject.Find("GameScene").GetComponent<KaisenScene>();
                KaisenScene.sakuId = 0;
                KaisenScene.sakuEffect = 0;
                KaisenScene.sakuFlg = false;
                KaisenScene.sakuBtn = null;
            } else {
                GameScene GameScene = GameObject.Find("GameScene").GetComponent<GameScene>();
                GameScene.sakuId = 0;
                GameScene.sakuEffect = 0;
                GameScene.sakuFlg = false;
                GameScene.sakuBtn = null;
            }

        }else { 
            //Color Reset
            foreach (Transform obj in Content.transform) {
                obj.GetComponent<Image>().color = unSelect;
            }
            GetComponent<Image>().color = Select;
            selectFlg = true;


            if (Application.loadedLevelName == "kaisen") {
                KaisenScene KaisenScene = GameObject.Find("GameScene").GetComponent<KaisenScene>();
                KaisenScene.sakuId = sakuId;
                KaisenScene.sakuEffect = sakuEffect;
                KaisenScene.sakuFlg = true;
                KaisenScene.sakuBtn = gameObject;

                if (sakuId == 3) {
                    KaisenScene.sakuHeisyu = sakuHeisyu;
                    KaisenScene.sakuHeiSts = sakuHeiSts;
                    KaisenScene.sakuBusyoId = sakuBusyoId;
                    KaisenScene.sakuBusyoSpeed = sakuBusyoSpeed;
                }


            } else {
                GameScene GameScene = GameObject.Find("GameScene").GetComponent<GameScene>();
                GameScene.sakuId = sakuId;
                GameScene.sakuEffect = sakuEffect;
                GameScene.sakuFlg = true;
                GameScene.sakuBtn = gameObject;

                if (sakuId == 3) {
                    GameScene.sakuHeisyu = sakuHeisyu;
                    GameScene.sakuHeiSts = sakuHeiSts;
                    GameScene.sakuBusyoId = sakuBusyoId;
                    GameScene.sakuBusyoSpeed = sakuBusyoSpeed;
                }

                if (sakuId == 7) {
                    GameScene.kengouQty = kengouQty;
                    GameScene.kengouCd = kengouCd;
                    GameScene.kengouName = kengouName;
                }

                if (sakuId == 10) {
                    GameScene.sakuHeiSts = sakuHeiSts;
                    GameScene.sakuBusyoId = sakuBusyoId;
                    GameScene.sakuBusyoSpeed = sakuBusyoSpeed;
                }
            }
        }
	}






	public List<string> getSakuInfo(int busyoId){
		List<string> sakuList = new List<string>();

		Entity_busyo_mst busyoMst = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
		int sakuId = busyoMst.param [busyoId - 1].saku_id;
		string temp = "saku" + busyoId.ToString ();
		int sakuLv = PlayerPrefs.GetInt(temp,0);
        if (sakuLv == 0) {
            sakuLv = 1;
            PlayerPrefs.SetInt(temp, 1);
            PlayerPrefs.Flush();
        }
        if (sakuLv > 20) {
            sakuLv = 20;
            PlayerPrefs.SetInt(temp, sakuLv);
            PlayerPrefs.Flush();
        }

        //Get Saku Status
        Entity_saku_mst sakuMst = Resources.Load ("Data/saku_mst") as Entity_saku_mst;
		object sakulst = sakuMst.param[sakuId-1];
		Type t = sakulst.GetType();
		String param = "lv" + sakuLv;
        FieldInfo f = t.GetField(param);
		int sakuStatus =(int)f.GetValue(sakulst);
        string effection = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            effection = sakuMst.param [sakuId - 1].effectionEng;
        }else {
            effection = sakuMst.param[sakuId - 1].effection;
        }


        if (Application.loadedLevelName != "touyou" && Application.loadedLevelName != "tutorialTouyou") {
            //Kahou Adjustment
            string kahouTemp = "kahou" + busyoId;
			string busyoKahou = PlayerPrefs.GetString (kahouTemp);
			char[] delimiterChars = { ',' };
			string[] busyoKahouList = busyoKahou.Split (delimiterChars);

            Entity_kahou_chishikisyo_mst Mst = Resources.Load("Data/kahou_chishikisyo_mst") as Entity_kahou_chishikisyo_mst;
            for (int i = 0; i < busyoKahouList.Length; i++) {
				if (i == 7) {
					int kahouId = int.Parse (busyoKahouList [i]);
					if (kahouId != 0) {					
						//Saku
						float calcSenpou = ((float)sakuStatus * (float)Mst.param [kahouId - 1].kahouEffect) / 100;
						sakuStatus = Mathf.CeilToInt ((float)sakuStatus + calcSenpou);

					}
				}
			}
		}
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            effection = effection.Replace("ABC", sakuStatus.ToString());
        }else {
            effection = effection.Replace("A", sakuStatus.ToString());
        }
		//sakuList.Add(
		sakuList.Add (sakuId.ToString());
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            sakuList.Add (sakuMst.param [sakuId - 1].nameEng); //Type		    
        }else {
            sakuList.Add(sakuMst.param[sakuId - 1].name); //Type
        }
        sakuList.Add(effection);
        sakuList.Add (sakuLv.ToString());
		sakuList.Add (sakuStatus.ToString());

		return sakuList;
		
	}

	public List<string> getSakuInfoForLabel(int busyoId){
		List<string> sakuList = new List<string>();

		Entity_busyo_mst busyoMst = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
		int sakuId = busyoMst.param [busyoId - 1].saku_id;
		string temp = "saku" + busyoId.ToString ();
		int sakuLv = PlayerPrefs.GetInt(temp,0);
        if (sakuLv == 0) {
            sakuLv = 1;
            PlayerPrefs.SetInt(temp, 1);
            PlayerPrefs.Flush();
        }

        //Get Saku Status
        Entity_saku_mst sakuMst = Resources.Load ("Data/saku_mst") as Entity_saku_mst;
		object sakulst = sakuMst.param[sakuId-1];
		Type t = sakulst.GetType();
		String param = "lv" + sakuLv;
		FieldInfo f = t.GetField(param);
		int sakuStatus =(int)f.GetValue(sakulst);
		float addStatus = 0;
        string effection = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            effection = sakuMst.param [sakuId - 1].effectionEng;
        }else {
            effection = sakuMst.param[sakuId - 1].effection;
        }

		//Kahou Adjustment
		string kahouTemp = "kahou" + busyoId;
		string busyoKahou = PlayerPrefs.GetString (kahouTemp);
		char[] delimiterChars = {','};
		string[] busyoKahouList = busyoKahou.Split (delimiterChars);

		for(int i=0;i<busyoKahouList.Length;i++){
			if(i==7){
				int kahouId = int.Parse(busyoKahouList[i]);
				if(kahouId !=0){

					Entity_kahou_chishikisyo_mst Mst = Resources.Load ("Data/kahou_chishikisyo_mst") as Entity_kahou_chishikisyo_mst;

					//Saku
					addStatus =  ((float)sakuStatus * (float)Mst.param [kahouId - 1].kahouEffect)/100;

				}
			}
		}
		string finalStatus = sakuStatus.ToString() + "<Color=#35D74BFF>(+" + (Mathf.CeilToInt(addStatus)).ToString() + ")</Color>";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            effection = effection.Replace("ABC", finalStatus);
        }else {
            effection = effection.Replace("A", finalStatus);
        }

		//sakuList.Add(
		sakuList.Add (sakuId.ToString());
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            sakuList.Add (sakuMst.param [sakuId - 1].nameEng); //Type
        }else {
            sakuList.Add(sakuMst.param[sakuId - 1].name); //Type
        }
		sakuList.Add (effection);
		sakuList.Add (sakuLv.ToString());
		sakuList.Add (sakuStatus.ToString());

		return sakuList;

	}


	public List<string> getSakuInfoForNextLv(int busyoId){
		List<string> sakuList = new List<string>();

		Entity_busyo_mst busyoMst = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
		int sakuId = busyoMst.param [busyoId - 1].saku_id;
		string temp = "saku" + busyoId.ToString ();
		int sakuLv = PlayerPrefs.GetInt(temp,0);
        if (sakuLv == 0) {
            sakuLv = 1;
            PlayerPrefs.SetInt(temp, 1);
            PlayerPrefs.Flush();
        }

        //Get Saku Status
        Entity_saku_mst sakuMst = Resources.Load ("Data/saku_mst") as Entity_saku_mst;
		object sakulst = sakuMst.param[sakuId-1];
		Type t = sakulst.GetType();
		String param = "lv" + sakuLv;
		FieldInfo f = t.GetField(param);
		int sakuStatus =(int)f.GetValue(sakulst);
        string effection = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            effection = sakuMst.param [sakuId - 1].effectionEng;
        }else {
            effection = sakuMst.param[sakuId - 1].effection;
        }
		int nextSakuLv = sakuLv + 1;
		String param2 = "lv" + nextSakuLv;
		FieldInfo f2 = t.GetField(param2);
		int nextSakuStatus =(int)f2.GetValue(sakulst);

		int diffStatus = nextSakuStatus - sakuStatus;
		string diffStatusString = sakuStatus.ToString() + "<Color=#35D74BFF>(+" + (diffStatus).ToString() + ")</Color>";

        if (Application.systemLanguage != SystemLanguage.Japanese) {
            effection = effection.Replace("ABC", diffStatusString);
        }else {
            effection = effection.Replace("A", diffStatusString);
        }

		//sakuList.Add(
		sakuList.Add (sakuId.ToString());
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            sakuList.Add (sakuMst.param [sakuId - 1].nameEng); //Type
        }else {
            sakuList.Add(sakuMst.param[sakuId - 1].name); //Type
        }
		sakuList.Add (effection);
		sakuList.Add (sakuLv.ToString());
		sakuList.Add (sakuStatus.ToString());

		return sakuList;

	}

	public List<string> getGokuiInfoByLv(int sakuId, int sakuLv){
		List<string> gokuiInfoList = new List<string>();

        if(sakuLv==0) {
            sakuLv = 1;
        }

		//Get Saku Status
		Entity_saku_mst sakuMst = Resources.Load ("Data/saku_mst") as Entity_saku_mst;
		object sakulst = sakuMst.param[sakuId-1];
		Type t = sakulst.GetType();
		String param = "lv" + sakuLv;
		FieldInfo f = t.GetField(param);
		int sakuStatus =(int)f.GetValue(sakulst);
		string effection = sakuMst.param [sakuId - 1].effection;
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            effection = sakuMst.param[sakuId - 1].effectionEng;
        }else {
            effection = sakuMst.param[sakuId - 1].effection;
        }
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            effection = effection.Replace("ABC", sakuStatus.ToString());
        }else {
            effection = effection.Replace("A", sakuStatus.ToString());
        }
		gokuiInfoList.Add (sakuId.ToString());
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            gokuiInfoList.Add (sakuMst.param [sakuId - 1].nameEng); //Name
        }else {
            gokuiInfoList.Add(sakuMst.param[sakuId - 1].name); //Name
        }
		gokuiInfoList.Add (effection);

		return gokuiInfoList;

	}

	public List<string> getGokuiInfoForLabel(int busyoId, int gokuiId){
		List<string> sakuList = new List<string>();

		Entity_busyo_mst busyoMst = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
		string temp = "saku" + busyoId.ToString ();
		int sakuLv = PlayerPrefs.GetInt(temp,0);
        if (sakuLv == 0) {
            sakuLv = 1;
            PlayerPrefs.SetInt(temp, 1);
            PlayerPrefs.Flush();
        }

        //Get Saku Status
        Entity_saku_mst sakuMst = Resources.Load ("Data/saku_mst") as Entity_saku_mst;
		object sakulst = sakuMst.param[gokuiId-1];
		Type t = sakulst.GetType();
		String param = "lv" + sakuLv;
		FieldInfo f = t.GetField(param);
		int sakuStatus =(int)f.GetValue(sakulst);
		float addStatus = 0;
        string effection = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            effection = sakuMst.param [gokuiId - 1].effectionEng;
        }else {
            effection = sakuMst.param[gokuiId - 1].effection;
        }

		//Kahou Adjustment
		string kahouTemp = "kahou" + busyoId;
		string busyoKahou = PlayerPrefs.GetString (kahouTemp);
		char[] delimiterChars = {','};
		string[] busyoKahouList = busyoKahou.Split (delimiterChars);

		for(int i=0;i<busyoKahouList.Length;i++){
			if(i==7){
				int kahouId = int.Parse(busyoKahouList[i]);
				if(kahouId !=0){

					Entity_kahou_chishikisyo_mst Mst = Resources.Load ("Data/kahou_chishikisyo_mst") as Entity_kahou_chishikisyo_mst;

					//Saku
					addStatus =  ((float)sakuStatus * (float)Mst.param [kahouId - 1].kahouEffect)/100;

				}
			}
		}
		string finalStatus = sakuStatus.ToString() + "<Color=#35D74BFF>(+" + (Mathf.CeilToInt(addStatus)).ToString() + ")</Color>";

        if (Application.systemLanguage != SystemLanguage.Japanese) {
            effection = effection.Replace("ABC", finalStatus);
        }else {
            effection = effection.Replace("A", finalStatus);
        }
		//sakuList.Add(
		sakuList.Add (gokuiId.ToString());
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            sakuList.Add (sakuMst.param [gokuiId - 1].nameEng); //Type
        }else {
            sakuList.Add(sakuMst.param[gokuiId - 1].name); //Type
        }
		sakuList.Add (effection);
		sakuList.Add (sakuLv.ToString());
		sakuList.Add (sakuStatus.ToString());

		return sakuList;

	}

	public List<string> getGokuiInfoForNextLv(int busyoId, int gokuiId){
		List<string> sakuList = new List<string>();

		string temp = "saku" + busyoId.ToString ();
		int sakuLv = PlayerPrefs.GetInt(temp,0);

        if (sakuLv == 0) {
            sakuLv = 1;
            PlayerPrefs.SetInt(temp, 1);
            PlayerPrefs.Flush();
        }

        //Get Saku Status
        Entity_saku_mst sakuMst = Resources.Load ("Data/saku_mst") as Entity_saku_mst;
		object sakulst = sakuMst.param[gokuiId-1];
		Type t = sakulst.GetType();
		String param = "lv" + sakuLv;
		FieldInfo f = t.GetField(param);
		int sakuStatus =(int)f.GetValue(sakulst);
        string effection = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            effection = sakuMst.param [gokuiId - 1].effectionEng;
        }else {
            effection = sakuMst.param[gokuiId - 1].effection;
        }
		int nextSakuLv = sakuLv + 1;
		String param2 = "lv" + nextSakuLv;
		FieldInfo f2 = t.GetField(param2);
		int nextSakuStatus =(int)f2.GetValue(sakulst);

		int diffStatus = nextSakuStatus - sakuStatus;
		string diffStatusString = sakuStatus.ToString() + "<Color=#35D74BFF>(+" + (diffStatus).ToString() + ")</Color>";

        if (Application.systemLanguage != SystemLanguage.Japanese) {
            effection = effection.Replace("ABC", diffStatusString);
        }else {
            effection = effection.Replace("A", diffStatusString);
        }
		//sakuList.Add(
		sakuList.Add (gokuiId.ToString());
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            sakuList.Add (sakuMst.param [gokuiId - 1].nameEng); //Type
        }else {
            sakuList.Add(sakuMst.param[gokuiId - 1].name); //Type
        }
		sakuList.Add (effection);
		sakuList.Add (sakuLv.ToString());
		sakuList.Add (sakuStatus.ToString());

		return sakuList;

	}


	public List<string> getGokuiInfo(int busyoId, int gokuiId){

		List<string> sakuList = new List<string>();

		Entity_busyo_mst busyoMst = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
		string temp = "saku" + busyoId.ToString ();
		int sakuLv = PlayerPrefs.GetInt(temp,0);

        if (sakuLv == 0) {
            sakuLv = 1;
            PlayerPrefs.SetInt(temp, 1);
            PlayerPrefs.Flush();
        }

        //Get Saku Status
        Entity_saku_mst sakuMst = Resources.Load ("Data/saku_mst") as Entity_saku_mst;
		object sakulst = sakuMst.param[gokuiId-1];
		Type t = sakulst.GetType();
		String param = "lv" + sakuLv;
		FieldInfo f = t.GetField(param);
		int sakuStatus =(int)f.GetValue(sakulst);
        string effection = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            effection = sakuMst.param [gokuiId - 1].effectionEng;
        }else {
            effection = sakuMst.param[gokuiId - 1].effection;
        }
        
        if (Application.loadedLevelName != "touyou" && Application.loadedLevelName != "tutorialTouyou") {
            //Kahou Adjustment
            string kahouTemp = "kahou" + busyoId;
			string busyoKahou = PlayerPrefs.GetString (kahouTemp);
			char[] delimiterChars = { ',' };
			string[] busyoKahouList = busyoKahou.Split (delimiterChars);

			for (int i = 0; i < busyoKahouList.Length; i++) {
				if (i == 7) {
					int kahouId = int.Parse (busyoKahouList [i]);
					if (kahouId != 0) {

						Entity_kahou_chishikisyo_mst Mst = Resources.Load ("Data/kahou_chishikisyo_mst") as Entity_kahou_chishikisyo_mst;

						//Saku
						float calcSenpou = ((float)sakuStatus * (float)Mst.param [kahouId - 1].kahouEffect) / 100;
						sakuStatus = Mathf.CeilToInt ((float)sakuStatus + calcSenpou);

					}
				}
			}
		}
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            effection = effection.Replace("ABC", sakuStatus.ToString());
        }else {
            effection = effection.Replace("A", sakuStatus.ToString());
        }
		//sakuList.Add(
		sakuList.Add (gokuiId.ToString());
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            sakuList.Add (sakuMst.param [gokuiId - 1].nameEng); //Type
        }else {
            sakuList.Add(sakuMst.param[gokuiId - 1].name); //Type
        }
		sakuList.Add (effection);
		sakuList.Add (sakuLv.ToString());
		sakuList.Add (sakuStatus.ToString());

		return sakuList;

	}

    public bool getSakuShipFlg(int sakuId) {

        bool shipFlg = false;
        Entity_saku_mst sakuMst = Resources.Load("Data/saku_mst") as Entity_saku_mst;
        shipFlg = sakuMst.param[sakuId - 1].shipFlg;
        
        return shipFlg;

    }

    public List<string> getSakuInfoLvMax(int busyoId) {
        List<string> sakuList = new List<string>();

        Entity_busyo_mst busyoMst = Resources.Load("Data/busyo_mst") as Entity_busyo_mst;
        int sakuId = busyoMst.param[busyoId - 1].saku_id;
        int sakuLv = 20;

        //Get Saku Status
        Entity_saku_mst sakuMst = Resources.Load("Data/saku_mst") as Entity_saku_mst;
        object sakulst = sakuMst.param[sakuId - 1];
        Type t = sakulst.GetType();
        String param = "lv" + sakuLv;
        FieldInfo f = t.GetField(param);
        int sakuStatus = (int)f.GetValue(sakulst);
        string effection = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            effection = sakuMst.param[sakuId - 1].effectionEng;
            effection = effection.Replace("ABC", sakuStatus.ToString());
        }
        else {
            effection = sakuMst.param[sakuId - 1].effection;
            effection = effection.Replace("A", sakuStatus.ToString());
        }

        sakuList.Add(sakuId.ToString());
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            sakuList.Add(sakuMst.param[sakuId - 1].nameEng); //Type		    
        }else {
            sakuList.Add(sakuMst.param[sakuId - 1].name); //Type
        }
        sakuList.Add(effection);
        sakuList.Add(sakuLv.ToString());
        sakuList.Add(sakuStatus.ToString());

        return sakuList;

    }

    public string getSakuName(int sakuId) {
        string sakuName = "";
        
        Entity_saku_mst sakuMst = Resources.Load("Data/saku_mst") as Entity_saku_mst;
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            sakuName = sakuMst.param[sakuId - 1].nameEng;           
        }else {
            sakuName = sakuMst.param[sakuId - 1].name;
        }
        return sakuName;

    }



}
