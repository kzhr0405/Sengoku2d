using UnityEngine;
using System.Collections;

public class UnitMover : MonoBehaviour {

	public bool touchFlg = false;

	public float speed = 1;
	public Vector3 touchStartPosition = new Vector3();
	public Vector3 touchEndPosition = new Vector3();
	public bool leftFlg = false;
	public string heisyu = "";

	//TP or YM
	public float DisTarget = 0;
	public float coolTime = 0;
	public float Dis = 0;
	public GameObject nearObj;
	public Animator anim;
	private float timeleft;


    void Start () {



        if (heisyu == "TP" || heisyu == "YM") {
			anim = this.GetComponent ("Animator") as Animator;


			if (heisyu == "TP") {
				DisTarget = 30;
				coolTime = 5;
			} else if (heisyu == "YM") {
				DisTarget = 20;
				coolTime = 3;
			}

			GetComponent<AttackLong> ().coolTime = coolTime;

            if(GetComponent<SenpouController>()) {
			    GetComponent<SenpouController> ().initCoolTime = coolTime;
			    GetComponent<SenpouController> ().initDisTarget = DisTarget;
            }
        }

    }

    void Update () {
		    
			if (touchEndPosition != Vector3.zero) {
				Move (touchEndPosition);
			} else {
				if (heisyu == "TP" || heisyu == "YM") {
					attackTPYM ();
				}
			}

			//Reset
			if (touchFlg) {
				timeleft -= Time.deltaTime;
				if (timeleft <= 0.0) {
					timeleft = 5.0f;
					touchFlg = false;
				}
			}

	}


	public void Move(Vector3 touchEndPosition){
		GetComponent<Rigidbody2D>().velocity = (touchEndPosition - transform.position).normalized * speed;

		//Change Direction
		if (touchEndPosition.x >= transform.position.x) {
			if (leftFlg) {
				Vector2 targetScale = transform.localScale;
				targetScale.x *= -1;
				transform.localScale = targetScale;
				leftFlg = false;

                //Name Bar
                GameObject BusyoDtlPlayer = transform.Find("BusyoDtlPlayer").gameObject;
                Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                targetChldScale.x *= -1;
                BusyoDtlPlayer.transform.localScale = targetChldScale;
            }

		} else {
			if (!leftFlg) {
				Vector2 targetScale = transform.localScale;
				targetScale.x *= -1;
				transform.localScale = targetScale;
				leftFlg = true;

                //Name Bar
                GameObject BusyoDtlPlayer = transform.Find("BusyoDtlPlayer").gameObject;
                Vector2 targetChldScale = BusyoDtlPlayer.transform.localScale;
                targetChldScale.x *= -1;
                BusyoDtlPlayer.transform.localScale = targetChldScale;
            }
		}

		if (heisyu == "TP" || heisyu == "YM") {
			attackTPYM ();
		}
        
	}

	public void attackTPYM(){

		if (!touchFlg) {			
			nearObj = serchTagOnLine (gameObject, "Enemy");
			if (Dis > DisTarget) {
				//Keep Move
				anim.SetBool ("IsAttack", false);
				GetComponent<AttackLong> ().enabled = false;

			} else {
				//Stop & Attack
				if(!GetComponent<AttackLong> ().enabled){
					GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
					GetComponent<Rigidbody2D> ().angularVelocity = 0;

					anim.SetBool ("IsAttack", true);
					GetComponent<AttackLong> ().enabled = true;
				}

			}
		} else {
			anim.SetBool ("IsAttack", false);
			GetComponent<AttackLong> ().enabled = false;
		}
	}


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
				Dis = nearDis;
				targetObj = obs;
			}
		}


		return targetObj;

	}




}
