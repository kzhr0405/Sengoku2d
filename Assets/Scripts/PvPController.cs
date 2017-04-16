using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class PvPController : MonoBehaviour {

    public string myUserId;
    public int kuniLv;
    public int HpBase;

    public int myJinkeiHeiryoku;
    private GameObject LeftView;

    public PvPDataStore PvPDataStore;
    public bool isHPFetched;
    public bool isMyPvPFetched;
    public bool isAtkDfcFetched;
    public bool isShowed;
    public int pvpCount = 0;
    public int hpRank = 0;
    public int atkNo = 0;
    public int atkWinNo = 0;
    public int dfcNo = 0;
    public int dfcWinNo = 0;
    public int totalWinNo = 0;
    public int winRank = 0;

    //PvP
    public bool showedFlg = false;

    void Start () {
        ShowKassen();        
    }
	
	public void ShowKassen() {

        /*Common*/
        myUserId = PlayerPrefs.GetString("userId");
        

        //Hyourou
        int hyourou = PlayerPrefs.GetInt("hyourou");
        GameObject.Find("HyourouCurrentValue").GetComponent<Text>().text = hyourou.ToString();

        //Tama
        int busyoDama = PlayerPrefs.GetInt("busyoDama");
        GameObject.Find("BusyoDamaValue").GetComponent<Text>().text = busyoDama.ToString();


        /*LeftView*/
        LeftView = GameObject.Find("LeftView").gameObject;
        //Name
        string PvPName = PlayerPrefs.GetString("PvPName");
        LeftView.transform.FindChild("myName").GetComponent<Text>().text = PvPName;

        //KuniLv
        kuniLv = PlayerPrefs.GetInt("kuniLv");
        LeftView.transform.FindChild("KuniLv").transform.FindChild("KuniLvValue").GetComponent<Text>().text = kuniLv.ToString();

        //Kamon & Busyo Image
        int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
        string imagePath = "Prefabs/Kamon/" + myDaimyo.ToString();
        LeftView.transform.FindChild("Kamon").GetComponent<Image>().sprite =
            Resources.Load(imagePath, typeof(Sprite)) as Sprite;
        
        int jinkei = PlayerPrefs.GetInt("jinkei");
        string soudaisyo = "soudaisyo" + jinkei.ToString();
        int soudaisyoBusyoId = PlayerPrefs.GetInt(soudaisyo);
        string imagePath2 = "Prefabs/Player/Sprite/unit" + soudaisyoBusyoId.ToString();
        LeftView.transform.FindChild("Soudaisyo").GetComponent<Image>().sprite =
            Resources.Load(imagePath2, typeof(Sprite)) as Sprite;

        //HP
        myJinkeiHeiryoku = PlayerPrefs.GetInt("jinkeiHeiryoku");
        LeftView.transform.FindChild("HP").transform.FindChild("Value").GetComponent<Text>().text = myJinkeiHeiryoku.ToString();


        //NCMB Data
        PvPDataStore = GameObject.Find("DataStore").GetComponent<PvPDataStore>();
        isHPFetched = false;
        isMyPvPFetched = false;
        isAtkDfcFetched = false;
        isShowed = false;

        //1 time run
        PvPDataStore.GetPvPCount();

        //PvP
        List<float> perList = new List<float>() {0.7f, 1.0f, 1.3f};
        int rdmId = UnityEngine.Random.Range(0, perList.Count);
        float per = perList[rdmId];
        HpBase = Mathf.CeilToInt((float)myJinkeiHeiryoku * per);
        PvPDataStore.GetRandomEnemy(myUserId, HpBase);


    }



    void Update() {
        //HpRank
        if (PvPDataStore.pvpCount != -1 && !isHPFetched) {
            //1 time
            PvPDataStore.GetHpRank();
            isHPFetched = true;
        }

        //MyPvP
        if (PvPDataStore.hpRank != -1 && isHPFetched && !isMyPvPFetched) {
            //1 time
            PvPDataStore.GetMyPvP();
            isMyPvPFetched = true;
        }
        
        if (PvPDataStore.dfcWinNo != -1 && isHPFetched && isMyPvPFetched && !isAtkDfcFetched) {
            //1 time
            pvpCount = PvPDataStore.pvpCount;
            hpRank = PvPDataStore.hpRank;
            atkNo = PvPDataStore.atkNo;
            atkWinNo = PvPDataStore.atkWinNo;
            dfcNo = PvPDataStore.dfcNo;
            dfcWinNo = PvPDataStore.dfcWinNo;
            totalWinNo = atkWinNo + dfcWinNo;
            PvPDataStore.GetWinRank(totalWinNo);
            isAtkDfcFetched = true;
        }

        if (PvPDataStore.winRank != -1 && isHPFetched && isMyPvPFetched && isAtkDfcFetched && !isShowed) {

            winRank = PvPDataStore.winRank;
            LeftView.transform.FindChild("HP").transform.FindChild("Rank").GetComponent<Text>().text = hpRank.ToString() + "/" + pvpCount;
            LeftView.transform.FindChild("Win").transform.FindChild("Value").GetComponent<Text>().text = totalWinNo.ToString();
            LeftView.transform.FindChild("Win").transform.FindChild("Rank").GetComponent<Text>().text = winRank.ToString() + "/" + pvpCount;

            //Active
            LeftView.transform.FindChild("Active").transform.FindChild("BtlValue").GetComponent<Text>().text = atkNo.ToString();
            LeftView.transform.FindChild("Active").transform.FindChild("WinValue").GetComponent<Text>().text = atkWinNo.ToString();
            int winAtkRatio = 0;
            if(atkNo !=0) {
                winAtkRatio = Mathf.FloorToInt((float)atkWinNo / (float)atkNo * 100);
            }
            LeftView.transform.FindChild("Active").transform.FindChild("RatioValue").GetComponent<Text>().text = winAtkRatio.ToString();

            //Passive
            LeftView.transform.FindChild("Passive").transform.FindChild("BtlValue").GetComponent<Text>().text = dfcNo.ToString();
            LeftView.transform.FindChild("Passive").transform.FindChild("WinValue").GetComponent<Text>().text = dfcWinNo.ToString();
            int winDfcRatio = 0;
            if (dfcNo != 0) {
                winDfcRatio = Mathf.FloorToInt((float)dfcWinNo / (float)dfcNo * 100);
            }
            LeftView.transform.FindChild("Passive").transform.FindChild("RatioValue").GetComponent<Text>().text = winDfcRatio.ToString();
            
            isShowed = true;
        }

        //PvP match
        if(PvPDataStore.matchedFlg && !showedFlg) {
            //PvP1
            GameObject PvP1 = GameObject.Find("1").gameObject;
            PvP1.transform.FindChild("NameValue").GetComponent<Text>().text = PvPDataStore.pvpUserNameList[0];
            PvP1.transform.FindChild("KuniLv").transform.FindChild("KuniLvValue").GetComponent<Text>().text = PvPDataStore.pvpKuniLvList[0].ToString();
            PvP1.transform.FindChild("HP").transform.FindChild("Value").GetComponent<Text>().text = PvPDataStore.pvpHpList[0].ToString();
            PvP1.transform.FindChild("Win").transform.FindChild("Value").GetComponent<Text>().text = PvPDataStore.pvpWinList[0].ToString();
            string imagePath = "Prefabs/Player/Sprite/unit" + PvPDataStore.pvpSoudaisyoList[0].ToString();
            PvP1.transform.FindChild("Busyo").transform.FindChild("Image").GetComponent<Image>().sprite =
                Resources.Load(imagePath, typeof(Sprite)) as Sprite;
            PvP1.transform.FindChild("StartBtn").GetComponent<StartKassenPvP>().pvpStageId = 1;
            PvP1.transform.FindChild("StartBtn").GetComponent<StartKassenPvP>().userId = PvPDataStore.pvpUserIdList[0];

            //PvP2
            if (PvPDataStore.pvpUserNameList.Count > 1) {
                GameObject PvP2 = GameObject.Find("2").gameObject;
                PvP2.transform.FindChild("NameValue").GetComponent<Text>().text = PvPDataStore.pvpUserNameList[1];
                PvP2.transform.FindChild("KuniLv").transform.FindChild("KuniLvValue").GetComponent<Text>().text = PvPDataStore.pvpKuniLvList[1].ToString();
                PvP2.transform.FindChild("HP").transform.FindChild("Value").GetComponent<Text>().text = PvPDataStore.pvpHpList[1].ToString();
                PvP2.transform.FindChild("Win").transform.FindChild("Value").GetComponent<Text>().text = PvPDataStore.pvpWinList[1].ToString();
                string imagePath2 = "Prefabs/Player/Sprite/unit" + PvPDataStore.pvpSoudaisyoList[1].ToString();
                PvP2.transform.FindChild("Busyo").transform.FindChild("Image").GetComponent<Image>().sprite =
                    Resources.Load(imagePath2, typeof(Sprite)) as Sprite;
                PvP2.transform.FindChild("StartBtn").GetComponent<StartKassenPvP>().pvpStageId = 2;
                PvP2.transform.FindChild("StartBtn").GetComponent<StartKassenPvP>().userId = PvPDataStore.pvpUserIdList[1];
                
                //PvP3
                if (PvPDataStore.pvpUserNameList.Count > 2) {
                    GameObject PvP3 = GameObject.Find("3").gameObject;
                    PvP3.transform.FindChild("NameValue").GetComponent<Text>().text = PvPDataStore.pvpUserNameList[2];
                    PvP3.transform.FindChild("KuniLv").transform.FindChild("KuniLvValue").GetComponent<Text>().text = PvPDataStore.pvpKuniLvList[2].ToString();
                    PvP3.transform.FindChild("HP").transform.FindChild("Value").GetComponent<Text>().text = PvPDataStore.pvpHpList[2].ToString();
                    PvP3.transform.FindChild("Win").transform.FindChild("Value").GetComponent<Text>().text = PvPDataStore.pvpWinList[2].ToString();
                    string imagePath3 = "Prefabs/Player/Sprite/unit" + PvPDataStore.pvpSoudaisyoList[2].ToString();
                    PvP3.transform.FindChild("Busyo").transform.FindChild("Image").GetComponent<Image>().sprite =
                        Resources.Load(imagePath3, typeof(Sprite)) as Sprite;
                    PvP3.transform.FindChild("StartBtn").GetComponent<StartKassenPvP>().pvpStageId = 3;
                    PvP3.transform.FindChild("StartBtn").GetComponent<StartKassenPvP>().userId = PvPDataStore.pvpUserIdList[2];
                }else {
                    Destroy(GameObject.Find("3").gameObject);
                }
            }else {
                Destroy(GameObject.Find("2").gameObject);
                Destroy(GameObject.Find("3").gameObject);
            }
            GameObject.Find("RightView").GetComponent<ScrollRect>().enabled = false;
            showedFlg = true;
        }
        if(PvPDataStore.zeroFlg && !showedFlg) {

            //dummy enemy
            
            showedFlg = true;
        }


       
    }



}
