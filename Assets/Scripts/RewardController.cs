using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class RewardController : MonoBehaviour {


    void Start() {
        DataReward DataReward = GameObject.Find("DataStore").GetComponent<DataReward>();
        GameObject content = GameObject.Find("Content").gameObject;

        for(int i=0; i< DataReward.itemTitleList.Count; i++) {
            string objectId = DataReward.objectIdList[i];
            string title = DataReward.itemTitleList[i];
            string grp = DataReward.itemGrpList[i];
            string rank = DataReward.itemRankList[i];
            int qty = DataReward.itemQtyList[i];
            
            string slotPath = "Prefabs/PvP/RewardSlot";
            GameObject slot = Instantiate(Resources.Load(slotPath)) as GameObject;
            slot.transform.SetParent(content.transform);
            slot.transform.localScale = new Vector2(1,1);

            //view
            slot.transform.FindChild("title").GetComponent<Text>().text = title;

            //hide other image
            GameObject imageContent = slot.transform.FindChild("ScrollView").transform.FindChild("Viewport").transform.FindChild("Content").gameObject;
            RewardReceive RewardReceive = slot.transform.FindChild("button").GetComponent<RewardReceive>();

            foreach (Transform obj in imageContent.transform) {
                if(obj.name != grp) {
                    Destroy(obj.gameObject);
                }else {
                    if (grp == "money") {
                        obj.transform.FindChild("qty").GetComponent<Text>().text = qty.ToString();
                    }
                    else if (grp == "stone") {
                        obj.transform.FindChild("qty").GetComponent<Text>().text = qty.ToString();
                    }
                    else if (grp == "busyo") {
                        obj.transform.FindChild("rank").GetComponent<Text>().text = rank;
                        BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
                        int busyoId = BusyoInfoGet.getRandomBusyoId(rank);
                        RewardReceive.busyoId = busyoId;
                    }
                    else if (grp == "kaho") {
                        obj.transform.FindChild("qty").GetComponent<Text>().text = "x " + qty.ToString();
                        obj.transform.FindChild("rank").GetComponent<Text>().text = rank;

                        //kahouType bugu,kabuto,gusoku,meiba,cyadougu,heihousyo,chishikisyo
                        for(int j=0; j<qty; j++) {
                            List<string> kahouuTypeList = new List<string>() {"bugu", "kabuto", "gusoku", "meiba", "cyadougu", "heihousyo", "chishikisyo" };
                            int rdmId = UnityEngine.Random.Range(0, kahouuTypeList.Count);
                            string kahoTyp = kahouuTypeList[rdmId];

                            Kahou Kahou = new Kahou();
                            int kahoId = Kahou.getRamdomKahouId(kahoTyp, rank);
                            RewardReceive.kahoTypList.Add(kahoTyp);
                            RewardReceive.kahoIdList.Add(kahoId);
                            RewardReceive.kahoNameList.Add(Kahou.getKahouName(kahoTyp, kahoId));
                        }
                    }
                    else if (grp == "syokaijyo") {
                        string rankTmp = "";
                        if (Application.systemLanguage != SystemLanguage.Japanese) {
                            if (rank=="S") {
                                rankTmp = "High";
                            }else if(rank=="A") {
                                rankTmp = "Mid";
                            }else if(rank=="B") {
                                rankTmp = "Low";
                            }
                        }else {
                            if (rank == "S") {
                                rankTmp = "上";
                            }else if (rank == "A") {
                                rankTmp = "中";
                            }else if (rank == "B") {
                                rankTmp = "下";
                            }
                        }
                        obj.transform.FindChild("qty").GetComponent<Text>().text = "x " + qty.ToString();
                        obj.transform.FindChild("rank").GetComponent<Text>().text = rankTmp;
                    }else if(grp.Contains("jinkei")) {


                    }
                }
            }

            //Set Value           
            RewardReceive.slot = slot;
            RewardReceive.objectId = objectId;
            RewardReceive.grp = grp;
            RewardReceive.qty = qty;
            RewardReceive.rank = rank;
        }


    }

   
}
