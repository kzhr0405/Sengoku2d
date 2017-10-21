using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class GacyaSpecialTouyouConfirm : MonoBehaviour {

    public GameObject board;
    public GameObject back;
    
    public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();

        if (name=="Yes") {
            audioSources[0].Play();
            audioSources[0].Play();
            audioSources[7].Play();
            audioSources[3].Play();

            GameObject Content = GameObject.Find("Content").gameObject;
            List<int> touyouBusyoList = new List<int>();
            foreach(Transform slot in Content.transform) {
                GacyaSpecialSelect GacyaSpecialSelect = slot.GetComponent<GacyaSpecialSelect>();
                if (GacyaSpecialSelect.selectFlg) {
                    touyouBusyoList.Add(GacyaSpecialSelect.busyoId);
                }
            }
            DoTouyou DoTouyou = new DoTouyou();        
            BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
            List<int> messageIdList = new List<int>();
            foreach (int busyoId in touyouBusyoList) {
                int myBusyoQty = PlayerPrefs.GetInt("myBusyoQty");
                messageIdList.Add(DoTouyou.doTouyou(myBusyoQty, busyoId, BusyoInfoGet.getRank(busyoId)));
            }
            int id1 = 0;
            int id2 = 0;
            int id3 = 0;
            int id4 = 0;
            int id5 = 0;

            foreach (int messageId in messageIdList) {
                if(messageId==1) {
                    id1++;
                }else if (messageId == 2) {
                    id2++;
                }else if (messageId == 3) {
                    id3++;
                }else if (messageId == 4) {
                    id4++;
                }else if (messageId == 5) {
                    id5++;
                }
            }
            List<string> messageList = new List<string>();

            int langId = PlayerPrefs.GetInt("langId");
           
                //5
            if (id5 != 0) {
                if (langId == 2) {
                    messageList.Add(id5 + " new samurai joined your clan.\n");
                }else {
                    messageList.Add(id5 + "人の武将を新規に登用。\n");
                }
            }
            //4
            if (id4 != 0) {
                if (langId == 2) {
                    messageList.Add(id4 + " samurai joined your clan again.\n");
                }else {
                    messageList.Add(id4 + "人の武将を改めて登用。\n");
                }
            }
            //3.
            if (id3 != 0) {
                if (langId == 2) {
                    messageList.Add(id3 + " samurai increased Lv.\n");
                }else {
                    messageList.Add(id3 + "人の武将が1レベルアップ。\n");
                }
            }
            //2.
            if (id2 != 0) {
                int gacyaSpecialBusyoDama = PlayerPrefs.GetInt("gacyaSpecialBusyoDama");
                PlayerPrefs.DeleteKey("gacyaSpecialBusyoDama");
                PlayerPrefs.Flush();
                if (langId == 2) {
                    messageList.Add(id2 + " samurai archived max lv.You got " + gacyaSpecialBusyoDama + " stone.\n");
                }else {
                    messageList.Add(id2 + "人が最大レベルに到達し、武将珠を" + gacyaSpecialBusyoDama +"入手。\n");
                }
                GameObject.Find("BusyoDamaValue").GetComponent<Text>().text = PlayerPrefs.GetInt("busyoDama").ToString();
            }
            //1.
            if (id1 !=0) {
                if (langId == 2) {
                    messageList.Add(id1 + " samurai increased max Lv.\n");
                }else {
                    messageList.Add(id1 + "人の武将の最大レベルが1上昇。\n");
                }
            }
                
            Message msg = new Message();
            string summmary = "";
            for(int i=0; i<messageList.Count; i++) {
                string messageText = messageList[i];
                if(i==messageList.Count -1) messageText = messageText.Replace("\n", "");
                summmary = summmary + messageText;
            }
            msg.makeMidMessage(summmary);

            Destroy(GameObject.Find("Busyo").transform.parent.gameObject);
        }else {
            audioSources[1].Play();
        }
        GameObject.Find("Canvas").GetComponent<Canvas>().sortingLayerName = "Default";
        Destroy(board);
        Destroy(back);
        
    }
}
