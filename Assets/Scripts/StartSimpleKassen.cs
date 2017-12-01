using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class StartSimpleKassen : MonoBehaviour {

    public List<GameObject> busyoObjList = new List<GameObject>();
    public bool playerLowerFlg = false;

	public void OnClick() {

        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();

        //hyourou check
        int hyourou = PlayerPrefs.GetInt("hyourou");
        if(hyourou>=3) {
            audioSources[0].Play();
            audioSources[7].Play();
            audioSources[9].Play();
            audioSources[11].Play();

            //Hyourou
            int newHyourou = hyourou - 3;
            PlayerPrefs.SetInt("hyourou", newHyourou);
            PlayerPrefs.Flush();
            GameObject.Find("HyourouCurrentValue").GetComponent<Text>().text = newHyourou.ToString();

            //Start
            /*
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Player")) {
                if (obs.GetComponent<Homing>()) {
                    obs.GetComponent<Homing>().enabled = true;
                    obs.transform.localScale = new Vector2(0.4f, 0.6f);
                }
            }
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Enemy")) {
                if (obs.GetComponent<Homing>()) {
                    obs.GetComponent<Homing>().enabled = true;
                    obs.transform.localScale = new Vector2(0.4f, 0.6f);
                }
            }
            */

            
            foreach (GameObject obj in busyoObjList) {
                if(obj!=null) {
                    if(obj.GetComponent<Homing>()) {
                        obj.GetComponent<Homing>().enabled = true;
                        obj.transform.localScale = new Vector2(0.4f, 0.6f);
                    }
                }
            }
            

            //Battle Powerup
            float myHei = (float)GameObject.Find("BattleBoard").transform.Find("Base").transform.Find("Player").transform.Find("Hei").GetComponent<simpleHPCounter>().life;
            float enemyHei = (float)GameObject.Find("BattleBoard").transform.Find("Base").transform.Find("Enemy").transform.Find("Hei").GetComponent<simpleHPCounter>().life;
            if(myHei > enemyHei) {
                //enemy flg on
                foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Enemy")) {
                    if (obs.GetComponent<SimpleAttack>()) {
                        obs.GetComponent<SimpleAttack>().lowerFlg = true;
                    }
                }
            }else {
                //player flg on
                foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Player")) {
                    if (obs.GetComponent<SimpleAttack>()) {
                        obs.GetComponent<SimpleAttack>().lowerFlg = true;
                    }
                }
            }
            
            //Button Disabled
            GameObject boardObj = transform.parent.gameObject;
            GameObject baseObj = boardObj.transform.Find("Base").gameObject;
            baseObj.transform.Find("YesButton").gameObject.SetActive(false);
            baseObj.transform.Find("NoButton").gameObject.SetActive(false);
            boardObj.transform.Find("Yes").GetComponent<Button>().enabled = false;
            boardObj.transform.Find("No").GetComponent<Button>().enabled = false;

            //Effect
            string pathBack = "Prefabs/PreKassen/backGroundSprite";
            GameObject back = Instantiate(Resources.Load(pathBack)) as GameObject;
            back.transform.localScale = new Vector2(5, 4);
            back.transform.localPosition = new Vector2(0,0);
            back.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
            back.GetComponent<SpriteRenderer>().sortingOrder = 500;
            foreach (Transform chObj in back.transform) {
                if (chObj.GetComponent<SpriteRenderer>()) {
                    chObj.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
                    chObj.GetComponent<SpriteRenderer>().sortingOrder = 501;
                }
            }
            
            string pathLight = "Prefabs/PreKassen/lightning";
            GameObject light = Instantiate(Resources.Load(pathLight)) as GameObject;
            light.transform.localScale = new Vector2(2, 2);
            light.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
            light.GetComponent<SpriteRenderer>().sortingOrder = 502;
        }else {
            audioSources[4].Play();

            Message msg = new Message();
            msg.makeMeshMessage(msg.getMessage(7));
        }
    }
}
