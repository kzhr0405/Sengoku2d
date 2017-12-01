using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;
using UnityEngine.UI;

public class ZukanMenu : MonoBehaviour {

    public Color pushedTabColor = new Color(118f / 255f, 103f / 255f, 16f / 255f, 255f / 255f);
    public Color pushedTextColor = new Color(219f / 255f, 219f / 255f, 212f / 255f, 255f / 255f);
    public Color normalTabColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
    public Color normalTextColor = new Color(255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
    public bool pushedFlg = false;
    public GameObject Content1;
    public GameObject Content2;
    public GameObject Content3;
    public GameObject Qty1;
    public GameObject Qty2;
    public GameObject Qty3;

    public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[0].Play();
        int senarioId = PlayerPrefs.GetInt("senarioId");

        //Not Pushed Tab Color						
        if (!pushedFlg) {
            Resources.UnloadUnusedAssets();
            
            changeTabColor(name);

            if (name == "Busyo") {
                Content2.SetActive(false);
                Qty2.SetActive(false);
                
                Content3.SetActive(false);
                Qty3.SetActive(false);
                
                Content1.transform.parent.GetComponent<ScrollRect>().content = Content1.GetComponent<RectTransform>();
                Qty1.SetActive(true);
                if (Content1.transform.childCount > 0) {
                    Content1.SetActive(true);
                }else {
                    Content1.SetActive(true);
                    showBusyoZukan(Content1, senarioId);
                }
            }
            else if (name == "Kahou") {
                Content1.SetActive(false);
                Qty1.SetActive(false);

                Content3.SetActive(false);
                Qty3.SetActive(false);
                
                Content2.transform.parent.GetComponent<ScrollRect>().content = Content2.GetComponent<RectTransform>();
                Qty2.SetActive(true);
                if (Content2.transform.childCount > 0) {
                    Content2.SetActive(true);
                }else {
                    Content2.SetActive(true);
                    showKahouZukan(Content2);
                }
            }
            else if (name == "Tenkahubu") {
                Content1.SetActive(false);
                Qty1.SetActive(false);

                Content2.SetActive(false);
                Qty2.SetActive(false);

                Content3.transform.parent.GetComponent<ScrollRect>().content = Content3.GetComponent<RectTransform>();
                Qty3.SetActive(true);
                if (Content3.transform.childCount > 0) {
                    Content3.SetActive(true);
                }else {
                    Content3.SetActive(true);
                    showTenkahubuZukan(Content3);
                }
                
            }
        }
    }

    public void changeTabColor(string pushedTabName) {
        //Change Button Color				
        GameObject UpperView = GameObject.Find("UpperView").gameObject;
        foreach (Transform obj in UpperView.transform) {
            if (obj.name == pushedTabName) {
                obj.GetComponent<Image>().color = pushedTabColor;
                obj.transform.Find("Text").GetComponent<Text>().color = pushedTextColor;
                obj.GetComponent<ZukanMenu>().pushedFlg = true;
            }
            else {
                obj.GetComponent<Image>().color = normalTabColor;
                obj.transform.Find("Text").GetComponent<Text>().color = normalTextColor;
                obj.GetComponent<ZukanMenu>().pushedFlg = false;

            }
        }
    }




