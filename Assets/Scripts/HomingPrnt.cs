using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class HomingPrnt : MonoBehaviour {

	Animator anim;
	private GameObject parent;
	public float speed = 2;			//移動速度
	public string targetTag;
	public GameObject nearObj;         //最も近いオブジェクト
	public bool leftFlg = false; //左を向いているか
	private NavMeshAgent2D navMeshAgent2D;

	// Use this for initialization
	void Start () {
		//親のスピードを取得
		parent = transform.root.gameObject;
		if (parent.name != "hukuhei") {
			if (this.tag == "Player") {
				speed = parent.GetComponent<UnitMover> ().speed;
			} else {
				speed = parent.GetComponent<Homing> ().speed;
			}
		}

		if (this.tag == "Enemy") {
			targetTag = "Player";	
		} else if (this.tag == "Player") {
			targetTag = "Enemy";
		}

		navMeshAgent2D = GetComponent<NavMeshAgent2D>();
		if(navMeshAgent2D == null){
			//Debug.Log("navMeshAgent2D is null!");
		}
	}
	
	// Update is called once per frame
	void Update () {

		//親が居なかったら自分でHomingする
		if (transform.parent == null) {
			nearObj = serchTag (gameObject, targetTag);

			if (nearObj != null) {
				MoveWithoutParent (nearObj);

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

				anim = GetComponent( "Animator" ) as Animator;
				anim.SetBool("IsAttack", false ); 
			}
		}
	}

	public void Move(GameObject target){
		nearObj = target;

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

	}

	public void Stop(){
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		GetComponent<Rigidbody2D> ().angularVelocity = 0;
		anim = GetComponent( "Animator" ) as Animator;
		anim.SetBool("IsAttack", false ); 
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
				//nearObjName = obs.name;
				targetObj = obs;
			}
			
		}
		//最も近かったオブジェクトを返す
		return targetObj;
	}

	public void MoveWithoutParent(GameObject target){
		nearObj = target;
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
}
