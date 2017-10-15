using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySaku : MonoBehaviour {

    public int id;
    public string typ;
    public int runPlace;
    public int status;
    public int sakuBusyoId;
    public string sakuHeisyu;
    public float sakuHeiSts;
    public float sakuBusyoSpeed;

    public static void InitDic() {
        Dictionary<int, EnemySaku> dic = new Dictionary<int, EnemySaku>();
    }

    public EnemySaku(int Id, string Typ, int RunPlace, int Status, int SakuBusyoId, string SakuHeisyu, float SakuHeiSts, float SakuBusyoSpeed) {
        id = Id;
        typ = Typ;
        runPlace = RunPlace;
        status = Status;
        sakuBusyoId = SakuBusyoId;
        sakuHeisyu = SakuHeisyu;
        sakuHeiSts = SakuHeiSts;
        sakuBusyoSpeed = SakuBusyoSpeed;
    }
}
