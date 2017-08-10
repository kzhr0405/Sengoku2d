using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class PvPDataStore : MonoBehaviour {

    //total
    public int myJinkeiHeiryoku = 0;
    public int pvpCount = -1;
    public int hpRank = -1;
    public string userId = "";
    public string myUserName = "";
    public int totalWinNo = -1;
    public int atkNo = -1;
    public int atkWinNo = -1;
    public int dfcNo = -1;
    public int dfcWinNo = -1;
    public int winRank = -1;

    //weekly
    public int pvpCountWeekly = -1;
    public int atkNoWeekly = -1;
    public int atkWinNoWeekly = -1;
    public int dfcNoWeekly = -1;
    public int dfcWinNoWeekly = -1;
    public int totalPtWeekly = -99999;
    public int ptRankWeekly = -1;

    //pvp random match
    public List<string> pvpUserIdList = new List<string>();
    public List<string> pvpUserNameList = new List<string>();
    public List<int> pvpSoudaisyoList = new List<int>();
    public List<int> pvpKuniLvList = new List<int>();
    public List<int> pvpHpList = new List<int>();
    public List<int> pvpPtList = new List<int>();
    public List<int> pvpPtRankList = new List<int>();
    public bool matchedFlg = false;
    public int matchCount = 0;

    //pvp jinkei
    public List<int> PvP1BusyoList = new List<int>();
    public List<int> PvP1LvList = new List<int>();
    public List<string> PvP1HeiList = new List<string>();
    public List<int> PvP1SenpouLvList = new List<int>();
    public List<int> PvP1SakuLvList = new List<int>();
    public List<string> PvP1KahouList = new List<string>();
    public int soudaisyo1;

    public List<int> PvP2BusyoList = new List<int>();
    public List<int> PvP2LvList = new List<int>();
    public List<string> PvP2HeiList = new List<string>();
    public List<int> PvP2SenpouLvList = new List<int>();
    public List<int> PvP2SakuLvList = new List<int>();
    public List<string> PvP2KahouList = new List<string>();
    public int soudaisyo2;

    public List<int> PvP3BusyoList = new List<int>();
    public List<int> PvP3LvList = new List<int>();
    public List<string> PvP3HeiList = new List<string>();
    public List<int> PvP3SenpouLvList = new List<int>();
    public List<int> PvP3SakuLvList = new List<int>();
    public List<string> PvP3KahouList = new List<string>();
    public int soudaisyo3;

    //pvp tran
    public bool PvPAtkNoFlg = false;
    public string enemyUserId = "";
    public string enemyUserName = "";
    public int getPt = 0;
    public bool doneMinusUpdatePtFlg = false;
    public bool donePlusUpdatePtFlg = false;

    //Top3 > Top10
    public List<string> Top3WinNameList = new List<string>();
    public List<string> Top3WinUserIdList = new List<string>();
    public List<int> Top3WinRankList = new List<int>();
    public List<int> Top3WinBusyoList = new List<int>();
    public List<int> Top3WinQtyList = new List<int>();

    public List<string> Top3HPNameList = new List<string>();
    public List<string> Top3HPUserIdList = new List<string>();
    public List<int> Top3HPRankList = new List<int>();
    public List<int> Top3HPBusyoList = new List<int>();
    public List<int> Top3HPQtyList = new List<int>();


    //Top 10 Weekly
    public List<string> Top10PtWeeklyNameList = new List<string>();
    public List<string> Top10PtWeeklyUserIdList = new List<string>();
    public List<int> Top10PtWeeklyRankList = new List<int>();
    public List<int> Top10PtWeeklyBusyoList = new List<int>();
    public List<int> Top10PtWeeklyQtyList = new List<int>();
    public List<int> Top10PtWeeklyHeiList = new List<int>();
    public List<int> Top10PtWeeklyWinList = new List<int>();
    public List<int> Top10PtWeeklyBattleList = new List<int>();


    //Time
    public PvPTimer PvPTimer;
    public int todayNCMB;

    public void Start() {
        //Get Current Time Script
        PvPTimer = GameObject.Find("Timer").GetComponent<PvPTimer>();
    }




    /* Total Start */
    public void GetPvPCount() {
        //PvPCount
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.CountAsync((int count, NCMBException e) => {
            if (e != null) {
                //件数取得失敗
                Debug.Log(count);
            }else {
                //件数取得成功
                pvpCount = count;                
            }
        });
    }

    public void GetHpRank() {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        myJinkeiHeiryoku = PlayerPrefs.GetInt("jinkeiHeiryoku");
        query.WhereGreaterThan("jinkeiHeiryoku", myJinkeiHeiryoku);
        query.CountAsync((int count, NCMBException e) => {
            if (e != null) {
                //件数取得失敗
            }else {
                //件数取得成功
                hpRank = count + 1;// 自分よりスコアが上の人がn人いたら自分はn+1位
            }
        });
    }

    public void GetMyPvP() {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        userId = PlayerPrefs.GetString("userId");
        query.WhereEqualTo("userId", userId);
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            if (e == null) {
                foreach (NCMBObject obj in objList) {
                    atkNo = System.Convert.ToInt32(obj["atkNo"]);
                    atkWinNo = System.Convert.ToInt32(obj["atkWinNo"]);
                    dfcNo = System.Convert.ToInt32(obj["dfcNo"]);
                    dfcWinNo = System.Convert.ToInt32(obj["dfcWinNo"]);
                    totalWinNo = System.Convert.ToInt32(obj["totalWinNo"]);
                }
            }
        });
    }

    
    public void GetWinRank(int totalWinNo) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.WhereGreaterThan("totalWinNo", totalWinNo);
        query.CountAsync((int count, NCMBException e) => {
            if (e != null) {
                //件数取得失敗
            }else {
                //件数取得成功
                winRank = count + 1;// 自分よりスコアが上の人がn人いたら自分はn+1位
                
            }
        });
    }
    /* Total End */

    /* Weekly Start */
    public void GetPvPCountWeekly(int todayNCMB) {

        Debug.Log(todayNCMB);

        //PvPCount
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpTmp");

        //date
        query.WhereGreaterThanOrEqualTo("endDate", todayNCMB);

        query.CountAsync((int count, NCMBException e) => {
            if (e != null) {
                //件数取得失敗
                Debug.Log("pvpTmp:" + count);
            }else {
                //件数取得成功
                pvpCountWeekly = count;
            }
        });
    }
    
    public void GetMyPvPWeekly(int startDateNCMB, int endDateNCMB, int todayNCMB) {

        //Common Info.
        string myUserName = PlayerPrefs.GetString("PvPName");
        int pvpHeiryoku = PlayerPrefs.GetInt("pvpHeiryoku");
        if (pvpHeiryoku == 0) {
            pvpHeiryoku = PlayerPrefs.GetInt("jinkeiHeiryoku");
        }
        int kuniLv = PlayerPrefs.GetInt("kuniLv");
        int jinkei = PlayerPrefs.GetInt("jinkei");
        string soudaisyoTmp = "soudaisyo" + jinkei.ToString();
        int soudaisyo = PlayerPrefs.GetInt(soudaisyoTmp);
        
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpTmp");
        userId = PlayerPrefs.GetString("userId");
        query.WhereEqualTo("userId", userId);

        //date
        query.WhereGreaterThanOrEqualTo("endDate", todayNCMB);
        query.OrderByDescending("endDate");
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null)
            {

                if (objList.Count == 0)
                { //never registered
                    InsertPvPWeekly(userId, startDateNCMB, endDateNCMB, myUserName, kuniLv, soudaisyo, pvpHeiryoku);
                    atkNoWeekly = 0;
                    atkWinNoWeekly = 0;
                    dfcNoWeekly = 0;
                    dfcWinNoWeekly = 0;
                    totalPtWeekly = 1000;
                }
                else
                { //registered
                    //Update info.
                    UpdatePvPWeekly(userId, myUserName, todayNCMB, kuniLv, soudaisyo, pvpHeiryoku);

                    //Get info.
                    foreach (NCMBObject obj in objList)
                    {
                        atkNoWeekly = System.Convert.ToInt32(obj["atkNo"]);
                        atkWinNoWeekly = System.Convert.ToInt32(obj["atkWinNo"]);
                        dfcNoWeekly = System.Convert.ToInt32(obj["dfcNo"]);
                        dfcWinNoWeekly = System.Convert.ToInt32(obj["dfcWinNo"]);
                        totalPtWeekly = System.Convert.ToInt32(obj["totalPt"]);

                    }
                }
            }
        });
    }

    public void InsertPvPWeekly(string userId, int startDateNCMB, int endDateNCMB, string userName, int kuniLv, int soudaisyo, int jinkeiHeiryoku) {
        NCMBObject pvpWeekly = new NCMBObject("pvpTmp");
        pvpWeekly["userId"] = userId;
        pvpWeekly["atkNo"] = 0;
        pvpWeekly["dfcNo"] = 0;
        pvpWeekly["atkWinNo"] = 0;
        pvpWeekly["dfcWinNo"] = 0;
        pvpWeekly["totalWinNo"] = 0;
        pvpWeekly["totalPt"] = 1000;
        pvpWeekly["startDate"] = startDateNCMB;
        pvpWeekly["endDate"] = endDateNCMB;
        pvpWeekly["userName"] = userName;
        pvpWeekly["kuniLv"] = kuniLv;
        pvpWeekly["soudaisyo"] = soudaisyo;
        pvpWeekly["jinkeiHeiryoku"] = jinkeiHeiryoku;
        pvpWeekly["rewardFlag"] = false;
        pvpWeekly.SaveAsync();
    }

    public void UpdatePvPWeekly(string userId, string userName, int todayNCMB, int kuniLv, int soudaisyo, int jinkeiHeiryoku) {

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpTmp");
        query.WhereEqualTo("userId", userId);
        query.WhereGreaterThanOrEqualTo("endDate", todayNCMB);
        query.OrderByDescending("endDate");

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count != 0) {
                    objList[0]["userName"] = userName;
                    objList[0]["kuniLv"] = kuniLv;
                    objList[0]["soudaisyo"] = soudaisyo;
                    objList[0]["jinkeiHeiryoku"] = jinkeiHeiryoku;

                    objList[0].SaveAsync();
                }
            }
        });
    }


    /*
    public void GetWinRankWeekly(int totalWinNo, bool enemyFlg) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpWeekly");
        query.WhereGreaterThan("totalWinNo", totalWinNo);
        query.CountAsync((int count, NCMBException e) => {
            if (e != null) {
                //件数取得失敗
            }else {
                //件数取得成功
                if (!enemyFlg) {
                    winRankWeekly = count + 1;// 自分よりスコアが上の人がn人いたら自分はn+1位
                }else {
                    pvpWinRankList.Add(count + 1);
                }
            }
        });
    }
    */

    public void GetPtRankWeekly(int totalPt, bool enemyFlg, int todayNCMB) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpTmp");
        query.WhereGreaterThan("totalPt", totalPt);
        
        //date
        query.WhereGreaterThanOrEqualTo("endDate", todayNCMB);

        query.CountAsync((int count, NCMBException e) => {
            if (e != null) {
                //件数取得失敗
            }else {
                //件数取得成功
                if (!enemyFlg) {
                    ptRankWeekly = count + 1;// 自分よりスコアが上の人がn人いたら自分はn+1位
                }else {
                    pvpPtRankList.Add(count + 1);
                }
            }
        });
    }

    /* Weekly End */







    /* Matching Start */
    public void GetRandomEnemy(string myUserId, int HpBase, int startDateNCMB, int endDateNCMB, int todayNCMB, int myTotalPt) {

        //Test
        //myTotalPt = 1000000;

        NCMBQuery<NCMBObject> queryPvPTmp = new NCMBQuery<NCMBObject>("pvpTmp");
        queryPvPTmp.WhereNotEqualTo("userId", myUserId);
        queryPvPTmp.WhereGreaterThanOrEqualTo("endDate", todayNCMB);
        queryPvPTmp.WhereLessThanOrEqualTo("totalPt", myTotalPt * 2);
        queryPvPTmp.WhereGreaterThanOrEqualTo("totalPt", myTotalPt/2);
        queryPvPTmp.WhereLessThanOrEqualTo("jinkeiHeiryoku", HpBase * 2);
        queryPvPTmp.WhereGreaterThanOrEqualTo("jinkeiHeiryoku", HpBase / 2);

        queryPvPTmp.CountAsync((int count, NCMBException eCount) => {
            if (eCount == null) {
                matchCount = count;
                int rdmSkip = UnityEngine.Random.Range(0, matchCount) - 3;
                if (rdmSkip < 0) rdmSkip = 0;

                /*From PvP*/
                
                if (matchCount == 0) {
                    NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
                    query.WhereNotEqualTo("userId", myUserId);
                    query.WhereLessThanOrEqualTo("jinkeiHeiryoku", Mathf.CeilToInt((float)HpBase * 1.5f));
                    query.WhereGreaterThanOrEqualTo("jinkeiHeiryoku", Mathf.CeilToInt((float)HpBase / 1.5f));
                    query.WhereNotEqualTo("atkNo", 0);

                    query.CountAsync((int pvpCount, NCMBException PvPexpection) => {
                        if (PvPexpection == null) {
                            matchCount = pvpCount;
                           
                            if (matchCount == 0) {
                                matchedFlg = true;
                            }else {
                                //Random Id
                                query.Skip = rdmSkip;
                                query.Limit = 3;

                                query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
                                    if (e != null) {
                                        Debug.Log("Ther is no user : exception");
                                    }else {
                                        int jinkeiJudgeCount = 0;
                                        for (int i = 0; i < objList.Count; i++) {

                                            int index = i;
                                            string userId = System.Convert.ToString(objList[index]["userId"]);

                                            // userIdに対応するpvpjinkeiが存在するか
                                            NCMBQuery<NCMBObject> jinkeiQuery = new NCMBQuery<NCMBObject>("pvpJinkei");
                                            jinkeiQuery.WhereEqualTo("userId", userId);
                                            jinkeiQuery.CountAsync((int jinkeiCount, NCMBException exception) => {
                                                if (exception == null) {
                                                    // pvpjinkeiが存在するもののみ追加
                                                    if (jinkeiCount > 0) {
                                                        string userName = System.Convert.ToString(objList[index]["userName"]);
                                                        int soudaisyo = System.Convert.ToInt32(objList[index]["soudaisyo"]);
                                                        int kuniLv = System.Convert.ToInt32(objList[index]["kuniLv"]);
                                                        int hp = System.Convert.ToInt32(objList[index]["jinkeiHeiryoku"]);

                                                        if(soudaisyo != 0) {
                                                            pvpUserIdList.Add(userId);
                                                            pvpUserNameList.Add(userName);
                                                            pvpSoudaisyoList.Add(soudaisyo);
                                                            pvpKuniLvList.Add(kuniLv);
                                                            pvpHpList.Add(hp);

                                                            //Enemy Pt & Rank                                                        
                                                            //InsertPvPWeekly(userId, startDateNCMB, endDateNCMB, userName, kuniLv, soudaisyo, hp);
                                                            pvpPtList.Add(1000);
                                                        }
                                                    }
                                                }

                                                jinkeiJudgeCount++;
                                                if (jinkeiJudgeCount == objList.Count) {
                                                    matchedFlg = true;
                                                }
                                            });

                                        }
                                    }
                                });
                            }
                        }
                    });

                /*From PvP Tmp(Weekly)*/
                }else {
                    queryPvPTmp.Skip = rdmSkip;
                    queryPvPTmp.Limit = 3;
                    queryPvPTmp.FindAsync((List<NCMBObject> objList, NCMBException e) => {
                        if (e != null) {
                            Debug.Log("Ther is no user : exception");
                        }else {
                            int jinkeiJudgeCount = 0;
                            for (int i = 0; i < objList.Count; i++) {

                                int index = i;
                                string userId = System.Convert.ToString(objList[index]["userId"]);

                                // userIdに対応するpvpjinkeiが存在するか
                                NCMBQuery<NCMBObject> jinkeiQuery = new NCMBQuery<NCMBObject>("pvpJinkei");
                                jinkeiQuery.WhereEqualTo("userId", userId);
                                jinkeiQuery.CountAsync((int jinkeiCount, NCMBException exception) => {
                                    if (exception == null) {
                                        // pvpjinkeiが存在するもののみ追加
                                        if (jinkeiCount > 0) {
                                            string userName = System.Convert.ToString(objList[index]["userName"]);
                                            int soudaisyo = System.Convert.ToInt32(objList[index]["soudaisyo"]);
                                            int kuniLv = System.Convert.ToInt32(objList[index]["kuniLv"]);
                                            int hp = System.Convert.ToInt32(objList[index]["jinkeiHeiryoku"]);
                                            int pt = System.Convert.ToInt32(objList[index]["totalPt"]);

                                            if (soudaisyo != 0) {
                                                pvpUserIdList.Add(userId);
                                                pvpUserNameList.Add(userName);
                                                pvpSoudaisyoList.Add(soudaisyo);
                                                pvpKuniLvList.Add(kuniLv);
                                                pvpHpList.Add(hp);
                                                pvpPtList.Add(pt);
                                            }
                                        }
                                    }

                                    jinkeiJudgeCount++;
                                    if (jinkeiJudgeCount == objList.Count) {
                                        matchedFlg = true;
                                    }
                                });

                            }
                        }
                    });
                }
            }
        });






        /*
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.WhereNotEqualTo("userId", myUserId);
        query.WhereLessThanOrEqualTo("jinkeiHeiryoku", Mathf.CeilToInt((float)HpBase * 1.5f));
        query.WhereGreaterThanOrEqualTo("jinkeiHeiryoku", Mathf.CeilToInt((float)HpBase/1.5f));
        query.WhereNotEqualTo("atkNo",0);

        int rdmSkip = 0;
        query.CountAsync((int count, NCMBException eCount) => {
            if (eCount == null) {
                matchCount = count;
                rdmSkip = UnityEngine.Random.Range(0, matchCount) - 3;
                if (rdmSkip < 0) rdmSkip = 0;

                if (matchCount == 0) {
                    matchedFlg = true;
                }else {
                    //Random Id
                    query.Skip = rdmSkip;
                    query.Limit = 3;

                    query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
                        if (e != null) {
                            Debug.Log("Ther is no user : exception");
                        }
                        else {
                            int jinkeiJudgeCount = 0;
                            for (int i = 0; i < objList.Count; i++) {

                                if (pvpUserIdList.Count == 3) {
                                    matchedFlg = true;
                                    break;
                                }

                                int index = UnityEngine.Random.Range(0, objList.Count);
                                string userId = System.Convert.ToString(objList[index]["userId"]);

                                // userIdに対応するpvpjinkeiが存在するか
                                NCMBQuery<NCMBObject> jinkeiQuery = new NCMBQuery<NCMBObject>("pvpJinkei");
                                jinkeiQuery.WhereEqualTo("userId", userId);
                                jinkeiQuery.CountAsync((int jinkeiCount, NCMBException exception) => {
                                    if (exception == null) {
                                        // pvpjinkeiが存在するもののみ追加
                                        if (jinkeiCount > 0) {
                                            string userName = System.Convert.ToString(objList[index]["userName"]);
                                            int soudaisyo = System.Convert.ToInt32(objList[index]["soudaisyo"]);
                                            int kuniLv = System.Convert.ToInt32(objList[index]["kuniLv"]);
                                            int hp = System.Convert.ToInt32(objList[index]["jinkeiHeiryoku"]);

                                            pvpUserIdList.Add(userId);
                                            pvpUserNameList.Add(userName);
                                            pvpSoudaisyoList.Add(soudaisyo);
                                            pvpKuniLvList.Add(kuniLv);
                                            pvpHpList.Add(hp);

                                            //Enemy Pt & Rank
                                            NCMBQuery<NCMBObject> queryPvPTmp = new NCMBQuery<NCMBObject>("pvpTmp");
                                            queryPvPTmp.WhereEqualTo("userId", userId);
                                            queryPvPTmp.WhereGreaterThanOrEqualTo("endDate", todayNCMB);
                                            queryPvPTmp.FindAsync((List<NCMBObject> objPvPList, NCMBException ePvP) => {
                                                if (ePvP == null) {
                                                    if (objPvPList.Count == 0) { //never registered
                                                        InsertPvPWeekly(userId, startDateNCMB, endDateNCMB, userName, kuniLv, soudaisyo, hp);
                                                        pvpPtList.Add(1000);
                                                    }
                                                    else { //Get Data
                                                        foreach (NCMBObject objPvP in objPvPList) {
                                                            int pt = System.Convert.ToInt32(objPvP["totalPt"]);
                                                            pvpPtList.Add(pt);
                                                        }
                                                    }
                                                }
                                            });
                                        }
                                    }

                                    jinkeiJudgeCount++;
                                    if (jinkeiJudgeCount == 3) {
                                        matchedFlg = true;
                                    }
                                });

                            }
                        }
                    });
                }
            }
        });
        */
    }

    public void GetEnemyJinkei(string userId, int PvPId) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpJinkei");

        query.WhereEqualTo("userId", userId);
        //userId = PlayerPrefs.GetString("userId"); //test
        query.Limit = 1;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e != null) {
                Debug.Log("Ther is no user : exception");
            }else {
                foreach (NCMBObject ncbObject in objList) {
                    for (int i = 0; i < 25; i++) {
                        int id = i + 1;
                        string mapId = "map" + id.ToString();
                        int busyoId = System.Convert.ToInt32(ncbObject[mapId]);
                        
                        if (PvPId== 1) {
                            PvP1BusyoList.Add(busyoId);
                        }else if(PvPId== 2) {
                            PvP2BusyoList.Add(busyoId);
                        }else if (PvPId == 3) {
                            PvP3BusyoList.Add(busyoId);
                        }
                        
                    }
                    if (PvPId == 1) {
                        ArrayList arraylist1 = (ArrayList)ncbObject["lvList"];
                        foreach (object o in arraylist1) PvP1LvList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist2 = (ArrayList)ncbObject["heiList"];
                        foreach (object o in arraylist2) PvP1HeiList.Add(System.Convert.ToString(o));

                        ArrayList arraylist3 = (ArrayList)ncbObject["senpouLvList"];
                        foreach (object o in arraylist3) PvP1SenpouLvList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist4 = (ArrayList)ncbObject["sakuLvList"];
                        foreach (object o in arraylist4) PvP1SakuLvList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist5 = (ArrayList)ncbObject["kahouList"];
                        foreach (object o in arraylist5) PvP1KahouList.Add(System.Convert.ToString(o));

                        soudaisyo1 = System.Convert.ToInt32(ncbObject["soudaisyo"]);

                    }else if(PvPId == 2) {
                        ArrayList arraylist1 = (ArrayList)ncbObject["lvList"];
                        foreach (object o in arraylist1) PvP2LvList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist2 = (ArrayList)ncbObject["heiList"];
                        foreach (object o in arraylist2) PvP2HeiList.Add(System.Convert.ToString(o));

                        ArrayList arraylist3 = (ArrayList)ncbObject["senpouLvList"];
                        foreach (object o in arraylist3) PvP2SenpouLvList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist4 = (ArrayList)ncbObject["sakuLvList"];
                        foreach (object o in arraylist4) PvP2SakuLvList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist5 = (ArrayList)ncbObject["kahouList"];
                        foreach (object o in arraylist5) PvP2KahouList.Add(System.Convert.ToString(o));

                        soudaisyo2 = System.Convert.ToInt32(ncbObject["soudaisyo"]);

                    }else if (PvPId == 3) {
                        ArrayList arraylist1 = (ArrayList)ncbObject["lvList"];
                        foreach (object o in arraylist1) PvP3LvList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist2 = (ArrayList)ncbObject["heiList"];
                        foreach (object o in arraylist2) PvP3HeiList.Add(System.Convert.ToString(o));

                        ArrayList arraylist3 = (ArrayList)ncbObject["senpouLvList"];
                        foreach (object o in arraylist3) PvP3SenpouLvList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist4 = (ArrayList)ncbObject["sakuLvList"];
                        foreach (object o in arraylist4) PvP3SakuLvList.Add(System.Convert.ToInt32(o));

                        ArrayList arraylist5 = (ArrayList)ncbObject["kahouList"];
                        foreach (object o in arraylist5) PvP3KahouList.Add(System.Convert.ToString(o));

                        soudaisyo3 = System.Convert.ToInt32(ncbObject["soudaisyo"]);
                    }
                }
            }
        });
    }

    public void GetEnemyBusyoStatus(string userId, int PvPId) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpJinkei");
        query.WhereNotEqualTo("userId", userId);

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e != null) {
                Debug.Log("Ther is no user : exception");
            }
            else {
                for (int i = 0; i < 25; i++) {
                    int id = i + 1;
                    string mapId = "map" + id.ToString();
                    int busyoId = System.Convert.ToInt32(objList[i][mapId]);
                    if (PvPId == 1) {
                        PvP1BusyoList.Add(busyoId);
                    }
                    else if (PvPId == 2) {
                        PvP2BusyoList.Add(busyoId);
                    }
                    else if (PvPId == 3) {
                        PvP3BusyoList.Add(busyoId);
                    }
                }
            }
        });
    }
    /* Matching End */


    //PvP 攻撃回数トランザクション登録
    public void UpdatePvPAtkNo(string userId, int todayNCMB) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.WhereEqualTo("userId", userId);
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count != 0) {
                    int atkNo = System.Convert.ToInt32(objList[0]["atkNo"]);
                    atkNo = atkNo + 1;
                    objList[0]["atkNo"] = atkNo;
                    objList[0].SaveAsync();                    
                }
            }
        });

        NCMBQuery<NCMBObject> queryWeekly = new NCMBQuery<NCMBObject>("pvpTmp");
        queryWeekly.WhereEqualTo("userId", userId);
        queryWeekly.WhereGreaterThanOrEqualTo("endDate", todayNCMB);
        queryWeekly.OrderByDescending("endDate");
        queryWeekly.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count != 0) {
                    int atkNo = System.Convert.ToInt32(objList[0]["atkNo"]);
                    atkNo = atkNo + 1;
                    objList[0]["atkNo"] = atkNo;
                    objList[0].SaveAsync();
                    PvPAtkNoFlg = true;
                }
            }
        });
    }


    //PvP 守備回数トランザクション登録
    public void UpdatePvPDfcNo(string userId, int todayNCMB) {

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.WhereEqualTo("userId", userId);

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {            
            if (e == null) {               
                if (objList.Count != 0) {
                    int dfcNo = System.Convert.ToInt32(objList[0]["dfcNo"]);
                    dfcNo = dfcNo + 1;
                    objList[0]["dfcNo"] = dfcNo;

                    objList[0].SaveAsync();                
                }
            }
        });

        NCMBQuery<NCMBObject> queryWeekly = new NCMBQuery<NCMBObject>("pvpTmp");
        queryWeekly.WhereGreaterThanOrEqualTo("endDate", todayNCMB);
        queryWeekly.OrderByDescending("endDate");
        queryWeekly.WhereEqualTo("userId", userId);

        queryWeekly.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {              
                if (objList.Count != 0) {
                    int dfcNo = System.Convert.ToInt32(objList[0]["dfcNo"]);
                    dfcNo = dfcNo + 1;
                    objList[0]["dfcNo"] = dfcNo;
                    objList[0].SaveAsync();                    
                }
            }
        });
    }

    //PvP Atkして勝利した回数の更新 > Total & Weekly
    public void UpdatePvPAtkWinNo(string userId, int todayNCMB) {

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.WhereEqualTo("userId", userId);

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count != 0) {
                    int atkWinNo = System.Convert.ToInt32(objList[0]["atkWinNo"]);
                    atkWinNo = atkWinNo + 1;
                    objList[0]["atkWinNo"] = atkWinNo;

                    int totalWinNo = System.Convert.ToInt32(objList[0]["totalWinNo"]);
                    totalWinNo = totalWinNo + 1;
                    objList[0]["totalWinNo"] = totalWinNo;
                    
                    objList[0].SaveAsync();
                }
            }
        });

        NCMBQuery<NCMBObject> queryWeekly = new NCMBQuery<NCMBObject>("pvpTmp");
        queryWeekly.WhereEqualTo("userId", userId);
        queryWeekly.WhereGreaterThanOrEqualTo("endDate", todayNCMB);
        queryWeekly.OrderByDescending("endDate");

        queryWeekly.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count != 0) {
                    int atkWinNo = System.Convert.ToInt32(objList[0]["atkWinNo"]);
                    atkWinNo = atkWinNo + 1;
                    objList[0]["atkWinNo"] = atkWinNo;

                    int totalWinNo = System.Convert.ToInt32(objList[0]["totalWinNo"]);
                    totalWinNo = totalWinNo + 1;
                    objList[0]["totalWinNo"] = totalWinNo;

                    objList[0].SaveAsync();
                }
            }
        });

    }

    //PvP Dfcして勝利した回数の更新 > Total & Weekly
    public void UpdatePvPDfcWinNo(string userId, int todayNCMB) {

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.WhereEqualTo("userId", userId);

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count != 0) {
                    int dfcWinNo = System.Convert.ToInt32(objList[0]["dfcWinNo"]);
                    dfcWinNo = dfcWinNo + 1;
                    objList[0]["dfcWinNo"] = dfcWinNo;

                    int totalWinNo = System.Convert.ToInt32(objList[0]["totalWinNo"]);
                    totalWinNo = totalWinNo + 1;
                    objList[0]["totalWinNo"] = totalWinNo;

                    objList[0].SaveAsync();
                }
            }
        });

        NCMBQuery<NCMBObject> queryWeekly = new NCMBQuery<NCMBObject>("pvpTmp");
        queryWeekly.WhereEqualTo("userId", userId);
        queryWeekly.WhereGreaterThanOrEqualTo("endDate", todayNCMB);
        queryWeekly.OrderByDescending("endDate");

        queryWeekly.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count != 0) {
                    int dfcWinNo = System.Convert.ToInt32(objList[0]["dfcWinNo"]);
                    dfcWinNo = dfcWinNo + 1;
                    objList[0]["dfcWinNo"] = dfcWinNo;

                    int totalWinNo = System.Convert.ToInt32(objList[0]["totalWinNo"]);
                    totalWinNo = totalWinNo + 1;
                    objList[0]["totalWinNo"] = totalWinNo;

                    objList[0].SaveAsync();
                }
            }
        });
    }

    //Rank Win
    public void GetTop10Win() {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.OrderByDescending("totalWinNo");
        query.Limit = 10;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e != null) {
            }else {
                foreach (NCMBObject obj in objList) {
                    Top3WinNameList.Add(System.Convert.ToString(obj["userName"]));
                    Top3WinUserIdList.Add(System.Convert.ToString(obj["userId"]));
                    Top3WinRankList.Add(System.Convert.ToInt32(obj["kuniLv"]));
                    Top3WinBusyoList.Add(System.Convert.ToInt32(obj["soudaisyo"]));
                    Top3WinQtyList.Add(System.Convert.ToInt32(obj["totalWinNo"]));
                }
            }
        });
    }

    //Rank HP
    public void GetTop10HP() {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.OrderByDescending("jinkeiHeiryoku");
        query.Limit = 10;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e != null) {
            }
            else {
                foreach (NCMBObject obj in objList) {
                    Top3HPUserIdList.Add(System.Convert.ToString(obj["userId"]));
                    Top3HPNameList.Add(System.Convert.ToString(obj["userName"]));
                    Top3HPRankList.Add(System.Convert.ToInt32(obj["kuniLv"]));
                    Top3HPBusyoList.Add(System.Convert.ToInt32(obj["soudaisyo"]));
                    Top3HPQtyList.Add(System.Convert.ToInt32(obj["jinkeiHeiryoku"]));
                }
            }
        });
    }


    /* Weekly Pt Start*/
    public void GetTop10Pt(int todayNCMB) {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpTmp");        
        query.WhereGreaterThanOrEqualTo("endDate", todayNCMB);

        query.OrderByDescending("totalPt");
        query.AddDescendingOrder("totalWinNo");
        query.Limit = 10;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e != null) {
            }else {
                foreach (NCMBObject obj in objList) {

                    //PvP Detail
                    string userId = System.Convert.ToString(obj["userId"]);
                    Top10PtWeeklyWinList.Add(System.Convert.ToInt32(obj["totalWinNo"]));
                    int atkNo = System.Convert.ToInt32(obj["atkNo"]);
                    int dfcNo = System.Convert.ToInt32(obj["dfcNo"]);
                    Top10PtWeeklyBattleList.Add(atkNo + dfcNo);
                    Top10PtWeeklyQtyList.Add(System.Convert.ToInt32(obj["totalPt"]));
                    Top10PtWeeklyUserIdList.Add(userId);
                    Top10PtWeeklyNameList.Add(System.Convert.ToString(obj["userName"]));
                    Top10PtWeeklyRankList.Add(System.Convert.ToInt32(obj["kuniLv"]));
                    Top10PtWeeklyBusyoList.Add(System.Convert.ToInt32(obj["soudaisyo"]));
                    Top10PtWeeklyHeiList.Add(System.Convert.ToInt32(obj["jinkeiHeiryoku"]));                    
                }
            }
        });
    }



    //Playerの前3位のユーザデータ取得
    /*
    public void GetNeighborsPt(int currentRank, int todayNCMB) {

        int numSkip = currentRank - 4;
        if (numSkip < 0) numSkip = 0;

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpTmp");
        query.WhereGreaterThanOrEqualTo("endDate", todayNCMB);

        query.OrderByDescending("totalPt");
        query.Skip = numSkip;
        query.Limit = 5;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e != null) {
                //検索失敗時の処理
            }else {
                foreach (NCMBObject obj in objList) {
                    string userId = System.Convert.ToString(obj["userId"]);

                    //PvP
                    NCMBQuery<NCMBObject> queryPvP = new NCMBQuery<NCMBObject>("pvp");
                    queryPvP.WhereEqualTo("userId", userId);

                    queryPvP.FindAsync((List<NCMBObject> objListPvP, NCMBException ePvP) => {
                        if (ePvP == null) {
                            if (objListPvP.Count != 0) {
                                NeighborsPtWeeklyNameList.Add(System.Convert.ToString(objListPvP[0]["userName"]));
                                NeighborsPtWeeklyRankList.Add(System.Convert.ToInt32(objListPvP[0]["kuniLv"]));
                                NeighborsPtWeeklyBusyoList.Add(System.Convert.ToInt32(objListPvP[0]["soudaisyo"]));
                                NeighborsPtWeeklyQtyList.Add(System.Convert.ToInt32(obj["totalPt"]));
                            }
                        }
                    });
                }
            }
        });
    }
    */
    /* Weekly Pt End*/
    



    //Point Up
    public void UpdatePvPPt(string userId, bool plusFlg, int getPt) { //true + : false - 

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpTmp");
        query.WhereEqualTo("userId", userId);
        query.WhereGreaterThanOrEqualTo("endDate", todayNCMB);
        query.OrderByDescending("endDate");

        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                if (objList.Count != 0) {
                    int totalPt = System.Convert.ToInt32(objList[0]["totalPt"]);
                    if(plusFlg) {
                        totalPt = totalPt + getPt;
                        objList[0]["totalPt"] = totalPt;
                        objList[0].SaveAsync();
                        donePlusUpdatePtFlg = true;
                        Debug.Log("A");
                    }else {
                        totalPt = totalPt - getPt;
                        if(totalPt < 0) {
                            totalPt = 0;
                        }
                        objList[0]["totalPt"] = totalPt;
                        objList[0].SaveAsync();
                        doneMinusUpdatePtFlg = true;
                        Debug.Log("B");

                    }
                }else {
                    if (plusFlg) {
                        donePlusUpdatePtFlg = true;
                    }else {
                        doneMinusUpdatePtFlg = true;                     
                    }
                }
            }
        });        

    }


}
