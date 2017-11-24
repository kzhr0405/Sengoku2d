using UnityEngine;
using System.Collections;

public class HomingLong : MonoBehaviour {

	public int AIType = 1;	//1.Zone, 2:Chase, 3:Wait
	public bool backShiroFlg = false;
	public GameObject backShiroObj;
	public bool helpTaisyoShiroFlg = false;

	public GameObject nearObj;         //最も近いオブジェクト
	public float speed = 2;			//移動速度
	public string targetTag;
	Animator anim;
	public float Dis = 0;
	public bool leftFlg = false; //左を向いているか
	public float DisTarget = 0;
	private float timeleft;
	public float coolTime = 0;
	public bool fireFlg = false;
	public float fireCoolTime = 0;

	void Start(){
		
		if (tag == "Player") {
			targetTag = "Enemy";
		} else {
			targetTag = "Player";
		}
		
		anim = this.GetComponent ("Animator") as Animator;

		nearObj = serchTagOnLine (gameObject, targetTag);

		if (AIType != 3) {
			Move (nearObj);
		}

		//Difference TP or YM
		string heisyu = this.GetComponent<Heisyu>().heisyu;
		if(heisyu=="TP"){
			DisTarget=30;
			coolTime = 5;

		}else if(heisyu=="YM"){
			DisTarget=20;
			coolTime = 3;
		}
		fireCoolTime = coolTime;

		GetComponent<AttackLong>().coolTime = coolTime;
		if (GetComponent<SenpouController> ()) {
			GetComponent<SenpouController> ().initCoolTime = coolTime;
			GetComponent<SenpouController> ().initDisTarget = DisTarget;
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (fireFlg) {
			fireCoolTime -= Time.deltaTime;
			if (fireCoolTime <= 0) {
				fireCoolTime = coolTime;
				fireFlg = false;
			}
		}



		if (!backShiroFlg) {
			if (!helpTaisyoShiroFlg) {
				nearObj = serchTagOnLine (gameObject, targetTag);
			}else {
                if (nearObj == null) {
                    nearObj = serchTagOnLine(gameObject, targetTag);
                }else {
                    //Dis update
                    Dis = Vector3.Distance(gameObject.transform.position, nearObj.transform.position);
                }
            }
            if (Dis > DisTarget) {
				//Move

				if (AIType != 3) { //Don't move and Waiting at shiro
					
					anim.SetBool ("IsAttack", false); 
					GetComponent<AttackLong> ().enabled = false;

					if (nearObj != null) {
						Move (nearObj);

					
					} else {
						GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
						GetComponent<Rigidbody2D> ().angularVelocity = 0;

					}
				}
			} else {

				if (!fireFlg) {
					fireFlg = true;

					//Attack
					GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
					GetComponent<Rigidbody2D> ().angularVelocity = 0;

					if (nearObj == null) {
						anim.SetBool ("IsAttack", false);
						GetComponent<AttackLong> ().enabled = false;

					} else {

						if (!GetComponent<AttackLong> ().enabled) {
							anim.SetBool ("IsAttack", true);
							GetComponent<AttackLong> ().enabled = true;

							//Change Direction
							if (nearObj.transform.position.x < transform.position.x) {
								if (!leftFlg) {
									Vector2 targetScale = transform.localScale;
									targetScale.x *= -1;
									transform.localScale = targetScale;
									leftFlg = true;

                                    //Name Bar
                                    if (transform.FindChild("BusyoDtlEnemy")) {
                                        GameObject BusyoDtlPlayer = transform.FindChild("BusyoDtlEnemy").gameObject;
                                        Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                                        if ((targetScale.x > 0 && targetChldScale.x < 0) || (targetScale.x < 0 && targetChldScale.x > 0)) {
                                            targetChldScale.x *= -1;
                                            BusyoDtlPlayer.transform.localScale = targetChldScale;
                                        }
                                    }else if(transform.FindChild("BusyoDtlPlayer")) {
                                        GameObject BusyoDtlPlayer = transform.FindChild("BusyoDtlPlayer").gameObject;
                                        Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                                        if ((targetScale.x > 0 && targetChldScale.x < 0) || (targetScale.x < 0 && targetChldScale.x > 0)) {
                                            targetChldScale.x *= -1;
                                            BusyoDtlPlayer.transform.localScale = targetChldScale;
                                        }
                                    }
                                }

							} else {
								if (leftFlg) {
									Vector2 targetScale = transform.localScale;
									targetScale.x *= -1;
									transform.localScale = targetScale;
									leftFlg = false;

                                    if (transform.FindChild("BusyoDtlEnemy")) {
                                        GameObject BusyoDtlPlayer = transform.FindChild("BusyoDtlEnemy").gameObject;
                                        Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                                        if ((targetScale.x > 0 && targetChldScale.x < 0) || (targetScale.x < 0 && targetChldScale.x > 0)) {
                                            targetChldScale.x *= -1;
                                            BusyoDtlPlayer.transform.localScale = targetChldScale;
                                        }
                                    }
                                    else if (transform.FindChild("BusyoDtlPlayer")) {
                                        GameObject BusyoDtlPlayer = transform.FindChild("BusyoDtlPlayer").gameObject;
                                        Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                                        if ((targetScale.x > 0 && targetChldScale.x < 0) || (targetScale.x < 0 && targetChldScale.x > 0)) {
                                            targetChldScale.x *= -1;
                                            BusyoDtlPlayer.transform.localScale = targetChldScale;
                                        }
                                    }
                                }
							}


						}
					}
				} else {
					if (nearObj != null) {
						if (nearObj.transform.position.x < transform.position.x) {
							if (!leftFlg) {
								Vector2 targetScale = transform.localScale;
								targetScale.x *= -1;
								transform.localScale = targetScale;
								leftFlg = true;

                                //Name Bar
                                if (transform.FindChild("BusyoDtlEnemy")) {
                                    GameObject BusyoDtlPlayer = transform.FindChild("BusyoDtlEnemy").gameObject;
                                    Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                                    if ((targetScale.x > 0 && targetChldScale.x < 0) || (targetScale.x < 0 && targetChldScale.x > 0)) {
                                        targetChldScale.x *= -1;
                                        BusyoDtlPlayer.transform.localScale = targetChldScale;
                                    }
                                }
                                else if (transform.FindChild("BusyoDtlPlayer")) {
                                    GameObject BusyoDtlPlayer = transform.FindChild("BusyoDtlPlayer").gameObject;
                                    Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                                    if ((targetScale.x > 0 && targetChldScale.x < 0) || (targetScale.x < 0 && targetChldScale.x > 0)) {
                                        targetChldScale.x *= -1;
                                        BusyoDtlPlayer.transform.localScale = targetChldScale;
                                    }
                                }
                            }

						} else {
							if (leftFlg) {
								Vector2 targetScale = transform.localScale;
								targetScale.x *= -1;
								transform.localScale = targetScale;
								leftFlg = false;

                                //Name Bar
                                if(transform.FindChild("BusyoDtlEnemy")) {
                                    GameObject BusyoDtlPlayer = transform.FindChild("BusyoDtlEnemy").gameObject;
                                    Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                                    if ((targetScale.x > 0 && targetChldScale.x < 0) || (targetScale.x < 0 && targetChldScale.x > 0)) {
                                        targetChldScale.x *= -1;
                                        BusyoDtlPlayer.transform.localScale = targetChldScale;
                                    }
                                }
                                else if (transform.FindChild("BusyoDtlPlayer")) {
                                    GameObject BusyoDtlPlayer = transform.FindChild("BusyoDtlPlayer").gameObject;
                                    Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                                    if ((targetScale.x > 0 && targetChldScale.x < 0) || (targetScale.x < 0 && targetChldScale.x > 0)) {
                                        targetChldScale.x *= -1;
                                        BusyoDtlPlayer.transform.localScale = targetChldScale;
                                    }
                                }
                            }
						}
					}
				}

			}
		
		} else {

			if (!anim.GetBool ("IsAttack")) {
				//Back to Shiro or Toride
				anim.SetBool ("IsAttack", false); 
				GetComponent<AttackLong> ().enabled = false;
				Move (backShiroObj);

				if (Mathf.Abs (backShiroObj.transform.position.x - transform.position.x) < 1) {
					//Close
					backShiroObj.GetComponent<ShiroSearch> ().busyoObjList.Add (gameObject);
					GetComponent<HomingLong> ().backShiroFlg = false;
					GetComponent<HomingLong> ().enabled = false;
					backShiroObj.GetComponent<ShiroSearch> ().busyoObjSize.Add (transform.localScale);
					backShiroObj.GetComponent<ShiroSearch> ().AITypeList.Add (AIType);
					transform.localScale = new Vector2 (0, 0);
					GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
					GetComponent<Rigidbody2D> ().angularVelocity = 0;
					backShiroObj.GetComponent<ShiroSearch> ().outBusyoObjList.Remove (gameObject);

				}
			}else {
                nearObj = serchTagOnLine(gameObject, targetTag);
                backShiroFlg = false;
            }
		}
	}

	void Move (GameObject target){

		GetComponent<Rigidbody2D> ().velocity = (target.transform.position - transform.position).normalized * speed;

		//Change Sprite Direction
		if (target.transform.position.x < transform.position.x) {
			if (!leftFlg) {
				Vector2 targetScale = transform.localScale;
				targetScale.x *= -1;
				transform.localScale = targetScale;
				leftFlg = true;

                //Name Bar
                if (transform.FindChild("BusyoDtlEnemy")) {
                    GameObject BusyoDtlPlayer = transform.FindChild("BusyoDtlEnemy").gameObject;
                    Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                    if ((targetScale.x > 0 && targetChldScale.x < 0) || (targetScale.x < 0 && targetChldScale.x > 0)) {
                        targetChldScale.x *= -1;
                        BusyoDtlPlayer.transform.localScale = targetChldScale;
                    }
                }
                else if (transform.FindChild("BusyoDtlPlayer")) {
                    GameObject BusyoDtlPlayer = transform.FindChild("BusyoDtlPlayer").gameObject;
                    Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                    if ((targetScale.x > 0 && targetChldScale.x < 0) || (targetScale.x < 0 && targetChldScale.x > 0)) {
                        targetChldScale.x *= -1;
                        BusyoDtlPlayer.transform.localScale = targetChldScale;
                    }
                }
            }
				
		} else {
			if (leftFlg) {
				Vector2 targetScale = transform.localScale;
				targetScale.x *= -1;
				transform.localScale = targetScale;
				leftFlg = false;

                //Name Bar
                if (transform.FindChild("BusyoDtlEnemy")) {
                    GameObject BusyoDtlPlayer = transform.FindChild("BusyoDtlEnemy").gameObject;
                    Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                    if ((targetScale.x > 0 && targetChldScale.x < 0) || (targetScale.x < 0 && targetChldScale.x > 0)) {
                        targetChldScale.x *= -1;
                        BusyoDtlPlayer.transform.localScale = targetChldScale;
                    }
                }
                else if (transform.FindChild("BusyoDtlPlayer")) {
                    GameObject BusyoDtlPlayer = transform.FindChild("BusyoDtlPlayer").gameObject;
                    Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                    if ((targetScale.x > 0 && targetChldScale.x < 0) || (targetScale.x < 0 && targetChldScale.x > 0)) {
                        targetChldScale.x *= -1;
                        BusyoDtlPlayer.transform.localScale = targetChldScale;
                    }
                }
            }
		}
	}

	//指定されたタグの中で最も近いものを取得
	private float intervalSearch;
	private float intervalSearchMax = 0.5f;
	GameObject serchTagOnLine(GameObject nowObj,string targetTag){
		float tmpDis = 0;           //距離用一時変数
		float nearDis = 0;          //最も近いオブジェクトの距離
		GameObject targetObj = null; //オブジェクト

		if(PlayerInstance.isDebugEnableOptimizeHoming){
			//一定のインターバルでのみ、近いオブジェクトのサーチを行う
			intervalSearch += Time.deltaTime;
			if(intervalSearch >= intervalSearchMax){
				intervalSearch -= intervalSearchMax;
				//このまま近いオブジェクトの検出処理に移行
			}else{
				if(nearObj != null){
					//既に近いオブジェクトを取得済みならそれを返す
					return nearObj;
				}
			}
		}

		//タグ指定されたオブジェクトを配列で取得する
		foreach (GameObject obs in  GameObject.FindGameObjectsWithTag(targetTag)){

			//自身と取得したオブジェクトの距離を取得
			tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);
	
			//オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
			//一時変数に距離を格納
			if (nearDis == 0 || nearDis > tmpDis){
				nearDis = tmpDis;
				Dis = nearDis;
				targetObj = obs;
			}

		}

		//最も近かったオブジェクトを返す
		return targetObj;
	}



}