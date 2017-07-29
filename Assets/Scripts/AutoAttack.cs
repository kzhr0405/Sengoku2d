using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoAttack : MonoBehaviour {

    public bool onFlg = false;
    public GameObject cameraObjLeft;
    public GameObject cameraObjRight;
    public float speed = 4;

    void Start() {
        cameraObjLeft = GameObject.Find("Left").gameObject;
        cameraObjRight = GameObject.Find("Right").gameObject;

    }


    public void OnClick() {
        
        if(!onFlg) {

            onFlg = true;
            Time.timeScale = speed;

            //Disabled button
            if (GameObject.Find("AutoBtn")) {
                //GetComponent<Button>().enabled = false;
                Color NGClorBtn = new Color(130 / 255f, 130 / 255f, 130 / 255f, 255f / 255f);
                Color NGClorTxt = new Color(180 / 255f, 180 / 255f, 180 / 255f, 150f / 255f);
                GetComponent<Image>().color = NGClorBtn;
                transform.FindChild("Text").GetComponent<Text>().color = NGClorTxt;
            }
            changeAutoScript();
            CameraSpeedDown();

        }else {
            onFlg = false;
            Time.timeScale = 1;
            //Disabled button
            if (GameObject.Find("AutoBtn")) {
                Color ManualClorBtn = new Color(255 / 255f, 255/ 255f, 255 / 255f, 100 / 255f);
                Color ManualClorTxt = new Color(50 / 255f, 50 / 255f, 50 / 255f, 100 / 255f);
                GetComponent<Image>().color = ManualClorBtn;
                transform.FindChild("Text").GetComponent<Text>().color = ManualClorTxt;
            }
            changeManualScript();
            CameraSpeedUp();

        }  
    }

    
    public void changeAutoScript() {
        //search
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Player")) {
            if (obs.GetComponent<UnitMover>()) {
                
                float spd = obs.GetComponent<UnitMover>().speed;
                string heisyu = obs.GetComponent<UnitMover>().heisyu;
                bool leftFlg = obs.GetComponent<UnitMover>().leftFlg;

                if (heisyu == "YR" || heisyu == "KB" || heisyu == "SHP") {
                    Destroy(obs.GetComponent<UnitMover>());
                    obs.AddComponent<Homing>();
                    obs.GetComponent<Homing>().speed = spd;
                    obs.GetComponent<Homing>().leftFlg = leftFlg;
                    //switchBarDirect(obs, leftFlg);
                }
                else if (heisyu == "TP" || heisyu == "YM") {
                    Destroy(obs.GetComponent<UnitMover>());
                    obs.AddComponent<HomingLong>();
                    obs.GetComponent<HomingLong>().speed = spd;
                    obs.GetComponent<HomingLong>().leftFlg = leftFlg;
                    //switchBarDirect(obs, leftFlg);
                }

                
            }
        }
    }

    public void changeManualScript() {
        //search
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Player")) {
            
            if (obs.GetComponent<Homing>()) {
                
                float spd = obs.GetComponent<Homing>().speed;
                bool leftFlg = obs.GetComponent<Homing>().leftFlg;
                Destroy(obs.GetComponent<Homing>());
                obs.AddComponent<UnitMover>();
                obs.GetComponent<UnitMover>().speed = spd;
                obs.GetComponent<UnitMover>().heisyu = obs.GetComponent<Heisyu>().heisyu;
                obs.GetComponent<UnitMover>().leftFlg = leftFlg;

                //switchBarDirect(obs);
            }
            else if(obs.GetComponent<HomingLong>()) {
                
                float spd = obs.GetComponent<HomingLong>().speed;
                bool leftFlg = obs.GetComponent<HomingLong>().leftFlg;
                Destroy(obs.GetComponent<HomingLong>());
                obs.AddComponent<UnitMover>();
                obs.GetComponent<UnitMover>().speed = spd;
                obs.GetComponent<UnitMover>().heisyu = obs.GetComponent<Heisyu>().heisyu;
                obs.GetComponent<UnitMover>().leftFlg = leftFlg;

                //switchBarDirect(obs);
            }
        }
    }


    public void switchBarDirect(GameObject obj, bool leftFlg) {
        
        if(leftFlg) {
            GameObject BusyoDtlPlayer = obj.transform.FindChild("BusyoDtlPlayer").gameObject;
            Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;

            if ((obj.transform.localScale.x<0 && targetChldScale.x>0) ||(obj.transform.localScale.x > 0 && targetChldScale.x < 0) ) {
                targetChldScale.x *= -1;
                BusyoDtlPlayer.transform.localScale = targetChldScale;
            }
        }

    }

    public void CameraSpeedDown() {
        cameraObjLeft.GetComponent<MoveCamera>().speed = cameraObjLeft.GetComponent<MoveCamera>().speed / speed;
        cameraObjRight.GetComponent<MoveCamera>().speed = cameraObjRight.GetComponent<MoveCamera>().speed / speed;

    }

    public void CameraSpeedUp() {
        cameraObjLeft.GetComponent<MoveCamera>().speed = cameraObjLeft.GetComponent<MoveCamera>().speed * speed;
        cameraObjRight.GetComponent<MoveCamera>().speed = cameraObjRight.GetComponent<MoveCamera>().speed * speed;

    }
}