    public void showBusyoZukan(GameObject Content, int senarioId) {
        int langId = PlayerPrefs.GetInt("langId");
        //Prepare Master & History				
        Entity_busyo_mst tempBusyoMst = Resources.Load("Data/busyo_mst") as Entity_busyo_mst;
        string zukanBusyoHst = PlayerPrefs.GetString("zukanBusyoHst");
        List<string> zukanBusyoHstListTmp = new List<string>();
        List<string> zukanBusyoHstList = new List<string>();
        char[] delimiterChars = { ',' };
        if (zukanBusyoHst != null && zukanBusyoHst != "") {
            if (zukanBusyoHst.Contains(",")) {
                zukanBusyoHstListTmp = new List<string>(zukanBusyoHst.Split(delimiterChars));
            }
            else {
                zukanBusyoHstListTmp.Add(zukanBusyoHst);
            }
        }

        //remove dup						
        string newZukanBusyoHst = "";
        for (int i = 0; i < zukanBusyoHstListTmp.Count; i++) {
            string busyo = zukanBusyoHstListTmp[i];
            if (!zukanBusyoHstList.Contains(busyo)) {
                zukanBusyoHstList.Add(busyo);

                if (newZukanBusyoHst == "") {
                    newZukanBusyoHst = busyo;
                }
                else {
                    newZukanBusyoHst = newZukanBusyoHst + "," + busyo;
                }
            }
        }
        if (zukanBusyoHst != newZukanBusyoHst) PlayerPrefs.SetString("zukanBusyoHst", newZukanBusyoHst);


        //add temporary daimyo busyo						
        int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
        Daimyo daimyo = new Daimyo();
        int myDaimyoBusyoId = daimyo.getDaimyoBusyoId(myDaimyo, senarioId);
        if (!zukanBusyoHstList.Contains(myDaimyoBusyoId.ToString())) {
            zukanBusyoHstList.Add(myDaimyoBusyoId.ToString());
        }

        //Sort Master by daimyo						
        Entity_busyo_mst busyoMst = new Entity_busyo_mst();
        busyoMst.param.AddRange(tempBusyoMst.param);
        List<Busyo> busyoList = new List<Busyo>();
        BusyoInfoGet busyoScript = new BusyoInfoGet();
        for (int i = 0; i < busyoMst.param.Count; i++) {
            int busyoId = busyoMst.param[i].id;
            int daimyoId = busyoMst.param[i].daimyoId;
            if (daimyoId == 0) daimyoId = busyoMst.param[i].daimyoHst;
            string busyoName = busyoScript.getName(busyoId,langId);
            string busyoRank = busyoMst.param[i].rank;
            int rankId = 0;
            if (busyoRank == "S") {
                rankId = 0;
            }
            else if (busyoRank == "A") {
                rankId = 1;
            }
            else if (busyoRank == "B") {
                rankId = 2;
            }
            else if (busyoRank == "C") {
                rankId = 3;
            }
            float hp = busyoMst.param[i].hp;
            float atk = busyoMst.param[i].atk;
            float dfc = busyoMst.param[i].dfc;
            float spd = busyoMst.param[i].spd;
            string heisyu = busyoMst.param[i].heisyu;
            int senpouId = busyoMst.param[i].senpou_id;
            busyoList.Add(new Busyo(busyoId, busyoName, busyoRank, rankId, heisyu, daimyoId, daimyoId, 100, hp, atk, dfc, spd, senpouId, 0));

        }

        busyoList.Sort((a, b) => {
            int result = a.daimyoId - b.daimyoId;
            return result != 0 ? result : a.rankId - b.rankId;
        });


        //Show busyo						
        string path = "Prefabs/Zukan/zukanBusyo";
        string nameRankPath = "Prefabs/Zukan/NameRank";

        int NowQty = 0;
        int AllQty = 0;

        foreach (Busyo Busyo in busyoList) {
            int daimyoId = Busyo.daimyoId;
            if (daimyoId != 0) {
                AllQty = AllQty + 1;
                int busyoId = Busyo.busyoId;
                string busyoName = Busyo.busyoName;
                string busyoRank = Busyo.rank;

                GameObject back = Instantiate(Resources.Load(path)) as GameObject;
                back.transform.SetParent(Content.transform);
                back.transform.localScale = new Vector2(1, 1);
                back.transform.localPosition = new Vector3(0, 0, 0);

                GameObject kamon = back.transform.Find("kamon").gameObject;

                //Busyo Icon		
                string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
                GameObject busyo = Instantiate(Resources.Load(busyoPath)) as GameObject;
                busyo.name = busyoId.ToString();
                busyo.transform.SetParent(back.transform);
                busyo.transform.localScale = new Vector2(4, 4);
                busyo.GetComponent<DragHandler>().enabled = false;
                foreach (Transform n in busyo.transform) {
                    GameObject.Destroy(n.gameObject);
                }
                RectTransform busyoRect = busyo.GetComponent<RectTransform>();
                busyoRect.anchoredPosition3D = new Vector3(80, 80, 0);
                busyoRect.sizeDelta = new Vector2(40, 40);

                //Kamon		
                string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                kamon.GetComponent<Image>().sprite =
                    Resources.Load(imagePath, typeof(Sprite)) as Sprite;

                //Name		
                GameObject nameRank = Instantiate(Resources.Load(nameRankPath)) as GameObject;
                nameRank.transform.SetParent(back.transform);
                nameRank.transform.localScale = new Vector2(1, 1);
                nameRank.transform.localPosition = new Vector3(0, 0, 0);


                GameObject rank = nameRank.transform.Find("Rank").gameObject;
                rank.GetComponent<Text>().text = busyoRank;


                //Have or not		
                if (zukanBusyoHstList.Contains(busyoId.ToString())) {
                    NowQty = NowQty + 1;

                    GameObject name = nameRank.transform.Find("Name").gameObject;
                    name.GetComponent<Text>().text = busyoName;

                    float hp = Busyo.hp;
                    float atk = Busyo.atk;
                    float dfc = Busyo.dfc;
                    float spd = Busyo.spd;
                    string heisyu = Busyo.heisyu;
                    int senpouId = Busyo.senpouId;

                    PopInfo popScript = back.GetComponent<PopInfo>();
                    popScript.busyoId = busyoId;
                    popScript.busyoName = busyoName;
                    popScript.hp = (int)hp;
                    popScript.atk = (int)atk;
                    popScript.dfc = (int)dfc;
                    popScript.spd = (int)spd;
                    popScript.heisyu = heisyu;
                    popScript.daimyoId = daimyoId;
                    popScript.senpouId = senpouId;

                }
                else {
                    Color noBusyoColor = new Color(0f / 255f, 0f / 255f, 0f / 255f, 255f / 255f); //Black	
                    Color backColor = new Color(95f / 255f, 95f / 255f, 95f / 255f, 240f / 255f); //Black	
                    Color kamonColor = new Color(105f / 255f, 105f / 255f, 105f / 255f, 135f / 255f); //Black	

                    back.GetComponent<Image>().color = backColor;
                    back.GetComponent<Button>().enabled = false;
                    busyo.GetComponent<Image>().color = noBusyoColor;
                    kamon.GetComponent<Image>().color = kamonColor;
                }


            }

        }

        //Qty				
        float percent = (float)NowQty / (float)AllQty * 100;
        GameObject.Find("Qty1").GetComponent<Text>().text = NowQty.ToString() + " / " + AllQty.ToString() + "(" + percent.ToString("F1") + "%)";

    }

