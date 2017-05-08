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
    public int winRankWeekly = -1;

    //pvp random match
    public List<string> pvpUserIdList = new List<string>();
    public List<string> pvpUserNameList = new List<string>();
    public List<int> pvpSoudaisyoList = new List<int>();
    public List<int> pvpKuniLvList = new List<int>();
    public List<int> pvpHpList = new List<int>();
    public List<int> pvpWinList = new List<int>();
    public List<int> pvpWinRankList = new List<int>();
    public bool matchedFlg = false;
    public int matchCount = 0;
    public bool zeroFlg = false;

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

    //Top3
    public List<string> Top3WinNameList = new List<string>();
    public List<int> Top3WinRankList = new List<int>();
    public List<int> Top3WinBusyoList = new List<int>();
    public List<int> Top3WinQtyList = new List<int>();

    public List<string> Top3HPNameList = new List<string>();
    public List<int> Top3HPRankList = new List<int>();
    public List<int> Top3HPBusyoList = new List<int>();
    public List<int> Top3HPQtyList = new List<int>();

    //Neibors, upper 2 & lower 2
    public List<string> NeighborsWinNameList = new List<string>();
    public List<int> NeighborsWinRankList = new List<int>();
    public List<int> NeighborsWinBusyoList = new List<int>();
    public List<int> NeighborsWinQtyList = new List<int>();

    public List<string> NeighborsHPNameList = new List<string>();
    public List<int> NeighborsHPRankList = new List<int>();
    public List<int> NeighborsHPBusyoList = new List<int>();
    public List<int> NeighborsHPQtyList = new List<int>();


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
    public void GetPvPCountWeekly() {
        //PvPCount
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpWeekly");
        query.CountAsync((int count, NCMBException e) => {
            if (e != null) {
                //件数取得失敗
                Debug.Log(count);
            }else {
                //件数取得成功
                pvpCountWeekly = count;
            }
        });
    }
    
    public void GetMyPvPWeekly() {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvpWeekly");
        userId = PlayerPrefs.GetString("userId");
        query.WhereEqualTo("userId", userId);
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (objList.Count == 0) { //never registered
                InsertPvPWeekly(userId);
            }else { //registered
                if (e == null) {
                    foreach (NCMBObject obj in objList) {
                        atkNoWeekly = System.Convert.ToInt32(obj["atkNo"]);
                        atkWinNoWeekly = System.Convert.ToInt32(obj["atkWinNo"]);
                        dfcNoWeekly = System.Convert.ToInt32(obj["dfcNo"]);
                        dfcWinNoWeekly = System.Convert.ToInt32(obj["dfcWinNo"]);
                    }
                }
            }
        });
    }

    public void InsertPvPWeekly(string userId) {
        NCMBObject pvpWeekly = new NCMBObject("pvpWeekly");
        pvpWeekly["userId"] = userId;
        pvpWeekly["atkNo"] = 0;
        pvpWeekly["dfcNo"] = 0;
        pvpWeekly["atkWinNo"] = 0;
        pvpWeekly["dfcWinNo"] = 0;
        pvpWeekly["totalWinNo"] = 0;

        pvpWeekly.SaveAsync();
    }

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

    /* Weekly End */







    /* Matching Start */
    public void GetRandomEnemy(string myUserId, int HpBase) {

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.WhereNotEqualTo("userId", myUserId);
        query.WhereLessThanOrEqualTo("jinkeiHeiryoku", HpBase);
        query.OrderByDescending("jinkeiHeiryoku");
        query.Limit = 3;
        
        query.CountAsync((int count, NCMBException e) => {
            if (e == null) {
                matchCount = count;
                if(matchCount == 0) {
                    zeroFlg = true;
                }
            }
        });

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
                    jinkeiQuery.CountAsync((int count, NCMBException exception) => {
                        if (exception == null) {
                            // pvpjinkeiが存在するもののみ追加
                            if (count > 0)
                            {
                                string userName = System.Convert.ToString(objList[index]["userName"]);
                                int soudaisyo = System.Convert.ToInt32(objList[index]["soudaisyo"]);
                                int kuniLv = System.Convert.ToInt32(objList[index]["kuniLv"]);
                                int hp = System.Convert.ToInt32(objList[index]["jinkeiHeiryoku"]);
                                int win = System.Convert.ToInt32(objList[index]["totalWinNo"]);

                                pvpUserIdList.Add(userId);
                                pvpUserNameList.Add(userName);
                                pvpSoudaisyoList.Add(soudaisyo);
                                pvpKuniLvList.Add(kuniLv);
                                pvpHpList.Add(hp);
                                pvpWinList.Add(win);
                            }
                            else
                            {
                                matchCount--;
                                if (matchCount == 0) zeroFlg = true;
                            }
                        }

                        jinkeiJudgeCount++;
                        if (jinkeiJudgeCount == objList.Count)
                        {
                            matchedFlg = true;
                        }
                    });
                }
            }
        });
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
    public void UpdatePvPAtkNo(string userId){
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

        NCMBQuery<NCMBObject> queryWeekly = new NCMBQuery<NCMBObject>("pvpWeekly");
        queryWeekly.WhereEqualTo("userId", userId);
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
    public void UpdatePvPDfcNo(string userId) {

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

        NCMBQuery<NCMBObject> queryWeekly = new NCMBQuery<NCMBObject>("pvpWeekly");
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
    public void UpdatePvPAtkWinNo(string userId) {

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

        NCMBQuery<NCMBObject> queryWeekly = new NCMBQuery<NCMBObject>("pvpWeekly");
        queryWeekly.WhereEqualTo("userId", userId);

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
    public void UpdatePvPDfcWinNo(string userId) {

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

        NCMBQuery<NCMBObject> queryWeekly = new NCMBQuery<NCMBObject>("pvpWeekly");
        queryWeekly.WhereEqualTo("userId", userId);

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
                    Top3HPNameList.Add(System.Convert.ToString(obj["userName"]));
                    Top3HPRankList.Add(System.Convert.ToInt32(obj["kuniLv"]));
                    Top3HPBusyoList.Add(System.Convert.ToInt32(obj["soudaisyo"]));
                    Top3HPQtyList.Add(System.Convert.ToInt32(obj["jinkeiHeiryoku"]));
                }
            }
        });
    }

    //Playerの前後2位のユーザデータ取得
    public void GetNeighborsWin(int currentRank) {
        
        int numSkip = currentRank - 3;
        if (numSkip < 0) numSkip = 0;
        
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.OrderByDescending("totalWinNo");
        query.Skip = numSkip;
        query.Limit = 5;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e != null) {
                //検索失敗時の処理
            }else {
                // 取得したレコードを保存
                foreach (NCMBObject obj in objList) {
                    Debug.Log(System.Convert.ToString(obj["userName"]));
                    NeighborsWinNameList.Add(System.Convert.ToString(obj["userName"]));
                    NeighborsWinRankList.Add(System.Convert.ToInt32(obj["kuniLv"]));
                    NeighborsWinBusyoList.Add(System.Convert.ToInt32(obj["soudaisyo"]));
                    NeighborsWinQtyList.Add(System.Convert.ToInt32(obj["totalWinNo"]));
                }                
            }
        });
    }

    //Playerの前後2位のユーザデータ取得
    public void GetNeighborsHP(int currentRank) {

        int numSkip = currentRank - 3;
        if (numSkip < 0) numSkip = 0;

        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("pvp");
        query.OrderByDescending("jinkeiHeiryoku");
        query.Skip = numSkip;
        query.Limit = 5;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e != null) {
                //検索失敗時の処理
            }else {
                // 取得したレコードを保存
                foreach (NCMBObject obj in objList) {
                    NeighborsHPNameList.Add(System.Convert.ToString(obj["userName"]));
                    NeighborsHPRankList.Add(System.Convert.ToInt32(obj["kuniLv"]));
                    NeighborsHPBusyoList.Add(System.Convert.ToInt32(obj["soudaisyo"]));
                    NeighborsHPQtyList.Add(System.Convert.ToInt32(obj["jinkeiHeiryoku"]));
                }
            }
        });
    }




}
