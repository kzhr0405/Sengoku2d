using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class QuestMenu : MonoBehaviour {

    public bool clicked = false;
    public bool daily = false;
    public GameObject otherObj;
    public List<int> activeList;
    public List<int> inactiveList;
    public List<int> activeCountList;
    public GameObject scrollObj;

    private void Start() {
        Entity_quest_mst questMst = Resources.Load("Data/quest_mst") as Entity_quest_mst;
        Entity_quest_count_mst questCountMst = Resources.Load("Data/quest_count_mst") as Entity_quest_count_mst;

        //Set common param
        if (name=="Daily") {
            daily = true;
        }
        foreach(Transform chld in transform.parent) {
            if(chld.name != gameObject.name) {
                otherObj = chld.gameObject;
            }
        }

        //Set quest count data
        List<int> skipGrpList = new List<int>();
        for(int i=0; i< questCountMst.param.Count; i++) {
            bool dailyFlg  = questCountMst.param[i].daily;
            int grpId  = questCountMst.param[i].grp;
            if(!skipGrpList.Contains(grpId)) {
                if (!dailyFlg && !daily) {
                    //Special                    
                    string tmp = "questSpecialCountReceivedFlg" + i.ToString();
                    bool receivedFlg = PlayerPrefs.GetBool(tmp, false);
                    
                    if (!receivedFlg) {
                        activeCountList.Add(i);
                        skipGrpList.Add(grpId);
                    }
                    
                } else if (dailyFlg && daily) {
                    //Daily
                    string tmp = "questDailyCountReceivedFlg" + i.ToString();
                    bool receivedFlg = PlayerPrefs.GetBool(tmp, false);
                    if (!receivedFlg) {
                        activeCountList.Add(i);
                        skipGrpList.Add(grpId);
                    }
                }
            }
        }

        //Set quest data
        for (int i = 0; i < questMst.param.Count; i++) {
            bool dailyFlg = questMst.param[i].daily;

            if (!dailyFlg && !daily) {
                //Special
                string tmp = "questSpecialFlg" + i.ToString();
                bool activeFlg = PlayerPrefs.GetBool(tmp, false);
                if (activeFlg) {
                    string tmp2 = "questSpecialReceivedFlg" + i.ToString();
                    bool activeFlg2 = PlayerPrefs.GetBool(tmp2, false);
                    if (!activeFlg2) {
                        activeList.Add(i);
                    }
                }else {
                    string tmp2 = "questSpecialReceivedFlg" + i.ToString();
                    bool activeFlg2 = PlayerPrefs.GetBool(tmp2, false);
                    if (!activeFlg2) {
                        inactiveList.Add(i);
                    }
                }
            }else if(dailyFlg && daily){
                //Daily
                string tmp = "questDailyFlg" + i.ToString();
                bool activeFlg = PlayerPrefs.GetBool(tmp, false);
                if (activeFlg) {
                    //active
                    string tmp2 = "questDailyReceivedFlg" + i.ToString();
                    bool activeFlg2 = PlayerPrefs.GetBool(tmp2, false);
                    if (!activeFlg2) {
                        activeList.Add(i);
                    }
                }else {
                    //inactive
                    string tmp2 = "questSpecialReceivedFlg" + i.ToString();
                    bool activeFlg2 = PlayerPrefs.GetBool(tmp2, false);
                    if (!activeFlg2) {
                        inactiveList.Add(i);
                    }
                }
            }
        }

        //initial set
        if(daily) {
            OnClick();
        }
        
    }

    public void OnClick() {

        AudioSource sound = GameObject.Find("SEController").GetComponent<AudioSource>();
        sound.PlayOneShot(sound.clip);

        if (!clicked) {
            clicked = true;

            //color
            Color pushedTabColor = new Color(118f / 255f, 103f / 255f, 16f / 255f, 255f / 255f);
            Color pushedTextColor = new Color(219f / 255f, 219f / 255f, 212f / 255f, 255f / 255f);
            Color normalTabColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            Color normalTextColor = new Color(255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);

            GetComponent<Image>().color = pushedTabColor;
            transform.FindChild("Text").GetComponent<Text>().color = pushedTextColor;
            otherObj.GetComponent<Image>().color = normalTabColor;
            otherObj.transform.FindChild("Text").GetComponent<Text>().color = normalTextColor;
            otherObj.GetComponent<QuestMenu>().clicked = false;

            //delete previous one
            GameObject content = scrollObj.transform.FindChild("Content").gameObject;
            foreach(Transform chld in content.transform) {
                Destroy(chld.gameObject);
            }

            //create count slot
            string activeCountSlotPath = "Prefabs/Map/quest/ActiveQuestCountSlot";
            Entity_quest_count_mst questCountMst = Resources.Load("Data/quest_count_mst") as Entity_quest_count_mst;
            for (int j = 0; j < activeCountList.Count; j++) {
                int id = activeCountList[j];

                GameObject slot = Instantiate(Resources.Load(activeCountSlotPath)) as GameObject;
                slot.transform.SetParent(content.transform);
                slot.transform.localScale = new Vector2(1, 1);
                slot.name = "ActiveQuestCountSlot" + id.ToString();
                string title = "";
                int langId = PlayerPrefs.GetInt("langId");
                if (langId == 2) {
                    title = questCountMst.param[id].titleEng;
                }else {
                    title = questCountMst.param[id].title;
                }
                string exp = "";
                if (langId == 2) {
                    exp = questCountMst.param[id].expEng;
                }else {
                    exp = questCountMst.param[id].exp;
                }
                string target = questCountMst.param[id].target;
                int amnt = questCountMst.param[id].amnt;
                bool daily = questCountMst.param[id].daily;
                string criteriaTyp = questCountMst.param[id].criteriaTyp;
                int criteria = questCountMst.param[id].criteria;
                
                string key = "";
                if (daily) {
                    key = "questDailyCountReceivedFlg" + id.ToString();
                }else {
                    key = "questSpecialCountReceivedFlg" + id.ToString();
                }

                int count = 0;
                if(criteriaTyp== "movieCount") {
                    count = PlayerPrefs.GetInt("movieCount");
                }else if(criteriaTyp== "zukanBusyoHstCount") {
                    string zukanBusyoHst = PlayerPrefs.GetString("zukanBusyoHst");
                    List<string> zukanBusyoHstList = new List<string>();
                    char[] delimiterChars = { ',' };
                    if (zukanBusyoHst != null && zukanBusyoHst != "") {
                        if (zukanBusyoHst.Contains(",")) {
                            zukanBusyoHstList = new List<string>(zukanBusyoHst.Split(delimiterChars));
                        }else {
                            zukanBusyoHstList.Add(zukanBusyoHst);
                        }
                    }
                    count = zukanBusyoHstList.Count;
                }else if (criteriaTyp == "gameClearDaimyoCount") {
                    string gameClearDaimyo = PlayerPrefs.GetString("gameClearDaimyo");
                    List<string> gameClearDaimyoList = new List<string>();
                    char[] delimiterChars = { ',' };
                    if (gameClearDaimyo != null && gameClearDaimyo != "") {
                        if (gameClearDaimyo.Contains(",")) {
                            gameClearDaimyoList = new List<string>(gameClearDaimyo.Split(delimiterChars));
                        }
                        else {
                            gameClearDaimyoList.Add(gameClearDaimyo);
                        }
                    }

                    string gameClearDaimyoHard = PlayerPrefs.GetString("gameClearDaimyoHard");
                    List<string> gameClearDaimyoHardList = new List<string>();
                    if (gameClearDaimyoHard != null && gameClearDaimyoHard != "") {
                        if (gameClearDaimyoHard.Contains(",")) {
                            gameClearDaimyoHardList = new List<string>(gameClearDaimyoHard.Split(delimiterChars));
                        }else {
                            gameClearDaimyoHardList.Add(gameClearDaimyoHard);
                        }
                    }
                    count = gameClearDaimyoList.Count + gameClearDaimyoHardList.Count;
                }
                
                GameObject itemImage = slot.transform.FindChild("itemImage").gameObject;
                GameObject itemQty = slot.transform.FindChild("itemQty").gameObject;
                GameObject titleValue = slot.transform.FindChild("titleValue").gameObject;
                GameObject expValue = slot.transform.FindChild("expValue").gameObject;

                if (target == "busyoDama") {
                    string imagePath = "Prefabs/Common/Sprite/busyoDama";
                    itemImage.GetComponent<Image>().sprite =
                        Resources.Load(imagePath, typeof(Sprite)) as Sprite;
                }else {
                    string imagePath = "Prefabs/Common/Sprite/money";
                    itemImage.GetComponent<Image>().sprite =
                        Resources.Load(imagePath, typeof(Sprite)) as Sprite;
                }

                itemQty.GetComponent<Text>().text = amnt.ToString();
                titleValue.GetComponent<Text>().text = title;
                expValue.GetComponent<Text>().text = exp;

                slot.transform.FindChild("ReceiveButton").transform.FindChild("count").GetComponent<Text>().text = count.ToString() + "/" + criteria.ToString();
                QuestReceive btnScript = slot.transform.FindChild("ReceiveButton").GetComponent<QuestReceive>();
                btnScript.criteria = criteria;
                btnScript.count = count;
                btnScript.id = id;
                btnScript.key = key;
                btnScript.target = target;
                btnScript.amnt = amnt;
                btnScript.slot = slot;

            }


            //create slot
            QuestInfo questScript = new QuestInfo();
            string activeSlotPath = "Prefabs/Map/quest/ActiveQuestSlot";
            for (int j = 0; j < activeList.Count; j++) {
                int id = activeList[j];
                GameObject slot = Instantiate(Resources.Load(activeSlotPath)) as GameObject;
                slot.transform.SetParent(content.transform);
                slot.transform.localScale = new Vector2(1, 1);
                slot.name = "ActiveQuestSlot" + id.ToString();

                string title = questScript.getQuestTitle(id);
                string exp = questScript.getQuestExp(id);
                string target = questScript.getQuestTarget(id);
                int amnt = questScript.getQuestAmnt(id);
                bool daily = questScript.getQuestDaily(id);
                string key = "";
                if (daily) {
                    key = "questDailyReceivedFlg" + id.ToString();
                }
                else {
                    key = "questSpecialReceivedFlg" + id.ToString();
                }

                GameObject itemImage = slot.transform.FindChild("itemImage").gameObject;
                GameObject itemQty = slot.transform.FindChild("itemQty").gameObject;
                GameObject titleValue = slot.transform.FindChild("titleValue").gameObject;
                GameObject expValue = slot.transform.FindChild("expValue").gameObject;

                if (target == "busyoDama") {
                    string imagePath = "Prefabs/Common/Sprite/busyoDama";
                    itemImage.GetComponent<Image>().sprite =
                        Resources.Load(imagePath, typeof(Sprite)) as Sprite;
                }
                else {
                    string imagePath = "Prefabs/Common/Sprite/money";
                    itemImage.GetComponent<Image>().sprite =
                        Resources.Load(imagePath, typeof(Sprite)) as Sprite;
                }

                itemQty.GetComponent<Text>().text = amnt.ToString();
                titleValue.GetComponent<Text>().text = title;
                expValue.GetComponent<Text>().text = exp;

                QuestReceive btnScript = slot.transform.FindChild("ReceiveButton").GetComponent<QuestReceive>();
                btnScript.id = id;
                btnScript.key = key;
                btnScript.target = target;
                btnScript.amnt = amnt;
                btnScript.slot = slot;

            }

            //Show Inactive QuestSlot
            string inactiveSlotPath = "Prefabs/Map/quest/InactiveQuestSlot";
            for (int k = 0; k < inactiveList.Count; k++) {
                int id = inactiveList[k];

                GameObject slot = Instantiate(Resources.Load(inactiveSlotPath)) as GameObject;
                slot.transform.SetParent(content.transform);
                slot.transform.localScale = new Vector2(1, 1);
                slot.name = "InactiveQuestSlot" + id.ToString();

                string title = questScript.getQuestTitle(id);
                string exp = questScript.getQuestExp(id);
                string target = questScript.getQuestTarget(id);
                int amnt = questScript.getQuestAmnt(id);

                GameObject itemImage = slot.transform.FindChild("itemImage").gameObject;
                GameObject itemQty = slot.transform.FindChild("itemQty").gameObject;
                GameObject titleValue = slot.transform.FindChild("titleValue").gameObject;
                GameObject expValue = slot.transform.FindChild("expValue").gameObject;

                if (target == "busyoDama") {
                    string imagePath = "Prefabs/Common/Sprite/busyoDama";
                    itemImage.GetComponent<Image>().sprite =
                        Resources.Load(imagePath, typeof(Sprite)) as Sprite;
                }
                else {
                    string imagePath = "Prefabs/Common/Sprite/money";
                    itemImage.GetComponent<Image>().sprite =
                        Resources.Load(imagePath, typeof(Sprite)) as Sprite;
                }

                itemQty.GetComponent<Text>().text = amnt.ToString();
                titleValue.GetComponent<Text>().text = title;
                expValue.GetComponent<Text>().text = exp;

            }

            //Scroll Position
            ScrollRect scrollRect = scrollObj.GetComponent<ScrollRect>();
            scrollRect.verticalNormalizedPosition = 1;
        }
    }


    public bool getQuestCount(int id, bool daily) {
        Entity_quest_count_mst questMst = Resources.Load("Data/quest_count_mst") as Entity_quest_count_mst;
        bool dailyTmp = questMst.param[id].daily;
        if(dailyTmp == daily) {

        }
        return daily;
    }
}
