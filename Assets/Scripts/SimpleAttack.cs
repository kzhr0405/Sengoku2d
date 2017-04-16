using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleAttack : MonoBehaviour {
    
    //Attack Power
    public float atk = 20;
    private float timeleft;
    public bool lowerFlg = false;


    void OnCollisionStay2D(Collision2D col) {
        if (tag=="Player") {
            if (col.gameObject.tag == "Enemy") {
                timeleft -= Time.deltaTime;
                if (timeleft <= 0.0) {
                    timeleft = 0.1f;
                    int Damage = (int)atk;
                   
                    col.gameObject.GetComponent<SimpleHP>().Damage(Damage);
                    
                }
            }
        }else {
            //Enemy
            if (col.gameObject.tag == "Player") {
                timeleft -= Time.deltaTime;
                if (timeleft <= 0.0) {
                    timeleft = 0.1f;
                    int Damage = (int)atk;

                    col.gameObject.GetComponent<SimpleHP>().Damage(Damage);

                }
            }
        }
    }

    void Update() {
        if(lowerFlg) {
            lowerFlg = false;

            int doNotDo = UnityEngine.Random.Range(0, 3);
            if(doNotDo!=1) {
                List<float> powerUpList = new List<float>() {1.2f,1.5f,2.0f,2.5f,3.0f};
                int rdmId = UnityEngine.Random.Range(0, powerUpList.Count);
                atk = atk * powerUpList[rdmId];

                //anim
                string path = "Prefabs/SimpleBattle/atk_up";
                GameObject atk_up = Instantiate(Resources.Load(path)) as GameObject;
                atk_up.transform.SetParent(gameObject.transform);
                atk_up.transform.localScale = new Vector2(3, 3);
                atk_up.transform.localPosition = new Vector2(0, 1.5f);
            }else{
                if(tag=="Enemy") {
                    List<float> powerDownList = new List<float>() { 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };
                    int rdmId = UnityEngine.Random.Range(0, powerDownList.Count);
                    atk = atk * powerDownList[rdmId];
                }
            }
        }
    }

}
