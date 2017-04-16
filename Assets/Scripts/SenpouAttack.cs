using UnityEngine;
using System.Collections;

public class SenpouAttack : MonoBehaviour {

    public bool senpouDone = false;
	public int senpouId = 0;
	private float timeleft;
	public int attack; 
	public string senpouName;
	private string targetHPScript;
	public Color downColor = new Color (200f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //Red down

	// Use this for initialization
	void Start () {
		if(gameObject.layer == LayerMask.NameToLayer("PlayerActEffect")){
			targetHPScript = "EnemyHP";
		}else if(gameObject.layer == LayerMask.NameToLayer("EnemyActEffect")){
			targetHPScript = "PlayerHP";
		}
	}

	private void OnTriggerEnter2D (Collider2D col){

        if(!senpouDone) {
		    timeleft -= Time.deltaTime;
		    if (timeleft <= 0.0) {
			    timeleft = 1.0f;
                senpouDone = true;

			    if (senpouId == 5 || senpouId == 6 || senpouId == 7) {
				    //Totsugeki
				    if (col.GetComponent<Heisyu> ().heisyu == "TP"||col.GetComponent<Heisyu> ().heisyu == "YM") {
					    attack = attack * 2;
				    }
				    if (col.name != "hukuhei" && col.GetComponent<Heisyu>().heisyu != "saku") {
					    downDfc (col.gameObject, 10);
				    }
			    }else if (senpouId == 2 || senpouId == 3 || senpouId == 4) {
				    //Yaribusuma
				    if (col.GetComponent<Heisyu> ().heisyu == "KB") {
					    attack = attack * 2;
				    }
				    if (col.name != "hukuhei" && col.GetComponent<Heisyu>().heisyu != "saku") {
					    downSpd (col.gameObject);
				    }
			    }else if (senpouId == 16 || senpouId == 17) {
                    //Hourokudama
                    downChild(col.gameObject, attack);
                }


                    col.gameObject.SendMessage ("DirectDamage", attack);

		    }
        }
	}
    public void downChild(GameObject col, int childQty) {
        if (targetHPScript == "PlayerHP") {
            int chldQty = col.GetComponent<PlayerHP>().childQty;
            int count = 0;
            if(chldQty!=0) {
                foreach(Transform chldObj in col.transform) {
                    if(count < childQty) {
                        if(chldObj.tag == "PlayerChild") {
                            Destroy(chldObj.gameObject);
                            count = count + 1;
                            col.GetComponent<PlayerHP>().childQty = col.GetComponent<PlayerHP>().childQty - 1;
                        }
                    }else {
                        break;
                    }
                }
            }

        }else {
            int chldQty = col.GetComponent<EnemyHP>().childQty;
            int count = 0;
            if (chldQty != 0) {
                foreach (Transform chldObj in col.transform) {
                    if (count < childQty) {
                        if (chldObj.tag == "EnemyChild") {
                            Destroy(chldObj.gameObject);
                            count = count + 1;
                            col.GetComponent<EnemyHP>().childQty = col.GetComponent<EnemyHP>().childQty - 1;
                        }
                    }else {
                        break;
                    }
                }
            }
        }
    }



    public void downDfc(GameObject col, int percent){

		//reduce
		GameObject dtl = new GameObject();
		float baseDfc;
		float rdcDfc;

		if (targetHPScript == "PlayerHP") {
			dtl = col.transform.FindChild ("BusyoDtlPlayer").gameObject;
			baseDfc = col.GetComponent<PlayerHP> ().dfc;
			float temp = baseDfc * percent;
			rdcDfc = temp / 100;
			float newDfc = Mathf.Ceil (baseDfc - rdcDfc);
			col.GetComponent<PlayerHP> ().dfc = newDfc;

		} else {
			dtl = col.transform.FindChild ("BusyoDtlEnemy").gameObject;
			baseDfc = col.GetComponent<EnemyHP> ().dfc;
			float temp = baseDfc * percent;
			rdcDfc = temp / 100;
			float newDfc = Mathf.Ceil (baseDfc - rdcDfc);
			col.GetComponent<EnemyHP> ().dfc = newDfc;
		}

		GameObject effect = dtl.transform.FindChild ("dfc_up").gameObject;
		effect.GetComponent<FadeoutOff> ().currentRemainTime = 5;
		effect.GetComponent<SpriteRenderer> ().enabled = true;
		effect.GetComponent<Animator> ().enabled = true;
		effect.GetComponent<FadeoutOff> ().enabled = true;

		GameObject value = dtl.transform.FindChild ("MinHpBar").transform.FindChild ("Value").gameObject;
		value.GetComponent<MeshRenderer> ().enabled = true;
		value.GetComponent<TextMeshFadeoutOff> ().enabled = true;
		value.GetComponent<TextMesh> ().text = (Mathf.Ceil(rdcDfc)).ToString () + "⇣";
		value.GetComponent<TextMesh> ().color = downColor;
	
	}

	public void downSpd(GameObject col){

		//reduce
		GameObject dtl = new GameObject();
		float baseSpd;
		bool downFlg = false;

		if (targetHPScript == "PlayerHP") {
			dtl = col.transform.FindChild ("BusyoDtlPlayer").gameObject;
			if (col.GetComponent<UnitMover> ()) {
				baseSpd = col.GetComponent<UnitMover> ().speed;
				if (baseSpd > 1) {
					baseSpd = baseSpd - 1;
					col.GetComponent<UnitMover> ().speed = baseSpd;
					downFlg = true;
				}
			}else{
                if (col.GetComponent<Homing>()) {
                    baseSpd = col.GetComponent<Homing>().speed;
                    if (baseSpd > 1) {
                        baseSpd = baseSpd - 1;
                        col.GetComponent<Homing>().speed = baseSpd;
                        downFlg = true;
                    }
                }else if(col.GetComponent<HomingLong>()) {
                    baseSpd = col.GetComponent<HomingLong>().speed;
                    if (baseSpd > 1) {
                        baseSpd = baseSpd - 1;
                        col.GetComponent<HomingLong>().speed = baseSpd;
                        downFlg = true;
                    }
                }
            }

		} else {
			dtl = col.transform.FindChild ("BusyoDtlEnemy").gameObject;
			if (col.GetComponent<Homing> ()) {
				baseSpd = col.GetComponent<Homing> ().speed;
				if (baseSpd > 1) {
					baseSpd = baseSpd - 1;
					col.GetComponent<Homing> ().speed = baseSpd;
					downFlg = true;
				}
			} else {
				baseSpd = col.GetComponent<HomingLong> ().speed;
				if (baseSpd > 1) {
					baseSpd = baseSpd - 1;
					col.GetComponent<HomingLong> ().speed = baseSpd;
					downFlg = true;
				}
			}
		}

		if (downFlg) {
			GameObject effect = dtl.transform.FindChild ("spd_up").gameObject;
			effect.GetComponent<FadeoutOff> ().currentRemainTime = 5;
			effect.GetComponent<SpriteRenderer> ().enabled = true;
			effect.GetComponent<Animator> ().enabled = true;
			effect.GetComponent<FadeoutOff> ().enabled = true;

			GameObject value = dtl.transform.FindChild ("MinHpBar").transform.FindChild ("Value").gameObject;
			value.GetComponent<MeshRenderer> ().enabled = true;
			value.GetComponent<TextMeshFadeoutOff> ().enabled = true;
			value.GetComponent<TextMesh> ().text = "1⇣";
			value.GetComponent<TextMesh> ().color = downColor;
		}
	}



}
