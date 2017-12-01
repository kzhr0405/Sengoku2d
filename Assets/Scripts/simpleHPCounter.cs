using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class simpleHPCounter : MonoBehaviour {

    public string targetTag = "";
    public int stageId = 0;
    public float life = 0;
    public SimpleHP hpSclipt;
    public TextMesh heiText;
    public bool finishFlg = false;
    public bool compFlg = false;
    public GameObject board;
    public float totalAtk = 0;
    public float totalDfc = 0;
    public TextMesh atkTxtScript;
    public TextMesh dfcTxtScript;
    public GameObject katanaBtnObj;
    public GameObject timer;
    public AudioSource[] audioSources;

    void Awake() {
        audioSources = GameObject.Find("SEController").GetComponents<AudioSource>(); GameObject.Find("SEController").GetComponents<AudioSource>();
        heiText = GetComponent<TextMesh>();
        atkTxtScript = transform.parent.Find("Atk").GetComponent<TextMesh>();
        dfcTxtScript = transform.parent.Find("Dfc").GetComponent<TextMesh>();
        //atkTxtScript = GameObject.Find(targetTag).transform.FindChild("Atk").GetComponent<TextMesh>();
        //dfcTxtScript = GameObject.Find(targetTag).transform.FindChild("Dfc").GetComponent<TextMesh>();
    }

    void Update () {
        if(!finishFlg) {
            life = getTotalHp(targetTag);
            if (life > 0) {
                heiText.text = life.ToString();
            } else if (life <= 0) {
                life = 0;
                heiText.text = life.ToString();
                finishFlg = true;
            }
        } else {   
            if(!compFlg) {

                heiText.text = "0";
                audioSources[11].Stop();

                compFlg = true;
                if (targetTag=="Enemy") {
                    //Player won
                    deleteKey();

                    //Effect
                    viewChar(true);
                    getWinItem();

                    audioSources[3].Play();
                }else {
                    //Enemy won

                    //Reduce Time to half
                    halfTime(stageId);

                    countRestart();

                    //Effect
                    viewChar(false);

                    audioSources[4].Play();

                }
            }   
        }
    }

    public float getTotalHp(string targetTag) {
        life = 0;
        totalAtk = 0;
        totalDfc = 0;
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(targetTag)) {
            life = life + obs.GetComponent<SimpleHP>().life;
            totalAtk = totalAtk + obs.GetComponent<SimpleAttack>().atk;
            totalDfc = totalDfc + obs.GetComponent<SimpleHP>().dfc;
        }

        viewAtkDfc(totalAtk, totalDfc,targetTag);

        return life;
    }

    public void viewChar(bool winFlg) {
        string path = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            path = "Prefabs/SimpleBattle/WinLoseTextEng";
        }else {
            path = "Prefabs/SimpleBattle/WinLoseText";
        }
        GameObject textObj = Instantiate(Resources.Load(path)) as GameObject;
        textObj.transform.SetParent(board.transform);
        textObj.transform.localPosition = new Vector2(0,0);
        textObj.transform.localScale = new Vector2(0.25f, 0.2f);
        textObj.GetComponent<Fadein>().destroyBoard = board;
 
        if (!winFlg) {
            if (langId == 2) {
                textObj.GetComponent<TextMesh>().text = "Lose";
            } else {
                textObj.GetComponent<TextMesh>().text = "敗北";
            }
        }
    }

    public void viewAtkDfc(float totalAtk, float totalDfc, string targetTag) {
        atkTxtScript.text = totalAtk.ToString();
        dfcTxtScript.text = totalDfc.ToString();
    }

    public void halfTime(int stageId) {

        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("StageGunzei")) {
            int toStageId = obs.GetComponent<TabStageGunzei>().toStageId;
            if (stageId == toStageId) {
                string key = obs.GetComponent<TabStageGunzei>().key;
                string deleteLabel = "remain" + key;

                float remainTime = PlayerPrefs.GetFloat(deleteLabel);
                float halfTime = remainTime / 2;
                if(halfTime<1) {
                    halfTime = 1;
                }
                PlayerPrefs.SetFloat(deleteLabel, halfTime);
                PlayerPrefs.Flush();
                //change remain time on board
                string remainTimeOnShiro = "stage" + toStageId.ToString();
                GameObject katanaObj = GameObject.Find(remainTimeOnShiro).transform.Find("cleared").transform.Find("enemyKatana").gameObject;
                katanaObj.GetComponent<ShiroAttack>().time = halfTime;

                string path = "Prefabs/EffectAnime/point_down";
                GameObject downObj = Instantiate(Resources.Load(path)) as GameObject;
                downObj.transform.SetParent(katanaObj.transform);
                downObj.transform.localScale = new Vector2(3,3);
                downObj.transform.localPosition = new Vector2(0, 1);

                break;
            }
        }


                
    }


    public void deleteKey() {
        int toStageId = 0;
        int fromStageId = 0;
        Color openColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f); //White

        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("StageGunzei")) {
            toStageId = obs.GetComponent<TabStageGunzei>().toStageId;

            if (stageId == toStageId) {
                string key = obs.GetComponent<TabStageGunzei>().key;
                toStageId = obs.GetComponent<TabStageGunzei>().toStageId;
                fromStageId = obs.GetComponent<TabStageGunzei>().fromStageId;

                //enable src stage
                string fromStageLable = "stage" + fromStageId.ToString();
                GameObject closeStageObj = GameObject.Find(fromStageLable).gameObject;
                closeStageObj.GetComponent<Button>().enabled = true;
                closeStageObj.transform.Find("shiroImage").GetComponent<SpriteRenderer>().color = openColor;

                string deleteLabel1 = "attack" + key;
                string deleteLabel2 = "remain" + key;
                PlayerPrefs.DeleteKey(deleteLabel1);
                PlayerPrefs.DeleteKey(deleteLabel2);
                Destroy(obs);
            }
        }
        PlayerPrefs.Flush();
        string stageLable = "stage" + stageId.ToString();
        if (GameObject.Find(stageLable).transform.Find("cleared").transform.Find("enemyKatana").gameObject) {
            GameObject katanaObj = GameObject.Find(stageLable).transform.Find("cleared").transform.Find("enemyKatana").gameObject;
            Destroy(katanaObj);
        }

    }

    public void countRestart() {
        if (GameObject.Find("enemyKatana")) {
            katanaBtnObj.GetComponent<Button>().enabled = true;
            katanaBtnObj = null;
        }

        timer.GetComponent<ShiroAttack>().rakujyoFlg = false;
    }

    public void getWinItem() {

        Message msg = new Message();

        AttackNaiseiView script = new AttackNaiseiView();
        HPCounter hpScript = new HPCounter();
        Item itemScript = new Item();
        string itemGrp = getRandomItemGrp();
        string itemTyp = "";
        int itemId = 0;
        int itemQty = 1;
        string itemName = "";
        string MsgText = "";
        int langId = PlayerPrefs.GetInt("langId");

        if (itemGrp == "item") {
            itemTyp = script.getRandomItemTyp(itemGrp);

            
            if (itemTyp == "tech") {
                itemId = script.getItemRank(30, 10);
                if (langId == 2) {
                    MsgText = "You got " + itemScript.getItemName(itemTyp + itemId.ToString()) + ".";
                }else {
                    MsgText = itemScript.getItemName(itemTyp + itemId.ToString()) + "を手に入れましたぞ。";
                }
                msg.makeMeshMessage(MsgText);
            }else if (itemTyp == "Tama") {
                itemId = script.getItemRank(10, 1);
                if (itemId == 3) {
                    itemQty = 100;
                }else if (itemId == 2) {
                    itemQty = 20;
                }else if (itemId == 1) {
                    itemQty = 5;
                }
                if (langId == 2) {
                    MsgText = "You got " + itemQty.ToString() + " stone.";
                }else {
                    MsgText = "武将珠を" + itemQty.ToString() + "個手に入れましたぞ。";
                }
                msg.makeMeshMessage(MsgText);
                
            }else {
                itemId = script.getItemRank(10, 1);
                if (langId == 2) {
                    MsgText = "You got " + itemScript.getItemName(itemTyp + itemId.ToString()) + ".";
                }else {
                    MsgText = itemScript.getItemName(itemTyp + itemId.ToString()) + "を手に入れましたぞ。";
                }
                msg.makeMeshMessage(MsgText);
            }   
        }else if (itemGrp == "kahou") {
            itemTyp = script.getRandomItemTyp(itemGrp);
            Kahou kahou = new Kahou();
            string kahouRank = getKahouRank();
            itemId = kahou.getRamdomKahouId(itemTyp, kahouRank);
            itemName = kahou.getKahouName(itemTyp, itemId);
            if (langId == 2) {
                MsgText = "You got treasure, " + itemName + ".";
            }else {
                MsgText = "家宝、" + itemName + "を手に入れましたぞ。";
            }
            msg.makeMeshMessage(MsgText);
            hpScript.addKahou(itemTyp, itemId);

        }else if(itemGrp == "money") {
            itemQty = UnityEngine.Random.Range(100, 500);
            if (langId == 2) {
                MsgText = "You got money " + itemQty.ToString() + ".";
            }else {
                MsgText = "金" + itemQty.ToString() + "を手に入れましたぞ。";
            }
            msg.makeMeshMessage(MsgText);

            int currentMoney = PlayerPrefs.GetInt("money");
            int newMoney = currentMoney + itemQty;
            if (newMoney < 0) {
                newMoney = int.MaxValue;
            }
            PlayerPrefs.SetInt("money",newMoney);
            GameObject.Find("MoneyValue").GetComponent<Text>().text = newMoney.ToString();

        }else {
           if (langId == 2) {
                MsgText = "No items";
            }else {
                MsgText = "戦利品はありませんでした。";
            }
            msg.makeMeshMessage(MsgText);
        }

        registerItemQty(itemGrp, itemTyp, itemId, itemQty);
        PlayerPrefs.Flush();
    }

    public string getRandomItemGrp() {
        string itemGrp = "no"; //no or item or kahou

        float percent = UnityEngine.Random.value;
        percent = percent * 100;
        
        
        if (percent <= 5) {
            itemGrp = "kahou";
        }else if (5 < percent && percent <= 30) {
            itemGrp = "item";
        }else if(30 < percent && percent <= 60) {
            itemGrp = "money";
        }
        
        return itemGrp;
    }

    public string getKahouRank() {
        string kahouRank = "C";

        float percent = UnityEngine.Random.value;
        percent = percent * 100;

        if (percent <= 1) {
            kahouRank = "S";
        }
        else if (1 < percent && percent <= 3) {
            kahouRank = "A";
        }
        else if (3 < percent && percent <= 20) {
            kahouRank = "B";
        }
        else if (20 < percent && percent <= 100) {
            kahouRank = "C";
        }

        return kahouRank;
    }

    public void registerItemQty(string itemGrp, string itemType, int itemId, int itemQty) {
        char[] delimiterChars = { ',' };

        if (itemGrp == "item") {
            //Cyouhei
            if (itemType.Contains("Cyouhei") == true) {
            string newCyouheiString = "";
            
            if (itemType.Contains("YR") == true) {
                string cyouheiString = PlayerPrefs.GetString("cyouheiYR");
                string[] cyouheiList = cyouheiString.Split(delimiterChars);
                if (itemId == 1) {
                    int tempQty = int.Parse(cyouheiList[0]);
                    tempQty = tempQty + 1;
                    newCyouheiString = tempQty.ToString() + "," + cyouheiList[1] + "," + cyouheiList[2];

                }
                else if (itemId == 2) {
                    int tempQty = int.Parse(cyouheiList[1]);
                    tempQty = tempQty + 1;
                    newCyouheiString = cyouheiList[0] + "," + tempQty.ToString() + "," + cyouheiList[2];

                }
                else if (itemId == 3) {
                    int tempQty = int.Parse(cyouheiList[2]);
                    tempQty = tempQty + 1;
                    newCyouheiString = cyouheiList[0] + "," + cyouheiList[1] + "," + tempQty.ToString();
                }
                    if (newCyouheiString != "") {
                        PlayerPrefs.SetString("cyouheiYR", newCyouheiString);
                    }
            }else if (itemType.Contains("KB") == true) {
                string cyouheiString = PlayerPrefs.GetString("cyouheiKB");
                string[] cyouheiList = cyouheiString.Split(delimiterChars);
                if (itemId == 1) {
                    int tempQty = int.Parse(cyouheiList[0]);
                    tempQty = tempQty + 1;
                    newCyouheiString = tempQty.ToString() + "," + cyouheiList[1] + "," + cyouheiList[2];

                }
                else if (itemId == 2) {
                    int tempQty = int.Parse(cyouheiList[1]);
                    tempQty = tempQty + 1;
                    newCyouheiString = cyouheiList[0] + "," + tempQty.ToString() + "," + cyouheiList[2];

                }
                else if (itemId == 3) {
                    int tempQty = int.Parse(cyouheiList[2]);
                    tempQty = tempQty + 1;
                    newCyouheiString = cyouheiList[0] + "," + cyouheiList[1] + "," + tempQty.ToString();
                }
                    if (newCyouheiString != "") {
                        PlayerPrefs.SetString("cyouheiKB", newCyouheiString);
                    }
            }else if (itemType.Contains("TP") == true) {
                string cyouheiString = PlayerPrefs.GetString("cyouheiTP");
                string[] cyouheiList = cyouheiString.Split(delimiterChars);
                if (itemId == 1) {
                    int tempQty = int.Parse(cyouheiList[0]);
                    tempQty = tempQty + 1;
                    newCyouheiString = tempQty.ToString() + "," + cyouheiList[1] + "," + cyouheiList[2];

                }
                else if (itemId == 2) {
                    int tempQty = int.Parse(cyouheiList[1]);
                    tempQty = tempQty + 1;
                    newCyouheiString = cyouheiList[0] + "," + tempQty.ToString() + "," + cyouheiList[2];

                }
                else if (itemId == 3) {
                    int tempQty = int.Parse(cyouheiList[2]);
                    tempQty = tempQty + 1;
                    newCyouheiString = cyouheiList[0] + "," + cyouheiList[1] + "," + tempQty.ToString();
                }
                if (newCyouheiString != "" ) {
                   PlayerPrefs.SetString("cyouheiTP", newCyouheiString);
                }
            }else if (itemType.Contains("YM") == true) {
                string cyouheiString = PlayerPrefs.GetString("cyouheiYM");
                string[] cyouheiList = cyouheiString.Split(delimiterChars);
                if (itemId == 1) {
                    int tempQty = int.Parse(cyouheiList[0]);
                    tempQty = tempQty + 1;
                    newCyouheiString = tempQty.ToString() + "," + cyouheiList[1] + "," + cyouheiList[2];

                }
                else if (itemId == 2) {
                    int tempQty = int.Parse(cyouheiList[1]);
                    tempQty = tempQty + 1;
                    newCyouheiString = cyouheiList[0] + "," + tempQty.ToString() + "," + cyouheiList[2];

                }
                else if (itemId == 3) {
                    int tempQty = int.Parse(cyouheiList[2]);
                    tempQty = tempQty + 1;
                    newCyouheiString = cyouheiList[0] + "," + cyouheiList[1] + "," + tempQty.ToString();
                }
                    if (newCyouheiString != "") {
                        PlayerPrefs.SetString("cyouheiYM", newCyouheiString);
                    }
            }


            //Kanjyo
        }else if (itemType == "Kanjyo") {
            
            string newKanjyoString = "";
            string kanjyoString = PlayerPrefs.GetString("kanjyo");
            string[] kanjyoList = kanjyoString.Split(delimiterChars);

            if (itemId == 1) {
                int tempQty = int.Parse(kanjyoList[0]);
                tempQty = tempQty + 1;
                newKanjyoString = tempQty.ToString() + "," + kanjyoList[1] + "," + kanjyoList[2];

            }
            else if (itemId == 2) {
                int tempQty = int.Parse(kanjyoList[1]);
                tempQty = tempQty + 1;
                newKanjyoString = kanjyoList[0] + "," + tempQty.ToString() + "," + kanjyoList[2];

            }
            else if (itemId == 3) {
                int tempQty = int.Parse(kanjyoList[2]);
                tempQty = tempQty + 1;
                newKanjyoString = kanjyoList[0] + "," + kanjyoList[1] + "," + tempQty.ToString();
            }
            PlayerPrefs.SetString("kanjyo", newKanjyoString);

            //Hidensyo
        }else if (itemType == "Hidensyo") {
            
            if (itemId == 1) {
                int hidensyoQty = PlayerPrefs.GetInt("hidensyoGe");
                hidensyoQty = hidensyoQty + 1;
                PlayerPrefs.SetInt("hidensyoGe", hidensyoQty);

            }
            else if (itemId == 2) {
                int hidensyoQty = PlayerPrefs.GetInt("hidensyoCyu");
                hidensyoQty = hidensyoQty + 1;
                PlayerPrefs.SetInt("hidensyoCyu", hidensyoQty);

            }
            else if (itemId == 3) {
                int hidensyoQty = PlayerPrefs.GetInt("hidensyoJyo");
                hidensyoQty = hidensyoQty + 1;
                PlayerPrefs.SetInt("hidensyoJyo", hidensyoQty);
            }

            //Shinobi
        }else if (itemType == "Shinobi") {
            
            if (itemId == 1) {
                int newQty = 0;
                int shinobiQty = PlayerPrefs.GetInt("shinobiGe");
                newQty = shinobiQty + 1;
                PlayerPrefs.SetInt("shinobiGe", newQty);

            }
            else if (itemId == 2) {
                int newQty = 0;
                int shinobiQty = PlayerPrefs.GetInt("shinobiCyu");
                newQty = shinobiQty + 1;
                PlayerPrefs.SetInt("shinobiCyu", newQty);

            }
            else if (itemId == 3) {
                int newQty = 0;
                int shinobiQty = PlayerPrefs.GetInt("shinobiJyo");
                newQty = shinobiQty + 1;
                PlayerPrefs.SetInt("shinobiJyo", newQty);

            }

            //tech
        }else if (itemType == "tech") {
            
            if (itemId == 1) {
                //TP
                int qty = PlayerPrefs.GetInt("transferTP", 0);
                int newQty = qty + 1;
                PlayerPrefs.SetInt("transferTP", newQty);
           
            }else if (itemId == 2) {
                int qty = PlayerPrefs.GetInt("transferKB", 0);
                int newQty = qty + 1;
                PlayerPrefs.SetInt("transferKB", newQty);
               
            }else if (itemId == 3) {
                int qty = PlayerPrefs.GetInt("transferSNB", 0);
                int newQty = qty + 1;
                PlayerPrefs.SetInt("transferSNB", newQty);

            }

            //cyoutei or koueki
        }else if (itemType == "cyoutei" || itemType == "koueki") {

            TabibitoItemGetter syoukaijyo = new TabibitoItemGetter();
            syoukaijyo.registerKouekiOrCyoutei(itemType, itemId);

        }else if (itemType == "Tama") {
            
            int nowQty = PlayerPrefs.GetInt("busyoDama");
            int newQty = nowQty + itemQty;
            PlayerPrefs.SetInt("busyoDama", newQty);
            GameObject.Find("BusyoDamaValue").GetComponent<Text>().text = newQty.ToString();
        }
        
    } else if (itemGrp == "kahou") {
            HPCounter addKahouScript = new HPCounter();

            //Register
            addKahouScript.addKahou(itemType, itemId);
    }
	PlayerPrefs.Flush ();
    }
}
