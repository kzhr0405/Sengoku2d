using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using NCMB;

public class RewardReceive : MonoBehaviour {

    public GameObject slot;
    public string objectId;
    public string grp;
    public string rank;
    public int qty;
    public string value;

    public int busyoId;
    public List<string> kahoTypList;
    public List<int> kahoIdList;
    public List<string> kahoNameList;

    public void OnClick() {

        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();

        if (Application.internetReachability == NetworkReachability.NotReachable) {
            //接続されていないときの処理
            Message msg = new Message();
            msg.makeMessage(msg.getMessage(136));
            audioSources[4].Play();

        } else {
            
            audioSources[3].Play();

            //NCMB delete
            NCMBObject query = new NCMBObject("reward");
            query.ObjectId = objectId;
            query.DeleteAsync();

            //List delete
            DataReward DataRewardObj = GameObject.Find("DataStore").GetComponent<DataReward>();
            int line = 0;
            for(int i=0; i< DataRewardObj.objectIdList.Count; i++) {
                if(objectId == DataRewardObj.objectIdList[i]) {
                    line = i;
                }
            }
            DataRewardObj.objectIdList.RemoveAt(line);
            DataRewardObj.itemTitleList.RemoveAt(line);
            DataRewardObj.itemGrpList.RemoveAt(line);
            DataRewardObj.itemRankList.RemoveAt(line);
            DataRewardObj.itemQtyList.RemoveAt(line);

            //visual delete
            Destroy(slot);

            //item register
            if (grp == "money") {
                Message msgScript = new Message();
                string msg = "";
                int money = PlayerPrefs.GetInt("money");
                int newMoney = money + qty;
                if (newMoney < 0) {
                    newMoney = int.MaxValue;
                }
                PlayerPrefs.SetInt("money", newMoney);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    msg = "You got " + qty + " money.";
                }else {
                    msg = "金を" + qty + "受領しました。";
                }
                msgScript.makeMessage(msg);
            }
            else if (grp == "stone") {
                Message msgScript = new Message();
                string msg = "";
                int busyoDama = PlayerPrefs.GetInt("busyoDama");
                int newBusyoDama = busyoDama + qty;
                PlayerPrefs.SetInt("busyoDama", newBusyoDama);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    msg = "You got " + qty + " stone.";
                }else {
                    msg = "武将珠を" + qty + "個受領しました。";
                }
                msgScript.makeMessage(msg);
            }
            else if(grp == "busyo") {
                audioSources[7].Play();
                receiveBusyo(busyoId);
            }
            else if (grp == "kaho") {
                receiveKaho();

            }
            else if (grp == "syokaijyo") {
                receiveShokaijyo(rank, qty);
            }
            else if(grp == "shiro") {
                Shiro shiro = new Shiro();
                int shiroId = shiro.getRandomId();
                string shiroName = shiro.getName(shiroId);
                Message msgScript = new Message();
                string msg = "";
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    msg = "You got " + shiroName + ". You can enhance your castle in town development.";
                }else {
                    msg = "天下の要害、" + shiroName + "を築城できますぞ。内政で城を増強しましょう。";
                }
                msgScript.makeMessage(msg);
                shiro.registerShiro(shiroId);
            }
            else if (grp == "jinkei") {
                if (rank == "1") {
                    PlayerPrefs.SetBool("addJinkei1", true);
                }else if (rank == "2") {
                    PlayerPrefs.SetBool("addJinkei2", true);
                }else if (rank == "3") {
                    PlayerPrefs.SetBool("addJinkei3", true);
                }else if (rank == "4") {
                    PlayerPrefs.SetBool("addJinkei4", true);
                }
                Message msgScript = new Message();
                string msg = "";
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    msg = "You got an additional formation.";
                }else {
                    msg = "追加陣形を受領しました。";
                }
                msgScript.makeMessage(msg);

            }else {
                PlayerPrefs.SetInt(grp, qty);
                Message msgScript = new Message();
                string msg = "";
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    msg = "You got " + qty.ToString() + " " + grp + ".";
                }else {
                    msg = grp + "を" + qty + "入手しました。";
                }
                msgScript.makeMessage(msg);
            }
            PlayerPrefs.Flush();
        }
    }

    public void receiveKaho() {
        
        Message msgScript = new Message();
        Kahou kahou = new Kahou();
        string kahouMsg = "";

        for(int i=0; i< kahoTypList.Count; i++) {
            string kahoTyp = kahoTypList[i];
            int kahoId = kahoIdList[i];
            string kahoName = kahoNameList[i];

            if (kahoTyp == "bugu") {
                kahou.registerBugu(kahoId);
            }
            else if (kahoTyp == "gusoku") {
                kahou.registerGusoku(kahoId);
            }
            else if (kahoTyp == "kabuto") {
                kahou.registerKabuto(kahoId);
            }
            else if (kahoTyp == "meiba") {
                kahou.registerMeiba(kahoId);
            }
            else if (kahoTyp == "cyadougu") {
                kahou.registerCyadougu(kahoId);
            }
            else if (kahoTyp == "chishikisyo") {
                kahou.registerChishikisyo(kahoId);
            }
            else if (kahoTyp == "heihousyo") {
                kahou.registerHeihousyo(kahoId);
            }

            if(kahouMsg == "") {
                kahouMsg = kahoName;
            }else {
                kahouMsg = kahouMsg + "," + kahoName;
            }

        }

        string msg = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            msg = "You got " + kahouMsg + ".";
        }else {
            msg = kahouMsg + "を受領しましたぞ。良い物を手に入れられましたな。";
        }
        msgScript.makeMessage(msg);
    }

    public void receiveShokaijyo(string rank, int qty) {

        Message msgScript = new Message();
        int cyouteiFlg = UnityEngine.Random.Range(0, 2);
        string msg = "";
        if (cyouteiFlg == 1) {
            //cyotei
            string nowQty = PlayerPrefs.GetString("cyoutei");
            List<string> nowQtyList = new List<string>();
            char[] delimiterChars = { ',' };
            nowQtyList = new List<string>(nowQty.Split(delimiterChars));
            string newQty = "";
            if (rank == "B") {
                int newUnitQty = int.Parse(nowQtyList[0]);
                newUnitQty = newUnitQty + qty;
                newQty = newUnitQty + "," + nowQtyList[1] + "," + nowQtyList[2];
            }
            else if (rank == "A") {
                int newUnitQty = int.Parse(nowQtyList[1]);
                newUnitQty = newUnitQty + qty;
                newQty = nowQtyList[0] + "," + newUnitQty + "," + nowQtyList[2];
            }
            else if (rank == "S") {
                int newUnitQty = int.Parse(nowQtyList[2]);
                newUnitQty = newUnitQty + qty;
                newQty = nowQtyList[0] + "," + nowQtyList[1] + "," + newUnitQty;
            }
            PlayerPrefs.SetString("cyoutei", newQty);

            if (Application.systemLanguage != SystemLanguage.Japanese) {
                msg = "You got letter of introduction for the imperial court.";
            }else {
                msg = "朝廷への紹介状を入手しましたぞ。";
            }

        } else {
            //koueki
            string nowQty = PlayerPrefs.GetString("koueki");
            List<string> nowQtyList = new List<string>();
            char[] delimiterChars = { ',' };
            nowQtyList = new List<string>(nowQty.Split(delimiterChars));

            string newQty = "";
            if (rank == "B") {
                int newUnitQty = int.Parse(nowQtyList[0]);
                newUnitQty = newUnitQty + qty;
                newQty = newUnitQty + "," + nowQtyList[1] + "," + nowQtyList[2];
            }
            else if (rank == "A") {
                int newUnitQty = int.Parse(nowQtyList[1]);
                newUnitQty = newUnitQty + qty;
                newQty = nowQtyList[0] + "," + newUnitQty + "," + nowQtyList[2];
            }
            else if (rank == "S") {
                int newUnitQty = int.Parse(nowQtyList[2]);
                newUnitQty = newUnitQty + qty;
                newQty = nowQtyList[0] + "," + nowQtyList[1] + "," + newUnitQty;
            }
            PlayerPrefs.SetString("koueki", newQty);

            if (Application.systemLanguage != SystemLanguage.Japanese) {
                msg = "You got letter of introduction for merchant.";
            }else {
                msg = "豪商への紹介状を入手しましたぞ。";
            }

        }
        
        msgScript.makeMessage(msg);

    }

    public void receiveBusyo(int busyoId) {

        //Common
        BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
        string busyoName = BusyoInfoGet.getName(busyoId);
        string heisyu = BusyoInfoGet.getHeisyu(busyoId);
        string rank = BusyoInfoGet.getRank(busyoId);
        int myBusyoQty = PlayerPrefs.GetInt("myBusyoQty");

        //Tracking
        int TrackNewBusyoHireNo = PlayerPrefs.GetInt("TrackNewBusyoHireNo", 0);
        TrackNewBusyoHireNo = TrackNewBusyoHireNo + 1;
        PlayerPrefs.SetInt("TrackNewBusyoHireNo", TrackNewBusyoHireNo);

        //Add zukan & gacya History Start
        string zukanBusyoHst = PlayerPrefs.GetString("zukanBusyoHst");
        if (zukanBusyoHst != null && zukanBusyoHst != "") {
            zukanBusyoHst = zukanBusyoHst + "," + busyoId.ToString();
        }else {
            zukanBusyoHst = busyoId.ToString();
        }
        PlayerPrefs.SetString("zukanBusyoHst", zukanBusyoHst);

        //Daimyo Busyo History
        Daimyo daimyo = new Daimyo();
        if (daimyo.daimyoBusyoCheck(busyoId)) {
            string gacyaDaimyoHst = PlayerPrefs.GetString("gacyaDaimyoHst");
            if (gacyaDaimyoHst != null && gacyaDaimyoHst != "") {
                gacyaDaimyoHst = gacyaDaimyoHst + "," + busyoId.ToString();
            }
            else {
                gacyaDaimyoHst = busyoId.ToString();
            }
            PlayerPrefs.SetString("gacyaDaimyoHst", gacyaDaimyoHst);
        }

        //My Busyo Exist Check
        string myBusyoString = PlayerPrefs.GetString("myBusyo");
        char[] delimiterChars = { ',' };
        List<string> myBusyoList = new List<string>();
        char[] delimiterChars2 = { ',' };
        if (myBusyoString.Contains(",")) {
            myBusyoList = new List<string>(myBusyoString.Split(delimiterChars));
        }else {
            myBusyoList.Add(myBusyoString);
        }

        if (myBusyoList.Contains(busyoId.ToString())) {

            //add lv
            string addLvTmp = "addlv" + busyoId.ToString();
            int addLvValue = 0;
            if (PlayerPrefs.HasKey(addLvTmp)) {
                addLvValue = PlayerPrefs.GetInt(addLvTmp);
                addLvValue = addLvValue + 1;
                if (addLvValue >= 100) {
                    addLvValue = 100;
                }
            }else {
                addLvValue = 1;
            }

            if (addLvValue < 100) {
                PlayerPrefs.SetInt(addLvTmp, addLvValue);
                PlayerPrefs.Flush();
                
                MessageBusyo msg = new MessageBusyo();
                string type = "touyou";
                string msgText = "";
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    msgText = "Max Lv of " + busyoName + " increased.";
                }
                else {
                    msgText = busyoName + "の最大レベルが1上がりました。";
                }
                msg.makeMessage(msgText, busyoId, type);
            }else {

                //Lv up
                int currentLv = PlayerPrefs.GetInt(busyoId.ToString());
                int maxLv = 100 + addLvValue;

                int newLv = 0;
                string lvUpText = "";

                //Already Lv Max
                if (currentLv == maxLv) {
                    newLv = currentLv;
                    int busyoDama = 0;
                    if (rank == "S") {
                        busyoDama = 200;
                    }else if (rank == "A") {
                        busyoDama = 50;
                    }else if (rank == "B") {
                        busyoDama = 20;
                    }else if (rank == "C") {
                        busyoDama = 10;
                    }
                    int myBusyoDama = PlayerPrefs.GetInt("busyoDama");
                    myBusyoDama = myBusyoDama + busyoDama;
                    PlayerPrefs.SetInt("busyoDama", myBusyoDama);
                    if (Application.systemLanguage != SystemLanguage.Japanese) {
                        lvUpText = "You got " + busyoDama + " stone.";
                    }else {
                        lvUpText = "武将珠" + busyoDama + "個を贈呈します。";
                    }

                }
                PlayerPrefs.SetInt(busyoId.ToString(), newLv);

                if (currentLv != maxLv) {
                    string exp = "exp" + busyoId.ToString();
                    Exp expCalc = new Exp();
                    int totalExp = expCalc.getExpforNextLv(currentLv);
                    PlayerPrefs.SetInt(exp, totalExp);
                }
                
                MessageBusyo msg = new MessageBusyo();
                string type = "touyou";
                msg.makeMessage(lvUpText, busyoId, type);

            }
        }else {

            int existCheck = PlayerPrefs.GetInt(busyoId.ToString());
            if (existCheck != 0 && existCheck != null) {
                //my Busyo not contain but player used him before daimyo was changed
                if (myBusyoString == null || myBusyoString == "") {
                    myBusyoString = busyoId.ToString();
                }
                else {
                    myBusyoString = myBusyoString + "," + busyoId.ToString();
                }
                PlayerPrefs.SetString("myBusyo", myBusyoString);

                //Add Qty
                myBusyoQty = myBusyoQty + 1;
                PlayerPrefs.SetInt("myBusyoQty", myBusyoQty);
                
                MessageBusyo msg = new MessageBusyo();
                string touyouuText = "";
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    touyouuText = "We hired " + busyoName + ".";
                }else {
                    touyouuText = busyoName + "を登用しました。";
                }
                string type = "touyou";
                msg.makeMessage(touyouuText, busyoId, type);

            }else {
                //Add Completely New Data
                if (myBusyoString == null || myBusyoString == "") {
                    myBusyoString = busyoId.ToString();
                }else {
                    myBusyoString = myBusyoString + "," + busyoId.ToString();
                }
                PlayerPrefs.SetString("myBusyo", myBusyoString);
                PlayerPrefs.SetInt(busyoId.ToString(), 1);

                string hei = "hei" + busyoId.ToString();
                string heiValue = heisyu + ":1:1:1";
                PlayerPrefs.SetString(hei, heiValue);

                string senpou = "senpou" + busyoId.ToString();
                PlayerPrefs.SetInt(senpou, 1); //Lv

                string saku = "saku" + busyoId.ToString();
                PlayerPrefs.SetInt(saku, 1); //Lv

                string kahou = "kahou" + busyoId.ToString();
                PlayerPrefs.SetString(kahou, "0,0,0,0,0,0,0,0");

                string exp = "exp" + busyoId.ToString();
                PlayerPrefs.SetInt(exp, 0);

                //Add Qty
                myBusyoQty = myBusyoQty + 1;
                PlayerPrefs.SetInt("myBusyoQty", myBusyoQty);

                //View Message Box
                Destroy(GameObject.Find("board(Clone)"));
                Destroy(GameObject.Find("Back(Clone)"));

                MessageBusyo msg = new MessageBusyo();
                string touyouuText = "";
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    touyouuText = "We hired " + busyoName + ".";
                }
                else {
                    touyouuText = busyoName + "を登用しました。";
                }
                string type = "touyou";
                msg.makeMessage(touyouuText, busyoId, type);
            }
        }
    }
    
    
        
}
