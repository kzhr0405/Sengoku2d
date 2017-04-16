using UnityEngine;
using System.Collections;

public class BusyoHPBar : MonoBehaviour {

	float life = 0;
	float nowSize = 0;
	public float initLife = 0;
	float initSize = 0;

	// Use this for initialization
	void Start () {
		initSize = this.transform.localScale.x;

	}
	
	// Update is called once per frame
	void Update () {
        GameObject busyoObj = gameObject.transform.parent.gameObject.transform.parent.gameObject;

        if (busyoObj.tag == "Player"){
			life = busyoObj.GetComponent<PlayerHP>().life;
		}else if(busyoObj.tag == "Enemy"){
			life = busyoObj.GetComponent<EnemyHP>().life;
		}

		nowSize = (initSize*life)/initLife;
		this.transform.localScale = new Vector3(nowSize, transform.localScale.y, transform.localScale.z);
	}
}
