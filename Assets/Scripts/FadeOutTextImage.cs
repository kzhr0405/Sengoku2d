using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeOutTextImage : MonoBehaviour {

	private Image image;
	private float time;
	public float fadetime = 3.0f;
	private Image mainImage;
	private Text mainText;

	void Start () {
		time = fadetime;
		image = GetComponent<Image>();
		mainText = image.transform.Find ("Message").GetComponent<Text>();
		mainImage = image.transform.Find ("Image").GetComponent<Image> ();
	}

	void Update () {
		time -= Time.deltaTime;//時間更新
		float a = time / fadetime;
		var color = image.color;
		var textColor = mainText.color;
		var imageColor = mainImage.color;

		color.a = a;
		textColor.a = a;
		imageColor.a = a;

		image.color = color;
		mainText.color = textColor;
		mainImage.color = imageColor;

		if(time <= 0f ) {
			// 残り時間が無くなったら自分自身を消滅
			GameObject.Destroy(gameObject);
			return;
		}

	}
}
