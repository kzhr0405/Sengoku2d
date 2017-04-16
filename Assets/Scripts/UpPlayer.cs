using UnityEngine;
using System.Collections;

public class UpPlayer : MonoBehaviour {


	public GameObject doSenpou(GameObject obj,  int senpouId, int senpouStatus, float initCoolTime, float initDisTarget){

		string path = "Prefabs/Effect/" + senpouId;
		GameObject effect = Instantiate(Resources.Load (path)) as GameObject;
		Vector2 effectOriginalScale = effect.transform.localScale;
		effect.transform.SetParent(obj.transform);
		effect.transform.localPosition = new Vector2 (0, -1);
		effect.transform.localScale = effectOriginalScale;
        effect.GetComponent<SenpouStatusUp>().senpouId = senpouId;
        effect.GetComponent<SenpouStatusUp> ().targetObj = obj;
		effect.GetComponent<SenpouStatusUp> ().senpouStatus = senpouStatus;
		effect.GetComponent<SenpouStatusUp> ().initCoolTime = initCoolTime;
		effect.GetComponent<SenpouStatusUp> ().initDisTarget = initDisTarget;

		return effect;
	}



}
