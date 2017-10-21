using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class TextOnlyEng : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            Destroy(gameObject);
        }
    }
	
}
