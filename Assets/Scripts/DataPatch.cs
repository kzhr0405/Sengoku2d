using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DataPatch : MonoBehaviour {

    public void DataPatch1() {

        //Money Overflow
        int money = PlayerPrefs.GetInt("money");
        if(money<0) {
            PlayerPrefs.SetInt("money", int.MaxValue);
            PlayerPrefs.Flush();
        }

        //Kenshin Hired
        string userId = PlayerPrefs.GetString("userId");
        if(userId == "jz4kb9e7va20170311171713" || userId == "uiyzg1yw3h20170301124246") {

            //patch
            bool DataPatch1 = PlayerPrefs.GetBool("DataPatch1");

            if(!DataPatch1) {
                string myBusyo = PlayerPrefs.GetString("myBusyo");
                char[] delimiterChars = { ',' };

                List<string> oldMyBusyoList = new List<string>();
                List<string> newMyBusyoList = new List<string>();
                if (myBusyo.Contains(",")) {
                    oldMyBusyoList = new List<string>(myBusyo.Split(delimiterChars));
                }else {
                    oldMyBusyoList.Add(myBusyo);
                }
                for(int i=0; i< oldMyBusyoList.Count; i++) {
                    string tmpBusyo = oldMyBusyoList[i];
                    if(!newMyBusyoList.Contains(tmpBusyo)) {
                        newMyBusyoList.Add(tmpBusyo);
                    }
                }
                string newMyBusyo = "";
                for(int j=0; j< newMyBusyoList.Count; j++) {
                    string tmpBusyo = newMyBusyoList[j];
                    if (j==0) {
                        newMyBusyo = tmpBusyo;
                    }else {
                        newMyBusyo = newMyBusyo + "," + tmpBusyo;
                    }
                }
                PlayerPrefs.SetString("myBusyo", newMyBusyo);

                string busyoId = "1";
                PlayerPrefs.SetInt(busyoId, 1);

                string hei = "hei" + busyoId;
                string heiValue = "KB:1:1:1";
                PlayerPrefs.SetString(hei, heiValue);

                string senpou = "senpou" + busyoId;
                PlayerPrefs.SetInt(senpou, 1); //Lv

                string saku = "saku" + busyoId;
                PlayerPrefs.SetInt(saku, 1); //Lv

                string kahou = "kahou" + busyoId;
                PlayerPrefs.SetString(kahou, "0,0,0,0,0,0,0,0");

                string exp = "exp" + busyoId;
                PlayerPrefs.SetInt(exp, 0);

                //Add Qty
                PlayerPrefs.SetInt("myBusyoQty", newMyBusyoList.Count);

                //Flg
                PlayerPrefs.SetBool("DataPatch1",true);
                PlayerPrefs.Flush();
            }
        }



    }
}
