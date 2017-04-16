using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KumoMove : MonoBehaviour {

	bool leftKumoFlg = false;
	private Vector3 pos;
	bool backFlg = false;	//come and back
	public float speed = 1.0f;
	public bool runFlg = false;
	float timer = 0;
	int waitingTime = 1;
	public GameObject dailyGacyaBtn;
	public GameObject busyoDamaGacyaBtn;

    public bool touyouSceneFlg = false;
    public bool tutorialDoneFlg = false;

	void Awake(){
		Application.targetFrameRate = 60;
	}

	void Start () {
		pos = transform.localPosition;
		if (name == "KumoLeft") {
			leftKumoFlg = true;
		}

        if(SceneManager.GetActiveScene().name == "touyou") {
            touyouSceneFlg = true;
            dailyGacyaBtn = GameObject.Find ("DailyGacyaButton").gameObject;
		    busyoDamaGacyaBtn = GameObject.Find ("BusyoDamaGacyaButton").gameObject;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (runFlg) {
			if (leftKumoFlg) {
				if (!backFlg) {
					transform.localPosition = pos;
					pos.x += speed;
					if (pos.x >= -310) {
						backFlg = true;
					}
				} else {
					timer += Time.deltaTime;
					if (timer > waitingTime) {
						transform.localPosition = pos;
						pos.x -= speed;
						if (pos.x <= -1700) {
							runFlg = false;
							backFlg = false;
							timer = 0;
                            if (touyouSceneFlg) {
                                dailyGacyaBtn.GetComponent<Button> ().enabled = true;
							    busyoDamaGacyaBtn.GetComponent<Button> ().enabled = true;
                            }

                            if (tutorialDoneFlg) {
                                Application.LoadLevel("mainStage");
                            }


						}
					}
				}
			} else {
				if (!backFlg) {
					transform.localPosition = pos;
					pos.x -= speed;
					if (pos.x <= 315) {
						backFlg = true;
					}
				} else {
					timer += Time.deltaTime;
					if (timer > waitingTime) {
						transform.localPosition = pos;
						pos.x += speed;
						if (pos.x >= 1700) {
							runFlg = false;
							backFlg = false;
							timer = 0;
						}
					}
				}
			}
		}
	}
}
