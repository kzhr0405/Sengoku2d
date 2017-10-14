using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class JinkeiConfirmButton : MonoBehaviour {

	public int totalLv = 0;
	public int aveLv = 0;
	public int totalChLv = 0;
	public int aveChLv = 0;
    public int totalChQty = 0;
    public int aveChQty = 0;
    public int soudaisyo = 0;

	public void OnClick() {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();


        if (Application.loadedLevelName == "tutorialHyojyo") {
            //Tutorial
            audioSources[5].Play();

            bool tutorialDoneFlg = PlayerPrefs.GetBool("tutorialDoneFlg");
            PlayerPrefs.SetInt("tutorialId", 14);
            
            if(!tutorialDoneFlg) {

                GameObject Slot13 = GameObject.Find("copiedJinkeiView").transform.FindChild("Slot13").gameObject;
                int chldBusyoId = 0;
                foreach (Transform chld in Slot13.transform) {
                    chldBusyoId = int.Parse(chld.name);
                }
                PlayerPrefs.SetInt("1map13", chldBusyoId);

                //Set Key
                int jinkeiBusyoQty = int.Parse(GameObject.Find("jinkeiQtyValue").GetComponent<Text>().text);
                aveLv = totalLv / jinkeiBusyoQty;
                aveChLv = totalChLv / jinkeiBusyoQty;
                aveChQty = totalChQty / jinkeiBusyoQty;
                int heiryoku = int.Parse(GameObject.Find("totalHpValue").GetComponent<Text>().text);

                PlayerPrefs.SetInt("jinkeiAveLv", aveLv);
                PlayerPrefs.SetInt("jinkeiAveChLv", aveChLv);
                PlayerPrefs.SetInt("jinkeiBusyoQty", jinkeiBusyoQty);
                PlayerPrefs.SetInt("jinkeiAveChQty", aveChQty);
                PlayerPrefs.SetInt("jinkeiHeiryoku", heiryoku);
                PlayerPrefs.SetInt("pvpHeiryoku", heiryoku);
            }

            //Kassen
            PlayerPrefs.SetInt("activePowerType", 1);
            PlayerPrefs.DeleteKey("emap1");
            PlayerPrefs.DeleteKey("emap2");
            PlayerPrefs.DeleteKey("emap3");
            PlayerPrefs.DeleteKey("emap4");
            PlayerPrefs.DeleteKey("emap5");
            PlayerPrefs.DeleteKey("emap6");
            PlayerPrefs.DeleteKey("emap7");
            PlayerPrefs.DeleteKey("emap9");
            PlayerPrefs.DeleteKey("emap10");
            PlayerPrefs.DeleteKey("emap11");
            PlayerPrefs.DeleteKey("emap12");
            PlayerPrefs.DeleteKey("emap15");
            PlayerPrefs.DeleteKey("emap16");
            PlayerPrefs.DeleteKey("emap17");
            PlayerPrefs.DeleteKey("emap18");
            PlayerPrefs.DeleteKey("emap19");
            PlayerPrefs.DeleteKey("emap20");
            PlayerPrefs.DeleteKey("emap21");
            PlayerPrefs.DeleteKey("emap22");
            PlayerPrefs.DeleteKey("emap23");
            PlayerPrefs.DeleteKey("emap24");
            PlayerPrefs.DeleteKey("emap25");
            
            PlayerPrefs.SetInt("emap14", 24);
            PlayerPrefs.SetInt("emap13", 202);
            PlayerPrefs.SetInt("emap8", 188);
                
            PlayerPrefs.Flush();
            
            //Application.LoadLevel("tutorialMain");
            Application.LoadLevel("tutorialKassen");

        } else { 
        

            //Check for Existing
            string busyoQty = GameObject.Find ("jinkeiQtyValue").GetComponent<Text> ().text;
            if (busyoQty == "0") {
                //Error
                audioSources[4].Play();

                Message msg = new Message();
                msg.makeMessage(msg.getMessage(134));

            }else {
                int busyoLimitQty = int.Parse(GameObject.Find("jinkeiLimitValue").GetComponent<Text>().text);

                if (int.Parse(busyoQty) > busyoLimitQty) {
                    //Error
                    audioSources[4].Play();

                    Message msg = new Message();
                    msg.makeMessage(msg.getMessage(139));

                }else {
                    bool hardFlg = PlayerPrefs.GetBool("hardFlg");
                    bool diffClanFlg = false;
                    int myDaimyo = PlayerPrefs.GetInt("myDaimyo");

                    if(hardFlg) {
                        //check same clan
                        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {       
                            foreach(Transform busyo in obs.transform) {
                                if(busyo.GetComponent<Senryoku>().belongDaimyoId != myDaimyo) {
                                    diffClanFlg = true;
                                }
                            }
                        }
                    }
                
                    if(diffClanFlg) {
                        audioSources[4].Play();

                        Message msg = new Message();
                        msg.makeMessage(msg.getMessage(144));

                    }else {
                        audioSources[5].Play();

                        //Register PlayerPref by Jinkei
                        int selectedJinkei = GetComponent<Jinkei>().selectedJinkei;

                        if (selectedJinkei == 1) {
                            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
                                if (obs.transform.childCount > 0) {
                                    //Get Name 
                                    int childId = int.Parse(obs.transform.GetChild(0).name);
                                    string mapId = selectedJinkei.ToString() + "map" + obs.name.Substring(4);
                                    //Set Key
                                    PlayerPrefs.SetInt(mapId, childId);
                                }
                                else {
                                    //Delete Key
                                    string mapId = selectedJinkei.ToString() + "map" + obs.name.Substring(4);
                                    PlayerPrefs.DeleteKey(mapId);
                                }
                            }

                            int jinkeiBusyoQty = int.Parse(GameObject.Find("jinkeiQtyValue").GetComponent<Text>().text);
                            aveLv = totalLv / jinkeiBusyoQty;
                            aveChLv = totalChLv / jinkeiBusyoQty;
                            aveChQty = totalChQty / jinkeiBusyoQty;
                            int heiryoku = int.Parse(GameObject.Find("totalHpValue").GetComponent<Text>().text);
                            int nowJinkeiAveLv = PlayerPrefs.GetInt("jinkeiAveLv");
                            int nowJinkeiAveChLv = PlayerPrefs.GetInt("jinkeiAveChLv");
                            int nowJinkeiBusyoQty = PlayerPrefs.GetInt("jinkeiBusyoQty");
                            int nowJinkeiAveChQty = PlayerPrefs.GetInt("jinkeiAveChQty");
                            int nowHeiryoku = PlayerPrefs.GetInt("jinkeiHeiryoku");
                            PlayerPrefs.SetInt("pvpHeiryoku", heiryoku);

                            if (nowJinkeiBusyoQty  < jinkeiBusyoQty){
                                PlayerPrefs.SetInt("jinkeiBusyoQty", jinkeiBusyoQty);
                            }
                            if (nowJinkeiAveLv  < aveLv){
                                PlayerPrefs.SetInt("jinkeiAveLv", aveLv);
                            }
                            if (nowJinkeiAveChLv  < aveChLv){
                                PlayerPrefs.SetInt("jinkeiAveChLv", aveChLv);                           
                            }
                            if (nowJinkeiAveChQty  < aveChQty){
                                PlayerPrefs.SetInt("jinkeiAveChQty", aveChQty);
                            }
                            if (nowHeiryoku < heiryoku) {
                                PlayerPrefs.SetInt("jinkeiHeiryoku", heiryoku);
                            }

                            //Soudaisyo
                            Jinkei jinkei = new Jinkei();
                            soudaisyo = jinkei.soudaisyoBusyoIdCheck(soudaisyo, selectedJinkei);
                            PlayerPrefs.SetInt("soudaisyo1", soudaisyo);

                            PlayerPrefs.SetInt("jinkei", selectedJinkei);

                        }
                        else if (selectedJinkei == 2) {
                            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
                                if (obs.transform.childCount > 0) {
                                    //Get Name 
                                    int childId = int.Parse(obs.transform.GetChild(0).name);
                                    string mapId = selectedJinkei.ToString() + "map" + obs.name.Substring(4);
                                    //Set Key
                                    PlayerPrefs.SetInt(mapId, childId);
                                }
                                else {
                                    //Delete Key
                                    string mapId = selectedJinkei.ToString() + "map" + obs.name.Substring(4);
                                    PlayerPrefs.DeleteKey(mapId);
                                }
                            }
                            int jinkeiBusyoQty = int.Parse(GameObject.Find("jinkeiQtyValue").GetComponent<Text>().text);
                            aveLv = totalLv / jinkeiBusyoQty;
                            aveChLv = totalChLv / jinkeiBusyoQty;
                            aveChQty = totalChQty / jinkeiBusyoQty;
                            int heiryoku = int.Parse(GameObject.Find("totalHpValue").GetComponent<Text>().text);
                            int nowJinkeiAveLv = PlayerPrefs.GetInt("jinkeiAveLv");
                            int nowJinkeiAveChLv = PlayerPrefs.GetInt("jinkeiAveChLv");
                            int nowJinkeiBusyoQty = PlayerPrefs.GetInt("jinkeiBusyoQty");
                            int nowJinkeiAveChQty = PlayerPrefs.GetInt("jinkeiAveChQty");
                            int nowHeiryoku = PlayerPrefs.GetInt("jinkeiHeiryoku");
                            PlayerPrefs.SetInt("pvpHeiryoku", heiryoku);

                            if (nowJinkeiBusyoQty < jinkeiBusyoQty) {
                                PlayerPrefs.SetInt("jinkeiBusyoQty", jinkeiBusyoQty);
                            }
                            if (nowJinkeiAveLv < aveLv) {
                                PlayerPrefs.SetInt("jinkeiAveLv", aveLv);
                            }
                            if (nowJinkeiAveChLv < aveChLv) {
                                PlayerPrefs.SetInt("jinkeiAveChLv", aveChLv);
                            }
                            if (nowJinkeiAveChQty < aveChQty) {
                                PlayerPrefs.SetInt("jinkeiAveChQty", aveChQty);
                            }
                            if (nowHeiryoku < heiryoku) {
                                PlayerPrefs.SetInt("jinkeiHeiryoku", heiryoku);
                            }

                            //Soudaisyo
                            Jinkei jinkei = new Jinkei();
                            soudaisyo = jinkei.soudaisyoBusyoIdCheck(soudaisyo, selectedJinkei);
                            PlayerPrefs.SetInt("soudaisyo2", soudaisyo);

                            PlayerPrefs.SetInt("jinkei", selectedJinkei);

                        }
                        else if (selectedJinkei == 3) {
                            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
                                if (obs.transform.childCount > 0) {
                                    //Get Name 
                                    int childId = int.Parse(obs.transform.GetChild(0).name);
                                    string mapId = selectedJinkei.ToString() + "map" + obs.name.Substring(4);
                                    //Set Key
                                    PlayerPrefs.SetInt(mapId, childId);
                                }
                                else {
                                    //Delete Key
                                    string mapId = selectedJinkei.ToString() + "map" + obs.name.Substring(4);
                                    PlayerPrefs.DeleteKey(mapId);
                                }
                            }

                            int jinkeiBusyoQty = int.Parse(GameObject.Find("jinkeiQtyValue").GetComponent<Text>().text);
                            aveLv = totalLv / jinkeiBusyoQty;
                            aveChLv = totalChLv / jinkeiBusyoQty;
                            aveChQty = totalChQty / jinkeiBusyoQty;
                            int heiryoku = int.Parse(GameObject.Find("totalHpValue").GetComponent<Text>().text);
                            int nowJinkeiAveLv = PlayerPrefs.GetInt("jinkeiAveLv");
                            int nowJinkeiAveChLv = PlayerPrefs.GetInt("jinkeiAveChLv");
                            int nowJinkeiBusyoQty = PlayerPrefs.GetInt("jinkeiBusyoQty");
                            int nowJinkeiAveChQty = PlayerPrefs.GetInt("jinkeiAveChQty");
                            int nowHeiryoku = PlayerPrefs.GetInt("jinkeiHeiryoku");
                            PlayerPrefs.SetInt("pvpHeiryoku", heiryoku);

                            if (nowJinkeiBusyoQty  < jinkeiBusyoQty){
                                PlayerPrefs.SetInt("jinkeiBusyoQty", jinkeiBusyoQty);
                            }
                            if (nowJinkeiAveLv  < aveLv){
                                PlayerPrefs.SetInt("jinkeiAveLv", aveLv);
                            }
                            if (nowJinkeiAveChLv  < aveChLv){
                                PlayerPrefs.SetInt("jinkeiAveChLv", aveChLv);                           
                            }
                            if (nowJinkeiAveChQty  < aveChQty){
                                PlayerPrefs.SetInt("jinkeiAveChQty", aveChQty);
                            }
                            if (nowHeiryoku < heiryoku) {
                                PlayerPrefs.SetInt("jinkeiHeiryoku", heiryoku);
                            }

                            //Soudaisyo
                            Jinkei jinkei = new Jinkei();
                            soudaisyo = jinkei.soudaisyoBusyoIdCheck(soudaisyo, selectedJinkei);
                            PlayerPrefs.SetInt("soudaisyo3", soudaisyo);

                            PlayerPrefs.SetInt("jinkei", selectedJinkei);

                        }
                        else if (selectedJinkei == 4) {
                            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
                                if (obs.transform.childCount > 0) {
                                    //Get Name 
                                    int childId = int.Parse(obs.transform.GetChild(0).name);
                                    string mapId = selectedJinkei.ToString() + "map" + obs.name.Substring(4);
                                    //Set Key
                                    PlayerPrefs.SetInt(mapId, childId);
                                }
                                else {
                                    //Delete Key
                                    string mapId = selectedJinkei.ToString() + "map" + obs.name.Substring(4);
                                    PlayerPrefs.DeleteKey(mapId);
                                }
                            }

                            int jinkeiBusyoQty = int.Parse(GameObject.Find("jinkeiQtyValue").GetComponent<Text>().text);
                            aveLv = totalLv / jinkeiBusyoQty;
                            aveChLv = totalChLv / jinkeiBusyoQty;
                            aveChQty = totalChQty / jinkeiBusyoQty;
                            int heiryoku = int.Parse(GameObject.Find("totalHpValue").GetComponent<Text>().text);
                            int nowJinkeiAveLv = PlayerPrefs.GetInt("jinkeiAveLv");
                            int nowJinkeiAveChLv = PlayerPrefs.GetInt("jinkeiAveChLv");
                            int nowJinkeiBusyoQty = PlayerPrefs.GetInt("jinkeiBusyoQty");
                            int nowJinkeiAveChQty = PlayerPrefs.GetInt("jinkeiAveChQty");
                            int nowHeiryoku = PlayerPrefs.GetInt("jinkeiHeiryoku");
                            PlayerPrefs.SetInt("pvpHeiryoku", heiryoku);

                            if (nowJinkeiBusyoQty < jinkeiBusyoQty) {
                                PlayerPrefs.SetInt("jinkeiBusyoQty", jinkeiBusyoQty);
                            }
                            if (nowJinkeiAveLv < aveLv) {
                                PlayerPrefs.SetInt("jinkeiAveLv", aveLv);
                            }
                            if (nowJinkeiAveChLv < aveChLv) {
                                PlayerPrefs.SetInt("jinkeiAveChLv", aveChLv);
                            }
                            if (nowJinkeiAveChQty < aveChQty) {
                                PlayerPrefs.SetInt("jinkeiAveChQty", aveChQty);
                            }
                            if (nowHeiryoku < heiryoku) {
                                PlayerPrefs.SetInt("jinkeiHeiryoku", heiryoku);
                            }

                            //Soudaisyo
                            Jinkei jinkei = new Jinkei();
                            soudaisyo = jinkei.soudaisyoBusyoIdCheck(soudaisyo, selectedJinkei);
                            PlayerPrefs.SetInt("soudaisyo4", soudaisyo);
                            PlayerPrefs.SetInt("jinkei", selectedJinkei);
                        }

                        PlayerPrefs.SetBool("questSpecialFlg6", true);
                        PlayerPrefs.Flush();
                        Application.LoadLevel("mainStage");

                        //Set Data
                        if (Application.internetReachability != NetworkReachability.NotReachable) {
                            if(GameObject.Find("DataStore")) {
                                DataPvP DataPvP = GameObject.Find("DataStore").GetComponent<DataPvP>();
                                DataJinkei DataJinkei = GameObject.Find("DataStore").GetComponent<DataJinkei>();
                                string userId = PlayerPrefs.GetString("userId");
                                DataPvP.UpdatePvP(userId);
                                DataJinkei.UpdateJinkei(userId);
                            }
                        }
                    }
                }
            }
        }
    }

    
}
