using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamagePopMesh : MonoBehaviour {

    TextMesh text;
    public float divSpeed = 1;
    public bool attackBoardflg = false;
    public float deleteTime = 1;

    void Start() {
        //delete
        Destroy(gameObject, deleteTime);
        text = gameObject.GetComponent<TextMesh>();
    }

    float time = 0;
    void Update() {
        if (attackBoardflg) {
            transform.Translate(Vector3.up * time);
            time += Time.deltaTime;
            time = time / divSpeed;
        }
        else {
            transform.Translate(Vector3.up * Time.deltaTime);
            time += Time.deltaTime;
        }
        //Clear
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1.5F - time);
    }
}
