using UnityEngine;
using System.Collections;

public class Escape : MonoBehaviour {

	public GameObject targetWall; 
	public bool leftFlg = false; //左を向いているか

	// Use this for initialization
	void Start () {
		
		int rdmId = UnityEngine.Random.Range(0,4);
		if (rdmId == 0) {
			targetWall = GameObject.Find ("wall_upper");
		} else if (rdmId == 1){
			targetWall = GameObject.Find ("wall_lower");
		} else if (rdmId == 2){
			targetWall = GameObject.Find ("wall_right");
		} else if (rdmId == 3){
			targetWall = GameObject.Find ("wall_left");
		}

		float x = transform.localScale.x;
		if (x < 0) {
			leftFlg = true;
		}


		if (targetWall.transform.position.x < transform.position.x) {
			if(!leftFlg){
				Vector2 targetScale = transform.localScale;
				targetScale.x *= -1;
				transform.localScale = targetScale;
				leftFlg = true;
			}

		} else {
			if(leftFlg){
				Vector2 targetScale = transform.localScale;
				targetScale.x *= -1;
				transform.localScale = targetScale;
				leftFlg = false;
			}
		}



	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody2D>().velocity = (targetWall.transform.position - transform.position).normalized * 1;

	}

	void OnCollisionEnter2D(Collision2D col){
		//if (col.gameObject.name == targetWall.name) {
			Destroy (gameObject);
		//}
	}
}