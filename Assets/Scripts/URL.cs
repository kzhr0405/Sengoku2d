using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URL : MonoBehaviour {

	
	public void OnClick () {

        if (Application.systemLanguage != SystemLanguage.Japanese) {
            Application.OpenURL("http://samurai-wars-game.wikia.com/wiki/Samurai_Wars_Game_Wiki");
        }else {
            Application.OpenURL("http://samurai_wars.a-wiki.net/");
        }

    }
}
