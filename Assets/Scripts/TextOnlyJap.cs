using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class TextOnlyJap : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2 || langId == 3) {
            Destroy(gameObject);
        }
    }
}
