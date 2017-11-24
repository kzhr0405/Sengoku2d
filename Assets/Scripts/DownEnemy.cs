using UnityEngine;
using System.Collections;

public class DownEnemy : MonoBehaviour {

	public static GameObject doSenpou(GameObject obj,  int senpouId, string senpouName, int senpouStatus, int senpouLv){

		string path = "Prefabs/Effect/" + senpouId;
		GameObject effect = Instantiate(Resources.Load (path)) as GameObject;
		Vector2 effectOriginalScale = effect.transform.localScale;
		Vector2 effectOriginalPosition = effect.transform.localPosition;

		effect.transform.SetParent(obj.transform);
		effect.transform.localPosition = effectOriginalPosition;
		effect.transform.localScale = effectOriginalScale;

		if(obj.tag == "Player"){
			effect.layer = LayerMask.NameToLayer("PlayerActEffect");
		}else if(obj.tag == "Enemy"){
			effect.layer = LayerMask.NameToLayer("EnemyActEffect");
		}


		//1 Target or All Target
		if (senpouId == 1) {
			//All
			effect.GetComponent<SenpouKeepAttack> ().senpouId = senpouId;
			effect.GetComponent<SenpouKeepAttack> ().attack = senpouStatus;
			effect.GetComponent<SenpouKeepAttack> ().senpouName = senpouName;
		} else {
			//1 time
			if (effect.GetComponent<SenpouAttack> ()) {
				effect.GetComponent<SenpouAttack> ().senpouId = senpouId;
				effect.GetComponent<SenpouAttack> ().attack = senpouStatus;
				effect.GetComponent<SenpouAttack> ().senpouName = senpouName;
			}else if(effect.GetComponent<SenpouBetray> ()){
				effect.GetComponent<SenpouBetray> ().senpouId = senpouId;
				effect.GetComponent<SenpouBetray> ().heiQty = senpouStatus;
			}
		}

		return effect;
	}

}