    public void showKahouZukan(GameObject Content) {

        int NowQty = 0;

        Entity_kahou_bugu_mst tmpbuguMst = Resources.Load("Data/kahou_bugu_mst") as Entity_kahou_bugu_mst;
        Entity_kahou_gusoku_mst tmpgusokuMst = Resources.Load("Data/kahou_gusoku_mst") as Entity_kahou_gusoku_mst;
        Entity_kahou_kabuto_mst tmpkabutoMst = Resources.Load("Data/kahou_kabuto_mst") as Entity_kahou_kabuto_mst;
        Entity_kahou_meiba_mst tmpmeibaMst = Resources.Load("Data/kahou_meiba_mst") as Entity_kahou_meiba_mst;
        Entity_kahou_cyadougu_mst tmpcyadouguMst = Resources.Load("Data/kahou_cyadougu_mst") as Entity_kahou_cyadougu_mst;
        Entity_kahou_chishikisyo_mst tmpchishikisyoMst = Resources.Load("Data/kahou_chishikisyo_mst") as Entity_kahou_chishikisyo_mst;
        Entity_kahou_heihousyo_mst tmpheihousyoMst = Resources.Load("Data/kahou_heihousyo_mst") as Entity_kahou_heihousyo_mst;

        Entity_kahou_bugu_mst buguMst = new Entity_kahou_bugu_mst();
        Entity_kahou_gusoku_mst gusokuMst = new Entity_kahou_gusoku_mst();
        Entity_kahou_kabuto_mst kabutoMst = new Entity_kahou_kabuto_mst();
        Entity_kahou_meiba_mst meibaMst = new Entity_kahou_meiba_mst();
        Entity_kahou_cyadougu_mst cyadouguMst = new Entity_kahou_cyadougu_mst();
        Entity_kahou_chishikisyo_mst chishikisyoMst = new Entity_kahou_chishikisyo_mst();
        Entity_kahou_heihousyo_mst heihousyoMst = new Entity_kahou_heihousyo_mst();

        buguMst.param.AddRange(tmpbuguMst.param);
        gusokuMst.param.AddRange(tmpgusokuMst.param);
        kabutoMst.param.AddRange(tmpkabutoMst.param);
        meibaMst.param.AddRange(tmpmeibaMst.param);
        cyadouguMst.param.AddRange(tmpcyadouguMst.param);
        chishikisyoMst.param.AddRange(tmpchishikisyoMst.param);
        heihousyoMst.param.AddRange(tmpheihousyoMst.param);


        //Bugu				
        //Prepare Master & History				
        string zukanBuguHst = PlayerPrefs.GetString("zukanBuguHst");
        List<string> zukanBuguHstList = new List<string>();
        char[] delimiterChars = { ',' };

        if (zukanBuguHst != "" && zukanBuguHst != null) {
            if (zukanBuguHst.Contains(",")) {
                zukanBuguHstList = new List<string>(zukanBuguHst.Split(delimiterChars));
            }
            else {
                zukanBuguHstList.Add(zukanBuguHst);
            }
        }

        //Sort Master by daimyo				
        buguMst.param.Sort((x, y) => { return x.kahouRank.CompareTo(y.kahouRank); });

        //Show Kahou				
        string noBuguPath = "Prefabs/Item/Sprite/NoBugu";
        for (int i = 0; i < buguMst.param.Count; i++) {
            int kahouId = buguMst.param[i].id;

            //Get Status			
            string kahouPath = "Prefabs/Item/Kahou/bugu" + kahouId;
            GameObject kahouIcon = Instantiate(Resources.Load(kahouPath)) as GameObject;
            kahouIcon.transform.SetParent(Content.transform);
            kahouIcon.transform.localScale = new Vector2(1, 1);
            kahouIcon.transform.localPosition = new Vector3(0, 0, 0);

            if (!zukanBuguHstList.Contains(kahouId.ToString())) {
                //Don't have		
                kahouIcon.GetComponent<Image>().sprite =
                    Resources.Load(noBuguPath, typeof(Sprite)) as Sprite;

                kahouIcon.GetComponent<Button>().enabled = false;
            }
            else {
                NowQty = NowQty + 1;
                kahouIcon.GetComponent<KahouInfo>().kahouType = "bugu";
                kahouIcon.GetComponent<KahouInfo>().kahouId = kahouId;
            }

        }

        //Gusoku				
        //Prepare Master & History				
        string zukanGusokuHst = PlayerPrefs.GetString("zukanGusokuHst");
        List<string> zukanGusokuHstList = new List<string>();
        if (zukanGusokuHst != "" && zukanGusokuHst != null) {
            if (zukanGusokuHst.Contains(",")) {
                zukanGusokuHstList = new List<string>(zukanGusokuHst.Split(delimiterChars));
            }
            else {
                zukanGusokuHstList.Add(zukanGusokuHst);
            }
        }

        //Sort Master by daimyo				
        gusokuMst.param.Sort((a, b) => { return a.kahouRank.CompareTo(b.kahouRank); });

        //Show Kahou				
        string noGusokuPath = "Prefabs/Item/Sprite/NoGusoku";
        for (int i = 0; i < gusokuMst.param.Count; i++) {
            int kahouId = gusokuMst.param[i].id;

            string kahouPath = "Prefabs/Item/Kahou/gusoku" + kahouId;
            GameObject kahouIcon = Instantiate(Resources.Load(kahouPath)) as GameObject;
            kahouIcon.transform.SetParent(Content.transform);
            kahouIcon.transform.localScale = new Vector2(1, 1);
            kahouIcon.transform.localPosition = new Vector3(0, 0, 0);

            if (!zukanGusokuHstList.Contains(kahouId.ToString())) {
                //Don't have		
                kahouIcon.GetComponent<Image>().sprite =
                    Resources.Load(noGusokuPath, typeof(Sprite)) as Sprite;

                kahouIcon.GetComponent<Button>().enabled = false;
            }
            else {
                NowQty = NowQty + 1;
                kahouIcon.GetComponent<KahouInfo>().kahouType = "gusoku";
                kahouIcon.GetComponent<KahouInfo>().kahouId = kahouId;
            }
        }


        //Kabuto				
        //Prepare Master & History				
        string zukanKabutoHst = PlayerPrefs.GetString("zukanKabutoHst");
        List<string> zukanKabutoHstList = new List<string>();

        if (zukanKabutoHst != "" && zukanKabutoHst != null) {
            if (zukanKabutoHst.Contains(",")) {
                zukanKabutoHstList = new List<string>(zukanKabutoHst.Split(delimiterChars));
            }
            else {
                zukanKabutoHstList.Add(zukanKabutoHst);
            }
        }

        //Sort Master by daimyo				
        kabutoMst.param.Sort((x, y) => { return x.kahouRank.CompareTo(y.kahouRank); });

        //Show Kahou				
        string noKabutoPath = "Prefabs/Item/Sprite/NoKabuto";
        for (int i = 0; i < kabutoMst.param.Count; i++) {
            int kahouId = kabutoMst.param[i].id;

            string kahouPath = "Prefabs/Item/Kahou/kabuto" + kahouId;
            GameObject kahouIcon = Instantiate(Resources.Load(kahouPath)) as GameObject;
            kahouIcon.transform.SetParent(Content.transform);
            kahouIcon.transform.localScale = new Vector2(1, 1);
            kahouIcon.transform.localPosition = new Vector3(0, 0, 0);

            if (!zukanKabutoHstList.Contains(kahouId.ToString())) {
                //Don't have		
                kahouIcon.GetComponent<Image>().sprite =
                    Resources.Load(noKabutoPath, typeof(Sprite)) as Sprite;

                kahouIcon.GetComponent<Button>().enabled = false;
            }
            else {
                NowQty = NowQty + 1;
                kahouIcon.GetComponent<KahouInfo>().kahouType = "kabuto";
                kahouIcon.GetComponent<KahouInfo>().kahouId = kahouId;
            }
        }


        // Meiba				
        //Prepare Master & History				
        string zukanMeibaHst = PlayerPrefs.GetString("zukanMeibaHst");
        List<string> zukanMeibaHstList = new List<string>();

        if (zukanMeibaHst != "" && zukanMeibaHst != null) {
            if (zukanMeibaHst.Contains(",")) {
                zukanMeibaHstList = new List<string>(zukanMeibaHst.Split(delimiterChars));
            }
            else {
                zukanMeibaHstList.Add(zukanMeibaHst);
            }
        }

        //Sort Master by daimyo				
        meibaMst.param.Sort((x, y) => { return x.kahouRank.CompareTo(y.kahouRank); });

        //Show Kahou				
        string noMeibaPath = "Prefabs/Item/Sprite/NoMeiba";
        for (int i = 0; i < meibaMst.param.Count; i++) {
            int kahouId = meibaMst.param[i].id;

            string kahouPath = "Prefabs/Item/Kahou/meiba" + kahouId;
            GameObject kahouIcon = Instantiate(Resources.Load(kahouPath)) as GameObject;
            kahouIcon.transform.SetParent(Content.transform);
            kahouIcon.transform.localScale = new Vector2(1, 1);
            kahouIcon.transform.localPosition = new Vector3(0, 0, 0);

            if (!zukanMeibaHstList.Contains(kahouId.ToString())) {
                //Don't have		
                kahouIcon.GetComponent<Image>().sprite =
                    Resources.Load(noMeibaPath, typeof(Sprite)) as Sprite;

                kahouIcon.GetComponent<Button>().enabled = false;
            }
            else {
                NowQty = NowQty + 1;
                kahouIcon.GetComponent<KahouInfo>().kahouType = "meiba";
                kahouIcon.GetComponent<KahouInfo>().kahouId = kahouId;
            }
        }



        //Cyadougu				
        //Prepare Master & History				
        string zukanCyadouguHst = PlayerPrefs.GetString("zukanCyadouguHst");
        List<string> zukanCyadouguHstList = new List<string>();

        if (zukanCyadouguHst != "" && zukanCyadouguHst != null) {
            if (zukanCyadouguHst.Contains(",")) {
                zukanCyadouguHstList = new List<string>(zukanCyadouguHst.Split(delimiterChars));
            }
            else {
                zukanCyadouguHstList.Add(zukanCyadouguHst);
            }
        }

        //Sort Master by daimyo				
        cyadouguMst.param.Sort((x, y) => { return x.kahouRank.CompareTo(y.kahouRank); });

        //Show Kahou				
        string noCyadouguPath = "Prefabs/Item/Sprite/NoCyadougu";
        for (int i = 0; i < cyadouguMst.param.Count; i++) {
            int kahouId = cyadouguMst.param[i].id;

            string kahouPath = "Prefabs/Item/Kahou/cyadougu" + kahouId;
            GameObject kahouIcon = Instantiate(Resources.Load(kahouPath)) as GameObject;
            kahouIcon.transform.SetParent(Content.transform);
            kahouIcon.transform.localScale = new Vector2(1, 1);
            kahouIcon.transform.localPosition = new Vector3(0, 0, 0);

            if (!zukanCyadouguHstList.Contains(kahouId.ToString())) {
                //Don't have		
                kahouIcon.GetComponent<Image>().sprite =
                    Resources.Load(noCyadouguPath, typeof(Sprite)) as Sprite;

                kahouIcon.GetComponent<Button>().enabled = false;
            }
            else {
                NowQty = NowQty + 1;
                kahouIcon.GetComponent<KahouInfo>().kahouType = "cyadougu";
                kahouIcon.GetComponent<KahouInfo>().kahouId = kahouId;
            }
        }


        // Chishikisyo				
        //Prepare Master & History				
        string zukanChishikisyoHst = PlayerPrefs.GetString("zukanChishikisyoHst");
        List<string> zukanChishikisyoHstList = new List<string>();

        if (zukanChishikisyoHst != "" && zukanChishikisyoHst != null) {
            if (zukanChishikisyoHst.Contains(",")) {
                zukanChishikisyoHstList = new List<string>(zukanChishikisyoHst.Split(delimiterChars));
            }
            else {
                zukanChishikisyoHstList.Add(zukanChishikisyoHst);
            }
        }

        //Sort Master by daimyo				
        chishikisyoMst.param.Sort((x, y) => { return x.kahouRank.CompareTo(y.kahouRank); });

        //Show Kahou				
        string noChishikisyoPath = "Prefabs/Item/Sprite/NoChishikisyo";
        for (int i = 0; i < chishikisyoMst.param.Count; i++) {
            int kahouId = chishikisyoMst.param[i].id;

            string kahouPath = "Prefabs/Item/Kahou/chishikisyo" + kahouId;
            GameObject kahouIcon = Instantiate(Resources.Load(kahouPath)) as GameObject;
            kahouIcon.transform.SetParent(Content.transform);
            kahouIcon.transform.localScale = new Vector2(1, 1);
            kahouIcon.transform.localPosition = new Vector3(0, 0, 0);

            if (!zukanChishikisyoHstList.Contains(kahouId.ToString())) {
                //Don't have		
                kahouIcon.GetComponent<Image>().sprite =
                    Resources.Load(noChishikisyoPath, typeof(Sprite)) as Sprite;

                kahouIcon.GetComponent<Button>().enabled = false;
            }
            else {
                NowQty = NowQty + 1;

                kahouIcon.GetComponent<KahouInfo>().kahouType = "chishikisyo";
                kahouIcon.GetComponent<KahouInfo>().kahouId = kahouId;

            }
        }



        // Heihousyo				
        //Prepare Master & History				
        string zukanHeihousyoHst = PlayerPrefs.GetString("zukanHeihousyoHst");
        List<string> zukanHeihousyoHstList = new List<string>();

        if (zukanHeihousyoHst != "" && zukanHeihousyoHst != null) {
            if (zukanHeihousyoHst.Contains(",")) {
                zukanHeihousyoHstList = new List<string>(zukanHeihousyoHst.Split(delimiterChars));
            }
            else {
                zukanHeihousyoHstList.Add(zukanHeihousyoHst);
            }
        }

        //Sort Master by daimyo				
        heihousyoMst.param.Sort((x, y) => { return x.kahouRank.CompareTo(y.kahouRank); });

        //Show Kahou				
        string noHeihousyoPath = "Prefabs/Item/Sprite/NoHeihousyo";
        for (int i = 0; i < heihousyoMst.param.Count; i++) {
            int kahouId = heihousyoMst.param[i].id;

            string kahouPath = "Prefabs/Item/Kahou/heihousyo" + kahouId;
            GameObject kahouIcon = Instantiate(Resources.Load(kahouPath)) as GameObject;
            kahouIcon.transform.SetParent(Content.transform);
            kahouIcon.transform.localScale = new Vector2(1, 1);
            kahouIcon.transform.localPosition = new Vector3(0, 0, 0);

            if (!zukanHeihousyoHstList.Contains(kahouId.ToString())) {
                //Don't have		
                kahouIcon.GetComponent<Image>().sprite =
                    Resources.Load(noHeihousyoPath, typeof(Sprite)) as Sprite;

                kahouIcon.GetComponent<Button>().enabled = false;
            }
            else {
                NowQty = NowQty + 1;

                kahouIcon.GetComponent<KahouInfo>().kahouType = "heihousyo";
                kahouIcon.GetComponent<KahouInfo>().kahouId = kahouId;
            }
        }



        //Qty				
        int AllQty = buguMst.param.Count + gusokuMst.param.Count + kabutoMst.param.Count + meibaMst.param.Count + cyadouguMst.param.Count + chishikisyoMst.param.Count + heihousyoMst.param.Count;
        float percent = (float)NowQty / (float)AllQty * 100;
        GameObject.Find("Qty2").GetComponent<Text>().text = NowQty.ToString() + " / " + AllQty.ToString() + "(" + percent.ToString("F1") + "%)";

    }

