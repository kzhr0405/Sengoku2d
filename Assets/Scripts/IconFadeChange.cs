using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IconFadeChange : MonoBehaviour {

	public int toDaimyoId = 0;
	public string iconPath = "";

	public float fadeoutTime = 1.5f;
	private float fadeoutRemainTime;
	public float fadeinTime = 1.5f;
	private float fadeinRemainTime;
	private Image image;
	bool fadeinFlg = false;

	void Start () {
		fadeoutRemainTime = fadeoutTime;
		image = GetComponent<Image> ();
		image.color = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
		iconPath = "Prefabs/Kamon/MyDaimyoKamon/" + toDaimyoId;;
	}
	
	void Update () {
		if (!fadeinFlg) {
			fadeoutRemainTime -= Time.deltaTime;

			float alpha = fadeoutRemainTime / fadeoutTime;
			var colorValue = image.color;
			colorValue.a = alpha;
			image.color = colorValue;

			if (fadeoutRemainTime <= 0f) {
				fadeinFlg = true;
				image.sprite = 
					Resources.Load (iconPath, typeof(Sprite)) as Sprite;
			}
		} else {
			fadeinRemainTime += Time.deltaTime;

			float alpha = fadeinRemainTime / fadeinTime;
			var colorValue = image.color;
			colorValue.a = alpha;
			image.color = colorValue;

			if (fadeinRemainTime >= fadeinTime) {
				Destroy (GetComponent<IconFadeChange>());
			}
		}



	}
}
