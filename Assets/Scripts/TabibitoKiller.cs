using UnityEngine;
using System.Collections;

public class TabibitoKiller : MonoBehaviour {

	public GameObject OppositObj;

	private void OnTriggerEnter2D(Collider2D col){
		if (col.GetComponent<TabibitoMove> ().destPoint.name == name) {
			Destroy (col.gameObject);
		}
	}
}
