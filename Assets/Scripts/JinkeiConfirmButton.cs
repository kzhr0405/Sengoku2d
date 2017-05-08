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

            GameObject Slot13 = GameObject.Find("copiedJinkeiView").transform.FindChild("Slot13").gameObject;
            int chldBusyoId = 0;
            foreach (Transform chld in Slot13.transform) {
                chldBusyoId = int.Parse(chld.name);
            }

            //Set Key
            PlayerPrefs.SetInt("1map13", chldBusyoId);
            PlayerPrefs.SetInt("tutorialId",12);
            
            int jinkeiBusyoQty = int.Parse(GameObject.Find("jinkeiQtyValue").GetComponent<Text>().text);
            aveLv = totalLv / jinkeiBusyoQty;
            aveChLv = totalChLv / jinkeiBusyoQty;
            aveChQty = totalChQty / jinkeiBusyoQty;
            PlayerPrefs.SetInt("jinkeiAveLv", aveLv);
            PlayerPrefs.SetInt("jinkeiAveChLv", aveChLv);
            PlayerPrefs.SetInt("jinkeiBusyoQty", jinkeiBusyoQty);
            PlayerPrefs.SetInt("jinkeiAveChQty", aveChQty);

            int heiryoku = int.Parse(GameObject.Find("totalHpValue").GetComponent<Text>().text);
            PlayerPrefs.SetInt("jinkeiHeiryoku", heiryoku);
            PlayerPrefs.SetInt("pvpHeiryoku", heiryoku);

            //Kassen
            PlayerPrefs.SetBool("isAttackedFlg",true);
            PlayerPrefs.SetInt("activeKuniId",1);
            PlayerPrefs.SetInt("activeStageId", 10);
            PlayerPrefs.SetInt("activePowerType", 1);
            PlayerPrefs.SetInt("pSRLv", 20);
            PlayerPrefs.SetInt("emap14", 24);
            PlayerPrefs.SetInt("emap13", 202);
            PlayerPrefs.SetInt("emap8", 188);
            PlayerPrefs.SetString("playerEngunList", "1-158-50-20-10:1-141-50-20-10:1-52-50-20-10");
            
            PlayerPrefs.Flush();

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
                        PlayerPrefs.SetInt("soudaisyo4", soudaisyo);
                        PlayerPrefs.SetInt("jinkei", selectedJinkei);
                    }

                    PlayerPrefs.SetBool("questSpecialFlg6", true);
                    PlayerPrefs.Flush();
                    Application.LoadLevel("mainStage");
                }
                
            }
        }
    }
}
