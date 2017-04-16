using UnityEngine;
using System.Collections;

public class SenpouKeepAttack : MonoBehaviour {

	public int senpouId = 0;
	private float timeleft;
	public int attack; 
	public string senpouName;
	private string targetHPScript;

	// Use this for initialization
	void Start () {
		if(gameObject.layer == LayerMask.NameToLayer("PlayerActEffect")){
			targetHPScript = "EnemyHP";
		}else if(gameObject.layer == LayerMask.NameToLayer("EnemyActEffect")){
			targetHPScript = "PlayerHP";
		}
	}
	
	private void OnTriggerStay2D (Collider2D col){
		timeleft -= Time.deltaTime;
		if (timeleft <= 0.0) {
			timeleft = 1.0f;

			if (targetHPScript == "PlayerHP") {
				int Damage = (attack);
				col.gameObject.SendMessage ("Damage", Damage);
			} else if (targetHPScript == "EnemyHP") {
				int Damage = (attack);
				col.gameObject.SendMessage ("Damage", Damage);
			}
		}
	}
}
