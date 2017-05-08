using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;
using System.Collections.Generic;

public class PvPController : MonoBehaviour {

    public string myUserId;
    public string myUserName;
    public int kuniLv;
    public int HpBase;
    public int soudaisyoBusyoId;

    public int myJinkeiHeiryoku;
    private GameObject LeftView;

    //Total
    public PvPDataStore PvPDataStore;
    public bool isHPFetched;
    public bool isMyPvPFetched;
    public bool isAtkDfcFetched;
    public bool isShowed;
    public bool isEnemyPvPRankFetched;
    public int pvpCount = 0;
    public int hpRank = 0;
    public int atkNo = 0;
    public int atkWinNo = 0;
    public int dfcNo = 0;
    public int dfcWinNo = 0;
    public int totalWinNo = 0;

    //Weekly
    public bool isMyPvPWeeklyFetched;
    public bool isAtkDfcWeeklyFetched;
    public int pvpCountWeekly = 0;
    public int atkNoWeekly = 0;
    public int atkWinNoWeekly = 0;
    public int dfcNoWeekly = 0;
    public int dfcWinNoWeekly = 0;
    public int totalWinNoWeekly = 0;
    public int winRankWeekly = 0;

    //PvP Match
    public bool showedFlg = false;
    public GameObject PvP1 = null;
    public GameObject PvP2 = null;
    public GameObject PvP3 = null;
    public bool PvP1doneFlg = false;
    public bool PvP2doneFlg = false;
    public bool PvP3doneFlg = false;
    public float per;

    //Ranking
    public bool isNeighborsFerched;

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
        myUserName = PlayerPrefs.GetString("PvPName");
        LeftView.transform.FindChild("myName").GetComponent<Text>().text = myUserName;

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
        soudaisyoBusyoId = PlayerPrefs.GetInt(soudaisyo);
        string imagePath2 = "Prefabs/Player/Sprite/unit" + soudaisyoBusyoId.ToString();
        LeftView.transform.FindChild("Soudaisyo").GetComponent<Image>().sprite =
            Resources.Load(imagePath2, typeof(Sprite)) as Sprite;

        //HP
        myJinkeiHeiryoku = PlayerPrefs.GetInt("pvpHeiryoku");
        if(myJinkeiHeiryoku==0) {
            myJinkeiHeiryoku = PlayerPrefs.GetInt("jinkeiHeiryoku");
            PlayerPrefs.SetInt("pvpHeiryoku", myJinkeiHeiryoku);
            PlayerPrefs.Flush();
        }
        //myJinkeiHeiryoku = 1000000;
        LeftView.transform.FindChild("HP").transform.FindChild("hpValue").GetComponent<Text>().text = myJinkeiHeiryoku.ToString();


        //NCMB Data
        PvPDataStore = GameObject.Find("PvPDataStore").GetComponent<PvPDataStore>();
        isHPFetched = false;
        isMyPvPFetched = false;
        isAtkDfcFetched = false;
        isShowed = false;

        //1 time run
        PvPDataStore.GetPvPCount();
        PvPDataStore.GetPvPCountWeekly();

        //PvP
        List<float> perList = new List<float>() {0.7f, 0.8f, 0.9f, 1.0f, 1.2f, 1.4f,1.6f, 1.8f};
        int rdmId = UnityEngine.Random.Range(0, perList.Count);
        per = perList[rdmId];
        HpBase = Mathf.CeilToInt((float)myJinkeiHeiryoku * per);
        PvPDataStore.GetRandomEnemy(myUserId, HpBase);

