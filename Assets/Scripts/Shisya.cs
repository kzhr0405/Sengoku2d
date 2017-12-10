using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class Shisya : MonoBehaviour {

	Entity_shisya_mst Mst = Resources.Load ("Data/shisya_mst") as Entity_shisya_mst;

	public string getName(int id){
		string value = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            value = Mst.param [id - 1].nameEng;
        }else if(langId==3){
            value = Mst.param[id - 1].nameSChn;
        }else {
            value = Mst.param[id - 1].name;
        }
		return value;
	}

	public string getSlot(int id){
		string value = "";
		value = Mst.param [id - 1].Slot;
		return value;
	}

	public string getSerihu1(int id){
		string value = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            value = Mst.param [id - 1].Serihu1Eng;
        }
        else if (langId == 3) {
            value = Mst.param[id - 1].Serihu1SChn;
        }
        else {
            value = Mst.param[id - 1].Serihu1;
        }
		return value;
	}

	public string getSerihu2(int id){
		string value = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            value = Mst.param [id - 1].Serihu2Eng;
        }
        else if (langId == 3) {
            value = Mst.param[id - 1].Serihu2SChn;
        }
        else {
            value = Mst.param[id - 1].Serihu2;
        }
		return value;
	}

	public string getSerihu3(int id){
		string value = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            value = Mst.param [id - 1].Serihu3Eng;
        }
        else if (langId == 3) {
            value = Mst.param[id - 1].Serihu3SChn;
        }
        else {
            value = Mst.param[id - 1].Serihu3;
        }
		return value;
	}

	public string getYesRequried1(int id){
		string value = "";
		value = Mst.param [id - 1].YesRequried1;
		return value;
	}

	public string getYesRequried2(int id){
		string value = "";
		value = Mst.param [id - 1].YesRequried2;
		return value;
	}

	public bool getSelectFlg(int id){
		bool selectFlg = false;
		selectFlg = Mst.param [id - 1].selectFlg;
		return selectFlg;
	}

	public string getOKSerihu(int id){
		string value = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            value = Mst.param [id - 1].OKSerihuEng;
        }
        else if (langId == 3) {
            value = Mst.param[id - 1].OKSerihuSChn;
        }
        else {
            value = Mst.param[id - 1].OKSerihu;
        }
		return value;
	}

	public string getNGSerihu(int id){
		string value = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            value = Mst.param [id - 1].NGSerihuEng;
        }
        else if (langId == 3) {
            value = Mst.param[id - 1].NGSerihuSChn;
        }
        else {
            value = Mst.param[id - 1].NGSerihu;
        }
		return value;
	}

}
