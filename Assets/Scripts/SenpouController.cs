	 using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SenpouController : MonoBehaviour {
	public int senpouId;
	public string senpouTyp;
	public string senpouName;
	public float senpouEach;
	public float senpouRatio;
	public float senpouTerm;
	public int senpouStatus;
	public int senpouLv;
	float senpouEachInit;
	float senpouTermInit;
	bool runflg;
	GameObject effectObj;
	public string senpouSerihu = "";

	//Initial Status
	public bool needStatusRecoveryFlg = false;
	public float initCoolTime = 0;
	public float initDisTarget = 0;



	// Use this for initialization
	void Start () {
		senpouEachInit = senpouEach;
		senpouTermInit = senpouTerm;

	}
	
	// Update is called once per frame
	void Update () {


		if(!runflg){
			senpouEach -= Time.deltaTime;
			if(senpouEach <= 0){
				//%
				float percent = Random.value;
				percent = percent * 100;
				
				 if(percent <= senpouRatio){
					//Do Senpou
					if (senpouTyp == "DownEnemy") {
						DownEnemy dn = new DownEnemy ();
						makeSenpouMessage (int.Parse (name));

						effectObj = dn.doSenpou (gameObject, senpouId, senpouName, senpouStatus, senpouLv);

					}else if(senpouTyp == "UpPlayer"){
						UpPlayer up = new UpPlayer ();
						makeSenpouMessage (int.Parse (name));
						
						effectObj = up.doSenpou (gameObject, senpouId, senpouStatus, initCoolTime, initDisTarget);
                        if (8 <= senpouId && senpouId <= 13) {
                            needStatusRecoveryFlg = true;
                        }
					}
					
					runflg = true;
				}
				//Initialization
				senpouEach = senpouEachInit;
			}
		}else{
			//Count Down
			senpouTerm -= Time.deltaTime;
			if(senpouTerm <= 0){

				//Recovery Status
				if(needStatusRecoveryFlg){
					if (GetComponent<Heisyu> ().heisyu == "YM" || GetComponent<Heisyu> ().heisyu == "TP" ) {
						GetComponent<AttackLong> ().coolTime = initCoolTime;
						if (GetComponent<UnitMover> ()) {
							GetComponent<UnitMover> ().DisTarget = initDisTarget;
						} else {
							GetComponent<HomingLong> ().DisTarget = initDisTarget;
						}
					}	
				}

				//Destroy
				Destroy(effectObj);

				//Flag Change
				runflg = false;
				senpouTerm = senpouTermInit;
			}
		}
	}

	public void makeSenpouMessage(int busyoId){

		if (gameObject.tag == "Player") {
			string path = "Prefabs/PreKassen/PlayerSenpouMsg";
			GameObject prefab = Instantiate(Resources.Load (path)) as GameObject;
			prefab.transform.SetParent (GameObject.Find("Canvas").transform);
			prefab.transform.localScale = new Vector3 (1,1,1);
			prefab.transform.localPosition = new Vector3 (-245,173,0);

			prefab.transform.FindChild ("SenpouName").GetComponent<Text> ().text = senpouName;
			prefab.transform.FindChild ("SerihuText").GetComponent<Text> ().text = senpouSerihu;

			string imagePath = "Prefabs/Player/Sprite/unit" + busyoId.ToString ();
			prefab.transform.FindChild ("Mask").transform.FindChild("BusyoImage").GetComponent<Image>().sprite = 
				Resources.Load (imagePath, typeof(Sprite)) as Sprite;
			
		} else {
			string path = "Prefabs/PreKassen/EnemySenpouMsg";
			GameObject prefab = Instantiate(Resources.Load (path)) as GameObject;
			prefab.transform.SetParent (GameObject.Find("Canvas").transform);
			prefab.transform.localScale = new Vector3 (-1,1,1);
			prefab.transform.localPosition = new Vector3 (245,173,0);

			prefab.transform.FindChild ("SenpouName").GetComponent<Text> ().text = senpouName;
			prefab.transform.FindChild ("SerihuText").GetComponent<Text> ().text = senpouSerihu;

			string imagePath = "Prefabs/Player/Sprite/unit" + busyoId.ToString ();
			prefab.transform.FindChild ("Mask").transform.FindChild("BusyoImage").GetComponent<Image>().sprite = 
				Resources.Load (imagePath, typeof(Sprite)) as Sprite;



		}




	}
}
