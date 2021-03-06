﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class ReplaceSpriteNameRank : MonoBehaviour {

	void Start () {
        int langId = PlayerPrefs.GetInt("langId");
        BusyoInfoGet busyoInfoScript = new BusyoInfoGet ();
		string busyoName = busyoInfoScript.getName (int.Parse(name), langId);
		string busyoRank = busyoInfoScript.getRank (int.Parse(name));

		transform.Find ("Text").GetComponent<Text> ().text = busyoName;
		transform.Find ("Rank").GetComponent<Text> ().text = busyoRank;

        if (Application.loadedLevelName != "preKaisen") {
                string imagePath = "Prefabs/Player/Sprite/unit" + name;
		    GetComponent<Image> ().sprite = 
			    Resources.Load (imagePath, typeof(Sprite)) as Sprite;
        }else {

            transform.Find("Text").GetComponent<Text>().fontSize = 70;
            Color white = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f); //white
            transform.Find("Text").GetComponent<Text>().color = white;
        }
		
	}
}
