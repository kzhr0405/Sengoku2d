using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShiroSearch : MonoBehaviour {

	public List<GameObject> busyoObjList;
	public List<Vector2> busyoObjSize;
	public List<GameObject> outBusyoObjList;
	public List<int> AITypeList;

	public string targetTag;
	public float DisTarget = 0;
	public GameObject nearObj;  
	public float Dis = 0;

	void Start () {

		if (tag == "Player") {
			targetTag = "Enemy";
		} else {
			targetTag = "Player";
		}

		if(name=="shiro"){
			DisTarget=50;
		}else if(name=="toride"){
			DisTarget=30;
		}

	}
	
	// Update is called once per frame
	void Update () {
		nearObj = serchTarget (gameObject, targetTag,DisTarget);

		if (nearObj) {
			if (busyoObjList.Count != 0) {
			
				//Syutujin
				for (int i = 0; i < busyoObjList.Count; i++) {
					GameObject busyoObj = busyoObjList [i];
					int AIType = AITypeList [i];
					if (busyoObj.GetComponent<Homing> ()) {
						busyoObj.GetComponent<Homing> ().enabled = true;
						busyoObj.GetComponent<Homing> ().AIType = AIType;
					} else if (busyoObj.GetComponent<HomingLong> ()) {
						busyoObj.GetComponent<HomingLong> ().enabled = true;			
						busyoObj.GetComponent<HomingLong> ().AIType = AIType;
					}
					busyoObj.transform.localScale = busyoObjSize [i];
					busyoObj.transform.localPosition = transform.localPosition;

					outBusyoObjList.Add (busyoObj);
					busyoObjList.Remove (busyoObj);
					busyoObjSize.RemoveAt (i);
				}
			}
		} else {
			if (outBusyoObjList.Count != 0) {

				//Tettai
				for (int i = 0; i < outBusyoObjList.Count; i++) {
					GameObject outBusyoObj = outBusyoObjList [i];
					if (outBusyoObj != null) {
						if (outBusyoObj.GetComponent<Homing> ()) {
							if (outBusyoObj.GetComponent<Homing> ().AIType != 2) {
								outBusyoObj.GetComponent<Homing> ().backShiroFlg = true;
							}
						} else if (outBusyoObj.GetComponent<HomingLong> ()) {
							if (outBusyoObj.GetComponent<HomingLong> ().AIType != 2) {
								outBusyoObj.GetComponent<HomingLong> ().backShiroFlg = true;
							}
						}
					}
				}
			}
		}
	}


	GameObject serchTarget(GameObject nowObj,string targetTag, float DisTarget){

		float tmpDis = 0;           //距離用一時変数
		float nearDis = 0;          //最も近いオブジェクトの距離
		GameObject targetObj = null; //オブジェクト

		//タグ指定されたオブジェクトを配列で取得する
		foreach (GameObject obs in  GameObject.FindGameObjectsWithTag(targetTag)){

			//自身と取得したオブジェクトの距離を取得
			tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

			//オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
			//一時変数に距離を格納
			if (tmpDis <= DisTarget) {
				if (nearDis == 0 || nearDis > tmpDis) {
					nearDis = tmpDis;
					Dis = nearDis;
					targetObj = obs;
				}
			}

		}

		//最も近かったオブジェクトを返す
		return targetObj;
	}
}
