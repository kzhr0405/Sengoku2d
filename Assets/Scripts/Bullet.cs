using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public int speed = 10;
	public float lifeTime = 1;
	public float attack;
	public string tag;
	public Transform parent;
	public Transform itObj;
	public float aim;
	public string myHeisyu;

	void Start (){

		Vector2 v;

		v.x = Mathf.Cos (Mathf.Deg2Rad * aim) * speed;
		v.y = Mathf.Sin (Mathf.Deg2Rad * aim) * speed;
		GetComponent<Rigidbody2D> ().velocity = v;

	}

	void Update(){
		// 経過時間
		Destroy(gameObject,lifeTime);
	}

	void OnCollisionEnter2D(Collision2D col){

		int Damage =0;
		string hitHeisyu = col.gameObject.GetComponent<Heisyu>().heisyu;

		if(col.gameObject.tag == "Enemy"){
			Damage = (int)attack;

			//Damage Adjustment
			if(myHeisyu=="TP" && hitHeisyu == "KB"){
				Damage = Damage * 2;
			}else if(myHeisyu == "TP" && hitHeisyu == "YR"){
                Damage = Damage / 2;
            }


			col.gameObject.SendMessage ("Damage", Damage);
			if(parent != null && parent.name !="hukuhei"){
				//For Kunkou
				if (myHeisyu == "TP") {
					Damage = Damage / 5;	
				} else if (myHeisyu == "YM") {
					Damage = Damage / 3;
				}
				parent.GetComponent<Kunkou>().kunkou = parent.GetComponent<Kunkou>().kunkou + Damage;
			}

			if (parent != null) {
				col.gameObject.GetComponent<EnemyHP> ().attackObj = parent.gameObject;
			}
			Destroy (gameObject);


		}else if(col.gameObject.tag == "Player"){
			Damage = (int)attack;

			//Damage Adjustment
			if(myHeisyu=="TP" && hitHeisyu == "KB"){
				Damage = Damage * 2;
			}

			col.gameObject.SendMessage ("Damage", Damage);

			if (parent != null) {
				col.gameObject.GetComponent<PlayerHP> ().attackObj = parent.gameObject;
			}
			Destroy (gameObject);

		}
	}
}
