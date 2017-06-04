using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class Slot : MonoBehaviour,IDropHandler {

	public GameObject item{
		get{
			if(transform.childCount>0){
				return transform.GetChild(0).gameObject;
			}
			return null;
		}
	}

	public int busyoLimitFlg{
		get{
            int busyoQty = 0;
            int busyoLimit = 0;
            if (Application.loadedLevelName == "preKassen") {
                preKassen preKassenScript = GameObject.Find("GameScene").GetComponent<preKassen>();
                busyoQty = preKassenScript.busyoCurrentQty;
                busyoLimit = preKassenScript.jinkeiLimit;
            }else if(Application.loadedLevelName == "preKaisen") {
                preKaisen preKassenScript = GameObject.Find("GameScene").GetComponent<preKaisen>();
                busyoQty = preKassenScript.busyoCurrentQty;
                busyoLimit = preKassenScript.jinkeiLimit;
            }
            else {
                Text busyoQtyObj = GameObject.Find ("jinkeiQtyValue").GetComponent<Text>();
			    busyoQty = int.Parse(busyoQtyObj.text);

			    Text busyoLimitObj = GameObject.Find ("jinkeiLimitValue").GetComponent<Text>();
                busyoLimit = int.Parse(busyoLimitObj.text);

			    
            }
            if (busyoQty + 1 > busyoLimit) {
                return 0; //NG
            }
            return busyoQty + 1; //OK

        }
    }



	public void OnDrop (PointerEventData eventData){

		JinkeiScene jinkeiScene = new JinkeiScene ();


		//From JinkeiView to ScrollView
		string path = "Prefabs/Jinkei/Slot";	
		bool limitFlg = true;
        bool diffClanFlg = false;
        AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (DragHandler.itemBeginDragged != null) {
			if (DragHandler.itemBeginDragged.transform.parent != null) {
				if (DragHandler.itemBeginDragged.transform.parent.name != "Slot") {
					if (transform.name == "ScrollView" || transform.name == "Slot") {
						//Drag JinkeiView -> Scroll
						GameObject prefab = Instantiate (Resources.Load (path)) as GameObject;
						prefab.transform.parent = GameObject.Find ("Content").transform;
						prefab.transform.localScale = new Vector3 (1, 1, 1);
						prefab.transform.localPosition = new Vector3(0, 0, 0);
						prefab.name = "Slot";
						DragHandler.itemBeginDragged.transform.SetParent(prefab.transform);

                        //Add Busyo Qty
                        if(Application.loadedLevelName == "preKassen") {
                            GameObject.Find("GameScene").GetComponent<preKassen>().busyoCurrentQty = GameObject.Find("GameScene").GetComponent<preKassen>().busyoCurrentQty - 1;
                            prefab.GetComponent<LayoutElement>().minHeight = 110;
                            prefab.GetComponent<LayoutElement>().minWidth = 110;
                        }else if(Application.loadedLevelName == "preKaisen") {
                            GameObject.Find("GameScene").GetComponent<preKaisen>().busyoCurrentQty = GameObject.Find("GameScene").GetComponent<preKaisen>().busyoCurrentQty - 1;
                            prefab.GetComponent<LayoutElement>().minHeight = 110;
                            prefab.GetComponent<LayoutElement>().minWidth = 110;
                        }else {
                            Text busyoQtyObj = GameObject.Find("jinkeiQtyValue").GetComponent<Text>();
                            int busyoQty = int.Parse(busyoQtyObj.text);
                            busyoQtyObj.text = (busyoQty - 1).ToString();
                        }
                        
					}

				}else if(DragHandler.itemBeginDragged.transform.parent.name == "Slot"){
					if(transform.name != "ScrollView" && transform.name !="Slot"){
						//Drag Scroll  -> JinkeiView 

						if(busyoLimitFlg !=0){

							if (!item) {

                                //hard
                                bool hardFlg = PlayerPrefs.GetBool("hardFlg");                                
                                if (hardFlg) {
                                    int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
                                    if(myDaimyo != DragHandler.itemBeginDragged.GetComponent<Senryoku>().belongDaimyoId) {
                                        diffClanFlg = true;
                                    }
                                }
                                if(diffClanFlg) {
                                    audioSources[4].Play();

                                    Message msg = new Message();
                                    msg.makeMessage(msg.getMessage(144));

                                } else { 

                                    if (Application.loadedLevelName == "preKassen") {
                                        GameObject.Find("GameScene").GetComponent<preKassen>().busyoCurrentQty = GameObject.Find("GameScene").GetComponent<preKassen>().busyoCurrentQty + 1;
                                    }else if(Application.loadedLevelName == "preKaisen") {
                                        GameObject.Find("GameScene").GetComponent<preKaisen>().busyoCurrentQty = GameObject.Find("GameScene").GetComponent<preKaisen>().busyoCurrentQty + 1;
                                    }else {
                                        GameObject.Find("jinkeiQtyValue").GetComponent<Text>().text = busyoLimitFlg.ToString();
                                    }
                                
								    audioSources [2].Play ();

                                    //Tutorial
                                    if (Application.loadedLevelName == "tutorialHyojyo") {
                                    
                                        GameObject RightView = GameObject.Find("RightView").gameObject;
                                        GameObject copied = Object.Instantiate(RightView) as GameObject;
                                        copied.transform.SetParent(GameObject.Find("Panel").transform);
                                        copied.transform.localScale = new Vector2(1,1);
                                        copied.transform.localPosition = new Vector2(530, -10);

                                        foreach(Transform chld in copied.transform.FindChild("JinkeiButton").transform) {
                                            chld.gameObject.SetActive(false);
                                        }
                                        GameObject status = copied.transform.FindChild("Status").gameObject;
                                        status.GetComponent<Image>().enabled = false;
                                        foreach (Transform chld in status.transform) {
                                            chld.gameObject.SetActive(false);
                                        }
                                        GameObject confirm = copied.transform.FindChild("KakuteiButton").gameObject;
                                        Vector2 vect = new Vector2(0, 50);
                                        TutorialController tutorialScript = new TutorialController();
                                        GameObject btn = tutorialScript.SetPointer(confirm, vect);
                                        btn.transform.localScale = new Vector2(150, 150);

                                        TextController txtScript = GameObject.Find("TextBoard").transform.FindChild("Text").GetComponent<TextController>();
                                        txtScript.SetText(11);
                                        txtScript.SetNextLine();
                                        txtScript.tutorialId = 11;
                                        txtScript.actOnFlg = false;
                                    }
                                }
							}else{
								audioSources [1].Play ();
								Debug.Log ("NOOOOOO");
							}

						}else{
							audioSources [4].Play ();
							limitFlg = false;
							Message msg = new Message(); 
							msg.makeMessage (msg.getMessage(59)); 

						}
					}
				}
			}
			if (!item) {
				if(limitFlg != false){
                    if(!diffClanFlg) {
					    DragHandler.itemBeginDragged.transform.SetParent (transform);
					    ExecuteEvents.ExecuteHierarchy<IHasChanged> (gameObject, null, (x,y) => x.HasChanged ());
                        audioSources [2].Play ();
                    }
                }
			} else {
				Debug.Log ("busyo exist");
			}
		}
	}
}
