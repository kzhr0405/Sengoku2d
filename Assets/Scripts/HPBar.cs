using UnityEngine;
using System.Collections;

public class HPBar : MonoBehaviour {

	public string targetTag;
	public EnemyHP EnemyHpSclipt;
	public PlayerHP PlayerHpSclipt;
	float life = 0;
	float nowSize = 0;
	float initLife = 0;
	float initSize = 0;

	// Use this for initialization
	void Start () {
		if (this.tag == "EnemyBar") {
			targetTag = "Enemy";
			
		} else if (this.tag == "PlayerBar") {
			targetTag = "Player";
		}
		life = getTotalHp (targetTag);
		initLife = life;
		initSize = this.transform.localScale.x;
	}
	
	void Update () {
		life = getTotalHp (targetTag);
		nowSize = (initSize*life)/initLife;
		this.transform.localScale = new Vector3(nowSize, transform.localScale.y, transform.localScale.z);
	}

	public float getTotalHp(string targetTag){
		//初期化
		life = 0;

		//タグ指定されたオブジェクトを配列で取得する
		foreach (GameObject obs in  GameObject.FindGameObjectsWithTag(targetTag)){

			if(targetTag == "Enemy"){
				EnemyHpSclipt = obs.GetComponent<EnemyHP>();
				life =	life + EnemyHpSclipt.life;
			}else if(targetTag == "Player"){
				PlayerHpSclipt = obs.GetComponent<PlayerHP>();
				life =	life + PlayerHpSclipt.life;
			}
		}
		//最も近かったオブジェクトを返す
		return life;
	}

	public bool flag = true;
	void OnGUI(){
		if (this.tag == "PlayerBar") {
			if (flag) {
				GUI.Label (new Rect(80,0,100,100),"兵力"+ life);
			}
			
		} else if (this.tag == "EnemyBar") {
			if (flag) {
				GUI.Label (new Rect(400,0,100,100),"兵力"+ life);
			}
		}
	}
}
