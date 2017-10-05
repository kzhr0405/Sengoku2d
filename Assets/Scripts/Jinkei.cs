using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class Jinkei : MonoBehaviour {
	public int selectedJinkei = 0;
    // 1. Gyorin
    // 2. Kakuyoku
    // 3. Engetsu
    // 4. Ganko

    public void jinkeiHpUpda(bool plusFlg, int var) {
        int jinkeiHeiryoku = PlayerPrefs.GetInt("jinkeiHeiryoku");
        int pvpHeiryoku = PlayerPrefs.GetInt("pvpHeiryoku");

        if (plusFlg) {
            jinkeiHeiryoku = jinkeiHeiryoku + var;
            pvpHeiryoku = pvpHeiryoku + var;
        }
        else {
            jinkeiHeiryoku = jinkeiHeiryoku - var;
            pvpHeiryoku = pvpHeiryoku - var;
        }

        PlayerPrefs.SetInt("jinkeiHeiryoku", jinkeiHeiryoku);
        PlayerPrefs.SetInt("pvpHeiryoku", pvpHeiryoku);
        PlayerPrefs.Flush();
    }

    public int soudaisyoBusyoIdCheck(int soudaisyoBusyoId, int jinkeiId) {        
        int newSoudaisyoBusyoId = soudaisyoBusyoId;
        if(newSoudaisyoBusyoId == 0) {
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
                if (obs.transform.childCount > 0) {
                    //Get Name 
                    newSoudaisyoBusyoId = int.Parse(obs.transform.GetChild(0).name);
                    break;
                }
            }
        }
        return newSoudaisyoBusyoId;
    }
}
