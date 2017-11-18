using UnityEngine;
using System.Collections;

public class SenpouBetray : MonoBehaviour {

	public string tag = "";
	public int senpouId = 0;
	public int heiQty = 0;

	void Start () {

		string searchTag = "";
		if (gameObject.layer == LayerMask.NameToLayer("PlayerActEffect")) {
			searchTag = "EnemyChild";
		} else {
			searchTag = "PlayerChild";
		}

		int counter = 0;
		foreach (GameObject obs in  GameObject.FindGameObjectsWithTag(searchTag)){

			if (searchTag == "EnemyChild") {
				betrayEnemy (obs);
			} else {
                betrayPlayer(obs);
            }


			//Counter
			counter = counter + 1;

			//Break
			if (counter == heiQty) {
				break;
			}
		}
	}

	public void betrayEnemy(GameObject obs){
		// Enemy to Player

		EnemyHP enemyHpScript = obs.transform.parent.GetComponent<EnemyHP> ();
		Heisyu enemyAtkDfcScript = obs.transform.parent.GetComponent<Heisyu> ();
		float child_hp = enemyHpScript.childHP;
		float child_atk = enemyAtkDfcScript.atk;
		float child_dfc = enemyAtkDfcScript.dfc;
		string heisyu = enemyAtkDfcScript.heisyu;
		Vector2 child_location = obs.transform.position;

		string busyoId = obs.transform.parent.name;
		float child_spd = 0;
		if (obs.transform.parent.GetComponent<Homing> ()) {
			child_spd = obs.transform.parent.GetComponent<Homing> ().speed;
		} else {
			child_spd = obs.transform.parent.GetComponent<HomingLong> ().speed;			
		}

		//Reduce Qty & Status
		enemyHpScript.childQty = enemyHpScript.childQty - 1;
		if (enemyAtkDfcScript.heisyu == "YR" || enemyAtkDfcScript.heisyu == "KB" || enemyAtkDfcScript.heisyu == "SHP") {
			obs.transform.parent.GetComponent<EnemyAttack> ().attack = obs.transform.parent.GetComponent<EnemyAttack> ().attack - child_atk;
			obs.transform.parent.GetComponent<EnemyHP> ().dfc = obs.transform.parent.GetComponent<EnemyHP> ().dfc - child_dfc;

		} else {
			//obs.transform.parent.GetComponent<AttackLong> ().childAttack = obs.transform.parent.GetComponent<AttackLong> ().childAttack - child_atk;
			obs.transform.parent.GetComponent<EnemyHP> ().dfc = obs.transform.parent.GetComponent<EnemyHP> ().dfc - child_dfc;
		}

		//Delete
		Destroy (obs.gameObject);

        //Create
        GameObject ch_prefab = null;
        GameObject sashimono = null;
        if (heisyu=="SHP") {
            string ch_path = "Prefabs/Kaisen/3";
            ch_prefab = Instantiate(Resources.Load(ch_path)) as GameObject;
            ch_prefab.name = "hukuhei";
            ch_prefab.AddComponent<Homing>();
            Destroy(ch_prefab.GetComponent<UnitMover>());
            Destroy(ch_prefab.GetComponent<SenpouController>());
            Destroy(ch_prefab.GetComponent<Kunkou>());

        }else {
            string ch_path = "Prefabs/Player/hukuhei" + heisyu;
		    ch_prefab = Instantiate (Resources.Load (ch_path)) as GameObject;
		    ch_prefab.name = "hukuhei";
		    string sashimono_path = "Prefabs/Sashimono/" + busyoId;
		    sashimono = Instantiate (Resources.Load (sashimono_path)) as GameObject;
		    sashimono.transform.parent = ch_prefab.transform;
		    sashimono.transform.localScale = new Vector2 (0.3f, 0.3f);
        }

        if (heisyu == "YR") {
			sashimono.transform.localPosition = new Vector2 (-1, 0.6f);
			ch_prefab.transform.position = child_location;
		} else if (heisyu == "KB") {
			sashimono.transform.localPosition = new Vector2 (-0.5f, 1);
			ch_prefab.transform.position = child_location;
		} else if (heisyu == "TP") {
			sashimono.transform.localPosition = new Vector2 (-0.8f, 0.5f);
			ch_prefab.transform.position = child_location;
		} else if (heisyu == "YM") {
			sashimono.transform.localPosition = new Vector2 (-0.8f, 0.5f);
			ch_prefab.transform.position = child_location;
		} else if(heisyu=="SHP") {
            ch_prefab.transform.position = child_location;
        }

		ch_prefab.GetComponent<PlayerHP> ().life = child_hp;
		if (ch_prefab.GetComponent<PlayerAttack> ()) {
			ch_prefab.GetComponent<PlayerAttack> ().attack = child_atk;
		} else {
			ch_prefab.GetComponent<AttackLong> ().attack = child_atk;
		}
		ch_prefab.GetComponent<PlayerHP> ().dfc = child_dfc;

		if (ch_prefab.GetComponent<Homing> () != null) {
			ch_prefab.GetComponent<Homing> ().speed = child_spd;
		} else if (ch_prefab.GetComponent<HomingLong> () != null) {
			ch_prefab.GetComponent<HomingLong> ().speed = child_spd;
		}

		//SE
		AudioController audio = new AudioController();
		audio.addComponentMoveAttack (ch_prefab,heisyu);


		//Effect
		string effect_path = "Prefabs/Effect/betray";
		GameObject effect = Instantiate (Resources.Load (effect_path)) as GameObject;
		effect.transform.SetParent (ch_prefab.transform);
		effect.transform.localScale = new Vector2 (1, 1);
		effect.transform.localPosition = new Vector2 (0, 0);

	}

