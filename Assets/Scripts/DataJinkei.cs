using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DataJinkei : MonoBehaviour {

    public bool RegisteredFlg = false;

    public void InsertJinkei(string userId) {
        int jinkeiId = PlayerPrefs.GetInt("jinkei");
        int jinkeiHeiryoku = PlayerPrefs.GetInt("jinkeiHeiryoku");
        string soudaisyo = "soudaisyo" + jinkeiId.ToString();
        int soudaisyoBusyoId = PlayerPrefs.GetInt(soudaisyo);

        NCMBObject jinkeiClass = new NCMBObject("pvpJinkei");
        jinkeiClass["userId"] = userId;
        jinkeiClass["jinkeiId"] = jinkeiId.ToString();
        jinkeiClass["jinkeiHeiryoku"] = jinkeiHeiryoku;
        jinkeiClass["soudaisyo"] = soudaisyoBusyoId;

        List<int> lvList = new List<int>();
        List<string> heiList = new List<string>();
        List<int> senpouLvList = new List<int>();        
        List<int> sakuLvList = new List<int>();
        List<string> kahouList = new List<string>();
        List<int> sakuList = new List<int>(); //saku + gokui
        BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();

        for (int i=1; i<26; i++) {
            string jinkeiMapId = jinkeiId.ToString() + "map" + i.ToString();
            string jinkeiMapId2 = "map" + i.ToString();
            int jinkeiBusyoId = PlayerPrefs.GetInt(jinkeiMapId);
            jinkeiClass[jinkeiMapId2] = jinkeiBusyoId;
            if(jinkeiBusyoId!=0) {
                int busyoLv = PlayerPrefs.GetInt(jinkeiBusyoId.ToString());
                lvList.Add(busyoLv);

                string heiTmp = "hei" + jinkeiBusyoId.ToString();
                string heiString = PlayerPrefs.GetString(heiTmp);
                heiList.Add(heiString);

                string senpouTmp = "senpou" + jinkeiBusyoId.ToString();
                int senpouLv = PlayerPrefs.GetInt(senpouTmp);
                senpouLvList.Add(senpouLv);

                string gokuiTmp = "gokui" + jinkeiBusyoId.ToString();
                int gokuiId = PlayerPrefs.GetInt(gokuiTmp);
                if (gokuiId != 0) {
                    sakuList.Add(gokuiId);
                }else {
                    sakuList.Add(BusyoInfoGet.getSakuId(jinkeiBusyoId));
                }
                
                string sakuTmp = "saku" + jinkeiBusyoId.ToString();
                int sakuLv = PlayerPrefs.GetInt(sakuTmp);
                sakuLvList.Add(sakuLv);

                string kahouTmp = "kahou" + jinkeiBusyoId.ToString();
                string kahouString = PlayerPrefs.GetString(kahouTmp);
                kahouList.Add(kahouString);
            }
        }
        jinkeiClass["lvList"] = lvList;
        jinkeiClass["heiList"] = heiList;
        jinkeiClass["senpouLvList"] = senpouLvList;
        jinkeiClass["sakuList"] = sakuList;
        jinkeiClass["sakuLvList"] = sakuLvList;
        jinkeiClass["kahouList"] = kahouList;

        jinkeiClass.SaveAsync();
        RegisteredFlg = true;
    }



    public void UpdateJinkei(string userId) {

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpJinkei");
        query.WhereEqualTo("userId", userId);

        List<int> lvList = new List<int>();
        List<string> heiList = new List<string>();
        List<int> senpouLvList = new List<int>();
        List<int> sakuLvList = new List<int>();
        List<string> kahouList = new List<string>();
        List<int> sakuList = new List<int>(); //saku + gokui
        BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count == 0) { //never registered
                    InsertJinkei(userId);
                }else { //Update              
                    int jinkeiId = PlayerPrefs.GetInt("jinkei");                    
                    int pvpHeiryoku = PlayerPrefs.GetInt("pvpHeiryoku");
                    if (pvpHeiryoku == 0) {
                        pvpHeiryoku = PlayerPrefs.GetInt("jinkeiHeiryoku");
                    }
                    string soudaisyo = "soudaisyo" + jinkeiId.ToString();
                    int soudaisyoBusyoId = PlayerPrefs.GetInt(soudaisyo);

                    objList[0]["jinkeiId"] = jinkeiId.ToString();
                    objList[0]["jinkeiHeiryoku"] = pvpHeiryoku;
                    objList[0]["soudaisyo"] = soudaisyoBusyoId;

                    for (int i = 1; i < 26; i++) {
                        string jinkeiMapId = jinkeiId.ToString() + "map" + i.ToString();
                        string jinkeiMapId2 = "map" + i.ToString();
                        int jinkeiBusyoId = PlayerPrefs.GetInt(jinkeiMapId);
                        objList[0][jinkeiMapId2] = jinkeiBusyoId;

                        if (jinkeiBusyoId != 0) {
                            int busyoLv = PlayerPrefs.GetInt(jinkeiBusyoId.ToString());
                            lvList.Add(busyoLv);

                            string heiTmp = "hei" + jinkeiBusyoId.ToString();
                            string heiString = PlayerPrefs.GetString(heiTmp);
                            heiList.Add(heiString);

                            string senpouTmp = "senpou" + jinkeiBusyoId.ToString();
                            int senpouLv = PlayerPrefs.GetInt(senpouTmp);
                            senpouLvList.Add(senpouLv);

                            string gokuiTmp = "gokui" + jinkeiBusyoId.ToString();
                            int gokuiId = PlayerPrefs.GetInt(gokuiTmp);
                            if (gokuiId != 0) {
                                sakuList.Add(gokuiId);
                            }else {
                                sakuList.Add(BusyoInfoGet.getSakuId(jinkeiBusyoId));
                            }

                            string sakuTmp = "saku" + jinkeiBusyoId.ToString();
                            int sakuLv = PlayerPrefs.GetInt(sakuTmp);
                            sakuLvList.Add(sakuLv);

                            string kahouTmp = "kahou" + jinkeiBusyoId.ToString();
                            string kahouString = PlayerPrefs.GetString(kahouTmp);
                            kahouList.Add(kahouString);
                        }

                    }
                    objList[0]["lvList"] = lvList;
                    objList[0]["heiList"] = heiList;
                    objList[0]["senpouLvList"] = senpouLvList;
                    objList[0]["sakuList"] = sakuList;
                    objList[0]["sakuLvList"] = sakuLvList;
                    objList[0]["kahouList"] = kahouList;

                    objList[0].SaveAsync();
                    RegisteredFlg = true;
                }
            }
        });
    }

    public int GetJinkeCount() {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpJinkei");
        int returnValue = 0;
        query.CountAsync((int count, NCMBException e) => {

            if (e != null) {
                //件数取得失敗
                returnValue = count;
            }else {
                //件数取得成功
                returnValue = count;
            }
        });

        return returnValue;
    }
}
