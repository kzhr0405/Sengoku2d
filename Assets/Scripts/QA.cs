using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class QA : MonoBehaviour {

	public int qaId = 0;
	public string qaTitle = "";
	public string qaExp = "";

    
    public void OnClick () {

		//SE
		AudioSource sound = GameObject.Find ("SEController").GetComponent<AudioSource> ();
		sound.PlayOneShot(sound.clip); 

		/*Common*/
		//Get Info
		Entity_qa_mst qaMst = Resources.Load ("Data/qa_mst") as Entity_qa_mst;

        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            qaTitle = qaMst.param [qaId - 1].titleEng;
		    qaExp = qaMst.param [qaId - 1].ExpEng;
        }else if (langId == 3) {
            qaTitle = qaMst.param[qaId - 1].titleSChn;
            qaExp = qaMst.param[qaId - 1].ExpSChn;
        } else {
            qaTitle = qaMst.param[qaId - 1].title;
            qaExp = qaMst.param[qaId - 1].Exp;
        }
		string boardPath = "Prefabs/Common/QABoard";
		GameObject qaBoard = Instantiate (Resources.Load (boardPath)) as GameObject;

		//Each Scene
		if (Application.loadedLevelName == "naisei" || Application.loadedLevelName == "touyou" || Application.loadedLevelName == "shisya" || Application.loadedLevelName == "busyo" || Application.loadedLevelName == "hyojyo" || Application.loadedLevelName == "tutorialBusyo" || Application.loadedLevelName == "tutorialTouyou" || Application.loadedLevelName == "dataRecovery") {
			qaBoard.transform.SetParent (GameObject.Find ("Panel").transform);
		} else if(Application.loadedLevelName == "pvp") {
            qaBoard.transform.SetParent(GameObject.Find("Panel").transform);
            GameObject.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "UI";
        } else if(Application.loadedLevelName == "reward" || Application.loadedLevelName == "touyouEvent") {
            qaBoard.transform.SetParent(GameObject.Find("Canvas").transform);
        }else {
            qaBoard.transform.SetParent(GameObject.Find("Map").transform);
        }


		qaBoard.transform.localScale = new Vector2 (1, 1);
		RectTransform qaBoardTransform = qaBoard.GetComponent<RectTransform> ();
		qaBoardTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
		qaBoard.name = "QABoard";
		qaBoard.transform.Find ("Kakejiku").transform.Find ("Exp").GetComponent<Text> ().text = qaExp;
		qaBoard.transform.Find ("Kakejiku").transform.Find ("Title").GetComponent<Text> ().text = qaTitle;

        //Sort
        if(GameObject.Find("Canvas")) {
            GameObject.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "UI";
        }else if (GameObject.Find("Map")) {
            GameObject.Find("Map").GetComponent<Canvas>().sortingLayerName = "UI";
        }else if (GameObject.Find("Jinkei")) {
            GameObject.Find("Jinkei").GetComponent<Canvas>().sortingLayerName = "UI";
        }
    }
}
