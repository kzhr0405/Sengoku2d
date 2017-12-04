using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class PrepBusyoScrollMenu : MonoBehaviour {

    // Use this for initialization
    public List<string> jinkeiBusyo_list;

    public void PrepareBusyoScrollMenu () {
        int senarioId = PlayerPrefs.GetInt("senarioId");
        //Clear Previous Unit
        foreach (Transform chd in GameObject.Find("Content").transform) {
            //Delete
            Destroy(chd.gameObject);

        }

        //Scroll View Change
        string myBusyoString = PlayerPrefs.GetString("myBusyo");
        char[] delimiterChars = { ',' };

        List<string> myBusyo_list = new List<string>();
        if (myBusyoString.Contains(",")) {
            myBusyo_list = new List<string>(myBusyoString.Split(delimiterChars));
        }else {
            myBusyo_list.Add(myBusyoString);
        }

        for (int i = 0; i < jinkeiBusyo_list.Count; i++) {
            myBusyo_list.Remove(jinkeiBusyo_list[i]);
        }

        //Instantiate scroll view
        string scrollPath = "Prefabs/Jinkei/Slot";
        BusyoInfoGet busyoScript = new BusyoInfoGet();
        for (int j = 0; j < myBusyo_list.Count; j++) {
            //Slot
            GameObject prefab = Instantiate(Resources.Load(scrollPath)) as GameObject;
            prefab.transform.SetParent(GameObject.Find("Content").transform);
            prefab.transform.localScale = new Vector3(1, 1, 1);
            prefab.transform.localPosition = new Vector3(0, 0, 0);
            prefab.name = "Slot";
            prefab.GetComponent<LayoutElement>().minHeight = 110;
            prefab.GetComponent<LayoutElement>().minWidth = 110;

            //Busyo
            if(Application.loadedLevelName == "preKassen") {
                string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
                GameObject busyo = Instantiate(Resources.Load(busyoPath)) as GameObject;
                busyo.name = myBusyo_list[j];

                //Add Kamon
                string KamonPath = "Prefabs/Jinkei/Kamon";
                GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                kamon.transform.SetParent(busyo.transform);
                kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                kamon.transform.localPosition = new Vector2(-15, -12);
                int daimyoId = busyoScript.getDaimyoId(int.Parse(busyo.name),senarioId);
                if (daimyoId == 0) {               
                    daimyoId = busyoScript.getDaimyoHst(int.Parse(busyo.name),senarioId);
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
                busyo.transform.localScale = new Vector3(3, 3, 3);
                busyo.name = myBusyo_list[j].ToString();
                busyo.AddComponent<Senryoku>().GetPlayerSenryoku(busyo.name);

                busyo.transform.localPosition = new Vector3(0, 0, 0);

            } else if(Application.loadedLevelName == "preKaisen") {

                preKaisen prekasienScript = new preKaisen();
                int busyoId = int.Parse(myBusyo_list[j]);
                string path = "Prefabs/Player/Unit/Ship";
                GameObject tsyBusyo = Instantiate(Resources.Load(path)) as GameObject;
                tsyBusyo.name = busyoId.ToString();
                prekasienScript.getShipSprite(tsyBusyo, busyoId);

                //Add Kamon
                string KamonPath = "Prefabs/Jinkei/Kamon";
                GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                kamon.transform.SetParent(tsyBusyo.transform);
                kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                kamon.transform.localPosition = new Vector2(-15, -12);
                int daimyoId = busyoScript.getDaimyoId(int.Parse(tsyBusyo.name),senarioId);
                if (daimyoId == 0)
                {
                    daimyoId = busyoScript.getDaimyoHst(int.Parse(tsyBusyo.name),senarioId);
                }
                string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                kamon.GetComponent<Image>().sprite =
                    Resources.Load(imagePath, typeof(Sprite)) as Sprite;


                tsyBusyo.transform.SetParent(prefab.transform);
                tsyBusyo.transform.localScale = new Vector3(3, 3, 3);
                tsyBusyo.name = myBusyo_list[j].ToString();
                tsyBusyo.AddComponent<Senryoku>().GetPlayerSenryoku(tsyBusyo.name);

                tsyBusyo.transform.localPosition = new Vector3(0, 0, 0);

            }
        }
    }
}
