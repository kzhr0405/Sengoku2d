using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Homing : MonoBehaviour {

	public int AIType = 1;
	public bool backShiroFlg = false;
	public GameObject backShiroObj;
	public bool helpTaisyoShiroFlg = false;

	public GameObject nearObj;         //最も近いオブジェクト
	public float speed = 2;	//移動速度
	public string targetTag;
	Animator anim;
	public bool leftFlg = false; //左を向いているか
    public bool KBFlg = false;
    public GameObject playerTaisyoObj;
	private NavMeshAgent2D navMeshAgent2D;

	void Start(){


		if (tag == "Player") {
			targetTag = "Enemy";
        } else {
			targetTag = "Player";
            //AIType = 4;//test
        }
        anim = this.GetComponent ("Animator") as Animator;
		nearObj = serchTagOnLine (gameObject, targetTag);

        if (GetComponent<Heisyu>().heisyu == "KB") KBFlg = true;

        if (AIType != 3) {
			Move (nearObj);
		}

		AudioSource[] audioSources =GetComponents<AudioSource>();
		if (audioSources.Length != 0) {
			audioSources [0].Play ();
		}

		navMeshAgent2D = GetComponent<NavMeshAgent2D>();
		if(navMeshAgent2D == null){
			//Debug.Log("navMeshAgent2D is null!");
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (!backShiroFlg) {
			if (!helpTaisyoShiroFlg) {
				nearObj = serchTagOnLine (gameObject, targetTag);
			}else {
                if(nearObj == null) {
                    nearObj = serchTagOnLine(gameObject, targetTag);
                }
            }

			if (nearObj != null) {
				if (AIType != 3) { //Don't move and Waiting at shiro
					Move (nearObj);
				}
			} else {
				//相手が居ない場合ストップ
				if(GameScene.isUseNavigation && navMeshAgent2D != null){
					if(navMeshAgent2D.pathStatus != NavMeshPathStatus.PathInvalid){
						navMeshAgent2D.destination = transform.position;//自分自身
					}
				}else{
					GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
					GetComponent<Rigidbody2D> ().angularVelocity = 0;
				}
				anim.SetBool ("IsAttack", false); 
			}
		} else {

			if (!anim.GetBool ("IsAttack")) {
				//Back to Shiro or Toride
				anim.SetBool ("IsAttack", false); 
				Move (backShiroObj);

				if (Mathf.Abs (backShiroObj.transform.position.x - transform.position.x) < 1) {
					backShiroObj.GetComponent<ShiroSearch> ().busyoObjList.Add (gameObject);
					GetComponent<Homing> ().enabled = false;
					GetComponent<Homing> ().backShiroFlg = false;
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

	void Move(GameObject target){
		// velocityは移動速度なので(target.transform.position - transform.position)だけでは
		// 距離によって移動速度が変わってしまう為normalizedで一定速度にする

        if(target != null) {
			if(GameScene.isUseNavigation && navMeshAgent2D != null){
				var movePos = target.transform.position;
				var pos = navMeshAgent2D.destination;
				if(pos.x != movePos.x || pos.y != movePos.y){
					if(navMeshAgent2D.pathStatus != NavMeshPathStatus.PathInvalid){
						navMeshAgent2D.destination = movePos;
					}
				}else{
					//既に設定済みの時は何もしない
				}
			}else{
				GetComponent<Rigidbody2D>().velocity = (target.transform.position - transform.position).normalized * speed;
			}

            //Change Sprite Direction
            if (target.transform.position.x < transform.position.x) {
			    if(!leftFlg){
				    Vector2 targetScale = transform.localScale;
				    targetScale.x *= -1;
				    transform.localScale = targetScale;
				    leftFlg = true;

                    //Name Bar
                    if(transform.Find("BusyoDtlEnemy")) {
                        GameObject BusyoDtlPlayer = transform.Find("BusyoDtlEnemy").gameObject;
                        Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                        if ((targetScale.x > 0 && targetChldScale.x < 0) || (targetScale.x < 0 && targetChldScale.x > 0)) {
                            targetChldScale.x *= -1;
                            BusyoDtlPlayer.transform.localScale = targetChldScale;
                        }
                    }else if(transform.Find("BusyoDtlPlayer")) {
                        GameObject BusyoDtlPlayer = transform.Find("BusyoDtlPlayer").gameObject;
                        Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                        if ((targetScale.x > 0 && targetChldScale.x < 0) || (targetScale.x < 0 && targetChldScale.x > 0)) {
                            targetChldScale.x *= -1;
                            BusyoDtlPlayer.transform.localScale = targetChldScale;
                        }
                    }
                }
			
		    } else {
			    if(leftFlg){
				    Vector2 targetScale = transform.localScale;
				    targetScale.x *= -1;
				    transform.localScale = targetScale;
				    leftFlg = false;

                    //Name Bar
                    if (transform.Find("BusyoDtlEnemy")) {
                        GameObject BusyoDtlPlayer = transform.Find("BusyoDtlEnemy").gameObject;
                        Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                        if((targetScale.x > 0 && targetChldScale.x <0) ||(targetScale.x < 0 && targetChldScale.x > 0)) {
                            targetChldScale.x *= -1;
                            BusyoDtlPlayer.transform.localScale = targetChldScale;
                        }
                    }else if (transform.Find("BusyoDtlPlayer")) {
                        GameObject BusyoDtlPlayer = transform.Find("BusyoDtlPlayer").gameObject;
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
				targetObj = obs;
			}
		}

        if (AIType == 4) {
            //Find Taisyo
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag(targetTag)) {
                if(obs.GetComponent<PlayerHP>().taisyo) {
                    targetObj = obs;
                }
            }
        }

        //最も近かったオブジェクトを返す
        return targetObj;
	}
}