﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class ShowStageDtl : MonoBehaviour {

	public bool clearedFlg = false;
	public bool kousakuFlg = false;
	public int kuniId = 0; 
	public int stageId = 0;
	public string stageName = "null";
	public string showExp = "";
	public int exp = 0;
	public string showMoney = "";
	public int money = 0;
	public string itemGrp = "";
	public string itemTyp = "";
	public string itemId = "";
	public int itemQty = 0;

	//Stage Value
	public int linkNo = 0;
	public int powerType = 0;
    public bool lastOneFlg;
    private Text labelDataStage;
	private Text labelDataExp;
	private Text labelDataMoney;

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		//Delete Previous Flame
		Destroy (GameObject.FindGameObjectWithTag ("BoardFire"));

		//Show Fire
		string pathFlame = "Prefabs/Map/flame";
		GameObject flame = Instantiate (Resources.Load (pathFlame)) as GameObject;
		flame.transform.parent = GameObject.FindGameObjectWithTag ("KuniMap").transform;
		flame.transform.localScale = new Vector2 (30, 30);

		//position is according to touch position
		flame.transform.localPosition = new Vector3 (transform.localPosition.x - 1, transform.localPosition.y + 2, 0);


		//Show Detail on Bottom
		labelDataStage = GameObject.Find ("Data_stage").GetComponent<Text> ();
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            labelDataStage.text = stageName + "\n" + "Castle";
        }else if (langId == 3) {
            labelDataStage.text = stageName + "之战";
        }else {
            labelDataStage.text = stageName + "の戦い";
        }

		labelDataExp = GameObject.Find ("Data_Exp").GetComponent<Text> ();
		labelDataExp.text = showExp;

		labelDataMoney = GameObject.Find ("Data_Money").GetComponent<Text> ();
		labelDataMoney.text = showMoney;

        //last one check
        GameObject parent = transform.parent.gameObject;
        lastOneFlg = true;
        foreach (Transform chld in parent.transform) {
            if(chld.tag == "Stage") {
                if(chld.GetComponent<ShowStageDtl>().stageId != stageId) {
                    if(!chld.GetComponent<ShowStageDtl>().clearedFlg) {
                        lastOneFlg = false;
                    }
                }
            }
        }
        if (lastOneFlg) {
            powerType = 3;
        }
        GameObject bttlBttn = GameObject.Find ("BattleButton");
		bttlBttn.GetComponent<StartKassen> ().activeKuniId = kuniId;
		bttlBttn.GetComponent<StartKassen> ().activeStageId = stageId;
		bttlBttn.GetComponent<StartKassen> ().activeStageName = stageName;
		bttlBttn.GetComponent<StartKassen> ().activeStageExp = exp;
		bttlBttn.GetComponent<StartKassen> ().activeStageMoney = money;
		bttlBttn.GetComponent<StartKassen> ().activeItemGrp = itemGrp;
		bttlBttn.GetComponent<StartKassen> ().activeItemType = itemTyp;
		bttlBttn.GetComponent<StartKassen> ().activeItemId = int.Parse (itemId);
		bttlBttn.GetComponent<StartKassen> ().activeItemQty = itemQty;
		bttlBttn.GetComponent<StartKassen> ().linkNo = linkNo;
		bttlBttn.GetComponent<StartKassen> ().powerType = powerType;
        bttlBttn.GetComponent<StartKassen>().lastOneFlg = lastOneFlg;
    }


}
