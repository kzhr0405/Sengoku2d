using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowReward : MonoBehaviour {

	public void OnClick() {
        GameObject.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "UI";

        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[0].Play();

        GameObject panel = GameObject.Find("Panel").gameObject;
        string pathOfBack = "Prefabs/Common/TouchBackForOne";
        GameObject back = Instantiate(Resources.Load(pathOfBack)) as GameObject;
        back.transform.SetParent(panel.transform);
        back.transform.localScale = new Vector2(1, 1);
        back.transform.localPosition = new Vector2(0, 0);
        string pathOfBoard = "Prefabs/PvP/Reward";
        GameObject board = Instantiate(Resources.Load(pathOfBoard)) as GameObject;
        board.transform.SetParent(panel.transform);
        board.transform.localScale = new Vector2(0.7f, 0.7f);
        board.transform.localPosition = new Vector2(0, 0);

        back.GetComponent<CloseOneBoard>().deleteObj = board;
    }
}
