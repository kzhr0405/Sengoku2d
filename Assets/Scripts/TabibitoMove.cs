using UnityEngine;
using System.Collections;

public class TabibitoMove : MonoBehaviour {

	public float aim;
	public float speed = 0.5f;
	public GameObject destPoint; 

	void Start (){
        if (Application.loadedLevelName == "tutorialNaisei") {
            speed = 5;
        }

        Vector2 v;
		aim = GetAim(transform.position, destPoint.transform.position);

		v.x = Mathf.Cos (Mathf.Deg2Rad * aim) * speed;
		v.y = Mathf.Sin (Mathf.Deg2Rad * aim) * speed;
		GetComponent<Rigidbody2D> ().velocity = v;
		
	}

	public float GetAim(Vector2 p1, Vector2 p2) {
		float dx = p2.x - p1.x;
		float dy = p2.y - p1.y;
		
		float rad = Mathf.Atan2(dy, dx);
		return rad * Mathf.Rad2Deg;
	}
}
