using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamagePop : MonoBehaviour {

	Text text;
    public float divSpeed = 1;
    public bool attackBoardflg = false;

	void Start () {
		//delete
		Destroy(gameObject, 1);
		text = gameObject.GetComponent<Text>();
	}

	float time = 0;
	void Update () {
        if(attackBoardflg) {
		    transform.Translate(Vector3.up * time);
		    time += Time.deltaTime;
            time = time/ divSpeed;
        }else {
            transform.Translate(Vector3.up * Time.deltaTime);
            time += Time.deltaTime;
        }
        //Clear
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1.5F - time);
	}
}
