using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class BuildShiro : MonoBehaviour {

    public GameObject item;
    public GameObject touchBack;

    public void OnClick() {

        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();

        if (name == "YesButton") {
            audioSources[3].Play();

            //Change Sprite & Register Data
            int shiroId = item.GetComponent<ItemInfo>().itemId;
            string itemName = item.GetComponent<ItemInfo>().itemName;

            string imagePath = "Prefabs/Naisei/Shiro/Sprite/" + shiroId;
            string effectPath = "Prefabs/EffectAnime/point_up";
            GameObject naiseiView = GameObject.Find("NaiseiView").gameObject;
            foreach(Transform chld in naiseiView.transform) {
                if(chld.GetComponent<AreaButton>().type == "shiro") {
                    chld.GetComponent<Image>().sprite =
                                Resources.Load(imagePath, typeof(Sprite)) as Sprite;

                    GameObject effect = Instantiate(Resources.Load(effectPath)) as GameObject;
                    effect.transform.SetParent(chld.transform);
                    effect.transform.localScale = new Vector2(100, 100);
                    effect.transform.localPosition = new Vector3(0, 0, 0);
                }
            }

            NaiseiController NaiseiControllerScript = GameObject.Find("NaiseiController").GetComponent<NaiseiController>();
            string shiroTmp = "shiro" + NaiseiControllerScript.activeKuniId;
            PlayerPrefs.SetInt(shiroTmp, shiroId);
            PlayerPrefs.Flush();

            //Buf
            NaiseiControllerScript.tabibitoSecMst = 1.5f;

            //Reduce Item
            Shiro shiro = new Shiro();
            shiro.deleteShiro(shiroId,1);

            //Close
            touchBack.GetComponent<CloseBoard>().onClick();
        }
        else if (name == "NoButton") {
            //Close
            audioSources[1].Play();
            Destroy(touchBack);
        }
    }
}
