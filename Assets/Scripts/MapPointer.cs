using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPointer : MonoBehaviour {

	
	// Update is called once per frame
	public void createKassenPointer (int daimyoId) {
        GameObject KuniIconView = GameObject.Find("KuniIconView").gameObject;
        string path = "Prefabs/Map/pointer/KassenPointer";

        foreach (Transform obs in KuniIconView.transform) {
            if(obs.GetComponent<SendParam>().daimyoId == daimyoId) {
                GameObject pointer = Instantiate(Resources.Load(path)) as GameObject;
                pointer.transform.SetParent(obs.transform);
                pointer.transform.localScale = new Vector2(0.6f,0.8f);
                pointer.transform.localPosition = new Vector2(0, 0);

            }
        }
    }
    public void createUpPointer(int daimyoId1, int daimyoId2) {
        GameObject KuniIconView = GameObject.Find("KuniIconView").gameObject;
        string path = "Prefabs/Map/pointer/upPointer";

        float randomB = Random.Range(0, 200);
        Color randomColor = new Color(255f / 255f, 0 / 255f, randomB / 255f, 200f / 255f);

        foreach (Transform obs in KuniIconView.transform) {
            if (obs.GetComponent<SendParam>().daimyoId == daimyoId1 || obs.GetComponent<SendParam>().daimyoId == daimyoId2) {
                GameObject pointer = Instantiate(Resources.Load(path)) as GameObject;
                pointer.transform.SetParent(obs.transform);
                pointer.transform.localScale = new Vector2(0.5f, 0.5f);
                pointer.transform.localPosition = new Vector2(0, 0);

                //Random Color              
                pointer.GetComponent<Image>().color = randomColor;

            }
        }
    }
    public void createDownPointer(int daimyoId1, int daimyoId2) {
        GameObject KuniIconView = GameObject.Find("KuniIconView").gameObject;
        string path = "Prefabs/Map/pointer/downPointer";
        float randomG = Random.Range(0, 200);
        Color randomColor = new Color(0 / 255f, randomG / 255f, 255f / 255f, 200f / 255f);

        foreach (Transform obs in KuniIconView.transform) {
            if (obs.GetComponent<SendParam>().daimyoId == daimyoId1 || obs.GetComponent<SendParam>().daimyoId == daimyoId2) {
                GameObject pointer = Instantiate(Resources.Load(path)) as GameObject;
                pointer.transform.SetParent(obs.transform);
                pointer.transform.localScale = new Vector2(0.5f, 0.5f);
                pointer.transform.localPosition = new Vector2(0, 20);

                //Random Color
                pointer.GetComponent<Image>().color = randomColor;

            }
        }
    }


}
