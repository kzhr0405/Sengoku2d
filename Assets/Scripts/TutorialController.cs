using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

    public int tutorialId = 0;

	void Start () {
        if (Application.loadedLevelName == "tutorialKassen") {
            StartCoroutine("taikoMusic");
        }
    }
	
	public void ActTutorial(int tutorialId) {
        
        if(tutorialId == 1) {
            //###To Naisei###
            //1. Kamon & Kuni Focus
            GameObject tButtonObj = GameObject.Find("tButton").gameObject;
            GameObject.Find("KuniMap").transform.FindChild("1").SetParent(tButtonObj.transform);
            GameObject.Find("KuniIconView").transform.FindChild("1").SetParent(tButtonObj.transform);

            Vector2 vect = new Vector2(20, 20);
            GameObject animObj = SetPointer(tButtonObj, vect);
            animObj.transform.localScale = new Vector2(100, 100);
        }else if(tutorialId ==2) {
            //2. Naise Shikichi Focus
            GameObject tButtonObj = GameObject.Find("tButton").gameObject;
            GameObject.Find("NaiseiView").transform.FindChild("12").SetParent(tButtonObj.transform);
            Vector2 vect = new Vector2(260, 150);
            GameObject animObj = SetPointer(tButtonObj, vect);
            animObj.transform.localScale = new Vector2(200,200);
        }else if (tutorialId == 3) {
            //3. Back to Main Focus
            GameObject buttonObj = GameObject.Find("Naisei").transform.FindChild("Panel").transform.FindChild("Button").gameObject;
            buttonObj.transform.SetParent(GameObject.Find("tButton").transform);
            Vector2 vect = new Vector2(0, 100);
            GameObject animObj = SetPointer(buttonObj, vect);
            animObj.transform.localScale = new Vector2(300, 300);

        }else if (tutorialId == 4) {
            //4. Cyosyu Focus
            GameObject SeiryokuInfoObj = GameObject.Find("SeiryokuInfo").gameObject;
            SeiryokuInfoObj.transform.SetParent(GameObject.Find("tButton").transform);
            Vector2 vect = new Vector2(0, 50);
            GameObject animObj = SetPointer(SeiryokuInfoObj, vect);
            animObj.transform.localScale = new Vector2(120, 120);

        }else if (tutorialId == 5) {
            //5. Touyou Focus
            GameObject btn = SetMainButton("Touyou");
            Vector2 vect = new Vector2(0, 50);
            GameObject animObj = SetPointer(btn, vect);
            animObj.transform.localScale = new Vector2(200, 200);

        }else if (tutorialId == 6) {
            //6. Touyou Button Focus
            GameObject original = GameObject.Find("BusyoDamaGacya");
            GameObject copied = Object.Instantiate(original) as GameObject;
            copied.name = "BusyoDamaGacya";
            copied.transform.SetParent(GameObject.Find("tButton").transform);
            copied.transform.localPosition = new Vector3(280,-240);
            copied.transform.localScale = new Vector3(1, 1);
            Vector2 vect = new Vector2(0, 50);
            GameObject animObj = SetPointer(copied, vect);
            animObj.transform.localScale = new Vector2(200, 200);
        }else if (tutorialId == 8) {
            //8. Back to Main button
            GameObject buttonObj = GameObject.Find("Panel").transform.FindChild("Button").gameObject;
            buttonObj.transform.SetParent(GameObject.Find("tButton").transform);
            Vector2 vect = new Vector2(0, 100);
            GameObject animObj = SetPointer(buttonObj, vect);
            animObj.transform.localScale = new Vector2(300, 300);

        }else if (tutorialId == 9) {
            GameObject btn = SetMainButton("Jinkei");
            Vector2 vect = new Vector2(0, 50);
            GameObject animObj = SetPointer(btn, vect);
            animObj.transform.localScale = new Vector2(200, 200);
        }else if (tutorialId == 10) {
            //10. Active Slot
            GameObject tBack = GameObject.Find("tBack").gameObject;
            GameObject sourceObj = GameObject.Find("ScrollView").transform.FindChild("Content").transform.FindChild("Slot").gameObject;
            sourceObj.transform.SetParent(tBack.transform);

            GameObject jinkeiView = GameObject.Find("JinkeiView").gameObject;
            GameObject copied = Object.Instantiate(jinkeiView) as GameObject;
            copied.transform.SetParent(GameObject.Find("Panel").transform,false);
            copied.name = "copiedJinkeiView";
            foreach (Transform chld in copied.transform) {
                if (chld.name == "Slot12") {
                    Destroy(chld.transform.FindChild("19").gameObject);
                }
                if (chld.name != "Slot13") {
                    chld.gameObject.SetActive(false);
                }
            }

            //Set Arrow
            string arrowPath = "Prefabs/PostKassen/Arrow";
            GameObject arrowObj = Instantiate(Resources.Load(arrowPath)) as GameObject;
            arrowObj.transform.SetParent(sourceObj.transform);
            arrowObj.transform.localPosition = new Vector2(120,0);
            arrowObj.transform.localScale = new Vector2(100, 100);
            arrowObj.transform.Rotate(new Vector3(0f, 0f, -30f));
            arrowObj.GetComponent<FadeoutArrowMove>().enabled = false;
        }else if (tutorialId == 12) {
            //Start Kassen
            string pathBack = "Prefabs/PreKassen/backGround";
            GameObject back = Instantiate(Resources.Load(pathBack)) as GameObject;
            back.transform.localScale = new Vector2(30, 15);

            string pathLight = "Prefabs/PreKassen/lightning";
            GameObject light = Instantiate(Resources.Load(pathLight)) as GameObject;
            light.transform.localScale = new Vector2(10, 10);

            StartEveryObject();

            Destroy(GameObject.Find("tBack").gameObject);

            GameObject.Find("timer").GetComponent<Timer>().paused = false;

        }
        else if (tutorialId == 13) {
            //Finalize
            AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
            audioSources[3].Play();

            PlayerPrefs.SetBool("tutorialDoneFlg",true);

            int busyoDama = PlayerPrefs.GetInt("busyoDama");
            busyoDama = busyoDama + 100;
            PlayerPrefs.SetInt("busyoDama", busyoDama);
            GameObject busyoDamaObj = GameObject.Find("BusyoDamaValue").gameObject;
            busyoDamaObj.GetComponent<Text>().text = busyoDama.ToString();
            Vector2 vect = new Vector2(500, 500);
            GameObject anim = SetFadeoutPointer(busyoDamaObj, vect);
            anim.transform.localScale = new Vector2(1000,1000);

            PlayerPrefs.SetInt("tutorialId", 14);

            TextController textScript = GameObject.Find("TextBoard").transform.FindChild("Text").GetComponent<TextController>();
            textScript.tutorialId = 14;
            textScript.actOnFlg = false;
            textScript.SetText(14);
            
            PlayerPrefs.Flush();
            
        }else if (tutorialId == 14) {
            GameObject.Find("KumoLeft").GetComponent<KumoMove>().runFlg = true;
            GameObject.Find("KumoRight").GetComponent<KumoMove>().runFlg = true;
            GameObject.Find("KumoLeft").GetComponent<KumoMove>().tutorialDoneFlg = true;
            GameObject.Find("KumoRight").GetComponent<KumoMove>().tutorialDoneFlg = true;
        }
    }


    public GameObject SetPointer(GameObject parentObj, Vector2 vect) {
        //delete previous point
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Tutorial")) {
            Destroy(obs);
        }
        
        string animPath = "Prefabs/EffectAnime/point_up";
        GameObject animObj = Instantiate(Resources.Load(animPath)) as GameObject;
        animObj.name = "point_up";
        animObj.tag = "Tutorial";
        animObj.transform.SetParent(parentObj.transform);
        animObj.transform.localPosition = vect;
        animObj.GetComponent<Fadeout>().enabled = false;

        return animObj;
    }

    public void SetDoublePointer(GameObject parentObj1, GameObject parentObj2, Vector2 vect1, Vector2 vect2, Vector2 size1, Vector2 size2) {
        //delete previous point
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Tutorial")) {
            Destroy(obs);
        }

        string animPath = "Prefabs/EffectAnime/point_up";
        GameObject animObj1 = Instantiate(Resources.Load(animPath)) as GameObject;
        animObj1.name = "point_up";
        animObj1.tag = "Tutorial";
        animObj1.transform.SetParent(parentObj1.transform);
        animObj1.transform.localPosition = vect1;
        animObj1.transform.localScale = size1;

        GameObject animObj2 = Instantiate(Resources.Load(animPath)) as GameObject;
        animObj2.name = "point_up";
        animObj2.tag = "Tutorial";
        animObj2.transform.SetParent(parentObj2.transform);
        animObj2.transform.localPosition = vect2;
        animObj2.transform.localScale = size2;

        animObj1.GetComponent<Fadeout>().fadeTime = 5.0f;
        animObj2.GetComponent<Fadeout>().fadeTime = 5.0f;

    }

    public GameObject SetFadeoutPointer(GameObject parentObj, Vector2 vect) {
        //delete previous point
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Tutorial")) {
            Destroy(obs);
        }

        string animPath = "Prefabs/EffectAnime/point_up";
        GameObject animObj = Instantiate(Resources.Load(animPath)) as GameObject;
        animObj.name = "point_up";
        animObj.tag = "Tutorial";
        animObj.transform.SetParent(parentObj.transform);
        animObj.transform.localPosition = vect;

        return animObj;
    }

    public GameObject SetMainButton(string targetButton) {
        GameObject original = GameObject.Find("MainButtonView").gameObject;
        GameObject copied = Object.Instantiate(original) as GameObject;
        copied.transform.SetParent(GameObject.Find("tButton").transform);
        copied.transform.localScale = new Vector2(1,1);
        copied.transform.localPosition = new Vector2(230, -340);

        GameObject returnObj = null;
        foreach (Transform chd in copied.transform) {
            if(chd.name != targetButton) {
                chd.GetComponent<Image>().enabled = false;
                chd.GetComponent<Button>().enabled = false;

            }else {
                returnObj = chd.gameObject;
            }
        }

        return returnObj;
    }

    public void StartEveryObject() {
        //Tutorial

        //Enemy
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) {
            if (obj.GetComponent<Homing>()) {
                obj.GetComponent<Homing>().enabled = true;
            }else {
                obj.GetComponent<HomingLong>().enabled = true;
            }
        }
        //Player
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player")) {
            if (obj.GetComponent<UnitMover>()) {
                obj.GetComponent<UnitMover>().enabled = true;
            }

        }

    }

    IEnumerator taikoMusic() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[9].Play(); //horagai
        audioSources[12].Play(); //bgm

        audioSources[5].Play();
        yield return new WaitForSeconds(0.5f);
        audioSources[5].Play();
        yield return new WaitForSeconds(0.5f);
        audioSources[5].Play();
        yield return new WaitForSeconds(0.5f);
        audioSources[5].Play();
    }


}
