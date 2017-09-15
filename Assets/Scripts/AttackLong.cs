using UnityEngine;
using System.Collections;

public class AttackLong : MonoBehaviour {

	public IEnumerator coroutine;

	public Animator anim;
	public float attack = 0;
	public float childAttack = 0;

	// 移動スピード
	public float speed = 5;
	// Bulletプレハブ
	public GameObject bullet;
	public float aim;
	public float coolTime = 0;
	public bool leftFlg;

	//OK or NG
	public bool fireFlg = false;

	//SE
	public AudioSource audioSourceBattle;

	void Awake() {
		coroutine = Attack();

	}


	// Startメソッドをコルーチンとして呼び出す
	IEnumerator Attack (){

		while (true) {

			//Rotation
			if (GetComponent<HomingLong> ()) {
				leftFlg = GetComponent<HomingLong> ().leftFlg;
				if (GetComponent<HomingLong> ().nearObj != null) {
					aim = GetAim (this.transform.position, this.transform.GetComponent<HomingLong> ().nearObj.transform.position);
				}

			} else {
				leftFlg = GetComponent<UnitMover> ().leftFlg;
				if (GetComponent<UnitMover> ().nearObj != null) {
					aim = GetAim (this.transform.position, this.transform.GetComponent<UnitMover> ().nearObj.transform.position);
				}
			}

			if (leftFlg) {
				if (GetComponent<UnitMover> ()) {
					if (GetComponent<UnitMover> ().nearObj != null) {
						if (this.transform.GetComponent<UnitMover> ().nearObj.transform.position.x < this.transform.position.x) {
							fireFlg = true;
						} else {
							fireFlg = false;
						}
					}
				} else if(this.transform.GetComponent<HomingLong> ()){
					if (this.transform.GetComponent<HomingLong> ().nearObj != null) {
						if (this.transform.GetComponent<HomingLong> ().nearObj.transform.position.x < this.transform.position.x) {
							fireFlg = true;
						} else {
							fireFlg = false;
						}
					}
				}
			} else {
				if (this.transform.GetComponent<UnitMover> ()) {
					if (this.transform.GetComponent<UnitMover> ().nearObj != null) {
						if (this.transform.GetComponent<UnitMover> ().nearObj.transform.position.x > this.transform.position.x) {
							fireFlg = true;
						} else {
							fireFlg = false;
						}
					}
				}else if(this.transform.GetComponent<HomingLong> ()){
					if (this.transform.GetComponent<HomingLong> ().nearObj != null) {
						if (this.transform.GetComponent<HomingLong> ().nearObj.transform.position.x > this.transform.position.x) {
							fireFlg = true;
						} else {
							fireFlg = false;
						}
					}
				}
			}

			if (fireFlg) {
				// 弾をプレイヤーと同じ位置/角度で作成,
				//Get leftFlg
				string heisyu = GetComponent<Heisyu> ().heisyu;
				Bullet b = new Bullet ();
				if (heisyu == "YM") {
					if (!leftFlg) {
						GameObject bull = Instantiate (bullet, transform.position, transform.rotation) as GameObject;
						b = bull.GetComponent<Bullet> ();
					} else {
						GameObject bull = Instantiate (bullet, transform.position, transform.rotation) as GameObject;
						b = bull.GetComponent<Bullet> ();
						Vector2 targetScale = b.transform.localScale;
						targetScale.x *= -1;
						b.transform.localScale = targetScale;
					}
				} else if (heisyu == "TP") {
					GameObject bull = Instantiate (bullet, transform.position, transform.rotation) as GameObject;
					b = bull.GetComponent<Bullet> ();
				}

				//Add Attack
				b.attack = attack;
				b.tag = this.tag;
				b.myHeisyu = heisyu;

				//Set Parent for Senkou Count
				b.parent = gameObject.transform;

				//Set Aim
				b.aim = aim;

				//SE
				AudioSource[] audioSources = GetComponents<AudioSource> ();
				if (audioSources.Length != 0) {
					audioSourceBattle = audioSources [1];
					audioSourceBattle.Play ();
				}
				ChildAttack (bullet, aim, heisyu, leftFlg);
			}

			//Wait Cool Time
			yield return new WaitForSeconds (coolTime);

		}
	}

	public float GetAim(Vector2 p1, Vector2 p2) {
		float dx = p2.x - p1.x;
		float dy = p2.y - p1.y;

		float rad = Mathf.Atan2(dy, dx);
		return rad * Mathf.Rad2Deg;
	}


	public void ChildAttack(GameObject bullet,float aim, string heisyu, bool leftFlg){
		foreach (Transform child in transform) {
			if (child.tag == "PlayerChild" || child.tag == "EnemyChild") {
				GameObject bull = Instantiate (bullet, child.transform.position, child.transform.rotation) as GameObject;
                Bullet b = bull.GetComponent<Bullet> ();
				if(leftFlg){
					Vector2 targetScale = b.transform.localScale;
					targetScale.x *= -1;
					b.transform.localScale = targetScale;
				}
				b.attack = childAttack;
				b.tag = this.tag;
				b.myHeisyu = heisyu;

				b.aim = aim;
				b.parent = gameObject.transform;

				//SE
				audioSourceBattle.Play();
			}
		}
	}
    /*
    public void ChildAttack(GameObject bullet, float aim, string heisyu, bool leftFlg) {
        foreach (Transform child in transform) {
            Bullet b = new Bullet();
            if (child.tag == "PlayerChild" || child.tag == "EnemyChild") {
                GameObject bull = Instantiate(bullet, child.transform.position, child.transform.rotation) as GameObject;
                b = bull.GetComponent<Bullet>();
                if (leftFlg) {
                    Vector2 targetScale = b.transform.localScale;
                    targetScale.x *= -1;
                    b.transform.localScale = targetScale;
                }
                b.attack = childAttack;
                b.tag = this.tag;
                b.myHeisyu = heisyu;

                b.aim = aim;
                b.parent = gameObject.transform;

                //SE
                audioSourceBattle.Play();
            }
        }
    }
    */


    void OnDisable(){
        StopCoroutine (coroutine);
    }

    void OnEnable(){
		StartCoroutine (coroutine);
	}

}