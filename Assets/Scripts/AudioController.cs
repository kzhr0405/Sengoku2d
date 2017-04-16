using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class AudioController : MonoBehaviour {

	public void addComponentMoveAttack(GameObject obj, string heisyu){

		//SE
		AudioSource moveSE = obj.AddComponent<AudioSource>();
		if (heisyu == "KB") {
			moveSE.clip = Resources.Load ("Prefabs/Sound/Move_KB") as AudioClip;
			moveSE.pitch = 1.5f;
			moveSE.loop = true;
		} else if(heisyu == "SHP") {
            moveSE.clip = Resources.Load("Prefabs/Sound/Move_SHP") as AudioClip;
            moveSE.pitch = 1.0f;
            moveSE.volume = 0.3f;
            moveSE.loop = true;
        } else {
			moveSE.clip = Resources.Load ("Prefabs/Sound/Move_YRYMTP") as AudioClip;
			moveSE.pitch = 1.2f;
			moveSE.volume = 0.05f;
			moveSE.loop = true;
		}
		AudioSource attackSE = obj.AddComponent<AudioSource>();
		if (heisyu == "KB" || heisyu == "YR" || heisyu == "SHP") {
			attackSE.clip = Resources.Load ("Prefabs/Sound/Attack_KBYR") as AudioClip;
			attackSE.volume = 0.8f;
			attackSE.loop = true;
		} else if (heisyu == "TP") {
			attackSE.clip = Resources.Load ("Prefabs/Sound/Attack_TP") as AudioClip;
		} else if (heisyu == "YM") {
			attackSE.clip = Resources.Load ("Prefabs/Sound/Attack_YM") as AudioClip;
		}

		bool SEOffFlg = PlayerPrefs.GetBool ("SEOffFlg");
		if (SEOffFlg) {
			moveSE.volume = 0;
			attackSE.volume = 0;

		}

		
	}
}
