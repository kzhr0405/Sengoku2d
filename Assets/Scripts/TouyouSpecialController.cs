﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class TouyouSpecialController : MonoBehaviour {

    public GameObject Gacya;
    public GameObject Panel;
    public Dictionary<int, Busyo> busyoListDic;
    public string targetHeisyu;
    public int targetDaimyoId1;
    public int targetDaimyoId2;
    public int targetDaimyoId3;
    public AudioSource[] audioSources;
    public GameObject Slot;
    public GameObject Content;
    public GameObject lightning;
    public GameObject gacyaEffectS;
    public GameObject gacyaEffectA;

    void Start () {
        //Common
        int busyoDama = PlayerPrefs.GetInt("busyoDama");
        int senarioId = PlayerPrefs.GetInt("senarioId");
        GameObject.Find("BusyoDamaValue").GetComponent<Text>().text = busyoDama.ToString();
        Panel = GameObject.Find("Panel").gameObject;
        audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();

        //gacya busyo set
        Entity_busyo_mst busyoMst = Resources.Load("Data/busyo_mst") as Entity_busyo_mst;
        busyoListDic = new Dictionary<int, Busyo>();
        int langId = PlayerPrefs.GetInt("langId");
        for (int i = 0; i < busyoMst.param.Count; i++) {
            int busyoId = busyoMst.param[i].id;            
            string rank = busyoMst.param[i].rank;
            string busyoName = "";           
            if (langId == 2) {
                busyoName = busyoMst.param[i].nameEng;
            }else if(langId==3) {
                busyoName = busyoMst.param[i].nameSChn;
            } else {
                busyoName = busyoMst.param[i].name;
            }
            string heisyu = busyoMst.param[i].heisyu;

            int daimyoId = 0;
            int daimyoHst = 0;
            if (senarioId==1) {
                daimyoId = busyoMst.param[i].daimyoId1;
                daimyoHst = busyoMst.param[i].daimyoHst1;
            }else if(senarioId==2) {
                daimyoId = busyoMst.param[i].daimyoId2;
                daimyoHst = busyoMst.param[i].daimyoHst2;
            }else if (senarioId == 3) {
                daimyoId = busyoMst.param[i].daimyoId3;
                daimyoHst = busyoMst.param[i].daimyoHst3;
            }else{
                daimyoId = busyoMst.param[i].daimyoId;
                daimyoHst = busyoMst.param[i].daimyoHst;
            }
            
            //busyoListDic.Add(busyoId, new Busyo { busyoId = busyoId, busyoName= busyoName, rank = rank, heisyu = heisyu, daimyoId = daimyoId , daimyoHst = daimyoHst });
            busyoListDic.Add(busyoId, new Busyo (busyoId, busyoName, rank, 0,heisyu, daimyoId, daimyoHst,0 ,0,0,0,0,0,0));
        }

        //target
        bool todayGacyaSpecialFlg = PlayerPrefs.GetBool("todayGacyaSpecialFlg");
        //todayGacyaSpecialFlg = false; //test
        if (!todayGacyaSpecialFlg) {
            List<string> targetHeisyuList = new List<string>() { "YR", "KB", "TP", "YM"};
            int rdmHeisyuId = UnityEngine.Random.Range(0, targetHeisyuList.Count);
            targetHeisyu = targetHeisyuList[rdmHeisyuId];

            List<int> SrankDaimyoList = getSrankDaimyoList(senarioId);
            int rdmDaimyo1 = UnityEngine.Random.Range(0, SrankDaimyoList.Count);
            targetDaimyoId1 = SrankDaimyoList[rdmDaimyo1];
            SrankDaimyoList.Remove(targetDaimyoId1);
            int rdmDaimyo2 = UnityEngine.Random.Range(0, SrankDaimyoList.Count);
            targetDaimyoId2 = SrankDaimyoList[rdmDaimyo2];
            SrankDaimyoList.Remove(targetDaimyoId2);
            int rdmDaimyo3 = UnityEngine.Random.Range(0, SrankDaimyoList.Count);
            targetDaimyoId3 = SrankDaimyoList[rdmDaimyo3];
            SrankDaimyoList.Remove(targetDaimyoId3);

            PlayerPrefs.SetString("targetHeisyu", targetHeisyu);
            PlayerPrefs.SetInt("targetDaimyoId1", targetDaimyoId1);
            PlayerPrefs.SetInt("targetDaimyoId2", targetDaimyoId2);
            PlayerPrefs.SetInt("targetDaimyoId3", targetDaimyoId3);
            PlayerPrefs.SetBool("todayGacyaSpecialFlg",true);
            PlayerPrefs.Flush();
        } else {
            targetHeisyu = PlayerPrefs.GetString("targetHeisyu");
            targetDaimyoId1 = PlayerPrefs.GetInt("targetDaimyoId1");
            targetDaimyoId2 = PlayerPrefs.GetInt("targetDaimyoId2");
            targetDaimyoId3 = PlayerPrefs.GetInt("targetDaimyoId3");
        }
        //heisyu
        GameObject heisyuBase = GameObject.Find("heisyu").gameObject;
        string heisyuPath = "Prefabs/Item/Cyouhei/Cyouhei" + targetHeisyu;
        GameObject heisyuObj = Instantiate(Resources.Load(heisyuPath)) as GameObject;
        heisyuObj.transform.SetParent(heisyuBase.transform,false);
        heisyuObj.transform.localScale = new Vector3(0.35f,0.35f,0);
        heisyuObj.transform.localPosition = new Vector3(270, 110, 0);
        Destroy(heisyuObj.transform.Find("CyouheiRank").gameObject);
        
        List<int> SrankHeisyuList = getSrankHeisyuList(targetHeisyu);
        int rdmHeisyuId1 = UnityEngine.Random.Range(0, SrankHeisyuList.Count);
        int heisyu1 = SrankHeisyuList[rdmHeisyuId1];
        if (SrankHeisyuList.Count > 1) SrankHeisyuList.Remove(heisyu1);
        string heisyuPath1 = "Prefabs/Player/Sprite/unit" + heisyu1.ToString();
        heisyuBase.transform.Find("1").GetComponent<Image>().sprite =
            Resources.Load(heisyuPath1, typeof(Sprite)) as Sprite;
        int rdmHeisyuId2 = UnityEngine.Random.Range(0, SrankHeisyuList.Count);
        int heisyu2 = SrankHeisyuList[rdmHeisyuId2];
        string heisyuPath2 = "Prefabs/Player/Sprite/unit" + heisyu2.ToString();
        heisyuBase.transform.Find("2").GetComponent<Image>().sprite =
            Resources.Load(heisyuPath2, typeof(Sprite)) as Sprite;
        
        //daimyo
        //1
        GameObject daimyo = GameObject.Find("daimyo").gameObject;
        string myDaimyoStatusPath1 = "Prefabs/Kamon/MyDaimyoKamon/" + targetDaimyoId1.ToString();        
        daimyo.transform.Find("Kamon1").GetComponent<Image>().sprite =
            Resources.Load(myDaimyoStatusPath1, typeof(Sprite)) as Sprite;
        //2
        string myDaimyoStatusPath2 = "Prefabs/Kamon/MyDaimyoKamon/" + targetDaimyoId2.ToString();
        daimyo.transform.Find("Kamon2").GetComponent<Image>().sprite =
            Resources.Load(myDaimyoStatusPath2, typeof(Sprite)) as Sprite;
        //3
        string myDaimyoStatusPath3 = "Prefabs/Kamon/MyDaimyoKamon/" + targetDaimyoId3.ToString();
        daimyo.transform.Find("Kamon3").GetComponent<Image>().sprite =
            Resources.Load(myDaimyoStatusPath3, typeof(Sprite)) as Sprite;


        //S rank
        List<int> SrankBusyoList = getSrankBusyoList(targetDaimyoId1, targetDaimyoId2, targetDaimyoId3, senarioId);
        int rdmDaimyoId1 = UnityEngine.Random.Range(0, SrankBusyoList.Count);
        int busyoId1 = SrankBusyoList[rdmDaimyoId1];
        if (SrankBusyoList.Count > 1) SrankBusyoList.Remove(busyoId1);
        string busyoPath1 = "Prefabs/Player/Sprite/unit" + busyoId1.ToString();
        daimyo.transform.Find("1").GetComponent<Image>().sprite =
            Resources.Load(busyoPath1, typeof(Sprite)) as Sprite;

        int rdmDaimyoId2 = UnityEngine.Random.Range(0, SrankBusyoList.Count);
        int busyoId2 = SrankBusyoList[rdmDaimyoId2];
        string busyoPath2 = "Prefabs/Player/Sprite/unit" + busyoId2.ToString();
        daimyo.transform.Find("2").GetComponent<Image>().sprite =
            Resources.Load(busyoPath2, typeof(Sprite)) as Sprite;

        //Initial Display
        string specialBusyoHst = PlayerPrefs.GetString("specialBusyoHst");
        string specialGacyaNameHst = PlayerPrefs.GetString("specialGacyaNameHst");
        if (specialBusyoHst != "" && specialBusyoHst != null) {
            GameObject GacyaObj = initialSet();
            Destroy(GacyaObj.transform.Find("Anim").gameObject);            
            setSpecialBusyoHst(specialBusyoHst, langId, GacyaObj, specialGacyaNameHst);            
        }
    }

    public GameObject initialSet() {
        GameObject GacyaObj = Instantiate(Gacya);
        GacyaObj.transform.SetParent(Panel.transform, false);
        Gacya.SetActive(true);

        //delete chld
        Content = GacyaObj.transform.Find("Busyo").transform.Find("ScrollView").transform.Find("Content").gameObject;
        foreach (Transform n in Content.transform) {
            Destroy(n.gameObject);
        }

        return GacyaObj;
    }


    public void doGacyaSpecial(string typ,int gacyaCount, int hireCount, Dictionary<int, Busyo> tmpBusyoListDic, string gacyaName) {

        GameObject GacyaObj = initialSet();

        //set parametor  
        List<Busyo> resultS = new List<Busyo>();
        List<Busyo> resultA = new List<Busyo>();
        List<Busyo> resultB = new List<Busyo>();

        if (typ == "heisyu") {            
            foreach (Busyo field in tmpBusyoListDic.Values) {
                if (field.heisyu == targetHeisyu) {
                    if(field.rank == "S") {
                        resultS.Add(field);
                    }else if (field.rank == "A") {
                        resultA.Add(field);
                    }else if (field.rank == "B") {
                        resultB.Add(field);
                    }
                }
            }
        }else if(typ=="daimyo") {
            foreach (Busyo field in tmpBusyoListDic.Values) {
                if (field.daimyoId == targetDaimyoId1 || field.daimyoHst == targetDaimyoId1
                    || field.daimyoId == targetDaimyoId2 || field.daimyoHst == targetDaimyoId2
                    || field.daimyoId == targetDaimyoId3 || field.daimyoHst == targetDaimyoId3) {
                    if (field.rank == "S") {
                        resultS.Add(field);
                    }else if (field.rank == "A") {
                        resultA.Add(field);
                    }else if (field.rank == "B") {
                        resultB.Add(field);
                    }
                }
            }
        }else if(typ == "s") {
            foreach (Busyo field in tmpBusyoListDic.Values) {
                if (field.rank == "S") {
                    resultS.Add(field);
                }
            }
        }else {
            foreach (Busyo field in tmpBusyoListDic.Values) {
                if (field.rank == "S") {
                    resultS.Add(field);
                }else if (field.rank == "A") {
                    resultA.Add(field);
                }else if (field.rank == "B") {
                    resultB.Add(field);
                }
            }
        }
        
        //random draw
        List<Busyo> hitBusyo = new List<Busyo>();
        for(int i=0; i< gacyaCount; i++) {
            if (typ == "s") {
                int rdmId = UnityEngine.Random.Range(0, resultS.Count);
                hitBusyo.Add(resultS[rdmId]);
            }else {
                //S>0.5,A>10,B>89.5
                float percent = UnityEngine.Random.value;
                percent = percent * 100;
                if (percent <= 0.5f) {
                    //S
                    if(resultS.Count !=0) {
                        int rdmId = UnityEngine.Random.Range(0, resultS.Count);
                        hitBusyo.Add(resultS[rdmId]);
                    }else {
                        if(resultA.Count != 0) {
                            int rdmId = UnityEngine.Random.Range(0, resultA.Count);
                            hitBusyo.Add(resultA[rdmId]);
                        }else {
                            int rdmId = UnityEngine.Random.Range(0, resultB.Count);
                            hitBusyo.Add(resultB[rdmId]);
                        }
                    }
                }else if(0.5f<percent && percent <=10.5f) {
                    //A
                    if (resultA.Count != 0) {
                        int rdmId = UnityEngine.Random.Range(0, resultA.Count);
                        hitBusyo.Add(resultA[rdmId]);
                    }else {
                        int rdmId = UnityEngine.Random.Range(0, resultB.Count);
                        hitBusyo.Add(resultB[rdmId]);
                    }
                }else if (10.5f < percent) {
                    //B
                    if (resultB.Count != 0) {
                        int rdmId = UnityEngine.Random.Range(0, resultB.Count);
                        hitBusyo.Add(resultB[rdmId]);
                    }else {
                        int rdmId = UnityEngine.Random.Range(0, resultA.Count);
                        hitBusyo.Add(resultA[rdmId]);
                    }
                }
            }
        }
        string specialBusyoHst = "";
        foreach (Busyo busyo in hitBusyo) {
            if (specialBusyoHst == "") {
                specialBusyoHst = busyo.busyoId.ToString();
            }else {
                specialBusyoHst = specialBusyoHst + "," + busyo.busyoId.ToString();
            }
        }
        PlayerPrefs.SetString("specialBusyoHst", specialBusyoHst);
        PlayerPrefs.SetString("specialGacyaNameHst", gacyaName);

        Text countDown = GacyaObj.transform.Find("Anim").transform.Find("Text").GetComponent<Text>();
        GameObject Anim = GacyaObj.transform.Find("Anim").gameObject;
        GameObject Busyo = GacyaObj.transform.Find("Busyo").gameObject;
        GameObject Button = GacyaObj.transform.Find("Busyo").transform.Find("Button").gameObject;
        GameObject Detail = GacyaObj.transform.Find("Busyo").transform.Find("Detail").gameObject;

        //Zukan Check
        string zukanBusyoHst = PlayerPrefs.GetString("zukanBusyoHst");
        List<string> myZukanList = new List<string>();
        if (zukanBusyoHst != null && zukanBusyoHst != "") {            
            char[] delimiterChars = { ',' };
            if (zukanBusyoHst.Contains(",")) {
                myZukanList = new List<string>(zukanBusyoHst.Split(delimiterChars));
            }else {
                myZukanList.Add(zukanBusyoHst);
            }            
        }
        
        StartCoroutine(loop(hitBusyo, countDown, Anim, Button, Detail, myZukanList));
        Busyo.transform.Find("Title").GetComponent<Text>().text = gacyaName;        
        Button.GetComponent<GacyaSpecialTouyou>().hireCount = hireCount;
        Button.transform.Find("b").GetComponent<Text>().text = "/" + hireCount;
        
    }

    private IEnumerator loop(List<Busyo> hitBusyo, Text countDown,GameObject Anim, GameObject Button, GameObject Detail, List<string> myZukanList) {
        while (true) {

            countDown.text = hitBusyo.Count.ToString();
            int busyoId = hitBusyo[0].busyoId;
            string rank = hitBusyo[0].rank;
            string busyoName = hitBusyo[0].busyoName;
            int daimyoId = hitBusyo[0].daimyoId;
            if(daimyoId ==0) daimyoId = hitBusyo[0].daimyoHst;
            string heisyu = hitBusyo[0].heisyu;
            hitBusyo.RemoveAt(0);
            audioSources[0].Play();
            
            //Set Busyo
            GameObject SlotObj = Instantiate(Slot);
            SlotObj.transform.SetParent(Content.transform, false);
            SlotObj.transform.Find("Name").GetComponent<Text>().text = busyoName;
            SlotObj.transform.Find("Rank").GetComponent<Text>().text = rank;
            string myDaimyoPath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
            SlotObj.transform.Find("Kamon").GetComponent<Image>().sprite =
                Resources.Load(myDaimyoPath, typeof(Sprite)) as Sprite;
            string myBusyoPath = "Prefabs/Player/Sprite/unit" + busyoId.ToString();
            SlotObj.transform.Find("Image").GetComponent<Image>().sprite =
                Resources.Load(myBusyoPath, typeof(Sprite)) as Sprite;
            SlotObj.GetComponent<GacyaSpecialSelect>().busyoId = busyoId;
            SlotObj.GetComponent<GacyaSpecialSelect>().Button = Button;
            SlotObj.GetComponent<GacyaSpecialSelect>().Detail = Detail;
            if(myZukanList.Contains(busyoId.ToString())) SlotObj.GetComponent<GacyaSpecialSelect>().zukanExistFlg = true;

            if (rank == "S" || rank == "A") {
                GameObject lightningObj = Instantiate(lightning);
                audioSources[6].Play();
                if (rank == "S") {
                    GameObject gacyaEffectSObj = Instantiate(gacyaEffectS);
                    gacyaEffectSObj.transform.SetParent(SlotObj.transform, false);
                    gacyaEffectSObj.transform.localScale = new Vector3(100,40,0);
                    gacyaEffectSObj.transform.localPosition = new Vector3(0, 15, 0);
                }else if (rank == "A") {
                    GameObject gacyaEffectAObj = Instantiate(gacyaEffectA);
                    gacyaEffectAObj.transform.SetParent(SlotObj.transform, false);
                    gacyaEffectAObj.transform.localScale = new Vector3(100, 40, 0);
                    gacyaEffectAObj.transform.localPosition = new Vector3(0, 15, 0);
                }
                lightningObj.transform.SetParent(SlotObj.transform, false);
                lightningObj.transform.localScale = new Vector3(100, 180, 0);
            }


            if (hitBusyo.Count != 0) {
                    yield return new WaitForSeconds(0.1f);
                } else {
                    countDown.text = "0";
                    Anim.transform.Find("Left").GetComponent<MoveBoard>().runFlg = true;
                    Anim.transform.Find("Right").GetComponent<MoveBoard>().runFlg = true;
                    yield break;
                }
        }
    }

    public List<int> getSrankDaimyoList(int senarioId) {
        List<int> SrankDaimyoList = new List<int>();
        Entity_busyo_mst busyoMst = Resources.Load("Data/busyo_mst") as Entity_busyo_mst;
        Entity_daimyo_mst daimyoMst = Resources.Load("Data/daimyo_mst") as Entity_daimyo_mst;
        BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
        for (int i=0; i< busyoMst.param.Count; i++) {
            string rank = busyoMst.param[i].rank;
            if(rank=="S") {
                int busyoId = i + 1;
                int daimyoId = BusyoInfoGet.getDaimyoId(busyoId, senarioId);
                if (daimyoId==0) daimyoId = BusyoInfoGet.getDaimyoHst(busyoId, senarioId);
                if (!SrankDaimyoList.Contains(daimyoId)) {
                    if (daimyoId < daimyoMst.param.Count+1) {
                        SrankDaimyoList.Add(daimyoId);                        
                    }
                }
            }            
        }
        return SrankDaimyoList;
    }

    public List<int> getSrankBusyoList(int targetDaimyoId1, int targetDaimyoId2, int targetDaimyoId3, int senarioId) {
        List<int> SrankBusyoList = new List<int>();
        Entity_busyo_mst busyoMst = Resources.Load("Data/busyo_mst") as Entity_busyo_mst;
        BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
        for (int i = 0; i < busyoMst.param.Count; i++) {
            int busyoId = i + 1;
            int daimyoId = BusyoInfoGet.getDaimyoId(busyoId, senarioId);
            if (daimyoId==0) daimyoId = BusyoInfoGet.getDaimyoHst(busyoId, senarioId);
            if (targetDaimyoId1 == daimyoId || targetDaimyoId2 == daimyoId|| targetDaimyoId3 == daimyoId) {
                string rank = busyoMst.param[i].rank;
                if(rank=="S") {
                    SrankBusyoList.Add(busyoId);
                }
            }
        }
        return SrankBusyoList;
    }

    public List<int> getSrankHeisyuList(string targetHeisyu) {
        List<int> SrankHeisyuList = new List<int>();
        Entity_busyo_mst busyoMst = Resources.Load("Data/busyo_mst") as Entity_busyo_mst;
        for (int i = 0; i < busyoMst.param.Count; i++) {
            string heisyu = busyoMst.param[i].heisyu;            
            if (targetHeisyu == heisyu) {
                string rank = busyoMst.param[i].rank;
                if (rank == "S") {
                    int busyoId = busyoMst.param[i].id;
                    SrankHeisyuList.Add(busyoId);
                }
            }
        }
        return SrankHeisyuList;
    }

    public void setSpecialBusyoHst(string specialBusyoHst, int langId, GameObject GacyaObj, string specialGacyaNameHst) {

        char[] delimiter = { ',' };
        List<string> specialBusyoHstList = new List<string>(specialBusyoHst.Split(delimiter));
        BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
        Text countDown = GacyaObj.transform.Find("Anim").transform.Find("Text").GetComponent<Text>();
        GameObject Anim = GacyaObj.transform.Find("Anim").gameObject;
        GameObject Busyo = GacyaObj.transform.Find("Busyo").gameObject;
        GameObject Button = GacyaObj.transform.Find("Busyo").transform.Find("Button").gameObject;
        GameObject Detail = GacyaObj.transform.Find("Busyo").transform.Find("Detail").gameObject;
        //Zukan Check
        string zukanBusyoHst = PlayerPrefs.GetString("zukanBusyoHst");
        List<string> myZukanList = new List<string>();
        if (zukanBusyoHst != null && zukanBusyoHst != "") {
            char[] delimiterChars = { ',' };
            if (zukanBusyoHst.Contains(",")) {
                myZukanList = new List<string>(zukanBusyoHst.Split(delimiterChars));
            }else {
                myZukanList.Add(zukanBusyoHst);
            }
        }

        foreach (string specialBusyo in specialBusyoHstList) {

            int busyoId = int.Parse(specialBusyo);
            string rank = BusyoInfoGet.getRank(busyoId);
            string busyoName = BusyoInfoGet.getName(busyoId, langId);
            int senarioId = PlayerPrefs.GetInt("senarioId");
            int daimyoId = BusyoInfoGet.getDaimyoId(busyoId,senarioId);
            if (daimyoId == 0) daimyoId = BusyoInfoGet.getDaimyoHst(busyoId,senarioId);
            string heisyu = BusyoInfoGet.getHeisyu(busyoId);            
            audioSources[0].Play();

            //Set Busyo
            GameObject SlotObj = Instantiate(Slot);
            SlotObj.transform.SetParent(Content.transform, false);
            SlotObj.transform.Find("Name").GetComponent<Text>().text = busyoName;
            SlotObj.transform.Find("Rank").GetComponent<Text>().text = rank;
            string myDaimyoPath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
            SlotObj.transform.Find("Kamon").GetComponent<Image>().sprite =
                Resources.Load(myDaimyoPath, typeof(Sprite)) as Sprite;
            string myBusyoPath = "Prefabs/Player/Sprite/unit" + busyoId.ToString();
            SlotObj.transform.Find("Image").GetComponent<Image>().sprite =
                Resources.Load(myBusyoPath, typeof(Sprite)) as Sprite;
            SlotObj.GetComponent<GacyaSpecialSelect>().busyoId = busyoId;
            SlotObj.GetComponent<GacyaSpecialSelect>().Button = Button;
            SlotObj.GetComponent<GacyaSpecialSelect>().Detail = Detail;
            if (myZukanList.Contains(busyoId.ToString())) SlotObj.GetComponent<GacyaSpecialSelect>().zukanExistFlg = true;
            
            if (rank == "S") {
                GameObject gacyaEffectSObj = Instantiate(gacyaEffectS);
                gacyaEffectSObj.transform.SetParent(SlotObj.transform, false);
                gacyaEffectSObj.transform.localScale = new Vector3(100, 40, 0);
                gacyaEffectSObj.transform.localPosition = new Vector3(0, 15, 0);
            }
            else if (rank == "A") {
                GameObject gacyaEffectAObj = Instantiate(gacyaEffectA);
                gacyaEffectAObj.transform.SetParent(SlotObj.transform, false);
                gacyaEffectAObj.transform.localScale = new Vector3(100, 40, 0);
                gacyaEffectAObj.transform.localPosition = new Vector3(0, 15, 0);
            }
            

        }

        int hireCount = 0;
        if(specialBusyoHstList.Count == 30) {
            hireCount = 10;
        }else {
            hireCount = 1;
        }

        Busyo.transform.Find("Title").GetComponent<Text>().text = specialGacyaNameHst;
        Button.GetComponent<GacyaSpecialTouyou>().hireCount = hireCount;
        Button.transform.Find("b").GetComponent<Text>().text = "/" + hireCount;

    }

}
