using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class EnemyHP : MonoBehaviour {
    public float initLife = 0;
    public float life = 100; //現在兵力
	public float dfc = 10; //知略
	public bool taisyo;
	public GameObject attackObj;
	public GameObject preAttackObj;

	public int childQty = 0;
	public float childHP = 0;
	public float childHPTmp = 0;

    //Damage
    GameObject canvas;
	string damagePath = "";
	public bool callHelpFlg = false;
    public float dmgOpt = 0;

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
                    text = "Enemy leader was defeated！";
                }else {
                    text = "敵方の大将、退却！";
                }


                msg.makeKassenMessage (text);

				//Atk, Dfc, Hp 1/2
				foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Enemy")){
					obs.GetComponent<EnemyHP>().life = Mathf.Ceil (obs.GetComponent<EnemyHP>().life/2);
					obs.GetComponent<EnemyHP>().dfc = Mathf.Ceil (obs.GetComponent<EnemyHP>().dfc/2);

					if(obs.GetComponent<EnemyAttack>() != null){
						obs.GetComponent<EnemyAttack>().attack = Mathf.Ceil (obs.GetComponent<EnemyAttack>().attack/2);
					}else if(obs.GetComponent<AttackLong>() != null){
						obs.GetComponent<AttackLong>().attack = Mathf.Ceil (obs.GetComponent<AttackLong>().attack/2);
					}
				}

				
				//Reflag
				taisyo = false;
			}
		}
	}

	public float Damage (float damage){
        float actualDamage = 0;

        GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        dmgOpt = (int)(dfc / 50);
        if (dmgOpt < 1) {
            dmgOpt = 1;
        }else if (dmgOpt > 90) {
            dmgOpt = 90;
        }
        damage = Mathf.Floor(damage * (100 - dmgOpt) / 100);
        if (damage <= 1) {
            damage = 1;
        }

        //Damage
        GameObject damageObj = Instantiate (Resources.Load (damagePath)) as GameObject;
		damageObj.transform.SetParent(canvas.transform);
		damageObj.GetComponent<Text> ().text = "-" + damage;
		damageObj.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y + 5, 0);
		damageObj.transform.localScale = new Vector3 (0.08f,0.08f,0);

		if (preAttackObj != attackObj) {
			callHelpFlg = false;
		}

        if(attackObj!=null) {
            if(GetComponent<Homing>()) {
                GetComponent<Homing>().nearObj = attackObj;
            }else if(GetComponent<HomingLong>()) {
                GetComponent<HomingLong>().nearObj = attackObj;
            }
        }

		if (!callHelpFlg) {
			if (taisyo || name == "shiro") {
				//Call My help
				preAttackObj = attackObj;
				callHelpFlg = true;
				foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Enemy")) {
					if (!obs.GetComponent<EnemyHP>().taisyo && obs.name !="shiro") {
						if (obs.GetComponent<Homing> ()) {
							Homing script = obs.GetComponent<Homing> ();
							script.helpTaisyoShiroFlg = true;
							script.nearObj = attackObj;
							script.AIType = 2;
							if (script.backShiroObj) {
								//toride
								ShiroSearch shiroScript = script.backShiroObj.GetComponent<ShiroSearch>();
								HelpSyutsujinFromToride(script.backShiroObj, shiroScript.busyoObjList, shiroScript.AITypeList,shiroScript.busyoObjSize, shiroScript.outBusyoObjList);

							}
						} else if (obs.GetComponent<HomingLong> ()) {
							HomingLong script = obs.GetComponent<HomingLong> ();
							script.helpTaisyoShiroFlg = true;
							script.nearObj = attackObj;
							script.AIType = 2;

							if (script.backShiroFlg) {
								//toride
								ShiroSearch shiroScript = script.backShiroObj.GetComponent<ShiroSearch>();
								HelpSyutsujinFromToride(script.backShiroObj, shiroScript.busyoObjList, shiroScript.AITypeList,shiroScript.busyoObjSize, shiroScript.outBusyoObjList);

							}
						}

					}
				}
			}
		}

		if (childQty <= 0) {
			life -= damage; //兵力減らす
            actualDamage = damage;
        } else {
            actualDamage = childHPTmp;
            childHPTmp -= damage;            
            if (childHPTmp <= 0) {

				//dead child
				foreach (Transform child in transform){
					if(child.tag == "EnemyChild" ){
                        if (child.name == "Child" + childQty.ToString()) {
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
                if(GetComponent<EnemyAttack>()) {
                    float prntAtk = GetComponent<EnemyAttack>().attack;
                    GetComponent<EnemyAttack>().attack = prntAtk - chldAtk;
                }
            }
		}

        return actualDamage;
    }

	public void HelpSyutsujinFromToride(GameObject sourceBuilding, List<GameObject> busyoObjList, List<int>AITypeList,List<Vector2>busyoObjSize, List<GameObject>outBusyoObjList){

		for (int i = 0; i < busyoObjList.Count; i++) {
			GameObject busyoObj = busyoObjList [i];
			if (busyoObj != null) {
				int AIType = 2;//Chasing

				if (busyoObj.GetComponent<Homing> ()) {
					busyoObj.GetComponent<Homing> ().enabled = true;
					busyoObj.GetComponent<Homing> ().AIType = AIType;
				} else if (busyoObj.GetComponent<HomingLong> ()) {
					busyoObj.GetComponent<HomingLong> ().enabled = true;			
					busyoObj.GetComponent<HomingLong> ().AIType = AIType;
				}
				busyoObj.transform.localScale = busyoObjSize [i];
				busyoObj.transform.localPosition = sourceBuilding.transform.localPosition;

				outBusyoObjList.Add (busyoObj);
				busyoObjList.Remove (busyoObj);
				busyoObjSize.RemoveAt (i);
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

			ShiroSearch script = GetComponent<ShiroSearch> ();
			List<GameObject> busyoObjList = script.busyoObjList;
			if (busyoObjList.Count != 0) {
				List<Vector2> busyoObjSize = script.busyoObjSize;
				for (int i = 0; i < busyoObjList.Count; i++) {
					GameObject busyoObj = busyoObjList [i];
					if (busyoObj != null) {
						if (busyoObj.GetComponent<Homing> ()) {
							busyoObj.GetComponent<Homing> ().enabled = true;
							busyoObj.GetComponent<Homing> ().AIType = 2; //Chase
						} else if (busyoObj.GetComponent<HomingLong> ()) {
							busyoObj.GetComponent<HomingLong> ().enabled = true;
							busyoObj.GetComponent<HomingLong> ().AIType = 2; //Chase
						}

						busyoObj.transform.localScale = busyoObjSize [i];
					}
				}
			}


			List<GameObject> outBusyoObjList = script.outBusyoObjList;
			if (outBusyoObjList.Count != 0) {
				for (int i = 0; i < outBusyoObjList.Count; i++) {
					GameObject outBusyoObj = outBusyoObjList [i];
					if (outBusyoObj != null) {
						if (outBusyoObj.GetComponent<Homing> ()) {
							outBusyoObj.GetComponent<Homing> ().backShiroFlg = false;
							outBusyoObj.GetComponent<Homing> ().backShiroObj = null;
							outBusyoObj.GetComponent<Homing> ().AIType = 2;	//Chase

						} else if (outBusyoObj.GetComponent<HomingLong> ()) {
							outBusyoObj.GetComponent<HomingLong> ().backShiroFlg = false;
							outBusyoObj.GetComponent<HomingLong> ().backShiroObj = null;
							outBusyoObj.GetComponent<HomingLong> ().AIType = 2;	//Chase
						}
					}
				}
			}

			//SE
			AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
			audioSources [6].Play ();

			//Animation
			string animPath = "Prefabs/Kassen/destroyShiro";
			GameObject destroyObj = Instantiate (Resources.Load (animPath)) as GameObject;		
			destroyObj.transform.localScale = new Vector2 (15,8);
			destroyObj.transform.localPosition = gameObject.transform.localPosition;

			Destroy (gameObject);

			if (name == "shiro") {
				foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Enemy")){
					Destroy (obs);
				}
			}

		} else {
			
			//stop animation &SE
			if (attackObj != null) {
				if (attackObj.GetComponent<PlayerAttack> ()) {
					attackObj.GetComponent<PlayerAttack> ().anim.SetBool ("IsAttack", false);
				}
                AudioSource[] audioSources = attackObj.GetComponents<AudioSource>();
                if (audioSources.Length > 0) {                    
				    audioSources [1].Stop ();
                }
            }

			//Delete Effect
			foreach (Transform child in transform) {
				if (child.tag == "Senpou") {
					Destroy (child.gameObject);
				}
			}

			//child 
			string heisyu = gameObject.GetComponent<Heisyu> ().heisyu;
			foreach (Transform chObj in gameObject.transform) {
				if (chObj.tag == "EnemyChild") {
					//add component
					chObj.name = "hukuhei";
					chObj.gameObject.AddComponent<EnemyHP> ();
					chObj.gameObject.GetComponent<EnemyHP> ().life = childHPTmp;
					chObj.tag = gameObject.tag;
                    chObj.gameObject.layer = LayerMask.NameToLayer("Enemy");
                    chObj.gameObject.AddComponent<PolygonCollider2D> ();
					chObj.gameObject.AddComponent<Rigidbody2D> ();
					chObj.gameObject.GetComponent<Rigidbody2D> ().gravityScale = 0;
					chObj.gameObject.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation;
					chObj.gameObject.AddComponent<Heisyu> ();
					chObj.gameObject.GetComponent<Heisyu> ().heisyu = heisyu;
					chObj.gameObject.AddComponent<Escape> ();

					chObj.parent = null;
				}
				if (chObj.tag == "EnemyHata") {
					chObj.parent = null;				
				}
			}

			Destroy (gameObject);
		}
	}


}
