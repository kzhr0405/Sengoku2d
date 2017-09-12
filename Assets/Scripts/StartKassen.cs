using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class StartKassen : MonoBehaviour {

	public int activeKuniId = 0;
	public int activeStageId = 0;
	public string activeStageName = "";
	public int activeStageMoney = 0;
	public int activeStageExp = 0;
	public string activeItemGrp = "";
	public string activeItemType = "";
	public int activeItemId = 0;
	public int activeItemQty = 0;
	public int activeBusyoQty;
	public int activeBusyoLv;
	public int activeButaiQty;
	public int activeButaiLv;
	public int activeDaimyoId;
	public bool doumeiFlg = false;

	public int linkNo = 0;
	public int powerType = 0;
    public bool lastOneFlg = false;

    public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
        Message msg = new Message();

        if (!checkJinkeiAvailable()) {
            audioSources[4].Play();
            msg.makeUpperMessageOnBoard(msg.getMessage(133));

        } else { 
            
		    //Hyourou Check
		    int nowHyourou = PlayerPrefs.GetInt ("hyourou");

		    if(nowHyourou >=5 ){

			    audioSources [5].Play ();

			    //Now on Kuni & Stage
			    PlayerPrefs.SetInt("activeKuniId", activeKuniId);
			    PlayerPrefs.SetInt("activeStageId", activeStageId);
			    PlayerPrefs.SetString("activeStageName", activeStageName);

			    //What we can get
			    PlayerPrefs.SetInt("activeStageMoney", activeStageMoney);
			    PlayerPrefs.SetInt("activeStageExp", activeStageExp);
			    PlayerPrefs.SetString("activeItemGrp", activeItemGrp);
			    PlayerPrefs.SetString("activeItemType", activeItemType);
			    PlayerPrefs.SetInt("activeItemId", activeItemId);
			    PlayerPrefs.SetInt("activeItemQty", activeItemQty);

			    //For Dramatic Enemy Creation
			    PlayerPrefs.SetInt("activeDaimyoId", activeDaimyoId);
			    PlayerPrefs.SetInt ("activeBusyoQty", activeBusyoQty);
			    PlayerPrefs.SetInt ("activeBusyoLv", activeBusyoLv);
			    PlayerPrefs.SetInt ("activeButaiQty", activeButaiQty);
			    PlayerPrefs.SetInt ("activeButaiLv", activeButaiLv);

                //lastOneFlg
                PlayerPrefs.SetBool("lastOneFlg", lastOneFlg);

                //Gaikou
                int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
			    Gaikou gaikou = new Gaikou ();
			    gaikou.downGaikouByAttack (myDaimyo, activeDaimyoId);

			    //Reverse Flag
			    PlayerPrefs.DeleteKey ("isAttackedFlg");
			    PlayerPrefs.DeleteKey ("isKessenFlg");

			    //Player Doumei Flg
			    PlayerPrefs.DeleteKey("playerEngunList");

			    //Enemy Doumei Handling
			    PlayerPrefs.DeleteKey("enemyEngunList");
			    string doumeiTemp = "doumei" + activeDaimyoId;
			    string enemyDoumeiString = PlayerPrefs.GetString (doumeiTemp);
			    char[] delimiterChars = {','};
			    List<string> doumeiList = new List<string>();
			    if(enemyDoumeiString != null && enemyDoumeiString !=""){
				    if(enemyDoumeiString.Contains(",")){
					    doumeiList = new List<string> (enemyDoumeiString.Split (delimiterChars));
				    }else{
					    doumeiList.Add(enemyDoumeiString);
				    }
			    }
			    string seiryoku = PlayerPrefs.GetString ("seiryoku");
			    List<string> seiryokuList = new List<string> ();
			    seiryokuList = new List<string> (seiryoku.Split (delimiterChars));

			    Doumei doumei = new Doumei();
			    List<string> okDaimyoList = new List<string> ();
			    List<string> checkedList = new List<string> ();
			    string dstEngunDaimyoId = "";
			    string dstEngunSts = ""; //BusyoId-BusyoLv-ButaiQty-ButaiLv:

			    okDaimyoList = doumei.traceNeighborDaimyo(activeKuniId, activeDaimyoId, doumeiList, seiryokuList, checkedList, okDaimyoList);

			    if(okDaimyoList.Count !=0){
				    for(int k=0; k<okDaimyoList.Count; k++){
					    string engunDaimyo = okDaimyoList[k];
					    int yukoudo = gaikou.getExistGaikouValue(int.Parse(engunDaimyo), activeDaimyoId);

					    //mydaimyo doumei check
					    bool myDoumeiFlg = false;
					    myDoumeiFlg= doumei.myDoumeiExistCheck (int.Parse(engunDaimyo));
					    if (myDoumeiFlg) {
						    yukoudo = yukoudo / 2;
					    }

					    //engun check
					    MainEventHandler main = new MainEventHandler();
					    bool dstEngunFlg = main.CheckByProbability (yukoudo);
					    if(dstEngunFlg){
						    //Engun OK
						    dstEngunFlg = true;
						    if(dstEngunDaimyoId !=null && dstEngunDaimyoId !=""){
							    dstEngunDaimyoId = dstEngunDaimyoId + ":" + engunDaimyo;
							    string tempEngunSts = main.getEngunSts(engunDaimyo);
							    dstEngunSts = dstEngunSts + ":" + engunDaimyo + "-" + tempEngunSts;
							
						    }else{
							    dstEngunDaimyoId = engunDaimyo;
							    string tempEngunSts = main.getEngunSts(engunDaimyo);
							    dstEngunSts = engunDaimyo + "-" + tempEngunSts;
							
						    }
					    }
				    }
				    PlayerPrefs.SetString("enemyEngunList", dstEngunSts);

			    }


			    //Kyoutou Handling

			    PlayerPrefs.DeleteKey("tempKyoutouList");
			    string tempKyoutouList = "";

			    char[] delimiterChars2 = {':'};
			    char[] delimiterChars3 = {'-'};
			    List<string> unitList = new List<string> ();
			    string nextKyoutouList = "";
			    string playerKyoutouList = PlayerPrefs.GetString("playerKyoutouList","");
			    if(playerKyoutouList !=null && playerKyoutouList != ""){
				    if(playerKyoutouList.Contains(":")){
					    unitList = new List<string> (playerKyoutouList.Split (delimiterChars2));
				    }else{
					    unitList.Add(playerKyoutouList);
				    }
				    List<string> unit2List = new List<string> ();
				    for(int i=0; i<unitList.Count; i++){
					    string playerKyoutouList2 = unitList[i];
					    unit2List = new List<string> (playerKyoutouList2.Split (delimiterChars3));

					    string tempString = unit2List[0] + "-" + unit2List[1]+ "-" + unit2List[2]+ "-" + unit2List[3]+ "-" + unit2List[4];
					    if(unit2List[0] == activeKuniId.ToString()){
						    if(tempKyoutouList != null && tempKyoutouList != ""){
							    tempKyoutouList = tempKyoutouList + ":" + tempString;
						    }else{
							    tempKyoutouList = tempString;
						    }
					    }else{
						    if(nextKyoutouList != "" && nextKyoutouList != null){
							    nextKyoutouList = nextKyoutouList + ":" + tempString;
						    }else{
							    nextKyoutouList = tempString;
						    }
					    }
				    }
			    }

			    PlayerPrefs.SetString("tempKyoutouList",tempKyoutouList);
			    PlayerPrefs.SetString("playerKyoutouList",nextKyoutouList);


			    //Power Keisu
			    PlayerPrefs.DeleteKey("activeLink");
			    PlayerPrefs.DeleteKey("activePowerType");
			    PlayerPrefs.SetInt("activeLink",linkNo);

                //Power Type Config
                if(powerType==1){
                    int rdm = UnityEngine.Random.Range(0, 3); //1:happen, 0,2:not happen
                    if(rdm==1){
                        powerType = 2;
                    }
                }else if(powerType == 2){
                    int rdm = UnityEngine.Random.Range(0, 3); //1:happen, 0,2:not happen
                    if (rdm == 1){
                        powerType = 3;
                    }
                }




			    PlayerPrefs.SetInt("activePowerType",powerType);

			    PlayerPrefs.Flush();
			    //Stop BGM
			    GameObject.Find ("BGMController").GetComponent<DontDestroySoundOnLoad> ().DestoryFlg = true;

                Stage stage = new Stage();
                int stageMapId = stage.getStageMap(activeKuniId, activeStageId);

                if(stageMapId != 4) {
                    Application.LoadLevel("preKassen");
                }else {
                    Application.LoadLevel("preKaisen");
                }

            }
            else{
			    //Error Message
			    audioSources [4].Play ();
                
			    msg.hyourouMovieMessage();

		    }
        }
	}

    public bool checkJinkeiAvailable() {
        bool okFlg = false;

        int jinkei = PlayerPrefs.GetInt("jinkei");
        List<int> slotList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
        
        for(int i=0; i<slotList.Count; i++) {
            string slotId = slotList[i].ToString();
            string mapId = jinkei.ToString() + "map" + slotId;
            if (jinkei == 1) {
                if (slotId == "1" || slotId == "2" || slotId == "7" || slotId == "8" ||
                    slotId == "11" || slotId == "12" || slotId == "13" || slotId == "14" ||
                   slotId == "17" || slotId == "18" || slotId == "21" || slotId == "22") {
                    int busyoId = PlayerPrefs.GetInt(mapId);
                    if(busyoId != 0 && busyoId != null) {
                        okFlg = true;
                        break;
                    }
                }
            }else if(jinkei==2){
                if (slotId == "3" || slotId == "4" || slotId == "5" || slotId == "7" ||
                  slotId == "8" || slotId == "11" || slotId == "12" || slotId == "17" ||
                   slotId == "18" || slotId == "23" || slotId == "24" || slotId == "25") {
                    int busyoId = PlayerPrefs.GetInt(mapId);
                    if (busyoId != 0 && busyoId != null) {
                        okFlg = true;
                        break;
                    }
                }
            } else if (jinkei == 3) {
                if (slotId == "3" || slotId == "7" || slotId == "8" || slotId == "9" ||
                   slotId == "11" || slotId == "12" || slotId == "14" || slotId == "15" ||
                  slotId == "16" || slotId == "20" || slotId == "21" || slotId == "25") {
                    int busyoId = PlayerPrefs.GetInt(mapId);
                    if (busyoId != 0 && busyoId != null) {
                        okFlg = true;
                        break;
                    }
                }
            } else if (jinkei == 4) {
                if (slotId == "1" || slotId == "2" || slotId == "7" || slotId == "8" ||
                   slotId == "12" || slotId == "13" || slotId == "14" || slotId == "18" ||
                   slotId == "19" || slotId == "20" || slotId == "24" || slotId == "25") {
                    int busyoId = PlayerPrefs.GetInt(mapId);
                    if (busyoId != 0 && busyoId != null) {
                        okFlg = true;
                        break;
                    }
                }
            }
        }
          

        



        return okFlg;
    }


}