        //PvP Top3 Ranking
        PvPDataStore.GetTop10Win();
        PvPDataStore.GetTop10HP();        
    }



    void Update() {

        /*** Total PvP Start ***/
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
            pvpCount = PvPDataStore.pvpCount;
            hpRank = PvPDataStore.hpRank;
            atkNo = PvPDataStore.atkNo;
            atkWinNo = PvPDataStore.atkWinNo;
            dfcNo = PvPDataStore.dfcNo;
            dfcWinNo = PvPDataStore.dfcWinNo;
            totalWinNo = PvPDataStore.totalWinNo;
            PvPDataStore.GetWinRank(totalWinNo);
            isAtkDfcFetched = true;
        }

        /* 自分の直近上位下位2位分の情報を取得する場合
        if(PvPDataStore.winRank > 0 && PvPDataStore.hpRank > 0 && !isNeighborsFerched) {
            isNeighborsFerched = true;
            PvPDataStore.GetNeighborsWin(PvPDataStore.winRank);
            PvPDataStore.GetNeighborsHP(PvPDataStore.hpRank);
        }
        */


        /*** Total PvP End ***/

        /*** Weekly PvP Start ***/
        if (PvPDataStore.pvpCountWeekly != -1 && !isMyPvPWeeklyFetched) {
            PvPDataStore.GetMyPvPWeekly();
            isMyPvPWeeklyFetched = true;
        }
        
        if (PvPDataStore.dfcWinNoWeekly != -1 && PvPDataStore.pvpCountWeekly != -1 && isMyPvPWeeklyFetched && !isAtkDfcWeeklyFetched) {
            pvpCountWeekly = PvPDataStore.pvpCountWeekly;
            atkNoWeekly = PvPDataStore.atkNoWeekly;
            atkWinNoWeekly = PvPDataStore.atkWinNoWeekly;
            dfcNoWeekly = PvPDataStore.dfcNoWeekly;
            dfcWinNoWeekly = PvPDataStore.dfcWinNoWeekly;
            totalWinNoWeekly = atkWinNoWeekly + dfcWinNoWeekly;
            PvPDataStore.GetWinRankWeekly(totalWinNoWeekly, false);            
            isAtkDfcWeeklyFetched = true;
        }
        /*** Weekly PvP End ***/

        /* Weekly Visualize Start */
        if (PvPDataStore.winRankWeekly != -1 && isMyPvPWeeklyFetched && isAtkDfcWeeklyFetched && !isShowed) {
            winRankWeekly = PvPDataStore.winRankWeekly;           
            LeftView.transform.FindChild("Win").transform.FindChild("winValue").GetComponent<Text>().text = totalWinNoWeekly.ToString();
            if(pvpCountWeekly< winRankWeekly) {
                winRankWeekly = pvpCountWeekly;
            }
            LeftView.transform.FindChild("Rank").transform.FindChild("rankValue").GetComponent<Text>().text = winRankWeekly.ToString();
            LeftView.transform.FindChild("Rank").transform.FindChild("rankTotalValue").GetComponent<Text>().text = "/" + pvpCountWeekly;

            //Active
            LeftView.transform.FindChild("Active").transform.FindChild("BtlValue").GetComponent<Text>().text = atkNoWeekly.ToString();
            LeftView.transform.FindChild("Active").transform.FindChild("WinValue").GetComponent<Text>().text = atkWinNoWeekly.ToString();
            int winAtkRatioWeekly = 0;
            if(atkNoWeekly != 0) {
                winAtkRatioWeekly = Mathf.FloorToInt((float)atkWinNoWeekly / (float)atkNoWeekly * 100);
            }
            LeftView.transform.FindChild("Active").transform.FindChild("RatioValue").GetComponent<Text>().text = winAtkRatioWeekly.ToString();

            //Passive
            LeftView.transform.FindChild("Passive").transform.FindChild("BtlValue").GetComponent<Text>().text = dfcNoWeekly.ToString();
            LeftView.transform.FindChild("Passive").transform.FindChild("WinValue").GetComponent<Text>().text = dfcWinNoWeekly.ToString();
            int winDfcRatioWeekly = 0;
            if (dfcNoWeekly != 0) {
                winDfcRatioWeekly = Mathf.FloorToInt((float)dfcWinNoWeekly / (float)dfcNoWeekly * 100);
            }
            LeftView.transform.FindChild("Passive").transform.FindChild("RatioValue").GetComponent<Text>().text = winDfcRatioWeekly.ToString();
            
            isShowed = true;
        }
        /* Weekly Visualize End */

        //PvP match
        if (PvPDataStore.matchedFlg && !showedFlg) {
            
            //PvP1          
                if(PvPDataStore.pvpUserNameList.Count > 0) { 
                if (!PvP1doneFlg) {
                    PvP1doneFlg = true;
                    PvP1 = GameObject.Find("1").gameObject;
                    PvP1.transform.FindChild("NameValue").GetComponent<Text>().text = PvPDataStore.pvpUserNameList[0];
                    PvP1.transform.FindChild("KuniLv").transform.FindChild("KuniLvValue").GetComponent<Text>().text = PvPDataStore.pvpKuniLvList[0].ToString();
                    PvP1.transform.FindChild("HP").transform.FindChild("Value").GetComponent<Text>().text = PvPDataStore.pvpHpList[0].ToString();
                    PvP1.transform.FindChild("Win").transform.FindChild("Value").GetComponent<Text>().text = PvPDataStore.pvpWinList[0].ToString();
                    PvPDataStore.GetWinRankWeekly(PvPDataStore.pvpWinList[0], true);

                    string objPath = "Prefabs/Player/" + PvPDataStore.pvpSoudaisyoList[0].ToString();
                    GameObject prefab = Instantiate(Resources.Load(objPath)) as GameObject;
                    prefab.transform.SetParent(PvP1.transform);
                    prefab.transform.localScale = new Vector2(-50,50);
                    prefab.transform.localPosition = new Vector2(-170, -20);
                    Animator anim = prefab.GetComponent("Animator") as Animator;
                    anim.SetBool("IsAttack", true);
                    deleteBusyoScript(prefab);
                    PvP1.transform.FindChild("StartBtn").GetComponent<StartKassenPvP>().pvpStageId = 1;
                    PvP1.transform.FindChild("StartBtn").GetComponent<StartKassenPvP>().userId = PvPDataStore.pvpUserIdList[0];
                    PvP1.transform.FindChild("StartBtn").GetComponent<StartKassenPvP>().enemyUserName = PvPDataStore.pvpUserNameList[0];
                }
                //PvP2
                if (PvPDataStore.pvpUserNameList.Count > 1) {
                    if (!PvP2doneFlg) {
                        PvP2doneFlg = true;
                        PvP2 = GameObject.Find("2").gameObject;
                        PvP2.transform.FindChild("NameValue").GetComponent<Text>().text = PvPDataStore.pvpUserNameList[1];
                        PvP2.transform.FindChild("KuniLv").transform.FindChild("KuniLvValue").GetComponent<Text>().text = PvPDataStore.pvpKuniLvList[1].ToString();
                        PvP2.transform.FindChild("HP").transform.FindChild("Value").GetComponent<Text>().text = PvPDataStore.pvpHpList[1].ToString();
                        PvP2.transform.FindChild("Win").transform.FindChild("Value").GetComponent<Text>().text = PvPDataStore.pvpWinList[1].ToString();
                        PvPDataStore.GetWinRankWeekly(PvPDataStore.pvpWinList[1], true);
                        string objPath = "Prefabs/Player/" + PvPDataStore.pvpSoudaisyoList[1].ToString();
                        GameObject prefab = Instantiate(Resources.Load(objPath)) as GameObject;
                        prefab.transform.SetParent(PvP2.transform);
                        prefab.transform.localScale = new Vector2(-50, 50);
                        prefab.transform.localPosition = new Vector2(-170, -20);
                        Animator anim = prefab.GetComponent("Animator") as Animator;
                        anim.SetBool("IsAttack", true);
                        deleteBusyoScript(prefab);
                        PvP2.transform.FindChild("StartBtn").GetComponent<StartKassenPvP>().pvpStageId = 2;
                        PvP2.transform.FindChild("StartBtn").GetComponent<StartKassenPvP>().userId = PvPDataStore.pvpUserIdList[1];
                        PvP2.transform.FindChild("StartBtn").GetComponent<StartKassenPvP>().enemyUserName = PvPDataStore.pvpUserNameList[1];

                    }

                    //PvP3
                    if (PvPDataStore.pvpUserNameList.Count > 2) {
                        if (!PvP3doneFlg) {
                            PvP3doneFlg = true;
                            PvP3 = GameObject.Find("3").gameObject;
                            PvP3.transform.FindChild("NameValue").GetComponent<Text>().text = PvPDataStore.pvpUserNameList[2];
                            PvP3.transform.FindChild("KuniLv").transform.FindChild("KuniLvValue").GetComponent<Text>().text = PvPDataStore.pvpKuniLvList[2].ToString();
                            PvP3.transform.FindChild("HP").transform.FindChild("Value").GetComponent<Text>().text = PvPDataStore.pvpHpList[2].ToString();
                            PvP3.transform.FindChild("Win").transform.FindChild("Value").GetComponent<Text>().text = PvPDataStore.pvpWinList[2].ToString();
                            PvPDataStore.GetWinRankWeekly(PvPDataStore.pvpWinList[2], true);
                            string objPath = "Prefabs/Player/" + PvPDataStore.pvpSoudaisyoList[2].ToString();
                            GameObject prefab = Instantiate(Resources.Load(objPath)) as GameObject;
                            prefab.transform.SetParent(PvP3.transform);
                            prefab.transform.localScale = new Vector2(-50, 50);
                            prefab.transform.localPosition = new Vector2(-170, -20);
                            Animator anim = prefab.GetComponent("Animator") as Animator;
                            anim.SetBool("IsAttack", true);
                            deleteBusyoScript(prefab);
                            PvP3.transform.FindChild("StartBtn").GetComponent<StartKassenPvP>().pvpStageId = 3;
                            PvP3.transform.FindChild("StartBtn").GetComponent<StartKassenPvP>().userId = PvPDataStore.pvpUserIdList[2];
                            PvP3.transform.FindChild("StartBtn").GetComponent<StartKassenPvP>().enemyUserName = PvPDataStore.pvpUserNameList[2];
                        }
                    }else {
                        Destroy(GameObject.Find("3").gameObject);
                    }
                }else {
                    Destroy(GameObject.Find("2").gameObject);
                    Destroy(GameObject.Find("3").gameObject);
                }
            }else {
                Destroy(GameObject.Find("1").gameObject);
                Destroy(GameObject.Find("2").gameObject); 
                Destroy(GameObject.Find("3").gameObject);

                string pathOfText = "Prefabs/PvP/NoEnemyText";
                GameObject txt = Instantiate(Resources.Load(pathOfText)) as GameObject;
                txt.transform.SetParent(GameObject.Find("KassenView").transform);
                txt.transform.localScale = new Vector2(0.5f, 0.5f);
                txt.transform.localPosition = new Vector2(350, 0);

            }
            GameObject.Find("RightView").GetComponent<ScrollRect>().enabled = false;
            showedFlg = true;
        }

        //PvP Enemy Ranking
        if (PvPDataStore.winRank != -1 && isHPFetched && isMyPvPFetched && isAtkDfcFetched && PvPDataStore.matchedFlg && showedFlg && PvPDataStore.pvpWinRankList.Count != 0 && !isEnemyPvPRankFetched) {
            isEnemyPvPRankFetched = true;
            if(PvPDataStore.pvpWinRankList.Count == 1) {
                int PvP1Rank = PvPDataStore.pvpWinRankList[0];
                if (pvpCount < PvP1Rank) PvP1Rank = pvpCount;
                PvP1.transform.FindChild("Rank").transform.FindChild("Value").GetComponent<Text>().text = PvP1Rank.ToString();
            }else if(PvPDataStore.pvpWinRankList.Count == 2) {
                int PvP1Rank = PvPDataStore.pvpWinRankList[0];
                if (pvpCount < PvP1Rank)PvP1Rank = pvpCount;               
                PvP1.transform.FindChild("Rank").transform.FindChild("Value").GetComponent<Text>().text = PvP1Rank.ToString();

                int PvP2Rank = PvPDataStore.pvpWinRankList[1];
                if (pvpCount < PvP2Rank)PvP2Rank = pvpCount;
                PvP2.transform.FindChild("Rank").transform.FindChild("Value").GetComponent<Text>().text = PvP2Rank.ToString();
            }else if(PvPDataStore.pvpWinRankList.Count == 3) {
                int PvP1Rank = PvPDataStore.pvpWinRankList[0];
                if (pvpCount < PvP1Rank) PvP1Rank = pvpCount;
                PvP1.transform.FindChild("Rank").transform.FindChild("Value").GetComponent<Text>().text = PvP1Rank.ToString();

                int PvP2Rank = PvPDataStore.pvpWinRankList[1];
                if (pvpCount < PvP2Rank) PvP2Rank = pvpCount;
                PvP2.transform.FindChild("Rank").transform.FindChild("Value").GetComponent<Text>().text = PvP2Rank.ToString();

                int PvP3Rank = PvPDataStore.pvpWinRankList[2];
                if (pvpCount < PvP3Rank) PvP3Rank = pvpCount;
                PvP3.transform.FindChild("Rank").transform.FindChild("Value").GetComponent<Text>().text = PvP3Rank.ToString();
            }            
        }

    }

    public void deleteBusyoScript(GameObject busyoObj) {
        if (busyoObj.GetComponent<PlayerHP>()) {
            Destroy(busyoObj.GetComponent<PlayerHP>());
        }
        if (busyoObj.GetComponent<Kunkou>()) {
            Destroy(busyoObj.GetComponent<Kunkou>());
        }
        if (busyoObj.GetComponent<UnitMover>()) {
            Destroy(busyoObj.GetComponent<UnitMover>());
        }
        if (busyoObj.GetComponent<Rigidbody2D>()) {
            Destroy(busyoObj.GetComponent<Rigidbody2D>());
        }
        if (busyoObj.GetComponent<LineLocation>()) {
            Destroy(busyoObj.GetComponent<LineLocation>());
        }
        if(busyoObj.GetComponent<PlayerAttack>()) {
            Destroy(busyoObj.GetComponent<PlayerAttack>());
        }else if(busyoObj.GetComponent<AttackLong>()) {
            Destroy(busyoObj.GetComponent<AttackLong>());
        }
        if (busyoObj.GetComponent<SenpouController>()) {
            Destroy(busyoObj.GetComponent<SenpouController>());
        }
        if (busyoObj.GetComponent<PolygonCollider2D>()) {
            Destroy(busyoObj.GetComponent<PolygonCollider2D>());
        }
        if (busyoObj.GetComponent<Heisyu>()) {
            Destroy(busyoObj.GetComponent<Heisyu>());
        }
    }

    public void ShowRank(GameObject rankViewObj) {
        GameObject WinContent = rankViewObj.transform.FindChild("LeftView").transform.FindChild("ScrollView").transform.FindChild("Viewport").transform.FindChild("Content").gameObject;
        GameObject HPContent = rankViewObj.transform.FindChild("RightView").transform.FindChild("ScrollView").transform.FindChild("Viewport").transform.FindChild("Content").gameObject;

        GameObject SlotWin = null;
        GameObject SlotHP = null;
        for (int i = 0; i < 10; i++) {

            int slotName = i + 1;
            SlotWin = WinContent.transform.FindChild(slotName.ToString()).gameObject;
            SlotHP = HPContent.transform.FindChild(slotName.ToString()).gameObject;
            
            //1st-3rd
            SlotWin.transform.FindChild("Name").GetComponent<Text>().text = PvPDataStore.Top3WinNameList[i];
            SlotHP.transform.FindChild("Name").GetComponent<Text>().text = PvPDataStore.Top3HPNameList[i];

            SlotWin.transform.FindChild("KuniLv").transform.FindChild("KuniLvValue").GetComponent<Text>().text = PvPDataStore.Top3WinRankList[i].ToString();
            SlotHP.transform.FindChild("KuniLv").transform.FindChild("KuniLvValue").GetComponent<Text>().text = PvPDataStore.Top3HPRankList[i].ToString();

            SlotWin.transform.FindChild("Win").transform.FindChild("winValue").GetComponent<Text>().text = PvPDataStore.Top3WinQtyList[i].ToString();
            SlotHP.transform.FindChild("HP").transform.FindChild("winValue").GetComponent<Text>().text = PvPDataStore.Top3HPQtyList[i].ToString();

            string imagePath1 = "Prefabs/Player/Sprite/unit" + PvPDataStore.Top3WinBusyoList[i].ToString();
            SlotWin.transform.FindChild("Image").GetComponent<Image>().sprite =
                Resources.Load(imagePath1, typeof(Sprite)) as Sprite;

            string imagePath2 = "Prefabs/Player/Sprite/unit" + PvPDataStore.Top3HPBusyoList[i].ToString();
            SlotHP.transform.FindChild("Image").GetComponent<Image>().sprite =
                Resources.Load(imagePath2, typeof(Sprite)) as Sprite;


            string imagePath3 = "Prefabs/Sashimono/" + PvPDataStore.Top3WinBusyoList[i].ToString();
            GameObject tmpObj = Resources.Load(imagePath3) as GameObject;
            SlotWin.transform.FindChild("Sashimono").GetComponent<Image>().sprite =
                tmpObj.GetComponent<SpriteRenderer>().sprite;

            string imagePath4 = "Prefabs/Sashimono/" + PvPDataStore.Top3HPBusyoList[i].ToString();
            GameObject tmpObj2 = Resources.Load(imagePath4) as GameObject;
            SlotHP.transform.FindChild("Sashimono").GetComponent<Image>().sprite =
                tmpObj2.GetComponent<SpriteRenderer>().sprite;


        }

        //Player
        SlotWin = rankViewObj.transform.FindChild("LeftView").transform.FindChild("Player").gameObject;
        SlotHP = rankViewObj.transform.FindChild("RightView").transform.FindChild("Player").gameObject;
        SlotWin.transform.FindChild("Name").GetComponent<Text>().text = myUserName;
        SlotHP.transform.FindChild("Name").GetComponent<Text>().text = myUserName;

        SlotWin.transform.FindChild("KuniLv").transform.FindChild("KuniLvValue").GetComponent<Text>().text = kuniLv.ToString();
        SlotHP.transform.FindChild("KuniLv").transform.FindChild("KuniLvValue").GetComponent<Text>().text = kuniLv.ToString();

        SlotWin.transform.FindChild("Win").transform.FindChild("winValue").GetComponent<Text>().text = totalWinNo.ToString();
        SlotHP.transform.FindChild("HP").transform.FindChild("winValue").GetComponent<Text>().text = myJinkeiHeiryoku.ToString();

        SlotWin.transform.FindChild("Rank").GetComponent<Text>().text = PvPDataStore.winRank.ToString();
        SlotHP.transform.FindChild("Rank").GetComponent<Text>().text = hpRank.ToString();
        
        string imagePlayerPath1 = "Prefabs/Player/Sprite/unit" + soudaisyoBusyoId.ToString();
        SlotWin.transform.FindChild("Image").GetComponent<Image>().sprite =
            Resources.Load(imagePlayerPath1, typeof(Sprite)) as Sprite;

        string imagePlayerPath2 = "Prefabs/Player/Sprite/unit" + soudaisyoBusyoId.ToString();
        SlotHP.transform.FindChild("Image").GetComponent<Image>().sprite =
            Resources.Load(imagePlayerPath2, typeof(Sprite)) as Sprite;

        string imagePlayerPath3 = "Prefabs/Sashimono/" + soudaisyoBusyoId.ToString();
        GameObject tmpObj3 = Resources.Load(imagePlayerPath3) as GameObject;
        SlotWin.transform.FindChild("Sashimono").GetComponent<Image>().sprite =
            tmpObj3.GetComponent<SpriteRenderer>().sprite;

        string imagePlayerPath4 = "Prefabs/Sashimono/" + soudaisyoBusyoId.ToString();
        GameObject tmpObj4 = Resources.Load(imagePlayerPath4) as GameObject;
        SlotHP.transform.FindChild("Sashimono").GetComponent<Image>().sprite =
            tmpObj4.GetComponent<SpriteRenderer>().sprite;


    }

}
