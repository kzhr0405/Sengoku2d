using UnityEngine;
using System.Collections;

public class HomingLongPrnt : MonoBehaviour {
	
	public Animator anim;
	private GameObject parent;
	public float speed = 2;			//移動速度
	public string targetTag;
	public GameObject nearObj;         //最も近いオブジェクト
	public float Dis = 0;
	public float DisTarget = 0;
	public bool leftFlg = false; //左を向いているか

	// Use this for initialization
	void Start () {

		anim = GetComponent( "Animator" ) as Animator;

		//親のスピードを取得
		parent = transform.root.gameObject;
		if (parent.name != "hukuhei") {
			if (this.tag == "Player") {
				speed = parent.GetComponent<UnitMover> ().speed;
			} else {
				speed = parent.GetComponent<HomingLong> ().speed;
			}
		}	
		if (this.tag == "Enemy") {
			targetTag = "Player";
		} else if (this.tag == "Player") {
			targetTag = "Enemy";
		}

		//TP or YM
		string heisyu = this.GetComponent<Heisyu>().heisyu;
		if(heisyu=="TP"){
			DisTarget=30;
			this.GetComponent<AttackLong>().coolTime = 5;
		}else if(heisyu=="YM"){
			DisTarget=15;
			this.GetComponent<AttackLong>().coolTime = 3;
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		//親が居なかったら自分でHomingする
		if (transform.parent == null) {
			nearObj = serchTag (gameObject, targetTag);

			if (Dis > DisTarget) {
				//Move
				if (nearObj != null) {
					Move (nearObj);
					
				} else {
					//相手が居ない場合ストップ
					GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
					GetComponent<Rigidbody2D> ().angularVelocity = 0;
					
					anim = GetComponent( "Animator" ) as Animator;
					anim.SetBool("IsAttack", false ); 
					GetComponent<AttackLong>().enabled = false;

				}
			}else{
				//Stop & Attack
				GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
				GetComponent<Rigidbody2D> ().angularVelocity = 0;
				
				anim.SetBool ("IsAttack", true);
				GetComponent<AttackLong>().enabled = true;
			}
		}
	}
	
	//public void Move(GameObject target){
	//	nearObj = target;
	//	GetComponent<Rigidbody2D>().velocity = (target.transform.position - transform.position).normalized * speed;
	//	anim = GetComponent( "Animator" ) as Animator;
	//}

	public void Move(GameObject target){
		// velocityは移動速度なので(target.transform.position - transform.position)だけでは
		// 距離によって移動速度が変わってしまう為normalizedで一定速度にする
		GetComponent<Rigidbody2D>().velocity = (target.transform.position - transform.position).normalized * speed;

		if (this.tag == "Player") { 
			if (target.transform.position.x > transform.position.x) {
				//Right
				if(leftFlg){
					Vector2 targetScale = transform.localScale;
					targetScale.x *= -1;
					transform.localScale = targetScale;
					leftFlg = false;
				}
				
			} else {
				//Left
				if(!leftFlg){
					Vector2 targetScale = transform.localScale;
					targetScale.x *= -1;
					transform.localScale = targetScale;
					leftFlg = true;
				}
			}
		}else if(this.tag == "Enemy"){
			if (target.transform.position.x < transform.position.x) {
				if(!leftFlg){
					Vector2 targetScale = transform.localScale;
					targetScale.x *= -1;
					transform.localScale = targetScale;
					leftFlg = true;
				}
				
			} else {
				if(leftFlg){
					Vector2 targetScale = transform.localScale;
					targetScale.x *= -1;
					transform.localScale = targetScale;
					leftFlg = false;
				}
			}
		}
	}



	public void Stop(){
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		GetComponent<Rigidbody2D> ().angularVelocity = 0;
		anim = GetComponent( "Animator" ) as Animator;
		anim.SetBool("IsAttack", false ); 
	}

	public void Attack(){
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		GetComponent<Rigidbody2D> ().angularVelocity = 0;
		anim = GetComponent( "Animator" ) as Animator;
		anim.SetBool("IsAttack", true );
		GetComponent<AttackLong>().enabled = true;
	}

	public void StopAttack(){
		anim = GetComponent( "Animator" ) as Animator;
		anim.SetBool("IsAttack", false );
		GetComponent<AttackLong>().enabled = false;
	}


	//指定されたタグの中で最も近いものを取得
	GameObject serchTag(GameObject nowObj,string tagName){
		float tmpDis = 0;           //距離用一時変数
		float nearDis = 0;          //最も近いオブジェクトの距離
		GameObject targetObj = null; //オブジェクト
		
		//タグ指定されたオブジェクトを配列で取得する
		foreach (GameObject obs in  GameObject.FindGameObjectsWithTag(tagName)){
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
