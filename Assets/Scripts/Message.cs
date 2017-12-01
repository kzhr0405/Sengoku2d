using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class Message : MonoBehaviour {


	public GameObject makeMessage (string Text) {
		string Path = "Prefabs/Common/MessageObject";
		GameObject messageObj = Instantiate (Resources.Load (Path)) as GameObject;
        if(GameObject.Find("Panel")) {
		    messageObj.transform.SetParent(GameObject.Find ("Panel").transform);
		    messageObj.transform.Find ("MessageText").transform.GetComponent<Text> ().text = Text;
		    messageObj.name = "MessageObject";

		    messageObj.transform.localScale = new Vector2 (1, 1);
		    messageObj.transform.localPosition = new Vector3(0, 11, 0);
        }
        return messageObj;

    }

    public GameObject makeMidMessage(string Text) {
        string Path = "Prefabs/Common/MessageMidObject";
        GameObject messageObj = Instantiate(Resources.Load(Path)) as GameObject;
        messageObj.transform.SetParent(GameObject.Find("Panel").transform);
        messageObj.transform.Find("MessageText").transform.GetComponent<Text>().text = Text;
        messageObj.name = "MessageObject";

        messageObj.transform.localScale = new Vector2(1, 1);
        messageObj.transform.localPosition = new Vector3(0, 11, 0);

        return messageObj;

    }

    public void makeMessageOnBoard (string Text) {
		string Path = "Prefabs/Common/MessageObject";
		GameObject messageObj = Instantiate (Resources.Load (Path)) as GameObject;
        GameObject panel = null;
        if (SceneManager.GetActiveScene().name == "mainStage") {
            panel = GameObject.Find("Map").gameObject;
        }else if(SceneManager.GetActiveScene().name == "pvp") {
            panel = GameObject.Find("Canvas").gameObject;
        }else {
            panel = GameObject.Find("Naisei").gameObject;
        }
        messageObj.transform.SetParent(panel.transform);
		messageObj.transform.Find ("MessageText").transform.GetComponent<Text> ().text = Text;
		messageObj.name = "MessageObject";
		
		messageObj.transform.localScale = new Vector2 (1, 1);
        messageObj.transform.localPosition = new Vector3(0, 0, 0);
        //RectTransform messageTransform = messageObj.GetComponent<RectTransform> ();
        //messageTransform.anchoredPosition = new Vector3 (0, 0, 0);
    }

	public GameObject makeKassenMessage (string Text) {
		string Path = "Prefabs/Common/KassenMessage";
		GameObject messageObj = Instantiate (Resources.Load (Path)) as GameObject;
		messageObj.transform.SetParent(GameObject.Find ("Canvas").transform);
		messageObj.transform.Find ("MessageText").transform.GetComponent<Text> ().text = Text;
		messageObj.name = "KassenMessage";
		
		messageObj.transform.localScale = new Vector2 (1, 1);
		RectTransform messageTransform = messageObj.GetComponent<RectTransform> ();
		messageTransform.anchoredPosition = new Vector3 (0, 0, 0);

        return messageObj;
    }

	public void makeUpperMessageOnBoard (string Text) {
		string Path = "Prefabs/Common/MessageUpperObject";
		GameObject messageObj = Instantiate (Resources.Load (Path)) as GameObject;
		messageObj.transform.SetParent(GameObject.Find ("Map").transform);
		messageObj.transform.Find ("MessageText").transform.GetComponent<Text> ().text = Text;
		messageObj.name = "MessageObject";

		messageObj.transform.localScale = new Vector2 (1, 1);
		RectTransform messageTransform = messageObj.GetComponent<RectTransform> ();
		messageTransform.anchoredPosition = new Vector3 (0, 260, 0);
	}

	public void makeMessageWithImage (string Text) {
		string Path = "Prefabs/Common/MessageWithImage";
		GameObject messageObj = Instantiate (Resources.Load (Path)) as GameObject;
		messageObj.transform.SetParent(GameObject.Find ("Panel").transform);
		messageObj.transform.Find ("Message").transform.GetComponent<Text> ().text = Text;
		messageObj.name = "MessageObject";

		messageObj.transform.localScale = new Vector2 (1, 1);
		RectTransform messageTransform = messageObj.GetComponent<RectTransform> ();
		messageTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
	}

	public void makeSlotMessage (List<string> MessageList) {
		string Path = "Prefabs/Common/SlotMessageBoard";
		GameObject messageBoardBase = Instantiate (Resources.Load (Path)) as GameObject;
		messageBoardBase.transform.SetParent(GameObject.Find ("Map").transform);
		messageBoardBase.name = "SlotMessageBoard";
		messageBoardBase.transform.localScale = new Vector2 (1, 1);
		messageBoardBase.transform.localPosition = new Vector3(0, 0, 0);

		GameObject content = messageBoardBase.transform.Find ("ScrollView").transform.Find ("Content").gameObject;

		string unitPath = "Prefabs/Common/SlotMessage";
		foreach(string message in MessageList){
			GameObject slotMessage = Instantiate (Resources.Load (unitPath)) as GameObject;
			messageBoardBase.GetComponent<FadeOutSlotMessage> ().contentList.Add (slotMessage);
			slotMessage.transform.SetParent (content.transform);
			slotMessage.transform.localScale = new Vector2 (1, 0.8f);
			slotMessage.transform.Find ("Text").GetComponent<Text> ().text = message;
		}
	
	}

    public void makeMeshMessage(string Text) {
        string Path = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId==2) {
            Path = "Prefabs/Common/MessageTextMeshEng";
        }else {
            Path = "Prefabs/Common/MessageTextMesh";
        }
        GameObject messageObj = Instantiate(Resources.Load(Path)) as GameObject;
        messageObj.transform.SetParent(GameObject.Find("Map").transform);
        messageObj.transform.Find("text").transform.GetComponent<TextMesh>().text = Text;
        messageObj.name = "MessageTextObject";

        RectTransform messageTransform = messageObj.GetComponent<RectTransform>();
        messageObj.transform.localScale = new Vector2(50, 15);
        messageTransform.anchoredPosition3D = new Vector3(0, 0, 0);
    
    }

    public GameObject makeIconExpMessage(string Text, GameObject canvasObj) {
        string Path = "Prefabs/Common/IconExpMessage";
        GameObject messageObj = Instantiate(Resources.Load(Path)) as GameObject;
        messageObj.transform.SetParent(canvasObj.transform);
        messageObj.transform.Find("MessageText").transform.GetComponent<Text>().text = Text;
        messageObj.name = "IconExpMessage";

        messageObj.transform.localScale = new Vector2(1, 1);
        messageObj.transform.localPosition = new Vector3(0, 0, 0);
        RectTransform messageTransform = messageObj.GetComponent<RectTransform>();
        messageTransform.anchoredPosition = new Vector3(0, 0, 0);

        return messageObj;
    }

    public GameObject makeMessageOnGameObject(string Text, GameObject obj) {
        string Path = "Prefabs/Common/MessageObject";
        GameObject messageObj = Instantiate(Resources.Load(Path)) as GameObject;
        messageObj.transform.SetParent(obj.transform);
        messageObj.transform.Find("MessageText").transform.GetComponent<Text>().text = Text;
        messageObj.name = "MessageObject";

        messageObj.transform.localScale = new Vector2(1, 1);
        messageObj.transform.localPosition = new Vector3(0, 11, 0);

        return messageObj;

    }


    public string getMessage(int id) {

        string message = "";
        int langId = PlayerPrefs.GetInt("langId");
        Entity_message_mst msgMst = Resources.Load("Data/message_mst") as Entity_message_mst;
        if (langId==2) {
            message = msgMst.param[id - 1].messageEng;
        }else {
            message = msgMst.param[id - 1].message;
        }
        
        return message;
    }


    public void hyourouMovieMessage() {
        GameObject canvas = null;
        GameObject MessageStaminaObject = null;
        if (SceneManager.GetActiveScene().name == "naisei") {
            canvas = GameObject.Find("Naisei").gameObject;
            MessageStaminaObject = GameObject.Find("NaiseiController").GetComponent<NaiseiController>().MessageStaminaObject;
        }else if(SceneManager.GetActiveScene().name == "pvp") {
            canvas = GameObject.Find("Canvas").gameObject;
            MessageStaminaObject = GameObject.Find("GameScene").GetComponent<PvPController>().MessageStaminaObject;
            canvas.GetComponent<Canvas>().sortingLayerName = "UI";
        }else {
            canvas = GameObject.Find("Map").gameObject;
            MessageStaminaObject = GameObject.Find("GameController").GetComponent<MainStageController>().MessageStaminaObject;
            canvas.GetComponent<Canvas>().sortingLayerName = "UI";
        }
        MessageStaminaObject.SetActive(true);
        
    }

    public void makeSpaceBuyBoard(string text) {

        int busyoDama = PlayerPrefs.GetInt("busyoDama");

        if(busyoDama<100) {
            makeMessage(text);
        }else {
            GameObject Panel = GameObject.Find("Panel");
            string pathOfBack = "Prefabs/Common/TouchBackForOne";
            GameObject back = Instantiate(Resources.Load(pathOfBack)) as GameObject;
            back.transform.SetParent(Panel.transform, false);

            string Path = "Prefabs/Common/SpaceBuyBoard";
            GameObject messageObj = Instantiate(Resources.Load(Path)) as GameObject;
            messageObj.transform.SetParent(Panel.transform,false);        
            messageObj.transform.Find("Text").GetComponent<Text>().text = text;

            //Slider
            GameObject slider = messageObj.transform.Find("Space").transform.Find("BusyoDamaSlider").gameObject;
            GameObject btn = messageObj.transform.Find("Space").transform.Find("DoBuy").gameObject;
            Slider sliderScript = slider.GetComponent<Slider>();
            int maxValue = busyoDama / 100;
            
            sliderScript.minValue = 1;
            sliderScript.maxValue = maxValue;
            btn.GetComponent<DoSpaceBuy>().busyoDamaOKflg = true;
            btn.GetComponent<DoSpaceBuy>().paiedBusyoDama = 100;
            btn.GetComponent<DoSpaceBuy>().touchBackObj = back;
            slider.GetComponent<BusyoDamaSlider>().doBtn = btn;
            back.GetComponent<CloseOneBoard>().deleteObj = messageObj;
        }


    }

}
