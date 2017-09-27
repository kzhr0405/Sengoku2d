using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GacyaSpecialSelect : MonoBehaviour {

    public GameObject Button;
    public GameObject Detail;
    public bool onlyOneFlg;
    public bool selectFlg;
    public bool zukanExistFlg;
    public Color selectColor;
    public Color deselectColor;
    public int busyoId;
    public AudioSource[] audioSources;

    private void Start() {
        audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        selectColor = new Color(40f / 255f, 20f / 255f, 10f / 255f, 255f / 255f);
        deselectColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
    }


    public void OnClick() {

        audioSources[2].Play();

        if (Button.GetComponent<GacyaSpecialTouyou>().hireCount == 1) onlyOneFlg = true;

        if (onlyOneFlg) {
            foreach(Transform chld in transform.parent) {
                setDeselectColor(chld.gameObject);
                chld.GetComponent<GacyaSpecialSelect>().selectFlg = false;
            }
            selectFlg = true;
            visualizeBusyo(busyoId);
            setSelectColor(gameObject);
            Button.GetComponent<GacyaSpecialTouyou>().OnlyOneCount();
        }else {   
            if (selectFlg) {
                selectFlg = false;
                setDeselectColor(gameObject);
                Button.GetComponent<GacyaSpecialTouyou>().MinusCount();
            }else {
                if(Button.GetComponent<GacyaSpecialTouyou>().selectCount < Button.GetComponent<GacyaSpecialTouyou>().hireCount) {
                    selectFlg = true;
                    setSelectColor(gameObject);
                    Button.GetComponent<GacyaSpecialTouyou>().PlusCount();
                    visualizeBusyo(busyoId);
                }else {
                    audioSources[4].Play();
                    Message msg = new Message();
                    msg.makeMessage(msg.getMessage(156));
                }
            }
        }
    }

    public void setSelectColor(GameObject slot) {        
        slot.GetComponent<Image>().color = selectColor;
    }

    public void setDeselectColor(GameObject slot) {        
        slot.GetComponent<Image>().color = deselectColor;
    }

    public void visualizeBusyo(int busyoId) {

        string daimyoImagePath = "Prefabs/Player/Sprite/unit" + busyoId.ToString();
        Detail.GetComponent<Image>().sprite =
            Resources.Load(daimyoImagePath, typeof(Sprite)) as Sprite;
        Detail.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);

        StatusGet StatusGet = new StatusGet();
        int hp = StatusGet.getHp(busyoId,100);
        hp = hp * 100;
        int atk = StatusGet.getAtk(busyoId, 100);
        int dfc = StatusGet.getDfc(busyoId, 100);
        atk = atk * 10;
        dfc = dfc * 10;
        int spd = StatusGet.getSpd(busyoId, 100);
        string name = StatusGet.getBusyoName(busyoId);
        Saku saku = new Saku();
        Senpou senpou = new Senpou();
        BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
        int sakuId = BusyoInfoGet.getSakuId(busyoId);
        int senpouId = BusyoInfoGet.getSenpouId(busyoId);
        string sakuName = saku.getSakuName(sakuId);
        string senpouName = senpou.getName(sakuId);

        Detail.transform.FindChild("HP").transform.FindChild("Value").GetComponent<Text>().text = hp.ToString();
        Detail.transform.FindChild("ATK").transform.FindChild("Value").GetComponent<Text>().text = atk.ToString();
        Detail.transform.FindChild("DFC").transform.FindChild("Value").GetComponent<Text>().text = dfc.ToString();
        Detail.transform.FindChild("SPD").transform.FindChild("Value").GetComponent<Text>().text = spd.ToString();
        Detail.transform.FindChild("Name").GetComponent<Text>().text = name;
        Detail.transform.FindChild("Senpou").transform.FindChild("Value").GetComponent<Text>().text = senpouName;
        Detail.transform.FindChild("Saku").transform.FindChild("Value").GetComponent<Text>().text = sakuName;

        if (GameObject.Find("zukan")) {
            Destroy(GameObject.Find("zukan").gameObject);
        }
        if (zukanExistFlg) {
            string zukanPath = "Prefabs/Touyou/Zukan";
            GameObject zukan = Instantiate(Resources.Load(zukanPath)) as GameObject;
            zukan.name = "zukan";
            zukan.transform.SetParent(Detail.transform);
            zukan.transform.localScale = new Vector2(1, 1);
            zukan.transform.localPosition = new Vector3(-100, -40, 0);
        }
    }

}
