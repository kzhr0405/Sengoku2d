using UnityEngine;
using System.Collections;

public class PopUp : MonoBehaviour {

    public float divSpeed = 1;

    void Start() {
        //delete
        Destroy(gameObject, 1);
    }

    float time = 0;
    void Update() {
        transform.Translate(Vector3.up * time);
        time += Time.deltaTime;
        time = time / divSpeed;
    }
}
