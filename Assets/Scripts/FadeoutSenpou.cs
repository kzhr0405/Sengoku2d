using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeoutSenpou : MonoBehaviour {

	private Image image;
	private float time;
	public float fadetime = 0.3f;
	private Text senpouText;
	private Text serihuText;
	private Image busyoImage;

	void Start () {
		time = fadetime;
		image = GetComponent<Image>();
		senpouText = image.transform.Find ("SenpouName").GetComponent<Text>();
		serihuText = image.transform.Find ("SerihuText").GetComponent<Text>();
		busyoImage = image.transform.Find ("Mask").transform.Find ("BusyoImage").GetComponent<Image> ();
	}

	void Update () {
		time -= Time.deltaTime;//時間更新
		float a = time / fadetime;
		var color = image.color;
		var senpouTextColor = senpouText.color;
		var serihuTextColor = serihuText.color;
		var busyoImageColor = busyoImage.color;

		color.a = a;
		senpouTextColor.a = a;
		serihuTextColor.a = a;
		busyoImageColor.a = a;

		image.color = color;
		senpouText.color = senpouTextColor;
		serihuText.color = serihuTextColor;
		busyoImage.color = busyoImageColor;


		if(time <= 0f ) {
			// 残り時間が無くなったら自分自身を消滅
			GameObject.Destroy(gameObject);
			return;
		}

	}
}
