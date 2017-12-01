using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class TabibitoSpdUp : MonoBehaviour {

    public GameObject baseBtnObj;
    public GameObject touchBackObj;

    public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();

        if (name == "YesButton") {
            Message msg = new Message();

            //check
            int busyoDama = PlayerPrefs.GetInt("busyoDama");
            if (busyoDama >= 100) {
                if(GameObject.Find("SwithTown")) {
                    GameObject.Find("SwithTown").GetComponent<SwitchTown>().spdUpFlg = true;
                }
                GameObject.Find("Panel").transform.Find("Button").GetComponent<BackMain>().spdUpFlg = true;

                audioSources[3].Play();

                busyoDama = busyoDama - 100;
                PlayerPrefs.SetInt("busyoDama", busyoDama);
                PlayerPrefs.Flush();

                //Okuni
                string tabibitoPath = "Prefabs/Naisei/Tabibito/bunka1";
                GameObject prefab = Instantiate(Resources.Load(tabibitoPath)) as GameObject;
                prefab.transform.SetParent(GameObject.Find("Panel").transform);
                prefab.transform.localPosition = new Vector2(0,80);
                prefab.transform.localScale = new Vector2(1, 1);
                prefab.GetComponent<TabibitoMove>().enabled = false;

                NaiseiController script = GameObject.Find("NaiseiController").GetComponent<NaiseiController>();
                script.tabibitoSecMst = 0.5f;
                msg.makeMessage(msg.getMessage(141));

                Destroy(baseBtnObj);
            }else {
                audioSources[4].Play();
                msg.makeMessage(msg.getMessage(2));
            }
        }else {
            audioSources[1].Play();
        }

        touchBackObj.GetComponent<CloseOneBoard>().OnClick();
    }
}
