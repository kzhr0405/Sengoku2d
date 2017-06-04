using UnityEngine;
using System.Collections;

public class CloseLayer : MonoBehaviour {

	public string syoukaijyoRank = "";
	public GameObject closeTargetObj;
	public GameObject closeTargetBack;

    public bool syouninCyouteiFlg = false;

	//Pre Calc Value
	public int yukouAddValue;
	public int yukouReducePoint;
	public int stopBattleRatio;
	public int stopBattleReducePoint;
	public int kanniId;
	public string kanniName = "";
	public int kanniRatio;
	public int kanniReducePoint;
	public bool kanniDoneFlg;
	public bool occupiedFlg = false;
	public int cyoutekiDaimyo = 0;
	public string cyoutekiDaimyoName = "";
	public int cyoutekiReducePoint = 0;

	//Syounin
	public string kahouCdString = ""; //bugu,gusoku...
	public string kahouIdString = ""; //1,2,5
	public string busshiQtyString = ""; //Qty of busshi
	public string busshiRankString = ""; //Rank of busshi
	public int rdmKengouId = 0;
	public int yasenAmt = 0;
	public int techId = 0;
	public float discount = 0;
	

	// Use this for initialization
	public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [1].Play ();

		Destroy (closeTargetObj);
		Destroy (closeTargetBack);

        if (syouninCyouteiFlg) {
            if (GameObject.Find("GameController")) {
                GameObject.Find("GameController").GetComponent<MainStageController>().eventStopFlg = false;
                CyouteiPop cyouteiPop = new CyouteiPop();
                cyouteiPop.startGunzei();
            }
        }
	}
}
