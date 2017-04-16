using UnityEngine;
using System.Collections;

public class SenpouStatusUp : MonoBehaviour {

    public int senpouId = 0;
    public GameObject targetObj;
	public float senpouStatus = 0;
	public float initCoolTime = 0;
	public float initDisTarget = 0;

	public float finalCoolTime = 0;
	public float finalDisTarget = 0;
    public Color upColor = new Color(50f / 255f, 190f / 255f, 35f / 255f, 255f / 255f); //Green Up

    void Start () {

        if(8<=senpouId && senpouId <= 13) {
            //YM or TP Up
		    float tmp = 100 - senpouStatus;
		    finalCoolTime = (initCoolTime * tmp) / 100;
		    finalDisTarget = initDisTarget + (initDisTarget * senpouStatus) / 100;
            
		    targetObj.GetComponent<AttackLong> ().coolTime = finalCoolTime;
		    if (targetObj.GetComponent<UnitMover> ()) {
			    targetObj.GetComponent<UnitMover> ().DisTarget = finalDisTarget;
		    } else {
			    targetObj.GetComponent<HomingLong> ().DisTarget = finalDisTarget;
		    }
        }else if(senpouId == 17) {
            //HP Up
            if(targetObj.tag == "Player") {
                float finalHP = 0;
                float initHP = targetObj.GetComponent<PlayerHP>().initLife;
                float nowHP = targetObj.GetComponent<PlayerHP>().life;
                float recoverTmpHP = (initHP * senpouStatus) / 100;
                float recoveredTotalHP = recoverTmpHP + nowHP;
                if(recoveredTotalHP > initHP) {
                    finalHP = initHP;
                }else {
                    finalHP = recoveredTotalHP;
                }
                targetObj.GetComponent<PlayerHP>().life = finalHP;

                GameObject dtl = targetObj.transform.FindChild("BusyoDtlPlayer").gameObject;
                GameObject effect = dtl.transform.FindChild("hp_up").gameObject;
                effect.GetComponent<FadeoutOff>().currentRemainTime = 5;
                effect.GetComponent<SpriteRenderer>().enabled = true;
                effect.GetComponent<Animator>().enabled = true;
                effect.GetComponent<FadeoutOff>().enabled = true;

                GameObject value = dtl.transform.FindChild("MinHpBar").transform.FindChild("Value").gameObject;
                value.GetComponent<MeshRenderer>().enabled = true;
                value.GetComponent<TextMeshFadeoutOff>().enabled = true;
                value.GetComponent<TextMesh>().text = recoverTmpHP.ToString() + "⇡";
                value.GetComponent<TextMesh>().color = upColor;

            }else {
                float finalHP = 0;
                float initHP = targetObj.GetComponent<EnemyHP>().initLife;
                float nowHP = targetObj.GetComponent<EnemyHP>().life;
                float recoverTmpHP = (initHP * senpouStatus) / 100;
                float recoveredTotalHP = recoverTmpHP + nowHP;
                if (recoveredTotalHP > initHP) {
                    finalHP = initHP;
                }
                else {
                    finalHP = recoveredTotalHP;
                }
                targetObj.GetComponent<EnemyHP>().life = finalHP;

                GameObject dtl = targetObj.transform.FindChild("BusyoDtlEnemy").gameObject;
                GameObject effect = dtl.transform.FindChild("hp_up").gameObject;
                effect.GetComponent<FadeoutOff>().currentRemainTime = 5;
                effect.GetComponent<SpriteRenderer>().enabled = true;
                effect.GetComponent<Animator>().enabled = true;
                effect.GetComponent<FadeoutOff>().enabled = true;

                GameObject value = dtl.transform.FindChild("MinHpBar").transform.FindChild("Value").gameObject;
                value.GetComponent<MeshRenderer>().enabled = true;
                value.GetComponent<TextMeshFadeoutOff>().enabled = true;
                value.GetComponent<TextMesh>().text = recoverTmpHP.ToString() + "⇡";
                value.GetComponent<TextMesh>().color = upColor;
            }

        }else if(senpouId == 18) {
            //Speed Up
            if (targetObj.tag == "Player") {
                float upSpd = 0;
                if (targetObj.GetComponent<UnitMover>()) {
                    float nowSpd = targetObj.GetComponent<UnitMover>().speed;
                    upSpd = (nowSpd * senpouStatus) / 100;
                    float finalSpd = nowSpd + upSpd;
                    targetObj.GetComponent<UnitMover>().speed = finalSpd;
                }else {
                    if (targetObj.GetComponent<Homing>()) {
                        float nowSpd = targetObj.GetComponent<Homing>().speed;
                        upSpd = (nowSpd * senpouStatus) / 100;
                        float finalSpd = nowSpd + upSpd;
                        targetObj.GetComponent<Homing>().speed = finalSpd;
                    }else if (targetObj.GetComponent<HomingLong>()) {
                        float nowSpd = targetObj.GetComponent<HomingLong>().speed;
                        upSpd = (nowSpd * senpouStatus) / 100;
                        float finalSpd = nowSpd + upSpd;
                        targetObj.GetComponent<HomingLong>().speed = finalSpd;
                    }
                }

                GameObject dtl = targetObj.transform.FindChild("BusyoDtlPlayer").gameObject;
                GameObject effect = dtl.transform.FindChild("spd_up").gameObject;
                effect.GetComponent<FadeoutOff>().currentRemainTime = 5;
                effect.GetComponent<SpriteRenderer>().enabled = true;
                effect.GetComponent<Animator>().enabled = true;
                effect.GetComponent<FadeoutOff>().enabled = true;

                GameObject value = dtl.transform.FindChild("MinHpBar").transform.FindChild("Value").gameObject;
                value.GetComponent<MeshRenderer>().enabled = true;
                value.GetComponent<TextMeshFadeoutOff>().enabled = true;
                value.GetComponent<TextMesh>().text = upSpd.ToString() + "⇡";
                value.GetComponent<TextMesh>().color = upColor;

            }else {
                float upSpd = 0;
                if (targetObj.GetComponent<Homing>()) {
                    float nowSpd = targetObj.GetComponent<Homing>().speed;
                    upSpd = (nowSpd * senpouStatus) / 100;
                    float finalSpd = nowSpd + upSpd;
                    targetObj.GetComponent<Homing>().speed = finalSpd;
                }else {
                    float nowSpd = targetObj.GetComponent<HomingLong>().speed;
                    upSpd = (nowSpd * senpouStatus) / 100;
                    float finalSpd = nowSpd + upSpd;
                    targetObj.GetComponent<HomingLong>().speed = finalSpd;
                }
                GameObject dtl = targetObj.transform.FindChild("BusyoDtlEnemy").gameObject;
                GameObject effect = dtl.transform.FindChild("spd_up").gameObject;
                effect.GetComponent<FadeoutOff>().currentRemainTime = 5;
                effect.GetComponent<SpriteRenderer>().enabled = true;
                effect.GetComponent<Animator>().enabled = true;
                effect.GetComponent<FadeoutOff>().enabled = true;

                GameObject value = dtl.transform.FindChild("MinHpBar").transform.FindChild("Value").gameObject;
                value.GetComponent<MeshRenderer>().enabled = true;
                value.GetComponent<TextMeshFadeoutOff>().enabled = true;
                value.GetComponent<TextMesh>().text = upSpd.ToString() + "⇡";
                value.GetComponent<TextMesh>().color = upColor;
            }

        }


    }

}
