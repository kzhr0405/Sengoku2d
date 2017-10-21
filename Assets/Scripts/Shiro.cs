using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class Shiro : MonoBehaviour {

    Entity_shiro_mst shiroMst = Resources.Load("Data/shiro_mst") as Entity_shiro_mst;

    public string getName(int busyoId) {
        string name = "";

        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            name = shiroMst.param[busyoId - 1].NameEng;
        }else {
            name = shiroMst.param[busyoId - 1].Name;
        }
        return name;
    }

    public int getRandomId() {
        return UnityEngine.Random.Range(1, shiroMst.param.Count + 1);
    }

    public void registerShiro(int shiroId) {

        string shiroString = PlayerPrefs.GetString("shiro","0,0,0,0");
        List<string> shiroQtyList = new List<string>();
        char[] delimiterChars = { ',' };
        shiroQtyList = new List<string>(shiroString.Split(delimiterChars));

        string newShiroQty = "";
        if (shiroId == 1) {
            int newUnitQty = int.Parse(shiroQtyList[0]);
            newUnitQty = newUnitQty + 1;
            newShiroQty = newUnitQty + "," + shiroQtyList[1] + "," + shiroQtyList[2] + "," + shiroQtyList[3];
        }
        else if (shiroId == 2) {
            int newUnitQty = int.Parse(shiroQtyList[1]);
            newUnitQty = newUnitQty + 1;
            newShiroQty = shiroQtyList[0] + "," + newUnitQty + "," + shiroQtyList[2] + "," + shiroQtyList[3];
        }
        else if (shiroId == 3) {
            int newUnitQty = int.Parse(shiroQtyList[2]);
            newUnitQty = newUnitQty + 1;
            newShiroQty = shiroQtyList[0] + "," + shiroQtyList[1] + "," + newUnitQty + "," + shiroQtyList[3];
        }
        else if (shiroId == 4) {
            int newUnitQty = int.Parse(shiroQtyList[2]);
            newUnitQty = newUnitQty + 1;
            newShiroQty = shiroQtyList[0] + "," + shiroQtyList[1] + "," + shiroQtyList[2] + "," + newUnitQty;
        }
        PlayerPrefs.SetString("shiro", newShiroQty);
        PlayerPrefs.Flush();


    }

    public void deleteShiro(int shiroId, int reduceQty) {

        string shiroString = PlayerPrefs.GetString("shiro", "0,0,0,0");
        List<string> shiroQtyList = new List<string>();
        char[] delimiterChars = { ',' };
        shiroQtyList = new List<string>(shiroString.Split(delimiterChars));

        string newShiroQty = "";
        if (shiroId == 1) {
            int newUnitQty = int.Parse(shiroQtyList[0]);
            newUnitQty = newUnitQty - reduceQty;
            newShiroQty = newUnitQty + "," + shiroQtyList[1] + "," + shiroQtyList[2] + "," + shiroQtyList[3];
        }
        else if (shiroId == 2) {
            int newUnitQty = int.Parse(shiroQtyList[1]);
            newUnitQty = newUnitQty - reduceQty;
            newShiroQty = shiroQtyList[0] + "," + newUnitQty + "," + shiroQtyList[2] + "," + shiroQtyList[3];
        }
        else if (shiroId == 3) {
            int newUnitQty = int.Parse(shiroQtyList[2]);
            newUnitQty = newUnitQty - reduceQty;
            newShiroQty = shiroQtyList[0] + "," + shiroQtyList[1] + "," + newUnitQty + "," + shiroQtyList[3];
        }
        else if (shiroId == 4) {
            int newUnitQty = int.Parse(shiroQtyList[3]);
            newUnitQty = newUnitQty - reduceQty;
            newShiroQty = shiroQtyList[0] + "," + shiroQtyList[1] + "," + shiroQtyList[2] + "," + newUnitQty;
        }
        PlayerPrefs.SetString("shiro", newShiroQty);
        PlayerPrefs.Flush();


    }

}
