﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class SakuCollider : MonoBehaviour {

	public int sakuId = 0;
	public int sakuEffect = 0;
	public float sakuTempEffect = 0;
    public Vector2 vect;
    public Color upColor = new Color (50f / 255f, 190f / 255f, 35f / 255f, 255f / 255f); //Green Up

    public void Start() {
        if (sakuId == 9) {
            string nanbanString = PlayerPrefs.GetString("nanbanItem");
            List<string> nanbanList = new List<string>();
            char[] delimiterChars = { ',' };
            nanbanList = new List<string>(nanbanString.Split(delimiterChars));

            int qty = int.Parse(nanbanList[1]);

            int remainQty = qty - 1;
            nanbanList[1] = remainQty.ToString();

            string newNanbanString = "";
            for (int i = 0; i < nanbanList.Count; i++) {

                if (i == 0) {
                    newNanbanString = nanbanList[i];
                }
                else {
                    newNanbanString = newNanbanString + "," + nanbanList[i];
                }
            }
            PlayerPrefs.SetString("nanbanItem", newNanbanString);
            PlayerPrefs.Flush();
            
            if (Application.loadedLevelName == "kaisen") {
                AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
                audioSources[10].Play();
                
                //Nanbansen
                string ch_path = "Prefabs/Kaisen/NBN";
                GameObject ch_prefab = Instantiate(Resources.Load(ch_path)) as GameObject;
                ch_prefab.name = "hukuhei";
                ch_prefab.transform.position = new Vector2(vect.x, vect.y);

                string dtlPath = "";
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    dtlPath = "Prefabs/BusyoDtl/BusyoDtlPlayerEng";
                }else {
                    dtlPath = "Prefabs/BusyoDtl/BusyoDtlPlayer";
                }
                GameObject dtl = Instantiate(Resources.Load(dtlPath)) as GameObject;
                dtl.transform.SetParent(ch_prefab.transform);
                dtl.transform.localPosition = new Vector2(0,1);
                dtl.transform.localScale = new Vector2(1, 1);
                dtl.name = "BusyoDtlPlayer";

                //Name
                GameObject nameLabel = dtl.transform.FindChild("NameLabel").gameObject;
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    nameLabel.GetComponent<TextMesh>().text = "Western Ship";
                }else {
                    nameLabel.GetComponent<TextMesh>().text = "南蛮船";
                }

                //Status Get
                KaisenScene kaisenScript = GameObject.Find("GameScene").GetComponent<KaisenScene>();
                int nbnHp = kaisenScript.soudaisyoHp * 300;
                int nbnAtk = kaisenScript.soudaisyoAtk * 30;
                int nbnDfc = kaisenScript.soudaisyoDfc * 30;
                float nbnSpd = kaisenScript.soudaisyoSpd;
                if (nbnSpd == 0) {
                    nbnSpd = 1;
                }

                //HP Bar
                GameObject minHpBar = dtl.transform.FindChild("MinHpBar").gameObject;
                minHpBar.GetComponent<BusyoHPBar>().initLife = nbnHp;

                ch_prefab.GetComponent<PlayerHP>().life = nbnHp;
                ch_prefab.GetComponent<PlayerAttack>().attack = nbnAtk;
                ch_prefab.GetComponent<Homing>().speed = nbnSpd;
                ch_prefab.GetComponent<PlayerHP>().dfc = nbnDfc;

                //SE
                AudioController audio = new AudioController();
                audio.addComponentMoveAttack(ch_prefab, "SHP");

            }
        }
    }



	private void OnTriggerEnter2D(Collider2D col){
		if (sakuId == 1) {
			//Kobu
			//Check Player
			if (col.tag == "Player") {

				//OK
				AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
				audioSources [7].Play ();//Sakebi

				//string 
				float addAtk = 0;

				//Check YR,KB or TP,YM
				if (col.GetComponent<PlayerAttack> () != null) {

					float baseAtk = col.GetComponent<PlayerAttack> ().attack;
					float temp = baseAtk * sakuEffect;
					addAtk = temp / 100;
					float newAtk = Mathf.Ceil (baseAtk + addAtk);
					col.GetComponent<PlayerAttack> ().attack = newAtk;

				} else if (col.GetComponent<AttackLong> () != null) {
					float baseAtk = col.GetComponent<AttackLong> ().attack;
					float temp = baseAtk * sakuEffect;
					addAtk = temp / 100;
					float newAtk = Mathf.Ceil (baseAtk + addAtk);
					col.GetComponent<AttackLong> ().attack = newAtk;
				}

				//Only Parent
				if (col.GetComponent<UnitMover> () != null) {
					GameObject dtl = col.transform.FindChild ("BusyoDtlPlayer").gameObject;
					GameObject effect = dtl.transform.FindChild ("atk_up").gameObject;
					effect.GetComponent<FadeoutOff> ().currentRemainTime = 5;
					effect.GetComponent<SpriteRenderer> ().enabled = true;
					effect.GetComponent<Animator> ().enabled = true;
					effect.GetComponent<FadeoutOff> ().enabled = true;

					GameObject value = dtl.transform.FindChild ("MinHpBar").transform.FindChild ("Value").gameObject;
					value.GetComponent<MeshRenderer> ().enabled = true;
					value.GetComponent<TextMeshFadeoutOff> ().enabled = true;
					value.GetComponent<TextMesh> ().text = (Mathf.Ceil(addAtk)).ToString () + "⇡";
					value.GetComponent<TextMesh> ().color = upColor;
				}
			}

		} else if (sakuId == 2 ||sakuId == 8) {
			//Hokyu or Seiyouigaku
			AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
			audioSources [3].Play ();

			//Check Player
			if (col.tag == "Player") {
				float baseHP = col.GetComponent<PlayerHP> ().life;
				float initLife = col.GetComponent<PlayerHP> ().initLife;
				float newHP = 0;

				if(sakuId == 2){
					newHP = baseHP + sakuEffect;
				}else if(sakuId == 8){
					sakuTempEffect = (int)(initLife*sakuEffect/100);
					newHP = baseHP + sakuTempEffect;
				}
				col.GetComponent<PlayerHP> ().life = newHP;
			
				//Only Parent
				if (col.name != "hukuhei") {
					if (col.GetComponent<UnitMover> () != null) {
						GameObject dtl = col.transform.FindChild ("BusyoDtlPlayer").gameObject;
						GameObject effect = dtl.transform.FindChild ("hp_up").gameObject;
						effect.GetComponent<FadeoutOff> ().currentRemainTime = 5;
						effect.GetComponent<SpriteRenderer> ().enabled = true;
						effect.GetComponent<Animator> ().enabled = true;
						effect.GetComponent<FadeoutOff> ().enabled = true;

						GameObject value = dtl.transform.FindChild ("MinHpBar").transform.FindChild ("Value").gameObject;
						value.GetComponent<MeshRenderer> ().enabled = true;
						value.GetComponent<TextMeshFadeoutOff> ().enabled = true;
						if (sakuId == 2) {
							value.GetComponent<TextMesh> ().text = sakuEffect.ToString () + "⇡";
						} else if (sakuId == 8) {
							value.GetComponent<TextMesh> ().text = sakuTempEffect.ToString () + "⇡";
						}
						value.GetComponent<TextMesh> ().color = upColor;
					}
				}
			}


		} else if (sakuId == 4) {
			//Mizuzeme
			AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
			audioSources [6].Play ();

			float reduceHp = sakuEffect;
			if (col.tag == "Player") {
				float baseHP = col.GetComponent<PlayerHP> ().life;
				if (col.GetComponent<Heisyu> ().heisyu == "TP" || col.GetComponent<Heisyu> ().heisyu == "YM") {
					reduceHp = 2 * reduceHp;
				}
				int temp = (int)(baseHP * reduceHp) / 100;
				float newHP = baseHP - temp;
				col.GetComponent<PlayerHP> ().life = newHP;
				
				if (col.GetComponent<UnitMover> () != null) {
					GameObject value = col.transform.FindChild ("BusyoDtlPlayer").transform.FindChild ("MinHpBar").transform.FindChild ("Value").gameObject;
					value.GetComponent<MeshRenderer> ().enabled = true;
					value.GetComponent<TextMeshFadeoutOff> ().enabled = true;
					value.GetComponent<TextMesh> ().text = temp.ToString () + "⇣";
				}

			} else if (col.tag == "Enemy") {
				float baseHP = col.GetComponent<EnemyHP> ().life;
				if (col.GetComponent<Heisyu> ().heisyu == "TP" || col.GetComponent<Heisyu> ().heisyu == "YM") {
					reduceHp = 2 * reduceHp;
				}
				int temp = (int)(baseHP * reduceHp) / 100;
				float newHP = baseHP - temp;

				col.GetComponent<EnemyHP> ().life = newHP;

				if (col.GetComponent<Homing> () != null || col.GetComponent<HomingLong> () != null) {
					GameObject value = col.transform.FindChild ("BusyoDtlEnemy").transform.FindChild ("MinHpBar").transform.FindChild ("Value").gameObject;
					value.GetComponent<MeshRenderer> ().enabled = true;
					value.GetComponent<TextMeshFadeoutOff> ().enabled = true;
					value.GetComponent<TextMesh> ().text = temp.ToString () + "⇣";
				}
			}

		} else if (sakuId == 6) {
			//Kakei
			AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
			audioSources [6].Play ();


			float reduceHp = sakuEffect;
			if (col.tag == "Player") {
				float baseHP = col.GetComponent<PlayerHP> ().life;
				if (col.GetComponent<Heisyu> ().heisyu == "YR" || col.GetComponent<Heisyu> ().heisyu == "KB") {
					reduceHp = 2 * reduceHp;
				}
				int temp = (int)(baseHP * reduceHp) / 100;
				float newHP = baseHP - temp;
				col.GetComponent<PlayerHP> ().life = newHP;

				
				if (col.GetComponent<UnitMover> () != null) {
					GameObject value = col.transform.FindChild ("BusyoDtlPlayer").transform.FindChild ("MinHpBar").transform.FindChild ("Value").gameObject;
					value.GetComponent<MeshRenderer> ().enabled = true;
					value.GetComponent<TextMeshFadeoutOff> ().enabled = true;
					value.GetComponent<TextMesh> ().text = temp.ToString () + "⇣";
				}
				
			} else if (col.tag == "Enemy") {

				float baseHP = col.GetComponent<EnemyHP> ().life;
				if (col.GetComponent<Heisyu> ().heisyu == "YR" || col.GetComponent<Heisyu> ().heisyu == "KB") {
					reduceHp = 2 * reduceHp;
				}
				int temp = (int)(baseHP * reduceHp) / 100;
				float newHP = baseHP - temp;
				
				col.GetComponent<EnemyHP> ().life = newHP;
				
				if (col.GetComponent<Homing> () != null || col.GetComponent<HomingLong> () != null) {
					GameObject value = col.transform.FindChild ("BusyoDtlEnemy").transform.FindChild ("MinHpBar").transform.FindChild ("Value").gameObject;
					value.GetComponent<MeshRenderer> ().enabled = true;
					value.GetComponent<TextMeshFadeoutOff> ().enabled = true;
					value.GetComponent<TextMesh> ().text = temp.ToString () + "⇣";
				}
			}

		} else if (sakuId == 9) {
            //Engosyageki
            if (Application.loadedLevelName != "kaisen") {

                AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
    			audioSources [10].Play ();
                
                float reduceHp = sakuEffect;
			    if (col.tag == "Player") {
				    float baseHP = col.GetComponent<PlayerHP> ().life;
				    int temp = (int)(baseHP * reduceHp) / 100;
				    float newHP = baseHP - temp;
				    col.GetComponent<PlayerHP> ().life = newHP;

				    if (col.GetComponent<UnitMover> () != null) {
					    GameObject value = col.transform.FindChild ("BusyoDtlPlayer").transform.FindChild ("MinHpBar").transform.FindChild ("Value").gameObject;
					    value.GetComponent<MeshRenderer> ().enabled = true;
					    value.GetComponent<TextMeshFadeoutOff> ().enabled = true;
					    value.GetComponent<TextMesh> ().text = temp.ToString () + "⇣";
				    }

			    } else if (col.tag == "Enemy") {
				
				    float baseHP = col.GetComponent<EnemyHP> ().life;

				    int temp = (int)(baseHP * reduceHp) / 100;
				    float newHP = baseHP - temp;

				    col.GetComponent<EnemyHP> ().life = newHP;

				    if (col.GetComponent<Homing> () != null || col.GetComponent<HomingLong> () != null) {
					    GameObject value = col.transform.FindChild ("BusyoDtlEnemy").transform.FindChild ("MinHpBar").transform.FindChild ("Value").gameObject;
					    value.GetComponent<MeshRenderer> ().enabled = true;
					    value.GetComponent<TextMeshFadeoutOff> ().enabled = true;
					    value.GetComponent<TextMesh> ().text = temp.ToString () + "⇣";
				    }
			    }
            
            }

        }
        else if(11<=sakuId && sakuId <=15){
			//Stop Once & Run Animation
			if (col.tag == "Enemy") {
				if (col.name != "shiro") {

					//Gokui
					if (sakuId == 11) {
						//Ittouryu
						float baseHP = col.GetComponent<EnemyHP> ().life;

						int temp = (int)(baseHP * sakuEffect) / 100;
						col.gameObject.GetComponent<EnemyHP> ().DirectDamage (temp);

						string damagePath = "Prefabs/PreKassen/ArialMessage";
						GameObject damageObj = Instantiate (Resources.Load (damagePath)) as GameObject;
						damageObj.transform.SetParent (gameObject.transform);
						damageObj.transform.position = new Vector3 (col.transform.position.x, col.transform.position.y, 0);
						damageObj.transform.localScale = new Vector3 (0.015f, 0.02f, 0);
						damageObj.GetComponent<TextMesh> ().text = "-" + temp;

					} else if (sakuId == 12) {	
						//ichino tachi
						bool successFlg = CheckByProbability (sakuEffect);
						string damagePath = "Prefabs/PreKassen/SakuMessage";
						GameObject damageObj = Instantiate (Resources.Load (damagePath)) as GameObject;
						damageObj.transform.SetParent (gameObject.transform);
						damageObj.transform.position = new Vector3 (col.transform.position.x, col.transform.position.y, 0);
						damageObj.transform.localScale = new Vector3 (0.015f, 0.02f, 0);

                        if (Application.systemLanguage == SystemLanguage.Japanese) {
                            if (successFlg) {
							    Destroy (col.gameObject);
							    damageObj.GetComponent<TextMesh> ().text = "撃破";
						    } else {
							    damageObj.GetComponent<TextMesh> ().text = "失敗";
						    }
                        }else {
                            if (successFlg) {
                                Destroy(col.gameObject);
                                damageObj.GetComponent<TextMesh>().text = "Destroied";
                            }else {
                                damageObj.GetComponent<TextMesh>().text = "Failed";
                            }
                        }
					} else if (sakuId == 13) {
						//kodachi
						int count = 1;
						foreach (Transform child in col.transform) {
							if (child.tag == "EnemyChild") {
								child.transform.DetachChildren ();
								Destroy (child.gameObject);

								count = count + 1;
								if (count >= sakuEffect) {
									break;
								}
							}
						}
						string damagePath = "Prefabs/PreKassen/SakuMessage";
						GameObject damageObj = Instantiate (Resources.Load (damagePath)) as GameObject;
						damageObj.transform.SetParent (gameObject.transform);
						damageObj.transform.position = new Vector3 (col.transform.position.x, col.transform.position.y, 0);
						damageObj.transform.localScale = new Vector3 (0.015f, 0.02f, 0);
                        if (Application.systemLanguage == SystemLanguage.Japanese) {
                            damageObj.GetComponent<TextMesh> ().text = "撃破" + sakuEffect + "部隊";
                        }else {
                            damageObj.GetComponent<TextMesh>().text = "Destroied " + sakuEffect + " unit";
                        }

					} else if (sakuId == 15) {
						//yagyu shingakeryu


						int count = 1;
						SenpouBetray betrayScript = new SenpouBetray ();
						foreach (Transform child in col.transform) {
							if (child.tag == "EnemyChild") {
								betrayScript.betrayEnemy(child.gameObject);
								count = count + 1;
								if (count >= sakuEffect) {
									break;
								}
							}
						}
						string damagePath = "Prefabs/PreKassen/SakuMessage";
						GameObject damageObj = Instantiate (Resources.Load (damagePath)) as GameObject;
						damageObj.transform.SetParent (gameObject.transform);
						damageObj.transform.position = new Vector3 (col.transform.position.x, col.transform.position.y, 0);
						damageObj.transform.localScale = new Vector3 (0.015f, 0.02f, 0);
                        if (Application.systemLanguage == SystemLanguage.Japanese) {
                            damageObj.GetComponent<TextMesh> ().text = "寝返" + sakuEffect + "部隊";
                        }else {
                            damageObj.GetComponent<TextMesh>().text = sakuEffect + " unit betrayed";
                        }

					}


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
}
