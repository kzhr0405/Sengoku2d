using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeOutBusyo : MonoBehaviour {
	private Image image;
	private float time;
	public float fadetime = 3.0f;
	private Text text;
	public Image busyoImage;
	private Image serihuImage;
	private Text serihuText;

	void Start () {
		time = fadetime;
		image = GetComponent<Image>();
		text = image.transform.Find ("MessageBusyo").GetComponent<Text>();
		//busyoImage = image.transform.FindChild ("Busyo").GetComponent<Image> ();
		serihuImage = busyoImage.transform.Find("Serihu").GetComponent<Image>();
		serihuText = serihuImage.transform.Find ("BusyoSerihu").GetComponent<Text>();
	}
	
	void Update () {
		time -= Time.deltaTime;//時間更新
		float a = time / fadetime;
		var color = image.color;
		var textColor = text.color;
		var imageColor = busyoImage.color;
		var serihuImageColor = serihuImage.color;
		var serihuTextColor = serihuText.color;

		color.a = a;
		textColor.a = a;
		imageColor.a = a;
		serihuImageColor.a = a;
		serihuTextColor.a = a;

		image.color = color;
		text.color = textColor;
		busyoImage.color = imageColor;
		serihuImage.color = serihuImageColor;
		serihuText.color = serihuTextColor;

		if(time <= 0f ) {
			// 残り時間が無くなったら自分自身を消滅
			GameObject.Destroy(gameObject);
			return;
		}
		
	}
}