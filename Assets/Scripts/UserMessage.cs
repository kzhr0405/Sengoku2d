using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using UnityEngine.UI;

public class UserMessage : MonoBehaviour {

    public AudioSource[] audioSources;
    public bool msgBoardOn = false;
    public bool msgBoardCreated = false;
    public int messageCount = -1;
    public List<string> messageList;

    void Start() {
        audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        if (Application.internetReachability != NetworkReachability.NotReachable) {
            GetMessage();
        }
    }

    public void OnClick () {
        Message msg = new Message();
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            audioSources[4].Play();
            msg.makeMessage(msg.getMessage(5));
        }else {
            audioSources[0].Play();
            msgBoardOn = true;            
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(msgBoardOn) {
           if(messageCount != -1 && !msgBoardCreated) {
                //board view
                msgBoardCreated = true;

                string pathOfBack = "Prefabs/Event/TouchEventBack";
                GameObject back = Instantiate(Resources.Load(pathOfBack)) as GameObject;
                GameObject panel = GameObject.Find("Panel").gameObject;
                back.transform.SetParent(panel.transform);
                back.transform.localScale = new Vector2(1, 1);
                back.transform.localPosition = new Vector2(0, 0);

                //make board
                string pathOfBoard = "Prefabs/Event/EventBoard";
                GameObject board = Instantiate(Resources.Load(pathOfBoard)) as GameObject;
                board.transform.SetParent(panel.transform);
                board.transform.localScale = new Vector2(1, 0.85f);

                back.GetComponent<CloseEventBoard>().deleteObj = board;
                back.GetComponent<CloseEventBoard>().deleteObj2 = back;
                Destroy(board.transform.FindChild("close").gameObject);                

                string pathOfScroll = "Prefabs/Event/EventScrollView";
                GameObject scroll = Instantiate(Resources.Load(pathOfScroll)) as GameObject;
                scroll.transform.SetParent(board.transform);
                scroll.transform.localScale = new Vector2(1, 1);
                RectTransform scrollTransform = scroll.GetComponent<RectTransform>();
                scrollTransform.anchoredPosition = new Vector3(0, -40, 0);

                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    board.transform.FindChild("popText").GetComponent<Text>().text = "Message";
                }else {
                    board.transform.FindChild("popText").GetComponent<Text>().text = "御連絡";
                }

                string pathOfMail = "Prefabs/Common/Mail";
                GameObject mail = Instantiate(Resources.Load(pathOfMail)) as GameObject;
                mail.transform.SetParent(board.transform);
                mail.transform.localScale = new Vector2(0.7f, 0.8f);
                mail.transform.localPosition = new Vector2(405,-265);

                string pathOfSlot = "Prefabs/Common/UserMessageSlot";
                foreach (string text in messageList) {
                    GameObject slot = Instantiate(Resources.Load(pathOfSlot)) as GameObject;
                    slot.transform.SetParent(scroll.transform.FindChild("Content").transform);
                    slot.transform.FindChild("EventText").GetComponent<Text>().text = text;
                    slot.transform.FindChild("EventText").GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
                    slot.transform.localScale = new Vector2(1, 1);
                }

                string pathOfProgress = "Prefabs/Common/Progress";
                GameObject progress = Instantiate(Resources.Load(pathOfProgress)) as GameObject;
                progress.transform.SetParent(board.transform);
                progress.transform.localScale = new Vector2(0.7f, 0.8f);
                progress.transform.localPosition = new Vector2(320, -265);
                progress.GetComponent<URL>().url = "https://trello.com/thesamuraiwars";

                msgBoardCreated = false;
                msgBoardOn = false;
            }           
        }
	}

    public void GetMessage() {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("message");
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if (e == null) {
                messageCount = objList.Count;
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    foreach (NCMBObject obj in objList) {
                        if(!System.Convert.ToBoolean(obj["stopFlg"])) {
                            messageList.Add(System.Convert.ToString(obj["messageEng"]));
                        }
                    }
                }else {
                    foreach (NCMBObject obj in objList) {
                        if (!System.Convert.ToBoolean(obj["stopFlg"])) {
                            messageList.Add(System.Convert.ToString(obj["time"]) + " " + System.Convert.ToString(obj["message"]));
                        }
                    }
                }         
            }else {
                audioSources[4].Play();
                Message msg = new Message();
                msg.makeMessage(msg.getMessage(113));
            }
        });
    }



}
