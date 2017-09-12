using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopScrollSlider : MonoBehaviour {
    public AnimationCurve animCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public Vector3 inPosition;        // スライドイン後の位置
    public Vector3 outPosition;      // スライドアウト後の位置
    public float duration = 1.0f;    // スライド時間（秒）

    public void SlideOut() {
        StartCoroutine(StartSlidePanel(true));
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[1].Play();
    }

    public void SlideIn() {
        //create busyo
        if(transform.FindChild("ScrollView").transform.FindChild("Content").childCount == 0) {
            transform.FindChild("ScrollView").transform.FindChild("Content").GetComponent<PrepBusyoScrollMenu>().PrepareBusyoScrollMenu();            
        }
        StartCoroutine(StartSlidePanel(false));
    }

    private IEnumerator StartSlidePanel(bool isSlideOut) {
        float startTime = Time.time;    // 開始時間
        Vector3 startPos = transform.localPosition;  // 開始位置
        Vector3 moveDistance;            // 移動距離および方向

        if (isSlideOut) {
            moveDistance = (inPosition - startPos);
            while ((Time.time - startTime) < duration) {
                transform.localPosition = startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration);
                yield return 0;        // 1フレーム後、再開
            }
            transform.localPosition = startPos + moveDistance;


        } else {
            moveDistance = (outPosition - startPos);

            while ((Time.time - startTime) < duration) {
                transform.localPosition = startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration);
                yield return 0;        // 1フレーム後、再開
            }
            transform.localPosition = startPos + moveDistance;
        }
    }
}