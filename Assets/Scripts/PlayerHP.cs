using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class PlayerHP : MonoBehaviour {
	public float initLife = 0;
	public float life = 100; //現在兵力
	public float dfc = 10; //知略
	public bool taisyo;
	public GameObject attackObj;

	public int childQty = 0;
	public float childHP = 0;
	public float childHPTmp = 0;
    public float dmgOpt = 0;

	//Damage
	GameObject canvas;
	string damagePath = "";

	void Start(){
		initLife = life;
		childHPTmp = childHP;

		//Damage
		canvas = GameObject.Find ("Canvas").gameObject;
		damagePath = "Prefabs/PreKassen/Damage";
	}

	// Update is called once per frame
	void Update () {
		if (life <= 0) {
			Dead();

			if(taisyo){
				//Message
				Message msg = new Message ();
                string text = "";
                int langId = PlayerPrefs.GetInt("langId");
                if (langId == 2) {
                    text = "Player leader was defeated！";
                }else {
                    text = "お味方の大将、退却！";
                }
				msg.makeKassenMessage (text);

				//Atk, Dfc, Hp 1/2
				foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Player")){
					obs.GetComponent<PlayerHP>().life = Mathf.Ceil (obs.GetComponent<PlayerHP>().life/2);
                    obs.GetComponent<PlayerHP>().childHPTmp = Mathf.Ceil(obs.GetComponent<PlayerHP>().childHPTmp / 2);
                    obs.GetComponent<PlayerHP>().childHP = Mathf.Ceil(obs.GetComponent<PlayerHP>().childHP / 2);
                    obs.GetComponent<PlayerHP>().dfc = Mathf.Ceil (obs.GetComponent<PlayerHP>().dfc/2);
					
					if(obs.GetComponent<PlayerAttack>() != null){
						obs.GetComponent<PlayerAttack>().attack = Mathf.Ceil (obs.GetComponent<PlayerAttack>().attack/2);
					}else if(obs.GetComponent<AttackLong>() != null){
						obs.GetComponent<AttackLong>().attack = Mathf.Ceil (obs.GetComponent<AttackLong>().attack/2);
					}
				}

				//Reflag
				taisyo = false;
			}
		}
	}
	
	public void Damage (float damage){

        GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        //Damage Optimize
        dmgOpt = (int)(dfc / 50);
        if(dmgOpt<1) {
            dmgOpt = 1;
        }else if(dmgOpt > 90) {
            dmgOpt = 90;
        }
        damage = Mathf.Floor(damage * (100 - dmgOpt) / 100);
        if(damage<=1) {
            damage = 1;
        }
        

        //Damage
        GameObject damageObj = Instantiate (Resources.Load (damagePath)) as GameObject;
		damageObj.transform.SetParent(canvas.transform);
		damageObj.GetComponent<Text> ().text = "-" + damage;
		damageObj.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y + 5, 0);
		damageObj.transform.localScale = new Vector3 (0.08f,0.08f,0);


		if (childQty <= 0) {
			life -= damage; //兵力減らす
		} else {
			childHPTmp -= damage;
			if (childHPTmp <= 0) {
				
				//dead child
				foreach (Transform child in transform){
					if(child.tag == "PlayerChild" ){
						if(child.name == "Child"+childQty.ToString()){
							child.transform.DetachChildren();
							Destroy(child.gameObject);
							break;
						}
					}
				}

				childQty -= 1;
				childHPTmp = childHP;

                //minus atk & dfc
                float chldAtk = GetComponent<Heisyu>().atk;
                float chldDfc = GetComponent<Heisyu>().dfc;
                dfc = dfc - chldDfc;
                if (GetComponent<PlayerAttack>()) {
                    float prntAtk = GetComponent<PlayerAttack>().attack;
                    GetComponent<PlayerAttack>().attack = prntAtk - chldAtk;
                }else if (GetComponent<AttackLong>()) {
                    float prntAtk = GetComponent<AttackLong>().attack;
                    GetComponent<AttackLong>().attack = prntAtk - chldAtk;
                }
            }
		}
	}

	public void DirectDamage (float damage){

        GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        //Damage
        GameObject damageObj = Instantiate (Resources.Load (damagePath)) as GameObject;
		damageObj.transform.SetParent(canvas.transform);
		damageObj.GetComponent<Text> ().text = "-" + damage;
		damageObj.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y + 5, 0);
		damageObj.transform.localScale = new Vector3 (0.08f,0.08f,0);

		life -= damage; //兵力減らす

	}

	public void Dead(){

		if (name == "shiro" || name == "toride") {


			//SE
			AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
			audioSources [6].Play ();

			//Animation
			string animPath = "Prefabs/Kassen/destroyShiro";
			GameObject destroyObj = Instantiate (Resources.Load (animPath)) as GameObject;		
			destroyObj.transform.localScale = new Vector2 (15, 8);
			destroyObj.transform.localPosition = gameObject.transform.localPosition;

			Destroy (gameObject);

			if (name == "shiro") {
				foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Player")){
					Destroy (obs);
				}
			}

		} else {


			//stop animation &SE
			if (attackObj) {
				if (attackObj.GetComponent<PlayerAttack> ()) {
					attackObj.GetComponent<PlayerAttack> ().anim.SetBool ("IsAttack", false);
				}
				AudioSource[] audioSources = attackObj.GetComponents<AudioSource> ();
                if(audioSources.Length > 0) {
				    audioSources [1].Stop ();
                }
            }

			//Delete Effect
			foreach (Transform child in transform) {
				if (child.tag == "Senpou") {
					Destroy (child.gameObject);
				}
			}
			//Add Kunkou to DeadList
			if (gameObject.GetComponent<Kunkou> () != null) {
				GameObject obj = GameObject.Find ("GameScene");
				if (name != "kengou") {
					obj.gameObject.SendMessage ("AddDeadBusyo", this.gameObject);
				}
			}


			//child 
			string heisyu = gameObject.GetComponent<Heisyu> ().heisyu;
			foreach (Transform chObj in gameObject.transform) {
				if (chObj.tag == "PlayerChild") {
					//add component
					chObj.name = "hukuhei";
					chObj.gameObject.AddComponent<PlayerHP> ();
					chObj.gameObject.GetComponent<PlayerHP> ().life = childHPTmp;
					chObj.tag = gameObject.tag;
					chObj.gameObject.AddComponent<PolygonCollider2D> ();
					chObj.gameObject.AddComponent<Rigidbody2D> ();
					chObj.gameObject.GetComponent<Rigidbody2D> ().gravityScale = 0;
					chObj.gameObject.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation;
					chObj.gameObject.AddComponent<Heisyu> ();
					chObj.gameObject.GetComponent<Heisyu> ().heisyu = heisyu;
					chObj.gameObject.AddComponent<Escape> ();

					chObj.parent = null;
				}
				if (chObj.tag == "PlayerHata") {
					chObj.parent = null;				
				}
			}

			Destroy (gameObject);
		
		}
	}

}
