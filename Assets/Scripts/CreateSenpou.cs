using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSenpou : MonoBehaviour {

    public GameObject doSenpou(GameObject obj, int senpouId, float calculatedHp, bool playerFlg) {

        string path = "Prefabs/Effect/" + senpouId;
        GameObject effect = Instantiate(Resources.Load(path)) as GameObject;
        Vector2 effectOriginalScale = effect.transform.localScale;
        effect.transform.SetParent(obj.transform,false);
        //effect.transform.localPosition = new Vector2(1, 0);
        effect.transform.localScale = effectOriginalScale;
        if(playerFlg) {
            effect.GetComponent<PlayerHP>().life = calculatedHp;
        }
        else {
            Destroy(effect.GetComponent<PlayerHP>());
            effect.AddComponent<EnemyHP>().life = calculatedHp;
            effect.tag = "Enemy";
            effect.layer = LayerMask.NameToLayer("EnemyNoColl");
        }
        effect.GetComponent<Heisyu>().senpouId = senpouId;
        return effect;
    }
}
