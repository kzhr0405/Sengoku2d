using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class BusyoInfoGet : MonoBehaviour {

	Entity_busyo_mst busyoMst  = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;

	public string getName(int busyoId, int langId) {
        string name = "";        
        if (langId == 2) {
            name = busyoMst.param[busyoId - 1].nameEng;
        } else if(langId==3) {
            name = busyoMst.param[busyoId - 1].nameSChn;
        } else {
            name = busyoMst.param[busyoId - 1].name;
        }
		return name;
	}

	public string getHeisyu(int busyoId){
		string heisyu = busyoMst.param[busyoId - 1].heisyu;
		return heisyu;
	}

	public int getMaxDfc(int busyoId){
		int maxDfc = busyoMst.param[busyoId - 1].dfc;
		return maxDfc;
	}

	public int getMaxAtk(int busyoId){
		int maxAtk = busyoMst.param[busyoId - 1].atk;
		return maxAtk;
	}

	public string getRank(int busyoId){
		string rank = busyoMst.param[busyoId - 1].rank;
		return rank;
	}

	public string getDaimyoBusyoQtyHeisyu(int daimyoId, int langId) {	//return busyo qty,the largest heisyu
		string qtyAndHeisyu = "";
		int qty = 0;
		int yrQty = 0;
		int kbQty = 0;
		int tpQty = 0;
		int ymQty = 0;

		//metsubou
		string metsubouTemp = "metsubou" + daimyoId.ToString();
		List<string> metsubouDaimyoList = new List<string> ();

		string metsubouDaimyoString = PlayerPrefs.GetString (metsubouTemp);
		char[] delimiterChars = {','};
		if (metsubouDaimyoString != null && metsubouDaimyoString != "") {
			if (metsubouDaimyoString.Contains (",")) {
				metsubouDaimyoList = new List<string> (metsubouDaimyoString.Split (delimiterChars));
			} else {
				metsubouDaimyoList = new List<string> (metsubouDaimyoString.Split (delimiterChars));
			}
		}
		metsubouDaimyoList.Add (daimyoId.ToString ());

		for (int i=0; i<busyoMst.param.Count; i++) {			
			int tmpDaimyoId = busyoMst.param [i].daimyoId;
			if (metsubouDaimyoList.Contains (tmpDaimyoId.ToString ())) {
				qty = qty + 1;

				string heisyu = busyoMst.param [i].heisyu;
				if (heisyu == "YR") {
					yrQty = yrQty + 1;
				} else if (heisyu == "KB") {
					kbQty = kbQty + 1;
				} else if (heisyu == "TP") {
					tpQty = tpQty + 1;
				} else if (heisyu == "YM") {
					ymQty = ymQty + 1;
				}
			}
		}

        //Make return value
        if (langId == 2) {
            if (yrQty >= kbQty && yrQty >= tpQty && yrQty >= ymQty) {
			    qtyAndHeisyu = qty.ToString () + ",Spear";
		    } else if (ymQty >= yrQty && ymQty >= kbQty && ymQty >= tpQty) {
			    qtyAndHeisyu = qty.ToString () + ",Bow";
		    } else if (kbQty >= yrQty && kbQty >= tpQty && kbQty >= ymQty) {
			    qtyAndHeisyu = qty.ToString () + ",Cavalry";
		    } else if (tpQty >= yrQty && tpQty >= kbQty && tpQty >= ymQty) {
			    qtyAndHeisyu = qty.ToString () + ",Gun";
		    }
        }else {
            if (yrQty >= kbQty && yrQty >= tpQty && yrQty >= ymQty) {
                qtyAndHeisyu = qty.ToString() + ",槍隊";
            }else if (ymQty >= yrQty && ymQty >= kbQty && ymQty >= tpQty) {
                qtyAndHeisyu = qty.ToString() + ",弓隊";
            }else if (kbQty >= yrQty && kbQty >= tpQty && kbQty >= ymQty) {
                qtyAndHeisyu = qty.ToString() + ",騎馬隊";
            }else if (tpQty >= yrQty && tpQty >= kbQty && tpQty >= ymQty) {
                qtyAndHeisyu = qty.ToString() + ",鉄砲隊";
            }
        }
		return qtyAndHeisyu;
	}

	public int getDaimyoId(int busyoId, int senarioId) {

        int daimyoId = 0;
        if (senarioId == 1) {
            daimyoId = busyoMst.param[busyoId - 1].daimyoId1;
        }else if (senarioId == 2) {
            daimyoId = busyoMst.param[busyoId - 1].daimyoId2;
        }else if (senarioId == 3) {
            daimyoId = busyoMst.param[busyoId - 1].daimyoId3;
        }else {
            daimyoId = busyoMst.param[busyoId - 1].daimyoId;
        }        
        return daimyoId;
	}

	public int getDaimyoHst(int busyoId, int senarioId){
        int daimyoHst = 0;
        if (senarioId == 1) {
            daimyoHst = busyoMst.param[busyoId - 1].daimyoHst1;
        }else if (senarioId == 2) {
            daimyoHst = busyoMst.param[busyoId - 1].daimyoHst2;
        }else if (senarioId == 3) {
            daimyoHst = busyoMst.param[busyoId - 1].daimyoHst3;
        }else {
            daimyoHst = busyoMst.param[busyoId - 1].daimyoHst;
        }        
        return daimyoHst;
	}

    public int getShipId(int busyoId) {
        int shipId = busyoMst.param[busyoId - 1].ship;
        return shipId;
    }

    public int getSenpouId(int busyoId) {
        int senpou_id = busyoMst.param[busyoId - 1].senpou_id;
        return senpou_id;
    }

    public int getSakuId(int busyoId) {
        int saku_id = busyoMst.param[busyoId - 1].saku_id;
        return saku_id;
    }

    public int getRandomBusyoId(string rank) {
        int busyoId = 0;
        List<int> randomBusyoList = new List<int>();

        for (int i= 0; i < busyoMst.param.Count; i++) {
            if(rank == busyoMst.param[i].rank) {
                randomBusyoList.Add(busyoMst.param[i].id);
            }
        }
        int rdmId = UnityEngine.Random.Range(0, randomBusyoList.Count);
        busyoId = randomBusyoList[rdmId];

        return busyoId;
    }

    public List<int> getDaimyoBusyoList(int senarioId, List<string>daimyoList, int daimyoBusyoId) {
        List<int> busyoList = new List<int>();
        for(int i=0; i<busyoMst.param.Count; i++) {
            if(senarioId==1) {
                int daimyoIdtmp = busyoMst.param[i].daimyoId1;
                if(daimyoIdtmp != 0) {
                    if(daimyoList.Contains(daimyoIdtmp.ToString())) {
                        int busyoId = busyoMst.param[i].id;
                        if (daimyoBusyoId!= busyoId) busyoList.Add(busyoId);
                    }
                }
            }else if (senarioId == 2) {
                int daimyoIdtmp = busyoMst.param[i].daimyoId2;
                if (daimyoIdtmp != 0) {
                    if (daimyoList.Contains(daimyoIdtmp.ToString())) {
                        int busyoId = busyoMst.param[i].id;
                        if (daimyoBusyoId != busyoId) busyoList.Add(busyoId);
                    }
                }
            }else if (senarioId == 3) {
                int daimyoIdtmp = busyoMst.param[i].daimyoId3;
                if (daimyoIdtmp != 0) {
                    if (daimyoList.Contains(daimyoIdtmp.ToString())) {
                        int busyoId = busyoMst.param[i].id;
                        if (daimyoBusyoId != busyoId) busyoList.Add(busyoId);
                    }
                }
            }else {
                int daimyoIdtmp = busyoMst.param[i].daimyoId;
                if (daimyoIdtmp != 0) {
                    if (daimyoList.Contains(daimyoIdtmp.ToString())) {
                        int busyoId = busyoMst.param[i].id;
                        if (daimyoBusyoId != busyoId) busyoList.Add(busyoId);
                    }
                }
            }
        }
        
        return busyoList;
    }

    public int getRandomBusyo(int activeDaimyoId, int daimyoBusyoId, int senarioId) {

        /*Busyo Master Setting Start*/
        //Active Busyo List
        List<string> metsubouDaimyoList = new List<string>();
        string metsubouTemp = "metsubou" + activeDaimyoId;
        string metsubouDaimyoString = PlayerPrefs.GetString(metsubouTemp);
        char[] delimiterChars2 = { ',' };
        if (metsubouDaimyoString != null && metsubouDaimyoString != "") {
            if (metsubouDaimyoString.Contains(",")) {
                metsubouDaimyoList = new List<string>(metsubouDaimyoString.Split(delimiterChars2));
            }else {
                metsubouDaimyoList = new List<string>(metsubouDaimyoString.Split(delimiterChars2));
            }
        }
        metsubouDaimyoList.Add(activeDaimyoId.ToString());

        List<int> busyoList = getDaimyoBusyoList(senarioId, metsubouDaimyoList, daimyoBusyoId);
        /*Busyo Master Setting End*/

        /*Random Shuffle*/
        for (int i = 0; i < busyoList.Count; i++) {
            int temp = busyoList[i];
            int randomIndex = UnityEngine.Random.Range(0, busyoList.Count);
            busyoList[i] = busyoList[randomIndex];
            busyoList[randomIndex] = temp;
        }


        int returnValue = 0;
        if (busyoList.Count == 0) {
            returnValue = 35;
        }
        else {
            returnValue = busyoList[0];
        }


        return returnValue;

    }

}
