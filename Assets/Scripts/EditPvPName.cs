using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class EditPvPName : MonoBehaviour {

    //public GameObject lightning;
    //public GameObject vs;

    void Start() {
        //lightning = GameObject.Find("lightning").gameObject;
        //vs = GameObject.Find("Vs").gameObject;
    }



    public void OnClick() {

        //lightning.SetActive(false);
        //vs.SetActive(false);
        GameObject.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "UI";

        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[0].Play();

        GameObject panel = GameObject.Find("Panel").gameObject;
        string pathOfBack = "Prefabs/Common/TouchBackForOne";
        GameObject back = Instantiate(Resources.Load(pathOfBack)) as GameObject;
        back.transform.SetParent(panel.transform);
        back.transform.localScale = new Vector2(1, 1);
        back.transform.localPosition = new Vector2(0, 0);
        string pathOfBoard = "Prefabs/PvP/FirstPvPBoard";
        GameObject board = Instantiate(Resources.Load(pathOfBoard)) as GameObject;
        board.transform.SetParent(panel.transform);
        board.transform.localScale = new Vector2(1, 0.9f);
        board.transform.localPosition = new Vector2(0, 0);

        back.GetComponent<CloseOneBoard>().deleteObj = board;
        board.transform.Find("NoButton").GetComponent<AddHyourou>().touchBackObj = back;

        //Adjust for 2nd Time
        board.transform.Find("YesButton").GetComponent<StartPvP>().secondTimeFlg = true;
        board.transform.Find("YesButton").GetComponent<StartPvP>().touchBackObj = back;
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            board.transform.Find("YesButton").transform.Find("Text").GetComponent<Text>().text = "Edit";
        }else if (langId == 3) {
            board.transform.Find("YesButton").transform.Find("Text").GetComponent<Text>().text = "变更";
        }
        else {
            board.transform.Find("YesButton").transform.Find("Text").GetComponent<Text>().text = "変更";
        }



    }
}
