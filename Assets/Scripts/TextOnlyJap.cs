using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOnlyJap : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            Destroy(gameObject);
        }
    }
}
