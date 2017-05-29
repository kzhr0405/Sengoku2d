using UnityEngine;
using System.Collections;

public class CloseBoard : MonoBehaviour {

	public int layer = 0;
	public int kuniId = 0;
	public int daimyoId = 0;
	public int daimyoBusyoId = 0;
	public string daimyoBusyoName = "";
	public int daimyoBusyoAtk = 0;
	public int daimyoBusyoDfc = 0;
	public string title = "";
	public bool doumeiFlg = false;
	public int kuniQty = 0;
	public int yukoudo = 0;
	public string naiseiItem = "";
	public bool cyouhouFlg = false;
	public int cyouhouSnbRankId = 0;
    public bool tabibitoNoticeBtnFlg = false;

	public void onClick () {

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [1].Play ();

        if(tabibitoNoticeBtnFlg) {
            TabibitoNoticeBtn TabibitoNoticeBtn = new TabibitoNoticeBtn();
            TabibitoNoticeBtn.onOffTabibitoMove(false);
            GameObject.Find("NaiseiController").GetComponent<NaiseiController>().stopFlg = false;
        }


		GameObject[] objects = GameObject.FindGameObjectsWithTag("Board");
		//配列内のオブジェクトの数だけループ
		foreach (GameObject obj in objects) {
			//オブジェクトを削除
			Destroy(obj);
		}
		GameObject boardBottom = GameObject.FindGameObjectWithTag("BoardBottm");
		Destroy (boardBottom);
		GameObject boardFire = GameObject.FindGameObjectWithTag("BoardFire");
		Destroy (boardFire);
       
    }
}
