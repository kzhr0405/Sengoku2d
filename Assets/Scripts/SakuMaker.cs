using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class SakuMaker : MonoBehaviour {

	public int sakuId = 0;
	public int sakuEffect = 0;

	public int sakuBusyoId = 0;
	public string sakuHeisyu = "";
	public float sakuHeiSts = 0;
	public Vector2 vect;
	public float sakuBusyoSpeed;

	//Kengou Saku
	public int kengouQty = 0;
	public string kengouCd = "";
	public string kengouName = "";
	public int kengouHp = 0;
	public int kengouAtk = 0;
	public int kengouDfc = 0;
	public int kengouSpd = 0;

	// Use this for initialization
	void Start () {

		if (sakuId == 3) {
            //Hukuhei
            string targetTag = "";
            if (LayerMask.LayerToName(gameObject.layer) == "PlayerSaku") {
                targetTag = "Player";
            }else {
                targetTag = "Enemy";
            }

            AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
			audioSources [7].Play ();

            string ch_path = "";
            if (Application.loadedLevelName == "kaisen") {
                if (targetTag == "Player") {
                    ch_path = "Prefabs/Kaisen/3";
                }else {
                    ch_path = "Prefabs/Kaisen/3Enemy";
                }
            }else {
                if(targetTag=="Player") {
			        ch_path = "Prefabs/Player/hukuhei" + sakuHeisyu;
                }else {
                    ch_path = "Prefabs/Enemy/hukuhei" + sakuHeisyu;
                }
            }
            float y1 = 3.0f;
			float y2 = 3.0f;
			float y3 = 3.0f;
			float y4 = 3.0f;

			for (int i = 1; i <= sakuEffect; i++) {
				//Make Relationship
				GameObject ch_prefab = Instantiate (Resources.Load (ch_path)) as GameObject;
				ch_prefab.name = "hukuhei";

                //Sashimono Making
                if (Application.loadedLevelName != "kaisen") {
                    string sashimono_path = "Prefabs/Sashimono/" + sakuBusyoId;
				    GameObject sashimono = Instantiate (Resources.Load (sashimono_path)) as GameObject;
				    sashimono.transform.SetParent (ch_prefab.transform);
				    sashimono.transform.localScale = new Vector2 (0.3f, 0.3f);
                
				    if (sakuHeisyu == "YR") {
					    sashimono.transform.localPosition = new Vector2 (-1, 0.6f);
					    //Location
					    if (i < 6) {
						    ch_prefab.transform.position = new Vector2 (vect.x, vect.y + y1);
						    y1 = y1 - 1.5f;
						
					    } else if (5 < i && i < 11) {
						    ch_prefab.transform.position = new Vector2 (vect.x - 3, vect.y + y2);
						    y2 = y2 - 1.5f;
						
					    } else if (10 < i && i < 16) {
						    ch_prefab.transform.position = new Vector2 (vect.x - 6, vect.y + y3);
						    y3 = y3 - 1.5f;
						
					    } else if (15 < i && i < 21) {
						    ch_prefab.transform.position = new Vector2 (vect.x - 9, vect.y + y4);
						    y4 = y4 - 1.5f;
					    }
				    } else if (sakuHeisyu == "KB") {
					    sashimono.transform.localPosition = new Vector2 (-0.5f, 1);
					    //Location
					    if (i < 6) {
						
						    ch_prefab.transform.position = new Vector2 (vect.x, vect.y + y1);
						    y1 = y1 - 1.5f;
						
					    } else if (5 < i && i < 11) {
						    ch_prefab.transform.position = new Vector2 (vect.x - 4, vect.y + y2);
						    y2 = y2 - 1.5f;
						
					    } else if (10 < i && i < 16) {
						    ch_prefab.transform.position = new Vector2 (vect.x - 7, vect.y + y3);
						    y3 = y3 - 1.5f;
						
					    } else if (15 < i && i < 21) {
						    ch_prefab.transform.position = new Vector2 (vect.x - 10, vect.y + y4);
						    y4 = y4 - 1.5f;
					    }
				    } else if (sakuHeisyu == "TP") {
					    sashimono.transform.localPosition = new Vector2 (-0.8f, 0.5f);
					    //Location
					    if (i < 6) {
						
						    ch_prefab.transform.position = new Vector2 (vect.x, vect.y + y1);
						    y1 = y1 - 1.5f;
						
					    } else if (5 < i && i < 11) {
						    ch_prefab.transform.position = new Vector2 (vect.x - 4, vect.y + y2);
						    y2 = y2 - 1.5f;
						
					    } else if (10 < i && i < 16) {
						    ch_prefab.transform.position = new Vector2 (vect.x - 7, vect.y + y3);
						    y3 = y3 - 1.5f;
						
					    } else if (15 < i && i < 21) {
						    ch_prefab.transform.position = new Vector2 (vect.x - 10, vect.y + y4);
						    y4 = y4 - 1.5f;
					    }
				    } else if (sakuHeisyu == "YM") {
					    sashimono.transform.localPosition = new Vector2 (-0.8f, 0.5f);
					    //Location
					    if (i < 6) {
						
						    ch_prefab.transform.position = new Vector2 (vect.x, vect.y + y1);
						    y1 = y1 - 1.5f;
						
					    } else if (5 < i && i < 11) {
						    ch_prefab.transform.position = new Vector2 (vect.x - 4, vect.y + y2);
						    y2 = y2 - 1.5f;
						
					    } else if (10 < i && i < 16) {
						    ch_prefab.transform.position = new Vector2 (vect.x - 7, vect.y + y3);
						    y3 = y3 - 1.5f;
						
					    } else if (15 < i && i < 21) {
						    ch_prefab.transform.position = new Vector2 (vect.x - 10, vect.y + y4);
						    y4 = y4 - 1.5f;
					    }
				    }
                }else {
                    ch_prefab.AddComponent<Homing>();
                    if (ch_prefab.GetComponent<UnitMover>()) {
                        Destroy(ch_prefab.GetComponent<UnitMover>());
                    }
                    Destroy(ch_prefab.GetComponent<Kunkou>());
                    Destroy(ch_prefab.GetComponent<SenpouController>());

                    //Location
                    if (i < 6) {
                        ch_prefab.transform.position = new Vector2(vect.x, vect.y + y1);
                        y1 = y1 - 1.5f;
                    }else if (5 < i && i < 11) {
                        ch_prefab.transform.position = new Vector2(vect.x - 3, vect.y + y2);
                        y2 = y2 - 1.5f;
                    } else if (10 < i && i < 16) {
                        ch_prefab.transform.position = new Vector2(vect.x - 6, vect.y + y3);
                        y3 = y3 - 1.5f;
                    }else if (15 < i && i < 21) {
                        ch_prefab.transform.position = new Vector2(vect.x - 9, vect.y + y4);
                        y4 = y4 - 1.5f;
                    }
                }
				
				//Half Heiryoku
				float totalHei = sakuHeiSts * 10;
                if (targetTag == "Player") {
                    ch_prefab.GetComponent<PlayerHP> ().life = totalHei;
				    if (ch_prefab.GetComponent<PlayerAttack> ()) {
					    ch_prefab.GetComponent<PlayerAttack> ().attack = sakuHeiSts;
				    } else {
					    ch_prefab.GetComponent<AttackLong> ().attack = sakuHeiSts;
				    }
				    ch_prefab.GetComponent<PlayerHP> ().dfc = sakuHeiSts;

				    if (ch_prefab.GetComponent<Homing> () != null) {
					    ch_prefab.GetComponent<Homing> ().speed = sakuBusyoSpeed;
				    } else if (ch_prefab.GetComponent<HomingLong> () != null) {
					    ch_prefab.GetComponent<HomingLong> ().speed = sakuBusyoSpeed;
				    }
                }else {
                    ch_prefab.GetComponent<EnemyHP>().life = totalHei;
                    if (ch_prefab.GetComponent<EnemyAttack>()) {
                        ch_prefab.GetComponent<EnemyAttack>().attack = sakuHeiSts;
                    }else {
                        ch_prefab.GetComponent<AttackLong>().attack = sakuHeiSts;
                    }
                    ch_prefab.GetComponent<EnemyHP>().dfc = sakuHeiSts;

                    if (ch_prefab.GetComponent<Homing>() != null) {
                        ch_prefab.GetComponent<Homing>().speed = sakuBusyoSpeed;
                    }else if (ch_prefab.GetComponent<HomingLong>() != null) {
                        ch_prefab.GetComponent<HomingLong>().speed = sakuBusyoSpeed;
                    }

                }

				//SE
				AudioController audio = new AudioController ();
				audio.addComponentMoveAttack (ch_prefab, sakuHeisyu);

			}

		} else if (sakuId == 7) {
            //Kengou
			AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
			audioSources [7].Play ();
			GameScene GameSceneScript = GameObject.Find ("GameScene").GetComponent<GameScene> ();
			kengouHp = GameSceneScript.soudaisyoHp * sakuEffect;            
			kengouAtk = GameSceneScript.soudaisyoAtk * sakuEffect / 10;
			kengouDfc = GameSceneScript.soudaisyoDfc * sakuEffect / 10;
			kengouSpd = GameSceneScript.soudaisyoSpd;
			if (kengouSpd == 0) {
				kengouSpd = 1;
			}
			kengouName = GameSceneScript.kengouName;
            
			//Reduce Qty
			string kengouString = PlayerPrefs.GetString ("kengouItem");
			List<string> kengouList = new List<string> ();
			char[] delimiterChars = { ',' };
			kengouList = new List<string> (kengouString.Split (delimiterChars));
			
			int itemId = int.Parse (kengouCd.Remove (0, 6));
			int qty = int.Parse (kengouList [itemId - 1]);
			
			int remainQty = qty - 1;
			kengouList [itemId - 1] = remainQty.ToString ();
			
			string newKengouString = "";
			for (int i = 0; i < kengouList.Count; i++) {
				
				if (i == 0) {
					newKengouString = kengouList [i];
				} else {
					newKengouString = newKengouString + "," + kengouList [i];
				}
			}
			PlayerPrefs.SetString ("kengouItem", newKengouString);
			PlayerPrefs.Flush ();


			//Make Instance
			string kengouPath = "Prefabs/Player/kengou";
			GameObject kengou = Instantiate (Resources.Load (kengouPath)) as GameObject;
			kengou.name = "kengou";

            string dtlPath = "";
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                dtlPath = "Prefabs/BusyoDtl/BusyoDtlPlayerEng";
            }else {
                dtlPath = "Prefabs/BusyoDtl/BusyoDtlPlayer";
            }
            GameObject dtl = Instantiate (Resources.Load (dtlPath)) as GameObject;
			dtl.transform.SetParent (kengou.transform);
            dtl.name = "BusyoDtlPlayer";
			kengou.transform.position = new Vector2 (vect.x, vect.y);

			//Name
			GameObject nameLabel = dtl.transform.FindChild ("NameLabel").gameObject;
			nameLabel.GetComponent<TextMesh> ().text = kengouName;
			
			//HP Bar
			GameObject minHpBar = dtl.transform.FindChild ("MinHpBar").gameObject;
			minHpBar.GetComponent<BusyoHPBar> ().initLife = kengouHp;

			kengou.GetComponent<PlayerHP> ().life = kengouHp;
			kengou.GetComponent<PlayerAttack> ().attack = kengouAtk;
			kengou.GetComponent<Homing> ().speed = kengouSpd;
			kengou.GetComponent<PlayerHP> ().dfc = kengouDfc;

			//SE
			AudioController audio = new AudioController ();
			audio.addComponentMoveAttack (kengou, "YR");


		} else if (sakuId == 10) {
			//Youhei

			AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
			audioSources [7].Play ();

			//Reduce
			string nanbanString = PlayerPrefs.GetString ("nanbanItem");
			List<string> nanbanList = new List<string> ();
			char[] delimiterChars = { ',' };
			nanbanList = new List<string> (nanbanString.Split (delimiterChars));
			
			int qty = int.Parse (nanbanList [2]);
			
			int remainQty = qty - 1;
			nanbanList [2] = remainQty.ToString ();
			
			string newNanbanString = "";
			for (int i = 0; i < nanbanList.Count; i++) {
				
				if (i == 0) {
					newNanbanString = nanbanList [i];
				} else {
					newNanbanString = newNanbanString + "," + nanbanList [i];
				}
			}
			PlayerPrefs.SetString ("nanbanItem", newNanbanString);
			PlayerPrefs.Flush ();


			//Teppou youhei
			string tp_path = "Prefabs/Player/hukuheiTP";
			float y1 = 3.0f;

			for (int i = 1; i <= 5; i++) {

				//Make Relationship
				GameObject tp_prefab = Instantiate (Resources.Load (tp_path)) as GameObject;
				tp_prefab.name = "hukuhei";

				//Location
				tp_prefab.transform.position = new Vector2 (vect.x, vect.y + y1);
				y1 = y1 - 1.5f;					

				float totalHei = sakuHeiSts * 10;
				tp_prefab.GetComponent<PlayerHP> ().life = totalHei;
				tp_prefab.GetComponent<AttackLong> ().attack = sakuHeiSts;
				tp_prefab.GetComponent<PlayerHP> ().dfc = sakuHeiSts;
				tp_prefab.GetComponent<HomingLong> ().speed = sakuBusyoSpeed;
				
				AudioController audio = new AudioController ();
				audio.addComponentMoveAttack (tp_prefab, "TP");
			}
		} else if (sakuId == 14) {
			//Gokui
			//Shinkage ryu
			AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
			audioSources [7].Play ();

            //Hikita
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                bool kengouFlg1 = CheckByProbability(sakuEffect);
			    if (kengouFlg1) {
				    makeKengou(100, "Hikita");
			    }
			    //Houzouin
			    bool kengouFlg2 = CheckByProbability(sakuEffect); 
			    if (kengouFlg2) {
				    makeKengou(80,"Hozoin");
			    }
			    //Jingo
			    bool kengouFlg3 = CheckByProbability(sakuEffect*2); 
			    if (kengouFlg3) {
				    makeKengou(40,"Jingo");
			    }
			    //Okuyama
			    bool kengouFlg4 = CheckByProbability(sakuEffect*2); 
			    if (kengouFlg4) {
				    makeKengou(40,"Okuyama");
			    }
            }else {
                bool kengouFlg1 = CheckByProbability(sakuEffect);
                if (kengouFlg1) {
                    makeKengou(100, "疋田景兼");
                }
                //Houzouin
                bool kengouFlg2 = CheckByProbability(sakuEffect);
                if (kengouFlg2) {
                    makeKengou(80, "宝蔵院胤栄");
                }
                //Jingo
                bool kengouFlg3 = CheckByProbability(sakuEffect * 2);
                if (kengouFlg3) {
                    makeKengou(40, "神後宗治");
                }
                //Okuyama
                bool kengouFlg4 = CheckByProbability(sakuEffect * 2);
                if (kengouFlg4) {
                    makeKengou(40, "奥山公重");
                }
            }

        }
	}

	public bool CheckByProbability (int ratio) {
		bool checkFlg = false;

		float percent = Random.value;
		percent = percent * 100;
		ratio = 100 - ratio;

		if(percent > ratio){
			checkFlg = true;
		}
		return checkFlg;
	}

	public void makeKengou(int effect, string kengouName){
        bool playerFlg;
        if (LayerMask.LayerToName(gameObject.layer) == "PlayerSaku") {
            playerFlg = true;
        }else {
            playerFlg = false;
        }

        GameScene GameSceneScript = GameObject.Find ("GameScene").GetComponent<GameScene> ();
		kengouHp = GameSceneScript.soudaisyoHp * effect;
		kengouAtk = GameSceneScript.soudaisyoAtk * effect / 10;
		kengouDfc = GameSceneScript.soudaisyoDfc * effect / 10;
		kengouSpd = GameSceneScript.soudaisyoSpd;
		if (kengouSpd == 0) {
			kengouSpd = 1;
		}

        //Make Instance
        string kengouPath = "";
        if(playerFlg) {
            kengouPath = "Prefabs/Player/kengou";
        }else {
            kengouPath = "Prefabs/Enemy/kengou";
        }
        GameObject kengou = Instantiate (Resources.Load (kengouPath)) as GameObject;
		kengou.name = "kengou";

        string dtlPath = "";
        if(playerFlg) {
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                dtlPath = "Prefabs/BusyoDtl/BusyoDtlPlayerEng";
            }else {
                dtlPath = "Prefabs/BusyoDtl/BusyoDtlPlayer";
            }
        }else {
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                dtlPath = "Prefabs/BusyoDtl/BusyoDtlEnemyEng";
            }else {
                dtlPath = "Prefabs/BusyoDtl/BusyoDtlEnemy";
            }
        }


        GameObject dtl = Instantiate (Resources.Load (dtlPath)) as GameObject;
		dtl.transform.SetParent (kengou.transform);
        if (playerFlg) {
            dtl.name = "BusyoDtlPlayer";
        }else {
            dtl.name = "BusyoDtlEnemy";
        }

		//random position
		int rdmX = UnityEngine.Random.Range (0, 5);
		int rdmY = UnityEngine.Random.Range (0, 5);
		int plusminusX = UnityEngine.Random.Range (0, 2);
		int plusminusY = UnityEngine.Random.Range (0, 2);
		if (plusminusX == 1) {
			rdmX = rdmX * -1;
		}
		if (plusminusY == 1) {
			rdmY = rdmY * -1;
		}

		kengou.transform.position = new Vector2 (vect.x + rdmX, vect.y + rdmY);

		//Name
		GameObject nameLabel = dtl.transform.FindChild ("NameLabel").gameObject;
		nameLabel.GetComponent<TextMesh> ().text = kengouName;

		//HP Bar
		GameObject minHpBar = dtl.transform.FindChild ("MinHpBar").gameObject;
		minHpBar.GetComponent<BusyoHPBar> ().initLife = kengouHp;

        if (playerFlg) {
            kengou.GetComponent<PlayerHP> ().life = kengouHp;
		    kengou.GetComponent<PlayerAttack> ().attack = kengouAtk;
		    kengou.GetComponent<Homing> ().speed = kengouSpd;
		    kengou.GetComponent<PlayerHP> ().dfc = kengouDfc;
        }else {
            kengou.GetComponent<EnemyHP>().life = kengouHp;
            kengou.GetComponent<EnemyAttack>().attack = kengouAtk;
            kengou.GetComponent<Homing>().speed = kengouSpd;
            kengou.GetComponent<EnemyHP>().dfc = kengouDfc;
        }

		//SE
		AudioController audio = new AudioController ();
		audio.addComponentMoveAttack (kengou, "YR");


		string damagePath = "Prefabs/PreKassen/SakuMessage";
		GameObject damageObj = Instantiate (Resources.Load (damagePath)) as GameObject;
		damageObj.transform.SetParent (gameObject.transform);
		damageObj.transform.position = new Vector3 (kengou.transform.position.x, kengou.transform.position.y, 0);
		damageObj.transform.localScale = new Vector3 (0.015f, 0.02f, 0);
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            damageObj.GetComponent<TextMesh> ().text = kengouName + " here";
        }else {
            damageObj.GetComponent<TextMesh>().text = kengouName + "推参";
        }
	}
}
