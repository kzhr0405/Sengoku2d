using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class TouyouSpecialController : MonoBehaviour {

    public GameObject Gacya;
    public GameObject Panel;
    public Dictionary<int, Busyo> busyoListDic;
    public string targetHeisyu;
    public int targetDaimyoId;
    public AudioSource[] audioSources;
    public GameObject Slot;
    public GameObject Content;

    void Start () {
        //Common
        int busyoDama = PlayerPrefs.GetInt("busyoDama");
        GameObject.Find("BusyoDamaValue").GetComponent<Text>().text = busyoDama.ToString();
        Panel = GameObject.Find("Panel").gameObject;
        audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        
        //gacya busyo set
        Entity_busyo_mst busyoMst = Resources.Load("Data/busyo_mst") as Entity_busyo_mst;
        busyoListDic = new Dictionary<int, Busyo>();
        for (int i = 0; i < busyoMst.param.Count; i++) {
            int busyoId = busyoMst.param[i].id;            
            string rank = busyoMst.param[i].rank;
            string busyoName = busyoMst.param[i].name;
            string heisyu = busyoMst.param[i].heisyu;
            int daimyoId = busyoMst.param[i].daimyoId;
            int daimyoHst = busyoMst.param[i].daimyoHst;
            busyoListDic.Add(busyoId, new Busyo { busyoId = busyoId, busyoName= busyoName, rank = rank, heisyu = heisyu, daimyoId = daimyoId , daimyoHst = daimyoHst });
        }

        //test
        targetHeisyu = "KB";
        targetDaimyoId = 1;

    }

    public void doGacyaSpecial(string typ,int gacyaCount, int hireCount, Dictionary<int, Busyo> tmpBusyoListDic) {
        GameObject GacyaObj = Instantiate(Gacya);
        GacyaObj.transform.SetParent(Panel.transform,false);
        Gacya.SetActive(true);

        //delete chld
        Content = GacyaObj.transform.FindChild("Busyo").transform.FindChild("ScrollView").transform.FindChild("Content").gameObject;
        foreach (Transform n in Content.transform) {
            Destroy(n.gameObject);
        }

        //set parametor  
        List<Busyo> resultS = new List<Busyo>();
        List<Busyo> resultA = new List<Busyo>();
        List<Busyo> resultB = new List<Busyo>();

        Debug.Log(typ);
        if (typ == "heisyu") {            
            foreach (Busyo field in tmpBusyoListDic.Values) {
                if (field.heisyu == targetHeisyu) {
                    if(field.rank == "S") {
                        resultS.Add(field);
                    }else if (field.rank == "A") {
                        resultA.Add(field);
                    }else if (field.rank == "B") {
                        resultB.Add(field);
                    }
                }
            }
        }else if(typ=="daimyo") {
            foreach (Busyo field in tmpBusyoListDic.Values) {
                if (field.daimyoId == targetDaimyoId || field.daimyoHst == targetDaimyoId) {
                    if (field.rank == "S") {
                        resultS.Add(field);
                    }else if (field.rank == "A") {
                        resultA.Add(field);
                    }else if (field.rank == "B") {
                        resultB.Add(field);
                    }
                }
            }
        }else if(typ == "s") {
            foreach (Busyo field in tmpBusyoListDic.Values) {
                if (field.rank == "S") {
                    resultS.Add(field);
                }
            }
        }else {
            foreach (Busyo field in tmpBusyoListDic.Values) {
                if (field.rank == "S") {
                    resultS.Add(field);
                }else if (field.rank == "A") {
                    resultA.Add(field);
                }else if (field.rank == "B") {
                    resultB.Add(field);
                }
            }
        }
        
        //random draw
        List<Busyo> hitBusyo = new List<Busyo>();
        for(int i=0; i< gacyaCount; i++) {
            if (typ == "s") {
                int rdmId = UnityEngine.Random.Range(0, resultS.Count);
                hitBusyo.Add(resultS[rdmId]);
            }else {
                //S>0.5,A>10,B>89.5
                float percent = UnityEngine.Random.value;
                percent = percent * 100;
                if (percent <= 0.5f) {
                    //S
                    if(resultS.Count !=0) {
                        int rdmId = UnityEngine.Random.Range(0, resultS.Count);
                        hitBusyo.Add(resultS[rdmId]);
                    }else {
                        if(resultA.Count != 0) {
                            int rdmId = UnityEngine.Random.Range(0, resultA.Count);
                            hitBusyo.Add(resultA[rdmId]);
                        }else {
                            int rdmId = UnityEngine.Random.Range(0, resultB.Count);
                            hitBusyo.Add(resultB[rdmId]);
                        }
                    }
                }else if(0.5f<percent && percent <=10.5f) {
                    //A
                    if (resultA.Count != 0) {
                        int rdmId = UnityEngine.Random.Range(0, resultA.Count);
                        hitBusyo.Add(resultA[rdmId]);
                    }else {
                        int rdmId = UnityEngine.Random.Range(0, resultB.Count);
                        hitBusyo.Add(resultB[rdmId]);
                    }
                }else if (10.5f < percent) {
                    //B
                    if (resultB.Count != 0) {
                        int rdmId = UnityEngine.Random.Range(0, resultB.Count);
                        hitBusyo.Add(resultB[rdmId]);
                    }else {
                        int rdmId = UnityEngine.Random.Range(0, resultA.Count);
                        hitBusyo.Add(resultA[rdmId]);
                    }
                }
            }
        }
        Text countDown = GacyaObj.transform.FindChild("Anim").transform.FindChild("Text").GetComponent<Text>();
        GameObject Anim = GacyaObj.transform.FindChild("Anim").gameObject;
        StartCoroutine(loop(hitBusyo, countDown, Anim));



    }

    private IEnumerator loop(List<Busyo> hitBusyo, Text countDown,GameObject Anim) {
        while (true) {

            countDown.text = hitBusyo.Count.ToString();
            int busyoId = hitBusyo[0].busyoId;
            string rank = hitBusyo[0].rank;
            string busyoName = hitBusyo[0].busyoName;
            int daimyoId = hitBusyo[0].daimyoId;
            if(daimyoId ==0) daimyoId = hitBusyo[0].daimyoHst;
            string heisyu = hitBusyo[0].heisyu;
            hitBusyo.RemoveAt(0);
            audioSources[0].Play();

            if (rank == "S") {
            //    audioSources[0].Play();
            } else if (rank == "A") {
              //  audioSources[1].Play();
            } else if (rank == "B") {
                //audioSources[0].Play();                
            }

            //Set Busyo
            GameObject SlotObj = Instantiate(Slot);
            SlotObj.transform.SetParent(Content.transform, false);
            SlotObj.transform.FindChild("Name").GetComponent<Text>().text = busyoName;
            SlotObj.transform.FindChild("Rank").GetComponent<Text>().text = rank;
            string myDaimyoPath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
            SlotObj.transform.FindChild("Kamon").GetComponent<Image>().sprite =
                Resources.Load(myDaimyoPath, typeof(Sprite)) as Sprite;
            string myBusyoPath = "Prefabs/Player/Sprite/unit" + busyoId.ToString();
            SlotObj.transform.FindChild("Image").GetComponent<Image>().sprite =
                Resources.Load(myBusyoPath, typeof(Sprite)) as Sprite;

            if (hitBusyo.Count != 0) {
                    yield return new WaitForSeconds(0.1f);
                } else {
                    countDown.text = "0";                    
                    Destroy(Anim);
                    yield break;
                }



        }
    }


}