    public void betrayPlayer(GameObject obs) {
        PlayerHP playerHpScript = obs.transform.parent.GetComponent<PlayerHP>();
        Heisyu playerAtkDfcScript = obs.transform.parent.GetComponent<Heisyu>();
        float child_hp = playerHpScript.childHP;
        float child_atk = playerAtkDfcScript.atk;
        float child_dfc = playerAtkDfcScript.dfc;
        string heisyu = playerAtkDfcScript.heisyu;
        Vector2 child_location = obs.transform.position;

        string busyoId = obs.transform.parent.name;
        float child_spd = 0;
        if (obs.transform.parent.GetComponent<UnitMover>()) {
            child_spd = obs.transform.parent.GetComponent<UnitMover>().speed;
        }else {
            if (obs.transform.parent.GetComponent<Homing>()) {
                child_spd = obs.transform.parent.GetComponent<Homing>().speed;
            }else if(obs.transform.parent.GetComponent<HomingLong>()) {
                child_spd = obs.transform.parent.GetComponent<HomingLong>().speed;
            }
        }

        //Reduce Qty & Status
        playerHpScript.childQty = playerHpScript.childQty - 1;
        if (playerAtkDfcScript.heisyu == "YR" || playerAtkDfcScript.heisyu == "KB" || playerAtkDfcScript.heisyu == "SHP") {
            obs.transform.parent.GetComponent<PlayerAttack>().attack = obs.transform.parent.GetComponent<PlayerAttack>().attack - child_atk;
            obs.transform.parent.GetComponent<PlayerHP>().dfc = obs.transform.parent.GetComponent<PlayerHP>().dfc - child_dfc;

        }else {
            //obs.transform.parent.GetComponent<AttackLong>().childAttack = obs.transform.parent.GetComponent<AttackLong>().childAttack - child_atk;
            obs.transform.parent.GetComponent<PlayerHP>().dfc = obs.transform.parent.GetComponent<PlayerHP>().dfc - child_dfc;
        }

        //Delete
        Destroy(obs.gameObject);

        //Create
        GameObject ch_prefab = null;
        GameObject sashimono = null;
        if (heisyu == "SHP") {
            string ch_path = "Prefabs/Kaisen/3";
            ch_prefab = Instantiate(Resources.Load(ch_path)) as GameObject;
            ch_prefab.name = "hukuhei";
            ch_prefab.AddComponent<Homing>();
            Destroy(ch_prefab.GetComponent<UnitMover>());
            Destroy(ch_prefab.GetComponent<SenpouController>());
            Destroy(ch_prefab.GetComponent<Kunkou>());
        } else {
            string ch_path = "Prefabs/Player/hukuhei" + heisyu;
            ch_prefab = Instantiate(Resources.Load(ch_path)) as GameObject;
            ch_prefab.name = "hukuhei";
            string sashimono_path = "Prefabs/Sashimono/" + busyoId;
            sashimono = Instantiate(Resources.Load(sashimono_path)) as GameObject;
            sashimono.transform.parent = ch_prefab.transform;
            sashimono.transform.localScale = new Vector2(0.3f, 0.3f);
        }

        if (heisyu == "YR") {
            sashimono.transform.localPosition = new Vector2(-1, 0.6f);
            ch_prefab.transform.position = child_location;

        }else if (heisyu == "KB") {
            sashimono.transform.localPosition = new Vector2(-0.5f, 1);
            ch_prefab.transform.position = child_location;

        }else if (heisyu == "TP") {
            sashimono.transform.localPosition = new Vector2(-0.8f, 0.5f);
            ch_prefab.transform.position = child_location;

        }else if (heisyu == "YM") {
            sashimono.transform.localPosition = new Vector2(-0.8f, 0.5f);
            ch_prefab.transform.position = child_location;
        }else if (heisyu == "SHP") {
            ch_prefab.transform.position = child_location;
        }

        //Change Component
        ch_prefab.AddComponent<EnemyHP>();
        if (playerAtkDfcScript.heisyu == "YR" || playerAtkDfcScript.heisyu == "KB" || playerAtkDfcScript.heisyu == "SHP") {
            ch_prefab.AddComponent<EnemyAttack>();
        }else {
            if(playerAtkDfcScript.heisyu == "TP") {
                ch_prefab.GetComponent<AttackLong>().bullet = Resources.Load("Prefabs/Enemy/EnemyBullet") as GameObject;
            }else if(playerAtkDfcScript.heisyu == "YM") {
                ch_prefab.GetComponent<AttackLong>().bullet = Resources.Load("Prefabs/Enemy/EnemyArrow") as GameObject;
            }
        }
        Destroy(ch_prefab.GetComponent<PlayerHP>());
        ch_prefab.tag = "Enemy";
        ch_prefab.layer = LayerMask.NameToLayer("EnemyNoColl");


        //Set Param
        ch_prefab.GetComponent<EnemyHP>().life = child_hp;
        if (ch_prefab.GetComponent<EnemyAttack>()) {
            ch_prefab.GetComponent<EnemyAttack>().attack = child_atk;
        }else {
            ch_prefab.GetComponent<AttackLong>().attack = child_atk;
        }
        ch_prefab.GetComponent<EnemyHP>().dfc = child_dfc;

        if (ch_prefab.GetComponent<Homing>() != null) {
            ch_prefab.GetComponent<Homing>().speed = child_spd;
        }else if (ch_prefab.GetComponent<HomingLong>() != null) {
            ch_prefab.GetComponent<HomingLong>().speed = child_spd;
        }

        //Effect
        string effect_path = "Prefabs/Effect/betray";
        GameObject effect = Instantiate(Resources.Load(effect_path)) as GameObject;
        effect.transform.SetParent(ch_prefab.transform);
        effect.transform.localScale = new Vector2(1, 1);
        effect.transform.localPosition = new Vector2(0, 0);

    }
}
