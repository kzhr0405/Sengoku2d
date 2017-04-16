using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopDown : MonoBehaviour {
    public float divSpeed = 1;

    // Use this for initialization
    void Start () {
        //delete
        Destroy(gameObject, 1);
    }

    float time = 0;
    void Update() {

        transform.Translate(Vector3.down * time);
        time -= Time.deltaTime;
        time = time / divSpeed;

    }
}
