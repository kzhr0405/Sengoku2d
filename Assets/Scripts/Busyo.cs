using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Busyo : MonoBehaviour {

    public int busyoId;
    public string busyoName;
    public string rank;
    public int rankId;
    public string heisyu;
    public int daimyoId;
    public int daimyoHst;
    public int lv;
    public float hp;
    public float atk;
    public float dfc;
    public float spd;
    public int senpouId;
    public int sakuId;

    public static void InitDic() {
        //Dictionary初期化
        Dictionary<int, Busyo> dic = new Dictionary<int, Busyo>();        
    }

    public Busyo(int BusyoId, string BusyoName, string Rank, int RankId, string Heisyu, int DaimyoId, int DaimyoHst, int Lv, float Hp, float Atk, float Dfc, float Spd, int SenpouId, int SakuId) {
        busyoId = BusyoId;
        busyoName = BusyoName;
        rank = Rank;
        rankId = RankId;
        heisyu = Heisyu;
        daimyoId = DaimyoId;
        daimyoHst = DaimyoHst;
        lv = Lv;
        hp = Hp;
        atk = Atk;
        dfc = Dfc;
        spd = Spd;
        senpouId = SenpouId;
        sakuId = SakuId;
    }
}
