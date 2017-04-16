using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

	public Animator anim;

	//自軍攻撃力
	public float attack = 20; 
	private float timeleft;

	//SE
	public bool atkSEOnFlg = false;
    public AudioSource[] audioSources;

    void Start() { 
		anim = GetComponent( "Animator" ) as Animator;
		anim.SetBool("IsAttack", false );
		audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
	} 

	void OnCollisionExit2D(Collision2D col){
		if (col.gameObject.tag == "Enemy") {
			anim.SetBool ("IsAttack", false);
			atkSEOnFlg = false;
		}
	}

	void OnCollisionStay2D(Collision2D col){
		
		bool backFlg = false;
		// 衝突対象のタグがEnemyの場合
		if (col.gameObject.tag == "Enemy") {
			
			timeleft -= Time.deltaTime;
			if (timeleft <= 0.0) {
				timeleft = 1.0f;
				int Damage = (int)attack;

				//Attack Adjustment
				string enemyHeisyu = col.gameObject.GetComponent<Heisyu>().heisyu;
				string playerHeisyu = this.gameObject.GetComponent<Heisyu>().heisyu;
				if((playerHeisyu=="KB" && enemyHeisyu=="YR")||(playerHeisyu=="KB" && enemyHeisyu=="YM")
				   ||(playerHeisyu=="YR" && enemyHeisyu=="TP")){
					Damage = Damage * 2;
				}

				//Get my left flg
				bool myLeftFlg = false;
				if (GetComponent<UnitMover> ()) {
					myLeftFlg = GetComponent<UnitMover> ().leftFlg;
				} else if (GetComponent<Homing> ()) {
					myLeftFlg = GetComponent<Homing> ().leftFlg;
				} else if (GetComponent<HomingLong> ()) {
					myLeftFlg = GetComponent<HomingLong> ().leftFlg;
				}

				if(myLeftFlg){
					//Player left & Enemy left
					if(col.gameObject.GetComponent<Homing>() !=null){
						if(col.gameObject.GetComponent<Homing>().leftFlg){
							Damage = Damage*2;
							makeKatanaBack(myLeftFlg);
							backFlg = true;
						}
					
					}else if(col.gameObject.GetComponent<HomingLong>() !=null){
						if(col.gameObject.GetComponent<HomingLong>().leftFlg){
							Damage = Damage*2;
							makeKatanaBack(myLeftFlg);
							backFlg = true;
						}

					}
				}else{
					//Enemy right & enemy right
					if(col.gameObject.GetComponent<Homing>() !=null){
						if(!col.gameObject.GetComponent<Homing>().leftFlg){
							Damage = Damage*2;
							makeKatanaBack(myLeftFlg);
							backFlg = true;
						}
						
					}else if(col.gameObject.GetComponent<HomingLong>() !=null){
						if(!col.gameObject.GetComponent<HomingLong>().leftFlg){
							Damage = Damage*2;
							makeKatanaBack(myLeftFlg);
							backFlg = true;
						}
						
					}

				}

				if(!backFlg){
					if(col.gameObject.GetComponent<Homing>() !=null){
						makeKatana(myLeftFlg);
					}else if(col.gameObject.GetComponent<HomingLong>() !=null){
						makeKatana(myLeftFlg);
					}
				}


				col.gameObject.GetComponent<EnemyHP>().attackObj = gameObject;
				float actDamage = col.gameObject.GetComponent<EnemyHP>().Damage(Damage);
				if (name != "hukuhei") {
					gameObject.GetComponent<Kunkou> ().kunkou = gameObject.GetComponent<Kunkou> ().kunkou + (int)actDamage;
				}
				anim.SetBool ("IsAttack", true);

				if (!atkSEOnFlg) {
					if (!audioSources[11].isPlaying) {
                        audioSources[11].Play ();
						atkSEOnFlg = true;
					}
				}
			}
		} 
	}


	public void makeKatanaBack(bool myLeftFlg){
		string katanaPath = "Prefabs/Common/KatanaBack";
		GameObject katana = Instantiate (Resources.Load (katanaPath)) as GameObject;
		katana.transform.SetParent(gameObject.transform);
		if (myLeftFlg) {
			katana.transform.localScale = new Vector2 (-1, 1);
		} else {
			katana.transform.localScale = new Vector2 (1, 1);
		}
		RectTransform katanaTransform = katana.GetComponent<RectTransform> ();
		katanaTransform.anchoredPosition = new Vector2( 1, 1);
	}

	public void makeKatana(bool myLeftFlg){
		string katanaPath = "Prefabs/Common/Katana";
		GameObject katana = Instantiate (Resources.Load (katanaPath)) as GameObject;
		katana.transform.SetParent(gameObject.transform);
		if (myLeftFlg) {
			katana.transform.localScale = new Vector2 (-1, 1);
		} else {
			katana.transform.localScale = new Vector2 (1, 1);
		}
		RectTransform katanaTransform = katana.GetComponent<RectTransform> ();
		katanaTransform.anchoredPosition = new Vector2( 1, 1);
	}
	
}