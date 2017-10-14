using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Busyo : MonoBehaviour {

    public int busyoId;
    public string busyoName;
    public string rank;
    public string heisyu;
    public int daimyoId;
    public int daimyoHst;
    public int lv;

    public static void InitDic() {
        //Dictionary初期化
        Dictionary<int, Busyo> dic = new Dictionary<int, Busyo>();        
    }

    public Busyo(int BusyoId, string BusyoName, string Rank, string Heisyu, int DaimyoId, int DaimyoHst, int Lv) {
        busyoId = BusyoId;
        busyoName = BusyoName;
        rank = Rank;
        heisyu = Heisyu;
        daimyoId = DaimyoId;
        daimyoHst = DaimyoHst;
        lv = Lv;
    }
}
