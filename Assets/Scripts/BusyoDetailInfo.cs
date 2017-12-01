using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class BusyoDetailInfo : MonoBehaviour {

    public int busyoId = 0;
    public string busyoName = "";
    public int lv = 0;
    public int hp = 0;
    public int atk = 0;
    public int dfc = 0;
    public int spd = 0;
    public int chQty = 0;
    public string heisyu = "";
    public int daimyoId = 0;
    public int senpouId = 0;
    public int sakuId = 0;

    public void OnClick() {

        int langId = PlayerPrefs.GetInt("langId");

        if (Application.loadedLevelName != "tutorialHyojyo") {
            //Get Senryoku
            Senryoku Senryoku = null;
            foreach (Transform child in transform) {
                Senryoku = child.GetComponent<Senryoku>();
                busyoId = int.Parse(child.name);
            }
            BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
            busyoName = BusyoInfoGet.getName(busyoId, langId);
            hp = Senryoku.totalHp;
            atk = Senryoku.totalAtk;
            dfc = Senryoku.totalDfc;
            spd = Senryoku.totalSpd;
            daimyoId = Senryoku.belongDaimyoId;
            lv = Senryoku.lv;
            chQty = Senryoku.chQty;
            heisyu = BusyoInfoGet.getHeisyu(busyoId);
            sakuId = BusyoInfoGet.getSakuId(busyoId);


            AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
            audioSources[0].Play();

            string pathOfBack = "Prefabs/Common/TouchBack";
            GameObject back = Instantiate(Resources.Load(pathOfBack)) as GameObject;
            back.transform.parent = GameObject.Find("Panel").transform;
            back.transform.localScale = new Vector2(1, 1);
            back.transform.localPosition = new Vector2(0, 0);

            string pathOfPop = "Prefabs/Jinkei/busyoDetail";
            GameObject pop = Instantiate(Resources.Load(pathOfPop)) as GameObject;
            pop.transform.parent = GameObject.Find("Panel").transform;
            pop.transform.localScale = new Vector2(1, 1);
            pop.transform.localPosition = new Vector2(0, 0);

            //Kamon
            GameObject kamon = pop.transform.Find("kamon").gameObject;
            string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
            kamon.GetComponent<Image>().sprite =
                Resources.Load(imagePath, typeof(Sprite)) as Sprite;

            //Busyo Icon
            string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
            GameObject busyo = Instantiate(Resources.Load(busyoPath)) as GameObject;
            busyo.name = busyoId.ToString();
            busyo.transform.SetParent(pop.transform);
            busyo.transform.localScale = new Vector2(7, 7);
            busyo.GetComponent<DragHandler>().enabled = false;
            RectTransform busyoRect = busyo.GetComponent<RectTransform>();
            busyoRect.anchoredPosition3D = new Vector3(180, 400, 0);
            busyoRect.sizeDelta = new Vector2(40, 40);
            busyo.transform.Find("Text").GetComponent<Text>().enabled = false;

            //Ship Rank
            string shipPath = "Prefabs/Busyo/ShipSts";
            GameObject ShipObj = Instantiate(Resources.Load(shipPath)) as GameObject;
            ShipObj.transform.SetParent(busyo.transform);
            preKaisen kaisenScript = new preKaisen();
            int shipId = kaisenScript.getShipSprite(ShipObj, busyoId);
            ShipObj.transform.localPosition = new Vector3(-10, -15, 0);
            ShipObj.transform.localScale = new Vector2(0.2f, 0.2f);
            if (langId == 2) {
                if (shipId == 1) {
                    ShipObj.transform.Find("Text").GetComponent<Text>().text = "High";
                }
                else if (shipId == 2) {
                    ShipObj.transform.Find("Text").GetComponent<Text>().text = "Mid";
                }
                else if (shipId == 3) {
                    ShipObj.transform.Find("Text").GetComponent<Text>().text = "Low";
                }
            }
            else {
                if (shipId == 1) {
                    ShipObj.transform.Find("Text").GetComponent<Text>().text = "上";
                }
                else if (shipId == 2) {
                    ShipObj.transform.Find("Text").GetComponent<Text>().text = "中";
                }
                else if (shipId == 3) {
                    ShipObj.transform.Find("Text").GetComponent<Text>().text = "下";
                }
            }

            //Name
            pop.transform.Find("busyoNameValue").GetComponent<Text>().text = busyoName;
            pop.transform.Find("lvValue").GetComponent<Text>().text = lv.ToString();
            string heisyuName = "";
            Message msg = new Message();
            if (heisyu == "YR") {
                heisyuName = msg.getMessage(56);
            }else if (heisyu == "KB") {
                heisyuName = msg.getMessage(55);
            }else if (heisyu == "YM") {
                heisyuName = msg.getMessage(58);
            }else if (heisyu == "TP") {
                heisyuName = msg.getMessage(57);
            }
            pop.transform.Find("childNameValue").GetComponent<Text>().text = heisyuName;
            pop.transform.Find("childNum").GetComponent<Text>().text = chQty.ToString();        
            pop.transform.Find("hpValue").GetComponent<Text>().text = hp.ToString();
            pop.transform.Find("atkValue").GetComponent<Text>().text = atk.ToString();
            pop.transform.Find("dfcValue").GetComponent<Text>().text = dfc.ToString();
            pop.transform.Find("spdValue").GetComponent<Text>().text = spd.ToString();

            //Senpou
            StatusGet sts = new StatusGet();
            ArrayList senpouArray = sts.getOriginalSenpou(busyoId, false);
            int senpouId = (int)senpouArray[0];
            string senpouTyp = senpouArray[1].ToString();
            string senpouName = senpouArray[2].ToString();
            string senpouExp = senpouArray[3].ToString();
            float senpouEach = (float)senpouArray[4];
            float senpouRatio = (float)senpouArray[5];
            float senpouTerm = (float)senpouArray[6];
            int senpouStatus = (int)senpouArray[7];
            int senpouLv = (int)senpouArray[8];

            //Kahou Adjustment
            KahouStatusGet kahouSts = new KahouStatusGet();
            string[] KahouSenpouArray = kahouSts.getKahouForSenpou(busyoId.ToString(), senpouStatus);
            string kahouTyp = KahouSenpouArray[0];
            string adjSenpouStatus = senpouStatus.ToString();
            if (kahouTyp != null) {
                if (kahouTyp == "Attack") {
                    int kahouStatus = int.Parse(KahouSenpouArray[1]);
                    adjSenpouStatus = adjSenpouStatus + "<color=#35d74bFF>(+" + kahouStatus.ToString() + ")</color>";
                }else {
                    Debug.Log("Not Yet except for Attack");
                }
            }
            //Explanation Modification
            if (langId == 2) {
                senpouExp = senpouExp.Replace("ABC", adjSenpouStatus);
                senpouExp = senpouExp.Replace("DEF", senpouEach.ToString());
                senpouExp = senpouExp.Replace("GHI", senpouRatio.ToString());
                senpouExp = senpouExp.Replace("JKL", senpouTerm.ToString());
            }else {
                senpouExp = senpouExp.Replace("A", adjSenpouStatus);
                senpouExp = senpouExp.Replace("B", senpouEach.ToString());
                senpouExp = senpouExp.Replace("C", senpouRatio.ToString());
                senpouExp = senpouExp.Replace("D", senpouTerm.ToString());
            }

            //Fill fields by got Senpou Value
            pop.transform.Find("senpouNameValue").GetComponent<Text>().text = senpouName;
            pop.transform.Find("senpouExpValue").GetComponent<Text>().text = senpouExp;
            pop.transform.Find("senpouLvValue").GetComponent<Text>().text = senpouLv.ToString();
        

            //Saku
            Saku saku = new Saku();
            List<string> sakuList = new List<string>();
            sakuList = saku.getSakuInfo(busyoId);

            string sakuPath = "Prefabs/Saku/saku" + sakuList[0];
            GameObject sakuIcon = Instantiate(Resources.Load(sakuPath)) as GameObject;
            foreach (Transform n in pop.transform) {
                if (n.tag == "Saku") {
                    GameObject.Destroy(n.gameObject);
                }
            }
            sakuIcon.transform.SetParent(pop.transform);
            sakuIcon.transform.localScale = new Vector2(0.5f, 0.5f);
            sakuIcon.GetComponent<Button>().enabled = false;
            RectTransform sakuIcon_transform = sakuIcon.GetComponent<RectTransform>();
            sakuIcon_transform.anchoredPosition = new Vector3(-220, -185, 0);
            sakuIcon_transform.transform.SetSiblingIndex(30);
            pop.transform.Find("sakuExpValue").GetComponent<Text>().text = sakuList[2];
            pop.transform.Find("sakuLvValue").GetComponent<Text>().text = sakuList[3];

            //adjust
            if (Application.loadedLevelName == "preKaisen") {
                foreach (Transform chld in pop.transform) {
                    if (chld.GetComponent<ReplaceSpriteNameRank>()) {
                        string busyoImagePath = "Prefabs/Player/Sprite/unit" + busyoId;
                        chld.GetComponent<Image>().sprite =
                            Resources.Load(busyoImagePath, typeof(Sprite)) as Sprite;
                    }
                }
            }
        }
    }
}
