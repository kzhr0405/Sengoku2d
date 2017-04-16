using UnityEngine;
using System.Collections;

public class DoDoumeiAttack : MonoBehaviour {

	public void OnClick () {
		if (name == "YesButton") {



		}else if(name == "NoButton"){
			//Back
			Destroy(GameObject.Find("DoumeiAttackConfirm"));
			Destroy(GameObject.Find("Back(Clone)"));
		}

		
	}
}