    public void showTenkahubuZukan(GameObject Content) {
        int langId = PlayerPrefs.GetInt("langId");
        int senarioId = PlayerPrefs.GetInt("senarioId");
        Entity_daimyo_mst daimyoMst = Resources.Load("Data/daimyo_mst") as Entity_daimyo_mst;
        Color backColor = new Color(95f / 255f, 95f / 255f, 95f / 255f, 210f / 255f); //Black				
        Color kamonColor = new Color(105f / 255f, 105f / 255f, 105f / 255f, 135f / 255f); //Black				

        string gameClearDaimyo = PlayerPrefs.GetString("gameClearDaimyo");
        List<string> gameClearDaimyoList = new List<string>();
        char[] delimiterChars = { ',' };
        if (gameClearDaimyo != null && gameClearDaimyo != "") {
            if (gameClearDaimyo.Contains(",")) {
                gameClearDaimyoList = new List<string>(gameClearDaimyo.Split(delimiterChars));
            }
            else {
                gameClearDaimyoList.Add(gameClearDaimyo);
            }
        }

        string gameClearDaimyoHard = PlayerPrefs.GetString("gameClearDaimyoHard");
        List<string> gameClearDaimyoHardList = new List<string>();
        if (gameClearDaimyoHard != null && gameClearDaimyoHard != "") {
            if (gameClearDaimyoHard.Contains(",")) {
                gameClearDaimyoHardList = new List<string>(gameClearDaimyoHard.Split(delimiterChars));
            }
            else {
                gameClearDaimyoHardList.Add(gameClearDaimyoHard);
            }
        }

        string tenkahubuPath = "Prefabs/Item/Tenkahubu/tenkahubu";
        string nameWhitePath = "Prefabs/Zukan/NameWhite";
        string nameBlackPath = "Prefabs/Zukan/NameBlack";
        Daimyo Daimyo = new Daimyo();
        for (int i = 0; i < daimyoMst.param.Count; i++) {
            int daimyoId = daimyoMst.param[i].daimyoId;
            GameObject tenkahubuIcon = Instantiate(Resources.Load(tenkahubuPath)) as GameObject;
            tenkahubuIcon.transform.SetParent(Content.transform);
            tenkahubuIcon.transform.localScale = new Vector2(1, 1);
            tenkahubuIcon.transform.localPosition = new Vector3(0, 0, 0);

            GameObject kamonIcon = tenkahubuIcon.transform.Find("kamon").gameObject;
            string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
            kamonIcon.GetComponent<Image>().sprite =
                Resources.Load(imagePath, typeof(Sprite)) as Sprite;

            RectTransform kamonRect = kamonIcon.GetComponent<RectTransform>();
            kamonRect.anchoredPosition3D = new Vector3(80, -95, 0);
            kamonRect.sizeDelta = new Vector3(100, 100, 0);

            GameObject tenkaIcon = tenkahubuIcon.transform.Find("Image").gameObject;
            RectTransform tenkaRect = tenkaIcon.GetComponent<RectTransform>();
            tenkaRect.anchoredPosition3D = new Vector3(20, 10, 0);
            tenkaRect.sizeDelta = new Vector3(80, 100, 0);
            
            if (!gameClearDaimyoList.Contains(daimyoId.ToString())) {
                tenkahubuIcon.GetComponent<Image>().color = backColor;
                kamonIcon.GetComponent<Image>().color = kamonColor;
                tenkahubuIcon.transform.Find("Image").GetComponent<Image>().enabled = false;

                GameObject nameObj = Instantiate(Resources.Load(nameWhitePath)) as GameObject;
                nameObj.transform.SetParent(tenkahubuIcon.transform);
                nameObj.transform.localScale = new Vector2(0.25f, 0.25f);
                
                nameObj.GetComponent<Text>().text = Daimyo.getName(i, langId, senarioId);
                
                nameObj.transform.localPosition = new Vector2(0, 60);
                Destroy(tenkahubuIcon.transform.Find("Hard").gameObject);

            }
            else {
                GameObject nameObj = Instantiate(Resources.Load(nameBlackPath)) as GameObject;
                nameObj.transform.SetParent(tenkahubuIcon.transform);
                nameObj.transform.localScale = new Vector2(0.25f, 0.25f);
                nameObj.GetComponent<Text>().text = Daimyo.getName(i, langId, senarioId);
                nameObj.transform.localPosition = new Vector2(0, 60);


                if (!gameClearDaimyoHardList.Contains((daimyoId.ToString()))) {
                    Destroy(tenkahubuIcon.transform.Find("Hard").gameObject);
                }


            }


        }

        //Qty				
        int NowQty = gameClearDaimyoList.Count;
        int AllQty = daimyoMst.param.Count;
        float percent = (float)NowQty / (float)AllQty * 100;
        GameObject.Find("Qty3").GetComponent<Text>().text = NowQty.ToString() + " / " + AllQty.ToString() + "(" + percent.ToString("F1") + "%)";


    }



}
