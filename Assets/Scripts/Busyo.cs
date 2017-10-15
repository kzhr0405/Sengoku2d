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
<<<<<<< HEAD
    public float hp;
    public float atk;
    public float dfc;
    public float spd;
=======
>>>>>>> 413620e13ab3157948fd4a086ffb3b7e50ac0478

    public static void InitDic() {
        //Dictionary初期化
        Dictionary<int, Busyo> dic = new Dictionary<int, Busyo>();        
    }

<<<<<<< HEAD
    public Busyo(int BusyoId, string BusyoName, string Rank, string Heisyu, int DaimyoId, int DaimyoHst, int Lv, float Hp, float Atk, float Dfc, float Spd) {
=======
    public Busyo(int BusyoId, string BusyoName, string Rank, string Heisyu, int DaimyoId, int DaimyoHst, int Lv) {
>>>>>>> 413620e13ab3157948fd4a086ffb3b7e50ac0478
        busyoId = BusyoId;
        busyoName = BusyoName;
        rank = Rank;
        heisyu = Heisyu;
        daimyoId = DaimyoId;
        daimyoHst = DaimyoHst;
        lv = Lv;
<<<<<<< HEAD
        hp = Hp;
        atk = Atk;
        dfc = Dfc;
        spd = Spd;

=======
>>>>>>> 413620e13ab3157948fd4a086ffb3b7e50ac0478
    }
}
