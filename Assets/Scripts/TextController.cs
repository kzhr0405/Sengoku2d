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

    public void Start() {

        if (Application.loadedLevelName == "tutorialMain") {
            tutorialId = PlayerPrefs.GetInt("tutorialId");
            if(tutorialId==3) {
                tutorialId = 4;
                PlayerPrefs.SetInt("tutorialId", tutorialId);
                PlayerPrefs.Flush();
            }
            if(tutorialId==12) {
                //to Kassen
                Application.LoadLevel("tutorialKassen");
            }
            if(tutorialId==14) {
                actOnFlg = false;
            }
        }else if (Application.loadedLevelName == "tutorialTouyou") {
            tutorialId = PlayerPrefs.GetInt("tutorialId");
            if (tutorialId == 5) {
                tutorialId = 6;

            }
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
                    GameObject Gunzei = SetGunzei();
                    gunzeiOnFlg = true;
                }
            }
            if(tutorialId == 13) {
                if (currentLine == 3) {
                    TutorialController tutorialScript = new TutorialController();
                    Vector2 vect = new Vector2(0, 0);
                    Vector2 size1 = new Vector2(60, 60);
                    Vector2 size2 = new Vector2(150, 150);
                    tutorialScript.SetDoublePointer(GameObject.Find("Video").gameObject, GameObject.Find("MainButtonView").transform.FindChild("Syounin").gameObject, vect, vect, size1, size2);
                    actOnFlg = true;
                }
            }

        }else {
            if (tutorialId == 13 && currentLine == 4) {
                actOnFlg = false;
            }else if (tutorialId == 14 && currentLine == 0) {
                actOnFlg = false;
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
        if(scenarios.Count > 0) {
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