using UnityEngine;
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

		AudioSource[] audioSources =GetComponents<AudioSource>();
		if (audioSources.Length != 0) {
			audioSources [0].Play ();
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
				GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
				GetComponent<Rigidbody2D> ().angularVelocity = 0;

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
		    GetComponent<Rigidbody2D>().velocity = (target.transform.position - transform.position).normalized * speed;

            //Change Sprite Direction
            if (target.transform.position.x < transform.position.x) {
			    if(!leftFlg){
				    Vector2 targetScale = transform.localScale;
				    targetScale.x *= -1;
				    transform.localScale = targetScale;
				    leftFlg = true;

                    //Name Bar
                    if(transform.FindChild("BusyoDtlEnemy")) {
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
			    if(leftFlg){
				    Vector2 targetScale = transform.localScale;
				    targetScale.x *= -1;
				    transform.localScale = targetScale;
				    leftFlg = false;

                    //Name Bar
                    if (transform.FindChild("BusyoDtlEnemy")) {
                        GameObject BusyoDtlPlayer = transform.FindChild("BusyoDtlEnemy").gameObject;
                        Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                        if((targetScale.x > 0 && targetChldScale.x <0) ||(targetScale.x < 0 && targetChldScale.x > 0)) {
                            targetChldScale.x *= -1;
                            BusyoDtlPlayer.transform.localScale = targetChldScale;
                        }
                    }else if (transform.FindChild("BusyoDtlPlayer")) {
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


	//指定されたタグの中で最も近いものを取得
	GameObject serchTagOnLine(GameObject nowObj,string targetTag){
		float tmpDis = 0;           //距離用一時変数
		float nearDis = 0;          //最も近いオブジェクトの距離
		GameObject targetObj = null; //オブジェクト
		
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

		//最も近かったオブジェクトを返す
		return targetObj;
	}
}