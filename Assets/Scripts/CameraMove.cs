using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	public bool scrollStartFlg = false; // スクロールが始まったかのフラグ
	public Vector2 scrollStartPos = new Vector2(); // スクロールの起点となるタッチポジション
	public static float SCROLL_END_LEFT = -36f; // 左側への移動制限(これ以上左には進まない)
	public static float SCROLL_END_RIGHT = 46f; // 右側への移動制限(これ以上右には進まない)
	public static float SCROLL_DISTANCE_CORRECTION = 0.8f; // スクロール距離の調整
	public Collider2D collide2dObj =   null; // タッチ位置にあるオブジェクトの初期化
	public Vector3 touchPosition = new Vector3(); // タッチポジション初期化

	//touch Object
	public Vector3 touchStartPosition = new Vector3();
	public Vector3 touchEndPosition = new Vector3();
	public Collider2D touchCollide2dObj =   null;
	public bool objectStartFlg = false;
	public string arrowPath = "";
	public string tapEndPointPath = "";
	public GameObject canvas;

    public bool nowCameraMove = false;

    void Start () {
		arrowPath = "Prefabs/PostKassen/Arrow";
		tapEndPointPath = "Prefabs/PostKassen/TapEndPoint";
		canvas = GameObject.Find ("Canvas").gameObject;

    }

	void Update () {
        if(!nowCameraMove) {
		    if (Input.GetMouseButton (0)) {

			    touchPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			    collide2dObj = Physics2D.OverlapPoint (touchPosition);

			    //Player Move
			    if (collide2dObj && !scrollStartFlg) {
				    if (collide2dObj.tag == "Player") {
					    scrollStartFlg = true;
					    objectStartFlg = true;
					    touchCollide2dObj = collide2dObj;

					    //Release point
					    touchStartPosition = touchPosition;

                        //Blinker
                        if (touchCollide2dObj.name != "hukuhei" && touchCollide2dObj.name != "kengou" && touchCollide2dObj.GetComponent<Heisyu>().heisyu != "saku") {
                            if (!touchCollide2dObj.GetComponent<Blinker>()) {
                                touchCollide2dObj.gameObject.AddComponent<Blinker>();
                                Time.timeScale = 0;
                            }
                        }                        
                    }
			    }

			    //Camera Move
                /*
			    if (!objectStartFlg) {
				    if (!scrollStartFlg || !collide2dObj) {

					    // タッチした場所に何もない場合、スクロールフラグをtrueに
					    scrollStartFlg = true;
					    if (scrollStartPos.x == 0.0f) {
						    // スクロール開始位置を取得
						    scrollStartPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					    } else {
						    Vector3 touchMovePos = touchPosition;
						    if (scrollStartPos.x != touchMovePos.x) {
							    // 直前のタッチ位置との差を取得する
							    float diffPos = SCROLL_DISTANCE_CORRECTION * (touchMovePos.x - scrollStartPos.x);

							    Vector3 pos = this.transform.position;
							    pos.x -= diffPos;
							    // スクロールが制限を超過する場合、処理を止める
							    if (pos.x > SCROLL_END_RIGHT || pos.x < SCROLL_END_LEFT) {
								    return;
							    }
							    this.transform.position = pos;
							    scrollStartPos = touchMovePos;
						    }
					    }
				    }
			    }
                */

		    }else if(Input.GetMouseButtonUp(0)){
			    if (objectStartFlg) {
				    touchEndPosition = touchPosition;

				    //Pass release position & order to move
				    if (touchCollide2dObj != null) {
					    if (touchCollide2dObj.name != "hukuhei" && touchCollide2dObj.name != "kengou" && touchCollide2dObj.GetComponent<Heisyu>().heisyu != "saku") {

                            if(touchCollide2dObj.GetComponent<UnitMover>()) {
                                touchCollide2dObj.GetComponent<UnitMover> ().touchEndPosition = touchEndPosition;
	    					    touchCollide2dObj.GetComponent<UnitMover> ().touchFlg = true;
                            }

                            //Touch Release Point
                            GameObject tapEnd = Instantiate(Resources.Load(tapEndPointPath)) as GameObject;
                            tapEnd.transform.position = new Vector2(touchEndPosition.x, touchEndPosition.y);


                            //Yajirushi
                            GameObject arrow = Instantiate (Resources.Load (arrowPath)) as GameObject;
                            arrow.transform.localPosition = touchCollide2dObj.transform.localPosition;
                            arrow.GetComponent<Follow>().objTarget = touchCollide2dObj.gameObject;

                            Vector3 posDif = touchEndPosition - arrow.transform.position;
                            float angle = Mathf.Atan2 (posDif.y, posDif.x) * Mathf.Rad2Deg;            
                            Vector3 euler = new Vector3(0, 0, angle);
                            arrow.transform.rotation = Quaternion.Euler(euler);

                            
						    //Move SE
						    AudioSource[] seAudioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
						    seAudioSources [0].Play ();

						    //Move SE
						    AudioSource[] audioSources =touchCollide2dObj.GetComponents<AudioSource>();
						    audioSources [0].Play ();

                            if(GameObject.Find("AutoBtn")) {
                                if(GameObject.Find("AutoBtn").GetComponent<AutoAttack>().onFlg) {
                                    Time.timeScale = GameObject.Find("AutoBtn").GetComponent<AutoAttack>().speed;
                                }else {
                                    Time.timeScale = 1;
                                }
                            }else {
                                Time.timeScale = 1;
                            }
                        }
				    }
			    }
		    }else{
			    // タッチを離したらフラグを落とし、スクロール開始位置も初期化する 
			    scrollStartFlg = false;
			    objectStartFlg = false;
			    scrollStartPos = new Vector3();

                if (GameObject.Find("AutoBtn")) {
                    if (GameObject.Find("AutoBtn").GetComponent<AutoAttack>().onFlg) {
                        Time.timeScale = GameObject.Find("AutoBtn").GetComponent<AutoAttack>().speed;
                    }else {
                        Time.timeScale = 1;
                    }
                }else {
                    Time.timeScale = 1;
                }
            }

        }


	}
}