using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BusyoDamaSlider : MonoBehaviour {

    private Slider bdSlider;
    private Text busyoDamaText;
    private Text spaceText;
    public GameObject doBtn;

    // Use this for initialization
    void Start() {
        bdSlider = this.GetComponent<Slider>();
        busyoDamaText = gameObject.transform.FindChild("BusyoDama").GetComponent<Text>();
        spaceText = gameObject.transform.FindChild("Space").GetComponent<Text>();
        float paiedBusyoDama = 0;

        bdSlider.onValueChanged.AddListener((value) => {
            paiedBusyoDama = value * 100;
            busyoDamaText.text = paiedBusyoDama.ToString();

            spaceText.text = " + " + value.ToString();

            doBtn.GetComponent<DoSpaceBuy>().paiedBusyoDama = (int)paiedBusyoDama;
            doBtn.GetComponent<DoSpaceBuy>().buySpace = (int)value;

        });
    }
}
