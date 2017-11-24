using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class ScenarioSelect : MonoBehaviour {

    public GameObject kuniIconView;
    public GameObject KuniMap;
    public GameObject ScrollView;
    public GameObject FixedMessage;
    public GameObject Back;

    public void OnClick() {
        //Show        
        showSeiryoku(int.Parse(name));
        Destroy(ScrollView);
        Destroy(GameObject.Find("Back"));
        Message Message = new Message();
        FixedMessage.transform.FindChild("MessageText").GetComponent<Text>().text = Message.getMessage(163);
    }

    public void showSeiryoku(int senarioId) {

        Entity_kuni_mst kuniMst = Resources.Load("Data/kuni_mst") as Entity_kuni_mst;
        KuniInfo kuniScript = new KuniInfo();
        Daimyo daimyoScript = new Daimyo();
        string kuniPath = "Prefabs/Map/kuni/";
        char[] delimiterChars = { ',' };
        int langId = PlayerPrefs.GetInt("langId");

        List<int> daimyoList = new List<int>();
        for (int i = 0; i < kuniMst.param.Count; i++) {
            int daimyoId = 0;
            if (senarioId == 1) {
                daimyoId = kuniMst.param[i].daimyoId1;
            }else if (senarioId == 2) {
                daimyoId = kuniMst.param[i].daimyoId2;
            }else if (senarioId == 3) {
                daimyoId = kuniMst.param[i].daimyoId3;
            }else {
                daimyoId = kuniMst.param[i].daimyoId;
            }
            if(daimyoId !=0)daimyoList.Add(daimyoId);                            
        }
        Dictionary<int, int> dic = new Dictionary<int, int>();
        foreach (int key in daimyoList) {
            if (dic.ContainsKey(key)) dic[key]++; else dic.Add(key, 1);
        }
        
        string gameClearDaimyo = PlayerPrefs.GetString("gameClearDaimyo");
        List<string> gameClearDaimyoList = new List<string>();
        if (gameClearDaimyo != null && gameClearDaimyo != "") {
            if (gameClearDaimyo.Contains(",")) {
                gameClearDaimyoList = new List<string>(gameClearDaimyo.Split(delimiterChars));
            }
            else {
                gameClearDaimyoList.Add(gameClearDaimyo);
            }
        }

        string myBusyo = PlayerPrefs.GetString("myBusyo");
        List<string> myBusyoList = new List<string>();
        if (myBusyo != null && myBusyo != "") {
            if (myBusyo.Contains(",")) {
                myBusyoList = new List<string>(myBusyo.Split(delimiterChars));
            }
            else {
                myBusyoList.Add(myBusyo);
            }
        }

        for (int i = 0; i < kuniMst.param.Count; i++) {
            int kuniId = kuniMst.param[i].kunId;

            string newKuniPath = kuniPath + kuniId.ToString();
            int locationX = kuniMst.param[i].locationX;
            int locationY = kuniMst.param[i].locationY;

            GameObject kuni = Instantiate(Resources.Load(newKuniPath)) as GameObject;

            kuni.transform.SetParent(kuniIconView.transform);
            kuni.name = kuniId.ToString();
            kuni.GetComponent<SendParam>().kuniId = kuniId;
            kuni.GetComponent<SendParam>().kuniName = kuniScript.getKuniName(kuniId, langId);
            kuni.transform.localScale = new Vector2(1, 1);
            kuni.GetComponent<SendParam>().naiseiItem = kuniMst.param[i].naisei;

            //Seiryoku Handling
            int daimyoId = kuniScript.getDaimyoId(kuniId, senarioId);      
            string daimyoName = daimyoScript.getName(daimyoId, langId, senarioId);
            int daimyoBusyoIdTemp = daimyoScript.getDaimyoBusyoId(daimyoId,senarioId);
            kuni.GetComponent<SendParam>().daimyoId = daimyoId;
            kuni.GetComponent<SendParam>().daimyoName = daimyoName;
            kuni.GetComponent<SendParam>().daimyoBusyoId = daimyoBusyoIdTemp;
            kuni.GetComponent<SendParam>().kuniQty = dic[daimyoId];

            if (gameClearDaimyoList.Contains(daimyoId.ToString())) {
                kuni.GetComponent<SendParam>().gameClearFlg = true;
            }

            if (myBusyoList.Contains(daimyoBusyoIdTemp.ToString())) {
                kuni.GetComponent<SendParam>().busyoHaveFlg = true;
            }

            //Color Handling
            float colorR = daimyoScript.getColorR(daimyoId);
            float colorG = daimyoScript.getColorG(daimyoId);
            float colorB = daimyoScript.getColorB(daimyoId);
            Color kuniColor = new Color(colorR / 255f, colorG / 255f, colorB / 255f, 255f / 255f);

            KuniMap.transform.FindChild(kuni.name).GetComponent<Image>().color = kuniColor;

            //Daimyo Kamon Image
            string imagePath = "Prefabs/Kamon/" + daimyoId.ToString();
            kuni.GetComponent<Image>().sprite =
                Resources.Load(imagePath, typeof(Sprite)) as Sprite;
            RectTransform kuniTransform = kuni.GetComponent<RectTransform>();
            kuniTransform.anchoredPosition = new Vector3(locationX, locationY, 0);
        }
        
    }

}
