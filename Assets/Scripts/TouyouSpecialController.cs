using System.Collections;
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

    void Start () {
        //Common
        int busyoDama = PlayerPrefs.GetInt("busyoDama");

        GameObject.Find("BusyoDamaValue").GetComponent<Text>().text = busyoDama.ToString();
        Panel = GameObject.Find("Panel").gameObject;
        audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();

        //gacya busyo set
        Entity_busyo_mst busyoMst = Resources.Load("Data/busyo_mst") as Entity_busyo_mst;
        busyoListDic = new Dictionary<int, Busyo>();
        for (int i = 0; i < busyoMst.param.Count; i++) {
            int busyoId = busyoMst.param[i].id;            
            string rank = busyoMst.param[i].rank;
            string busyoName = busyoMst.param[i].name;
            string heisyu = busyoMst.param[i].heisyu;
            int daimyoId = busyoMst.param[i].daimyoId;
            int daimyoHst = busyoMst.param[i].daimyoHst;
            busyoListDic.Add(busyoId, new Busyo { busyoId = busyoId, busyoName= busyoName, rank = rank, heisyu = heisyu, daimyoId = daimyoId , daimyoHst = daimyoHst });
        }

        //target
        //bool todayGacyaSpecialFlg = PlayerPrefs.GetBool("todayGacyaSpecialFlg");
        bool todayGacyaSpecialFlg = false; //test
        if (!todayGacyaSpecialFlg) {
            List<string> targetHeisyuList = new List<string>() { "YR", "KB", "TP", "YM"};
            int rdmHeisyuId = UnityEngine.Random.Range(0, targetHeisyuList.Count);
            targetHeisyu = targetHeisyuList[rdmHeisyuId];

            List<int> SrankDaimyoList = getSrankDaimyoList();
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
        Destroy(heisyuObj.transform.FindChild("CyouheiRank").gameObject);
        
        List<int> SrankHeisyuList = getSrankHeisyuList(targetHeisyu);
        int rdmHeisyuId1 = UnityEngine.Random.Range(0, SrankHeisyuList.Count);
        int heisyu1 = SrankHeisyuList[rdmHeisyuId1];
        if (SrankHeisyuList.Count > 1) SrankHeisyuList.Remove(heisyu1);
        string heisyuPath1 = "Prefabs/Player/Sprite/unit" + heisyu1.ToString();
        heisyuBase.transform.FindChild("1").GetComponent<Image>().sprite =
            Resources.Load(heisyuPath1, typeof(Sprite)) as Sprite;
        int rdmHeisyuId2 = UnityEngine.Random.Range(0, SrankHeisyuList.Count);
        int heisyu2 = SrankHeisyuList[rdmHeisyuId2];
        string heisyuPath2 = "Prefabs/Player/Sprite/unit" + heisyu2.ToString();
        heisyuBase.transform.FindChild("2").GetComponent<Image>().sprite =
            Resources.Load(heisyuPath2, typeof(Sprite)) as Sprite;
        
        //daimyo
        //1
        GameObject daimyo = GameObject.Find("daimyo").gameObject;
        string myDaimyoStatusPath1 = "Prefabs/Kamon/MyDaimyoKamon/" + targetDaimyoId1.ToString();        
        daimyo.transform.FindChild("Kamon1").GetComponent<Image>().sprite =
            Resources.Load(myDaimyoStatusPath1, typeof(Sprite)) as Sprite;
        //2
        string myDaimyoStatusPath2 = "Prefabs/Kamon/MyDaimyoKamon/" + targetDaimyoId2.ToString();
        daimyo.transform.FindChild("Kamon2").GetComponent<Image>().sprite =
            Resources.Load(myDaimyoStatusPath2, typeof(Sprite)) as Sprite;
        //3
        string myDaimyoStatusPath3 = "Prefabs/Kamon/MyDaimyoKamon/" + targetDaimyoId3.ToString();
        daimyo.transform.FindChild("Kamon3").GetComponent<Image>().sprite =
            Resources.Load(myDaimyoStatusPath3, typeof(Sprite)) as Sprite;


        //S rank
        List<int> SrankBusyoList = getSrankBusyoList(targetDaimyoId1, targetDaimyoId2, targetDaimyoId3);
        int rdmDaimyoId1 = UnityEngine.Random.Range(0, SrankBusyoList.Count);
        int busyoId1 = SrankBusyoList[rdmDaimyoId1];
        if (SrankBusyoList.Count > 1) SrankBusyoList.Remove(busyoId1);
        string busyoPath1 = "Prefabs/Player/Sprite/unit" + busyoId1.ToString();
        daimyo.transform.FindChild("1").GetComponent<Image>().sprite =
            Resources.Load(busyoPath1, typeof(Sprite)) as Sprite;

        int rdmDaimyoId2 = UnityEngine.Random.Range(0, SrankBusyoList.Count);
        int busyoId2 = SrankBusyoList[rdmDaimyoId2];
        string busyoPath2 = "Prefabs/Player/Sprite/unit" + busyoId2.ToString();
        daimyo.transform.FindChild("2").GetComponent<Image>().sprite =
            Resources.Load(busyoPath2, typeof(Sprite)) as Sprite;
        

    }

    public void doGacyaSpecial(string typ,int gacyaCount, int hireCount, Dictionary<int, Busyo> tmpBusyoListDic, string gacyaName) {
        GameObject GacyaObj = Instantiate(Gacya);
        GacyaObj.transform.SetParent(Panel.transform,false);
        Gacya.SetActive(true);

        //delete chld
        Content = GacyaObj.transform.FindChild("Busyo").transform.FindChild("ScrollView").transform.FindChild("Content").gameObject;
        foreach (Transform n in Content.transform) {
            Destroy(n.gameObject);
        }

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
        Text countDown = GacyaObj.transform.FindChild("Anim").transform.FindChild("Text").GetComponent<Text>();
        GameObject Anim = GacyaObj.transform.FindChild("Anim").gameObject;
        GameObject Busyo = GacyaObj.transform.FindChild("Busyo").gameObject;
        GameObject Button = GacyaObj.transform.FindChild("Busyo").transform.FindChild("Button").gameObject;
        GameObject Detail = GacyaObj.transform.FindChild("Busyo").transform.FindChild("Detail").gameObject;

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
        Busyo.transform.FindChild("Title").GetComponent<Text>().text = gacyaName;        
        Button.GetComponent<GacyaSpecialTouyou>().hireCount = hireCount;
        Button.transform.FindChild("b").GetComponent<Text>().text = "/" + hireCount;

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

            if (rank == "S" || rank == "A") {
                GameObject lightningObj = Instantiate(lightning);
                lightningObj.transform.SetParent(Panel.transform,false);
                lightningObj.transform.localScale = new Vector3(100,120,0);
            }

            //Set Busyo
            GameObject SlotObj = Instantiate(Slot);
            SlotObj.transform.SetParent(Content.transform, false);
            SlotObj.transform.FindChild("Name").GetComponent<Text>().text = busyoName;
            SlotObj.transform.FindChild("Rank").GetComponent<Text>().text = rank;
            string myDaimyoPath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
            SlotObj.transform.FindChild("Kamon").GetComponent<Image>().sprite =
                Resources.Load(myDaimyoPath, typeof(Sprite)) as Sprite;
            string myBusyoPath = "Prefabs/Player/Sprite/unit" + busyoId.ToString();
            SlotObj.transform.FindChild("Image").GetComponent<Image>().sprite =
                Resources.Load(myBusyoPath, typeof(Sprite)) as Sprite;
            SlotObj.GetComponent<GacyaSpecialSelect>().busyoId = busyoId;
            SlotObj.GetComponent<GacyaSpecialSelect>().Button = Button;
            SlotObj.GetComponent<GacyaSpecialSelect>().Detail = Detail;
            if(myZukanList.Contains(busyoId.ToString())) SlotObj.GetComponent<GacyaSpecialSelect>().zukanExistFlg = true;

            if (hitBusyo.Count != 0) {
                    yield return new WaitForSeconds(0.1f);
                } else {
                    countDown.text = "0";
                    Anim.transform.FindChild("Left").GetComponent<MoveBoard>().runFlg = true;
                    Anim.transform.FindChild("Right").GetComponent<MoveBoard>().runFlg = true;
                    yield break;
                }



        }
    }

    public List<int> getSrankDaimyoList() {
        List<int> SrankDaimyoList = new List<int>();
        Entity_busyo_mst busyoMst = Resources.Load("Data/busyo_mst") as Entity_busyo_mst;
        for(int i=0; i< busyoMst.param.Count; i++) {
            string rank = busyoMst.param[i].rank;
            if(rank=="S") {
                int daimyoId = busyoMst.param[i].daimyoId;
                if(daimyoId==0) daimyoId = busyoMst.param[i].daimyoHst;
                if (!SrankDaimyoList.Contains(daimyoId)) {
                    if (daimyoId < 47) {
                        SrankDaimyoList.Add(daimyoId);                        
                    }
                }
            }            
        }
        return SrankDaimyoList;
    }

    public List<int> getSrankBusyoList(int targetDaimyoId1, int targetDaimyoId2, int targetDaimyoId3) {
        List<int> SrankBusyoList = new List<int>();
        Entity_busyo_mst busyoMst = Resources.Load("Data/busyo_mst") as Entity_busyo_mst;
        for (int i = 0; i < busyoMst.param.Count; i++) {
            int daimyoId = busyoMst.param[i].daimyoId;
            if(daimyoId==0) daimyoId = busyoMst.param[i].daimyoHst;
            if (targetDaimyoId1 == daimyoId || targetDaimyoId2 == daimyoId|| targetDaimyoId3 == daimyoId) {
                string rank = busyoMst.param[i].rank;
                if(rank=="S") {
                    int busyoId = busyoMst.param[i].id;
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

}
