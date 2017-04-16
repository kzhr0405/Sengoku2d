using UnityEngine;
using System.Collections;

public class SimpleHP : MonoBehaviour {

    public float initLife = 0;
    public float life = 100;
    public int dfc = 10;
    public int baseDfc = 10;
    public int belongDaimyoId = 0;

    public bool myDaimyoBusyoFlg;
    public int numSameDaimyo = 0;

    void Awake() {
        initLife = life;

        if(name != "shiro") {
            BusyoInfoGet busyo = new BusyoInfoGet();
            belongDaimyoId = busyo.getDaimyoId(int.Parse(name));
            if (belongDaimyoId == 0) {
                belongDaimyoId = busyo.getDaimyoHst(int.Parse(name));
            }
        }

    }

    void Update() {
        if (life <= 0) {
            Dead();
        }
    }

    public void Damage(float damage) {
        life -= damage;
        if(life<0) {
            life = 0;
        }
    }

    public void Dead() {
        Destroy(gameObject);
    }


}
