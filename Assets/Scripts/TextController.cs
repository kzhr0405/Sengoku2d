using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class TextController : MonoBehaviour {
    //public string[] scenarios;
    public List<string> scenarios;
    [SerializeField]
    Text uiText;

    [SerializeField]
    [Range(0.001f, 0.3f)]
    float intervalForCharacterDisplay = 0.01f;  // 1文字の表示にかかる時間

    public int currentLine = 0;
    private string currentText = string.Empty;  // 現在の文字列
    private float timeUntilDisplay = 0;     // 表示にかかる時間
    private float timeElapsed = 1;          // 文字列の表示を開始した時間
    private int lastUpdateCharacter = -1;       // 表示中の文字数

    public int tutorialId = 0;
    public bool actOnFlg = false;
    public bool gunzeiOnFlg = false;
    public bool doneFlg = false;

    public void Start() {
        if (Application.loadedLevelName == "tutorialMain") {
            tutorialId = PlayerPrefs.GetInt("tutorialId");
            if(tutorialId==3) {
                tutorialId = 4;
                PlayerPrefs.SetInt("tutorialId", tutorialId);
                PlayerPrefs.Flush();
            }
            if(tutorialId== 14) {
                //to Kassen
                Application.LoadLevel("tutorialKassen");
            }
            if(tutorialId==15) {
                actOnFlg = false;
            }           
        }else if (Application.loadedLevelName == "tutorialTouyou") {
            tutorialId = PlayerPrefs.GetInt("tutorialId");
            if (tutorialId == 5) {
                tutorialId = 6;

            }
        }else if (Application.loadedLevelName == "tutorialKassen") {
            tutorialId = PlayerPrefs.GetInt("tutorialId");

        }
        
        SetText(tutorialId);
        SetNextLine();
        
    }

    void Update() {

        if (currentLine < scenarios.Count && Input.GetMouseButtonDown(0)) {
            SetNextLine();
        }

        //Tutorial Action
        if(!actOnFlg) {
            if(currentLine == scenarios.Count) {
                TutorialController tutorialScript = new TutorialController();
                tutorialScript.ActTutorial(tutorialId);
                actOnFlg = true;
            }
            if (tutorialId ==5 && currentLine == 1) {
                if(!gunzeiOnFlg) {
                    AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
                    audioSources[9].Play();
                    GameObject Gunzei = SetGunzei();
                    gunzeiOnFlg = true;
                }
            }
            if(tutorialId == 10) {
                //busyo 
                TutorialController tutorialScript = new TutorialController();
                Color enabledColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
                Color disabledColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 255f / 255f);

                if (currentLine == 1) {                   
                    GameObject tBack = GameObject.Find("tBack").gameObject;
                    GameObject BusyoStatus = GameObject.Find("BusyoStatus").gameObject;
                    BusyoStatus.transform.FindChild("StatusLv").transform.SetParent(tBack.transform);
                    BusyoStatus.transform.FindChild("StatusTosotsu").transform.SetParent(tBack.transform);
                    BusyoStatus.transform.FindChild("StatusBuyuu").transform.SetParent(tBack.transform);
                    BusyoStatus.transform.FindChild("StatusChiryaku").transform.SetParent(tBack.transform);
                    BusyoStatus.transform.FindChild("StatusSpeed").transform.SetParent(tBack.transform);
                    actOnFlg = true;
                }else if(currentLine == 5) {
                    GameObject tBack = GameObject.Find("tBack").gameObject;
                    GameObject BusyoStatus = GameObject.Find("BusyoStatus").gameObject;
                    tBack.transform.FindChild("StatusLv").transform.SetParent(BusyoStatus.transform);
                    tBack.transform.FindChild("StatusTosotsu").transform.SetParent(BusyoStatus.transform);
                    tBack.transform.FindChild("StatusBuyuu").transform.SetParent(BusyoStatus.transform);
                    tBack.transform.FindChild("StatusChiryaku").transform.SetParent(BusyoStatus.transform);
                    tBack.transform.FindChild("StatusSpeed").transform.SetParent(BusyoStatus.transform);
                    GameObject.Find("ButaiStatus").transform.SetParent(tBack.transform);                   
                    GameObject btn1 = GameObject.Find("ButtonCyouhei").gameObject;
                    GameObject btn2 = GameObject.Find("ButtonKunren").gameObject;
                    btn1.GetComponent<Button>().enabled = false;
                    btn2.GetComponent<Button>().enabled = false;
                    btn1.GetComponent<Image>().color = disabledColor;
                    btn2.GetComponent<Image>().color = disabledColor;
                    btn1.transform.FindChild("Text").GetComponent<Text>().color = disabledColor;
                    btn2.transform.FindChild("Text").GetComponent<Text>().color = disabledColor;

                    actOnFlg = true;
                }else if (currentLine == 7) {
                    GameObject btn = GameObject.Find("ButtonCyouhei").gameObject;
                    btn.GetComponent<Button>().enabled = true;
                    Vector2 vect = new Vector2(0, 50);
                    GameObject animObj = tutorialScript.SetPointer(btn, vect);
                    animObj.transform.localScale = new Vector2(100, 100);

                    btn.GetComponent<Button>().enabled = true;
                    btn.GetComponent<Image>().color = enabledColor;
                    btn.transform.FindChild("Text").GetComponent<Text>().color = enabledColor;
                    actOnFlg = true;
                }
            }
            
            if (tutorialId == 15) {
                if (currentLine == 3) {
                    TutorialController tutorialScript = new TutorialController();
                    Vector2 vect = new Vector2(0, 0);
                    Vector2 size1 = new Vector2(60, 60);
                    Vector2 size2 = new Vector2(150, 150);
                    tutorialScript.SetDoublePointer(GameObject.Find("Video").gameObject, GameObject.Find("MainButtonView").transform.FindChild("Syounin").gameObject, vect, vect, size1, size2);
                    actOnFlg = true;
                }else if(currentLine == 5){
                    TutorialController tutorialScript = new TutorialController();
                    GameObject btn = GameObject.Find("PvP").gameObject;
                    Vector2 vect = new Vector2(0, 50);
                    GameObject animObj = tutorialScript.SetFadeoutPointer(btn, vect);
                    animObj.transform.localScale = new Vector2(200, 200);
                    actOnFlg = true;
                }
            }

        }else {

            if (tutorialId == 10) {
                if(currentLine == 4) {
                    actOnFlg = false;
                }else if(currentLine == 6) {
                    actOnFlg = false;
                }
            }

            if (tutorialId == 15) {
                if (currentLine == 0 ||currentLine == 4 || currentLine == 6) {
                    actOnFlg = false;
                }
            }
            if (tutorialId == 16 && !doneFlg) {
                actOnFlg = false;
                doneFlg = true;
            }
        }
        

        // クリックから経過した時間が想定表示時間の何%か確認し、表示文字数を出す
        int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);

        // 表示文字数が前回の表示文字数と異なるならテキストを更新する
        if (displayCharacterCount != lastUpdateCharacter) {
            uiText.text = currentText.Substring(0, displayCharacterCount);
            lastUpdateCharacter = displayCharacterCount;
        }

    }


    public void SetNextLine() {

        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[2].Play();

        currentText = scenarios[currentLine];
        currentLine++;

        // 想定表示時間と現在の時刻をキャッシュ
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;

        // 文字カウントを初期化
        lastUpdateCharacter = -1;


    }

    public void SetText(int tutorialId) {
        if (scenarios.Count > 0) {
            scenarios.Clear();
        }
        currentLine = 0;
        Entity_tutorial_mst tutorialMst = Resources.Load("Data/tutorial_mst") as Entity_tutorial_mst;
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            for (int i = 0; i < tutorialMst.param.Count; i++) {
                if (tutorialId == tutorialMst.param[i].tutorialId) {                    
                    scenarios.Add(tutorialMst.param[i].serihuEng);
                }
            }
        }else {
            for (int i = 0; i < tutorialMst.param.Count; i++) {
                if (tutorialId == tutorialMst.param[i].tutorialId) {
                    scenarios.Add(tutorialMst.param[i].serihu);                    
                }
            }
        }
        if(tutorialId==15) {
            bool tutorialDoneFlg = PlayerPrefs.GetBool("tutorialDoneFlg");
            if (tutorialDoneFlg) {
                if (Application.systemLanguage == SystemLanguage.Japanese) {
                    scenarios[6] = "そうそう、山城国には朝廷が、摂津和泉や河内、筑前国には商人がおりますぞ。特別な紹介状が必要ですが様々なことが出来るようになります。";
                }else {
                    scenarios[6] = "…Well, There is royal court in Yamashiro country and also merchant in Settsu Izumi, Kawachi, Chikuzen country. You can do several requests to them.";
                }
            }
        } 
    }

    public GameObject SetGunzei() {
        string path = "Prefabs/Map/Gunzei";
        GameObject Gunzei = Instantiate(Resources.Load(path)) as GameObject;
        Gunzei.transform.SetParent(GameObject.Find("tButton").transform);

        Gunzei.transform.localScale = new Vector2(-1.2f, 1.2f);
        Gunzei.transform.localPosition = new Vector3(30, -50,0);

        Gunzei.GetComponent<Gunzei>().atkFlg = true;

        string animPath = "Prefabs/EffectAnime/point_up";
        GameObject pointUp = Instantiate(Resources.Load(animPath)) as GameObject;
        pointUp.transform.SetParent(Gunzei.transform);
        pointUp.transform.localScale = new Vector2(100, 100);
        pointUp.transform.localPosition = new Vector3(0, 30, 0);

        return Gunzei;
    }


}