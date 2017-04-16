using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloseEventBoard : MonoBehaviour {

	public GameObject deleteObj;
	public GameObject deleteObj2;
	public bool shisyaSceneFlg = false;


    //Event History for Pointer
    public bool activityUpdateFlg = false;
    public List<int> kassenDaimyoList = new List<int>();
    public List<int> upDaimyo1List = new List<int>();
    public List<int> upDaimyo2List = new List<int>();
    public List<int> downDaimyo1List = new List<int>();
    public List<int> downDaimyo2List = new List<int>();
    public List<int> metsubouKuniList = new List<int>();

    public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [1].Play ();

		if (shisyaSceneFlg) {
			Application.LoadLevel ("shisya");
		} else {
			Destroy (deleteObj);
			Destroy (deleteObj2);

            if(activityUpdateFlg) {
                MapPointer pointerScript = new MapPointer();
                if(kassenDaimyoList.Count != 0) {
                    foreach(int daimyoId in kassenDaimyoList) {
                        pointerScript.createKassenPointer(daimyoId);
                    }
                }
                if(upDaimyo1List.Count !=0) {
                    for (int i = 0; i < upDaimyo1List.Count; i++) {
                        int downDaimyo1 = upDaimyo1List[i];
                        int downDaimyo2 = upDaimyo2List[i];

                        pointerScript.createUpPointer(downDaimyo1, downDaimyo2);
                    }
                }
                if (downDaimyo1List.Count != 0) {
                    for (int i=0; i < downDaimyo1List.Count; i++) {
                        int downDaimyo1 = downDaimyo1List[i];
                        int downDaimyo2 = downDaimyo2List[i];

                        pointerScript.createDownPointer(downDaimyo1, downDaimyo2);
                    } 
                }
                if(metsubouKuniList.Count !=0) {
                    Gunzei gunzeiScript = new Gunzei();
                    for (int i = 0; i < metsubouKuniList.Count; i++) {
                        int kuniId = metsubouKuniList[i];
                        gunzeiScript.fire(kuniId);
                    }
                }
            }
		}
	}

}
