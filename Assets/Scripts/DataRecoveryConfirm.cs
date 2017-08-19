using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class DataRecoveryConfirm : MonoBehaviour {

    public GameObject back;
    public GameObject msg;
    

    //pvp jinkei
    public List<int> busyoList = new List<int>();
    public List<int> lvList = new List<int>();
    public List<string> heiList = new List<string>();
    public List<string> kahouList = new List<string>();
    public List<int> senpouLvList = new List<int>();
    public List<int> sakuLvList = new List<int>();


    public void OnClick() {

        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();

        if (name == "YesButton") {
            audioSources[0].Play();
            RecoveryDataStore RecoveryDataStore = GameObject.Find("RecoveryDataStore").GetComponent<RecoveryDataStore>();

            /***user id***/
            PlayerPrefs.SetBool("tutorialDoneFlg",true);
            PlayerPrefs.SetInt("kuniLv", RecoveryDataStore.kuniLv);
            Exp expScript = new Exp();
            int totalExp = expScript.getTotalExp(RecoveryDataStore.kuniLv);
            PlayerPrefs.SetInt("exp", totalExp);
            int jinkeiLimit = expScript.getJinkeiLimit(RecoveryDataStore.kuniLv);
            int stockLimit = expScript.getStockLimit(RecoveryDataStore.kuniLv);
            PlayerPrefs.SetInt("jinkeiLimit", jinkeiLimit);
            PlayerPrefs.SetInt("stockLimit", stockLimit);
            PlayerPrefs.SetString("userId", RecoveryDataStore.inputUserId);
            if (RecoveryDataStore.addJinkei1) {
                PlayerPrefs.SetBool("addJinkei1", true);
            }else {
                PlayerPrefs.SetBool("addJinkei1", false);
            }
            if (RecoveryDataStore.addJinkei2) {
                PlayerPrefs.SetBool("addJinkei2", true);
            }else {
                PlayerPrefs.SetBool("addJinkei2", false);
            }
            if (RecoveryDataStore.addJinkei3) {
                PlayerPrefs.SetBool("addJinkei3", true);
            }else {
                PlayerPrefs.SetBool("addJinkei3", false);
            }
            if (RecoveryDataStore.addJinkei4) {
                PlayerPrefs.SetBool("addJinkei4", true);
            }else {
                PlayerPrefs.SetBool("addJinkei4", false);
            }

            //pvp jinkei
            string myBusyoString = PlayerPrefs.GetString("myBusyo");
            List<string> myBusyoList = new List<string>();
            char[] delimiterChars = { ',' };
            if (myBusyoString != null && myBusyoString != "") {
                if (myBusyoString.Contains(",")) {
                    myBusyoList = new List<string>(myBusyoString.Split(delimiterChars));
                }else {
                    myBusyoList.Add(myBusyoString);
                }
            }

            //zukan
            string zukanBusyoHst = PlayerPrefs.GetString("zukanBusyoHst");
            List<string> zukanBusyoHstList = new List<string>();
            if(zukanBusyoHst != null && zukanBusyoHst != "") {
                if (zukanBusyoHst.Contains(",")) {
                    zukanBusyoHstList = new List<string>(zukanBusyoHst.Split(delimiterChars));
                }else {
                    zukanBusyoHstList.Add(zukanBusyoHst);
                }
            }

            //Daimyo Busyo History       
            Daimyo daimyo = new Daimyo();
            string gacyaDaimyoHst = PlayerPrefs.GetString("gacyaDaimyoHst");
            List<string> gacyaDaimyoHstList = new List<string>();
            if (gacyaDaimyoHst != null && gacyaDaimyoHst != "") {
                if (gacyaDaimyoHst.Contains(",")) {
                    gacyaDaimyoHstList = new List<string>(gacyaDaimyoHst.Split(delimiterChars));
                }else {
                    gacyaDaimyoHstList.Add(gacyaDaimyoHst);
                }
            }
            
            int count = 0;
            for (int i=0;i<RecoveryDataStore.busyoList.Count; i++) {
                int busyoId = RecoveryDataStore.busyoList[i];
                
                if(busyoId != 0) {                                    
                    //add busyo             
                    if (!myBusyoList.Contains(busyoId.ToString())) {
                        if (myBusyoString == null || myBusyoString == "") {
                            myBusyoString = busyoId.ToString();
                        }else {
                            myBusyoString = myBusyoString + "," + busyoId.ToString();
                        }
                        PlayerPrefs.SetString("myBusyo", myBusyoString);
                    }                       
                    
                    //add zukan
                    if (!zukanBusyoHstList.Contains(busyoId.ToString())) {
                        if (zukanBusyoHst == null || zukanBusyoHst == "") {
                            zukanBusyoHst = busyoId.ToString();
                        }else {
                            zukanBusyoHst = zukanBusyoHst + "," + busyoId.ToString();
                        }
                        PlayerPrefs.SetString("zukanBusyoHst", zukanBusyoHst);
                    }

                    //add daimyo busyo
                    if (daimyo.daimyoBusyoCheck(busyoId)) {
                        if (!gacyaDaimyoHstList.Contains(busyoId.ToString())) {
                            if (gacyaDaimyoHst == null || gacyaDaimyoHst == "") {
                                gacyaDaimyoHst = busyoId.ToString();
                            }else {
                                gacyaDaimyoHst = gacyaDaimyoHst + "," + busyoId.ToString();
                            }
                            PlayerPrefs.SetString("gacyaDaimyoHst", gacyaDaimyoHst);
                        }
                    }
                    
                    //add parametor
                    int lv = RecoveryDataStore.lvList[count];
                    string hei = RecoveryDataStore.heiList[count];
                    int senpouLv = RecoveryDataStore.senpouLvList[count];
                    int sakuLv = RecoveryDataStore.sakuLvList[count];
                    string kahou = RecoveryDataStore.kahouList[count];

                    //lv
                    PlayerPrefs.SetInt(busyoId.ToString(), lv);

                    //hei
                    string heiTmp = "hei" + busyoId.ToString();                    
                    PlayerPrefs.SetString(heiTmp, hei);

                    //senpou
                    string senpou = "senpou" + busyoId.ToString();
                    PlayerPrefs.SetInt(senpou, senpouLv); //Lv

                    //saku
                    string saku = "saku" + busyoId.ToString();
                    PlayerPrefs.SetInt(saku, sakuLv); //Lv

                    //kahou
                    string kahouTmp = "kahou" + busyoId.ToString();
                    PlayerPrefs.SetString(kahouTmp, kahou);

                    //exp
                    string exp = "exp" + busyoId.ToString();
                    int totalBusyoExp = expScript.getExpforNextLv(lv - 1);
                    PlayerPrefs.SetInt(exp, totalBusyoExp);

                    count = count + 1;
                }
            }
            PlayerPrefs.Flush();

            //Add Qty & PvP Name
            string myBusyoStringCount = PlayerPrefs.GetString("myBusyo");
            List<string> myBusyoListCount = new List<string>();
            if (myBusyoStringCount.Contains(",")) {
                myBusyoListCount = new List<string>(myBusyoStringCount.Split(delimiterChars));
            }else {
                myBusyoListCount.Add(myBusyoStringCount);
            }
            PlayerPrefs.SetInt("myBusyoQty", myBusyoListCount.Count);
            string userName = RecoveryDataStore.userName;
            if (userName == "") userName = "unknown";
            PlayerPrefs.SetString("PvPName", userName);
            PlayerPrefs.Flush();            

            //add data recovery history
            RecoveryDataStore.InsertDataRecovery(RecoveryDataStore.inputUserId);


            //Final Process
            audioSources[3].Play();
            Destroy(back.gameObject);
            Destroy(msg.gameObject);
            GameObject.Find("Start").GetComponent<Button>().enabled = false;
            Message msgScript = new Message();
            msgScript.makeMessage(msgScript.getMessage(151));

        }else {
            RecoveryDataStore RecoveryDataStore = GameObject.Find("RecoveryDataStore").GetComponent<RecoveryDataStore>();
            RecoveryDataStore.ResetValue();

            audioSources[1].Play();
            Destroy(back.gameObject);
            Destroy(msg.gameObject);
        }
    }

}
