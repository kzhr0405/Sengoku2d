using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StaminaVideo : MonoBehaviour {

	public void OnClick() {
        if (SceneManager.GetActiveScene().name == "mainStage") {
            GameObject.Find("Panel").transform.FindChild("Video").GetComponent<AdfuriMoviepop>().PushAdsense();
        }
    }
}
