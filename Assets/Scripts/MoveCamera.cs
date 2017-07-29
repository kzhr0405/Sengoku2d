using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public bool leftFlg = false;
    public GameObject cameraObj = null;
    public AutoAttack autoScript = null;
    public bool now = false;
    public float speed = 40.0f;
    public int maxRightX = 50;
    public int maxLeftX = -40;

    void Start () {
		if(name=="Left") {
            leftFlg = true;
        }

        cameraObj = GameObject.Find("Main Camera").gameObject;
        if(GameObject.Find("AutoBtn")) {
            autoScript = GameObject.Find("AutoBtn").GetComponent<AutoAttack>();
        }
    }
	
    void Update() {
        if (now == true) {
            
            if(leftFlg) {
                Vector3 Qpos = cameraObj.transform.localPosition;
                Qpos.x -= (speed * Time.deltaTime);
                if (Qpos.x < maxLeftX) {
                    Qpos.x = maxLeftX;
                }
                cameraObj.transform.localPosition = Qpos;
            }else {
                Vector3 Qpos = cameraObj.transform.localPosition;
                Qpos.x += (speed * Time.deltaTime);
                if (Qpos.x > maxRightX) {
                    Qpos.x = maxRightX;
                }
                cameraObj.transform.localPosition = Qpos;
            }
        }
    }

    public void OnRD() {
        now = true;
        cameraObj.GetComponent<CameraMove>().nowCameraMove = true;
    }

    public void OnRU() {
        now = false;
        cameraObj.GetComponent<CameraMove>().nowCameraMove = false;
    }
}
