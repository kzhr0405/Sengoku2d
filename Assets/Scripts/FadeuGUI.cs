using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeuGUI : MonoBehaviour {
		private Image image;
		private float time;
		public float fadetime = 0.3f;
		private Text text;
        public bool iapRunFlg = false;

		void Start () {
			time = fadetime;
			image = GetComponent<Image>();
			text = image.transform.FindChild ("MessageText").GetComponent<Text>();
		}
		
		void Update () {
			time -= Time.deltaTime;//時間更新
			float a = time / fadetime;
			var color = image.color;
			var textColor = text.color;
			color.a = a;
			textColor.a = a;
			image.color = color;
			text.color = textColor;
			if(time <= 0f ) {
				// 残り時間が無くなったら自分自身を消滅
                if(iapRunFlg) {
                    GameObject.Find("GameController").GetComponent<MainStageController>().iapRunFlg = false;
                }
				GameObject.Destroy(gameObject);
				return;
			}

		}
	}