using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler {
	public static GameObject itemBeginDragged;
	Vector3 startPosition;
	Transform startParent;

	public void OnBeginDrag (PointerEventData eventData){
		itemBeginDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
	}
	
	public void OnDrag (PointerEventData eventData){
//		transform.position = Input.mousePosition;

		Vector2 pos;
		Canvas myCanvas = new Canvas();
		if (Application.loadedLevelName == "hyojyo" || Application.loadedLevelName == "tutorialHyojyo") {
			myCanvas = GameObject.Find ("Jinkei").GetComponent<Canvas> ();
		}else if(Application.loadedLevelName == "preKassen"|| Application.loadedLevelName == "preKaisen") {
			myCanvas = GameObject.Find ("preKassen").GetComponent<Canvas> ();
		}
		RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
		transform.position = myCanvas.transform.TransformPoint(pos);

	}

	public void OnEndDrag (PointerEventData eventData){

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		itemBeginDragged = null;
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
		if (transform.parent == startParent) {
			transform.position = startPosition;

            GridLayoutGroup GridLayoutGroup = startParent.GetComponent<GridLayoutGroup>();
            GridLayoutGroup.enabled = false;
            GridLayoutGroup.enabled = true;

        }else {

			//From ScrollView to JinkeiView
			if(startParent.name == "Slot"){
				audioSources [7].Play ();

				//Delete Container Box
				Destroy(startParent.gameObject);

				//Button
				gameObject.AddComponent<Button>();
				gameObject.AddComponent<Soudaisyo>();
				GetComponent<Button>().onClick.AddListener(GetComponent<Soudaisyo>().OnClick);

				//Update Busyo Status
				JinkeiPowerEffection powerEffection = new JinkeiPowerEffection ();
				powerEffection.UpdateSenryoku ();

                //In the case of 1st Jinkei Busyo, add soudaisyo
                if (Application.loadedLevelName == "preKassen") {
                    if (GameObject.Find("GameScene").GetComponent<preKassen>().busyoCurrentQty == 1) {
                        gameObject.GetComponent<Soudaisyo>().OnClick();
                    }
                }else if(Application.loadedLevelName == "preKaisen") {
                    if (GameObject.Find("GameScene").GetComponent<preKaisen>().busyoCurrentQty == 1) {
                        gameObject.GetComponent<Soudaisyo>().OnClick();
                    }
                }else {
                    if (GameObject.Find("jinkeiQtyValue").GetComponent<Text>().text == "1") {
                        gameObject.GetComponent<Soudaisyo>().OnClick();
                    }
                }




            }
            else{
				if(transform.parent.name == "Slot"){
					//JinkeiView to ScrollView
					audioSources [2].Play ();

					Destroy(gameObject.GetComponent("Button"));
					Destroy(gameObject.GetComponent("Soudaisyo"));

					//Update Busyo Status
					JinkeiPowerEffection powerEffection = new JinkeiPowerEffection ();
					powerEffection.UpdateSenryoku ();

					if(gameObject.transform.Find("soudaisyo")!=null){
				
						Destroy(gameObject.transform.Find("soudaisyo").gameObject);

                        if (Application.loadedLevelName != "preKassen" && Application.loadedLevelName != "preKaisen") {
                            GameObject.Find ("KakuteiButton").GetComponent<JinkeiConfirmButton>().soudaisyo = 0;
                        }else {
                            GameObject.Find("StartBtn").GetComponent<startKassen2>().soudaisyo = 0;
                        }
						//Switch if there is another busyo in Jinkei View
						foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Slot")){
							if(obs.transform.childCount >0){

								foreach (Transform chd in obs.transform){
									chd.GetComponent<Soudaisyo>().OnClick();
								}
								break;
							}
						}


					}
				}
			}
		}
		transform.position = startPosition;

	}


}
