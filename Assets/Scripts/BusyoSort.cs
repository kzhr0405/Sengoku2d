using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class BusyoSort : MonoBehaviour {

    public List<string> myBusyoRankSortList;
    public List<string> myBusyoDaimyoSortList;
    public List<string> myBusyoLvSortList;
    public List<string> jinkeiTrueBusyoList;

    private void Start() {
        //label
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            Dropdown Dropdown = GetComponent<Dropdown>();
            Dropdown.options[0].text = "Rank";
            Dropdown.options[1].text = "Clan";
            Dropdown.options[2].text = "Lv";

            Text text = transform.FindChild("Label").GetComponent<Text>();
            text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;                
        }
    }



    public void OnValueChanged(int result) {
        if(result==0) {
            //rank
            updateScroll(myBusyoRankSortList);
        }else if(result==1){
            //daimyoId
            updateScroll(myBusyoDaimyoSortList);
        }else if (result == 2) {
            //Lv
            updateScroll(myBusyoLvSortList);
        }
    }
    

    public void updateScroll(List<string> myBusyoList) {

        GameObject content = GameObject.Find("Content");
        BusyoInfoGet busyoScript = new BusyoInfoGet();
        
        //Instantiate scroll view
        if (SceneManager.GetActiveScene().name == "busyo") {
            foreach (Transform chld in content.transform) {
                Destroy(chld.gameObject);
            }
            string scrollPath = "Prefabs/Busyo/Slot";
            for (int j = 0; j < myBusyoList.Count; j++) {
                //Slot
                GameObject prefab = Instantiate(Resources.Load(scrollPath)) as GameObject;
                prefab.transform.SetParent(content.transform);
                prefab.transform.localScale = new Vector3(1, 1, 1);
                prefab.transform.localPosition = new Vector3(330, -75, 0);

                //Busyo
                string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
                GameObject busyo = Instantiate(Resources.Load(busyoPath)) as GameObject;
                busyo.name = myBusyoList[j].ToString();
                busyo.transform.SetParent(prefab.transform);
                busyo.transform.localScale = new Vector3(4, 4, 4);
                busyo.transform.localPosition = new Vector3(100, -75, 0);
                prefab.name = "Slot" + busyo.name;

                busyo.GetComponent<DragHandler>().enabled = false;

                //kamon
                string KamonPath = "Prefabs/Jinkei/Kamon";
                GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                kamon.transform.SetParent(busyo.transform);
                kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                kamon.transform.localPosition = new Vector2(-15, -12);
                int daimyoId = busyoScript.getDaimyoId(int.Parse(busyo.name));
                if (daimyoId == 0) {
                    daimyoId = busyoScript.getDaimyoHst(int.Parse(busyo.name));
                }
                string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                kamon.GetComponent<Image>().sprite =
                    Resources.Load(imagePath, typeof(Sprite)) as Sprite;

                //Heisyu
                string heisyu = busyoScript.getHeisyu(int.Parse(busyo.name));
                string heisyuPath = "Prefabs/Jinkei/" + heisyu;
                GameObject heisyuObj = Instantiate(Resources.Load(heisyuPath)) as GameObject;
                heisyuObj.transform.SetParent(busyo.transform, false);
                heisyuObj.transform.localPosition = new Vector2(10, -10);
                heisyuObj.transform.SetAsFirstSibling();

                if (jinkeiTrueBusyoList.Contains(busyo.name)) {
                    prefab.GetComponent<BusyoView>().jinkeiFlg = true;
                }                
            }
        }else if (SceneManager.GetActiveScene().name == "hyojyo") {
            int childCount = content.transform.childCount;
            foreach (string busyoId in myBusyoList) {
                foreach(Transform slot in content.transform) {
                    if(busyoId == slot.transform.GetChild(0).name) {
                        slot.transform.SetSiblingIndex(childCount - 1);
                    }                   
                }
            }

            /*
            string scrollPath = "Prefabs/Jinkei/Slot";
            for (int j = 0; j < myBusyoList.Count; j++) {

                if (myBusyoList[j] != "0") {
                    //Slot
                    GameObject prefab = Instantiate(Resources.Load(scrollPath)) as GameObject;
                    prefab.transform.SetParent(GameObject.Find("Content").transform);
                    prefab.transform.localScale = new Vector3(1, 1, 1);
                    prefab.transform.localPosition = new Vector3(0, 0, 0);
                    prefab.name = "Slot";

                    //Busyo
                    string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
                    GameObject busyo = Instantiate(Resources.Load(busyoPath)) as GameObject;
                    busyo.name = myBusyoList[j];

                    //Add Kamon
                    string KamonPath = "Prefabs/Jinkei/Kamon";
                    GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                    kamon.transform.SetParent(busyo.transform);
                    kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                    kamon.transform.localPosition = new Vector2(-15, -12);
                    int daimyoId = busyoScript.getDaimyoId(int.Parse(busyo.name));
                    if (daimyoId == 0) {
                        daimyoId = busyoScript.getDaimyoHst(int.Parse(busyo.name));
                    }
                    string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                    kamon.GetComponent<Image>().sprite =
                        Resources.Load(imagePath, typeof(Sprite)) as Sprite;

                    //Add Heisyu
                    string heisyu = busyoScript.getHeisyu(int.Parse(busyo.name));
                    string heisyuPath = "Prefabs/Jinkei/" + heisyu;
                    GameObject heisyuObj = Instantiate(Resources.Load(heisyuPath)) as GameObject;
                    heisyuObj.transform.SetParent(busyo.transform, false);
                    heisyuObj.transform.localPosition = new Vector2(10, -10);
                    heisyuObj.transform.SetAsFirstSibling();


                    busyo.transform.SetParent(prefab.transform);
                    busyo.transform.localScale = new Vector3(4, 4, 4);
                    busyo.name = myBusyoList[j].ToString();
                    busyo.AddComponent<Senryoku>().GetPlayerSenryoku(busyo.name);

                    busyo.transform.localPosition = new Vector3(0, 0, 0);
                }
            }*/
        }
    }


}
