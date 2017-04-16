using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CloseSimpleBattle : MonoBehaviour {

    public GameObject boardObj;
    public GameObject katanaBtnObj;
    public GameObject timer;

    public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[1].Play();

        Destroy(boardObj);
        if(GameObject.Find("enemyKatana")) {
            katanaBtnObj.GetComponent<Button>().enabled = true;
            katanaBtnObj = null;
        }

        timer.GetComponent<ShiroAttack>().rakujyoFlg = false;
    }
}
