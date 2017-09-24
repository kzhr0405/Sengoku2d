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

    public static void InitDic() {
        //Dictionary初期化
        Dictionary<int, Busyo> dic = new Dictionary<int, Busyo>();        
    }
}
