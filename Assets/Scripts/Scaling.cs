using UnityEngine;
using System.Collections;

public class Scaling : MonoBehaviour {

    public float x = 0.001f;
    public float y = 0.001f;


    // Update is called once per frame
    void Update () {
		transform.localScale += new Vector3 (x,y,0);
	}
}
