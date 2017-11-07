using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class KuniInfo : MonoBehaviour {

	Entity_kuni_mst kuniMst = Resources.Load ("Data/kuni_mst") as Entity_kuni_mst;
	Entity_kuni_mapping_mst kuniMapMst  = Resources.Load ("Data/kuni_mapping_mst") as Entity_kuni_mapping_mst;

	//Get cleared Kuni and check current open and register the difference to open kuni
	public void registerOpenKuni(int clearedKuniId){
		List<string> newOpenKuniList = new List<string>();

		//New Open Kuni
		for(int i=0; i<kuniMapMst.param.Count; i++){
			int temClearedKuniId = kuniMapMst.param[i].Souce;
			if(temClearedKuniId == clearedKuniId){
				newOpenKuniList.Add(kuniMapMst.param[i].Open.ToString());
			}
		}

		//Existed Open Kuni
		List<string> oldOpenKuniList = new List<string>();
		string openKuniString = PlayerPrefs.GetString("openKuni");
		char[] delimiterChars = {','};
		if (openKuniString != null && openKuniString != "") {
			if (openKuniString.Contains (",")) {
				oldOpenKuniList = new List<string> (openKuniString.Split (delimiterChars));
			} else {
				oldOpenKuniList.Add (openKuniString);
			}
		}

		//Merge
		foreach (string i in newOpenKuniList) {
			if(!oldOpenKuniList.Contains(i)){
				if (openKuniString != null && openKuniString != "") {
					openKuniString = openKuniString + "," + i;
				} else {
					openKuniString = i;
				}

				string tempKuni = "naisei" + i;
				if(!PlayerPrefs.HasKey(tempKuni)){
					PlayerPrefs.SetString(tempKuni,"1,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0");
				}
			}
		}

		//Open kuni initial data setting
		PlayerPrefs.SetString("openKuni", openKuniString);
		PlayerPrefs.Flush();

	}

	public List<int> getMappingKuni(int targetKuni){
		List<int> targetKuniList = new List<int>();

		for(int i=0; i<kuniMapMst.param.Count; i++){
			int temClearedKuniId = kuniMapMst.param[i].Souce;
			if(temClearedKuniId == targetKuni){
				targetKuniList.Add(kuniMapMst.param[i].Open);
			}
		}

		return targetKuniList;
	}

    public string getLinkStage(int srcKuni, int dstKuni){
        string linkStage = "";
        
        for(int i=0; i<kuniMapMst.param.Count; i++){
            int srcKuniTmp = kuniMapMst.param[i].Souce;
            if(srcKuniTmp == srcKuni) {
                int opnKuniTmp = kuniMapMst.param[i].Open;
                if(opnKuniTmp == dstKuni){
                    linkStage = kuniMapMst.param[i].linkStageId;
                    break;
                }
            }
        }
        
        return linkStage;
    }

    public List<int> getLinkStageXY(int srcKuni, int dstKuni){
        List<int> XYList = new List<int>();

        for (int i = 0; i < kuniMapMst.param.Count; i++){
            int srcKuniTmp = kuniMapMst.param[i].Souce;
            if (srcKuniTmp == srcKuni){
                int opnKuniTmp = kuniMapMst.param[i].Open;
                if (opnKuniTmp == dstKuni){
                    XYList.Add(kuniMapMst.param[i].ArrowX);
                    XYList.Add(kuniMapMst.param[i].ArrowY);
                    break;
                }
            }
        }
        return XYList;
    }



    public int getKuniLocationX(int kuniId){
		int x = kuniMst.param[kuniId -1].locationX;
		return x;
	}

	public int getKuniLocationY(int kuniId){
		int y = kuniMst.param[kuniId -1].locationY;
		return y;
	}

	public string getKuniName(int kuniId,int langId){
        string kuniName = "";
        if (langId == 2) {
            kuniName = kuniMst.param[kuniId -1].kuniNameEng;
        }else if (langId == 3) {
            kuniName = kuniMst.param[kuniId - 1].kuniNameSChn;
        }else {
            kuniName = kuniMst.param[kuniId - 1].kuniName;
        }
		return kuniName;
	}

	public string getKuniNaisei(int kuniId){
		string naiseiParm = kuniMst.param[kuniId -1].naisei;
		return naiseiParm;
	}

	public bool getKuniIsSeaFlg(int kuniId){
		bool flg = kuniMst.param[kuniId -1].isSeaFlg;
		return flg;
	}

	public bool getKuniIsSnowFlg(int kuniId){
		bool flg = kuniMst.param[kuniId -1].isSnowFlg;
		return flg;
	}

	public void updateOpenKuni(int myDaimyo, string seiryoku) {

        List<string> seiryokuList = new List<string> ();
		char[] delimiterChars = {','};
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));

		List<int> newOpenKuniList =  new List<int> ();
		for(int i=0; i<seiryokuList.Count; i++){
			int compDaimyo = int.Parse(seiryokuList[i]);
			if(myDaimyo == compDaimyo){
				int kuniId = i+1; 
				newOpenKuniList.AddRange(getMappingKuni (kuniId));
            }
		}

		string newOpenKuni = "";
		List<int> usedOpenKuniList = new List<int> ();
		for (int l = 0; l < newOpenKuniList.Count; l++) {
			int kuniId = newOpenKuniList [l];
			if (!usedOpenKuniList.Contains (kuniId)) {
				usedOpenKuniList.Add (kuniId);
				if (l == 0) {
					newOpenKuni = kuniId.ToString ();			
				} else {
					newOpenKuni = newOpenKuni + "," + kuniId.ToString ();						
				}
			}
		}

        //Open kuni initial data setting
        PlayerPrefs.SetString("openKuni", newOpenKuni);
		PlayerPrefs.Flush();
		
	}

    public void updateClearedKuni(int myDaimyo, string seiryoku) {

        List<string> seiryokuList = new List<string>();
        char[] delimiterChars = { ',' };
        seiryokuList = new List<string>(seiryoku.Split(delimiterChars));
        List<string> mySeiryokuList = new List<string>();

        string clearedKuni = "";
        for (int i = 0; i < seiryokuList.Count; i++) {
            int compDaimyo = int.Parse(seiryokuList[i]);
            if (myDaimyo == compDaimyo) {
                int kuniId = i + 1;
                if(clearedKuni == "" || clearedKuni == null) {
                    clearedKuni = kuniId.ToString();
                }else {
                    clearedKuni = clearedKuni + "," + kuniId.ToString();
                }
                string tmpClearedStage = "kuni" + kuniId.ToString();
                PlayerPrefs.SetString(tmpClearedStage, "1,2,3,4,5,6,7,8,9,10");
            }
        }


        PlayerPrefs.SetString("clearedKuni", clearedKuni);
        PlayerPrefs.Flush();

    }


    public void deleteDoumeiKuniIcon(int doumeiDaimyoId){

		GameObject kuniIconView = GameObject.Find ("KuniIconView");

		//Set Seiryoku
		string seiryoku = PlayerPrefs.GetString ("seiryoku");
		List<string> seiryokuList = new List<string> ();
		char[] delimiterChars = {','};
		seiryokuList = new List<string> (seiryoku.Split (delimiterChars));
		

		
		//Change Kuni Icon Color & Param
		Color normalColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f); //White
		for (int i=0; i<seiryokuList.Count; i++) {
			int seiryokuDaimyoId = int.Parse(seiryokuList[i]);
			if(seiryokuDaimyoId == doumeiDaimyoId){
				int KuniId = i+1; //Kuni Id
				SendParam sp = kuniIconView.transform.FindChild (KuniId.ToString ()).GetComponent<SendParam> ();
				Image image = kuniIconView.transform.FindChild (KuniId.ToString ()).GetComponent<Image> ();

				sp.doumeiFlg = false;
				image.color = normalColor;
			}
		}
		

		//Open Kuni
		Color openKuniColor = new Color (255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f); //Yellow
		string openKuni = PlayerPrefs.GetString ("openKuni");
		List<string> openKuniList = new List<string> ();
		if (openKuni != null && openKuni != "") {
			if (openKuni.Contains (",")) {
				openKuniList = new List<string> (openKuni.Split (delimiterChars));
			} else {
				openKuniList.Add (openKuni);
			}
		}
		string clearedKuni = PlayerPrefs.GetString ("clearedKuni");
		List<string> clearedKuniList = new List<string> ();
		if (clearedKuni != null && clearedKuni != "") {
			if (clearedKuni.Contains (",")) {
				clearedKuniList = new List<string> (clearedKuni.Split (delimiterChars));
			} else {
				clearedKuniList.Add (clearedKuni);
			}
		}

		for (int i=0; i<openKuniList.Count; i++) {
			string openKuniId = openKuniList [i];
			
			//Flg Change
			GameObject targetOpenKuni = GameObject.Find ("KuniIconView").transform.FindChild (openKuniId).gameObject;
			targetOpenKuni.GetComponent<SendParam> ().openFlg = true;
			bool doumeiFlg = targetOpenKuni.GetComponent<SendParam>().doumeiFlg;
			
			//Color Change
			if (!clearedKuniList.Contains (openKuniId)) {
				if (!doumeiFlg) {
					targetOpenKuni.GetComponent<Image> ().color = openKuniColor;
				}
			}
		}

	}

	public int getOneKuniId(int daimyoId){
		int kuniId = 0;

		for (int i=0; i<kuniMst.param.Count; i++) {
			int tmpDaimyoId = kuniMst.param[i].daimyoId;
			if(daimyoId == tmpDaimyoId){
				kuniId = kuniMst.param[i].kunId;
				break;
			}
		}
		return kuniId;
	}

}
