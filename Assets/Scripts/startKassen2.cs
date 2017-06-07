using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class startKassen2 : MonoBehaviour {

	public int myJinkei = 0;
	public int enemyJinkei = 0;
	public int soudaisyo = 0;
	public int enemySoudaisyo = 0;
	public int weatherId = 0;
	public int myHei = 0;
	public int enemyHei = 0;
	public int activeDaimyoId = 0;
	public bool roujyouFlg = false;
	public bool isAttackedFlg = false;
	public int myShiroLv = 0;
	public List<int> myTorideLvList;
	public List<string> myTorideSlotNoList;
    public int busyoQty = 0;
    public int busyoLimitQty = 0;
    public GameObject JinkeiView = null;
    public Text playerHeiText;
    public int totalLv = 0;
    public int totalChLv = 0;
    public int totalChQty = 0;



    public void Start() {
        JinkeiView = GameObject.Find("PlayerJinkeiView").gameObject;
        playerHeiText = GameObject.Find("PlayerHei").transform.FindChild("HeiValue").GetComponent<Text>();
    }

    public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        if(GameObject.Find("GameScene").GetComponent<preKassen>()) {
            busyoQty = GameObject.Find("GameScene").GetComponent<preKassen>().busyoCurrentQty;
            busyoLimitQty = GameObject.Find("GameScene").GetComponent<preKassen>().jinkeiLimit;
        }else if(GameObject.Find("GameScene").GetComponent<preKaisen>()) {
            busyoQty = GameObject.Find("GameScene").GetComponent<preKaisen>().busyoCurrentQty;
            busyoLimitQty = GameObject.Find("GameScene").GetComponent<preKaisen>().jinkeiLimit;
        }
        if (busyoQty == 0) {
            //Error
            audioSources[4].Play();

            Message msg = new Message();
            msg.makeMessage(msg.getMessage(134));

        }else {
            
            if(busyoQty> busyoLimitQty) {
                //Error
                audioSources[4].Play();
                Message msg = new Message();
                msg.makeMessage(msg.getMessage(139));
            }else {

                bool hardFlg = PlayerPrefs.GetBool("hardFlg");
                bool diffClanFlg = false;
                int myDaimyo = PlayerPrefs.GetInt("myDaimyo");

                if (hardFlg) {
                    //check same clan
                    foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
                        foreach (Transform busyo in obs.transform) {
                            if (busyo.GetComponent<Senryoku>().belongDaimyoId != myDaimyo) {
                                diffClanFlg = true;
                            }
                        }
                    }
                }
                if (diffClanFlg) {
                    audioSources[4].Play();
                    Message msg = new Message();
                    msg.makeMessage(msg.getMessage(144));

                }else {
                    
                    audioSources [5].Play ();

		            //Roujyou
		            PlayerPrefs.SetBool("roujyouFlg",roujyouFlg);

		            //Track
		            int TrackTotalKassenNo = PlayerPrefs.GetInt("TrackTotalKassenNo",0);
		            TrackTotalKassenNo = TrackTotalKassenNo + 1;
		            PlayerPrefs.SetInt("TrackTotalKassenNo",TrackTotalKassenNo);

		            int TrackBiggestDaimyoHei = PlayerPrefs.GetInt("TrackBiggestDaimyoHei",0);
		            if (TrackBiggestDaimyoHei < enemyHei) {
			            TrackBiggestDaimyoHei = enemyHei;
			            PlayerPrefs.SetInt("TrackBiggestDaimyoHei",TrackBiggestDaimyoHei);
			            PlayerPrefs.SetInt("TrackBiggestDaimyoId",activeDaimyoId);
		            }


		            int TrackMyBiggestHei = PlayerPrefs.GetInt("TrackMyBiggestHei",0);
		            if (TrackMyBiggestHei < myHei) {
			            TrackMyBiggestHei = myHei;
			            PlayerPrefs.SetInt("TrackMyBiggestHei",TrackMyBiggestHei);
		            }

		            //Quest
		            PlayerPrefs.SetBool ("questDailyFlg17",true);
		            if ((2*myHei) <= enemyHei) {
			            PlayerPrefs.SetBool ("twiceHeiFlg",true);
			            PlayerPrefs.Flush ();
		            }

		            //Delete Previous Senryoku
		            //1-25
		            for(int i=1;i<26;i++){
			            string key = "addSenryokuSlot" + i.ToString ();
			            PlayerPrefs.DeleteKey (key);
		            }
		            PlayerPrefs.Flush ();




		            //Reduce Hyourou
		            if (!isAttackedFlg) {
			            int nowHyourou = PlayerPrefs.GetInt ("hyourou");

			            bool isKessenFlg = PlayerPrefs.GetBool ("isKessenFlg");
			            int newHyourou = 0;
			            if (!isKessenFlg) {
				            newHyourou = nowHyourou - 5;
			            } else {
				            int kessenHyourou = PlayerPrefs.GetInt ("kessenHyourou");
				            newHyourou = nowHyourou - kessenHyourou;
				            PlayerPrefs.DeleteKey ("kessenHyourou");
			            }
			            PlayerPrefs.SetInt ("hyourou", newHyourou);
		            } else {
			            //Remove Player Shiro & Tride History
			            for (int i = 1; i < 26; i++) {
				            string pSRMap ="pSRLv";
				            string pTRMap ="pTRLv" + i.ToString ();
				            PlayerPrefs.DeleteKey (pTRMap);
				            PlayerPrefs.DeleteKey (pSRMap);
			            }

			            //Register
			            PlayerPrefs.SetInt("pSRLv",myShiroLv);
			            for (int i = 0; i < myTorideSlotNoList.Count; i++) {
				            string tmp = myTorideSlotNoList [i];
				            string pTRMap ="pTRLv" + tmp.ToString ();
				            int torideLv = myTorideLvList [i];
				            PlayerPrefs.SetInt(pTRMap,torideLv);
			            }
			            PlayerPrefs.Flush ();
		            }

                    //Change My Jinkei

                    if (myJinkei == 1) {
			            foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Slot")) {
				            if (obs.transform.childCount > 0) {
                                //Get Name
                                int childId = int.Parse (obs.transform.GetChild (0).name);
					            string slotId = obs.name.Substring (4);
					            string mapId = myJinkei.ToString () + "map" + slotId;
					            //Set Key
					            PlayerPrefs.SetInt (mapId, childId);

					            //Senryoku Add
					            Senryoku senryokuScript = obs.transform.GetChild (0).GetComponent<Senryoku> ();
                                int addAtk = senryokuScript.myDaimyoAddAtk + senryokuScript.belongDaimyoAddAtk;
					            int addDfc = senryokuScript.myDaimyoAddDfc + senryokuScript.belongDaimyoAddDfc;
					            if (addAtk != 0 || addDfc != 0) {
						            string result = addAtk.ToString () + "," + addDfc.ToString ();
						            string key = "addSenryokuSlot" + slotId;
						            PlayerPrefs.SetString (key, result);
					            }

				            } else {
					            //Delete Key
					            string mapId = myJinkei.ToString () + "map" + obs.name.Substring (4);
					            PlayerPrefs.DeleteKey (mapId);
				            }
			            }

                        //Soudaisyo
                        PlayerPrefs.SetInt("jinkei", myJinkei);
                        PlayerPrefs.SetInt ("soudaisyo1", soudaisyo);			
			            PlayerPrefs.Flush ();


		            } else if (myJinkei == 2) {
			            foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Slot")) {
				            if (obs.transform.childCount > 0) {
                                //Get Name 
                                int childId = int.Parse (obs.transform.GetChild (0).name);
					            string slotId = obs.name.Substring (4);
					            string mapId = myJinkei.ToString () + "map" + slotId;
					            //Set Key
					            PlayerPrefs.SetInt (mapId, childId);

					            //Senryoku Add
					            Senryoku senryokuScript = obs.transform.GetChild (0).GetComponent<Senryoku> ();
                                int addAtk = senryokuScript.myDaimyoAddAtk + senryokuScript.belongDaimyoAddAtk;
					            int addDfc = senryokuScript.myDaimyoAddDfc + senryokuScript.belongDaimyoAddDfc;
					            if (addAtk != 0 || addDfc != 0) {
						            string result = addAtk.ToString () + "," + addDfc.ToString ();
						            string key = "addSenryokuSlot" + slotId;
						            PlayerPrefs.SetString (key, result);
					            }
				            } else {
					            //Delete Key
					            string mapId = myJinkei.ToString () + "map" + obs.name.Substring (4);
					            PlayerPrefs.DeleteKey (mapId);
				            }
			            }

                        //Soudaisyo
                        PlayerPrefs.SetInt("jinkei", myJinkei);
                        PlayerPrefs.SetInt ("soudaisyo2", soudaisyo);
			            PlayerPrefs.Flush ();

		            } else if (myJinkei == 3) {
			            foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Slot")) {
				            if (obs.transform.childCount > 0) {
                                //Get Name 
                                int childId = int.Parse (obs.transform.GetChild (0).name);
					            string slotId = obs.name.Substring (4);
					            string mapId = myJinkei.ToString () + "map" + slotId;
					            //Set Key
					            PlayerPrefs.SetInt (mapId, childId);

					            //Senryoku Add
					            Senryoku senryokuScript = obs.transform.GetChild (0).GetComponent<Senryoku> ();
                                int addAtk = senryokuScript.myDaimyoAddAtk + senryokuScript.belongDaimyoAddAtk;
					            int addDfc = senryokuScript.myDaimyoAddDfc + senryokuScript.belongDaimyoAddDfc;
					            if (addAtk != 0 || addDfc != 0) {
						            string result = addAtk.ToString () + "," + addDfc.ToString ();
						            string key = "addSenryokuSlot" + slotId;
						            PlayerPrefs.SetString (key, result);
					            }
				            } else {
					            //Delete Key
					            string mapId = myJinkei.ToString () + "map" + obs.name.Substring (4);
					            PlayerPrefs.DeleteKey (mapId);
				            }
			            }


                        //Soudaisyo
                        PlayerPrefs.SetInt("jinkei", myJinkei);
                        PlayerPrefs.SetInt ("soudaisyo3", soudaisyo);
			            PlayerPrefs.Flush ();


		            } else if (myJinkei == 4) {
			            foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Slot")) {
				            if (obs.transform.childCount > 0) {
                                //Get Name 
                                int childId = int.Parse (obs.transform.GetChild (0).name);
					            string slotId = obs.name.Substring (4);
					            string mapId = myJinkei.ToString () + "map" + slotId;
					            //Set Key
					            PlayerPrefs.SetInt (mapId, childId);

					            //Senryoku Add
					            Senryoku senryokuScript = obs.transform.GetChild (0).GetComponent<Senryoku> ();
                                int addAtk = senryokuScript.myDaimyoAddAtk + senryokuScript.belongDaimyoAddAtk;
					            int addDfc = senryokuScript.myDaimyoAddDfc + senryokuScript.belongDaimyoAddDfc;
					            if (addAtk != 0 || addDfc != 0) {
						            string result = addAtk.ToString () + "," + addDfc.ToString ();
						            string key = "addSenryokuSlot" + slotId;
						            PlayerPrefs.SetString (key, result);
					            }
				            } else {
					            //Delete Key
					            string mapId = myJinkei.ToString () + "map" + obs.name.Substring (4);
					            PlayerPrefs.DeleteKey (mapId);
				            }
			            }


                        PlayerPrefs.SetInt("jinkei",myJinkei);
			            PlayerPrefs.SetInt ("soudaisyo4", soudaisyo);
			            PlayerPrefs.Flush ();


		            }

		            //Remove Enemy Shiro & Tride History
		            for (int i = 1; i < 26; i++) {
			            string eSRMap = "eSRMap" + i.ToString ();
			            string eTRMap ="eTRMap" + i.ToString ();
			            PlayerPrefs.DeleteKey (eSRMap);
			            PlayerPrefs.DeleteKey (eTRMap);
		            }

		            //Enemy Temp Jinkei
		            PlayerPrefs.SetInt ("enemyJinkei", enemyJinkei);
		            foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("EnemySlot")) {
			            if (obs.transform.childCount > 0) {
				            //Get Name
				            int childId = int.Parse (obs.transform.GetChild (0).name);
				            string mapId = "emap" + obs.name.Substring (4);
				            //Set Key
				            PlayerPrefs.SetInt (mapId, childId);

				            //Roujyo
				            if (roujyouFlg) {
					            if (obs.transform.childCount > 1) {
						            if (obs.transform.GetChild (1).name == "shiro") {
							            string eSRMap ="eSRMap" + obs.name.Substring (4);
							            PlayerPrefs.SetBool (eSRMap, true);
						            } else {
							            string eTRMap ="eTRMap" + obs.name.Substring (4);
							            PlayerPrefs.SetBool (eTRMap, true);
						            }
					            }
				            }

			            } else {
				            //Delete Key
				            string mapId = "emap" + obs.name.Substring (4);
				            PlayerPrefs.DeleteKey (mapId);
			            }
		            }


		            //Enemy Soudaisyo
		            PlayerPrefs.SetInt ("enemySoudaisyo", enemySoudaisyo);

		            //Weather
		            PlayerPrefs.SetInt ("weather", weatherId);


                    //Jinkei Status Update           
                    int aveLv = totalLv / busyoQty;
                    int aveChLv = totalChLv / busyoQty;
                    int aveChQty = totalChQty / busyoQty;
                    int heiryoku = int.Parse(GameObject.Find("PlayerHei").transform.FindChild("HeiValue").GetComponent<Text>().text);
                    PlayerPrefs.SetInt("jinkeiAveLv", aveLv);
                    PlayerPrefs.SetInt("jinkeiAveChLv", aveChLv);
                    PlayerPrefs.SetInt("jinkeiBusyoQty", busyoQty);
                    PlayerPrefs.SetInt("jinkeiAveChQty", aveChQty);
                    PlayerPrefs.SetInt("jinkeiHeiryoku", heiryoku);
                    PlayerPrefs.SetInt("pvpHeiryoku", heiryoku);

                    /*
                    int nowJinkeiAveLv = PlayerPrefs.GetInt("jinkeiAveLv");
                    int nowJinkeiAveChLv = PlayerPrefs.GetInt("jinkeiAveChLv");
                    int nowJinkeiBusyoQty = PlayerPrefs.GetInt("jinkeiBusyoQty");
                    int nowJinkeiAveChQty = PlayerPrefs.GetInt("jinkeiAveChQty");
                    float randomPercentage = Random.Range(0.6f, 1.0f);
                    float randomPercentage2 = Random.Range(0.6f, 1.0f);
                    float randomPercentage3 = Random.Range(0.6f, 1.0f);
                    float randomPercentage4 = Random.Range(0.6f, 1.0f);
            
                    if (nowJinkeiBusyoQty * randomPercentage <= busyoQty){
                        PlayerPrefs.SetInt("jinkeiBusyoQty", busyoQty);
                    }if (nowJinkeiAveLv * randomPercentage2 <= aveLv){
                        PlayerPrefs.SetInt("jinkeiAveLv", aveLv);
                    }if (nowJinkeiAveChLv * randomPercentage3 <= aveChLv){
                        PlayerPrefs.SetInt("jinkeiAveChLv", aveChLv);
                    }if (nowJinkeiAveChQty * randomPercentage4 <= aveChQty){
                        PlayerPrefs.SetInt("jinkeiAveChQty", aveChQty);
                    }
                    */




                    PlayerPrefs.Flush ();

		            //Stop BGM
		            GameObject.Find ("BGMController").GetComponent<DontDestroySoundOnLoad> ().DestoryFlg = true;

                    //Scene Change
                    if (Application.loadedLevelName == "preKaisen") {
                        Application.LoadLevel("kaisen");
                    } else { 
                        Application.LoadLevel("kassen");
                    }
                }
            }
        }

    }

    void FixedUpdate() {
        SenryokuView();
    }

    public void SenryokuView() {
        //Current Status

        int totalTmpHp = 0;
        int totalTmpAtk = 0;
        int totalTmpDfc = 0;
        int totalTmpLv = 0;
        int totalTmpChLv = 0;
        int totalTmpChQty = 0;
        foreach (Transform childSlot in JinkeiView.transform) {
            //Count up Mago Exist
            if (childSlot.childCount > 0) {
                //senryoku
                foreach (Transform busyo in childSlot.transform) {
                    Senryoku senryokuScript = busyo.gameObject.GetComponent<Senryoku>();
                    totalTmpHp = totalTmpHp + senryokuScript.totalHp;
                    totalTmpAtk = totalTmpAtk + senryokuScript.totalAtk + senryokuScript.myDaimyoAddAtk + senryokuScript.belongDaimyoAddAtk;
                    totalTmpDfc = totalTmpDfc + senryokuScript.totalDfc + senryokuScript.myDaimyoAddDfc + senryokuScript.belongDaimyoAddDfc;
                    totalTmpLv = totalTmpLv + senryokuScript.lv;
                    totalTmpChLv = totalTmpChLv + senryokuScript.chlv;
                    totalTmpChQty = totalTmpChQty + senryokuScript.chQty;
                }
            }
        }

        playerHeiText.text = totalTmpHp.ToString();
        myHei = totalTmpHp;
        totalLv = totalTmpLv;
        totalChLv = totalTmpChLv;
        totalChQty = totalTmpChQty;
    }
}
