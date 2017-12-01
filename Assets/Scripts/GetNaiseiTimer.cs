using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GetNaiseiTimer : MonoBehaviour {
    
    public double timer = 0;
	public Text TimerText;


    public void Start () {
        GameObject GameController = GameObject.Find("GameController").gameObject;
        MainStageController MainStageController = GameController.GetComponent<MainStageController>();
        timer = MainStageController.yearTimer;
        TimerText = transform.Find("TimerText").GetComponent<Text>();

    }

	void Update(){
		//Countdown
		timer -= Time.deltaTime;
			
		if (timer > 0.0f) {
			//On Play
			TimeSpan ts = new TimeSpan (0, 0, (int)timer);
			string hms = ts.ToString ();
            TimerText.text = hms;
			
		} 
	}
}