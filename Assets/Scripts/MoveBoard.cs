using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoard : MonoBehaviour {

    public bool runFlg;
    public bool leftFlg;
    float timer = 0;
    private Vector3 pos;
    public float speed = 5.0f;

    void Start () {
        pos = transform.localPosition;
        speed = 8.0f;
        if (name == "Left") {
            leftFlg = true;
        }
    }

    void Update() {
        if (runFlg) {
            if (leftFlg) {
                timer += Time.deltaTime;
                transform.localPosition = pos;
                pos.x -= speed;
                if (pos.x <= -1000) {
                    runFlg = false;
                    timer = 0;
                    GameObject.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "Default";
                    Destroy(transform.parent.gameObject);
                }                
            }else {
                timer += Time.deltaTime;
                transform.localPosition = pos;
                pos.x += speed;
                if (pos.x > 1000) {
                    runFlg = false;
                    timer = 0;
                }
            }
        }
    }
}
